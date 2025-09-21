using System;
using System.Drawing;
using System.Windows.Forms;

namespace Account_Manager
{
    public partial class Setup : Form
    {
        // properties
        AccountData AD
        {
            get
            {
                return Program.accountData;
            }
        }

        // constructor
        public Setup()
        {
            InitializeComponent();
        }

        // events
        void Setup_Load(object sender, EventArgs e)
        {
            // set form colors
            Program.SetThemeColor(this);

            mismatchLabel.ForeColor = Color.Red;

            // focus on create text box
            createTextBox.Focus();
        }
        void confirmTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                EncryptAndStore();
            }
        }
        void confirmButton_Click(object sender, EventArgs e)
        {
            EncryptAndStore();
        }

        // methods
        void EncryptAndStore()
        {
            // encrypt and store main password or prompt user
            if (createTextBox.Text.Length > 6)
            {
                if (createTextBox.Text == confirmTextBox.Text)
                {
                    mismatchLabel.Visible = false;
                    AD.menuPass = AD.CreateHash(confirmTextBox.Text);
                    Program.exit = false;

                    // use form to center next form
                    Program.GetLastFormDimensions(this);

                    Close();
                }
                else
                {
                    mismatchLabel.Text = "Passwords do not match. Please try again.";
                    mismatchLabel.Visible = true;
                }
            }
            else
            {
                mismatchLabel.Text = "Password must be at least 7 characters.";
                mismatchLabel.Visible = true;
            }
        }
    }
}
