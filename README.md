# Projeto Barbearia Backend

Este é o backend do projeto de barbearia, desenvolvido em **C# .NET 8**. O objetivo do projeto é gerenciar o agendamento de serviços, implementar um sistema de pontuação/fidelização e permitir a troca de pontos acumulados por produtos.

## 📋 Pré-requisitos

Antes de começar, certifique-se de que sua máquina atende aos seguintes requisitos:

- [SDK .NET 8](https://dotnet.microsoft.com/)
- Um banco de dados MySQL configurado
- [Postman](https://www.postman.com/) ou outra ferramenta para testar as APIs (opcional)

## ⚙️ Configuração

### 1. Variáveis de Ambiente

O modelo das Variáveis de Ambiente pode ser consultado no arquivo .env.exemple

### 2. Migrar o Banco de Dados

Certifique-se de que o banco de dados esteja configurado corretamente. Em seguida, execute as migrações para criar as tabelas:

```bash
dotnet ef database update
```

### 3. Rodar o Seeder do Usuário

Para popular o banco de dados com dados iniciais de usuário, execute o comando abaixo:

```bash
dotnet run --seed User
```

### 4. Executar a Aplicação

Inicie o servidor utilizando o seguinte comando:

```bash
dotnet run
```

O backend estará disponível no endereço:

```
http://localhost:5000
```

## ▶️ Funcionalidades

- **Gerenciamento de Agendamentos:** Criação, edição e exclusão de serviços agendados.
- **Sistema de Pontuação:** Os clientes acumulam pontos a cada serviço concluído.
- **Troca de Pontos:** Pontos acumulados podem ser trocados por produtos no sistema.

## 🛠️ Tecnologias Utilizadas

- **[C# .NET 8](https://dotnet.microsoft.com/):** Framework para desenvolvimento de aplicações modernas e performáticas.
- **[Entity Framework Core](https://docs.microsoft.com/ef/):** ORM para gerenciamento do banco de dados.
- **[MySQL](https://www.mysql.com/):** Sistema de gerenciamento de banco de dados.
- **[JWT](https://jwt.io/):** Implementação de autenticação segura.

## 🚀 Deploy

Para realizar o deploy da aplicação, utilize um servidor web como o IIS ou conteinerize com Docker. Aqui está um exemplo básico de Dockerfile:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "BarbeariaBackend.dll"]
```

## 🤝 Contribuindo

Contribuições são bem-vindas! Se você deseja contribuir com o projeto, siga os passos abaixo:

1. Faça um fork do repositório.
2. Crie uma branch com sua feature ou correção: `git checkout -b minha-feature`.
3. Commit suas mudanças: `git commit -m 'Adicionei uma nova feature'`.
4. Faça um push para a branch: `git push origin minha-feature`.
5. Abra um pull request.

## 📝 Licença

Este projeto está licenciado sob a licença MIT. Consulte o arquivo `LICENSE` para mais informações.

---

Feito com ❤️ por [Thiago Santos](https://github.com/tbsantosDev).
