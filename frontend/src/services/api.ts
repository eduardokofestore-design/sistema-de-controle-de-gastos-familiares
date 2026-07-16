const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5175';

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    headers: {
      'Content-Type': 'application/json',
      ...(init?.headers ?? {}),
    },
    ...init,
  });

  if (!response.ok) {
    const errorData = await response.json().catch(() => null);
    const validationMessage = Object.values(errorData?.errors ?? {})
      .flat()
      .find((message): message is string => typeof message === 'string');
    const message =
      typeof errorData?.mensagem === 'string'
        ? errorData.mensagem
        : validationMessage ??
          'Ocorreu um erro inesperado.';

    throw new Error(message);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json() as Promise<T>;
}

export interface PessoaCreateDto {
  nome: string;
  idade: number;
}

export interface PessoaResponseDto {
  id: number;
  nome: string;
  idade: number;
}

export interface TransacaoCreateDto {
  descricao: string;
  valor: number;
  tipo: number;
  pessoaId: number;
}

export interface TransacaoResponseDto {
  id: number;
  descricao: string;
  valor: number;
  tipo: number;
  pessoaId: number;
  pessoaNome: string;
}

export interface RelatorioTotalDto {
  pessoas: Array<{
    pessoaId: number;
    nome: string;
    totalReceitas: number;
    totalDespesas: number;
    saldo: number;
  }>;
  totalReceitasGeral: number;
  totalDespesasGeral: number;
  saldoGeral: number;
}

export const pessoaService = {
  listar: () => request<PessoaResponseDto[]>('/api/pessoas'),
  criar: (payload: PessoaCreateDto) => request<PessoaResponseDto>('/api/pessoas', {
    method: 'POST',
    body: JSON.stringify(payload),
  }),
  excluir: (id: number) => request<void>(`/api/pessoas/${id}`, {
    method: 'DELETE',
  }),
};

export const transacaoService = {
  listar: () => request<TransacaoResponseDto[]>('/api/transacoes'),
  criar: (payload: TransacaoCreateDto) => request<TransacaoResponseDto>('/api/transacoes', {
    method: 'POST',
    body: JSON.stringify(payload),
  }),
};

export const relatorioService = {
  obterTotais: () => request<RelatorioTotalDto>('/api/relatorios/totais'),
};
