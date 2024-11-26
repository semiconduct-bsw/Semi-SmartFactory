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
using static _20241112_CIMPCGUI01.Reports.ReportStructs;

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
            OnlineRemoteChangedReport orr = new OnlineRemoteChangedReport
            {
                ReportID = "10104",
                ModelID = "Sample Model",
                OPID = tbOPID.Text.Trim()
            };

            if (string.IsNullOrEmpty(orr.OPID))
            {
                MessageBox.Show("빈 칸을 채워주세요!", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // MainForm에 설정
            mainForm.anoOPID = orr.OPID;
            mainForm.anoModelID = orr.ModelID;
            mainForm.lbOPIDOut.Text = $"오늘의 작업자 : {orr.OPID}";

            // OPID를 MES로 전송
            if (mainForm.serverRealSocket != null && mainForm.serverRealSocket.Connected)
            {
                try
                {
                    string message = $"_{orr.ReportID}/{orr.OPID}/{orr.ModelID}";
                    byte[] dataToSend = Encoding.UTF8.GetBytes(message);

                    // 데이터 전송
                    mainForm.serverRealSocket.Send(dataToSend);

                    // LogBox에 기록
                    mainForm.LogMessage($"MES로 전송: {orr.OPID}, {orr.ModelID}");
                }
                catch (SocketException ex)
                {
                    mainForm.LogMessage($"MES로 전송 실패: {ex.Message}");
                    MessageBox.Show("MES와의 통신 중 오류가 발생했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                mainForm.LogMessage("MES와의 연결이 끊어져 전송할 수 없습니다.");
                MessageBox.Show("MES 연결 상태를 확인하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // 설정이 완료되면 폼을 닫음
            this.Close();
        }
    }
}
