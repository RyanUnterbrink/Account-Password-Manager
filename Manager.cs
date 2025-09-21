using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.IO;

namespace Account_Manager
{
    public partial class Manager : Form
    {
        // fields
        int currentPage = 0;
        int lastPage;
        Button[] tableButtons = new Button[10];
        Entry currentEntry;
        List<string> emails = new List<string>();
        Image eyeOpen, eyeClosed, eyeOpenWhite, eyeClosedWhite;
        bool hidePass = false;

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
        public Manager()
        {
            InitializeComponent();

            Program.SetupAfk(this);
        }

        // events
        void Manager_Load(object sender, EventArgs e)
        {
            notesRichTextBox.Text = "";

            // center form window
            Location = Program.CenterForm(this);

            // set form colors
            Program.SetThemeColor(this);

            // get eye images
            eyeOpen = Properties.Resources.Eye_Open;
            eyeClosed = Properties.Resources.Eye_Closed;
            eyeOpenWhite = new Bitmap(new MemoryStream(Properties.Resources.Eye_Open_White));
            eyeClosedWhite = new Bitmap(new MemoryStream(Properties.Resources.Eye_Closed_White));
                
            hidePass = CF.hidePass;

            // add entries to search combo box
            for (int i = 0; i < AD.entries.Count; i++)
            {
                searchComboBox.Items.Add(AD.entries[i].AccountName);
            }

            // configure table buttons
            AddButtonsIntoArray();

            // configure and show table panel
            OrganizeTable();
            ChangePanel(tablePanel);

            // get emails for autofill
            UpdateEmails();
        }
        void Manager_Resize(object sender, EventArgs e)
        {
            Program.PrivacyPanel(this, privacyPanel, WindowState == FormWindowState.Minimized);
        }
        void Manager_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.ExitApp();
        }

