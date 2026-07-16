import type { FormEvent } from 'react';
import type { PessoaResponseDto } from '../services/api';

type TransacaoFormProps = {
  descricao: string;
  valor: string;
  tipo: string;
  pessoaId: string;
  pessoas: PessoaResponseDto[];
  onDescricaoChange: (value: string) => void;
  onValorChange: (value: string) => void;
  onTipoChange: (value: string) => void;
  onPessoaChange: (value: string) => void;
  onSubmit: (event: FormEvent<HTMLFormElement>) => void;
};

export function TransacaoForm({
  descricao,
  valor,
  tipo,
  pessoaId,
  pessoas,
  onDescricaoChange,
  onValorChange,
  onTipoChange,
  onPessoaChange,
  onSubmit,
}: TransacaoFormProps) {
  const pessoaSelecionada = pessoas.find((pessoa) => pessoa.id === Number(pessoaId));
  const receitaBloqueada = pessoaSelecionada !== undefined && pessoaSelecionada.idade < 18;

  return (
    <section className="panel">
      <h2>Registrar transação</h2>
      <form onSubmit={onSubmit} className="stack">
        <input
          placeholder="Descrição"
          value={descricao}
          onChange={(event) => onDescricaoChange(event.target.value)}
        />
        <input
          type="number"
          step="0.01"
          placeholder="Valor"
          min="0.01"
          required
          value={valor}
          onChange={(event) => onValorChange(event.target.value)}
        />
        <select value={tipo} onChange={(event) => onTipoChange(event.target.value)}>
          <option value="0">Despesa</option>
          <option value="1" disabled={receitaBloqueada}>Receita</option>
        </select>
        <select value={pessoaId} onChange={(event) => onPessoaChange(event.target.value)} required>
          <option value="">Selecione uma pessoa</option>
          {pessoas.map((pessoa) => (
            <option key={pessoa.id} value={pessoa.id}>
              {pessoa.nome}
            </option>
          ))}
        </select>
        {receitaBloqueada ? <p className="field-hint">Menores de idade só podem registrar despesas.</p> : null}
        <button type="submit">Salvar transação</button>
      </form>
    </section>
  );
}
