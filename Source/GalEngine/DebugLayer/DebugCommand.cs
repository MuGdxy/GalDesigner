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

        public static string CommandNotice => "Command>";

        public static string BackGround => "DebugCommandBackGround";

        public static string Font => "DebugCommandFont";

        public static float ScrollSpeed = 0.5f;
        
        internal static void Update(SizeF DebugCommandSize)
        {
            GameResource.SetFont(Font, new Font("Consolas", DebugCommandSize.Height * 0.03f));
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
            Transform.Position.Y = LastCommmandLine.Transform.Position.Y + LastCommmandLine.Size.Height;
        }
    }

    class DebugCommand : GameObject
    {
        private List<DebugCommandLine> debugCommandLines = new List<DebugCommandLine>();

        private Camera camera = new Camera();

        public Camera Camera { get => camera; }

        private void AddCommandLine(string Text)
        {
            DebugCommandLine commandLine = new DebugCommandLine(DebugCommandProperty.LineNmae + debugCommandLines.Count, Text);
            DebugCommandLine lastCommandLine = debugCommandLines[debugCommandLines.Count - 2];

            commandLine.SetSharp(Size);
            commandLine.SetPosition(lastCommandLine);

            debugCommandLines[debugCommandLines.Count - 1].SetPosition(commandLine);

            debugCommandLines.Insert(debugCommandLines.Count - 1, commandLine);

            SetChild(commandLine);
        }

        private void ScrollCommandLine(float Offset)
        {
            var firstCommandLine = debugCommandLines[0];
            var lastCommandLine = debugCommandLines[debugCommandLines.Count - 1];

            float height = lastCommandLine.Transform.Position.Y - firstCommandLine.Transform.Position.Y + lastCommandLine.Size.Height;

            firstCommandLine.Transform.Position.Y += Offset;
            firstCommandLine.IsEnableVisual = false;

            firstCommandLine.Transform.Position.Y = Math.Max(Size.Height - height,
                firstCommandLine.Transform.Position.Y);
            firstCommandLine.Transform.Position.Y = Math.Min(0,
                firstCommandLine.Transform.Position.Y);

            for (int i = 1; i < debugCommandLines.Count; i++)
            {
                debugCommandLines[i].SetPosition(debugCommandLines[i - 1]);
            }
        }

        private void FocusCommand()
        {
            var commandLine = debugCommandLines[debugCommandLines.Count - 1];

            if (commandLine.Transform.Position.Y < 0 ||
                commandLine.Transform.Position.Y + commandLine.Size.Height > Size.Height)
            {
                ScrollCommandLine((Size.Height - commandLine.Size.Height) - commandLine.Transform.Position.Y);
            }
        }

        private void SendCommand(string Command)
        {

        }

        private void OnReadCommand(KeyCode KeyCode)
        {
            void AddCommandText(string Text)
            {
                debugCommandLines[debugCommandLines.Count - 1].TextLayout.Add(Text);

                FocusCommand();
            }

            void RemoveCommandText()
            {
                int length = debugCommandLines[debugCommandLines.Count - 1].TextLayout.Text.Length;

                if (length == DebugCommandProperty.CommandNotice.Length) return;

                debugCommandLines[debugCommandLines.Count - 1].TextLayout.Remove(length - 1);

                FocusCommand();
            }

            void SendCommandText()
            {
                var commandLine = debugCommandLines[debugCommandLines.Count - 1];
                float commandLineHalfHeight = commandLine.Size.Height * 0.5f;

                string command = commandLine.TextLayout.Text;

                AddCommandLine(command);
                SendCommand(command);

                commandLine.TextLayout.Text = DebugCommandProperty.CommandNotice;

                FocusCommand();
            }

            switch (KeyCode)
            {
                case KeyCode.Space: AddCommandText(" "); break;
                case KeyCode.Back: RemoveCommandText(); break;
                case KeyCode.Enter: SendCommandText(); break;
                default: break;
            }

            if (KeyCode >= KeyCode.A && KeyCode <= KeyCode.Z)
                AddCommandText(((char)((char)KeyCode - 'A' + 'a')).ToString());
        }

        protected override void OnMouseClick(object sender, MouseClickEvent eventArg)
        {
            base.OnMouseClick(sender, eventArg);
        }

        protected override void OnMouseWheel(object sender, MouseWheelEvent eventArg)
        {
            if (IsEnableVisual == false) return;

            float offset = eventArg.Offset * DebugCommandProperty.ScrollSpeed;

            ScrollCommandLine(offset);

            base.OnMouseWheel(sender, eventArg);
        }

        protected override void OnBoardClick(object sender, BoardClickEvent eventArg)
        {
            if (eventArg.IsDown is true)
            {
                if (eventArg.KeyCode == KeyCode.Tab)
                {
                    IsEnableVisual ^= true;
                    return;
                }

                OnReadCommand(eventArg.KeyCode);
            }

            base.OnBoardClick(sender, eventArg);
        }

        public DebugCommand() : base(DebugCommandProperty.Name, new SizeF(0, 0))
        {
            Opacity = 0.7f;

            IsEnableVisual = false;
            IsEnableRead = true;

            BackGround.Color = DebugCommandProperty.BackGround;

            debugCommandLines.Add(new DebugCommandLine("RootCommandLine", ""));
            debugCommandLines.Add(new DebugCommandLine("Command", "Command>"));

            foreach (var item in debugCommandLines)
            {
                SetChild(item);
            }

            AddCommandLine("Version 0.7beta");
            AddCommandLine("Welcome DebugComand.");
            AddCommandLine(" ");
        }

        public void SetSharp(Size Resolution)
        {
            DebugCommandProperty.Update(Resolution);

            Size = Resolution;
            Transform.Position = new PositionF(0, 0);

            Transform.Update(Size);

            camera = new Camera(0, 0, Resolution.Width, Resolution.Height);

            debugCommandLines[0].Transform.Position.Y = 0;
            debugCommandLines[0].IsEnableVisual = false;

            for (int i = 1; i < debugCommandLines.Count; i++)
            {
                debugCommandLines[i].SetSharp(Resolution);
                debugCommandLines[i].SetPosition(debugCommandLines[i - 1]);
            }
        }
    }
}
