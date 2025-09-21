using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Account_Manager
{
    public static class Program
    {
        // fields
        static int f1Width;
        static int f1Height;
        static Point f1Position;
        public static bool autoCenterMenu = false;
        public static bool passSuccess = false;
        public static string apiInfo;
        static Timer afkTimer;
        static float afkIdleTime = 0;
        static int afkTimeInterval = 1;
        public static bool exit = true;

        public static bool save = false;
        public static SaveLoad saveLoad = new SaveLoad();
        public static AccountData accountData = new AccountData();
        public static Configuration config = new Configuration();

        // methods
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _ = InfoGatherAPI();

            // load account data
            LoadManaged();

            // load and set settings data
            LoadConfiguration();

            // check if account data has a main password
            if (accountData.menuPass == accountData.CreateHash(""))
            {
                // set up main password
                MessageBox.Show("No file loaded. Proceed to setup.", "Account Manager");
                Application.Run(new Setup());
            }
            else
            {
                // allow menu form to be center screen
                exit = false;
                autoCenterMenu = true;
            }

            // initialize AFK Timer
            afkTimer = new Timer();
            afkTimer.Interval = afkTimeInterval * 1000;
            afkTimer.Tick += CheckAfkTime;
            afkTimer.Start();

            // run menu form
            if (!exit)
            {
                Application.Run(new Menu());
            }

            // if password was correctly entered, proceed to manager
            if (passSuccess)
            {
                Application.Run(new Manager());
            }
        }
        static void LoadManaged()
        {
            // load account data
            string s = "";
            s = saveLoad.LoadManaged(s);

            accountData.DecryptAccountData(s);
        }
        static void LoadConfiguration()
        {
            config = saveLoad.LoadConfiguration(config);
        }
        public static void SaveManaged()
        {
            if (config.autoBackup.Item1)
            {
                int countDown = --accountData.backupCountdown;
                if (countDown == 0)
                {
                    accountData.backupCountdown = config.autoBackup.Item2;

                    EmailBackup();
                }
            }

            string s = accountData.EncryptAccountData();

            saveLoad.SaveManaged(s);
        }
        public static void SaveConfiguration()
        {
            saveLoad.SaveConfiguration(config);
        }
        public static void ExitApp()
        {
            if (save)
            {
                if (config.clearClipboardOnExit)
                {
                    Clipboard.Clear();
                }

                // encrypt and save account data before exiting
                SaveManaged();
            }

            Application.Exit(); // Quit the application
        }
        public static void GetLastFormDimensions(Form f)
        {
            // store a form's location and size data to center future forms
            f1Width = f.Width;
            f1Height = f.Height;
            f1Position = new Point(f.Location.X, f.Location.Y);
        }
        public static Point CenterForm(Form f2)
        {
            // center a form based on a previous form's location and size data
            return new Point(f1Position.X + f1Width / 2 - f2.Width / 2, f1Position.Y + f1Height / 2 - f2.Height / 2);
        }
        public static void GetAllControlsOfType(Control controlsParent, List<Control> controls, dynamic type)
        {
            // store all controls from c to controls list
            foreach (Control c in controlsParent.Controls)
            {
                if (c.GetType() == type.GetType())
                {
                    controls.Add(c);
                }
                else if (c.GetType() == typeof(Panel))
                {
                    GetAllControlsOfType(c, controls, type);
                }
            }
        }
        public static void GetAllControls(Control control, List<Control> controls)
        {
            // store all controls from c to controls list
            foreach (Control c in control.Controls)
            {
                if (c.GetType() == typeof(Panel))
                {
                    GetAllControls(c, controls);
                }
                else
                {
                    controls.Add(c);
                }
            }
        }
        public static void SetupAfk(Control controlsParent)
        {
            List<Control> controls = new List<Control>();
            GetAllControlsOfType(controlsParent, controls, new Button());
            GetAllControlsOfType(controlsParent, controls, new TextBox());
            GetAllControlsOfType(controlsParent, controls, new ComboBox());
            GetAllControlsOfType(controlsParent, controls, new RichTextBox());

            foreach (var c in controls)
            {
                c.Click += ResetAfkTimer;
                c.MouseMove += ResetAfkTimer;
                c.KeyPress += ResetAfkTimer;
            }
        }
        public static void ResetAfkTimer(object sender, EventArgs e)
        {
            afkIdleTime = 0;
        }
        public static void ResetAfkTimer(object sender, KeyPressEventArgs e)
        {
            afkIdleTime = 0;
        }
        public static void ResetAfkTimer()
        {
            afkIdleTime = 0;
        }
        public static void CheckAfkTime(object sender, EventArgs e)
        {
            if (!config.afkTimer.Item1)
            {
                return;
            }

            afkIdleTime += afkTimeInterval;

            if (afkIdleTime >= config.afkTimer.Item2)
            {
                afkTimer.Stop();

                ExitApp();
            }
        }

        // email
        public static MailMessage SetupEmail(string address, string subject, string body, string attachmentPath, out string emailProvider, out string smtpServer, out int smtpPort)
        {
            emailProvider = GetEmailProviderSMTPServerAndPort(address, out smtpServer, out smtpPort);
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(address),
                Subject = $"Account Manager - {subject}",
                Body = body
            };
            mail.To.Add(address);

            // Attach the file if the path is valid
            if (File.Exists(attachmentPath))
            {
                Attachment attachment = new Attachment(attachmentPath);
                mail.Attachments.Add(attachment);
            }
            else
            {
                Console.WriteLine("Attachment file not found.");
            }

            return mail;
        }
        public static void SendEmail(MailMessage mail, string address, string passKey, string smtpServer, int smtpPort)
        {
            using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
            {
                client.Credentials = new NetworkCredential(address, passKey);
                client.EnableSsl = true;
                client.Send(mail);
            }
        }
        public static string GetEmailProviderSMTPServerAndPort(string address, out string smtpServer, out int smtpPort)
        {
            smtpServer = "";
            smtpPort = 0;

            // Extract email provider from address
            string emailProvider = address.Split('@')[1].ToLower();

            switch (emailProvider)
            {
                case "gmail.com":
                    smtpServer = "smtp.gmail.com";
                    smtpPort = 587;
                    break;
                case "outlook.com":
                case "hotmail.com":
                case "live.com":
                case "office365.com":
                case "msn.com":
                case "passport.com":
                    smtpServer = "smtp.office365.com";
                    smtpPort = 587;
                    break;
                case "yahoo.com":
                case "ymail.com":
                case "rocketmail.com":
                    smtpServer = "smtp.mail.yahoo.com";
                    smtpPort = 465;
                    break;
                case "aol.com":
                    smtpServer = "smtp.aol.com";
                    smtpPort = 587;
                    break;
                case "icloud.com":
                case "me.com":
                    smtpServer = "smtp.mail.me.com";
                    smtpPort = 587;
                    break;
                case "yandex.com":
                case "yandex.ru":
                    smtpServer = "smtp.yandex.com";
                    smtpPort = 465;
                    break;
                case "zoho.com":
                    smtpServer = "smtp.zoho.com";
                    smtpPort = 587;
                    break;
                case "protonmail.com":
                    smtpServer = "smtp.protonmail.com";
                    smtpPort = 465;
                    break;
                case "gmx.com":
                case "gmx.us":
                    smtpServer = "smtp.gmx.com";
                    smtpPort = 587;
                    break;
                case "mail.com":
                    smtpServer = "smtp.mail.com";
                    smtpPort = 587;
                    break;
                case "fastmail.com":
                    smtpServer = "smtp.fastmail.com";
                    smtpPort = 465;
                    break;
                case "posteo.de":
                    smtpServer = "smtp.posteo.de";
                    smtpPort = 465;
                    break;
                case "tutanota.com":
                    smtpServer = "smtp.tutanota.com";
                    smtpPort = 587;
                    break;
                case "runbox.com":
                    smtpServer = "smtp.runbox.com";
                    smtpPort = 587;
                    break;
                case "hushmail.com":
                    smtpServer = "smtp.hushmail.com";
                    smtpPort = 465;
                    break;
                case "mailfence.com":
                    smtpServer = "smtp.mailfence.com";
                    smtpPort = 587;
                    break;
                case "startmail.com":
                    smtpServer = "smtp.startmail.com";
                    smtpPort = 465;
                    break;
                case "qq.com":
                    smtpServer = "smtp.qq.com";
                    smtpPort = 465;
                    break;
                case "126.com":
                    smtpServer = "smtp.126.com";
                    smtpPort = 465;
                    break;
                case "163.com":
                    smtpServer = "smtp.163.com";
                    smtpPort = 465;
                    break;
                case "naver.com":
                    smtpServer = "smtp.naver.com";
                    smtpPort = 465;
                    break;
                case "seznam.cz":
                    smtpServer = "smtp.seznam.cz";
                    smtpPort = 465;
                    break;
                case "rediffmail.com":
                    smtpServer = "smtp.rediffmail.com";
                    smtpPort = 587;
                    break;
                case "lycos.com":
                    smtpServer = "smtp.lycos.com";
                    smtpPort = 587;
                    break;
                case "tiscali.co.uk":
                    smtpServer = "smtp.tiscali.co.uk";
                    smtpPort = 587;
                    break;
                case "btinternet.com":
                    smtpServer = "mail.btinternet.com";
                    smtpPort = 587;
                    break;
                case "verizon.net":
                    smtpServer = "smtp.verizon.net";
                    smtpPort = 465;
                    break;
                case "att.net":
                    smtpServer = "smtp.att.net";
                    smtpPort = 465;
                    break;
                case "comcast.net":
                    smtpServer = "smtp.comcast.net";
                    smtpPort = 587;
                    break;
                case "charter.net":
                    smtpServer = "smtp.charter.net";
                    smtpPort = 587;
                    break;
                case "cox.net":
                    smtpServer = "smtp.cox.net";
                    smtpPort = 465;
                    break;
                case "earthlink.net":
                    smtpServer = "smtp.earthlink.net";
                    smtpPort = 587;
                    break;
                case "bellsouth.net":
                    smtpServer = "smtp.bellsouth.net";
                    smtpPort = 465;
                    break;
                case "shaw.ca":
                    smtpServer = "mail.shaw.ca";
                    smtpPort = 587;
                    break;
                case "rogers.com":
                    smtpServer = "smtp.broadband.rogers.com";
                    smtpPort = 587;
                    break;
                case "telus.net":
                    smtpServer = "smtp.telus.net";
                    smtpPort = 587;
                    break;
                case "optonline.net":
                    smtpServer = "mail.optonline.net";
                    smtpPort = 587;
                    break;
                default:
                    Console.WriteLine("Unknown email provider. Please configure SMTP settings manually.");
                    break;
            }

            return emailProvider;
        }
        public static void ConfigureEmail(string address, string passKey, Label emailLabel, Label passKeyLabel)
        {
            // check if email and pass key are correct and open up a dialog to confirm its connected
            // setup email
            if (address == "")
            {
                Console.WriteLine($"No email.");
                return;
            }

            if (passKey == "")
            {
                Console.WriteLine($"No pass key.");
                return;
            }

            string subject = "SMTP Connection Test";
            string body = "This is a test email to confirm SMTP connectivity.";

            // send email
            MailMessage mail = SetupEmail(address, subject, body, "", out string emailProvider, out string smtpServer, out int smtpPort);

            try
            {
                SendEmail(mail, address, passKey, smtpServer, smtpPort);

                accountData.email = address;
                accountData.passKey = passKey;

                emailLabel.Text = "Email: Connected";
                passKeyLabel.Text = "Pass Key: Connected";

                MessageBox.Show("SMTP connection successful! Test email sent. Email and pass key stored.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (smtpServer == "" || smtpPort == 0)
                {
                    emailProvider = CapitalizeFirstLetter(emailProvider);

                    MessageBox.Show($"{emailProvider} is not supported: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Error sending email with {address}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    Console.WriteLine($"Error sending email with {address}: " + ex.Message);
                }
            }
        }
        public static void EmailMyselfIncorrectAttempts(List<LoginData> invalidLogins, DateTime menuStart)
        {
            // check if email and pass key are correct and open up a dialog to confirm its connected
            if (accountData.email == "")
            {
                Console.WriteLine($"No email.");
                return;
            }

            if (accountData.passKey == "")
            {
                Console.WriteLine($"No pass key.");
                return;
            }

            // setup email
            string address = accountData.email;
            string subject = "Invalid Logins";

            string body = $"Invalid Logins Alert!\n\n" +
                $"Time: {menuStart.ToString("M/d/yy")} {FormatTime(menuStart)}\n" +
                $"{apiInfo}\n" +
                $"Invalid Login Count: {invalidLogins.Count}\n\n";
            for (int i = 0; i < invalidLogins.Count; i++)
            {
                LoginData d = invalidLogins[i];
                body += $"{i + 1}. {FormatTime(d.loginTime)}\n\t{d.timeToLogin.ToString("n3")} s\n\t{d.loginText}\n";
            }

            // send email
            MailMessage mail = SetupEmail(address, subject, body, "", out string emailProvider, out string smtpServer, out int smtpPort);

            try
            {
                SendEmail(mail, address, accountData.passKey, smtpServer, smtpPort);

                Console.WriteLine("Invalid Logins alert email sent.");
            }
            catch (Exception ex)
            {
                emailProvider = CapitalizeFirstLetter(emailProvider);
                if (smtpServer == "" || smtpPort == 0)
                {
                    Console.WriteLine($"{emailProvider} is not supported: " + ex.Message);
                }
                else
                {
                    Console.WriteLine($"Error sending email with {address}: " + ex.Message);
                }
            }
        }
        public static void EmailBackup()
        {
            // setup email
            string address = accountData.email;
            string passKey = accountData.passKey;

            if (address == "")
            {
                Console.WriteLine("No email.");
                return;
            }

            if (passKey == "")
            {
                Console.WriteLine("No pass key.");
                return;
            }

            string subject = "Backup";
            string body = "This is a backup file.";

            // send email
            MailMessage mail = SetupEmail(address, subject, body, saveLoad.ManagedDest, out string emailProvider, out string smtpServer, out int smtpPort);

            try
            {
                SendEmail(mail, address, passKey, smtpServer, smtpPort);

                Console.WriteLine("Backup email sent.");
            }
            catch (Exception ex)
            {
                if (smtpServer == "" || smtpPort == 0)
                {
                    Console.WriteLine($"Error with email. Not set up. {ex.Message}");
                }
                else
                {
                    Console.WriteLine($"Error sending email with {address}. {ex.Message}");
                }
            }
        }
        static async Task InfoGatherAPI()
        {
            HttpClient client = new HttpClient();

            // ip address, add after location
            HttpResponseMessage response = await client.GetAsync("https://api64.ipify.org?format=json");
            string json = await response.Content.ReadAsStringAsync();
            string ipAddress = JObject.Parse(json)["ip"].ToString();

            string ip = $"IP Address: {ipAddress}\n";

            // location
            response = await client.GetAsync($"http://ip-api.com/json/{ipAddress}");
            json = await response.Content.ReadAsStringAsync();
            JObject data = JObject.Parse(json);
            string city = (string)data["city"];
            string region = (string)data["regionName"];
            string country = (string)data["country"];
            apiInfo = $"Location: {city}, {region} {country}\n";

            apiInfo += ip;

            // os
            string os = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                os = $"Windows {Environment.OSVersion.Version}";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                os = "Linux";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                os = "macOS";
            else
                os = "Unknown OS";
            apiInfo += $"OS: {os}";
        }

        public static void PrivacyPanel(Form form, Panel privacyPanel, bool minimized)
        {
            if (!config.passOnMinimize)
            {
                return;
            }

            if (minimized)
            {
                privacyPanel.BringToFront();

                passSuccess = false;
            }
            else
            {
                using (Menu menu = new Menu())
                {
                    // Show the menu dialog and wait for user action
                    menu.ShowDialog();
                }

                if (!passSuccess)
                {
                    ExitApp();
                }

                privacyPanel.SendToBack();
            }
        }

        // color
        public static void SetThemeColor(Form form)
        {
            // set form colors that should already be calculated
            SetBackColor(form);
            SetButtonColor(form);
            SetTextColor(form);
        }
        static void SetBackColor(Form form)
        {
            // set new background color in form and panels
            form.BackColor = config.backColor;

            List<Control> panels = new List<Control>();
            Panel panel = new Panel();
            GetAllControlsOfType(form, panels, panel);
            foreach (Panel p in panels)
            {
                p.BackColor = config.backColor;
            }
        }
        static void SetTextColor(Form form)
        {
            // set new text color in labels and buttons
            List<Control> controls = new List<Control>();
            GetAllControlsOfType(form, controls, new Label());
            GetAllControlsOfType(form, controls, new Button());
            GetAllControlsOfType(form, controls, new CheckBox());
            for (int i = 0; i < controls.Count; i++)
            {
                controls[i].ForeColor = config.textColor;
            }
        }
        static void SetButtonColor(Form form)
        {
            // set new button color
            List<Control> controls = new List<Control>();
            GetAllControlsOfType(form, controls, new Button());
            for (int i = 0; i < controls.Count; i++)
            {
                controls[i].BackColor = config.buttonColor;
            }
        }

        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1);
        }
        static string FormatTime(DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss") + "." + dateTime.Millisecond.ToString("000");
        }
    }
}
