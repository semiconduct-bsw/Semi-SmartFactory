using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20241014_Size01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Size_ 기본 선언
            Size sz1 = new Size(100, 200);
            Size2f sz2 = new Size2f(192.3f, 25.3f);
            Size2d sz3 = new Size2d(100.2, 30.9);

            Size sz4 = new Size(120, 69);
            Size2f sz5 = new Size2f(0.3f, 0.0f);
            Size2d sz6 = new Size(0.25, 0.6);

            Point2d pt1 = new Point2d(0.25, 0.6);

            // Size sz7 = sz1 + (Size)sz2; 는 Error!
            Size sz7 = new Size(
                sz1.Width + (int)sz2.Width,
                sz2.Height + (int)sz2.Height);

            Size2d sz8 = new Size2d(
                sz3.Width - (double)sz4.Width,
                sz3.Height - (double)sz4.Height);

            // 출력
            Console.Write($"sz1.Width = {sz1.Width},  ");
            Console.WriteLine($"sz2.Height = {sz2.Height}");
            Console.WriteLine($"넓이 : {sz1.Width * sz1.Height}");

            Console.WriteLine($"[sz7] : {sz7},  [sz8] : {sz8}");
        }
    }
}
