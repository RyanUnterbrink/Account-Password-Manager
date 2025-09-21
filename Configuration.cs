using System;
using System.Drawing;

namespace Account_Manager
{
    [Serializable]
    public class Configuration
    {
        // fields
        public int themeColorIndex = 0;
        public Color backColor = Color.AliceBlue;
        public Color buttonColor = Color.FromArgb(215, 223, 229);
        public Color textColor = Color.Black;
        public Tuple<bool, int> invalidLogins = new Tuple<bool, int>(true, 5);
        public Tuple<bool, int> afkTimer = new Tuple<bool, int>(true, 300);
        public bool hidePass;
        public bool passOnMinimize;
        public bool clearClipboardOnExit;
        public Tuple<bool, int> autoBackup = new Tuple<bool, int>(true, 5);
        public bool invalidLoginsAlert;
    }
}
