using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using Builder;
using Presenter;

namespace GalEngine
{
    using ResourceList = Internal.ResourceList;

    /// <summary>
    /// A handle to solve the input command.
    /// </summary>
    /// <param name="commandParameters">The command. We will divide it to some string (split by ' ').</param>
    /// <returns></returns>
    public delegate bool CommandHandle(string[] commandParameters);

    /// <summary>
    /// Debug Command.
    /// </summary>
    public static class DebugCommand
    {
        /// <summary>
        /// The default distance to DebugCommand border-X (relative DebugCommand).
        /// </summary>
        private const float borderX = 0.02f;

        /// <summary>
        /// The default distance to DebugCommand border-Y (relative DebugCommand).
        /// </summary>
        private const float borderY = 0.02f;

        /// <summary>
        /// The border size. (relative DebugCommand)
        /// </summary>
        private const float borderSize = 0.001f;

        /// <summary>
        /// The cursor show timespan (s).
        /// </summary>
        private const float cursorShowTimeSpan = 0.5f;

        /// <summary>
        /// The cursor size. (relative DebugCommand Width) 
        /// </summary>
        private const float cursorStrokeSize = 0.001f;

        /// <summary>
        /// The MouseScroll offset speed (relative DebugCommand Height).
        /// </summary>
        private const float offsetSpeed = 0.001f;

        /// <summary>
        /// DebugCommand Border Brush.
        /// </summary>
        private static CanvasBrush BorderBrush => ResourceList.BlackBrush;

        /// <summary>
        /// DebugCommand Text Brush.
        /// </summary>
        private static CanvasBrush TextBrush => ResourceList.BlackBrush;

        /// <summary>
        /// Command Message Item. We use this to show the command.
        /// </summary>
        private class CommandItem
        {
            /// <summary>
            /// The distance from text border-Y to CommandItem's border-Y (relative text's size).
            /// </summary>
            private const float borderY = 0.1f;

            /// <summary>
            /// CommandItem text.
            /// </summary>
            private string text;

            /// <summary>
            /// CommandItem Width (pixel).
            /// </summary>
            private float width;

            /// <summary>
            /// CommandItem text instance for rendering.
            /// </summary>
            private CanvasText commandText;

            /// <summary>
            /// The information about our text.
            /// </summary>
            private TextMetrics textMetrics;

            /// <summary>
            /// The Text Brush.
            /// </summary>
            private CanvasBrush textBrush = ResourceList.BlackBrush;

            /// <summary>
            /// Create a CommmandItem.
            /// </summary>
            /// <param name="Text">The CommandItem text.</param>
            /// <param name="Width">The CommandItem width (pixel).</param>
            public CommandItem(string Text, float Width)
            {
                text = Text;

                commandText = new CanvasText("> " + text, width = Width, 0, ResourceList.DefaultTextFormat);

                textMetrics = commandText.Metrics;

                commandText.Reset("> " + text, width, Height);
            }

            /// <summary>
            /// Reset the CommmandItem.
            /// </summary>
            /// <param name="Text">The CommandItem text.</param>
            /// <param name="Width">The CommandItem width (pixel).</param>
            public void Reset(string Text, float Width)
            {
                if (text == Text && width == Width) return;

                text = Text;
                width = Width;

                commandText.Reset("> " + text, width, 0);

                textMetrics = commandText.Metrics;

                commandText.Reset("> " + text, width, Height);
            }

            /// <summary>
            /// OnRender.
            /// </summary>
            public void OnRender()
            {
                Canvas.DrawText(0, 0, commandText, TextBrush);
                //Canvas.DrawText("> " + text, 0, 0, width, Height, LayerConfig.TextFormat, TextBrush);
            }

            /// <summary>
            /// CommandItem text.
            /// </summary>
            public string Text
            {
                set => Reset(value, width);
                get => text;
            }

            /// <summary>
            /// CommandItem width (pixel).
            /// </summary>
            public float Width
            {
                set => Reset(text, value);
                get => width;
            }

            /// <summary>
            /// CommandItem height (pixel).
            /// </summary>
            public float Height => textMetrics.Height + borderY * ResourceList.DefaultTextFormat.Size * 2;

            /// <summary>
            /// The Text Brush.
            /// </summary>
            public CanvasBrush TextBrush { get => textBrush; set => textBrush = value; }
        }

