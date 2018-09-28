using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Camera
    {
        private RectangleF area = new RectangleF();
        private SizeF size = new SizeF(); 

        public RectangleF Area
        {
            get => area; set
            {
                area = value;
                size = new SizeF(area.Right - area.Left, area.Bottom - area.Top);
            }
        }
        public SizeF Size { get => size; }

        public Camera()
        {

        }

        public Camera(float Left, float Top, float Right, float Bottom)
        {
            area = new RectangleF(Left, Top, Right, Bottom);
            size = new SizeF(Right - Left, Bottom - Top);
        }

        public Camera(int Left, int Top, int Right, int Bottom)
        {
            area = new RectangleF(Left, Top, Right, Bottom);
            size = new SizeF(Right - Left, Bottom - Top);
        }

        public Camera(PositionF Center, SizeF Radius)
        {
            area = new RectangleF(Center.X - Radius.Width, Center.Y - Radius.Height,
                Center.X + Radius.Width, Center.Y + Radius.Height);
            size = new SizeF(Radius.Width * 2, Radius.Height * 2);
        }

        public void Move(PositionF position)
        {
            area.Left += position.X;
            area.Top += position.Y;
            area.Right += position.X;
            area.Bottom += position.Y;
        }
    }
}
