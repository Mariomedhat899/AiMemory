using AiMemoryManagment.Classes;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

string historyPath = "chat_history.json";
using HttpClient client = new HttpClient();

string GROQ_API_KEY = Environment.GetEnvironmentVariable("GROQ_API_KEY") ?? "";

if (string.IsNullOrWhiteSpace(GROQ_API_KEY))
{
    Console.WriteLine("❌ Error: GROQ_API_KEY environment variable is not set.");
    Console.WriteLine("💡 Set it with: $env:GROQ_API_KEY = 'your_key' in PowerShell");
    Console.WriteLine("🔗 Get a free key: https://console.groq.com/keys");
    Environment.Exit(1);
}

client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GROQ_API_KEY);

List<Message> history = new();

if (File.Exists(historyPath))
{
    try
    {
        string savedJson = File.ReadAllText(historyPath);
        history = JsonSerializer.Deserialize<List<Message>>(savedJson) ?? new();
        Console.WriteLine($"--- System: Loaded {history.Count} messages from memory ---");
    }
    catch
    {
        Console.WriteLine("--- System: Memory file was corrupted, starting fresh. ---");
    }
}

Console.Write("Enter model name (default: qwen/qwen3-32b): ");
string modelName = Console.ReadLine()!;
if (string.IsNullOrWhiteSpace(modelName)) modelName = "qwen/qwen3-32b";

Console.WriteLine($"\n--- Using {modelName} on Groq | Type '/exit' to quit | '/clear' to reset ---");

while (true)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("\nYou: ");
    string input = Console.ReadLine()!;
    Console.ResetColor();

    if (string.IsNullOrWhiteSpace(input)) continue;
    if (input.ToLower() == "/exit") break;

    if (input.ToLower() == "/clear")
    {
        history.Clear();
        File.Delete(historyPath);
        Console.WriteLine("--- System: Memory wiped. ---");
        continue;
    }

    history.Add(new Message { role = "user", content = input });

    try
    {
        var requestBody = new
        {
            model = modelName,
            messages = history,
            max_tokens = 500,
            temperature = 0.7
        };

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(requestBody, options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);

        if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        {
            Console.WriteLine($"\n⚠️ Rate limit exceeded! Please wait ~60 seconds.");
            Console.WriteLine($"💡 Groq free tier: ~30 requests/minute");
            history.RemoveAt(history.Count - 1);
            await Task.Delay(60000);
            continue;
        }

        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(responseJson);
        var root = doc.RootElement;

        string aiText = root.GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()!;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nAI: {aiText}");
        Console.ResetColor();

        history.Add(new Message { role = "assistant", content = aiText });

        var saveOptions = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(historyPath, JsonSerializer.Serialize(history, saveOptions));

        await Task.Delay(1000);

    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"\n[Error]: Groq API request failed.");
        Console.WriteLine($"Status: {ex.StatusCode}");
        Console.WriteLine($"Details: {ex.Message}");

        if (history.Count > 0 && history[^1].role == "user")
            history.RemoveAt(history.Count - 1);
    }
    catch (JsonException ex)
    {
        Console.WriteLine($"\n[Error]: Failed to parse API response.");
        Console.WriteLine($"Details: {ex.Message}");

        if (history.Count > 0 && history[^1].role == "user")
            history.RemoveAt(history.Count - 1);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n[Error]: {ex.Message}");

        if (history.Count > 0 && history[^1].role == "user")
            history.RemoveAt(history.Count - 1);
    }
}