        /// <summary>
        /// Command error type.
        /// </summary>
        private enum CommandErrorType
        {
            /// <summary>
            /// Parameters count is not right.
            /// </summary>
            CommandParametersCount,
            /// <summary>
            /// Parameters format is not right.
            /// </summary>
            CommandParametersFormat,
            /// <summary>
            /// The value is not in GlobalValue.
            /// </summary>
            CommandGlobalValueExist,
            /// <summary>
            /// The value is not watching.
            /// </summary>
            CommandWatchValueExist
        }

        /// <summary>
        /// Enable or Disable the DebugCommand.
        /// </summary>
        private static bool isEnable = false;

        /// <summary>
        /// DebugCommand width (pixel).
        /// </summary>
        private static float width;

        /// <summary>
        /// DebugCommand height (pixel).
        /// </summary>
        private static float height;

        /// <summary>
        /// The VisualLayer opacity.
        /// </summary>
        private static float opacity = 0.5f;

        /// <summary>
        /// The input cursor position (char).
        /// </summary>
        private static int cursorPosition = 0;

        /// <summary>
        /// The input cursor position (pixel).
        /// </summary>
        private static float cursorRealPosition = 0;

        /// <summary>
        /// The input cursor pass time from last count.
        /// </summary>
        private static float cursorPassTime = 0;

        /// <summary>
        /// The input cursor state. If true then show it, else hide it.
        /// </summary>
        private static bool cursorState = false;

        /// <summary>
        /// The cursor brush.
        /// </summary>
        private static CanvasBrush cursorBrush = ResourceList.BlackBrush;

        /// <summary>
        /// The input command instance.
        /// </summary>
        private static CanvasText inputCommand = new CanvasText("", float.MaxValue,
            CommandInputPadHeight, ResourceList.DefaultTextFormat);

        /// <summary>
        /// The input command rect (relative DebugCommand, pixel).
        /// </summary>
        private static Rect inputCommandRect = new Rect();

        /// <summary>
        /// The command list rect (relative DebugCommmand, pixel).
        /// </summary>
        private static Rect commandListRect = new Rect();

        /// <summary>
        /// The command list. We use it to record the message that we want to show.
        /// </summary>
        private static List<CommandItem> commandList = new List<CommandItem>();

        /// <summary>
        /// Map the error to text.
        /// </summary>
        private static Dictionary<CommandErrorType, string> CommandErrorText = new Dictionary<CommandErrorType, string>();

        /// <summary>
        /// </summary>
        private static float currentCommandListStart;

        /// <summary>
        /// </summary>
        private static float currentCommandListEnd;

        /// <summary>
        /// </summary>
        private static float maxCommandListHeight;

        /// <summary>
        /// The input command offset the start position-X of <see cref="DebugCommand.inputCommandRect"/>.
        /// And the + is right, - is left (relative InputCommand, pixel).
        /// </summary>
        private static float inputCommandOffset = 0; //+ is right , - is left, relative InputCommand

        /// <summary>
        /// Cursor move left.
        /// </summary>
        private static void CursorMoveLeft()
        {
            if (cursorPosition is 0) return;

            float currentX = inputCommand.HitTestPosition(cursorPosition, false).X;
            float nextX = inputCommand.HitTestPosition(cursorPosition - 1, false).X;

            cursorRealPosition -= currentX - nextX;
            //if the cursorRealPosition < the left border, move InputCommand right
            if (cursorRealPosition < 0)
            {
                inputCommandOffset -= cursorRealPosition;
                cursorRealPosition = 0;
            }

            cursorPosition--;
        }

        /// <summary>
        /// Cursor move right.
        /// </summary>
        private static void CursorMoveRight()
        {
            if (cursorPosition == inputCommand.Text.Length) return;

            float currentX = inputCommand.HitTestPosition(cursorPosition, false).X;
            float nextX = inputCommand.HitTestPosition(cursorPosition + 1, false).X;

            cursorRealPosition += nextX - currentX;
            //if the cursorRealPosition > the right border, move InputCommand left
            if (cursorRealPosition > inputCommandRect.Right - inputCommandRect.Left)
            {
                float width = inputCommandRect.Right - inputCommandRect.Left;

                inputCommandOffset -= cursorRealPosition - width;
                cursorRealPosition = width;
            }

            cursorPosition++;
        }

