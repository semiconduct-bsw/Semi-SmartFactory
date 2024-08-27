using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20240730_WinFormsAppChap10App01
{
    public partial class Form5 : Form
    {
        private int Sajin_1 = 1;
        private int Sajin_2 = 1;
        private int Sajin_3 = 1;
        private int Sajin_1_Max = 4;
        private int Sajin_2_Max = 5;
        private int Sajin_3_Max = 7;
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Interval = 500;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Interval = 200;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Interval = 10;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            timer1.Interval = hScrollBar1.Value;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(System.Environment.CurrentDirectory
                + "/재롱피우는 오버액션토끼/" + Sajin_1 + ".jpg");
            pictureBox2.Image = Image.FromFile(System.Environment.CurrentDirectory
                + "/다가오는 코끼리 두마리/" + Sajin_2 + ".jpg");
            pictureBox3.Image = Image.FromFile(System.Environment.CurrentDirectory
                + "/돌아서는 신랑신부/" + Sajin_3 + ".jpg");

            Sajin_1++;
            Sajin_2++;
            Sajin_3++;
            if (Sajin_1 > Sajin_1_Max) Sajin_1 = 1;
            if (Sajin_2 > Sajin_2_Max) Sajin_2 = 1;
            if (Sajin_3 > Sajin_3_Max) Sajin_3 = 1;
        }
    }
}
