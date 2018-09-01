using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Sharp : Commponent
    {
        private SizeF size = GameDefault.SizeF;
        private Border border = GameDefault.Border;
        private BackGround backGround = GameDefault.BackGround;
        private float opacity = 1.0f;

        public SizeF Size { get => size; set => size = value; }
        public Border Border { get => border; set => border = value; }
        public BackGround BackGround { get => backGround; set => backGround = value; }
        public float Opacity { get => opacity; set => opacity = value; }

        protected internal override void OnRender(GameObject gameObject)
        {
            var rectangle = new RectangleF(0, 0, size.Width, size.Height);

            if (Border != null && Border.Width != GameDefault.BorderWidth)
            {
                var borderColor = GameResource.IsColorExist(Border.Color) is true ? Border.Color : GameDefault.Color;

                Systems.Graphics.DrawRectangle(rectangle, borderColor, Opacity, Border.Width);
            }

            if (BackGround == null) return;

            if (BackGround.Bitmap == GameDefault.Bitmap || BackGround.Bitmap == null)
            {
                var backgroundColor = GameResource.IsColorExist(BackGround.Color) is true ? BackGround.Color : null;

                if (backgroundColor == GameDefault.Color) backgroundColor = null;

                if (backgroundColor != null) Systems.Graphics.FillRectangle(rectangle, backgroundColor, Opacity);
            }

            if (BackGround.Bitmap != GameDefault.Bitmap && BackGround.Bitmap != null)
            {
                var bitmap = GameResource.GetBitmap(BackGround.Bitmap);

                if (bitmap != null) Systems.Graphics.DrawBitmap(bitmap, rectangle, Opacity);
            }
        }
    }
}
