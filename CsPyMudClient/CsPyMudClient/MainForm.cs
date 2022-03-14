using System;
using System.Drawing;
using System.Windows.Forms;

namespace CsPyMudClient
{
    public class MainForm : Form
    {
        // GUI stuffs
        private TableLayoutPanel topLayout;
        private RichTextBox outputBox;
        private TableLayoutPanel inputLayout;
        private TextBox inputTextBox;
        private Button sendButton;

        // connection to server
        private ServerConnection connection;

        public MainForm()
        {
            BuildComponents();
        }

        private void BuildComponents()
        {
            topLayout = new TableLayoutPanel();
            topLayout.Dock = DockStyle.Fill;
            topLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            topLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));

            this.Controls.Add(topLayout);

            outputBox = new RichTextBox();
            outputBox.Dock = DockStyle.Fill;
            outputBox.ReadOnly = true;
            outputBox.BackColor = Color.White;
            outputBox.Multiline = true;
            outputBox.TabStop = false;
            topLayout.Controls.Add(outputBox, 0, 0);

            inputLayout = new TableLayoutPanel();
            inputLayout.Dock = DockStyle.Fill;
            inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 64));
            topLayout.Controls.Add(inputLayout, 0, 1);

            inputTextBox = new TextBox();
            inputTextBox.Dock = DockStyle.Fill;
            inputTextBox.KeyDown += (sender, e) => CheckForEnter(e.KeyCode);
            inputLayout.Controls.Add(inputTextBox, 0, 0);

            sendButton = new Button();
            sendButton.Dock = DockStyle.Fill;
            sendButton.Text = "Send";
            sendButton.Click += (sender, e) => SendCommand();
            inputLayout.Controls.Add(sendButton, 1, 0);

            this.Shown += (sender, e) => OpenConnection();
            this.Closed +=(sender, e) => CloseConnection();
        }

        private void OpenConnection()
        {
            string serverName = "localhost";
            int port = 8000;
            connection = new ServerConnection(serverName, port);
            connection.MessageHandler = (msg) => this.ReceiveMessage(msg);
            if(connection.Open())
            {
                InternalMessage(string.Format("Connected to {0}:{1}", serverName, port));
            }
            else
            {
                InternalMessage(string.Format("Connection to {0}:{1} failed.", serverName, port));
            }
        }

        private void CloseConnection()
        {
            connection.Close();
        }

        //=====================================================================
        // Controller methods
        //=====================================================================
        private void CheckForEnter(Keys keyCode)
        {
            if(keyCode == Keys.Enter)
            {
                SendCommand();
            }
        }

        private void SendCommand()
        {
            // collect input and reset input text box
            string command = inputTextBox.Text;
            inputTextBox.Text = "";

            // send command through connection


            // echo command to output
            outputBox.SelectionFont = new Font("Courier New", 12, FontStyle.Regular);
            outputBox.SelectionColor = Color.DarkGray;
            outputBox.AppendText("> "+command+"\n");
        }

        public int ReceiveMessage(string text)
        {
            string[] lines = text.Split('\n');
            outputBox.SelectionFont = new Font("Courier New", 12, FontStyle.Bold);
            outputBox.SelectionColor = Color.Black;
            foreach (string line in lines)
            {
                outputBox.AppendText(line + "\n");
            }
            return 0;
        }

        private void InternalMessage(string text)
        {
            outputBox.SelectionFont = new Font("Courier New", 12, FontStyle.Underline);
            outputBox.SelectionColor = Color.Red;
            outputBox.AppendText(text + "\n");
        }

        //=====================================================================
        // The main method
        //=====================================================================
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
