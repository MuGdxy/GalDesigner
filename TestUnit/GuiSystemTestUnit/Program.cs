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
        public class FontAssetDescription : AssetDescription
        {
            public FontAssetDescription(string name) :
                base(name: name,
                     type: "Font",
                     size: 0,
                     createFunction: (out object x, byte[] y, List<Asset> z) => x = new Font("Consolas", 25, y),
                     destoryFunction: (ref object x) => { (x as Font).Dispose(); x = null; },
                     isKeepDependentAssets: false)
            {
                
            }
        }

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
                
                mLogicGuiComponent.SetProperty(GuiComponentStatusProperty.Drag, true);
                
                mLogicGuiComponent.SetEventSolver(GuiComponentStatusProperty.Hover, (x, y) =>
                {
                    (mVisualGuiComponent as RectangleGuiComponent).RenderMode = (y as GuiComponentHoverEvent).Hover ?
                        GuiRenderMode.Solid : GuiRenderMode.WireFrame;
                });

                mLogicGuiComponent.SetEventSolver(GuiComponentStatusProperty.Focus, (x, y) =>
                {
                    (mVisualGuiComponent as RectangleGuiComponent).Color = (y as GuiComponentFocusEvent).Focus ?
                        new Color<float>(1, 0, 0, 1) : new Color<float>(0, 0, 0, 1);
                });

                SetShowStatus(show);
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

            GameSystems.SystemScene.Root.AddChild(new Package("Package", "Package"));

            GameSystems.AssetSystem.AddAssetDescription(
                package: GameSystems.SystemScene.Root .GetChild("Package") as Package,
                description: new FontAssetDescription("consola.ttf"));

            var asset = GameSystems.AssetSystem.CreateAsset((GameSystems.SystemScene.Root.GetChild("Package") as Package)
                .GetAssetDescription("consola.ttf"));

            GameSystems.SystemScene.Root.AddChild(
                new SimpleGuiObject(new Position<float>(30, 30), new Size<float>(100, 100)));

            GameSystems.SystemScene.Root.AddChild(
                new SimpleGuiObject(new Position<float>(30, 30), new Size<float>(100, 100)));

            GameSystems.GuiSystem.GuiDebugProperty = new GuiDebugProperty()
            {
                //ShapeProperty = new ShapeDebugProperty(2.0f, new Color<float>(1, 0, 0, 1))
            };

            GameSystems.RunLoop();

            GameSystems.AssetSystem.DestoryAsset(asset);
        }
    }
}
