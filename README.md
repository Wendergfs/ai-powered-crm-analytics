# 🤖 AI-Powered CRM Analytics

> A smart CRM platform built with **ASP.NET Core 8** that leverages AI to analyze client profiles, generate insights, and export reports — powered by either a local **Ollama** model or a rule-based fallback engine.

---

## ✨ Features

- 👥 **Client Management** — Create, view, update, and delete client records
- 🧠 **AI-Powered Analysis** — Analyze client behavior with Ollama (local LLM) or a rule-based engine
- 📊 **Dashboard & Charts** — Visual analytics via ScottPlot
- 📄 **PDF Export** — Generate professional client reports with QuestPDF
- 🔐 **Authentication** — Secure login/register system powered by ASP.NET Identity
- 🗄️ **SQL Server Database** — Persistent storage with Entity Framework Core & migrations

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 (MVC) |
| Language | C# (.NET 8) |
| Database | SQL Server + EF Core 8 |
| Auth | ASP.NET Identity |
| AI Engine | Ollama (local LLM) + Rule-Based fallback |
| PDF Generation | QuestPDF 2025 |
| Charts | ScottPlot 5 |
| Frontend | Razor Views + HTML/CSS/JS |

---

## 📁 Project Structure

```
AIClientManager/
├── Controllers/       # MVC Controllers (Clients, Account, Dashboard...)
├── DTOs/              # Data Transfer Objects
├── Data/              # AppDbContext + EF Core configuration
├── Migrations/        # EF Core database migrations
├── Models/            # Domain models (Client, ApplicationUser...)
├── Services/          # Business logic & AI services
│   ├── OllamaClientAnalysisService.cs   # AI via Ollama HTTP client
│   └── RuleBasedClientAnalysisService.cs # Fallback rule engine
├── Pdf/               # PDF report generation
├── Views/             # Razor templates
├── wwwroot/           # Static assets (CSS, JS, images)
├── Program.cs         # App entry point & DI configuration
└── appsettings.json   # Configuration (connection string, etc.)
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (or SQL Server Express / LocalDB)
- *(Optional)* [Ollama](https://ollama.ai/) running locally for AI features

### 1. Clone the repository

```bash
git clone https://github.com/Wendergfs/ai-powered-crm-analytics.git
cd ai-powered-crm-analytics
```

### 2. Configure the database

Edit `appsettings.Development.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AIClientManager;Trusted_Connection=True;"
  }
}
```

### 3. Apply migrations

```bash
dotnet ef database update
```

### 4. (Optional) Start Ollama

To enable the AI analysis feature, make sure Ollama is running locally:

```bash
ollama serve
ollama pull <your-model>
```

> If Ollama is not available, the app automatically falls back to the built-in rule-based analysis engine.

### 5. Run the application

```bash
dotnet run
```

The app will be available at `https://localhost:5001` (or the port shown in the terminal).

---

## ⚙️ Configuration

| Key | Description |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| Ollama base URL | Configured in `appsettings.json` under the Ollama service |

---

## 🧠 AI Analysis Modes

The application supports two analysis modes, resolved via dependency injection:

- **`OllamaClientAnalysisService`** — Sends client data to a local Ollama LLM and returns AI-generated insights.
- **`RuleBasedClientAnalysisService`** — A deterministic fallback that analyzes clients using predefined business rules.

Both implement the `IClientAnalysisService` interface, making them interchangeable.

---

## 📄 PDF Reports

Client reports are generated using **QuestPDF** (Community License) and can be downloaded directly from the client detail page.

---

## 🤝 Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request.

1. Fork the project
2. Create your feature branch: `git checkout -b feature/my-feature`
3. Commit your changes: `git commit -m 'Add my feature'`
4. Push to the branch: `git push origin feature/my-feature`
5. Open a Pull Request

---

## 📜 License

This project is open source. See [LICENSE](LICENSE) for details.

---

<div align="center">
  Made by <a href="https://github.com/Wendergfs">Mohamed Aziz Gafsi</a>
</div>
