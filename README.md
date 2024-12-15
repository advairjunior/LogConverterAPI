# LogConverterAPI

## Descrição
A **LogConverterAPI** é uma aplicação desenvolvida em .NET para conversão de logs em diferentes formatos. Ela permite a manipulação, consulta, armazenamento e exportação de logs, sendo uma solução versátil para integração com diversos sistemas.

## Funcionalidades
- Conversão de logs do formato CND para o formato Agora.
- Armazenamento de logs em banco de dados.
- Recuperação de logs armazenados, com suporte à busca por ID.
- Exportação de logs para arquivos.
- Suporte a padrões de API RESTful.

## Tecnologias Utilizadas
### Backend
- **.NET 8.0**: Framework principal para desenvolvimento da aplicação.
- **ASP.NET Core**: Para criação de endpoints RESTful.
- **Newtonsoft.Json**: Para serialização e desserialização de objetos JSON.
- **Entity Framework Core**: Para mapeamento objeto-relacional e manipulação do banco de dados.

### Testes
- **xUnit**: Framework utilizado para testes unitários e de integração.
- **HttpClient**: Para simular requisições HTTP nos testes.

### Banco de Dados
- **SQL Server**: Gerenciador de banco de dados utilizado para armazenamento.

### Ferramentas de Suporte
- **Git**: Controle de versão.
- **Visual Studio**: IDE utilizada no desenvolvimento.
