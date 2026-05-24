namespace AiMemory.WinForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtContent = new TextBox();
            btnAddMessage = new Button();
            lstMessages = new ListBox();
            SuspendLayout();
            // 
            // txtContent
            // 
            txtContent.Location = new Point(20, 20);
            txtContent.Multiline = true;
            txtContent.Name = "txtContent";
            txtContent.ScrollBars = ScrollBars.Vertical;
            txtContent.Size = new Size(400, 150);
            txtContent.TabIndex = 0;
            // 
            // btnAddMessage
            // 
            btnAddMessage.Location = new Point(252, 208);
            btnAddMessage.Name = "btnAddMessage";
            btnAddMessage.Size = new Size(120, 30);
            btnAddMessage.TabIndex = 2;
            btnAddMessage.Text = "Add Message";
            btnAddMessage.UseVisualStyleBackColor = true;
            btnAddMessage.Click += btnAddMessage_Click_1;
            // 
            // lstMessages
            // 
            lstMessages.FormattingEnabled = true;
            lstMessages.ItemHeight = 21;
            lstMessages.Location = new Point(1, 287);
            lstMessages.Name = "lstMessages";
            lstMessages.Size = new Size(813, 151);
            lstMessages.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lstMessages);
            Controls.Add(btnAddMessage);
            Controls.Add(txtContent);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtContent;
        private Button btnAddMessage;
        private ListBox lstMessages;
    }
}
