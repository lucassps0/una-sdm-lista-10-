# una-sdm-lista-10

Entrega da Lista 10 de Sistemas Distribuidos e Mobile.

## Estrutura

- `OscarFilmeApi/` -> codigo da API
- `prints/` -> prints das rotas funcionando
- `LICENSE` -> licenca do repositorio

## Como rodar

```powershell
cd OscarFilmeApi
dotnet restore
dotnet run
```

Abra o Swagger no endereco informado pelo terminal.

## Sobre o banco In-Memory

Os dados ficam apenas na memoria. Se o servidor for encerrado e iniciado novamente, a lista de filmes volta vazia.
