using AiMemoryManagment.Classes;
using System.Text;
using System.Text.Json;

string historyPath = "chat_history.json";
using HttpClient client = new HttpClient();

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

Console.Write("Enter model name (default: hellhorns): ");
string modelName = Console.ReadLine()!;
if (string.IsNullOrWhiteSpace(modelName)) modelName = "hellhorns";

Console.WriteLine($"\n--- Using {modelName} | Type '/exit' to quit | '/clear' to reset ---");

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
        var requestBody = new ChatRequest(modelName, history);

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(requestBody, options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("http://localhost:11434/api/chat", content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(responseJson);
        var root = doc.RootElement;

        string aiText = root.GetProperty("message").GetProperty("content").GetString()!;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nAI: {aiText}");
        Console.ResetColor();

        history.Add(new Message { role = "assistant", content = aiText });

        var saveOptions = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(historyPath, JsonSerializer.Serialize(history, saveOptions));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n[Error]: Connection failed. Ensure Ollama is running.");
        Console.WriteLine($"Details: {ex.Message}");
    }
}