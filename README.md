# 🧠 AiMemory — AI Chat with Persistent Memory

[![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet)](https://dotnet.microsoft.com)
[![Groq](https://img.shields.io/badge/Groq-API-00B85A?logo=data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjQiIGhlaWdodD0iMjQiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48Y2lyY2xlIGN4PSIxMiIgY3k9IjEyIiByPSIxMCIgZmlsbD0id2hpdGUiLz48L3N2Zz4=)](https://console.groq.com)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A full-featured AI chat application built with **C# .NET 8** and the **Groq API**. Features a modern dark-themed Windows Forms UI, persistent conversation history, and support for multiple AI models including Qwen, Llama 3, Mixtral, and more.

![AiMemory WinForms App](https://via.placeholder.com/900x520/121218/58A6FF?text=AiMemory+WinForms+UI+—+Dark+Theme+Chat+Interface)

---

## ✨ Features

### 🖥️ Windows Forms App
- **Modern dark UI** — sleek navy/charcoal theme with blue accent colors
- **Chat bubbles** — rounded message bubbles (blue for user, dark gray for AI)
- **Typing indicator** — animated dots while waiting for AI response
- **Sidebar** — quick access to new chats and conversation history
- **Settings dialog** — configure API key and model without restarting
- **Auto-resize input** — input box grows as you type
- **Persistent history** — conversations saved to `%AppData%\AiMemoryWinForms\`
- **Keyboard shortcuts** — `Enter` to send, `Shift+Enter` for new line

### 💬 Console App
- **Color-coded output** — cyan for user, green for AI, red for errors
- **Lightweight** — runs in any terminal
- **Same core engine** — shared library with the WinForms app

### 🔧 Shared Engine
- **Persistent memory** — conversation history saved to JSON
- **Secure by default** — API key via environment variable (never hardcoded)
- **Multi-model support** — switch between Groq models at runtime
- **Rate limit handling** — automatic retry with user feedback
- **Error resilience** — graceful handling of network/API errors

---

## 🚀 Quick Start

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Groq API key](https://console.groq.com/keys) (free tier available)

### 1. Clone & Build
```bash
git clone https://github.com/Mariomedhat899/AiMemory.git
cd AiMemory
dotnet build
```

### 2. Set Your API Key

**Windows PowerShell (permanent, user-level):**
```powershell
[Environment]::SetEnvironmentVariable("GROQ_API_KEY", "gsk_YOUR_KEY_HERE", "User")
```
> ⚠️ Restart your terminal/IDE after setting.

**Linux / macOS (add to `~/.bashrc` or `~/.zshrc`):**
```bash
export GROQ_API_KEY="gsk_YOUR_KEY_HERE"
```

**Temporary (current session only):**
```bash
# PowerShell
$env:GROQ_API_KEY = "gsk_YOUR_KEY_HERE"

# Linux/macOS
export GROQ_API_KEY="gsk_YOUR_KEY_HERE"
```

### 3. Run the Windows Forms App
```bash
dotnet run --project AiMemoryWinForms
```

### 4. Run the Console App
```bash
dotnet run --project MemoryManagment
```

### 5. Choose a Model
When prompted (console) or in Settings (WinForms), pick a model:

| Model | Speed | Best For |
|---|---|---|
| `llama-3.1-8b-instant` | ⚡⚡⚡ | Fast chat, testing |
| `qwen/qwen3-32b` | ⚡ | Multilingual, coding |
| `llama-3.3-70b-versatile` | ⚡ | Complex reasoning |
| `mixtral-8x7b-32768` | ⚡⚡ | Long context (32K) |
| `gemma2-9b-it` | ⚡⚡ | Lightweight tasks |

> Full model list: [Groq Console](https://console.groq.com/docs/models)

---

## 💬 Usage

### Windows Forms
1. Launch the app
2. Click **⚙ Settings** → enter your Groq API key
3. Type a message and press **Enter** or click **Send**
4. Click **🗑 Clear** to wipe the current conversation
5. Click **➕ New Chat** to start fresh

### Console
```
--- Using qwen/qwen3-32b on Groq | Type '/exit' to quit | '/clear' to reset ---

You: Hello! What can you do?
AI: Hello! I can help with coding, writing, analysis, and more...

You: /clear
--- System: Memory wiped. ---

You: /exit
👋 Goodbye!
```

### Commands
| Command | Action |
|---|---|
| `/exit` | Quit the application |
| `/clear` | Wipe conversation memory |

---

## 📁 Project Structure

```
AiMemory/
├── AiMemoryManagment/              # Shared class library (.NET 8)
│   ├── Classes/
│   │   ├── Message.cs              # Message model for JSON serialization
│   │   └── ChatRequest.cs          # Chat request model
│   └── AiMemoryManagment.csproj
│
├── MemoryManagment/                # Console app (.NET 8)
│   ├── Program.cs                  # Console chat loop & API logic
│   └── MemoryManagment.csproj
│
├── AiMemoryWinForms/               # Windows Forms app (.NET 8)
│   ├── MainForm.cs                 # Full chat UI with dark theme
│   ├── Program.cs                  # Application entry point
│   └── AiMemoryWinForms.csproj
│
├── AiMemoryManagment.slnx          # Solution file
├── .gitignore                      # Excludes secrets, build artifacts, etc.
└── README.md                       # This file
```

### Architecture

```
┌─────────────────────┐     ┌─────────────────────┐
│   MemoryManagment   │     │   AiMemoryWinForms   │
│   (Console App)     │     │   (Windows Forms)    │
└────────┬────────────┘     └────────┬─────────────┘
         │                           │
         └───────────┬───────────────┘
                     │
         ┌───────────▼───────────────┐
         │   AiMemoryManagment       │
         │   (Shared Library)        │
         │   • Message.cs            │
         │   • ChatRequest.cs        │
         └───────────┬───────────────┘
                     │
         ┌───────────▼───────────────┐
         │      Groq API             │
         │   api.groq.com            │
         └───────────────────────────┘
```

---

## 🔐 Security Notes

- ✅ API key is read from environment variable (`GROQ_API_KEY`) — never hardcoded
- ✅ `chat_history.json` is stored in `%AppData%` (not in the repo)
- ✅ `.gitignore` excludes build artifacts, secrets, and local files
- ❌ **Never commit** your API key or real conversation logs
- 🔁 [Regenerate keys](https://console.groq.com/keys) if accidentally exposed

---

## 🛠️ Development

### Add a New Model
Just type the model ID when prompted — no code changes needed! The app supports any model available on Groq.

### Change Memory Save Location
**Console app** — edit `historyPath` in `MemoryManagment/Program.cs`:
```csharp
string historyPath = "chat_history.json";
```

**WinForms app** — history is saved to:
```
%AppData%\AiMemoryWinForms\chat_history.json
```

### Add Streaming Support
Groq supports streaming responses. Both apps can be extended to use `stream: true` in the request body. See [Groq API Docs](https://console.groq.com/docs/api-reference#chat-create).

### Build All Projects
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feat/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feat/amazing-feature`
5. Open a Pull Request

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

## 🙏 Acknowledgments

- [Groq](https://groq.com) — ultra-low-latency AI inference
- [.NET](https://dotnet.microsoft.com) — cross-platform development framework
- All open-source contributors