        /// <summary>
        /// Render and update cursor.
        /// </summary>
        private static void CursorStateUpdate()
        {
            cursorPassTime += Time.DeltaSeconds;

            //Check the cursor state.
            if (cursorPassTime >= cursorShowTimeSpan)
            {
                cursorPassTime -= cursorShowTimeSpan;
                cursorState ^= true;
            }

            if (cursorState is true)
            {
                float inputStartPosition = height - CommandInputPadHeight;
                float offset = (CommandInputPadHeight - ResourceList.DefaultTextFormat.Size) / 2.3f;

                Canvas.DrawLine(inputCommandRect.Left + cursorRealPosition, inputStartPosition + offset,
                    inputCommandRect.Left + cursorRealPosition, height - offset, cursorBrush, width * cursorStrokeSize);
            }
        }

        /// <summary>
        /// Insert a word to InputCommand (position is decide to cursor).
        /// </summary>
        /// <param name="word">The word.</param>
        private static void Insert(char word)
        {
            inputCommand.Insert(word, cursorPosition);
            CursorMoveRight();
        }

        /// <summary>
        /// Remove a word from InputCommand (position is decide to cursor).
        /// </summary>
        private static void Remove()
        {
            if (cursorPosition is 0) return;

            CursorMoveLeft();
            inputCommand.Remove(cursorPosition, 1);
        }

        /// <summary>
        /// Make a error.
        /// </summary>
        /// <param name="type">Error type.</param>
        private static void ReportCommandError(CommandErrorType type)
        {
            WriteCommand(CommandErrorText[type]);
        }

        /// <summary>
        /// Test value. If true, report error.
        /// </summary>
        /// <param name="testValue">Test value.</param>
        /// <param name="type">Error type.</param>
        /// <returns>Test value.</returns>
        private static bool Assert(bool testValue, CommandErrorType type)
        {
            if (testValue is true) { ReportCommandError(type); return true; }
            return false;
        }

        /// <summary>
        /// Split a command to Parameters.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <returns>Command Parameters</returns>
        private static string[] GetCommandParameters(string command)
        {
            return command.Split(' ');
        }

        /// <summary>
        /// Analyse and run Command.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <returns>If input string is not a command, return true.</returns>
        private static bool AnalyseCommand(string command)
        {
            string[] commandParameters = GetCommandParameters(command);

            switch (commandParameters[0])
            {
                case "Set":
                    if (Assert(commandParameters.Length != 3, CommandErrorType.CommandParametersCount) is true)
                        return true;

                    if (Assert(GlobalValue.Contains(commandParameters[1]) is false, CommandErrorType.CommandGlobalValueExist) is true)
                        return true;

                    string valueName = commandParameters[1];
                    string value = commandParameters[2];

                    switch (GlobalValue.GetValue(valueName))
                    {
                        case int intValue:
                            GlobalValue.SetValue(valueName, Convert.ToInt32(value));
                            break;

                        case bool boolValue:
                            GlobalValue.SetValue(valueName, Convert.ToBoolean(value));
                            break;

                        case float floatValue:
                            GlobalValue.SetValue(valueName, (float)Convert.ToDouble(value));
                            break;

                        case string stringValue:
                            GlobalValue.SetValue(valueName, Convert.ToString(value));
                            break;

                        default:
                            break;
                    }
                    return true;

                case "Get":
                    if (Assert(commandParameters.Length != 2, CommandErrorType.CommandParametersCount) is true)
                        return true;

                    if (Assert(GlobalValue.Contains(commandParameters[1]) is false, CommandErrorType.CommandGlobalValueExist) is true)
                        return true;

                    WriteCommand(commandParameters[1] + " = " + GlobalValue.GetValue(commandParameters[1]));

                    return true;

                case "Clear":
                    if (Assert(commandParameters.Length != 1, CommandErrorType.CommandParametersCount) is true)
                        return true;

                    ClearCommandList();

                    return true;
                case "SpecialThanks":
                    WriteCommand(SystemProperty.SpecialThanksName, ResourceList.GoldBrush);

                    return true;

                default:
                    break;
            }

            //Custom Command, if the command is not a system command , we will try this.
            if (CommandAnalyser != null)
            {
                bool result = false;

                foreach (CommandHandle item in CommandAnalyser.GetInvocationList())
                {
                    result |= item.Invoke(commandParameters);
                }

                return result;
            }

            return false;
        }

        /// <summary>
        /// Test a point that if DebugCommand contains.
        /// </summary>
        /// <param name="realPositionX">Point position-X (pixel).</param>
        /// <param name="realPositionY">Point position-Y (pixel).</param>
        /// <returns></returns>
        internal static bool Contains(int realPositionX, int realPositionY)
        {
            return commandListRect.Contains(realPositionX, realPositionY);
        }

