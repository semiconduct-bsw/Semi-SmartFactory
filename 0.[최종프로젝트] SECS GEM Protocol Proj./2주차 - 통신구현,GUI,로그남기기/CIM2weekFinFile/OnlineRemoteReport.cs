using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20241112_CIMPCGUI01
{
    public partial class OnlineRemoteReportForm : Form
    {
        private CIMMainForm mainForm;

        public OnlineRemoteReportForm(CIMMainForm cIMMainForm)
        {
            InitializeComponent();
            this.mainForm = cIMMainForm;
        }

        private void btnSaveORR_Click(object sender, EventArgs e)
        {
            string opid = tbOPID.Text.Trim(); // 입력된 OPID를 가져옴
            if (string.IsNullOrEmpty(opid))
            {
                MessageBox.Show("OPID를 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // OPID를 MainForm에 설정
            mainForm.anoOPID = opid;

            // OPID를 MES로 전송
            if (mainForm.serverRealSocket != null && mainForm.serverRealSocket.Connected)
            {
                try
                {
                    string message = $"_10104/{opid}";
                    byte[] dataToSend = Encoding.UTF8.GetBytes(message);

                    // 데이터 전송
                    mainForm.serverRealSocket.Send(dataToSend);

                    // LogBox에 기록
                    mainForm.LogMessage($"MES로 OPID 전송: {opid}");
                }
                catch (SocketException ex)
                {
                    mainForm.LogMessage($"MES로 OPID 전송 실패: {ex.Message}");
                    MessageBox.Show("MES와의 통신 중 오류가 발생했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                mainForm.LogMessage("MES와의 연결이 끊어져 OPID를 전송할 수 없습니다.");
                MessageBox.Show("MES 연결 상태를 확인하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // 설정이 완료되면 폼을 닫음
            this.Close();
        }
    }
}
