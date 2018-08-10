using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class BackGround
    {
        private string bitmap = GameDefault.Bitmap;
        private string color = GameDefault.Color;

        public string Bitmap { get => bitmap; set => bitmap = value; }
        public string Color { get => color; set => color = value; }
    }
}
