import { useEffect, useState } from 'react';
import { ResumoPanel } from '../components/ResumoPanel';
import { relatorioService, type RelatorioTotalDto } from '../services/api';

export function RelatoriosPage() {
  const [relatorio, setRelatorio] = useState<RelatorioTotalDto | null>(null);
  const [loading, setLoading] = useState(false);
  const [feedback, setFeedback] = useState<string | null>(null);

  useEffect(() => {
    const carregarRelatorio = async () => {
      setLoading(true);
      try {
        const response = await relatorioService.obterTotais();
        setRelatorio(response);
        setFeedback(null);
      } catch (error) {
        setFeedback(error instanceof Error ? error.message : 'Falha ao carregar relatório.');
      } finally {
        setLoading(false);
      }
    };

    carregarRelatorio();
  }, []);

  return (
    <div className="page-stack">
      <header className="page-header">
        <div>
          <p className="eyebrow">Relatórios</p>
          <h2>Resumo financeiro</h2>
        </div>
      </header>

      {feedback ? <div className="feedback">{feedback}</div> : null}

      <ResumoPanel loading={loading} relatorio={relatorio} />

      {relatorio ? (
        <section className="panel">
          <h2>Totais por pessoa</h2>
          <div className="table-wrapper">
            <table>
              <thead>
                <tr>
                  <th>Pessoa</th>
                  <th>Receitas</th>
                  <th>Despesas</th>
                  <th>Saldo</th>
                </tr>
              </thead>
              <tbody>
                {relatorio.pessoas.map((pessoa) => (
                  <tr key={pessoa.pessoaId}>
                    <td>{pessoa.nome}</td>
                    <td>{formatarMoeda(pessoa.totalReceitas)}</td>
                    <td>{formatarMoeda(pessoa.totalDespesas)}</td>
                    <td>{formatarMoeda(pessoa.saldo)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>
      ) : null}
    </div>
  );
}

const formatarMoeda = (valor: number) => new Intl.NumberFormat('pt-BR', {
  style: 'currency',
  currency: 'BRL',
}).format(valor);
