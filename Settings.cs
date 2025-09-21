using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Account_Manager
{
    public partial class Settings : Form
    {
        // fields
        int themeColorIndex;

        // properties
        AccountData AD
        {
            get
            {
                return Program.accountData;
            }
        }
        Configuration CF
        {
            get
            {
                return Program.config;
            }
        }
        
        // constructor
        public Settings()
        {
            InitializeComponent();

            Program.SetupAfk(this);
        }

        // events
        void Settings_Load(object sender, EventArgs e)
        {
            // center form window
            Location = Program.CenterForm(this);

            // update controls
            invalidLoginsCheckBox.Checked = CF.invalidLogins.Item1;
            invalidLoginsNumericUpDown.Value = CF.invalidLogins.Item2;

            afkTimerCheckBox.Checked = CF.afkTimer.Item1;
            afkTimerNumericUpDown.Value = CF.afkTimer.Item2;

            hidePassCheckBox.Checked = CF.hidePass;
            passOnMinCheckBox.Checked = CF.passOnMinimize;
            clearClipboardCheckBox.Checked = CF.clearClipboardOnExit;

            autoBackupCheckBox.Checked = CF.autoBackup.Item1;
            autoBackupNumericUpDown.Value = CF.autoBackup.Item2;

            invalidLoginsAlertCheckBox.Checked = CF.invalidLoginsAlert;

            emailTextBox.Text = AD.email;
            passKeyTextBox.Text = AD.passKey;

            if (AD.email != "" && AD.passKey != "")
            {
                emailLabel.Text = "Email: Connected";
                passKeyLabel.Text = "Pass Key: Connected";
            }

            themeColorIndex = CF.themeColorIndex;

            // set form colors and save original color
            Program.SetThemeColor(this);
        }
        void Settings_Resize(object sender, EventArgs e)
        {
            Program.PrivacyPanel(this, privacyPanel, WindowState == FormWindowState.Minimized);
        }

        //// general
        void helpButton_Click(object sender, EventArgs e)
        {
            // show help
            using (Help help = new Help())
            {
                help.ShowDialog();
            }
        }

        //// password
        void changePassButton_Click(object sender, EventArgs e)
        {
            // encrypt inputed current password
            string currMenuPass = AD.CreateHash(currentPassTextBox.Text);

            // compare current password with inputed current password
            if (currMenuPass == AD.menuPass)
            {
                if (createPassTextBox.Text == confirmPassTextBox.Text)
                {
                    if (confirmPassTextBox.Text.Length > 6)
                    {
                        AD.menuPass = AD.CreateHash(confirmPassTextBox.Text);
                        MessageBox.Show("Password change success!", "Settings");

                        currentPassTextBox.Text = "";
                        createPassTextBox.Text = "";
                        confirmPassTextBox.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Password must be at least 7 characters.", "Settings");
                        createPassTextBox.Text = "";
                        confirmPassTextBox.Text = "";
                        createPassTextBox.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Created and confirmed passwords do not match.", "Settings");
                    createPassTextBox.Text = "";
                    confirmPassTextBox.Text = "";
                    createPassTextBox.Focus();
                }
            }
            else
            {
                MessageBox.Show("Current password is incorrect.", "Settings");
                currentPassTextBox.Text = "";
                currentPassTextBox.Focus();
            }
        }

        //// email
        void setupEmailButton_Click(object sender, EventArgs e)
        {
            Program.ConfigureEmail(emailTextBox.Text, passKeyTextBox.Text, emailLabel, passKeyLabel);
        }
        void sendBackupButton_Click(object sender, EventArgs e)
        {
            Program.EmailBackup();
        }

        //// color
        void ChangeThemeColor(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            Match match = Regex.Match(panel.Name, @"\d+$");
            int index = 0;

            if (match.Success)
            {
                index = int.Parse(match.Value);
            }

            ChangeThemeColor(index);

            Program.SetThemeColor(this);

            Program.ResetAfkTimer();
        }

        //// buttons
        void doneButton_Click(object sender, EventArgs e)
        {
            bool save = false;
            if (CF.invalidLogins.Item1 != invalidLoginsCheckBox.Checked || CF.invalidLogins.Item2 != invalidLoginsNumericUpDown.Value)
            {
                CF.invalidLogins = new Tuple<bool, int>(invalidLoginsCheckBox.Checked, (int)invalidLoginsNumericUpDown.Value);
                save = true;
            }

            if (CF.afkTimer.Item1 != afkTimerCheckBox.Checked || CF.afkTimer.Item2 != afkTimerNumericUpDown.Value)
            {
                CF.afkTimer = new Tuple<bool, int>(afkTimerCheckBox.Checked, (int)afkTimerNumericUpDown.Value);

                save = true;
            }

            if (CF.hidePass != hidePassCheckBox.Checked)
            {
                CF.hidePass = hidePassCheckBox.Checked;

                save = true;
            }

            if (CF.passOnMinimize != passOnMinCheckBox.Checked)
            {
                CF.passOnMinimize = passOnMinCheckBox.Checked;

                save = true;
            }

            if (CF.clearClipboardOnExit != clearClipboardCheckBox.Checked)
            {
                CF.clearClipboardOnExit = clearClipboardCheckBox.Checked;

                save = true;
            }

            if (CF.autoBackup.Item1 != autoBackupCheckBox.Checked)
            {
                CF.autoBackup = new Tuple<bool, int>(autoBackupCheckBox.Checked, (int)autoBackupNumericUpDown.Value);

                save = true;
            }

            if (CF.autoBackup.Item2 != autoBackupNumericUpDown.Value)
            {
                AD.backupCountdown = (int)autoBackupNumericUpDown.Value;

                CF.autoBackup = new Tuple<bool, int>(autoBackupCheckBox.Checked, (int)autoBackupNumericUpDown.Value);

                save = true;
            }

            if (CF.invalidLoginsAlert != invalidLoginsAlertCheckBox.Checked)
            {
                CF.invalidLoginsAlert = invalidLoginsAlertCheckBox.Checked;
                 save = true;
            }

            Program.save = save;

            Hide();
        }
        void cancelButton_Click(object sender, EventArgs e)
        {
            ChangeThemeColor(themeColorIndex);

            Hide();
        }

        // methods
        void Unfocus(object sender, EventArgs e)
        {
            // remove focus from other controls
            settingsPanel.Focus();
        }
        void ChangeThemeColor(int index)
        {
            switch (index)
            {
                // alice blue
                case 0:
                    CF.backColor = Color.AliceBlue;
                    CF.buttonColor = AlterColor(CF.backColor, 0.9f);
                    CF.textColor = Color.Black;
                    break;
                    // light blue
                case 1:
                    CF.backColor = Color.LightBlue;
                    CF.buttonColor = AlterColor(CF.backColor, 0.9f);
                    CF.textColor = Color.Black;
                    break;
                    // teal
                case 2:
                    CF.backColor = Color.FromArgb(255, 80, 140, 190);
                    CF.buttonColor = AlterColor(CF.backColor, 1.1f);
                    CF.textColor = Color.Black;
                    break;
                    // dark blue
                case 3:
                    CF.backColor = Color.FromArgb(255, 95, 135, 225);
                    CF.buttonColor = AlterColor(CF.backColor, 1.1f);
                    CF.textColor = Color.Black;
                    break;
                    // purple
                case 4:
                    CF.backColor = Color.FromArgb(255, 107, 82, 169);
                    CF.buttonColor = AlterColor(CF.backColor, 1.1f);
                    CF.textColor = Color.White;
                    break;
                    // red
                case 5:
                    CF.backColor = Color.FromArgb(255, 168, 54, 54);
                    CF.buttonColor = AlterColor(CF.backColor, 1.1f);
                    CF.textColor = Color.White;
                    break;
                    // orange
                case 6:
                    CF.backColor = Color.FromArgb(255, 255, 125, 87);
                    CF.buttonColor = AlterColor(CF.backColor, 1.1f);
                    CF.textColor = Color.Black;
                    break;
                    // gold
                case 7:
                    CF.backColor = Color.FromArgb(255, 255, 215, 70);
                    CF.buttonColor = AlterColor(CF.backColor, 0.9f);
                    CF.textColor = Color.Black;
                    break;
                    // light green
                case 8:
                    CF.backColor = Color.FromArgb(255, 104, 198, 104);
                    CF.buttonColor = AlterColor(CF.backColor, 0.9f);
                    CF.textColor = Color.Black;
                    break;
                    // sky blue
                case 9:
                    CF.backColor = Color.FromArgb(255, 40, 144, 245);
                    CF.buttonColor = AlterColor(CF.backColor, 1.1f);
                    CF.textColor = Color.Black;
                    break;
                    // pink
                case 10:
                    CF.backColor = Color.Plum;
                    CF.buttonColor = AlterColor(CF.backColor, 1.1f);
                    CF.textColor = Color.Black;
                    break;
                    // black
                case 11:
                    CF.backColor = Color.Black;
                    CF.buttonColor = AlterColor(CF.backColor, 1.1f);
                    CF.textColor = Color.White;
                    break;
                    // dark gray
                case 12:
                    CF.backColor = Color.DimGray;
                    CF.buttonColor = AlterColor(CF.backColor, 1.1f);
                    CF.textColor = Color.White;
                    break;
                    // gray
                case 13:
                    CF.backColor = Color.DarkGray;
                    CF.buttonColor = AlterColor(CF.backColor, 1.1f);
                    CF.textColor = Color.White;
                    break;
                    // light gray
                case 14:
                    CF.backColor = Color.Gainsboro;
                    CF.buttonColor = AlterColor(CF.backColor, 0.9f);
                    CF.textColor = Color.Black;
                    break;
                    // white
                case 15:
                    CF.backColor = Color.White;
                    CF.buttonColor = AlterColor(CF.backColor, 0.9f);
                    CF.textColor = Color.Black;
                    break;
                default:
                    break;
            }
        }
        Color AlterColor(Color color, float alter)
        {
            float luminance = .299f * color.R + .587f * color.G + .114f * color.B;

            int newR = Clamp((int)(color.R * alter), 0, 255);
            int newG = Clamp((int)(color.G * alter), 0, 255);
            int newB = Clamp((int)(color.B * alter), 0, 255);

            return Color.FromArgb(newR, newG, newB);
        }
        int Clamp(int num, int min, int max)
        {
            if (num >= min)
            {
                if (num <= max)
                {
                    return num;
                }
                return max;
            }
            return min;
        }
    }
}
