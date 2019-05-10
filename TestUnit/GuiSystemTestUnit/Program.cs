using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace GuiSystemTestUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            GameSystems.Initialize(new GameStartInfo()
            {
                GameName = "GuiSystemTestUnit",
                WindowName = "GuiSystemTestUnit",
                IconName = "",
                WindowSize = new Size<int>(1920, 1080)
            });

            var guiText = new GuiText("This is a test text!", new Font(20), new Color<float>(0, 0, 0, 1));
            var guiButton = new GuiButton();
            var guiInputText = new GuiInputText("input", 100, 
                DefaultInputTextProperty.Background, 
                DefaultInputTextProperty.Frontground, new Font(30));
            //guiInputText.GetComponent<InputTextGuiComponent>().Content = "2333";
            guiInputText.GetComponent<InputTextGuiComponent>().CursorLocation = 1;
            guiInputText.GetComponent<TransformGuiComponent>().Position = new Position<float>(100, 100);

            (guiButton.GetComponent<ButtonGuiComponent>().Shape as RectangleShape).Size = new Size<float>(80, 40);
            //guiButton.GetComponent<TransformGuiComponent>().Position = new Position<float>(100, 100);

            guiButton.GetComponent<LogicGuiComponent>().EventParts.Get(GuiComponentSupportEvent.MouseClick)
                .Solver += (x, y) =>
                 {
                     var eventArg = y as GuiComponentMouseClickEvent;

                     if (eventArg.IsDown && eventArg.Button == MouseButton.Left)
                         Console.WriteLine(1);
                 };

            GameSystems.SystemScene.Root.AddChild(guiButton);
            //GameSystems.SystemScene.Root.AddChild(guiText);
            GameSystems.SystemScene.Root.AddChild(guiInputText);


            GameSystems.VisualGuiSystem.GuiRenderDebugProperty = new GuiRenderDebugProperty()
            {
                //ShapeProperty = new ShapeDebugProperty(2.0f, new Color<float>(1, 0, 0, 1))
            };

            GameSystems.RunLoop();
        }
    }
}
