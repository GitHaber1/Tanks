using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfTanks
{
    public class ComputerTank : Tank
    {
        private int delta = 0;
        public int TimeToRotate { get; set; }
        public int restoreTime { get; set; }
        public bool isStoped { get; set; }
        public Point restoreCoord { get; set; }
        private void Rotate()
        {
            Random rand = new Random();
            if (delta > 300)
            {
                Mode = rand.Next(1, 5);
                delta = 0;
            }
        }
        public void Movement(Rectangle pic, UIElementCollection forms)
        {           
            int Right = 10000;
            int Left = 10000;
            int Up = 10000;
            int Down = 10000;
            int counter = 0;
            foreach (UIElement con in forms)
            {
                if (con is Rectangle)
                {
                    Rect collision = new Rect(Canvas.GetLeft(con), Canvas.GetTop(con), ((Rectangle)con).ActualWidth, ((Rectangle)con).ActualHeight);
                    if (((Rectangle)con)!= pic)
                    {                        
                        while (!collision.IntersectsWith(new Rect(Canvas.GetLeft(pic) + counter, Canvas.GetTop(pic), 60, 60)) && isValidMove(new Tuple<int, int>((int)(Canvas.GetLeft(pic) + counter), (int)Canvas.GetTop(pic))))
                            counter += 2;
                        if (counter < Right && counter != 0)
                        {
                            Right = counter;
                            counter = 0;
                        }
                        else
                            counter = 0;
                        while (!collision.IntersectsWith(new Rect(Canvas.GetLeft(pic) - counter, Canvas.GetTop(pic), 60, 60)) && isValidMove(new Tuple<int, int>((int)(Canvas.GetLeft(pic) - counter), (int)Canvas.GetTop(pic))))
                            counter += 2;
                        if (counter < Left && counter != 0)
                        {
                            Left = counter;
                            counter = 0;
                        }
                        else
                            counter = 0;
                        while (!collision.IntersectsWith(new Rect(Canvas.GetLeft(pic), Canvas.GetTop(pic) - counter, 60, 60)) && isValidMove(new Tuple<int, int>((int)Canvas.GetLeft(pic), (int)(Canvas.GetTop(pic) - counter))))
                            counter += 2;
                        if (counter < Up && counter != 0)
                        {
                            Up = counter;
                            counter = 0;
                        }
                        else
                            counter = 0;
                        while (!collision.IntersectsWith(new Rect(Canvas.GetLeft(pic), Canvas.GetTop(pic) + counter, 60, 60)) && isValidMove(new Tuple<int, int>((int)Canvas.GetLeft(pic), (int)(Canvas.GetTop(pic) + counter))))
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
        public void Movement(Rectangle pic)
        {
            if (!isStoped && !isDead)
            {
                Speed = 2;
                if (Mode == 1)
                    Canvas.SetLeft(pic, Canvas.GetLeft(pic) + Speed);
                else if (Mode == 2)
                    Canvas.SetLeft(pic, Canvas.GetLeft(pic) - Speed);
                else if (Mode == 3)
                    Canvas.SetTop(pic, Canvas.GetTop(pic) - Speed);
                else if (Mode == 4)
                    Canvas.SetTop(pic, Canvas.GetTop(pic) + Speed);
            }
        }
        private void Restore()
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
        public void DrawItem(Rectangle pic, Rectangle bul)
        {
            delta++;
            Rotate();
            if (bullet.coordinates.Item1 < 0)
            {
                if (bullet.reload > 100)
                {
                    bullet.Spawn(GetDirection(), GetFirePosition(pic));
                    bullet.reload = 0;
                }
                else
                {
                    bullet.Reloading();
                    Canvas.SetLeft(bul, -60);
                    Canvas.SetTop(bul, -60);
                }
            }
            else
            {                
                bullet.Reloading();
                bullet.DrawItem(bul);
                bullet.Movement();
            }
            if (restoreTime > 0 && !isDead)
            {
                Canvas.SetLeft(pic, restoreCoord.X);
                Canvas.SetTop(pic, restoreCoord.Y);
                restoreTime = 0;
            }
            if (isStoped)
                Speed = 0;
            else
                Speed = 2;
            if (Canvas.GetLeft(pic) == -60)
            {
                isDead = true;
                Restore();
                return;
            }
            coordinates = new Tuple<int, int>((int)Canvas.GetLeft(pic), (int)Canvas.GetTop(pic));
            if (isDead)
                Restore();
            BitmapImage img = new BitmapImage(new Uri("CompTankUp.png", UriKind.Relative));
            if (Mode == 1)
            {
                img = new BitmapImage(new Uri("CompTankRight.png", UriKind.Relative));
                NextRadius = new Rect(Canvas.GetLeft(pic) + 7, Canvas.GetTop(pic), 60, 60);
            }
            else if (Mode == 2)
            {
                img = new BitmapImage(new Uri("CompTankLeft.png", UriKind.Relative));
                NextRadius = new Rect(Canvas.GetLeft(pic) - 7, Canvas.GetTop(pic), 60, 60);
            }
            else if (Mode == 3)
            {
                img = new BitmapImage(new Uri("CompTankUp.png", UriKind.Relative));
                NextRadius = new Rect(Canvas.GetLeft(pic), Canvas.GetTop(pic) - 7, 60, 60);
            }
            else if (Mode == 4)
            {
                img = new BitmapImage(new Uri("CompTankDown.png", UriKind.Relative));
                NextRadius = new Rect(Canvas.GetLeft(pic), Canvas.GetTop(pic) + 7, 60, 60);
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
                        Canvas.SetLeft(pic, 1120);
                        Canvas.SetTop(pic, returnValue);
                        isStoped = true;
                    }
                    else if (coordinates.Item2 > 700)
                    {
                        returnValue = coordinates.Item1;
                        Canvas.SetLeft(pic, returnValue);
                        Canvas.SetTop(pic, 700);
                        isStoped = true;
                    }
                    else if (coordinates.Item1 < 0)
                    {
                        returnValue = coordinates.Item2;
                        Canvas.SetLeft(pic, 1);
                        Canvas.SetTop(pic, returnValue);
                        isStoped = true;
                    }
                    else if (coordinates.Item2 < 0)
                    {
                        returnValue = coordinates.Item1;
                        Canvas.SetLeft(pic, returnValue);
                        Canvas.SetTop(pic, 1);
                        isStoped = true;
                    }
                }
            }
            ImageBrush image = new ImageBrush();
            image.ImageSource = img;
            pic.Fill = image;
            pic.Width = width;
            pic.Height = height;
            pic.Visibility = Visibility.Visible;
            Radius = new Rect(Canvas.GetLeft(pic), Canvas.GetTop(pic), 60, 60);
        }
    }
}
