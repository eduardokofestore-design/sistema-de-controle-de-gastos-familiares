import { useEffect, useState } from 'react';
import { ResumoPanel } from '../components/ResumoPanel';
import { ItemList } from '../components/ItemList';
import { pessoaService, relatorioService, transacaoService, type PessoaResponseDto, type RelatorioTotalDto, type TransacaoResponseDto } from '../services/api';

export function DashboardPage() {
  const [pessoas, setPessoas] = useState<PessoaResponseDto[]>([]);
  const [transacoes, setTransacoes] = useState<TransacaoResponseDto[]>([]);
  const [relatorio, setRelatorio] = useState<RelatorioTotalDto | null>(null);
  const [loading, setLoading] = useState(false);
  const [feedback, setFeedback] = useState<string | null>(null);

  useEffect(() => {
    const carregarDados = async () => {
      setLoading(true);
      try {
        const [pessoasResponse, transacoesResponse, relatorioResponse] = await Promise.all([
          pessoaService.listar(),
          transacaoService.listar(),
          relatorioService.obterTotais(),
        ]);

        setPessoas(pessoasResponse);
        setTransacoes(transacoesResponse);
        setRelatorio(relatorioResponse);
        setFeedback(null);
      } catch (error) {
        setFeedback(error instanceof Error ? error.message : 'Falha ao carregar dados.');
      } finally {
        setLoading(false);
      }
    };

    carregarDados();
  }, []);

  return (
    <div className="page-stack">
      <header className="page-header">
        <div>
          <p className="eyebrow">Painel</p>
          <h2>Visão geral</h2>
        </div>
      </header>

      {feedback ? <div className="feedback">{feedback}</div> : null}

      <ResumoPanel loading={loading} relatorio={relatorio} />

      <ItemList
        title="Pessoas cadastradas"
        items={pessoas}
        getKey={(pessoa) => pessoa.id}
        emptyMessage="Nenhuma pessoa cadastrada ainda."
        renderItem={(pessoa) => <span>{pessoa.nome} • {pessoa.idade} anos</span>}
      />

      <ItemList
        title="Últimas transações"
        items={transacoes}
        getKey={(transacao) => transacao.id}
        emptyMessage="Nenhuma transação registrada ainda."
        renderItem={(transacao) => (
          <>
            <span>{transacao.descricao} • {transacao.pessoaNome} • {formatarMoeda(transacao.valor)}</span>
            <span>{transacao.tipo === 1 ? 'Receita' : 'Despesa'}</span>
          </>
        )}
      />
    </div>
  );
}

const formatarMoeda = (valor: number) => new Intl.NumberFormat('pt-BR', {
  style: 'currency',
  currency: 'BRL',
}).format(valor);
