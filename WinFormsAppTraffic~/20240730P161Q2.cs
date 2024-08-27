private int direction = 1;

private void timer1_Tick(object sender, EventArgs e)
{
    ChangeTrafficSign(Shinhodoong_Color);
    Shinhodoong_Color += direction;

    if (Shinhodoong_Color >= 4) { direction = -1; }

    else if (Shinhodoong_Color <= 1) { direction = 1; }
}
