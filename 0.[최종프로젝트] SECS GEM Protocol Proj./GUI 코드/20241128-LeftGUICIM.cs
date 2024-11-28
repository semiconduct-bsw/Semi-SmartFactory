#region GUI All
private void pictureBox2_Paint(object sender, PaintEventArgs e)
{
    // 텍스트 스타일 설정
    string text = "Open LogBox";
    float baseFontSize = 16; // 기본 글씨 크기
    float scaleFactor = Math.Min((float)pictureBox2.Width / 200, (float)pictureBox2.Height / 100); // 비율 계산
    float scaledFontSize = baseFontSize * scaleFactor; // 비율에 따른 동적 글씨 크기

    Font font = new Font("한컴 고딕", scaledFontSize, FontStyle.Bold);
    Color textColor = Color.White;

    // 텍스트 크기 측정
    SizeF textSize = e.Graphics.MeasureString(text, font);

    // 텍스트 위치 계산 (X축 가운데 정렬)
    float x = (pictureBox2.Width - textSize.Width) / 2; // 가운데 정렬
    float y = (pictureBox2.Height - textSize.Height - 10); // 하단에서 약간 위로

    // 텍스트 그리기
    e.Graphics.DrawString(text, font, new SolidBrush(textColor), new PointF(x, y));
}

private void pictureBox2_Resize(object sender, EventArgs e)
{
    pictureBox2.Invalidate(); // PictureBox 다시 그리기 요청
}

private void pictureBox3_Paint(object sender, PaintEventArgs e)
{
    // 텍스트 스타일 설정
    string text = "Online Remote Report";
    float baseFontSize = 14; // 기본 글씨 크기
    float scaleFactor = Math.Min((float)pictureBox3.Width / 200, (float)pictureBox3.Height / 100); // 비율 계산
    float scaledFontSize = baseFontSize * scaleFactor; // 비율에 따른 동적 글씨 크기

    Font font = new Font("한컴 고딕", scaledFontSize, FontStyle.Bold);
    Color textColor = Color.White;

    // 텍스트 크기 측정
    SizeF textSize = e.Graphics.MeasureString(text, font);

    // 텍스트 위치 계산 (X축 가운데 정렬)
    float x = (pictureBox3.Width - textSize.Width) / 2; // 가운데 정렬
    float y = (pictureBox3.Height - textSize.Height - 10); // 하단에서 약간 위로

    // 텍스트 그리기
    e.Graphics.DrawString(text, font, new SolidBrush(textColor), new PointF(x, y));
}

private void pictureBox3_Resize(object sender, EventArgs e)
{
    pictureBox3.Invalidate();
}

private void pictureBox4_Paint(object sender, PaintEventArgs e)
{
    // 텍스트 스타일 설정
    string text = "Model List";
    float baseFontSize = 15; // 기본 글씨 크기
    float scaleFactor = Math.Min((float)pictureBox4.Width / 200, (float)pictureBox4.Height / 100); // 비율 계산
    float scaledFontSize = baseFontSize * scaleFactor; // 비율에 따른 동적 글씨 크기

    Font font = new Font("한컴 고딕", scaledFontSize, FontStyle.Bold);
    Color textColor = Color.White;

    // 텍스트 크기 측정
    SizeF textSize = e.Graphics.MeasureString(text, font);

    // 텍스트 위치 계산 (X축 가운데 정렬)
    float x = (pictureBox4.Width - textSize.Width) / 2; // 가운데 정렬
    float y = (pictureBox4.Height - textSize.Height - 10); // 하단에서 약간 위로

    // 텍스트 그리기
    e.Graphics.DrawString(text, font, new SolidBrush(textColor), new PointF(x, y));
}

private void pictureBox4_Resize(object sender, EventArgs e)
{
    pictureBox4.Invalidate();
}
#endregion
