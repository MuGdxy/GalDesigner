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
        public class SimpleGuiObject : GameObject
        {
            private LogicGuiComponent mLogicGuiComponent;
            private TransformGuiComponent mTransformComponent;
            private FrameVisualGuiComponent mVisualGuiComponent;
            
            public SimpleGuiObject(Position<float> position, Size<float> size, Color<float> color)
            {
                AddComponent(mLogicGuiComponent = new LogicGuiComponent());
                AddComponent(mTransformComponent = new TransformGuiComponent(position));
                AddComponent(mVisualGuiComponent = new FrameVisualGuiComponent(size, color));
            }
        }

        static void Main(string[] args)
        {
            GameSystems.Initialize(new GameStartInfo()
            {
                GameName = "GuiSystemTestUnit",
                WindowName = "GuiSystemTestUnit",
                IconName = "",
                WindowSize = new Size<int>(1920, 1080)
            });
            GameSystems.SystemScene.Root.GetChild(StringProperty.GuiControlRoot).AddChild(
                new SimpleGuiObject(
                    new Position<float>(),
                    new Size<float>(100, 100), 
                    new Color<float>(1, 0, 0, 1)));

            GameSystems.RunLoop();
        }
    }
}
