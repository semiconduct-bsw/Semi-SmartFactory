using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20241128_CIMUpgrade
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }

        private void LogForm_Load(object sender, EventArgs e)
        {
            LoadExistingLogs(); // 기존 로그 불러오기
        }

        private void LoadExistingLogs()
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
            string logFilePath = Path.Combine(logDirectory, $"{DateTime.Now:yyyyMMdd}.log");

            // 로그 파일이 존재하면 내용 읽어서 LogBox에 추가
            if (File.Exists(logFilePath))
            {
                // 파일의 모든 내용을 한 번에 읽어옴
                string allLogs = File.ReadAllText(logFilePath);

                // LogBox에 한 번에 설정
                LogBox.Text = allLogs;

                // 스크롤을 맨 아래로 이동
                LogBox.SelectionStart = LogBox.TextLength;
                LogBox.ScrollToCaret();
            }
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
