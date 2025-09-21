using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Account_Manager
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();

            Program.SetupAfk(this);
        }

        void Help_Load(object sender, EventArgs e)
        {
            // center form window
            Location = Program.CenterForm(this);

            // set form colors and save original color
            Program.SetThemeColor(this);
        }
    }
}
