
# Bike4Me

API para gestão de aluguel de motos e entregadores, desenvolvida como desafio técnico para vaga de backend. O projeto utiliza .NET 9, clean architecture e diversos padrões do ecossistema .NET.

---

## Como rodar o projeto

O projeto utiliza Docker Compose para facilitar a configuração dos serviços necessários (banco de dados, RabbitMQ, Redis, etc).

1. **Pré-requisitos**  
   - Docker e Docker Compose instalados

2. **Suba os containers**  
   ```bash
   docker-compose up -d
   ```

3. **Acesse a API**  
   - A aplicação estará disponível em: [http://localhost:5000](http://localhost:5000)
   - O Swagger (documentação interativa) estará em: [http://localhost:5000/swagger](http://localhost:5000/swagger)

---

## Arquitetura do Projeto

O projeto foi desenhado para demonstrar boas práticas de arquitetura, escalabilidade e manutenibilidade, utilizando conceitos modernos do desenvolvimento backend com .NET.

### Principais características

- **.NET 9**: Utiliza a última versão estável do .NET, aproveitando recursos de performance e minimal APIs.
- **Minimal APIs**: Endpoints enxutos, com menos boilerplate, facilitando manutenção e leitura.
- **CQRS (Command Query Responsibility Segregation)**:  
  - **Commands**: Escritas e alterações de estado usando EF Core.
  - **Queries**: Leitura otimizada usando Dapper para maior performance.
- **Event Driven com RabbitMQ**:  
  - Eventos importantes (ex: cadastro de moto) são publicados em filas RabbitMQ.
  - A própria aplicação consome eventos da fila e persiste em um banco NoSQL (MongoDB), seguindo uma das regras do desafio.
- **Versionamento de API**:  
  - Suporte a múltiplas versões de API, facilitando evolução sem breaking changes.
- **Swagger/OpenAPI**:  
  - Documentação automática e interativa dos endpoints, seguindo o padrão OpenAPI.
- **Autenticação JWT**:  
  - Implementação simples de autenticação baseada em JWT com ASP.NET Core.
- **Redis Output Cache**:  
  - Cache de respostas para consultas de motos, melhorando performance e escalabilidade.
- **Logger**:  
  - Logging estruturado, principalmente em integrações e pontos críticos como integraçao com a fila.
- **Seed e Migrations**:  
  - Migrations automáticas via Fluent API e seed de dados no DbContext.
- **Testes**:  
  - Testes unitários, de integração e funcionais, utilizando Fluent Assertions e Result Pattern com Fluent Validation.
  - Observação: Nem todas as classes possuem testes devido ao tempo do desafio, mas o projeto demonstra como testar as três camadas principais.
- **DDD (Domain-Driven Design)**:  
  - Separação clara entre Value Objects, Entities e Aggregates.
  - Uso de Aggregates para garantir consistência transacional.
  - **Rich Domain Model**:  
    - Utilização do padrão Factory Method para criação de entidades, garantindo invariantes e encapsulando regras de negócio.
- **Aggregator Pattern**:  
  - Entidades agrupadas para garantir regras de consistência.
- **Código limpo e organizado**:  
  - Padrões de nomenclatura, separação de responsabilidades e uso de inglês no código.

---

## Como contribuir

Sinta-se à vontade para abrir issues ou pull requests com sugestões de melhorias.

---

## Referências e artigos recomendados

- [CQRS](https://martinfowler.com/bliki/CQRS.html)
- [Minimal APIs no .NET](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis)
- [Event Driven Architecture](https://microservices.io/patterns/data/event-driven-architecture.html)
- [Factory Method Pattern](https://refactoring.guru/pt-br/design-patterns/factory-method)
- [Fluent Validation](https://fluentvalidation.net/)
- [Testing in .NET](https://learn.microsoft.com/dotnet/core/testing/)
- [OpenAPI/Swagger](https://swagger.io/specification/)
- [Redis Output Caching](https://learn.microsoft.com/aspnet/core/performance/caching/response)
- [RabbitMQ com .NET](https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html)

---

## Observações

- O projeto foi desenvolvido em tempo limitado, priorizando demonstrar arquitetura, padrões e boas práticas.
- Algumas classes não possuem testes, mas o projeto cobre exemplos de testes unitários, de integração e funcionais.
- O domínio foi modelado de forma rica, utilizando padrões de DDD e Factory Method para garantir invariantes.


---