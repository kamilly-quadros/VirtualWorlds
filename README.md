# 📚 Virtual Worlds

> Uma variedade de mundos na palma da sua mão!

## 📋 Descrição do Projeto

Este projeto consiste em uma API RESTful desenvolvida em .NET para gerenciamento de um catálogo de livros. A aplicação permite buscar livros a partir de um arquivo JSON, com funcionalidades de filtragem, ordenação e cálculo de frete.

## ✨ Funcionalidades

- 🔍 Busca de livros por diversos critérios (título, autor, preço, etc.)
- 🔄 Ordenação dos resultados por preço (crescente e decrescente)
- 📦 Cálculo automático de frete (20% do valor do livro)
- 📚 Catálogo baseado em arquivo JSON
- 📄 Documentação interativa via Swagger

## 🚀 Requisitos

- [.NET 9.0](https://dotnet.microsoft.com/download/dotnet/9.0) ou superior
- Git (para clonar o repositório)

## 🛠️ Como Executar

1. **Clonar o repositório**
   ```bash
   git clone https://github.com/kamilly-quadros/VirtualWorlds.git
   cd VirtualWorlds
   ```

2. **Executar o projeto**
   ```bash
   cd VirtualWorlds.Server
   dotnet clean
   dotnet restore
   dotnet build
   dotnet run
   ```

3. **Acessar a documentação da API**
   - HTTP: [http://localhost:5172/swagger/index.html](http://localhost:5172/swagger/index.html)
   - HTTPS: [https://localhost:7282/swagger/index.html](https://localhost:7282/swagger/index.html)

4. **Executar testes**
  ```bash
  Acessar o projeto de teste: cd VirtualWorlds.Test
  Executar: dotnet test
  ```

## 🏗️ Estrutura do Projeto

O projeto segue uma arquitetura em camadas bem definida:

- **Business**
  - Lógica de negócios e validações
  - Implementação de regras específicas do domínio

- **Controllers**
  - Endpoints da API REST
  - Tratamento de requisições HTTP

- **Data**
  - Acesso a dados
  - Carga inicial do catálogo de livros
  - Configuração do contexto de dados

- **DTOs (Data Transfer Objects)**
  - Objetos para transferência de dados
  - Mapeamento entre entidades e modelos de API

- **Models**
  - Entidades de domínio
  - Estrutura de dados do sistema

- **Services**
  - Serviços compartilhados
  - Lógica de negócios reutilizável

- **AppSettings**
  - Configurações da aplicação
  - Parâmetros como porcentagem do frete
