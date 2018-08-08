using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static partial class Application
    {
        private static ValueManager<string> name = new ValueManager<string>(ChangeNameEvent);
        private static ValueManager<string> icon = new ValueManager<string>();
        private static ValueManager<Size> size = new ValueManager<Size>(ChangeSizeEvent);

        private static Position mousePosition = new Position();

        public static string Name { set => name.Value = value; get => name.Value; }
        public static Size Size { set => size.Value = value; get => size.Value; }

        public static RectangleF ViewPort => Utility.ComputeViewPort(Size, GameScene.Resolution);

        public static event MouseMoveHandler MouseMove;
        public static event MouseClickHandler MouseClick;
        public static event MouseWheelHandler MouseWheel;
        public static event BoardClickHandler BoardClick;
        public static event UpdateHandler Update;

        private static void ChangeNameEvent(string oldName, string newName)
        {
            Systems.Windows.SetWindowText(newName);
        }

        private static void ChangeSizeEvent(Size oldSize, Size newSize)
        {
            Systems.Windows.SetWindowSize(newSize);
        }

        internal static void OnMouseMove(object sender, MouseMoveEvent eventArg)
        {
            mousePosition = eventArg.MousePosition;

            MouseMove?.Invoke(sender, eventArg);

            GameScene.OnMouseMove(sender, eventArg);
        }

        internal static void OnMouseClick(object sender, MouseClickEvent eventArg)
        {
            eventArg.MousePosition = mousePosition;

            MouseClick?.Invoke(sender, eventArg);

            GameScene.OnMouseClick(sender, eventArg);
        }

        internal static void OnMouseWheel(object sender, MouseWheelEvent eventArg)
        {
            eventArg.MousePosition = mousePosition;

            MouseWheel?.Invoke(sender, eventArg);

            GameScene.OnMouseWheel(sender, eventArg);
        }

        internal static void OnBoardClick(object sender, BoardClickEvent eventArg)
        {
            BoardClick?.Invoke(sender, eventArg);

            GameScene.OnBoardClick(sender, eventArg);
        }

        internal static void OnUpdate(object sender)
        {
            name.Update();
            icon.Update();
            size.Update();
            
            Update?.Invoke(sender);

            GameScene.OnUpdate();

            Systems.Windows.PresentBitmap();
        }

        internal static void Create(string Name, Size Size, string Icon)
        {
            name.Value = Name;
            size.Value = Size;
            icon.Value = Icon;

            Systems.Windows.CreateWindow(name.Value, size.Value, icon.Value);
        }

        internal static void RunLoop()
        {
            while (Systems.Windows.UpdateWindow() is false)
            {
                OnUpdate(null);
            }
        }
    }
}
