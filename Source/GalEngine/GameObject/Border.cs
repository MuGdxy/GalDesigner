using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Border
    {
        private float width = GameDefault.BorderWidth;
        private string color = GameDefault.Color;

        public float Width { get => width; set => width = value; }
        public string Color { get => color; set => color = value; }

        public Border(float Width = 0.0f)
        {
            width = Width;
            color = GameDefault.Color;
        }

        public Border(float Width, string Color)
        {
            width = Width;
            color = Color;
        }
    }
}
