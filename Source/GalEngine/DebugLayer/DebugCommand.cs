using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class DebugCommandProperty
    {
        private static float commandInputBoxHeightProportion = 0.07f;

        public static string Name => "DebugCommand";
        public static string CommandInputBoxName => "CommandInputBox";

        public static string BackGround => "DebugCommandBackGround";
        public static string CommandInputBoxBorderColor => "DebugCommandCommandInputBoxBorderColor";

        public static float CommandInputBoxHeightProportion => commandInputBoxHeightProportion;

        static DebugCommandProperty()
        {
            GameResource.SetColor(BackGround, new Color(0.5f, 0.5f, 0.5f));
            GameResource.SetColor(CommandInputBoxBorderColor, new Color(0, 0, 0));
        }
    }

    class DebugCommandInputBox : GameObject
    {
        public DebugCommandInputBox() : base(DebugCommandProperty.CommandInputBoxName, new Size())
        {
            Border.Width = 2.0f;
            Border.Color = DebugCommandProperty.CommandInputBoxBorderColor;
            
        }

        public void SetSharp(SizeF DebugCommandSize)
        {
                Size = new SizeF(DebugCommandSize.Width, DebugCommandSize.Height *
                DebugCommandProperty.CommandInputBoxHeightProportion);

            Transform.Position = new PositionF(0, (DebugCommandSize.Height - Size.Height) * 0.5f);

            Transform.Update();
        }
    }

    class DebugCommand : GameObject
    {
        private DebugCommandInputBox DebugCommandInputBox = new DebugCommandInputBox();

        protected override void OnBoardClick(object sender, BoardClickEvent eventArg)
        {
            if (eventArg.IsDown is true && eventArg.KeyCode is KeyCode.Tab)
                IsEnableVisual ^= true;

            base.OnBoardClick(sender, eventArg);
        }

        public DebugCommand() : base(DebugCommandProperty.Name, new Size(0, 0))
        {
            Opacity = 0.7f;

            IsEnableVisual = false;
            IsEnableRead = true;

            BackGround.Color = DebugCommandProperty.BackGround;

            SetChild(DebugCommandInputBox);
        }

        public void SetSharp(Size Resolution)
        {
            Size = Resolution;
            Transform.Position = new PositionF(Resolution.Width * 0.5f, Resolution.Height * 0.5f);

            Transform.Update();

            DebugCommandInputBox.SetSharp(Size);
        }
    }
}
