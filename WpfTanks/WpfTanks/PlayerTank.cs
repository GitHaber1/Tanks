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
    public class PlayerTank : Tank
    {
        public bool isNoBarrier(List<Tank> tanks)
        {
            for (int i = 1; i < tanks.Count; i++)
            {
                if (tanks[0].NextRadius.IntersectsWith(tanks[i].Radius) && !tanks[i].isDead)
                    return false;
            }
            return true;
        }
        public override void DrawItem(Rectangle pic)
        {            
            BitmapImage img = new BitmapImage(new Uri("playerTankUp.png", UriKind.Relative));
            if (Mode == 1)
            {
                img = new BitmapImage(new Uri("playerTankRight.png", UriKind.Relative));
                NextRadius = new Rect(Canvas.GetLeft(pic) + 7, Canvas.GetTop(pic), 60, 60);
            }
            else if (Mode == 2)
            {
                img = new BitmapImage(new Uri("playerTankLeft.png", UriKind.Relative));
                NextRadius = new Rect(Canvas.GetLeft(pic) - 7, Canvas.GetTop(pic), 60, 60);
            }
            else if (Mode == 3)
            {
                img = new BitmapImage(new Uri("playerTankUp.png", UriKind.Relative));
                NextRadius = new Rect(Canvas.GetLeft(pic), Canvas.GetTop(pic) - 7, 60, 60);
            }
            else if (Mode == 4)
            {
                img = new BitmapImage(new Uri("playerTankDown.png", UriKind.Relative));
                NextRadius = new Rect(Canvas.GetLeft(pic), Canvas.GetTop(pic) + 7, 60, 60);
            }
            else
                return;
            if (coordinates != null)
            {
                if (!isValidMove(coordinates))
                {
                    if (coordinates.Item1 < -10)
                    {
                        isDead = true;
                        return;
                    }

                    int returnValue;
                    if (coordinates.Item1 > 1120)
                    {
                        returnValue = coordinates.Item2;
                        Canvas.SetLeft(pic, 1120);
                        Canvas.SetTop(pic, returnValue);
                    }
                    else if (coordinates.Item2 > 700)
                    {
                        returnValue = coordinates.Item1;
                        Canvas.SetLeft(pic, returnValue);
                        Canvas.SetTop(pic, 700);
                    }
                    else if (coordinates.Item1 < 1)
                    {
                        returnValue = coordinates.Item2;
                        Canvas.SetLeft(pic, 1);
                        Canvas.SetTop(pic, returnValue);
                    }
                    else
                    {
                        returnValue = coordinates.Item1;
                        Canvas.SetLeft(pic, returnValue);
                        Canvas.SetTop(pic, 1);
                    }
                }
            }
            ImageBrush image = new ImageBrush();
            image.ImageSource = img;
            coordinates = new Tuple<int, int>((int)Canvas.GetLeft(pic), (int)Canvas.GetTop(pic));
            pic.Fill = image;
            pic.Width = width;
            pic.Height = height;
            pic.Visibility = Visibility.Visible;
            Radius = new Rect((int)Canvas.GetLeft(pic), (int)Canvas.GetTop(pic), 60, 60);
        }
    }
}
