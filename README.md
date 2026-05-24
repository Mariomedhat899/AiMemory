# 🤖 GroqChat MemoryBot

A persistent-memory console chatbot for **Groq API** models (Qwen, Llama 3, Mixtral, and more). Built with C# .NET 8.

![Demo](https://via.placeholder.com/800x400/1e1e2e/ffffff?text=Console+Chat+Demo)  
*Screenshot: Chatting with Qwen 32B via Groq*

---

## ✨ Features

- 🧠 **Persistent Memory**: Conversation history saved to `chat_history.json`
- 🔐 **Secure by Default**: API key via environment variable (never hardcoded)
- 🔄 **Multi-Model Support**: Switch between Groq models at runtime
- 🎨 **Console UI**: Color-coded messages, clean input/output
- ⚡ **Fast**: Leverages Groq's ultra-low-latency inference
- 🛠️ **Commands**: `/exit` to quit, `/clear` to wipe memory

---

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK ([Download](https://dotnet.microsoft.com/download))
- Groq API key ([Get free key](https://console.groq.com/keys))

### 1. Clone & Build
```bash
git clone https://github.com/yourusername/groq-chat-memorybot.git
cd groq-chat-memorybot
dotnet build
