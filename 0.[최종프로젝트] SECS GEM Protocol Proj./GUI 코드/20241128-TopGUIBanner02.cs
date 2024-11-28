private void labelBanner_Resize(object sender, EventArgs e)
{
    // 라벨 크기는 부모 컨트롤 크기를 기반으로 조정
    if (labelBanner.Parent != null)
    {
        int newWidth = (int)(labelBanner.Parent.Width * 0.75);
        int newHeight = labelBanner.Parent.Height;

        // 크기 변경
        labelBanner.Size = new Size(newWidth, newHeight);

        // 강제 다시 그리기
        labelBanner.Invalidate();
    }
}

private void labelBanner_Paint(object sender, PaintEventArgs e)
{
    // 그라데이션 시작 색상과 끝 색상을 설정
    Color startColor = Color.FromArgb(244, 198, 107);
    Color endColor = Color.FromArgb(214, 106, 36);

    // Panel의 클라이언트 영역
    Rectangle panelRect = labelBanner.ClientRectangle;

    // 둥근 모서리를 위한 반지름 설정
    int cornerRadius = 5;

    // 둥근 사각형 생성
    using (GraphicsPath path = new GraphicsPath())
    {
        int diameter = cornerRadius * 2;
        path.StartFigure();
        path.AddArc(panelRect.Left, panelRect.Top, diameter, diameter, 180, 90);
        path.AddArc(panelRect.Right - diameter, panelRect.Top, diameter, diameter, 270, 90);
        path.AddArc(panelRect.Right - diameter, panelRect.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(panelRect.Left, panelRect.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();

        // 그라데이션 효과
        using (LinearGradientBrush brush = new LinearGradientBrush(panelRect, startColor, endColor, LinearGradientMode.Horizontal))
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillPath(brush, path);
        }

        // 둥근 영역 설정
        labelBanner.Region = new Region(path);
    }

    // 텍스트 그리기
    string text = "EDS CIM PC";
    Font font = new Font("한컴 고딕", 25, FontStyle.Bold);
    using (Brush textBrush = new SolidBrush(Color.White))
    {
        // 텍스트를 중앙에 배치
        SizeF textSize = e.Graphics.MeasureString(text, font);
        PointF textPosition = new PointF(
            (panelRect.Width - textSize.Width) / 2,
            (panelRect.Height - textSize.Height) / 2
        );
        e.Graphics.DrawString(text, font, textBrush, textPosition);
    }
}
