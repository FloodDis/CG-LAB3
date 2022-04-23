using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace LAB3
{

    public partial class Form1 : Form
    {
        // концы отрезка
        public int xn;
        public int yn;
        public int xk;
        public int yk;
        public int xl;
        public int yl;
        public int xr;
        public int yr;
        Bitmap mybitmap; // объект Bitmap для вывода отрезка
        Color current_color_line; // текущий цвет отрезка
        Color current_color_fill; // текущий цвет заливки
        Graphics _graphics;
        Pen _pen = new Pen(Color.Black, 1);
        static int MAX_GOR = 308;
        static int MAX_VER = 333;

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult D = colorDialog1.ShowDialog();
            if (D == System.Windows.Forms.DialogResult.OK)
            {
                current_color_line = colorDialog1.Color;
                _pen.Color = colorDialog1.Color;
            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            textBox1.Text = e.X.ToString();
            textBox2.Text = e.Y.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult D = colorDialog1.ShowDialog();
            if (D == System.Windows.Forms.DialogResult.OK)
            {
                current_color_fill = colorDialog1.Color;
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            xn = e.X;
            yn = e.Y;

            xr = e.X;
            yr = e.Y;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            xl = e.X;
            yl = e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                int i;
                int n;
                double xt;
                double yt;
                double dx;
                double dy;
                xk = 0;
                yk = 0;
                xk = e.X;
                yk = e.Y;

                dx = xk - xl;
                dy = yk - yl;
                n = (int)Math.Sqrt(dx * dx + dy * dy);
                xt = xl;
                yt = yl;

                for (i = 1; i <= n; i++)
                {
                    _graphics.DrawRectangle(_pen, (int)xt, (int)yt, 2, 2);
                    xt = xt + dx / n;
                    yt = yt + dy / n;
                }
            }
        }
        private void CDA(int x1, int y1, int x2, int y2)
        {
            int i, n;
            double xt, yt, dx, dy;

            xn = x1;
            yn = y1;
            xk = x2;
            yk = y2;
            dx = xk - xn;
            dy = yk - yn;
            n = (int)Math.Sqrt(dx * dx + dy * dy);
            xt = xn;
            yt = yn;
            for (i = 1; i <= n; i++)
            {
                mybitmap.SetPixel((int)xt, (int)yt, current_color_line);
                xt = xt + dx / n;
                yt = yt + dy / n;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            mybitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (_graphics = Graphics.FromHwnd(pictureBox1.Handle))
            {
                if (radioButton1.Checked == true)
                {
                    //рисуем прямоугольник
                    CDA(10, 10, 10, 110);
                    CDA(10, 10, 110, 10);
                    CDA(10, 110, 110, 110);
                    CDA(110, 10, 110, 110);
                    //рисуем треугольник
                    CDA(150, 10, 150, 150);
                    CDA(250, 50, 150, 150);
                    CDA(150, 10, 250, 90);
                }
                else if (radioButton2.Checked == true)
                {
                    //получаем растр созданного рисунка в mybitmap
                    mybitmap = pictureBox1.Image as Bitmap;
                    Zaliv(xn, yn);
                }
                else if (radioButton3.Checked == true)
                {
                    CDA(10, 10, 10, 100);
                    CDA(10, 100, 50, 170);
                    CDA(50, 170, 200, 250);
                    CDA(200, 250, 10, 10);
                }
                else if (radioButton5.Checked == true)
                {
                    mybitmap = pictureBox1.Image as Bitmap;
                    Stack<Point> points = new Stack<Point>(64);
                    points.Push(new Point(xr, yr));
                    while (points.Count() != 0)
                    {
                        Point currentPoint = points.Pop();
                        mybitmap.SetPixel(currentPoint.X, currentPoint.Y,
                            current_color_fill);
                        if (currentPoint.X >= 0
                            && ++currentPoint.X < MAX_GOR
                            && currentPoint.Y >= 0
                            && currentPoint.Y < MAX_VER
                            && mybitmap.GetPixel(currentPoint.X, currentPoint.Y)
                            != current_color_fill
                            && mybitmap.GetPixel(currentPoint.X, currentPoint.Y)
                            != current_color_line)
                        {
                            points.Push(currentPoint);
                        }
                        if (--currentPoint.X < MAX_GOR
                            && currentPoint.X >= 0
                            && ++currentPoint.Y < MAX_VER
                            && currentPoint.Y >= 0
                            && mybitmap.GetPixel(currentPoint.X, currentPoint.Y)
                            != current_color_fill
                            && mybitmap.GetPixel(currentPoint.X, currentPoint.Y)
                            != current_color_line)
                        {
                            points.Push(currentPoint);
                        }
                        if (--currentPoint.X < MAX_GOR
                            && currentPoint.X >= 0
                            && --currentPoint.Y < MAX_VER
                            && currentPoint.Y >= 0
                            && mybitmap.GetPixel(currentPoint.X, currentPoint.Y)
                            != current_color_fill
                            && mybitmap.GetPixel(currentPoint.X, currentPoint.Y)
                            != current_color_line)
                        {
                            points.Push(currentPoint);
                        }
                        if (currentPoint.X >= 0
                            && ++currentPoint.X < MAX_GOR
                            && --currentPoint.Y < MAX_VER
                            && currentPoint.Y >= 0
                            && mybitmap.GetPixel(currentPoint.X, currentPoint.Y)
                            != current_color_fill
                            && mybitmap.GetPixel(currentPoint.X, currentPoint.Y)
                            != current_color_line)
                        {
                            points.Push(currentPoint);
                        }
                    }
                }
                //передаем полученный растр mybitmap в элемент pictureBox
                pictureBox1.Image = mybitmap;
                // обновляем pictureBox и активируем кнопки
                pictureBox1.Refresh();
                button1.Enabled = true;
                button2.Enabled = true;
            }
        }

        // Заливка с затравкой (рекурсивная)
        private void Zaliv(int x1, int y1)
        {
            if (x1 < 0 || x1 >= 308 || y1 < 0 || y1 >= 333)
            {
                return;
            }
            Color old_color = mybitmap.GetPixel(x1, y1);
            if ((old_color.ToArgb() != current_color_line.ToArgb()) &&
           (old_color.ToArgb() != current_color_fill.ToArgb()))
            {
                mybitmap.SetPixel(x1, y1, current_color_fill);

                Zaliv(x1 + 1, y1);
                Zaliv(x1 - 1, y1);
                Zaliv(x1, y1 + 1);
                Zaliv(x1, y1 - 1);
            }
            else
            {
                return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _graphics = Graphics.FromHwnd(pictureBox1.Handle);
        }
    }
}