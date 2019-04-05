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
                     createFunction: (out object x, byte[] y, List<Asset> z) => x = new Font(20, y),
                     destoryFunction: (ref object x) => { (x as Font).Dispose(); x = null; },
                     isKeepDependentAssets: false)
            {

            }
        }

        public class SimpleGuiObject : GuiControl
        {
            private LogicGuiComponent mLogicGuiComponent;
            private TransformGuiComponent mTransformComponent;
            private TextGuiComponent mTextComponent;

            public SimpleGuiObject(string text, Position<float> position, Size<float> size, Font font)
            {
                AddComponent(mLogicGuiComponent = new LogicGuiComponent());
                AddComponent(mTransformComponent = new TransformGuiComponent(position));
                AddComponent(mTextComponent = new TextGuiComponent(new RectangleShape(size), text, font, new Color<float>(0, 0, 0, 1)));

                mLogicGuiComponent.SetEventStatus(GuiComponentStatusProperty.Drag, true);
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

            GameSystems.AssetSystem.AddAssetDescription(
                package: GameSystems.SystemScene.Root .GetChild(StringProperty.PackageRoot) as Package,
                description: new FontAssetDescription("consola.ttf"));

            var asset = GameSystems.AssetSystem.CreateAsset((GameSystems.SystemScene.Root.GetChild(StringProperty.PackageRoot) as Package)
                .GetAssetDescription("consola.ttf"));

            GameSystems.SystemScene.Root.GetChild(StringProperty.GuiControlRoot).AddChild(
                new SimpleGuiObject("Hello, World!", new Position<float>(30, 30), new Size<float>(200, 100), asset.Instance as Font));

            GameSystems.GuiSystem.GuiDebugProperty = new GuiDebugProperty()
            {
                ShapeProperty = new ShapeDebugProperty(2.0f, new Color<float>(1, 0, 0, 1))
            };

            GameSystems.RunLoop();

            GameSystems.AssetSystem.DestoryAsset(asset);
        }
    }
}
