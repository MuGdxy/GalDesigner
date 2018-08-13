using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class DebugCommandProperty
    {
        public static string Name => "DebugCommand";
        public static string LineNmae => "DebugCommandLine";

        public static string BackGround => "DebugCommandBackGround";

        public static string Font => "DebugCommandFont";
        
        internal static void Update(SizeF DebugCommandSize)
        {
            GameResource.SetFont(Font, new Font("Consolas", DebugCommandSize.Height * 0.04f));
        }

        static DebugCommandProperty()
        {
            GameResource.SetColor(BackGround, new Color(0.5f, 0.5f, 0.5f));
        }
    }

    class DebugCommandLine : GameObject
    {
        public DebugCommandLine(string Name, string Text) : base(Name, new SizeF())
        {
            TextLayout.Text = Text;
            TextLayout.Font = DebugCommandProperty.Font;
        }

        public void SetSharp(SizeF DebugCommandSize)
        {
            var textMetrics = TextLayout.ComputeTextMetrics(TextLayout, new SizeF(DebugCommandSize.Width, 0));

            Size = new SizeF(DebugCommandSize.Width, textMetrics.Height);
        }

        public void SetPosition(DebugCommandLine LastCommmandLine)
        {
            Transform.Position.Y = LastCommmandLine.Transform.Position.Y + (LastCommmandLine.Size.Height + Size.Height) * 0.5f;
        }
    }

    class DebugCommand : GameObject
    {
        private List<DebugCommandLine> debugCommandLines = new List<DebugCommandLine>();

        private void AddCommandLine(string Text)
        {
            DebugCommandLine commandLine = new DebugCommandLine(DebugCommandProperty.LineNmae + debugCommandLines.Count, Text);
            DebugCommandLine lastCommandLine = debugCommandLines[debugCommandLines.Count - 1];

            commandLine.SetPosition(lastCommandLine);
            
            debugCommandLines.Add(commandLine);

            SetChild(commandLine);
        }

        protected override void OnMouseClick(object sender, MouseClickEvent eventArg)
        {
            base.OnMouseClick(sender, eventArg);
        }

        protected override void OnMouseWheel(object sender, MouseWheelEvent eventArg)
        {
            float offset = eventArg.Offset;

            debugCommandLines[0].Transform.Position.Y = Math.Min(
                debugCommandLines[0].Transform.Position.Y + offset, -Size.Height * 0.5f);

            for (int i = 1; i < debugCommandLines.Count; i++)
            {
                debugCommandLines[i].SetPosition(debugCommandLines[i - 1]);
            }

            base.OnMouseWheel(sender, eventArg);
        }

        protected override void OnBoardClick(object sender, BoardClickEvent eventArg)
        {
            if (eventArg.IsDown is true && eventArg.KeyCode is KeyCode.Tab)
                IsEnableVisual ^= true;

            base.OnBoardClick(sender, eventArg);
        }

        public DebugCommand() : base(DebugCommandProperty.Name, new SizeF(0, 0))
        {
            Opacity = 0.7f;

            IsEnableVisual = false;
            IsEnableRead = true;

            BackGround.Color = DebugCommandProperty.BackGround;

            debugCommandLines.Add(new DebugCommandLine("RootCommandLine", ""));

            AddCommandLine("sasdasdsa");
            AddCommandLine("sasdasdsa"); AddCommandLine("sasdasdsa");
        }

        public void SetSharp(Size Resolution)
        {
            DebugCommandProperty.Update(Resolution);

            Size = Resolution;
            Transform.Position = new PositionF(Resolution.Width * 0.5f, Resolution.Height * 0.5f);

            Transform.Update();

            debugCommandLines[0].Transform.Position.Y = -Resolution.Height * 0.5f;
            debugCommandLines[0].IsEnableVisual = false;

            for (int i = 1; i < debugCommandLines.Count; i++)
            {
                debugCommandLines[i].SetSharp(Resolution);
                debugCommandLines[i].SetPosition(debugCommandLines[i - 1]);
            }
        }
    }
}
