using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine.Runtime.Graphics;

namespace GalEngine
{
    public class GuiRender
    {
        protected GraphicsDevice mDevice;

        public GuiRender(GraphicsDevice device)
        {
            //gui render is a simple render to render gui object
            //gui render can provide some simple object draw function
            //we can replace it to our render and we can use it in the gui system render function
            mDevice = device;
        }

        public virtual void DrawLine(Position<float> start, Position<float> end, Color<float> color, float padding, float opacity)
        {
            //draw line with start and end position
            //support property: color(0.9f), padding(1.0f), opacity(1.0f)
            //(value) means suggested value


        }
    }
}
