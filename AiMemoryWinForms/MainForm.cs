namespace AiMemoryWinForms;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using AiMemoryManagment.Classes;

public partial class MainForm : Form
{
    private readonly HttpClient _httpClient = new();
    private readonly List<Message> _history = new();
    private readonly string _historyPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "AiMemoryWinForms", "chat_history.json");

    private const string DefaultModel = "qwen/qwen3-32b";
    private string _modelName = DefaultModel;
    private string _apiKey = "";
    private bool _isWaitingForResponse = false;

    // UI Controls
    private Panel headerPanel = null!;
    private Label titleLabel = null!;
    private Label statusLabel = null!;
    private FlowLayoutPanel chatPanel = null!;
    private Panel inputPanel = null!;
    private TextBox messageInput = null!;
    private Button sendButton = null!;
    private Button settingsButton = null!;
    private Button clearButton = null!;
    private Panel sidebarPanel = null!;
    private Label historyTitleLabel = null!;
    private ListBox conversationList = null!;
    private Button newChatButton = null!;

    // Colors
    private static readonly Color DarkBg = Color.FromArgb(18, 18, 24);
    private static readonly Color SidebarBg = Color.FromArgb(12, 12, 18);
    private static readonly Color HeaderBg = Color.FromArgb(22, 22, 32);
    private static readonly Color InputBg = Color.FromArgb(28, 28, 40);
    private static readonly Color UserBubble = Color.FromArgb(45, 85, 135);
    private static readonly Color AiBubble = Color.FromArgb(35, 35, 50);
    private static readonly Color AccentColor = Color.FromArgb(88, 166, 255);
    private static readonly Color TextPrimary = Color.FromArgb(230, 230, 240);
    private static readonly Color TextSecondary = Color.FromArgb(140, 140, 165);
    private static readonly Color DangerColor = Color.FromArgb(220, 60, 60);

    public MainForm()
    {
        InitializeComponent();
        LoadSettings();
        LoadHistory();
        ApplyTheme();
    }

    private void InitializeComponent()
    {
        // Form
        Text = "AI Memory Chat";
        Size = new Size(1100, 720);
        MinimumSize = new Size(800, 550);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = DarkBg;
        Font = new Font("Segoe UI", 10F);
        Icon = SystemIcons.Application;

        // ===== SIDEBAR =====
        sidebarPanel = new Panel
        {
            Dock = DockStyle.Left,
            Width = 240,
            BackColor = SidebarBg,
            Padding = new Padding(0, 10, 0, 10)
        };

        newChatButton = CreateSidebarButton("➕  New Chat", AccentColor);
        newChatButton.Click += NewChatButton_Click;
        newChatButton.Dock = DockStyle.Top;
        newChatButton.Height = 44;
        newChatButton.Margin = new Padding(10, 0, 10, 10);

        historyTitleLabel = new Label
        {
            Text = "CONVERSATIONS",
            ForeColor = TextSecondary,
            Font = new Font("Segoe UI", 8F, FontStyle.Bold),
            Dock = DockStyle.Top,
            Height = 30,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(15, 0, 0, 0)
        };

        conversationList = new ListBox
        {
            Dock = DockStyle.Fill,
            BackColor = SidebarBg,
            ForeColor = TextPrimary,
            BorderStyle = BorderStyle.None,
            Font = new Font("Segoe UI", 9.5F),
            ItemHeight = 28,
            IntegralHeight = false
        };
        conversationList.DrawMode = DrawMode.OwnerDrawFixed;
        conversationList.DrawItem += ConversationList_DrawItem;

        sidebarPanel.Controls.Add(conversationList);
        sidebarPanel.Controls.Add(historyTitleLabel);
        sidebarPanel.Controls.Add(newChatButton);

        // ===== HEADER =====
        headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 56,
            BackColor = HeaderBg,
            Padding = new Padding(20, 0, 0, 0)
        };

        titleLabel = new Label
        {
            Text = "🧠 AI Memory Chat",
            Font = new Font("Segoe UI Semibold", 14F),
            ForeColor = TextPrimary,
            AutoSize = true,
            Location = new Point(20, 14)
        };

        statusLabel = new Label
        {
            Text = "● Ready",
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.FromArgb(80, 200, 120),
            AutoSize = true,
            Location = new Point(titleLabel.Right + 20, 18)
        };

        clearButton = CreateHeaderButton("🗑 Clear");
        clearButton.Location = new Point(headerPanel.Width - 180, 12);
        clearButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        clearButton.Click += ClearButton_Click;

        settingsButton = CreateHeaderButton("⚙ Settings");
        settingsButton.Location = new Point(headerPanel.Width - 100, 12);
        settingsButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        settingsButton.Click += SettingsButton_Click;

        headerPanel.Controls.AddRange(new Control[] { titleLabel, statusLabel, clearButton, settingsButton });

        // ===== CHAT AREA =====
        chatPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = DarkBg,
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(20, 15, 20, 15)
        };
        chatPanel.HorizontalScroll.Enabled = false;
        chatPanel.HorizontalScroll.Visible = false;
        chatPanel.AutoScroll = true;

        // ===== INPUT AREA =====
        inputPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 72,
            BackColor = InputBg,
            Padding = new Padding(15, 12, 15, 12)
        };

        messageInput = new TextBox
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(38, 38, 55),
            ForeColor = TextPrimary,
            Font = new Font("Segoe UI", 11F),
            BorderStyle = BorderStyle.None,
            Multiline = true,
            PlaceholderText = "Type your message... (Enter to send)"
        };
        messageInput.KeyDown += MessageInput_KeyDown;

        sendButton = new Button
        {
            Text = "Send",
            Dock = DockStyle.Right,
            Width = 80,
            FlatStyle = FlatStyle.Flat,
            BackColor = AccentColor,
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 10F),
            Cursor = Cursors.Hand,
            Margin = new Padding(10, 0, 0, 0)
        };
        sendButton.FlatAppearance.BorderSize = 0;
        sendButton.Click += SendButton_Click;

        inputPanel.Controls.Add(messageInput);
        inputPanel.Controls.Add(sendButton);

        // Add controls to form
        Controls.Add(chatPanel);
        Controls.Add(inputPanel);
        Controls.Add(headerPanel);
        Controls.Add(sidebarPanel);

        // Handle resize for input panel
        messageInput.TextChanged += (s, e) =>
        {
            messageInput.Height = Math.Min(48, TextRenderer.MeasureText(
                messageInput.Text, messageInput.Font, new Size(messageInput.Width, int.MaxValue),
                TextFormatFlags.WordBreak).Height + 20);
            inputPanel.Height = messageInput.Height + 24;
        };
        _isInitialized = true;
    }

    // ===== SIDEBAR BUTTON =====
    private static Button CreateSidebarButton(string text, Color accent)
    {
        var btn = new Button
        {
            Text = text,
            FlatStyle = FlatStyle.Flat,
            BackColor = accent,
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 10F),
            Cursor = Cursors.Hand,
            TextAlign = ContentAlignment.MiddleCenter
        };
        btn.FlatAppearance.BorderSize = 0;
        return btn;
    }

    // ===== HEADER BUTTONS =====
    private Button CreateHeaderButton(string text)
    {
        var btn = new Button
        {
            Text = text,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(40, 40, 58),
            ForeColor = TextSecondary,
            Font = new Font("Segoe UI", 9F),
            Cursor = Cursors.Hand,
            Size = new Size(80, 32),
            TextAlign = ContentAlignment.MiddleCenter
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(55, 55, 75);
        btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(40, 40, 58);
        return btn;
    }

    // ===== CHAT BUBBLE =====
    private Panel CreateChatBubble(string text, bool isUser)
    {
        var bubble = new Panel
        {
            AutoSize = true,
            MaximumSize = new Size(chatPanel.Width - 80, 0),
            MinimumSize = new Size(60, 0),
            BackColor = isUser ? UserBubble : AiBubble,
            Padding = new Padding(14, 10, 14, 10),
            Margin = new Padding(0, 4, 0, 4)
        };

        // Rounded corners via region
        bubble.Paint += (s, e) =>
        {
            using var path = CreateRoundedRect(bubble.ClientRectangle, 14);
            bubble.Region = new Region(path);
            using var brush = new SolidBrush(isUser ? UserBubble : AiBubble);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillPath(brush, path);
        };

        var label = new Label
        {
            Text = text,
            ForeColor = TextPrimary,
            Font = new Font("Segoe UI", 10.5F),
            AutoSize = true,
            MaximumSize = new Size(bubble.MaximumSize.Width - 28, 0),
            Padding = new Padding(0),
            Margin = new Padding(0)
        };

        bubble.Controls.Add(label);
        bubble.Height = label.Height + 20;

        // Align right for user, left for AI
        bubble.Anchor = isUser ? AnchorStyles.Right : AnchorStyles.Left;

        return bubble;
    }

    private static System.Drawing.Drawing2D.GraphicsPath CreateRoundedRect(Rectangle rect, int radius)
    {
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        int d = radius * 2;
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    // ===== TYPING INDICATOR =====
    private Panel CreateTypingIndicator()
    {
        var panel = new Panel
        {
            AutoSize = true,
            BackColor = AiBubble,
            Padding = new Padding(14, 10, 14, 10),
            Margin = new Padding(0, 4, 0, 4),
            Anchor = AnchorStyles.Left
        };

        var label = new Label
        {
            Text = "●●●",
            ForeColor = TextSecondary,
            Font = new Font("Segoe UI", 14F),
            AutoSize = true
        };

        panel.Controls.Add(label);
        panel.Height = label.Height + 20;

        // Animate dots
        var timer = new System.Windows.Forms.Timer { Interval = 400 };
        int frame = 0;
        string[] frames = { "●○○", "●●○", "●●●", "●●○" };
        timer.Tick += (s, e) =>
        {
            frame = (frame + 1) % frames.Length;
            label.Text = frames[frame];
        };
        timer.Start();
        panel.Tag = timer;

        panel.Paint += (s, e) =>
        {
            using var path = CreateRoundedRect(panel.ClientRectangle, 14);
            panel.Region = new Region(path);
            using var brush = new SolidBrush(AiBubble);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillPath(brush, path);
        };

        return panel;
    }

    // ===== SEND MESSAGE =====
    private async void SendButton_Click(object? sender, EventArgs e)
    {
        await SendMessage();
    }

    private async void MessageInput_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter && !e.Shift)
        {
            e.SuppressKeyPress = true;
            await SendMessage();
        }
    }

    private async Task SendMessage()
    {
        string input = messageInput.Text.Trim();
        if (string.IsNullOrEmpty(input) || _isWaitingForResponse) return;

        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            MessageBox.Show("Please set your Groq API key in Settings first.",
                "API Key Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ShowSettings();
            return;
        }

        _isWaitingForResponse = true;
        messageInput.Text = "";
        messageInput.Enabled = false;
        sendButton.Enabled = false;
        statusLabel.Text = "● Thinking...";
        statusLabel.ForeColor = Color.FromArgb(255, 180, 50);

        // Add user bubble
        var userBubble = CreateChatBubble(input, true);
        chatPanel.Controls.Add(userBubble);
        chatPanel.ScrollControlIntoView(userBubble);

        _history.Add(new Message { role = "user", content = input });

        // Add typing indicator
        var typing = CreateTypingIndicator();
        chatPanel.Controls.Add(typing);
        chatPanel.ScrollControlIntoView(typing);

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            var requestBody = new
            {
                model = _modelName,
                messages = _history,
                max_tokens = 1000,
                temperature = 0.7
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(requestBody, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);

            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                chatPanel.Controls.Remove(typing);
                if (typing.Tag is System.Windows.Forms.Timer t) t.Stop();

                var errorBubble = CreateChatBubble("⚠️ Rate limit exceeded! Please wait ~60 seconds and try again.", false);
                chatPanel.Controls.Add(errorBubble);
                _history.RemoveAt(_history.Count - 1);
                return;
            }

            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);
            var root = doc.RootElement;

            string aiText = root.GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No response received.";

            // Remove typing indicator
            chatPanel.Controls.Remove(typing);
            if (typing.Tag is System.Windows.Forms.Timer timer) timer.Stop();

            // Add AI bubble
            var aiBubble = CreateChatBubble(aiText, false);
            chatPanel.Controls.Add(aiBubble);
            chatPanel.ScrollControlIntoView(aiBubble);

            _history.Add(new Message { role = "assistant", content = aiText });
            SaveHistory();
        }
        catch (HttpRequestException ex)
        {
            chatPanel.Controls.Remove(typing);
            if (typing.Tag is System.Windows.Forms.Timer timer) timer.Stop();

            var errorBubble = CreateChatBubble($"❌ API Error: {ex.Message}", false);
            chatPanel.Controls.Add(errorBubble);

            if (_history.Count > 0 && _history[^1].role == "user")
                _history.RemoveAt(_history.Count - 1);
        }
        catch (Exception ex)
        {
            chatPanel.Controls.Remove(typing);
            if (typing.Tag is System.Windows.Forms.Timer timer) timer.Stop();

            var errorBubble = CreateChatBubble($"❌ Error: {ex.Message}", false);
            chatPanel.Controls.Add(errorBubble);

            if (_history.Count > 0 && _history[^1].role == "user")
                _history.RemoveAt(_history.Count - 1);
        }
        finally
        {
            _isWaitingForResponse = false;
            messageInput.Enabled = true;
            sendButton.Enabled = true;
            statusLabel.Text = "● Ready";
            statusLabel.ForeColor = Color.FromArgb(80, 200, 120);
            messageInput.Focus();
        }
    }

    // ===== CLEAR CHAT =====
    private void ClearButton_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("Clear all chat history?", "Confirm",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            _history.Clear();
            chatPanel.Controls.Clear();
            SaveHistory();
        }
    }

    // ===== NEW CHAT =====
    private void NewChatButton_Click(object? sender, EventArgs e)
    {
        _history.Clear();
        chatPanel.Controls.Clear();
        SaveHistory();
        messageInput.Focus();
    }

    // ===== SETTINGS =====
    private void SettingsButton_Click(object? sender, EventArgs e)
    {
        ShowSettings();
    }

    private void ShowSettings()
    {
        using var form = new Form
        {
            Text = "Settings",
            Size = new Size(480, 320),
            StartPosition = FormStartPosition.CenterParent,
            BackColor = DarkBg,
            ForeColor = TextPrimary,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false,
            Font = new Font("Segoe UI", 10F)
        };

        int y = 25;

        var lblKey = new Label { Text = "Groq API Key:", Location = new Point(25, y), AutoSize = true, ForeColor = TextPrimary };
        var txtKey = new TextBox
        {
            Text = _apiKey,
            Location = new Point(25, y + 28),
            Size = new Size(420, 30),
            BackColor = InputBg,
            ForeColor = TextPrimary,
            BorderStyle = BorderStyle.None,
            UseSystemPasswordChar = true,
            Padding = new Padding(8, 4, 8, 4)
        };

        y += 75;
        var lblModel = new Label { Text = "Model:", Location = new Point(25, y), AutoSize = true, ForeColor = TextPrimary };
        var txtModel = new TextBox
        {
            Text = _modelName,
            Location = new Point(25, y + 28),
            Size = new Size(420, 30),
            BackColor = InputBg,
            ForeColor = TextPrimary,
            BorderStyle = BorderStyle.None,
            Padding = new Padding(8, 4, 8, 4)
        };

        y += 75;
        var lblHint = new Label
        {
            Text = "💡 Get a free API key at: https://console.groq.com/keys",
            Location = new Point(25, y),
            AutoSize = true,
            ForeColor = TextSecondary,
            Font = new Font("Segoe UI", 9F)
        };

        var btnSave = new Button
        {
            Text = "Save",
            Location = new Point(355, y + 40),
            Size = new Size(90, 36),
            FlatStyle = FlatStyle.Flat,
            BackColor = AccentColor,
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 10F),
            Cursor = Cursors.Hand
        };
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.Click += (s, e) =>
        {
            _apiKey = txtKey.Text.Trim();
            _modelName = txtModel.Text.Trim();
            SaveSettings();
            form.DialogResult = DialogResult.OK;
            form.Close();
        };

        form.Controls.AddRange(new Control[] { lblKey, txtKey, lblModel, txtModel, lblHint, btnSave });
        form.ShowDialog(this);
    }

    // ===== PERSISTENCE =====
    private void LoadSettings()
    {
        string settingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AiMemoryWinForms", "settings.json");

        if (File.Exists(settingsPath))
        {
            try
            {
                var json = File.ReadAllText(settingsPath);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                _apiKey = root.GetProperty("ApiKey").GetString() ?? "";
                _modelName = root.GetProperty("ModelName").GetString() ?? DefaultModel;
            }
            catch { /* use defaults */ }
        }
    }

    private void SaveSettings()
    {
        string dir = Path.GetDirectoryName(_historyPath)!;
        Directory.CreateDirectory(dir);

        var settings = new { ApiKey = _apiKey, ModelName = _modelName };
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        string settingsPath = Path.Combine(dir, "settings.json");
        File.WriteAllText(settingsPath, json);
    }

    private void LoadHistory()
    {
        if (File.Exists(_historyPath))
        {
            try
            {
                var json = File.ReadAllText(_historyPath);
                var loaded = JsonSerializer.Deserialize<List<Message>>(json);
                if (loaded != null)
                {
                    _history.Clear();
                    _history.AddRange(loaded);

                    // Rebuild chat bubbles
                    foreach (var msg in _history)
                    {
                        var bubble = CreateChatBubble(msg.content, msg.role == "user");
                        chatPanel.Controls.Add(bubble);
                    }
                }
            }
            catch { /* start fresh */ }
        }
    }

    private void SaveHistory()
    {
        string dir = Path.GetDirectoryName(_historyPath)!;
        Directory.CreateDirectory(dir);
        var json = JsonSerializer.Serialize(_history, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_historyPath, json);
    }

    // ===== THEME =====
    private void ApplyTheme()
    {
        // Already applied via control properties
    }

    // ===== CONVERSATION LIST DRAWING =====
    private void ConversationList_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0) return;

        e.DrawBackground();
        var bounds = e.Bounds;
        bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

        using var bgBrush = new SolidBrush(selected ? Color.FromArgb(50, 50, 75) : SidebarBg);
        e.Graphics.FillRectangle(bgBrush, bounds);

        using var textBrush = new SolidBrush(selected ? TextPrimary : TextSecondary);
        e.Graphics.DrawString(conversationList.Items[e.Index].ToString(),
            e.Font ?? Font, textBrush, bounds.X + 15, bounds.Y + 4);

        e.DrawFocusRectangle();
    }

    // ===== RESIZE HANDLER =====
    private bool _isInitialized = false;

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (!_isInitialized) return;
        foreach (Control ctrl in chatPanel.Controls)
        {
            if (ctrl is Panel bubble)
            {
                bubble.MaximumSize = new Size(chatPanel.Width - 80, 0);
                foreach (Control inner in bubble.Controls)
                {
                    if (inner is Label lbl)
                    {
                        lbl.MaximumSize = new Size(bubble.MaximumSize.Width - 28, 0);
                    }
                }
            }
        }
    }
}
