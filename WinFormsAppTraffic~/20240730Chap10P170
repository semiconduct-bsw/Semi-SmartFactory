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
    public partial class Form6 : Form
    {
        private int Sajin_1 = 1;
        private int Sajin_2 = 1;
        private int Sajin_3 = 1;
        private int Sajin_1_Max = 4;
        private int Sajin_2_Max = 5;
        private int Sajin_3_Max = 7;
        public Form6()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(System.Environment.CurrentDirectory
                + "/재롱피우는 오버액션토끼/" + Sajin_1 + ".jpg");
            Sajin_1++;
            if (Sajin_1 > Sajin_1_Max) Sajin_1 = 1;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            pictureBox2.Image = Image.FromFile(System.Environment.CurrentDirectory
                + "/다가오는 코끼리 두마리/" + Sajin_2 + ".jpg");
            Sajin_2++;
            if (Sajin_2 > Sajin_2_Max) Sajin_2 = 1;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            pictureBox3.Image = Image.FromFile(System.Environment.CurrentDirectory
                + "/돌아서는 신랑신부/" + Sajin_3 + ".jpg");
            Sajin_3++;
            if (Sajin_3 > Sajin_3_Max) Sajin_3 = 1;
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void 빠르게ToolStripMenuItem_Click(object sender, EventArgs e)
        {   // 전체 빠르게
            timer1.Interval = 10; timer2.Interval = 10; timer3.Interval = 10;
        }

        private void 중간ToolStripMenuItem_Click(object sender, EventArgs e)
        {   // 전체 중간
            timer1.Interval = 500; timer2.Interval = 500; timer3.Interval = 500;
        }

        private void 느리게ToolStripMenuItem_Click(object sender, EventArgs e)
        {   // 전체 느리게
            timer1.Interval = 1000; timer2.Interval = 1000; timer3.Interval = 1000;
        }

        private void 빠르게ToolStripMenuItem1_Click(object sender, EventArgs e)
        {   // 재롱피우는 오버액션토끼만 빠르게
            timer1.Interval = 10;
        }

        private void 중간ToolStripMenuItem1_Click(object sender, EventArgs e)
        {   // 재롱피우는 오버액션토끼만 중간
            timer1.Interval = 500;
        }

        private void 느리게ToolStripMenuItem1_Click(object sender, EventArgs e)
        {   // 재롱피우는 오버액션토끼만 느리게
            timer1.Interval = 1000;
        }

        private void 빠르게ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            timer2.Interval = 10;
        }

        private void 중간ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            timer2.Interval = 500;
        }

        private void 느리게ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            timer2.Interval = 1000;
        }

        private void 빠르게ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            timer3.Interval = 10;
        }

        private void 중간ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            timer3.Interval = 500;
        }

        private void 느리게ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            timer3.Interval = 1000;
        }

        private void 전체중지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true && timer2.Enabled == true && timer3.Enabled == true)
            {
                timer1.Enabled = false; timer2.Enabled = false; timer3.Enabled = false;
                전체중지ToolStripMenuItem.Text = "전체 동작";
            }
            else if (timer1.Enabled == false && timer2.Enabled == false && timer3.Enabled == false)
            {
                timer1.Enabled = true; timer2.Enabled = true; timer3.Enabled = true;
                전체중지ToolStripMenuItem.Text = "전체 중지";
            }
        }

        private void 개발환경ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("운영 체제 : Windows 10\n개발 도구 : Microsoft Visual Studio" +
                " Community 2022\n개발 언어 : C#", "[ 개발 환경 ]");
        }
    }
}
