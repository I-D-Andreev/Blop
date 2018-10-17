using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blop.Properties;

namespace Blop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Bitmap bmp = new Bitmap(new Bitmap("panda.png"), 50, 50);
            this.Cursor = new Cursor(bmp.GetHicon());
            label4.Text = time.ToString();

            
            label6.Text = MostCarrots.ToString();
            
        }

        Random r = new Random();
        int radius = 25, carrotRadius=15;
        int moveSpeed=5;
        int carrots = 0;
        int time = 30;
        int MostCarrots = (int)Settings.Default["Record"];
        // picturebox1=Blop, picturebox6=carrot

        void Start()
        {
            carrots = 0;
            label1.Text = carrots.ToString();

            l1.Visible = false;
            pictureBox1.Visible = true;
            button1.Visible = false;
            timer1.Start();
            timer2.Start();
            pictureBox1.Location = new Point(r.Next(10, this.ClientSize.Width - 61), r.Next(10, this.ClientSize.Height - 61));
            pictureBox6.Location = new Point(r.Next(10, this.ClientSize.Width - 61), r.Next(10, this.ClientSize.Height - 61)); 
           
        }


        Label l1 = new Label();

        void BEnd()
        {
            l1.Visible = true;
            l1.Text = "Game Over";
            l1.Size = new System.Drawing.Size(500, 500);
            l1.Font = new Font("Arial", 50);
            l1.Parent = this;
            l1.Location = new System.Drawing.Point(this.ClientSize.Width / 2 -200, this.ClientSize.Height / 2 -50);

            if (MostCarrots < carrots)
            {
                MostCarrots = carrots;
                label6.Text = MostCarrots.ToString();
                Settings.Default["Record"] = MostCarrots;
                Settings.Default.Save();
            }

            timer1.Stop();
            timer2.Stop();
            button1.Visible = true;

        }

        bool Endanger()
        {
            double BlopCenterX = pictureBox1.Location.X + radius;
            double BlopCenterY = pictureBox1.Location.Y + radius;

            double mouseLocationX = this.PointToClient(Cursor.Position).X;
            double mouseLocationY = this.PointToClient(Cursor.Position).Y;

            if (Math.Sqrt((BlopCenterX - mouseLocationX) * (BlopCenterX - mouseLocationX) + (BlopCenterY - mouseLocationY) * (BlopCenterY - mouseLocationY)) < 3*radius)
            {
                return true;
            }
            else
                return false;
        }

        void BMove()
        {
            double BlopCenterX = pictureBox1.Location.X + pictureBox1.Width/2;
            double BlopCenterY = pictureBox1.Location.Y + pictureBox1.Height/2;

            double mouseLocationX = this.PointToClient(Cursor.Position).X;
            double mouseLocationY = this.PointToClient(Cursor.Position).Y;

            double toMoveX = BlopCenterX - mouseLocationX;
            double toMoveY = BlopCenterY - mouseLocationY;

            if (toMoveX == 0) toMoveX = 1;
            if (toMoveY == 0) toMoveY = 1;


            if (radioButton1.Checked == true) // Easy move + diagonal
            {

                toMoveX = toMoveX / Math.Abs(toMoveX) * 1.5 * moveSpeed;
                toMoveY = toMoveY / Math.Abs(toMoveY) * 1.5 * moveSpeed;

            }
            else // hard movement
            {
                double absToMoveX = Math.Abs(toMoveX);
                double absToMoveY = Math.Abs(toMoveY);

                toMoveX = toMoveX / absToMoveX * (absToMoveX / (absToMoveX + absToMoveY)) * 2 * moveSpeed;
                toMoveY = toMoveY / absToMoveY * (absToMoveY / (absToMoveX + absToMoveY)) * 2 * moveSpeed;
                
            }

            pictureBox1.Location = new Point(pictureBox1.Location.X + (int)toMoveX, pictureBox1.Location.Y + (int)toMoveY);
        }

        bool CheckBorders()
        {
            if (pictureBox1.Location.X <= 10 || pictureBox1.Location.X >= this.ClientSize.Width - 61 ||
               pictureBox1.Location.Y <= 10 || pictureBox1.Location.Y >= this.ClientSize.Height - 61)
            {
                return true;
            }
            else
                return false;
        }

        bool GetCarrot()
        {
            double CarrotCenterX=pictureBox6.Location.X+pictureBox6.Width/2;
            double CarrotCenterY = pictureBox6.Location.Y + pictureBox6.Height / 2;

            double BlopCenterX = pictureBox1.Location.X + pictureBox1.Width / 2;
            double BlopCenterY = pictureBox1.Location.Y + pictureBox1.Height / 2;

            if (Math.Sqrt((CarrotCenterX - BlopCenterX) * (CarrotCenterX - BlopCenterX) + (CarrotCenterY - BlopCenterY) * (CarrotCenterY - BlopCenterY)) < radius+carrotRadius)
                return true;
            else
                return false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Endanger())
            {
                BMove();
            }

            if (GetCarrot())
            {
                carrots++; // easy
                if (radioButton2.Checked == true)
                    carrots++;
                // hard gets 2*carrots

                time += 3;
                label4.Text = time.ToString();

                label1.Text = carrots.ToString();
                pictureBox6.Location = new Point(r.Next(10, this.ClientSize.Width - 50), r.Next(10, this.ClientSize.Height - 61));
            }

            if (CheckBorders())
            {
                BEnd();
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            time--;
            label4.Text = time.ToString();
            if (time <= 0)
            {
                BEnd();
            }

        }
    }
}

/*
            solution explorer - > properties - > settings.settings
            Settings.Default["Record"] = "0";
            Settings.Default.Save();

*/