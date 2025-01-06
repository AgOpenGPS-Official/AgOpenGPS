using AgOpenGPS.Culture;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormKeyboard : Form
    {
        public string ReturnString { get; set; }

        public FormKeyboard(string currentString)
        {
            InitializeComponent();

            this.Text = "Enter a Value";
            //fill in the display
            keyboardString.Text = currentString.ToString();
        }

        private void FormKeyboard_Load(object sender, EventArgs e)
        {
            keyboardString.SelectionStart = keyboardString.Text.Length;
            keyboardString.SelectionLength = 0;
            keyboard1.Focus();

            if (CultureInfo.CurrentUICulture.Name == "fr")
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
            //clear the error as user entered new values
            if (keyboardString.Text == gStr.gsError)
            {
                keyboardString.Text = "";
            }

            //Backspace key, remove 1 char
            if (e.KeyChar == '\u0008')
            {
                if (keyboardString.Text.Length > 0)
                {
                    var selectionIndex = keyboardString.SelectionStart;

                    if (selectionIndex > 0)
                    {
                        keyboardString.Text = keyboardString.Text.Remove(selectionIndex - 1, 1);

                        keyboardString.SelectionStart = selectionIndex - 1;
                        keyboardString.SelectionLength = 0;
                        keyboardString.Focus();
                    }
                    else
                    {
                        keyboardString.SelectionStart = 1;
                        keyboardString.SelectionLength = 0;
                    }
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
                var insertText = e.KeyChar.ToString();
                var selectionIndex = keyboardString.SelectionStart;
                keyboardString.Text = keyboardString.Text.Insert(selectionIndex, insertText);
                keyboardString.SelectionStart = selectionIndex + insertText.Length;
            }

            //Show the cursor
            keyboardString.SelectionLength = 0;
            keyboardString.Focus();
        }

        private void btnCharLeft_Click(object sender, EventArgs e)
        {
            int spot = keyboardString.SelectionStart;
            spot--;
            if (spot >= 0)
            {
                keyboardString.SelectionStart = spot;
                keyboardString.SelectionLength = 0;
                keyboardString.Focus();
            }
            else
            {
                spot = 0;
                keyboardString.SelectionStart = spot;
                keyboardString.SelectionLength = 0;
                keyboardString.Focus();
            }
        }

        private void btnCharRight_Click(object sender, EventArgs e)
        {
            int spot = keyboardString.SelectionStart;
            spot++;
            if (spot <= keyboardString.Text.Length)
            {
                keyboardString.SelectionStart = spot;
                keyboardString.SelectionLength = 0;
                keyboardString.Focus();
            }
            else
            {
                spot = keyboardString.Text.Length;
                keyboardString.SelectionStart = spot;
                keyboardString.SelectionLength = 0;
                keyboardString.Focus();
            }
        }
    }
}