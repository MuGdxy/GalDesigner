using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    struct GlobalGuiElementStatus
    {
        public GuiElement DragElement;
        public GuiElement FocusElement;
        public Point2f Position;
    }

    public static class Gui
    {
        private static GuiRender mRender;

        internal static GlobalGuiElementStatus GlobalElementStatus;

        public static Dictionary<string, GuiGroup> Groups { get; }

        public static Image Canvas { get; private set; }

        public static InputMapped InputMapped { get; }

        public static bool AutoSize { get; set; }

        internal static void Initialize()
        {
            mRender = new GuiRender(GameSystems.GpuDevice);

            ///auto size, if true means when window size is changed, the canvas size is also changed.
            ///false means we do not change the size when window size changed.
            AutoSize = true;

            ///resize the canvas size
            ReSizeCanvas(GameSystems.EngineWindow.Size);

            GameSystems.EngineWindow.OnSizeChangeEvent += (sender, eventArg) =>
            {
                if (AutoSize == false) return;

                ReSizeCanvas(eventArg.After);
            };

            ///add input mapped
            ///axis input mapped
            InputMapped.Mapped(InputProperty.MouseX, GuiProperty.InputMoveX);
            InputMapped.Mapped(InputProperty.MouseY, GuiProperty.InputMoveY);
            InputMapped.Mapped(InputProperty.MouseWheel, GuiProperty.InputWheel);
            ///button input mapped
            InputMapped.Mapped(InputProperty.LeftButton, GuiProperty.InputClick);
            ///mapped the keycode to input text
            InputProperty.KeyBoardMapped.ForEach((name) => InputMapped.Mapped(name, GuiProperty.InputText));

            LogEmitter.Apply(LogLevel.Information, "[Initialize Gui System Finish] from [Gui]");
        }

        internal static void Draw()
        {
            mRender.BeginDraw(Canvas);
            mRender.Clear(Canvas, new Colorf(1, 1, 1, 1));

            foreach (var group in Groups) group.Value.Draw(mRender);

            mRender.EndDraw();
        }

        internal static void Update(float delta)
        {
            foreach (var group in Groups) group.Value.Update(delta);
        }

        internal static void Input(Queue<InputAction> actions)
        {
            foreach (var action in actions)
            {
                foreach (var group in Groups)
                {
                    group.Value.Input(action);
                }
            }
        }

        internal static void Present(PresentRender render)
        {
            render.Mask(Canvas, new Rectangle(
                left: 0, top: 0,
                right: GameSystems.EngineWindow.Size.Width,
                bottom: GameSystems.EngineWindow.Size.Height));
        }

        static Gui()
        {
            Groups = new Dictionary<string, GuiGroup>();
            InputMapped = new InputMapped();
        }

        public static void Add(GuiGroup group) => Groups.Add(group.Name, group);
        public static void Remove(string name) => Groups.Remove(name);
        public static void Remove(GuiGroup group) => Groups.Remove(group.Name);

        public static void ReSizeCanvas(Size size)
        {
            if (Canvas?.Size == size) return;

            if (Canvas != null) Canvas.Dispose();

            Canvas = new Image(size, PixelFormat.RedBlueGreenAlpha8bit);
        }
    }
}
