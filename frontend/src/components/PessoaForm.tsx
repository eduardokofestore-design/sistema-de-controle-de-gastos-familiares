import type { FormEvent } from 'react';

type PessoaFormProps = {
  nome: string;
  idade: string;
  onNomeChange: (value: string) => void;
  onIdadeChange: (value: string) => void;
  onSubmit: (event: FormEvent<HTMLFormElement>) => void;
};

export function PessoaForm({ nome, idade, onNomeChange, onIdadeChange, onSubmit }: PessoaFormProps) {
  return (
    <section className="panel">
      <h2>Cadastrar pessoa</h2>
      <form onSubmit={onSubmit} className="stack">
        <input
          placeholder="Nome"
          value={nome}
          onChange={(event) => onNomeChange(event.target.value)}
        />
        <input
          type="number"
          placeholder="Idade"
          min="0"
          required
          value={idade}
          onChange={(event) => onIdadeChange(event.target.value)}
        />
        <button type="submit">Salvar pessoa</button>
      </form>
    </section>
  );
}
