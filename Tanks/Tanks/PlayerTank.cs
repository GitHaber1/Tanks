using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tanks
{
    class PlayerTank : Tank // класс для танка игрока
    {
        public bool isNoBarrier(List<Tank> tanks) // проверяет, есть ли преграда перед танком
        {
            for (int i = 1; i < tanks.Count; i++)
            {
                if (tanks[0].NextRadius.IntersectsWith(tanks[i].Radius) && !tanks[i].isDead)
                    return false;
            }
            return true;
        }
        public override void DrawItem(Graphics item, PictureBox pic) // отрисовка танка
        {
            Bitmap img = new Bitmap("playerTankUp.png");
            if (Mode == 1)
            {
                img = new Bitmap("playerTankRight.png");
                NextRadius = new Rectangle(pic.Location.X + 7, pic.Location.Y, 60, 60);
            }
            else if (Mode == 2)
            {
                img = new Bitmap("playerTankLeft.png");
                NextRadius = new Rectangle(pic.Location.X - 7, pic.Location.Y, 60, 60);
            }
            else if (Mode == 3)
            {
                img = new Bitmap("playerTankUp.png");
                NextRadius = new Rectangle(pic.Location.X, pic.Location.Y - 7, 60, 60);
            }
            else if (Mode == 4)
            {
                img = new Bitmap("playerTankDown.png");
                NextRadius = new Rectangle(pic.Location.X, pic.Location.Y - 7, 60, 60);
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
                        pic.Location = new Point(1120, returnValue);
                    }
                    else if (coordinates.Item2 > 700)
                    {
                        returnValue = coordinates.Item1;
                        pic.Location = new Point(returnValue, 700);
                    }
                    else if (coordinates.Item1 < 1)
                    {
                        returnValue = coordinates.Item2;
                        pic.Location = new Point(1, returnValue);
                    }
                    else
                    {
                        returnValue = coordinates.Item1;
                        pic.Location = new Point(returnValue, 1);
                    }
                }
            }
            coordinates = new Tuple<int, int>(pic.Location.X, pic.Location.Y);
            pic.Size = new Size(width, height);
            Radius = new Rectangle(pic.Location.X, pic.Location.Y, 60, 60);
            pic.Image = img;
            pic.BackColor = Color.Transparent;
        }
    }
}