        /// <summary>
        /// On mouse scroll.
        /// </summary>
        /// <param name="offset">Offset</param>
        internal static void OnMouseScroll(float offset)
        {
            offset *= height * offsetSpeed;
            offset *= -1;

            float commandListHeight = commandListRect.Bottom - commandListRect.Top;

            currentCommandListStart += offset;
            currentCommandListEnd = currentCommandListStart + commandListHeight;

            if (offset > 0) // Down
            {
                if (currentCommandListEnd > maxCommandListHeight)
                {
                    currentCommandListEnd = maxCommandListHeight;
                    currentCommandListStart = currentCommandListEnd - commandListHeight;
                }
            }
            else
            {
                if (currentCommandListStart < 0)
                {
                    currentCommandListStart = 0;
                    currentCommandListEnd = currentCommandListStart + commandListHeight;
                }
            }
        }

        /// <summary>
        /// On key event.
        /// </summary>
        /// <param name="e">Key event args.</param>
        internal static void OnKeyEvent(KeyEventArgs e)
        {
            if (e.IsDown is true)
            {
                switch (e.KeyCode)
                {
                    case KeyCode.Left:
                        CursorMoveLeft();
                        break;

                    case KeyCode.Right:
                        CursorMoveRight();
                        break;

                    case KeyCode.Back:
                        Remove();
                        break;

                    case KeyCode.OemMinus:
                        if (Application.IsKeyDown(KeyCode.ShiftKey) is true)
                            Insert('_');
                        break;

                    case KeyCode.Enter:
                        SendCommand(inputCommand.Text);

                        inputCommand.Reset("", float.MaxValue, CommandInputPadHeight, ResourceList.DefaultTextFormat);

                        inputCommandOffset = 0;
                        cursorPosition = 0;
                        cursorRealPosition = 0;
                        break;

                    case KeyCode.Space:

                        Insert(' ');
                        break;
                    default:
                        break;
                }

                bool specialMode = Application.IsCapsLock ^ Application.IsKeyDown(KeyCode.ShiftKey);

                if (e.KeyCode >= KeyCode.A && e.KeyCode <= KeyCode.Z)
                {
                    if (specialMode is true)
                        Insert((char)e.KeyCode);
                    else Insert((char)(e.KeyCode + 'a' - 'A'));
                }

                if (e.KeyCode >= KeyCode.D0 && e.KeyCode <= KeyCode.D9)
                    Insert((char)e.KeyCode);
            }
        }

        /// <summary>
        /// Render.
        /// </summary>
        internal static void OnRender()
        {
            Canvas.PushLayer(0, 0, width, height, opacity);

            Canvas.ClearBuffer(ResourceList.DefaultBackGroundBrush.Red, ResourceList.DefaultBackGroundBrush.Green,
                ResourceList.DefaultBackGroundBrush.Blue, ResourceList.DefaultBackGroundBrush.Alpha);

            //Render CommandText
            if (commandList.Count != 0)
            {
                Canvas.PushAxisAlignedClip(commandListRect.Left, commandListRect.Top, commandListRect.Right, commandListRect.Bottom);

                Matrix3x2 currentTransform = Matrix3x2.CreateTranslation(new Vector2(commandListRect.Left,
                    commandListRect.Top));

                Canvas.Transform = currentTransform;

                float currentHeight = 0;
                float commandListRectHeight = commandListRect.Bottom - commandListRect.Top;

                //Make sure the postion is right
                if (maxCommandListHeight < commandListRectHeight)
                {
                    currentCommandListStart = 0;
                    currentCommandListEnd = commandListRectHeight;
                }

                //Render CommandText List
                foreach (var item in commandList)
                {
                    float height = item.Height;

                    if (currentHeight + height >= currentCommandListStart
                            && currentHeight <= currentCommandListEnd)
                    {
                        Canvas.Transform = currentTransform * Matrix3x2.CreateTranslation(0, currentHeight - currentCommandListStart);

                        item.OnRender();
                    }

                    currentHeight += height;
                }

                Canvas.PopAxisAlignedClip();
            }

            Canvas.Transform = Matrix3x2.Identity;

            float inputStartPosition = height - CommandInputPadHeight;

            //Render Command Input Rect
            Canvas.DrawRectangle(0, inputStartPosition, width,
                height, BorderBrush, borderSize * width);

            //Render CommandText
            Canvas.PushAxisAlignedClip(inputCommandRect.Left - borderSize * width,
                inputCommandRect.Top, inputCommandRect.Right + borderSize * width, inputCommandRect.Bottom);

            Canvas.DrawText(inputCommandRect.Left + inputCommandOffset, inputStartPosition,
                inputCommand, TextBrush);

            //Render and Update Cursor
            CursorStateUpdate();

            Canvas.PopAxisAlignedClip();

            Canvas.Transform = Matrix3x2.Identity;

            Canvas.PopLayer();
        }

