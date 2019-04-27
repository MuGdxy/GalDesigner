using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

using GalEngine.Runtime.Graphics;

namespace GalEngine
{
    
    public class GuiRenderDebugProperty
    {
        public ShapeDebugProperty ShapeProperty { get; set; }
    }

    public class VisualGuiSystem : BehaviorSystem
    {
        private Image mCanvas;
        private GuiRender mRender;

        public Rectangle<int> Area { get; set; }
        public GuiRenderDebugProperty GuiRenderDebugProperty { get; set; }
        
        public VisualGuiSystem(GpuDevice device, Rectangle<int> area) : base("VisualGuiSystem")
        {
            RequireComponents.AddRequireComponentType<TransformGuiComponent>();
            RequireComponents.AddRequireComponentType<VisualGuiComponent>();
            
            Area = area;

            mRender = new GuiRender(device);

            mCanvas = new Image(
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

                mCanvas = new Image(
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
            //solve to render debug property
            void renderDebugPropertySolver(GuiRender render, Shape shape)
            {
                if (GuiRenderDebugProperty != null && GuiRenderDebugProperty.ShapeProperty != null)
                {
                    var padding = GuiRenderDebugProperty.ShapeProperty.Padding;

                    switch (shape)
                    {
                        case RectangleShape rectangle:
                            //draw debug shape
                            render.DrawRectangle(
                                rectangle: new Rectangle<float>(
                                    left: -padding,
                                    top: -padding,
                                    right: rectangle.Size.Width + padding,
                                    bottom: rectangle.Size.Height + padding),
                                color: GuiRenderDebugProperty.ShapeProperty.Color,
                                padding: GuiRenderDebugProperty.ShapeProperty.Padding);
                            break;
                        default: break;
                    }
                }
            }

            //solve to render text component
            void textGuiComponentSolver(GuiRender render, TextGuiComponent textComponent)
            {
                //update the property and create asset(Text)
                textComponent.SetPropertyToAsset();

                //not invisable text, we will render it
                if (textComponent.mTextAsset.Image.GpuTexture != null)
                    render.DrawText(new Position<float>(), textComponent.mTextAsset, textComponent.Color);
            }

            //solve to render rectangle compoent
            void rectangleComponentSolver(GuiRender render, RectangleGuiComponent rectangleComponent)
            {
                //get size of rectangle
                var size = (rectangleComponent.Shape as RectangleShape).Size;

                //swtich render mode
                switch (rectangleComponent.RenderMode)
                {
                    case GuiRenderMode.WireFrame:
                        render.DrawRectangle(
                            rectangle: new Rectangle<float>(0, 0, size.Width, size.Height),
                            color: rectangleComponent.Color,
                            padding: rectangleComponent.Padding);
                        break;
                    case GuiRenderMode.Solid:
                        render.FillRectangle(
                            rectangle: new Rectangle<float>(0, 0, size.Width, size.Height),
                            color: rectangleComponent.Color);
                        break;
                    default: break;
                }
            }

            //solve to render image component
            void imageComponentSolver(GuiRender render, ImageGuiComponent imageComponent)
            {
                var size = (imageComponent.Shape as RectangleShape).Size;

                //draw image
                render.DrawImage(new Rectangle<float>(
                    left: 0, top: 0, right: size.Width, bottom: size.Height),
                    image: imageComponent.Image,
                    opacity: imageComponent.Opacity);
            }

            //solve to render button component
            void buttonComponentSolver(GuiRender render, ButtonGuiComponent buttonComponent)
            {
                //update the property and create asset(text)
                buttonComponent.SetPropertyToAsset();

                //get size of rectangle
                var size = (buttonComponent.Shape as RectangleShape).Size;

                //get position of button text
                var position = new Position<float>(
                    x: (size.Width - buttonComponent.mTextAsset.Size.Width) * 0.5f,
                    y: (size.Height - buttonComponent.mTextAsset.Size.Height) * 0.5f);

                //render button shape
                render.FillRectangle(
                    new Rectangle<float>(0, 0, size.Width, size.Height),
                    buttonComponent.BackGround);

                //render button text
                if (buttonComponent.mTextAsset.Image.GpuTexture != null)
                    render.DrawText(position, buttonComponent.mTextAsset, buttonComponent.FrontGround);
            }

            //solve to render input text component
            void inputTextComponentSolver(GuiRender render, InputTextGuiComponent inputTextComponent)
            {
                //update the property and create asset(input text)
                inputTextComponent.SetPropertyToAsset();

                var size = (inputTextComponent.Shape as RectangleShape).Size;
                var cursorLocation = inputTextComponent.CursorLocation;

                var inputTextPosition = new Position<float>(
                    x: 0 + GuiProperty.InputTextPadding,
                    y: 0 + (size.Height - inputTextComponent.mInputText.Size.Height) * 0.5f);
                var cursorPosition = new Position<float>(
                    x: inputTextPosition.X + (cursorLocation == 0 ? 0 : inputTextComponent.mInputText.GetCharacterPostLocation(cursorLocation - 1)), 
                    y: inputTextPosition.Y);

                //render background box
                render.FillRectangle(
                    new Rectangle<float>(0, 0, size.Width, size.Height),
                    inputTextComponent.BackGround);

                //render input text
                render.DrawText(inputTextPosition, inputTextComponent.mInputText, inputTextComponent.FrontGround);

                //render cursor
                render.DrawLine(
                    start: cursorPosition,
                    end: new Position<float>(cursorPosition.X, cursorPosition.Y + inputTextComponent.mInputText.Size.Height),
                    color: inputTextComponent.FrontGround,
                    padding: GuiProperty.InputTextCursorWidth);
            }

            //stack to maintain the path of game object's tree from node to root
            //matrix is the transform from root to node
            //bool is the visable status sum(operator &) from root to node
            Stack<Tuple<GameObject, Matrix4x4>> transformStack = new Stack<Tuple<GameObject, Matrix4x4>>();

            //add virtual root to stack
            transformStack.Push(new Tuple<GameObject, Matrix4x4>(null, Matrix4x4.Identity));

            //visual component solver
            mRender.BeginDraw(mCanvas);
            mRender.Clear(mCanvas, new Color<float>(1, 1, 1, 1));

            foreach (var gameObject in passedGameObjectList)
            {
                //maintain the elments in the stack are ancestors from root to node(with deep order, first element is root)
                //because the list of game objects is the dfs order of tree
                while (transformStack.Count != 1 && transformStack.Peek().Item1 != gameObject.Parent) transformStack.Pop();

                //get current transform matrix, it is the result of root to node's transform matrix
                var transformComponent = gameObject.GetComponent<TransformGuiComponent>();
                var transformMatrix = transformComponent.Transform * transformStack.Peek().Item2;
                
                //set transform 
                mRender.SetTransform(transformMatrix);

                switch (gameObject.GetComponent<VisualGuiComponent>())
                {
                    case TextGuiComponent textComponent:
                        textGuiComponentSolver(mRender, textComponent); break;
                    case RectangleGuiComponent rectangleComponent:
                        rectangleComponentSolver(mRender, rectangleComponent); break;
                    case ImageGuiComponent imageComponent:
                        imageComponentSolver(mRender, imageComponent); break;
                    case ButtonGuiComponent buttonComponent:
                        buttonComponentSolver(mRender, buttonComponent); break;
                    case InputTextGuiComponent inputComponent:
                        inputTextComponentSolver(mRender, inputComponent); break;
                    default: break;
                }

                //draw debug shape
                renderDebugPropertySolver(mRender, gameObject.GetComponent<VisualGuiComponent>().Shape);
                    
                //update stack
                transformStack.Push(new Tuple<GameObject, Matrix4x4>(gameObject, transformMatrix));
            }

            mRender.EndDraw();
        }
    }
}
