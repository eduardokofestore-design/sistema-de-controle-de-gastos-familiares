import { useEffect, useState } from 'react';
import { TransacaoForm } from '../components/TransacaoForm';
import { ItemList } from '../components/ItemList';
import { pessoaService, transacaoService, type PessoaResponseDto, type TransacaoCreateDto, type TransacaoResponseDto } from '../services/api';

export function TransacoesPage() {
  const [pessoas, setPessoas] = useState<PessoaResponseDto[]>([]);
  const [transacoes, setTransacoes] = useState<TransacaoResponseDto[]>([]);
  const [descricao, setDescricao] = useState('');
  const [valor, setValor] = useState('');
  const [tipo, setTipo] = useState('0');
  const [pessoaId, setPessoaId] = useState('');
  const [feedback, setFeedback] = useState<string | null>(null);

  const carregarDados = async () => {
    try {
      const [pessoasResponse, transacoesResponse] = await Promise.all([
        pessoaService.listar(),
        transacaoService.listar(),
      ]);

      setPessoas(pessoasResponse);
      setTransacoes(transacoesResponse);
    } catch (error) {
      setFeedback(error instanceof Error ? error.message : 'Falha ao carregar dados.');
    }
  };

  useEffect(() => {
    void Promise.all([pessoaService.listar(), transacaoService.listar()])
      .then(([pessoasResponse, transacoesResponse]) => {
        setPessoas(pessoasResponse);
        setTransacoes(transacoesResponse);
      })
      .catch((error: unknown) => {
        setFeedback(error instanceof Error ? error.message : 'Falha ao carregar dados.');
      });
  }, []);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    if (!descricao.trim() || !pessoaId) {
      setFeedback('Descrição e pessoa são obrigatórios.');
      return;
    }

    const payload: TransacaoCreateDto = {
      descricao: descricao.trim(),
      valor: Number(valor || 0),
      tipo: Number(tipo),
      pessoaId: Number(pessoaId),
    };

    try {
      await transacaoService.criar(payload);
      setDescricao('');
      setValor('');
      setTipo('0');
      setPessoaId('');
      await carregarDados();
      setFeedback('Transação registrada com sucesso.');
    } catch (error) {
      setFeedback(error instanceof Error ? error.message : 'Erro ao registrar transação.');
    }
  };

  return (
    <div className="page-stack">
      <header className="page-header">
        <div>
          <p className="eyebrow">Transações</p>
          <h2>Registre receitas e despesas</h2>
        </div>
      </header>

      {feedback ? <div className="feedback">{feedback}</div> : null}

      <TransacaoForm
        descricao={descricao}
        valor={valor}
        tipo={tipo}
        pessoaId={pessoaId}
        pessoas={pessoas}
        onDescricaoChange={setDescricao}
        onValorChange={setValor}
        onTipoChange={setTipo}
        onPessoaChange={(id) => {
          setPessoaId(id);
          const pessoa = pessoas.find((item) => item.id === Number(id));
          if (pessoa && pessoa.idade < 18) {
            setTipo('0');
          }
        }}
        onSubmit={handleSubmit}
      />

      <ItemList
        title="Histórico de transações"
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
