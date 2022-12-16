using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tanks
{
    class ComputerTank : Tank // класс для вражеских танков
    {
        private int delta = 0; // отслеживает время, после которого надо сменить направление движения        
        public int restoreTime { get; set; } // время возраждения танка
        public bool isStoped { get; set; } // отслеживает, остановлен ли танк
        public Point restoreCoord { get; set; } // координаты для появления танка
       
        private void Rotate() // поворот танка в определнный промежуток времени
        {
            Random rand = new Random();
            if (delta > 300)
            {
                Mode = rand.Next(1, 5);
                delta = 0;
            }
        }
        public void Movement(PictureBox pic, Control.ControlCollection forms) // алгоритм движения танков при их столкновении
        {
            int Right = 10000;
            int Left = 10000;
            int Up = 10000;
            int Down = 10000;
            int counter = 0;
            foreach (Control con in forms)
            {
                if (con is PictureBox)
                {
                    if (((PictureBox)con) != pic)
                    {
                        while (!((PictureBox)con).Bounds.IntersectsWith(new Rectangle(pic.Location.X + counter, pic.Location.Y, 60, 60)) && isValidMove(new Tuple<int, int>(pic.Location.X + counter, pic.Location.Y)))
                            counter += 2;
                        if (counter < Right && counter != 0)
                        {
                            Right = counter;
                            counter = 0;
                        }
                        else
                            counter = 0;
                        while (!((PictureBox)con).Bounds.IntersectsWith(new Rectangle(pic.Location.X - counter, pic.Location.Y, 60, 60)) && isValidMove(new Tuple<int, int>(pic.Location.X - counter, pic.Location.Y)))
                            counter += 2;
                        if (counter < Left && counter != 0)
                        {
                            Left = counter;
                            counter = 0;
                        }
                        else
                            counter = 0;
                        while (!((PictureBox)con).Bounds.IntersectsWith(new Rectangle(pic.Location.X, pic.Location.Y - counter, 60, 60)) && isValidMove(new Tuple<int, int>(pic.Location.X, pic.Location.Y - counter)))
                            counter += 2;
                        if (counter < Up && counter != 0)
                        {
                            Up = counter;
                            counter = 0;
                        }
                        else
                            counter = 0;
                        while (!((PictureBox)con).Bounds.IntersectsWith(new Rectangle(pic.Location.X, pic.Location.Y + counter, 60, 60)) && isValidMove(new Tuple<int, int>(pic.Location.X, pic.Location.Y + counter)))
                            counter += 2;
                        if (counter < Down && counter != 0)
                        {
                            Down = counter;
                            counter = 0;
                        }
                        else
                            counter = 0;
                    }
                }
            }
            List<int> search = new List<int>();
            search.Add(Right);
            search.Add(Left);
            search.Add(Up);
            search.Add(Down);
            if (Right == search.Max())
            {
                Mode = 1;
                delta = 0;
            }
            else if (Left == search.Max())
            {
                Mode = 2;
                delta = 0;
            }
            else if (Up == search.Max())
            {
                Mode = 3;
                delta = 0;
            }
            else
            {
                Mode = 4;
                delta = 0;
            }
            isStoped = false;
        }
        public void Movement(PictureBox pic) // алгоритм движения танков
        {
            if (!isStoped && !isDead)
            {
                Speed = 2;
                if (Mode == 1)
                    pic.Location = new Point(pic.Location.X + Speed, pic.Location.Y);
                else if (Mode == 2)
                    pic.Location = new Point(pic.Location.X - Speed, pic.Location.Y);
                else if (Mode == 3)
                    pic.Location = new Point(pic.Location.X, pic.Location.Y - Speed);
                else if (Mode == 4)
                    pic.Location = new Point(pic.Location.X, pic.Location.Y + Speed);
            }
        }
        private void Restore() // возраждения танков
        {
            if (!isDead)
            {
                restoreTime = 0;
            }
            else
            {
                restoreTime++;
                if (restoreTime > 400)
                {
                    coordinates = new Tuple<int, int>(1, 1);
                    isDead = false;
                }
            }
        }
        public override void DrawItem(Graphics item, PictureBox pic) // отрисовка танка, если он живой
        {
            delta++;
            Rotate();
            if (bullet.coordinates.Item1 < 0)
            {
                if (bullet.reload > 100)
                {
                    bullet.Spawn(GetDirection(), GetFirePosition(pic.Location, pic.Width));
                    bullet.reload = 0;
                }
                else
                    bullet.Reloading();
            }
            else
            {
                bullet.Reloading();
                bullet.DrawItem(item, pic);
                bullet.Movement();
            }
            if (restoreTime > 0 && !isDead)
            {
                pic.Location = restoreCoord;
                restoreTime = 0;
            }
            if (isStoped)
                Speed = 0;
            else
                Speed = 2;
            if (pic.Location == new Point(-60, -60))
            {
                isDead = true;
                Restore();
                return;
            }
            coordinates = new Tuple<int, int>(pic.Location.X, pic.Location.Y);
            if (isDead)
                Restore();
            Bitmap img = new Bitmap("CompTankUp.png");
            if (Mode == 1)
            {
                img = new Bitmap("CompTankRight.png");
                NextRadius = new Rectangle(pic.Location.X + 7, pic.Location.Y, 60, 60);
            }
            else if (Mode == 2)
            {
                img = new Bitmap("CompTankLeft.png");
                NextRadius = new Rectangle(pic.Location.X - 7, pic.Location.Y, 60, 60);
            }
            else if (Mode == 3)
            {
                img = new Bitmap("CompTankUp.png");
                NextRadius = new Rectangle(pic.Location.X, pic.Location.Y - 7, 60, 60);
            }
            else if (Mode == 4)
            {
                img = new Bitmap("CompTankDown.png");
                NextRadius = new Rectangle(pic.Location.X, pic.Location.Y + 7, 60, 60);
            }
            else
                return;
            if (coordinates != null)
            {
                if (!isValidMove(coordinates))
                {
                    int returnValue;
                    if (coordinates.Item1 > 1120)
                    {
                        returnValue = coordinates.Item2;
                        pic.Location = new Point(1120, returnValue);
                        isStoped = true;
                    }
                    else if (coordinates.Item2 > 700)
                    {
                        returnValue = coordinates.Item1;
                        pic.Location = new Point(returnValue, 700);
                        isStoped = true;
                    }
                    else if (coordinates.Item1 < 0)
                    {
                        returnValue = coordinates.Item2;
                        pic.Location = new Point(1, returnValue);
                        isStoped = true;
                    }
                    else if (coordinates.Item2 < 0)
                    {
                        returnValue = coordinates.Item1;
                        pic.Location = new Point(returnValue, 1);
                        isStoped = true;
                    }
                }
            }
            pic.Size = new Size(width, height);
            Radius = new Rectangle(pic.Location.X, pic.Location.Y, 60, 60);
            pic.Image = img;
            pic.BackColor = Color.Transparent;
        }
    }
}