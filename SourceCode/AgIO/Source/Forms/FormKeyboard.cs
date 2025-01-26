using Microsoft.Win32;
using System;
using System.Threading;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormKeyboard : Form
    {
        public string ReturnString { get; set; }

        public FormKeyboard(string currentString)
        {
            InitializeComponent();

            this.Text = "Enter Something";
            keyboardString.Text = currentString.ToString();
        }

        private void FormKeyboard_Load(object sender, EventArgs e)
        {
            keyboardString.SelectionStart = keyboardString.Text.Length;
            keyboardString.SelectionLength = 0;
            keyboard1.Focus();

            if (Thread.CurrentThread.CurrentCulture.Name == "fr")
            {
                this.Height = 575;
            }
            else
            {
                this.Height = 500;
            }
        }

        private void RegisterKeyboard1_ButtonPressed(object sender, KeyPressEventArgs e)
        {
            //if (isFirstKey)
            //{
            //    keyboardString.Text = "";
            //    isFirstKey = false;
            //}

            //clear the error as user entered new values
            if (keyboardString.Text == "Error")
            {
                keyboardString.Text = "";
            }

            //Backspace key, remove 1 char
            if (e.KeyChar == '\u0008')
            {
                if (keyboardString.Text.Length > 0)
                {
                    keyboardString.Text = keyboardString.Text.Remove(keyboardString.Text.Length - 1);
                }
            }

            //Exit or cancel
            else if (e.KeyChar == '\u0027')
            {
                this.DialogResult = DialogResult.Cancel;
                Close();
            }

            //clear whole display
            else if (e.KeyChar == '\u0005')
            {
                keyboardString.Text = "";
            }

            //ok button
            else if (e.KeyChar == '\u0004')
            {
                //all good, return the value
                this.ReturnString = keyboardString.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

            //if its a character just add it
            else
            {
                keyboardString.Text += e.KeyChar;
            }

            //Show the cursor
            keyboardString.SelectionStart = keyboardString.Text.Length;
            keyboardString.SelectionLength = 0;
            keyboardString.Focus();
        }
    }
}