using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;
using GalEngine.GameResource;

namespace GuiSystemTestUnit
{
    class Program
    {
        public class SimpleGuiObject : GuiControl
        {
            private LogicGuiComponent mLogicGuiComponent;
            private TransformGuiComponent mTransformComponent;
            private VisualGuiComponent mVisualGuiComponent;

            public SimpleGuiObject(Position<float> position, Size<float> size, bool show = true)
            {
                AddComponent(mLogicGuiComponent = new LogicGuiComponent());
                AddComponent(mTransformComponent = new TransformGuiComponent(position));
                AddComponent(mVisualGuiComponent = new RectangleGuiComponent(
                    new RectangleShape(size), new Color<float>(0, 0, 0, 1.0f), GuiRenderMode.WireFrame));

                mLogicGuiComponent?.EventParts.Add(GuiComponentSupportEvent.Hover, new GuiComponentHoverEventPart(
                    (x, y) =>
                    {
                        (mVisualGuiComponent as RectangleGuiComponent).RenderMode =
                        (y as GuiComponentHoverEvent).Hover ? GuiRenderMode.Solid : GuiRenderMode.WireFrame;
                    }));

                mLogicGuiComponent?.EventParts.Add(GuiComponentSupportEvent.Focus, new GuiComponentFocusEventPart(
                    (x, y) =>
                    {
                        (mVisualGuiComponent as RectangleGuiComponent).Color =
                        (y as GuiComponentFocusEvent).Focus ? new Color<float>(1, 0, 0, 1) : new Color<float>(0, 0, 0, 1);
                    }));

                mLogicGuiComponent?.EventParts.Add(GuiComponentSupportEvent.Drag, new GuiComponentDragEventPart());
            }
        }

        static void Main(string[] args)
        {
            GameSystems.Initialize(new GameStartInfo()
            {
                GameName = "GuiSystemTestUnit",
                WindowName = "GuiSystemTestUnit",
                IconName = "",
                WindowSize = new Size<int>(1280, 720)
            });

            GameSystems.SystemScene.Root.AddChild(
                new SimpleGuiObject(new Position<float>(30, 30), new Size<float>(100, 100)));

            GameSystems.SystemScene.Root.AddChild(
                new SimpleGuiObject(new Position<float>(300, 300), new Size<float>(100, 100)));

            var guiButton0 = new GuiButton();
            var guiButton1 = new GuiButton();

            (guiButton0.GetComponent<VisualGuiComponent>().Shape as RectangleShape).Size = new Size<float>(80, 40);
            guiButton0.GetComponent<TransformGuiComponent>().Position = new Position<float>(100, 100);

            guiButton0.GetComponent<LogicGuiComponent>().EventParts.Get(GuiComponentSupportEvent.MouseClick)
                .Solver += (x, y) =>
                 {
                     var eventArg = y as GuiComponentMouseClickEvent;

                     if (eventArg.IsDown && eventArg.Button == MouseButton.Left)
                         Console.WriteLine(1);
                 };

            (guiButton1.GetComponent<VisualGuiComponent>().Shape as RectangleShape).Size = new Size<float>(80, 40);
            (guiButton1.GetComponent<VisualGuiComponent>() as ButtonGuiComponent).Text = "Button1";

            GameSystems.SystemScene.Root.AddChild(guiButton0);
            GameSystems.SystemScene.Root.AddChild(guiButton1);



            GameSystems.VisualGuiSystem.GuiRenderDebugProperty = new GuiRenderDebugProperty()
            {
                //ShapeProperty = new ShapeDebugProperty(2.0f, new Color<float>(1, 0, 0, 1))
            };

            GameSystems.RunLoop();
        }
    }
}
