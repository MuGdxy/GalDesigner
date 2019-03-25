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
        private Texture2D mTexture;

        private GuiRender mRender;

        public Rectangle<int> Area { get; set; }

        public GuiSystem(GpuDevice device, Rectangle<int> area) : base("GuiSystem")
        {
            RequireComponents.AddRequireComponentType<TransformGuiComponent>();
            RequireComponents.AddRequireComponentType<VisualGuiComponent>();
            RequireComponents.AddRequireComponentType<LogicGuiComponent>();

            mRender = new GuiRender(device);

            Area = area;

            mTexture = new Texture2D(
                new Size<int>(Area.Right - Area.Left, Area.Bottom - Area.Top),
                PixelFormat.RedBlueGreenAlpha8bit,
                mRender.Device);
        }

        protected internal override void Update()
        {
            //update the render area, we need to update the canvas and render target
            //if the area's size is not equal the canvas's size
            if (mTexture.Size.Width != Area.Right - Area.Left ||
                mTexture.Size.Height != Area.Bottom - Area.Top)
            {
                Utility.Dispose(ref mTexture);

                mTexture = new Texture2D(
                    new Size<int>(Area.Right - Area.Left, Area.Bottom - Area.Top),
                    PixelFormat.RedBlueGreenAlpha8bit,
                    mRender.Device);
            }
        }

        protected internal override void Present(PresentRender render)
        {
            render.Mask(mTexture, Area, 1.0f);
        }

        protected internal override void Excute(List<GameObject> passedGameObjectList)
        {
            mRender.BeginDraw(mTexture);

            foreach (var gameObject in passedGameObjectList)
            {
                var transformComponent = gameObject.GetComponent<TransformGuiComponent>();

                //run rendering code
                switch (gameObject.GetComponent<VisualGuiComponent>())
                {
                    case FrameVisualGuiComponent frameComponent:
                        //draw rectangle
                        mRender.DrawRectangle(
                            new Rectangle<float>(
                                transformComponent.Position.X,
                                transformComponent.Position.Y,
                                transformComponent.Position.X + frameComponent.Size.Width,
                                transformComponent.Position.Y + frameComponent.Size.Height),
                            frameComponent.Color,
                            frameComponent.Padding);
                        break;
                }
            }

            mRender.EndDraw();
        }
    }
}
