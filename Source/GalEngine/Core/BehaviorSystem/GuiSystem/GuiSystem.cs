using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalEngine.Runtime.Graphics;

namespace GalEngine
{
    public class GuiSystem : BehaviorSystem
    {
        private Texture2D mCanvas;

        private GuiRender mRender;

        public Rectangle<int> Area { get; set; }

        public GuiSystem(GpuDevice device, Rectangle<int> area) : base("GuiSystem")
        {
            RequireComponents.AddRequireComponentType<TransformGuiComponent>();
            RequireComponents.AddRequireComponentType<VisualGuiComponent>();
            RequireComponents.AddRequireComponentType<LogicGuiComponent>();

            mRender = new GuiRender(device);

            Area = area;

            mCanvas = new Texture2D(
                new Size<int>(Area.Right - Area.Left, Area.Bottom - Area.Top),
                PixelFormat.RedBlueGreenAlpha8bit,
                mRender.Device);
        }

        protected internal override void Update()
        {
            //update the render area, we need to update the canvas and render target
            //if the area's size is not equal the canvas's size
            if (mCanvas.Size.Width != Area.Right - Area.Left ||
                mCanvas.Size.Height != Area.Bottom - Area.Top)
            {
                Utility.Dispose(ref mCanvas);

                mCanvas = new Texture2D(
                    new Size<int>(Area.Right - Area.Left, Area.Bottom - Area.Top),
                    PixelFormat.RedBlueGreenAlpha8bit,
                    mRender.Device);
            }
        }

        protected internal override void Present(PresentRender render)
        {
            render.Mask(mCanvas, Area, 1.0f);
        }

        protected internal override void Excute(List<GameObject> passedGameObjectList)
        {
            mRender.BeginDraw(mCanvas);

            foreach (var gameObject in passedGameObjectList)
            {
                var transformComponent = gameObject.GetComponent<TransformGuiComponent>();

               
            }

            mRender.EndDraw();
        }
    }
}
