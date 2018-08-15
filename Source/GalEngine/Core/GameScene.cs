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

        private static Camera defaultCamera = new Camera();
        private static Camera usedCamera = defaultCamera;

        private static GameObject root = new GameObject("Root", new Size(0, 0));

        internal static Bitmap renderTarget = null;

        public static Size Resolution { set => resolution.Value = value; get => resolution.Value; }
        public static GameObject Root { get => root; }

        public static Camera Camera { get => usedCamera; }

        public static event MouseMoveHandler MouseMove;
        public static event MouseClickHandler MouseClick;
        public static event MouseWheelHandler MouseWheel;
        public static event BoardClickHandler BoardClick;
        public static event UpdateHandler Update;

        private static void ChangeResolutionEvent(Size oldSize, Size newSize)
        {
            Utility.Dispose(ref renderTarget);

            renderTarget = new Bitmap(newSize);

            defaultCamera.Area = new RectangleF(0, 0, newSize.Width, newSize.Height);

            DebugLayer.DebugCommand.SetSharp(newSize);
        }

        internal static void OnMouseMove(object sender, MouseMoveEvent eventArg)
        {
            mousePosition = Utility.ComputePosition(eventArg.MousePosition, Application.ViewPort, Resolution);

            eventArg.MousePosition = mousePosition;

            MouseMove?.Invoke(sender, eventArg);

            GameObject.ProcessMouseMove(Root, eventArg, System.Numerics.Matrix3x2.Identity);
            GameObject.ProcessMouseMove(DebugLayer.DebugCommand, eventArg, System.Numerics.Matrix3x2.Identity);
        }

        internal static void OnMouseClick(object sender, MouseClickEvent eventArg)
        {
            eventArg.MousePosition = mousePosition;

            MouseClick?.Invoke(sender, eventArg);

            GameObject.ProcessMouseClick(Root, eventArg, System.Numerics.Matrix3x2.Identity);
            GameObject.ProcessMouseClick(DebugLayer.DebugCommand, eventArg, System.Numerics.Matrix3x2.Identity);
        }

        internal static void OnMouseWheel(object sender, MouseWheelEvent eventArg)
        {
            eventArg.MousePosition = mousePosition;

            MouseWheel?.Invoke(sender, eventArg);

            GameObject.ProcessMouseWheel(Root, eventArg, System.Numerics.Matrix3x2.Identity);
            GameObject.ProcessMouseWheel(DebugLayer.DebugCommand, eventArg, System.Numerics.Matrix3x2.Identity);
        }

        internal static void OnBoardClick(object sender, BoardClickEvent eventArg)
        {
            BoardClick?.Invoke(sender, eventArg);

            GameObject.ProcessBoardClick(Root, eventArg);
            GameObject.ProcessBoardClick(DebugLayer.DebugCommand, eventArg);
        }

        internal static void OnUpdate()
        {
            resolution.Update();

            Update?.Invoke(null);

            GameObject.ProcessUpdate(Root);
            GameObject.ProcessUpdate(DebugLayer.DebugCommand);

            Systems.Graphics.BeginDraw(renderTarget);
            Systems.Graphics.Clear(new Color(1, 1, 1, 1));

            GameObject.RenderGameObject(Root, System.Numerics.Matrix3x2.Identity, Camera);
            GameObject.RenderGameObject(DebugLayer.DebugCommand, System.Numerics.Matrix3x2.Identity, DebugLayer.DebugCommand.Camera);

            Systems.Graphics.EndDraw();
        }

        public static void Create(string GameName, Size Resolution, string GameIcon)
        {
            resolution.Value = Resolution;

            Application.Create(GameName, Resolution, GameIcon);
        }

        public static void SetCamera(Camera Camera)
        {
            usedCamera = Camera;
        }

        public static void SetCamera()
        {
            usedCamera = defaultCamera;
        }

        public static void SetGameObject(GameObject GameObject)
        {
            Root.SetChild(GameObject);
        }

        public static void CancelGameObject(GameObject GameObject)
        {
            Root.CancelChild(GameObject);
        }

        public static void CancelGameObject(string GameObject)
        {
            Root.CancelChild(GameObject);
        }

        public static void RunLoop()
        {
            Application.RunLoop();
        }
    }
}
