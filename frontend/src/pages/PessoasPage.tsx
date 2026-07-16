import { useEffect, useState } from 'react';
import { PessoaForm } from '../components/PessoaForm';
import { ItemList } from '../components/ItemList';
import { pessoaService, type PessoaResponseDto } from '../services/api';

export function PessoasPage() {
  const [pessoas, setPessoas] = useState<PessoaResponseDto[]>([]);
  const [nome, setNome] = useState('');
  const [idade, setIdade] = useState('');
  const [feedback, setFeedback] = useState<string | null>(null);

  const carregarPessoas = async () => {
    try {
      const response = await pessoaService.listar();
      setPessoas(response);
    } catch (error) {
      setFeedback(error instanceof Error ? error.message : 'Falha ao carregar pessoas.');
    }
  };

  useEffect(() => {
    void pessoaService.listar()
      .then((response) => setPessoas(response))
      .catch((error: unknown) => {
        setFeedback(error instanceof Error ? error.message : 'Falha ao carregar pessoas.');
      });
  }, []);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    if (!nome.trim()) {
      setFeedback('Informe um nome para a pessoa.');
      return;
    }

    try {
      await pessoaService.criar({ nome: nome.trim(), idade: Number(idade || 0) });
      setNome('');
      setIdade('');
      await carregarPessoas();
      setFeedback('Pessoa cadastrada com sucesso.');
    } catch (error) {
      setFeedback(error instanceof Error ? error.message : 'Erro ao cadastrar pessoa.');
    }
  };

  const excluirPessoa = async (id: number) => {
    if (!window.confirm('Excluir esta pessoa também apagará todas as suas transações. Deseja continuar?')) {
      return;
    }

    try {
      await pessoaService.excluir(id);
      await carregarPessoas();
      setFeedback('Pessoa removida com sucesso.');
    } catch (error) {
      setFeedback(error instanceof Error ? error.message : 'Erro ao remover pessoa.');
    }
  };

  return (
    <div className="page-stack">
      <header className="page-header">
        <div>
          <p className="eyebrow">Pessoas</p>
          <h2>Cadastro e gestão</h2>
        </div>
      </header>

      {feedback ? <div className="feedback">{feedback}</div> : null}

      <PessoaForm
        nome={nome}
        idade={idade}
        onNomeChange={setNome}
        onIdadeChange={setIdade}
        onSubmit={handleSubmit}
      />

      <ItemList
        title="Lista de pessoas"
        items={pessoas}
        getKey={(pessoa) => pessoa.id}
        emptyMessage="Nenhuma pessoa cadastrada ainda."
        renderItem={(pessoa) => (
          <>
            <span>{pessoa.nome} • {pessoa.idade} anos</span>
            <button type="button" onClick={() => excluirPessoa(pessoa.id)}>
              Excluir
            </button>
          </>
        )}
      />
    </div>
  );
}
