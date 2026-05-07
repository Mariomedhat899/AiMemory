namespace AiMemoryManagment.Classes
{
    public class ChatRequest
    {

        public string model { get; set; }

        public List<Message> messages { get; set; }

        public bool stream { get; set; } = false;

        public ChatRequest(string modelName, List<Message> history)
        {
            model = modelName;
            messages = history;

        }
    }
}
