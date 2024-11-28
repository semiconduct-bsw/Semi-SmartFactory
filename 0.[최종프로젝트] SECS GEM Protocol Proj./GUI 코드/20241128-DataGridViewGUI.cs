private void Form1_Load(object sender, EventArgs e)
{
    ShowDataGridView();

    //InitializeTimers();

    //// 장비 PC 로부터 메시지를 수신 시도하고, 메시지가 없으면 재연결 시도하는 쓰레드.
    //receiveFlag_FromEqp = true;
    //receiveThread_FromEqp = new Thread(receive_FromEqp);
    //receiveThread_FromEqp.Start();

    //// 장비 PC 로부터 메시지를 수신 시도하고, 메시지가 없으면 재연결 시도하는 쓰레드.
    //receiveFlag_FromMES = true;
    //receiveThread_FromMES = new Thread(receive_FromMES);
    //receiveThread_FromMES.Start();
}

private void ShowDataGridView()
{
    // Panel에 있는 기존 컨트롤 제거
    panelMain.Controls.Clear();

    // 새로운 DataGridView 생성
    DataGridView dgvCIM = new DataGridView
    {
        Dock = DockStyle.Fill, // Panel 내부를 꽉 채우도록 설정
        BackgroundColor = Color.White,
        AllowUserToAddRows = false,
        ReadOnly = true,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        ColumnCount = 7, // 열 개수 설정
        EnableHeadersVisualStyles = false // 헤더 스타일 강제 적용
    };

    // 열 이름 설정
    dgvCIM.Columns[0].Name = "ReportID";
    dgvCIM.Columns[1].Name = "ModelID";
    dgvCIM.Columns[2].Name = "OPID";
    dgvCIM.Columns[3].Name = "ProcID";
    dgvCIM.Columns[4].Name = "MaterialID";
    dgvCIM.Columns[5].Name = "LotID";
    dgvCIM.Columns[6].Name = "Cancel Reason";

    #region DataGridView Style
    // DataGridView 헤더 스타일
    dgvCIM.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(224, 159, 57); // 헤더
    dgvCIM.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // 헤더 텍스트 색상
    dgvCIM.ColumnHeadersDefaultCellStyle.Font = new Font("한컴 고딕", 10F, FontStyle.Bold); // 헤더 폰트

    // DataGridView 교차 행 색상
    dgvCIM.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 222, 123); // 교차 색상

    // DataGridView 선택된 셀 배경색
    dgvCIM.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 73, 94); // 진한 회색
    dgvCIM.DefaultCellStyle.Font = new Font("한컴 고딕", 9F, FontStyle.Regular); // 셀 폰트
    #endregion

    // Panel에 추가
    panelMain.Controls.Add(dgvCIM);
}

private void pictureBox1_Click(object sender, EventArgs e)
{
    ShowDataGridView();
}
