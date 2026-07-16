# Controle de Gastos

Aplicação full stack para cadastro de pessoas, lançamento de receitas e despesas e consulta de totais financeiros.

## Tecnologias

- Back-end: .NET 10, ASP.NET Core, Entity Framework Core e SQLite
- Front-end: React, TypeScript e Vite
- Testes: xUnit

## Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) 20 ou superior

## Como executar

Em um terminal, inicie a API:

```powershell
cd ControleGastos.API
dotnet run
```

A API fica disponível em `http://localhost:5175` e a documentação Swagger, em desenvolvimento, em `http://localhost:5175/swagger`. As migrações são aplicadas automaticamente e os dados ficam no arquivo SQLite local `controlegastos.db`.

Em outro terminal, inicie o front-end:

```powershell
cd frontend
npm ci
npm run dev
```

No PowerShell, caso `npm` seja bloqueado pela política de execução, use `npm.cmd` nos comandos acima. A aplicação abre normalmente em `http://localhost:5173`.

## Configuração

O front-end aceita a variável opcional `VITE_API_BASE_URL`. Copie o exemplo para um arquivo local antes de alterar a URL da API:

```powershell
Copy-Item frontend/.env.example frontend/.env.local
```

Arquivos `.env` e bancos SQLite são ignorados pelo Git. Não envie credenciais, tokens ou dados reais ao repositório.

## Testes e verificações

Na raiz do repositório:

```powershell
dotnet test ControleGastos.API.Tests/ControleGastos.API.Tests.csproj
dotnet build ControleGastos.API/ControleGastos.API.csproj
```

No diretório `frontend`:

```powershell
npm run lint
npm run build
```

## Regras de negócio

- Pessoas possuem identificador automático, nome e idade.
- Excluir uma pessoa remove suas transações.
- Menores de 18 anos só podem registrar despesas.
- O relatório exibe receitas, despesas e saldo por pessoa, além dos totais gerais.

## Contribuição

Consulte [CONTRIBUTING.md](CONTRIBUTING.md) e o [Código de Conduta](CODE_OF_CONDUCT.md) antes de abrir uma issue ou pull request.

## Segurança

Para reportar uma vulnerabilidade, siga as instruções em [SECURITY.md](SECURITY.md).

## Licença

Distribuído sob a licença [MIT](LICENSE).
