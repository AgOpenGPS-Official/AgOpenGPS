using RateController;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace GPS_Out
{
    public class clsTools
    {
        private static Hashtable HTapp;
        private string cAppName = "GPS_Out";
        private string cAppVersion = "1.2.2";
        private string cPropertiesApp;
        private string cSettingsDir;
        private frmStart mf;

        public clsTools(frmStart CallingForm)
        {
            mf = CallingForm;
            CheckFolders();
            OpenFile(Properties.Settings.Default.FileName);
        }

        public string AppVersion()
        {
            return cAppVersion;
        }

        public byte CRC(byte[] Data, int Length, byte Start = 0)
        {
            byte Result = 0;
            if (Length <= Data.Length)
            {
                int CK = 0;
                for (int i = Start; i < Length; i++)
                {
                    CK += Data[i];
                }
                Result = (byte)CK;
            }
            return Result;
        }

        public void DrawGroupBox(GroupBox box, Graphics g, Color BackColor, Color textColor, Color borderColor)
        {
            // useage:
            // point the Groupbox paint event to this sub:
            //private void GroupBoxPaint(object sender, PaintEventArgs e)
            //{
            //    GroupBox box = sender as GroupBox;
            //    mf.Tls.DrawGroupBox(box, e.Graphics, this.BackColor, Color.Black, Color.Blue);
            //}

            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                // Clear text and border
                g.Clear(BackColor);

                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Drawing Border
                //Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                //Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)strSize.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }

        public bool GoodCRC(byte[] Data, byte Start = 0)
        {
            bool Result = false;
            int Length = Data.Length;
            byte cr = CRC(Data, Length - 1, Start);
            Result = cr == Data[Length - 1];
            return Result;
        }

        public bool IsOnScreen(Form form, bool PutOnScreen = false)
        {
            // Create rectangle
            Rectangle formRectangle = new Rectangle(form.Left, form.Top, form.Width, form.Height);

            // Test
            bool IsOn = Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(formRectangle));

            if (!IsOn & PutOnScreen)
            {
                form.Top = 0;
                form.Left = 0;
            }

            return IsOn;
        }

        public string LoadAppProperty(string Key)
        {
            string Prop = "";
            if (HTapp.Contains(Key)) Prop = HTapp[Key].ToString();
            return Prop;
        }

        public void LoadFormData(Form Frm)
        {
            int Leftloc = 0;
            int.TryParse(LoadAppProperty(Frm.Name + ".Left"), out Leftloc);
            Frm.Left = Leftloc;

            int Toploc = 0;
            int.TryParse(LoadAppProperty(Frm.Name + ".Top"), out Toploc);
            Frm.Top = Toploc;

            IsOnScreen(Frm, true);
        }

        public void OpenFile(string NewFile)
        {
            try
            {
                string PathName = Path.GetDirectoryName(NewFile); // only works if file name present
                string FileName = Path.GetFileName(NewFile);
                if (FileName == "") PathName = NewFile;     // no file name present, fix path name
                if (Directory.Exists(PathName)) Properties.Settings.Default.FilesDir = PathName; // set the new files dir

                cPropertiesApp = Properties.Settings.Default.FilesDir + "\\AppData.txt";
                if (!File.Exists(cPropertiesApp)) File.Create(cPropertiesApp).Dispose();
                LoadAppData(cPropertiesApp);
            }
            catch (Exception ex)
            {
                WriteErrorLog("Tools: OpenFile: " + ex.Message);
            }
        }

        public void SaveAppProperty(string Key, string Value)
        {
            bool Changed = false;
            if (HTapp.Contains(Key))
            {
                if (!HTapp[Key].ToString().Equals(Value))
                {
                    HTapp[Key] = Value;
                    Changed = true;
                }
            }
            else
            {
                HTapp.Add(Key, Value);
                Changed = true;
            }
            if (Changed) SaveAppProperties();
        }

        public void SaveFormData(Form Frm)
        {
            try
            {
                SaveAppProperty(Frm.Name + ".Left", Frm.Left.ToString());
                SaveAppProperty(Frm.Name + ".Top", Frm.Top.ToString());
            }
            catch (Exception)
            {
            }
        }

        public void ShowHelp(string Message, string Title = "Help",
            int timeInMsec = 30000, bool LogError = false, bool Modal = false, bool PlayErrorSound = false)
        {
            var Hlp = new frmHelp(mf, Message, Title, timeInMsec);
            if (Modal)
            {
                Hlp.ShowDialog();
            }
            else
            {
                Hlp.Show();
            }

            if (LogError) WriteErrorLog(Message);
            if (PlayErrorSound) SystemSounds.Exclamation.Play();
        }

        public void WriteErrorLog(string strErrorText)
        {
            try
            {
                string FileName = cSettingsDir + "\\Error Log.txt";
                TrimFile(FileName);
                File.AppendAllText(FileName, DateTime.Now.ToString("MMM-dd hh:mm:ss") + "  -  " + strErrorText + "\r\n\r\n");
            }
            catch (Exception)
            {
            }
        }

        private void CheckFolders()
        {
            try
            {
                // SettingsDir
                cSettingsDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + cAppName;

                if (!Directory.Exists(cSettingsDir)) Directory.CreateDirectory(cSettingsDir);

                string FilesDir = Properties.Settings.Default.FilesDir;
                if (!Directory.Exists(FilesDir)) Properties.Settings.Default.FilesDir = cSettingsDir;

                // erase old debug file
                string FileName = cSettingsDir + "\\" + "AGIOdata.txt";
                if (File.Exists(FileName)) File.Delete(FileName);
            }
            catch (Exception)
            {
            }
        }

        private void LoadAppData(string path)
        {
            // property:  key=value  ex: "LastFile=Main.mdb"
            try
            {
                HTapp = new Hashtable();
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    if (line.Contains("=") && !string.IsNullOrEmpty(line.Split('=')[0]) && !string.IsNullOrEmpty(line.Split('=')[1]))
                    {
                        string[] splitText = line.Split('=');
                        HTapp.Add(splitText[0], splitText[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog("Tools: LoadProperties: " + ex.Message);
            }
        }

        private void SaveAppProperties()
        {
            try
            {
                string[] NewLines = new string[HTapp.Count];
                int i = -1;
                foreach (DictionaryEntry Pair in HTapp)
                {
                    i++;
                    NewLines[i] = Pair.Key.ToString() + "=" + Pair.Value.ToString();
                }
                if (i > -1) File.WriteAllLines(cPropertiesApp, NewLines);
            }
            catch (Exception)
            {
            }
        }

        private void TrimFile(string FileName, int MaxSize = 100000)
        {
            try
            {
                if (File.Exists(FileName))
                {
                    long FileSize = new FileInfo(FileName).Length;
                    if (FileSize > MaxSize)
                    {
                        // trim file
                        string[] Lines = File.ReadAllLines(FileName);
                        int Len = Lines.Length;
                        int St = (int)(Len * .1); // skip first 10% of old lines
                        string[] NewLines = new string[Len - St];
                        Array.Copy(Lines, St, NewLines, 0, Len - St);
                        File.Delete(FileName);
                        File.AppendAllLines(FileName, NewLines);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
