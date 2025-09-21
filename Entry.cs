
namespace Account_Manager
{
    public class Entry
    {
        // fields
        string accountName;
        string email;
        string username;
        string pass;
        string notes;

        // properties
        public string AccountName
        {
            get
            {
                return accountName;
            }
            set
            {
                accountName = value;
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }
        public string Pass
        {
            get
            {
                return pass;
            }
            set
            {
                pass = value;
            }
        }
        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = value;
            }
        }

        // constructors
        public Entry()
        {
            AccountName = "";
            Email = "";
            Username = "";
            Pass = "";
            Notes = "";
        }
        public Entry(string accountName, string email, string username, string pass, string notes)
        {
            AccountName = accountName;
            Email = email;
            Username = username;
            Pass = pass;
            Notes = notes;
        }
    }
}
