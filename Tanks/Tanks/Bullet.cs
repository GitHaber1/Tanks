using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tanks
{
    class Bullet : GameItem
    {
        public int reload;
        public Tuple<int, int> coordinates { get; set; }
        private Tuple<int, int> direction;
        int speed { get; set; }
        public Bullet()
        {
            reload = 0;
            speed = 6;
            width = 23;
            height = 23;
            direction = new Tuple<int, int>(0, 0);
            coordinates = new Tuple<int, int>(-50, -50);
        }
        public void Reloading()
        {
            reload++;
        }
        public bool isVisible(Tuple<int, int> coord)
        {
            if (coord.Item1 > 1140 || coord.Item2 > 740 || coord.Item1 < 1 || coord.Item2 < 0)
            {
                return false;
            }
            else
                return true;
        }
        public void Destroy()
        {
            Reloading();
            coordinates = new Tuple<int, int>(-50, -50);
            direction = new Tuple<int, int>(0, 0);
        }
        public void Movement()
        {
            int X = coordinates.Item1;
            int Y = coordinates.Item2;
            if (coordinates.Item1 > -10)
                coordinates = new Tuple<int, int>(X + direction.Item1 * speed, Y + direction.Item2 * speed);
        }
        public void Spawn(Tuple<int, int> dir, Tuple<int, int> p1)
        {
            direction = dir;
            coordinates = new Tuple<int, int>(p1.Item1, p1.Item2);
        }
        public override void DrawItem(Graphics item, PictureBox pic)
        {
            Bitmap img = new Bitmap("Bullet.png");
            item.DrawImage(img, coordinates.Item1, coordinates.Item2);
        }
    }
}
