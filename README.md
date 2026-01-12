Um cliente tem necessidade de buscar livros em um catálogo. Esse cliente quer ler e buscar esse catálogo de um arquivo JSON, e esse arquivo não pode ser modificado. Então com essa informação, é preciso desenvolver:

    Criar uma API para buscar produtos no arquivo JSON disponibilizado.
    Que seja possível buscar livros por suas especificações(autor, nome do livro ou outro atributo)
    É preciso que o resultado possa ser ordenado pelo preço.(asc e desc)
    Disponibilizar um método que calcule o valor do frete em 20% o valor do livro.

Será avaliado no desafio:

    Organização de código;
    Manutenibilidade;
    Princípios de orientação à objetos;
    Padrões de projeto;
    Teste unitário

Para nos enviar o código, crie um fork desse repositório e quando finalizar, mande um pull-request para nós.

O projeto deve ser desenvolvido em C#, utilizando o .NET Core 3.1 ou superior.

Gostaríamos que fosse evitado a utilização de frameworks, e que tivesse uma explicação do que é necessário para funcionar o projeto e os testes.

----------
### Requisitos do projeto
1. .Net 9 ou superior
----------
### Como executar o projeto
1. clonar o projeto `git clone https://github.com/kamilly-quadros/VirtualWorlds.git`
2. acessar o projeto e executar `dotnet run`
3. link para acessar a API:
- http://localhost:5172/swagger/index.html
- https://localhost:7282/swagger/index.html
----------
## Virtual Worlds

Uma variedade de mundos na palma da sua mão!

## Estrutura do projeto

**Business**

- Camada que contém validações

**Controllers**

- Camada que contém os endpoints

**Data**

- Camada que lida com o banco de dados, carregamento inicial e contexto

**DTOs**

- Camada _Data Transfer Object_ para auxiliar na leitura do JSON

**Models**

- Camada que espelha as tabelas do banco

**Services**

- Camada de utilidades gerais

**AppSettings**

- Os arquivos _appsettings_ contém a porcentagem do frete. Escolhi colocar no arquivo de configurações para facilitar na manutenção caso precise alterar posteriormente