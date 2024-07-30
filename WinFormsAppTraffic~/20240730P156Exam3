namespace _20240730_WinFormsAppTrafficLights03
{
    public partial class Form1 : Form
    {
        private int Shinhodoong_Color = 1;
        public Form1()
        {
            InitializeComponent();
        }
        public void ChangeTrafficSign(int Color)
        {
            switch (Color)
            {
                case 0:
                    pictureBox1.Image = Image.FromFile(System.Environment.CurrentDirectory
                    + "/신호등(준비중).png");
                    break;
                case 1:
                    pictureBox1.Image = Image.FromFile(System.Environment.CurrentDirectory
                    + "/신호등(빨간색).png");
                    break;
                case 2:
                    pictureBox1.Image = Image.FromFile(System.Environment.CurrentDirectory
                + "/신호등(노란색).png");
                    break;
                case 3:
                    pictureBox1.Image = Image.FromFile(System.Environment.CurrentDirectory
                + "/신호등(녹색).png");
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ChangeTrafficSign(Shinhodoong_Color);
            Shinhodoong_Color++;
            if (Shinhodoong_Color >= 4) Shinhodoong_Color = 1;
        }
    }
}