        //// table panel
        void searchComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (searchComboBox.Text != "")
            {
                ShowEntry(searchComboBox.Text);
            }
        }
        void button_Click(object sender, EventArgs e)
        {
            // match and show entry with button text
            Button button = sender as Button;
            ShowEntry(button.Text);
        }
        void settingsButton_Click(object sender, EventArgs e)
        {
            // use form to center next form
            Program.GetLastFormDimensions(this);

            // show settings
            using (Settings settings = new Settings())
            {
                // Show the menu dialog and wait for user action
                settings.ShowDialog();
            }

            // set form colors
            Program.SetThemeColor(this);

            // save settings
            Program.SaveConfiguration();
        }
        void addButton_Click(object sender, EventArgs e)
        {
            ShowEntryAddOrEdit(true);
            accountTextBox.Focus();
        }
        void tableNextButton_Click(object sender, EventArgs e)
        {
            // switch pages
            if (currentPage == lastPage)
            {
                currentPage = 0;
            }
            else
            {
                ++currentPage;
            }

            // configure and show table panel
            OrganizeTable();
        }
        void tablePrevButton_Click(object sender, EventArgs e)
        {
            // switch pages
            if (currentPage == 0)
            {
                currentPage = lastPage;
            }
            else
            {
                --currentPage;
            }

            // configure and show table panel
            OrganizeTable();
        }

        //// entry panel
        void copyLabel_DoubleClick(object sender, EventArgs e)
        {
            Label label = sender as Label;
            label.Focus();
            if (label.Text != "")
            {
                label.BackColor = SystemColors.GradientActiveCaption;
                if (label.Name == "passwordLabel")
                {
                    Clipboard.SetText(currentEntry.Pass);
                }
                else
                {
                    Clipboard.SetText(label.Text);
                }
            }
        }
        void focusLabel_Leave(object sender, EventArgs e)
        {
            Label label = sender as Label;
            label.BackColor = SystemColors.Control;
        }
        void entryNextButton_Click(object sender, EventArgs e)
        {
            // show next entry in list
            int index = AD.entries.IndexOf(currentEntry);
            if (index + 1 == AD.entries.Count)
            {
                index = 0;
                currentEntry = AD.entries[index];
            }
            else
            {
                currentEntry = AD.entries[++index];
            }

            ShowEntry(currentEntry.AccountName);
        }
        void entryPrevButton_Click(object sender, EventArgs e)
        {
            // show previous entry in list
            int index = AD.entries.IndexOf(currentEntry);

            if (index == 0)
            {
                index = AD.entries.Count - 1;
                currentEntry = AD.entries[index];
            }
            else
            {
                currentEntry = AD.entries[--index];
            }
            
            ShowEntry(currentEntry.AccountName);
        }
        void tableButton_Click(object sender, EventArgs e)
        {
            // configure and show table panel
            OrganizeTable();
            ChangePanel(tablePanel);
        }
        void editButton_Click(object sender, EventArgs e)
        {
            ShowEntryAddOrEdit(false);

            accountTextBox.Text = currentEntry.AccountName;
            emailComboBox.Text = currentEntry.Email;
            usernameTextBox.Text = currentEntry.Username;
            passwordTextBox.Text = currentEntry.Pass;
            notesEditRichTextBox.Text = currentEntry.Notes;
        }

        //// entry edit panel
        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (addAddButton.Visible)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    Add();
                    e.Handled = true;
                }
            }
            else
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    Edit();
                    e.Handled = true;
                }
            }
        }
        void addAddButton_Click(object sender, EventArgs e)
        {
            Add();
        }
        void cancelAddButton_Click(object sender, EventArgs e)
        {
            // show table panel and reset entry edit
            ChangePanel(tablePanel);
            ResetEntryEdit();
        }
        void doneEditButton_Click(object sender, EventArgs e)
        {
            Edit();
        }
        void deleteEditButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this account?", "Account Manager", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // delete current entry
                Delete();
            }
        }
        void eyeButton_Click(object sender, EventArgs e)
        {
            hidePass = !hidePass;

            ChangeEyeButtonImage();

            if (hidePass)
            {
                passwordLabel.Text = "*******";
            }
            else
            {
                passwordLabel.Text = currentEntry.Pass;
            }
        }

        //// all panels
        void Unfocus(object sender, EventArgs e)
        {
            // remove focus from other controls
            entryPanel.Focus();
        }

        void ChangePanel(Panel panel)
        {
            // make panel array for index access
            Panel[] panels = new Panel[3]
            {
                tablePanel,
                entryPanel,
                entryEditPanel
            };

            // make array of certain controls that could be focused
            Control[] focusControls = new Control[3]
            {
                    searchComboBox,
                    entryNextButton,
                    cancelAddButton
            };

            // iterate through panels to change to correct one
            for (int i = 0; i < panels.Length; i++)
            {
                // store button controls of panel in list
                List<Control> controls = new List<Control>();
                Program.GetAllControlsOfType(panels[i], controls, addAddButton);

                // if panel is table panel, add search combo box to list and make empty 
                if (panels[i] == tablePanel)
                {
                    controls.Add(searchComboBox);
                    searchComboBox.Text = "";
                }

                // isolate panel visibility and tab control
                if (panels[i] == panel)
                {
                    panels[i].Visible = true;
                    for (int j = 0; j < controls.Count; j++)
                    {
                        controls[j].TabStop = true;
                    }

                    focusControls[i].Focus();
                }
                else
                {
                    panels[i].Visible = false;
                    for (int j = 0; j < controls.Count; j++)
                    {
                        controls[j].TabStop = false;
                    }
                }
            }
        }
        void AddButtonsIntoArray()
        {
            // put all table panel controls in a temp list
            List<Control> tempButtons = new List<Control>();
            Button b = new Button();
            Program.GetAllControlsOfType(tablePanel, tempButtons, b);

            // get table buttons from temp list
            for (int i = 0; i < 10; i++)
            {
                foreach (var button in tempButtons)
                {
                    if (button.Name == $"button{i + 1}")
                    {
                        tableButtons[i] = (Button)button;
                        break;
                    }
                }
            }
        }
        void OrganizeTable()
        {
            // set pages
            lastPage = (AD.entries.Count - 1) / 10;
            if (currentPage > lastPage)
            {
                currentPage = lastPage;
            }

            // (re)set table buttons visibility to false
            for (int i = 0; i < 10; i++)
            {
                tableButtons[i].Visible = false;
            }

            // set correct button names to buttons on specific page
            for (int i = 0; i < AD.entries.Count - (10 * currentPage) && i < 10; i++)
            {
                tableButtons[i].Text = AD.entries[i + (10 * currentPage)].AccountName;
                tableButtons[i].Click += new EventHandler(button_Click);
                tableButtons[i].Visible = true;
            }

            // configure prev and next buttons
            if (AD.entries.Count < 11)
            {
                tableNextButton.Visible = false;
                tablePrevButton.Visible = false;
            }
            else
            {
                tableNextButton.Visible = true;
                tablePrevButton.Visible = true;
            }
        }
        void ShowEntry(string accountName)
        {
            // match entry with text
            bool isAccount = false;
            for (int i = 0; i < AD.entries.Count; i++)
            {
                if (accountName == AD.entries[i].AccountName)
                {
                    isAccount = true;

                    accountLabel.Text = AD.entries[i].AccountName;
                    emailLabel.Text = AD.entries[i].Email;
                    usernameLabel.Text = AD.entries[i].Username;
                    if (AD.entries[i].Pass != "")
                    {
                        if (CF.hidePass)
                        {
                            passwordLabel.Text = "*******";
                        }
                        else
                        {
                            passwordLabel.Text = AD.entries[i].Pass;
                        }
                    }
                    else
                    {
                        passwordLabel.Text = "";
                    }
                    notesRichTextBox.Text = AD.entries[i].Notes;

                    // set current entry
                    currentEntry = AD.entries[i];
                    
                    // match current page with entry
                    currentPage = i / 10;

                    break;
                }
            }

            if (!isAccount)
            {
                return;
            }

            // disable next and previous buttons if only one entry in list
            if (AD.entries.Count < 2)
            {
                entryNextButton.Visible = false;
                entryPrevButton.Visible = false;
            }
            else
            {
                entryNextButton.Visible = true;
                entryPrevButton.Visible = true;
            }

            hidePass = CF.hidePass;
            ChangeEyeButtonImage();

            ChangePanel(entryPanel);
        }
        void ShowEntryAddOrEdit(bool add)
        {
            ChangePanel(entryEditPanel);
            addAddButton.Visible = add;
            cancelAddButton.Visible = add;
            doneEditButton.Visible = !add;
            deleteEditButton.Visible = !add;
        }
        void Add()
        {
            // check if account should be created
            if (accountTextBox.Text == "")
            {
                MessageBox.Show("The account must have a name.", "Account Manager");
                return;
            }

            foreach (Entry entry in AD.entries)
            {
                if (accountTextBox.Text.ToUpper() == entry.AccountName.ToUpper())
                {
                    MessageBox.Show("This account already exists.", "Account Manager");
                    return;
                }
            }

            // add new entry to entries and search combo box
            Entry newEntry = new Entry(accountTextBox.Text, emailComboBox.Text, usernameTextBox.Text, passwordTextBox.Text, notesEditRichTextBox.Text);
            currentPage = AD.SortAndAdd(newEntry) / 10;
            searchComboBox.Items.Add(newEntry.AccountName);

            // configure and show table panel, and reset entry edit
            OrganizeTable();
            ChangePanel(tablePanel);
            ResetEntryEdit();

            // get emails for autofill
            UpdateEmails();

            Program.save = true;
        }
        void Edit()
        {
            bool changed = currentEntry.AccountName != accountTextBox.Text ||
                currentEntry.Email != emailComboBox.Text ||
                currentEntry.Username != usernameTextBox.Text ||
                currentEntry.Pass != passwordTextBox.Text ||
                currentEntry.Notes != notesEditRichTextBox.Text;

            // see if anything changed
            if (changed)
            {
                // remove old current entry account name from search combo box
                searchComboBox.Items.Remove(currentEntry.AccountName);

                // replace current entry from entry edit panel
                currentEntry.AccountName = accountTextBox.Text;
                currentEntry.Email = emailComboBox.Text;
                currentEntry.Username = usernameTextBox.Text;
                currentEntry.Pass = passwordTextBox.Text;
                currentEntry.Notes = notesEditRichTextBox.Text;

                // add new current entry account name to search combo box
                searchComboBox.Items.Add(currentEntry.AccountName);

                // update entry panel with current entry
                accountLabel.Text = currentEntry.AccountName;
                emailLabel.Text = currentEntry.Email;
                usernameLabel.Text = currentEntry.Username;
                if (currentEntry.Pass != "")
                {
                    passwordLabel.Text = "*******";
                }
                else
                {
                    passwordLabel.Text = "";
                }
                notesRichTextBox.Text = currentEntry.Notes;

                AD.ResortAll();

                // get emails for autofill
                UpdateEmails();

                Program.save = true;
            }

            // show entry panel and reset entry edit
            ChangePanel(entryPanel);
            ResetEntryEdit();
        }
        void Delete()
        {
            // set current entry to a new entry before removal
            Entry temp = currentEntry;
            int index = AD.entries.IndexOf(currentEntry);
            if (AD.entries.Count > 1)
            {
                if (index + 1 == AD.entries.Count)
                {
                    currentEntry = AD.entries[--index];
                }
                else
                {
                    currentEntry = AD.entries[index];
                }
            }

            // remove entry from entries and search box
            searchComboBox.Items.Remove(temp.AccountName);
            AD.entries.Remove(temp);

            // configure and show table panel, and reset entry edit
            OrganizeTable();
            ChangePanel(tablePanel);
            ResetEntryEdit();

            // get emails for autofill
            UpdateEmails();

            Program.save = true;
        }
        void UpdateEmails()
        {
            emails = AD.entries.Select(em => em.Email).Where(e => !string.IsNullOrWhiteSpace(e)).Distinct().ToList();

            emailComboBox.Items.Clear();
            emailComboBox.Items.AddRange(emails.ToArray());
        }
        void ResetEntryEdit()
        {
            accountTextBox.Text = "";
            emailComboBox.Text = "";
            usernameTextBox.Text = "";
            passwordTextBox.Text = "";
            notesEditRichTextBox.Text = "";
        }
        void ChangeEyeButtonImage()
        {
            if (CF.textColor == Color.White)
            {
                eyeButton.BackgroundImage = hidePass ? eyeClosedWhite : eyeOpenWhite;
            }
            else
            {
                eyeButton.BackgroundImage = hidePass ? eyeClosed : eyeOpen;
            }
        }
    }
}
