using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GameScene
    {
        private static ValueManager<Size> resolution = new ValueManager<Size>(ChangeResolutionEvent);

        private static Position mousePosition = new Position();

        internal static Bitmap renderTarget = null;

        public static Size Resolution { set => resolution.Value = value; get => resolution.Value; }

        public static event MouseMoveHandler MouseMove;
        public static event MouseClickHandler MouseClick;
        public static event MouseWheelHandler MouseWheel;
        public static event BoardClickHandler BoardClick;
        public static event UpdateHandler Update;

        private static void ChangeResolutionEvent(Size oldSize, Size newSize)
        {
            Utility.Dispose(ref renderTarget);

            renderTarget = new Bitmap(newSize);
        }

        internal static void OnMouseMove(object sender, MouseMoveEvent eventArg)
        {
            mousePosition = Utility.ComputePosition(eventArg.MousePosition, Application.ViewPort, Resolution);

            eventArg.MousePosition = mousePosition;

            MouseMove?.Invoke(sender, eventArg);
        }

        internal static void OnMouseClick(object sender, MouseClickEvent eventArg)
        {
            eventArg.MousePosition = mousePosition;

            MouseClick?.Invoke(sender, eventArg);
        }

        internal static void OnMouseWheel(object sender, MouseWheelEvent eventArg)
        {
            eventArg.MousePosition = mousePosition;

            MouseWheel?.Invoke(sender, eventArg);
        }

        internal static void OnBoardClick(object sender, BoardClickEvent eventArg)
        {
            BoardClick?.Invoke(sender, eventArg);
        }

        internal static void OnUpdate()
        {
            resolution.Update();

            Systems.Graphics.BeginDraw(renderTarget);
            Systems.Graphics.Clear(new Color(1, 1, 1, 1));

            Systems.Graphics.EndDraw();
            
            Update?.Invoke(null);
        }

        public static void Create(string GameName, Size Resolution, string GameIcon)
        {
            resolution.Value = Resolution;

            Application.Create(GameName, Resolution, GameIcon);
        }

        public static void RunLoop()
        {
            Application.RunLoop();
        }
    }
}
