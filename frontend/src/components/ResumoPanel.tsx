import type { RelatorioTotalDto } from '../services/api';

type ResumoPanelProps = {
  loading: boolean;
  relatorio: RelatorioTotalDto | null;
};

export function ResumoPanel({ loading, relatorio }: ResumoPanelProps) {
  return (
    <section className="panel">
      <h2>Resumo</h2>
      {loading ? <p>Carregando...</p> : null}
      {relatorio ? (
        <div className="summary-grid">
          <div>
            <strong>Receitas</strong>
            <p>R$ {relatorio.totalReceitasGeral.toFixed(2)}</p>
          </div>
          <div>
            <strong>Despesas</strong>
            <p>R$ {relatorio.totalDespesasGeral.toFixed(2)}</p>
          </div>
          <div>
            <strong>Saldo</strong>
            <p>R$ {relatorio.saldoGeral.toFixed(2)}</p>
          </div>
        </div>
      ) : null}
    </section>
  );
}
