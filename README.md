# AiMemory 🧠

> A persistent memory backend for AI agents built with **.NET 8 Web API**. Enable long-term context retention, semantic organization, and seamless conversation continuity for LLM-powered applications.

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Swagger](https://img.shields.io/badge/API-Docs-85EA2D?logo=swagger)](https://swagger.io/)
[![AI Ready](https://img.shields.io/badge/AI-LLM%20Integration-purple?logo=openai)](https://platform.openai.com/)

---

## 📋 Table of Contents
- [✨ Why AiMemory?](#-why-aimemory)
- [🎯 Key Features](#-key-features)
- [🏗️ Architecture Overview](#️-architecture-overview)
- [🛠️ Tech Stack](#️-tech-stack)
- [🚀 Getting Started](#-getting-started)
- [🔌 API Usage Examples](#-api-usage-examples)
- [🤖 AI Integration Guide](#-ai-integration-guide)
- [🗄️ Data Model & Storage](#️-data-model--storage)
- [🧪 Testing & Development](#-testing--development)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)
- [👨‍💻 Author](#-author)

---

## ✨ Why AiMemory?

Standard LLM integrations suffer from **context amnesia** — every conversation starts fresh. AiMemory solves this by providing:

| Problem | AiMemory Solution |
|---------|------------------|
| ❌ No memory between sessions | ✅ Persistent contextual storage with user/session IDs |
| ❌ Flat conversation history | ✅ Semantic organization with metadata tagging |
| ❌ Manual context management | ✅ Clean REST API for injection & retrieval |
| ❌ Scaling challenges | ✅ Async-first architecture for high-frequency AI requests |

> 💡 Think of AiMemory as a **hippocampus for your AI agents** — storing, organizing, and recalling what matters.

---

## 🎯 Key Features

| Feature | Description |
|---------|-------------|
| 🔹 Context Persistence | Store user interactions, AI responses, and metadata with TTL support |
| 🔹 Semantic Organization | Tag, categorize, and search memories by intent, topic, or custom metadata |
| 🔹 Session Management | Isolate memories by user ID, session ID, or conversation thread |
| 🔹 Async-First Design | Non-blocking I/O for high-throughput AI agent workloads |
| 🔹 Flexible Schema | Extendable memory model for custom AI use cases |
| 🔹 RESTful API | Clean endpoints for CRUD operations on memory entries |
| 🔹 Swagger Documentation | Interactive API explorer with example payloads |
| 🔹 AI SDK Ready | Designed for easy integration with OpenAI, Semantic Kernel, or custom LLMs |

---

## 🏗️ Architecture Overview
AiMemory/
├── Core/ # Domain Layer
│ ├── Entities/ # MemoryEntry, UserContext, Session
│ ├── Interfaces/ # IMemoryRepository, IMemoryService
│ └── Specifications/ # Query filters (by user, date, tags, etc.)
│
├── Infrastructure/ # External Implementations
│ ├── Data/ # EF Core DbContext + Migrations
│ ├── Repositories/ # EF Core repository implementations
│ └── Services/ # Memory orchestration + AI adapter interfaces
│
├── Shared/ # Cross-cutting Concerns
│ ├── DTOs/ # Request/Response models (MemoryDto, QueryDto)
│ ├── Exceptions/ # Custom error types (MemoryNotFoundException)
│ └── Helpers/ # Extensions, serializers, utils
│
└── AiMemory.API/ # Presentation Layer
├── Controllers/ # MemoryController, SessionController
├── Middleware/ # Global error handling, logging
├── Properties/ # appsettings, launchSettings
└── Program.cs # DI setup, Swagger config, CORS

**Design Patterns:**
- ✅ **Clean Architecture** – Separation of concerns, testable layers
- ✅ **Repository + Unit of Work** – Abstracted, transactional data access
- ✅ **Specification Pattern** – Reusable, composable query logic
- ✅ **Dependency Injection** – Built-in .NET 8 DI container
- ✅ **CQRS-Lite** – Separated read/write models for memory operations

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| **Framework** | .NET 8 Web API (Minimal APIs or Controllers) |
| **Language** | C# 12 with modern async/await patterns |
| **ORM** | Entity Framework Core 8 |
| **Database** | SQL Server / PostgreSQL / SQLite (provider-agnostic) |
| **Caching** | Optional: Redis for frequent memory lookups |
| **AI Integration** | OpenAI SDK / Semantic Kernel / Custom adapters |
| **Docs** | Swagger/OpenAPI 3.0 + XML comments |
| **Testing** | xUnit + Moq ready structure |
| **Observability** | Serilog + Application Insights ready |

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) / PostgreSQL / or SQLite
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code + C# Dev Kit
- [Postman](https://www.postman.com/) or [Bruno](https://www.usebruno.com/) for API testing

### Quick Start
```bash
# 1. Clone the repository
git clone https://github.com/Mariomedhat899/AiMemory.git
cd AiMemory

# 2. Restore dependencies
dotnet restore

# 3. Configure connection string
# Edit: AiMemory.API/appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AiMemory;Trusted_Connection=True;"
}

# 4. Apply Entity Framework migrations
Update-Database
# OR via CLI:
dotnet ef database update

# 5. Run the API
dotnet run --project AiMemory.API
# Or press F5 in Visual Studio

🔌 API Usage Examples
➕ Store a Memory
POST /api/memory
Content-Type: application/json

{
  "userId": "user_123",
  "sessionId": "session_abc",
  "content": "User prefers dark mode and notifications disabled",
  "metadata": {
    "category": "preferences",
    "tags": ["ui", "settings"],
    "importance": "high"
  },
  "ttlHours": 720
}

🔍 Retrieve Memories
GET /api/memory?userId=user_123&tags=preferences&limit=10
Authorization: Bearer your_token_if_enabled

Response:
{
  "memories": [
    {
      "id": "mem_xyz",
      "content": "User prefers dark mode and notifications disabled",
      "metadata": { "category": "preferences", "tags": ["ui", "settings"] },
      "createdAt": "2026-05-17T10:30:00Z",
      "relevanceScore": 0.95
    }
  ],
  "totalCount": 1,
  "hasMore": false
}
🗑️ Delete Memory
DELETE /api/memory/{memoryId}
💡 All endpoints support filtering by userId, sessionId, tags, dateRange, and metadata fields.
🤖 AI Integration Guide
With OpenAI SDK
// 1. Retrieve relevant memories before calling LLM
var memories = await _memoryService.GetMemoriesAsync(
    userId: "user_123", 
    tags: new[] { "preferences", "history" },
    limit: 5);

// 2. Inject into system prompt
var systemPrompt = $@"
You are a helpful assistant. 
User context from memory:
{string.Join("\n", memories.Select(m => m.Content))}

Respond naturally while respecting stored preferences.";

// 3. Call OpenAI
var response = await openAiChatClient.CompleteAsync(systemPrompt, userMessage);

// 4. Optionally store new memory from interaction
await _memoryService.StoreMemoryAsync(new MemoryEntry {
    UserId = "user_123",
    Content = $"User asked about: {userMessage}",
    Metadata = new { category = "interaction", timestamp = DateTime.UtcNow }
});
With Semantic Kernel
// Register AiMemory as a SK plugin
kernel.Plugins.AddFromObject(new MemoryPlugin(_memoryService));

// Use in prompt function
var prompt = @"
{{$input}}
Context from memory: {{Memory.GetRelevant $userId $tags}}
";
🔗 See /examples folder for full integration samples with OpenAI, Semantic Kernel, and custom LLM adapters.
🗄️ Data Model & Storage
Core Entity: MemoryEntry
public class MemoryEntry
{
    public Guid Id { get; set; }
    public string UserId { get; set; }      // Indexed for fast lookup
    public string? SessionId { get; set; }  // Optional conversation thread
    public string Content { get; set; }     // The actual memory text
    public string? MetadataJson { get; set; } // Flexible JSON metadata
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; } // TTL support
    public int RelevanceScore { get; set; }  // For semantic ranking
}
Indexing Strategy
✅ UserId + CreatedAt – Fast user timeline queries
✅ MetadataJson (partial) – Filter by category/tags via JSON functions
✅ Full-text search ready – Enable for semantic content lookup

Migration Example
# Add new memory field
Add-Migration AddRelevanceScoreToMemory

# Update database
Update-Database

# Seed initial schema
dotnet run -- --seed

🧪 Testing & Development
# Run unit tests
dotnet test

# Run integration tests (requires test DB)
dotnet test --filter "Category=Integration"

# Watch mode for development
dotnet watch run --project AiMemory.API

# Generate Swagger JSON
curl https://localhost:7001/swagger/v1/swagger.json -o openapi.json

Test Coverage Goals:
✅ Core domain logic: 90%+
✅ API endpoints: 80%+
✅ Integration tests: Critical paths covered
🤝 Contributing
Contributions are welcome! Please follow these steps:
Fork the repository
Create your feature branch (git checkout -b feat/add-semantic-search)
Commit your changes (git commit -m 'feat: add vector similarity search')
Push to the branch (git push origin feat/add-semantic-search)
Open a Pull Request with a clear description and tests
Development Guidelines
Follow C# coding conventions & .editorconfig
Add XML comments for public APIs (auto-generates Swagger docs)
Include unit tests for new services/controllers
Use conventional commits: feat:, fix:, docs:, refactor:, test:
Update API examples if endpoints change
📄 License
Distributed under the MIT License. See LICENSE for more information.
👨‍💻 Author
Mario Medhat
GitHub: @Mariomedhat899
.NET Backend Developer | AI Infrastructure Enthusiast
