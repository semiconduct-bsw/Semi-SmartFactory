private void ResizeLabelFont(Label label)
{
    float newFontSize = Math.Max(label.Height * 0.4f, 10f); // 최소 폰트 크기는 10
    label.Font = new Font("한컴 고딕", newFontSize, FontStyle.Bold);
}

private void labelBanner_Resize(object sender, EventArgs e)
{
    // 라벨 크기는 부모 컨트롤 크기를 기반으로 조정
    if (labelBanner.Parent != null)
    {
        int newWidth = (int)(labelBanner.Parent.Width * 0.75);
        int newHeight = labelBanner.Parent.Height;

        // 크기 변경
        labelBanner.Size = new Size(newWidth, newHeight);

        // 폰트 크기 동적 조정
        ResizeLabelFont(labelBanner);

        // 강제 다시 그리기
        labelBanner.Invalidate();
    }
}

private void lbPLCConnect_Resize(object sender, EventArgs e)
{
    // 라벨 크기는 부모 컨트롤 크기를 기반으로 조정
    if (lbPLCConnect.Parent != null)
    {
        int newWidth = (int)(lbPLCConnect.Parent.Width * 0.25);
        int newHeight = lbPLCConnect.Parent.Height / 2;

        // 크기 변경
        lbPLCConnect.Size = new Size(newWidth, newHeight);

        // 폰트 크기 동적 조정
        ResizeLabelFont(lbPLCConnect);

        // 강제 다시 그리기
        lbPLCConnect.Invalidate();
    }
}

private void lbMESConnect_Resize(object sender, EventArgs e)
{
    // 라벨 크기는 부모 컨트롤 크기를 기반으로 조정
    if (lbMESConnect.Parent != null)
    {
        int newWidth = (int)(lbMESConnect.Parent.Width * 0.25);
        int newHeight = lbMESConnect.Parent.Height / 2;

        // 크기 변경
        lbMESConnect.Size = new Size(newWidth, newHeight);

        // 폰트 크기 동적 조정
        ResizeLabelFont(lbMESConnect);

        // 강제 다시 그리기
        lbMESConnect.Invalidate();
    }
}
