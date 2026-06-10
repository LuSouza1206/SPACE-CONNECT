# SpaceConnect

Sistema web para catalogar e visualizar tecnologias derivadas da exploração espacial.

Desenvolvido como Global Solution — FIAP 2025 | Engenharia de Software | C# Development

---

## Stack

- **.NET 9** com .NET Aspire (orquestração)
- **ASP.NET Core Web API** — back-end com EF Core + MySQL
- **ASP.NET Core MVC** — front-end desacoplado via HttpClient
- **MySQL** — banco relacional
- **BCrypt** — hash de senhas
- **Bootstrap 5** — painel com cards, barras de progresso e tabela responsiva

---

## Como rodar

### Pré-requisitos

- .NET 9 SDK
- MySQL rodando na porta 3306
- Workload Aspire: `dotnet workload install aspire`

### Banco de dados

```bash
mysql -u root -p < spaceconnect_db.sql
```

### Projetos individuais (sem Aspire)

```bash
# API (porta 5001)
cd SpaceConnect.ApiService
dotnet run

# Web (porta 5000)
cd SpaceConnect.Web
dotnet run
```

### Com Aspire

```bash
cd SpaceConnect.AppHost
dotnet run
```

Acesse o dashboard Aspire em `http://localhost:15888`

---

## Estrutura

```
SpaceConnect/
├── SpaceConnect.AppHost/          # Orquestrador .NET Aspire
├── SpaceConnect.ApiService/       # Web API REST
│   ├── Controllers/               # TecnologiasController, AuthController, etc
│   ├── Data/
│   │   ├── Interfaces/            # ITecnologiaRepository, IUsuarioRepository
│   │   └── Repositories/         # Implementações concretas
│   ├── DTOs/                      # Data Transfer Objects
│   └── Models/                    # Entidades EF Core
└── SpaceConnect.Web/              # ASP.NET Core MVC
    ├── Controllers/               # HomeController, TecnologiasController, AuthController
    ├── Models/                    # ViewModels
    ├── Services/                  # ApiService (HttpClient)
    ├── Views/                     # Razor Views
    └── wwwroot/css/               # CSS personalizado
```

---

## Endpoints da API

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | /api/tecnologias | Lista todas as tecnologias |
| GET | /api/tecnologias/{id} | Busca tecnologia por ID |
| POST | /api/tecnologias | Cria nova tecnologia |
| PUT | /api/tecnologias/{id} | Atualiza tecnologia |
| DELETE | /api/tecnologias/{id} | Remove tecnologia |
| GET | /api/tecnologias/stats | Estatísticas para o dashboard |
| GET | /api/categorias | Lista categorias de impacto |
| GET | /api/missoes | Lista missões espaciais |
| POST | /api/auth/login | Autenticação |
| POST | /api/auth/cadastro | Cadastro de usuário |

---

## Perfis de acesso

| Perfil | Pode fazer |
|--------|-----------|
| Pesquisador | Visualizar dashboard, listar e ver detalhes |
| Administrador | Tudo acima + criar, editar e excluir tecnologias |

---

## Commits semânticos sugeridos

```
feat: setup .NET Aspire solution structure
feat: add database models and EF Core context
feat: implement repository pattern with interfaces
feat: add tecnologias CRUD endpoints
feat: add stats endpoint for dashboard data
feat: add BCrypt authentication endpoints
feat: setup MVC project with HttpClient service
feat: add dashboard with metrics cards, progress bars and table
feat: implement cookie-based auth with claims
feat: add tecnologias CRUD views with spatial design
fix: adjust CORS policy for local development
docs: add README with setup instructions
docs: add equipes.txt and spaceconnect_db.sql with demo users
```

---

## Entrega FIAP

| Item | Arquivo |
|------|---------|
| Script do banco | `spaceconnect_db.sql` |
| Equipe (RM, nome, turma) | `equipes.txt` |
| Modelo Workbench | `spaceconnect.mwb` — ver passos abaixo |

### Contas demo

| Perfil | E-mail | Senha |
|--------|--------|-------|
| Administrador | `admin@spaceconnect.fiap` | `Admin@2025` |
| Pesquisador | `pesquisador@spaceconnect.fiap` | `Pesq@2025` |

### Como gerar o `spaceconnect.mwb` (5 minutos)

Você **não precisa** modelar na mão. O Workbench monta o diagrama a partir do nosso SQL.

1. Instale o [MySQL Workbench](https://dev.mysql.com/downloads/workbench/) (grátis).
2. Abra o Workbench → **File** → **New Model**.
3. **File** → **Run SQL Script…** → escolha `spaceconnect_db.sql` (pasta do projeto) → **Run**.
4. No menu: **File** → **Import** → **Reverse Engineer SQL Script…** → de novo o `spaceconnect_db.sql` → avançar até concluir.
5. Aparece o diagrama com as 4 tabelas e as setas (FK). Confira se bate: `categorias_impacto`, `missoes`, `usuarios`, `tecnologias`.
6. **File** → **Save Model As…** → salve na pasta do projeto como **`spaceconnect.mwb`**.

Se der erro no passo 3, pule para o 4 (só o Import Reverse Engineer SQL já costuma bastar).

### Git (repositório)

O repositório local já pode ser inicializado na pasta `SpaceConnect/`. Peça ao colega de equipe ou use o Cursor para criar os commits semânticos listados acima antes de subir no GitHub/GitLab da FIAP.
