# AiMemory 🧠

**AiMemory** is a dedicated backend service built with **.NET 8** to solve the "forgetfulness" problem in standard AI integrations. It provides a structured way to store and retrieve contextual memory, allowing AI agents to maintain continuity across multiple sessions.

## 🚀 Key Features
* **Context Persistence:** Efficiently stores user interactions and AI responses.
* **Semantic Organization:** Designed to bridge the gap between volatile conversation history and long-term storage.
* **Scalable Architecture:** Built with asynchronous patterns to handle high-frequency AI requests.
* **Developer-Friendly API:** Clean REST endpoints for memory injection and retrieval.

## 🛠️ Technical Stack
* **Framework:** .NET 8 (Web API)
* **Data Access:** Entity Framework Core
* **Intelligence Layer:** (Add your specific AI SDK here, e.g., OpenAI / Semantic Kernel)
* **Tools:** Swagger UI for API documentation.

## 🔧 Configuration
To get started, update your connection strings and AI API keys in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_SQL_Server_Connection_String"
  },
  "AiConfig": {
    "ApiKey": "Your_Key_Here"
  }
}
