using System;
using System.Windows.Forms;
using System.Globalization;

/* 
 * 
 * 
 * Special thanks to Ray Bear for pounding out the original program
 * for keys.
 * 
 */

namespace Keypad
{
    public partial class Keyboard : GenericKeypad
    {
        public Keyboard()
        {
            InitializeComponent();
        }

        private void Keyboard_Load(object sender, EventArgs e)
        {
            changeCase();

            switch (CultureInfo.CurrentCulture.Name)
            {
                case "fr": // ------------------------------------------French

                    break;

                case "de": // -------------------------------------------Deutsch

                    b1.Visible = false;
                    b2.Visible = false;
                    b3.Visible = false;
                    b4.Visible = false;
                    b5.Visible = false;
                    b6.Visible = false;
                    b7.Visible = false;
                    b8.Visible = false;
                    b9.Visible = false;
                    b10.Visible = false;
                    b11.Visible = false;
                    b12.Visible = false;

                    this.Height = 396;
                    break;

                default:
                    b1.Visible = false;
                    b2.Visible = false;
                    b3.Visible = false;
                    b4.Visible = false;
                    b5.Visible = false;
                    b6.Visible = false;
                    b7.Visible = false;
                    b8.Visible = false;
                    b9.Visible = false;
                    b10.Visible = false;
                    b11.Visible = false;
                    b12.Visible = false;

                    c12.Visible = false;

                    d12.Visible = false;

                    this.Height = 396;
                    break;
            }
        }

        private void Chk_shift_CheckedChanged(object sender, EventArgs e)
        {
            changeCase();
        }

        private void Chk_capslock_CheckedChanged(object sender, EventArgs e)
        {
            changeCase();
        }

        private void SendChar(Button senderb)
        {
            Button btn = (Button)senderb;
            RaiseButtonPressed(btn.Text[0]);
            //if (chk_shift.Checked) chk_shift.Checked = false;
        }

        private void BtnClick(object sender, EventArgs e)
        {
            SendChar((Button)sender);
        }