        /// <summary>
        /// Reset the DebugCommand Size.
        /// </summary>
        /// <param name="newWidth">New Width. (pixel)</param>
        /// <param name="newHeight">New Height. (pixel)</param>
        internal static void ReSize(int newWidth, int newHeight)
        {
            width = newWidth;
            height = newHeight;

            commandListRect = new Rect(borderX * width * 0.5f,
                borderY * height, width, height - CommandInputPadHeight);

            inputCommandRect = new Rect(borderX * width, height - CommandInputPadHeight,
                width * (1 - borderX), height);

            //Make sure the InputCommand is right
            inputCommand.Reset(inputCommand.Text, float.MaxValue, CommandInputPadHeight);

            //Make sure the CommandText's width and MaxListHeight is right
            maxCommandListHeight = 0;
            float rectWidth = commandListRect.Right - commandListRect.Left;
            foreach (var item in commandList)
            {
                item.Reset(item.Text, rectWidth);

                maxCommandListHeight += item.Height;
            }

            currentCommandListEnd = currentCommandListStart + maxCommandListHeight;
        }

        /// <summary>
        /// Write a command to Command List.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="brush">Command Text Brush.</param>
        /// <param name="isGoBottom">Is go bottom.</param>
        public static void WriteCommand(string command, CanvasBrush brush, bool isGoBottom = true)
        {
            CommandItem newItem = new CommandItem(command, commandListRect.Right - commandListRect.Left);

            if (brush is null)
                newItem.TextBrush = ResourceList.BlackBrush;
            else newItem.TextBrush = brush;

            commandList.Add(newItem);

            maxCommandListHeight += newItem.Height;

            if (isGoBottom is true)
            {
                currentCommandListEnd = maxCommandListHeight;
                currentCommandListStart = currentCommandListEnd - (commandListRect.Bottom - commandListRect.Top);
            }
        }

        /// <summary>
        /// Create DebugCommand.
        /// </summary>
        static DebugCommand()
        {
            WriteCommand("Hi! Welcome to use Command.");

            CommandErrorText.Add(CommandErrorType.CommandParametersCount, "There is wrong number of CommandParameters!");
            CommandErrorText.Add(CommandErrorType.CommandParametersFormat, "There is wrong format of Command!");
            CommandErrorText.Add(CommandErrorType.CommandGlobalValueExist, "The value is not in GlobalValue!");
            CommandErrorText.Add(CommandErrorType.CommandWatchValueExist, "The value is not in WatchList!");
        }

        /// <summary>
        /// Write a command to Command List.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="isGoBottom">Is go bottom.</param>
        public static void WriteCommand(string command, bool isGoBottom = true)
        {
            WriteCommand(command, ResourceList.BlackBrush);
        }

        /// <summary>
        /// Send a command that we will anlyse and run it.
        /// </summary>
        /// <param name="command">Command.</param>
        public static void SendCommand(string command)
        {
            WriteCommand(command);

            bool result = AnalyseCommand(command);

            if (result is false)
                WriteCommand("No Command Support!", Internal.ResourceList.RedBrush);
        }

        /// <summary>
        /// Clear Command List.
        /// </summary>
        public static void ClearCommandList()
        {
            commandList.Clear();

            currentCommandListStart = 0;
            currentCommandListEnd = 0;
            maxCommandListHeight = 0;
        }

        /// <summary>
        /// CommandItem height (pixel).
        /// </summary>
        private static float CommandInputPadHeight => ResourceList.DefaultTextFormat.Size * 2.5f;

        /// <summary>
        /// Enable or Disable the DebugCommand.
        /// </summary>
        public static bool IsEnable { get => isEnable; set => isEnable = value; }

        /// <summary>
        /// Command Analyser.
        /// </summary>
        public static event CommandHandle CommandAnalyser;


    }
}
