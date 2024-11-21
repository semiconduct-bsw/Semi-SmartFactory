using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20241112_CIMPCGUI01
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }

        private void LogForm_Load(object sender, EventArgs e)
        {

        }

        public void AppendLog(string logMessage)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendLog), logMessage);
            }
            else
            {
                LogBox.AppendText(logMessage + Environment.NewLine);
            }
        }
    }
}
