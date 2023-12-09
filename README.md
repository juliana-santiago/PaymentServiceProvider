# Desafio - Stone

#### Executando o projeto

1) Rode o seguinte comando na pasta root do projeto: ```docker-compose up```
2) Navegue até a página http://localhost:8080/swagger/index.html

### Executando Testes

1. Navegue até o projeto de testes ```cd PaymentServiceProvider/test/PaymentServiceProvider.Test```
2. Execute o seguinte comando para executar os testes utilizando xUnit: ```dotnet test```
   
### Endpoints

#### 1. GET /transactions

- *Método:* GET
- *Endpoint:* /api/transactions
- *Descrição:* Retorna todas as transações.

*Exemplo de Retorno da Requisição*
```json
[
  {
    "transactionType": 2456.30,
    "cardNumber": "6961",
    "cardHolderNamer": "Nome do Proprietario do Cartao",
    "expirationDate": "2024-12-15T00:00:00",
    "codeCVC": "145",
    "description": "Smartband XYZ 3.0",
    "value": 0.0
  }
]
```

#### 2. POST /transactions

- *Método:* POST
- *Endpoint:* /api/transactions
- *Descrição:* Insere uma nova transação e o seu respectivo payable.

*Exemplo de Corpo da Requisição*
```json
{
  "transactionValue": 0.0,
  "transactionDescription": "string",
  "paymentMethod": "credit_card|debit_card",
  "cardNumber": "string",
  "cardHolderName": "string",
  "expirationDate": "mm/dd/yyyy",
  "codeCVC": "string"
}
```

#### 3. GET /payables/{cardHolderName}

- *Método:* GET
- *Endpoint:* /api/payables/{cardHolderName}
- *Descrição:* Recupera lista de payables através do nome do usuário.

*Exemplo de Retorno da Requisição*
```json
{
  "totalAvailable": 119729.04,
  "totalWaitingFunds": 1564.87
}
```
