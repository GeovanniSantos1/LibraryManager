# 📚 LibraryManager - Sistema de Gerenciamento de Biblioteca  

## 🚀 Visão Geral  
O **LibraryManager** é um sistema completo de gerenciamento de biblioteca desenvolvido em **ASP.NET Core**, seguindo os princípios da **Arquitetura Limpa** e padrões modernos de desenvolvimento.  

Este projeto foi desenvolvido como parte do programa de mentoria da **Next Wave Education**, sob orientação do mentor **Luis Felipe**.  

---

## 🏗️ Arquitetura  

O sistema segue uma arquitetura em camadas bem definidas, garantindo **separação de responsabilidades** e facilitando a manutenção.  

### 📋 Camadas do Projeto  

- **API**: Controllers, middlewares e configurações para exposição dos endpoints RESTful.  
- **Application**: Implementação dos casos de uso utilizando **CQRS** com **MediatR**.  
- **Core**: Contém as entidades de domínio e regras de negócio fundamentais.  
- **Infrastructure**: Implementações concretas de repositórios, serviços externos e persistência.  

---

## 💻 Tecnologias Utilizadas  

- **ASP.NET Core 8.0** - Framework web moderno e de alto desempenho  
- **Entity Framework Core** - ORM para acesso a dados  
- **SQL Server** - Banco de dados relacional  
- **MediatR** - Implementação do padrão **Mediator** para **CQRS**  
- **Mailgun** - Serviço para envio de e-mails transacionais  
- **JWT** - Autenticação baseada em tokens  

---

## ✨ Funcionalidades Implementadas  

### 📖 Gerenciamento de Livros  
✔ Cadastro, consulta, atualização e remoção de livros  
✔ Busca por **título, autor e categoria**  
✔ Controle de **disponibilidade**  

### 👥 Gerenciamento de Usuários  
✔ Cadastro e autenticação de usuários  
✔ Perfis de acesso (**Administrador, Bibliotecário, Leitor**)  
✔ Recuperação de senha via e-mail  

### 📝 Sistema de Empréstimos  
✔ Registro de **empréstimos de livros**  
✔ Definição automática de **data de devolução**  
✔ Controle de **devoluções**  
✔ Verificação de **disponibilidade**  

### 🔐 Segurança  
✔ **Autenticação via JWT**  
✔ **Autorização baseada em perfis**  
✔ **Proteção de endpoints sensíveis**  

---

## 🛠️ Padrões de Projeto Implementados  

- **CQRS** (Command Query Responsibility Segregation) - Separação entre operações de leitura e escrita  
- **Repository Pattern** - Abstração da camada de acesso a dados  
- **Mediator** - Comunicação desacoplada entre componentes  
- **Dependency Injection** - Inversão de controle para melhor testabilidade  
- **Unit of Work** - Gerenciamento de transações  

---

## 🚀 Como Executar o Projeto  

### 📌 Pré-requisitos  
- **.NET 8.0 SDK**  
- **SQL Server**  
- **Conta no Mailgun** (para envio de e-mails)  

### 🔧 Configuração  
1. **Clone o repositório**  
   ```bash
   git clone https://github.com/seu-usuario/librarymanager.git
   cd librarymanager
   ```

2. **Configure as variáveis de ambiente ou User Secrets**  
3. **Execute as migrações do banco de dados**  
   ```bash
   dotnet ef database update
   ```
4. **Execute o projeto**  
   ```bash
   dotnet run
   ```

---

## 📊 Próximos Passos  

✔ Dashboard administrativo
✔ Relatórios e estatísticas de uso
✔ Integração com sistemas de catalogação externos
✔ Integração com AWS S3 para armazenamento de arquivos e imagens de livros
✔ Uso do AWS Cognito para autenticação e gestão de usuários
✔ Implementação de AWS Lambda para processamento assíncrono
✔ Configuração de AWS SNS/SQS para filas e notificações de eventos
✔ Uso do AWS RDS para escalabilidade do banco de dados 

---

## 👨‍💻 Contribuição  

Contribuições são **bem-vindas**! Sinta-se à vontade para abrir **issues** ou enviar **pull requests** com melhorias.  

---

## 📄 Licença  

N/A

---

💙 **Desenvolvido com ❤️ como parte do programa de mentoria da Next Wave Education.**  

---

Essa versão melhora a **legibilidade**, estrutura e formatação, garantindo que o **README.md** fique mais profissional e fácil de entender. 🚀
