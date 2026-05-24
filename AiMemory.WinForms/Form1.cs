namespace AiMemory.WinForms

{
    public partial class Form1 : Form
    {

        private List<AiMemoryManagment.Classes.Message> messages = new();


        private void btnAddMessage_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtContent.Text))
            {
                MessageBox.Show("Please Enter Some Content!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            var msg = new AiMemoryManagment.Classes.Message
            {
                content = txtContent.Text,
                role = "user"

            };

            messages.Add(msg);
            File.AppendAllText("Memory.txt", msg.content + Environment.NewLine);
            lstMessages.Items.Add(msg.content);
            txtContent.Clear();
            txtContent.Focus();
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }


    }
}
