using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tanks
{
    class Tank : GameItem
    {
        public Rectangle NextRadius { get; set; }
        public Rectangle Radius { get; set; }
        public bool isDead { get; set; }
        public Tuple<int, int> coordinates { get; set; }
        public Bullet bullet { get; set; }
        public int Mode { get; set; }
        public int Speed { get; set; }
        public Tank()
        {
            isDead = false;
            bullet = new Bullet();
            Speed = 2;
            Mode = 3;
            width = 60;
            height = 60;
        }
        public Tuple<int, int> GetFirePosition(PictureBox pic)
        {
            Tuple<int, int> dir;
            if (Mode == 1)
                dir = new Tuple<int, int>(pic.Location.X + pic.Width / 2 + 40, pic.Location.Y + pic.Width / 2 - 10);
            else if (Mode == 2)
                dir = new Tuple<int, int>(pic.Location.X - pic.Width / 2, pic.Location.Y + pic.Width / 2 - 10);
            else if (Mode == 3)
                dir = new Tuple<int, int>(pic.Location.X + pic.Width / 2 - 10, pic.Location.Y - pic.Width / 2);
            else
                dir = new Tuple<int, int>(pic.Location.X + pic.Width / 2 - 10, pic.Location.Y + pic.Width / 2 + 40);
            return dir;
        }
        public Tuple<int, int> GetDirection()
        {
            Tuple<int, int> dir;
            if (Mode == 1)
                dir = new Tuple<int, int>(1, 0);
            else if (Mode == 2)
                dir = new Tuple<int, int>(-1, 0);
            else if (Mode == 3)
                dir = new Tuple<int, int>(0, -1);
            else
                dir = new Tuple<int, int>(0, 1);
            return dir;
        }
        public bool isValidMove(Tuple<int, int> coord)
        {
            if (coord.Item1 > 1120 || coord.Item2 > 700 || coord.Item1 < 1 || coord.Item2 < 1)
            {
                return false;
            }
            else
                return true;
        }
        public override void DrawItem(Graphics item, PictureBox pic)
        {
        }
    }
}
