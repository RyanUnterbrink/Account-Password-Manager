using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Account_Manager
{
    public partial class Menu : Form
    {
        // fields
        string typedByUser = "";
        DateTime menuStart;
        List<LoginData> invalidLogins = new List<LoginData>();
        float bruteForceThreshold = .333f;

        bool test = false;

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
        public Menu()
        {
            menuStart = DateTime.Now;

            InitializeComponent();

            Program.SetupAfk(this);
        }

        // events
        void Menu_Load(object sender, EventArgs e)
        {
            // center form window
            if (Program.autoCenterMenu)
            {
                CenterToScreen();
            }
            else
            {
                Location = Program.CenterForm(this);
            }

            // set form colors
            Program.SetThemeColor(this);

            mismatchLabel.ForeColor = Color.Red;
        }
        void passwordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // pressing enter will compare and encrypt user input
            if (e.KeyChar == (char)Keys.Enter)
            {
                typedByUser += $"{passwordTextBox.Text}\n";

                EncryptAndCompare();
            }
        }
        void confirmButton_Click(object sender, EventArgs e)
        {
            EncryptAndCompare();
        }

        // methods
        void EncryptAndCompare()
        {
            // compare input with stored password or prompt user
            string menuPass = AD.CreateHash(passwordTextBox.Text);
            if (menuPass == AD.menuPass || test)
            {
                mismatchLabel.Visible = false;
                Program.passSuccess = true;

                // use form to center next form
                Program.GetLastFormDimensions(this);

                invalidLogins.Clear();

                Close();
            }
            else
            {
                IncorrectAttempt();
            }
        }
        void IncorrectAttempt()
        {

            double timeToLogin;
            DateTime now = DateTime.Now;
            if (invalidLogins.Count > 0)
                timeToLogin = (now - invalidLogins[0].loginTime).TotalSeconds;
            else
                timeToLogin = (now - menuStart).TotalSeconds;
            invalidLogins.Add(new LoginData(typedByUser, now, (float)timeToLogin));

            typedByUser = "";

            mismatchLabel.Visible = true;

            bool maxInvalidLoginsReached = invalidLogins.Count == CF.invalidLogins.Item2;
            bool exitOnMaxInvalidLogins = CF.invalidLogins.Item1;
            bool emailMyselfOnMaxInvalidLogins = CF.invalidLoginsAlert;

            if (maxInvalidLoginsReached)
            {
                if (emailMyselfOnMaxInvalidLogins)
                {
                    Program.EmailMyselfIncorrectAttempts(invalidLogins, menuStart);
                }

                bool bruteForcing = invalidLogins.Average(data => data.timeToLogin) < bruteForceThreshold;
                if (exitOnMaxInvalidLogins || bruteForcing)
                {
                    Application.Exit();
                }

                invalidLogins.Clear();
            }
        }
        
        void Unfocus(object sender, EventArgs e)
        {
            // remove focus from other controls
            menuPanel.Focus();
        }
    }
}