        private void changeCase()
        {

            switch (CultureInfo.CurrentCulture.Name)
            {
                case "fr": // ------------------------------------------French
                    if (chk_shift.Checked == true)
                    {
                        a1.Text = "!";
                        a2.Text = "@";
                        a3.Text = "#";
                        a4.Text = "$";
                        a5.Text = "%";
                        a6.Text = "^";
                        a7.Text = "&&";
                        a8.Text = "*";
                        a9.Text = "(";
                        a10.Text = ")";
                        a11.Text = "+";
                        e12.Text = "€";

                        b1.Text = "À";
                        b2.Text = "Â";
                        b3.Text = "Æ";
                        b4.Text = "Ç";
                        b5.Text = "É";
                        b6.Text = "È";
                        b7.Text = "Ê";
                        b8.Text = "Ë";
                        b9.Text = "Ï";
                        b10.Text = "Î";
                        b11.Text = "Ô";
                        b12.Text = "Œ";

                        c1.Text = "A";
                        c2.Text = "Z";
                        c3.Text = "E";
                        c4.Text = "R";
                        c5.Text = "T";
                        c6.Text = "Y";
                        c7.Text = "U";
                        c8.Text = "I";
                        c9.Text = "O";
                        c10.Text = "P";
                        c11.Text = "Ü";
                        c12.Text = "Û";


                        d1.Text = "Q";
                        d2.Text = "S";
                        d3.Text = "D";
                        d4.Text = "F";
                        d5.Text = "G";
                        d6.Text = "H";
                        d7.Text = "J";
                        d8.Text = "K";
                        d9.Text = "L";
                        d10.Text = "M";
                        d11.Text = "Ù";
                        d12.Text = "-";

                        e1.Text = ">";
                        e2.Text ="W";
                        e3.Text ="X" ;
                        e4.Text ="C" ;
                        e5.Text ="V" ;
                        e6.Text ="B" ;
                        e7.Text ="N" ;
                        e8.Text = "?";
                        e9.Text = ".";
                        e10.Text = "/";
                        e11.Text = "§";
                        e12.Text = "€";

                        btnApos.Text = "’";
                    }
                    else
                    {
                        a1.Text = "1";
                        a2.Text = "2";
                        a3.Text = "3";
                        a4.Text = "4";
                        a5.Text = "5";
                        a6.Text = "6";
                        a7.Text = "7";
                        a8.Text = "8";
                        a9.Text = "9";
                        a10.Text = "0";
                        a11.Text = "=";
                        e12.Text = "€";

                        b1.Text = "à";
                        b2.Text = "â";
                        b3.Text = "æ";
                        b4.Text = "ç";
                        b5.Text = "é";
                        b6.Text = "è";
                        b7.Text = "ê";
                        b8.Text = "ë";
                        b9.Text = "ï";
                        b10.Text = "î";
                        b11.Text = "ô";
                        b12.Text = "œ";


                        c1.Text = "a";
                        c2.Text = "z";
                        c3.Text = "e";
                        c4.Text = "r";
                        c5.Text = "t";
                        c6.Text = "y";
                        c7.Text = "u";
                        c8.Text = "i";
                        c9.Text = "o";
                        c10.Text = "p";
                        c11.Text = "ü";
                        c12.Text = "û";


                        d1.Text = "q";
                        d2.Text = "s";
                        d3.Text = "d";
                        d4.Text = "f";
                        d5.Text = "g";
                        d6.Text = "h";
                        d7.Text = "j";
                        d8.Text = "k";
                        d9.Text = "l";
                        d10.Text = "m";
                        d11.Text = "ù";
                        d12.Text = "_";

                        e1.Text = "<";
                        e2.Text = "w";
                        e3.Text = "x";
                        e4.Text = "c";
                        e5.Text = "v";
                        e6.Text = "b";
                        e7.Text = "n";
                        e8.Text = ",";
                        e9.Text = ";";
                        e10.Text = ":";
                        e11.Text = ".";
                        e12.Text = "|";

                        btnApos.Text = "’";
                    }

                    break;

                case "de": // -------------------------------------------Deutsch
                    if (chk_shift.Checked == true)
                    {
                        a1.Text = "!";
                        a2.Text = "@";
                        a3.Text = "#";
                        a4.Text = "$";
                        a5.Text = "%";
                        a6.Text = "^";
                        a7.Text = "&&";
                        a8.Text = "*";
                        a9.Text = "(";
                        a10.Text = ")";
                        a11.Text = "\u00DF";

                        c1.Text = "?";
                        c2.Text = "Q";
                        c3.Text = "W";
                        c4.Text = "E";
                        c5.Text = "R";
                        c6.Text = "T";
                        c7.Text = "Z";
                        c8.Text = "U";
                        c9.Text = "I";
                        c10.Text ="O";
                        c11.Text = "P";
                        c12.Text = "\u00DC";

                       
                        d1.Text = "-";
                        d2.Text = "A";
                        d3.Text = "S";
                        d4.Text = "D";
                        d5.Text = "F";
                        d6.Text = "G";
                        d7.Text = "H";
                        d8.Text = "J";
                        d9.Text = "K";
                        d10.Text ="L";
                        d11.Text = "\u00D6";
                        d12.Text = "\u00C4"; 

                        e1.Text = "~";
                        e2.Text = "+";
                        e3.Text = "Y";
                        e4.Text = "X";
                        e5.Text = "C";
                        e6.Text = "V";
                        e7.Text = "B";
                        e8.Text = "N";
                        e9.Text = "M";
                        e10.Text = "<";
                        e11.Text = ">";
                        e12.Text = ":";

                        btnApos.Text = "\u0060";
                    }
                    else
                    {
                        c1.Text = "/";
                        c2.Text = "q";
                        c3.Text = "w";
                        c4.Text = "e";
                        c5.Text = "r";
                        c6.Text = "t";
                        c7.Text = "z";
                        c8.Text = "u";
                        c9.Text = "i";
                        c10.Text ="o";
                        c11.Text = "p";
                        c12.Text = "\u00FC";


                        d1.Text = "_";
                        d2.Text = "a";
                        d3.Text = "s";
                        d4.Text = "d";
                        d5.Text = "f";
                        d6.Text = "g";
                        d7.Text = "h";
                        d8.Text = "j";
                        d9.Text = "k";
                        d10.Text ="l";
                        d11.Text = "\u00F6";
                        d12.Text = "\u00E4"; 

                        e1.Text = "|";
                        e2.Text = "=";
                        e3.Text = "y";
                        e4.Text = "x";
                        e5.Text = "c";
                        e6.Text = "v";
                        e7.Text = "b";
                        e8.Text = "n";
                        e9.Text = "m";
                        e10.Text = ",";
                        e11.Text = ".";
                        e12.Text = ";";

                        a1.Text = "1";
                        a2.Text = "2";
                        a3.Text = "3";
                        a4.Text = "4";
                        a5.Text = "5";
                        a6.Text = "6";
                        a7.Text = "7";
                        a8.Text = "8";
                        a9.Text = "9";
                        a10.Text = "0";
                        a11.Text = "\u00DF";

                        btnApos.Text = "\u00B4";
                    }

                    break;
                   

                default:

                    if (chk_shift.Checked == true)
                    {
                        a1.Text = "!";
                        a2.Text = "@";
                        a3.Text = "#";
                        a4.Text = "$";
                        a5.Text = "%";
                        a6.Text = "^";
                        a7.Text = "&&";
                        a8.Text = "*";
                        a9.Text = "(";
                        a10.Text = ")";
                        a11.Text = "+";

                        c1.Text = "?";
                        c2.Text = "Q";
                        c3.Text = "W";
                        c4.Text = "E";
                        c5.Text = "R";
                        c6.Text = "T";
                        c7.Text = "Y";
                        c8.Text = "U";
                        c9.Text = "I";
                        c10.Text ="O";
                        c11.Text = "P"; 
                        c12.Text = ".";

                        
                        d1.Text = "|";
                        d2.Text = "A";
                        d3.Text = "S";
                        d4.Text = "D";
                        d5.Text = "F";
                        d6.Text = "G";
                        d7.Text = "H";
                        d8.Text = "J";
                        d9.Text = "K";
                        d10.Text ="L"; 
                        d11.Text = "-";

                        e1.Text = "{";
                        e2.Text = "}";
                        e3.Text = "Z";
                        e4.Text = "X";
                        e5.Text = "C";
                        e6.Text = "V";
                        e7.Text = "B";
                        e8.Text = "N";
                        e9.Text = "M";
                        e10.Text = "<";
                        e11.Text = ">";
                        e12.Text = ":";

                        btnApos.Text = "\u0060";
                    }
                    else
                    {
                        a1.Text = "1";
                        a2.Text = "2";
                        a3.Text = "3";
                        a4.Text = "4";
                        a5.Text = "5";
                        a6.Text = "6";
                        a7.Text = "7";
                        a8.Text = "8";
                        a9.Text = "9";
                        a10.Text = "0";
                        a11.Text = "=";

                        c1.Text = "/";
                        c2.Text = "q";
                        c3.Text = "w";
                        c4.Text = "e";
                        c5.Text = "r";
                        c6.Text = "t";
                        c7.Text = "y";
                        c8.Text = "u";
                        c9.Text = "i";
                        c10.Text ="o";
                        c11.Text = "p";

                        
                        d1.Text = "~";
                        d2.Text = "a";
                        d3.Text = "s";
                        d4.Text = "d";
                        d5.Text = "f";
                        d6.Text = "g";
                        d7.Text = "h";
                        d8.Text = "j";
                        d9.Text = "k";
                        d10.Text ="l"; 
                        d11.Text = "_";

                        e1.Text = "[";
                        e2.Text = "]";
                        e3.Text = "z";
                        e4.Text = "x";
                        e5.Text = "c";
                        e6.Text = "v";
                        e7.Text = "b";
                        e8.Text = "n";
                        e9.Text = "m";
                        e10.Text =",";
                        e11.Text = ".";
                        e12.Text = ";";

                        btnApos.Text = "\u00B4";
                    }

                    break;
            }
        }

                      
        private void Btn_space_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed(' ');
        }

        private void Btn_backspace_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('\u0008');
        }

        private void Btn_clear_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('\u0005');
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('\u0027');
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('\u0004');
        }

    }
}
