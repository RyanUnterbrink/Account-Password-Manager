using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Manager
{
    public class LoginData
    {
        public string loginText;
        public DateTime loginTime;
        public float timeToLogin;

        public LoginData(string _loginText, DateTime _loginTime, float _timeToLogin)
        {
            loginText = _loginText;
            loginTime = _loginTime;
            timeToLogin = _timeToLogin;
        }
    }
}
