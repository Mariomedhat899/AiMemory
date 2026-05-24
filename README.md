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
2. Set Your API Key (One-Time Setup)
Windows PowerShell (User-level, permanent):
[Environment]::SetEnvironmentVariable("GROQ_API_KEY", "gsk_YOUR_KEY_HERE", "User")

⚠️ Restart your terminal/IDE after setting.
Linux/macOS (Add to ~/.bashrc or ~/.zshrc):
export GROQ_API_KEY="gsk_YOUR_KEY_HERE"
3. Run the Bot
dotnet run
4. Choose a Model
When prompted, enter a model ID:
Model
Speed
Best For
llama-3.1-8b-instant
⚡⚡⚡
Fast chat, testing
qwen/qwen3-32b
⚡
Multilingual, coding
llama-3.3-70b-versatile
⚡
Complex reasoning
mixtral-8x7b-32768
⚡⚡
Long context (32K)

💬 Usage
--- Using qwen/qwen3-32b on Groq | Type '/exit' to quit | '/clear' to reset ---

You: Hello! What can you do?
AI: Hello! I can help with coding, writing, analysis, and more...

You: /clear
--- System: Memory wiped. ---

You: /exit
👋 Goodbye!

📁 Project Structure
groq-chat-memorybot/
├── Program.cs                 # Main chat loop & API logic
├── AiMemoryManagment/
│   └── Classes/
│       └── Message.cs         # Message model for JSON serialization
├── chat_history.json          # Auto-generated: persistent memory
├── .gitignore                 # Excludes secrets & build artifacts
├── README.md                  # This file
└── groq-chat-memorybot.csproj # .NET 8 project file

🔐 Security Notes
✅ API key is read from environment variable (GROQ_API_KEY)
✅ chat_history.json is gitignored by default
❌ Never commit your API key or real conversation logs
🔁 Regenerate keys if accidentally exposed
🛠️ Development
Add a New Model
Just type the model ID when prompted — no code changes needed!
Modify Memory Behavior
Edit historyPath in Program.cs to change save location.
Add Streaming Support
Groq supports streaming; see Groq API Docs for implementation.
🤝 Contributing
Fork the repo
Create a feature branch (git checkout -b feat/amazing-feature)
Commit changes (git commit -m 'Add amazing feature')
Push (git push origin feat/amazing-feature)
Open a Pull Request

---


