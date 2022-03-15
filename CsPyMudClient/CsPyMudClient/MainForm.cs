using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CsPyMudClient
{
    public class MainForm : Form
    {
        // text style stuff
        private enum MessageStyle
        {
            INTERNAL,
            REMOTE,
            LOCAL
        }

        class MessageStyleDef
        {
            public MessageStyleDef(string fontName, FontStyle fontStyle, Color _color)
            {
                font = new Font(fontName, 12, fontStyle);
                color = _color;
            }
            public Font font;
            public Color color;
        }

        private Dictionary<MessageStyle, MessageStyleDef> messageStyles;

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
            InitStyles();
            BuildComponents();
        }

        /// <summary>
        /// Set up the styles dictionary for later use
        /// </summary>
        private void InitStyles()
        {
            messageStyles = new Dictionary<MessageStyle, MessageStyleDef>();
            messageStyles.Add(MessageStyle.INTERNAL, new MessageStyleDef("Courier New", FontStyle.Bold, Color.Red));
            messageStyles.Add(MessageStyle.REMOTE, new MessageStyleDef("Courier New", FontStyle.Regular, Color.Black));
            messageStyles.Add(MessageStyle.LOCAL, new MessageStyleDef("Courier New", FontStyle.Italic, Color.DarkGray));
        }

        /// <summary>
        /// Set up the controls in the main form
        /// </summary>
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

        /// <summary>
        /// Callback to if check key press is an ENTER 
        /// </summary>
        /// <param name="keyCode">Key code.</param>
        private void CheckForEnter(Keys keyCode)
        {
            if(keyCode == Keys.Enter)
            {
                SendCommand();
            }
        }

        // use a delegate to execute AppendText later from main thread
        private delegate void MsgDelegate();

        /// <summary>
        /// Send the command currently in the inputTextBox
        /// </summary>
        private void SendCommand()
        {
            // collect input and reset input text box
            string command = inputTextBox.Text;
            inputTextBox.Text = "";

            // send command through connection
            connection.SendMessage(command);

            string message = "> " + command + Environment.NewLine;
            Invoke(new MsgDelegate(() => AppendText(MessageStyle.LOCAL, message)));
        }

        /// <summary>
        /// Add the received message to the output display
        /// </summary>
        /// <param name="text">Text.</param>
        public void ReceiveMessage(string text)
        {
            string message = text + Environment.NewLine;
            Invoke(new MsgDelegate(() => AppendText(MessageStyle.REMOTE, message)));
        }

        /// <summary>
        /// Display the message from the client's internal gubbins
        /// </summary>
        /// <param name="text">Text.</param>
        private void InternalMessage(string text)
        {
            string message = text + Environment.NewLine;
            Invoke(new MsgDelegate(() => AppendText(MessageStyle.INTERNAL, message)));
        }

        /// <summary>
        /// This is meant to be used in a delegate so that the main thread can
        /// update the output display in a thread-safe manner
        /// </summary>
        /// <param name="style">Style.</param>
        /// <param name="message">Message.</param>
        private void AppendText(MessageStyle style, string message)
        {
            outputBox.SelectionFont = messageStyles[style].font;
            outputBox.SelectionColor = messageStyles[style].color;
            outputBox.AppendText(message);
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
