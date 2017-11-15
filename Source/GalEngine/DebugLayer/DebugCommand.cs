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
    using LayerConfig = VisualLayerConfig;
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
        private const float borderX = 0.01f;

        /// <summary>
        /// The default distance to DebugCommand border-Y (relative DebugCommand).
        /// </summary>
        private const float borderY = 0.01f;

        /// <summary>
        /// The cursor show timespan (s).
        /// </summary>
        private const float cursorShowTimeSpan = 0.5f; 
        
        /// <summary>
        /// Command Message Item. We use this to show the command.
        /// </summary>
        private class CommandItem
        {
            /// <summary>
            /// The Text Brush.
            /// </summary>
            private static CanvasBrush TextBrush => ResourceList.BlackBrush;

            /// <summary>
            /// The Border Brush.
            /// </summary>
            private static CanvasBrush BorderBrush => ResourceList.BlackBrush;


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
            /// Create a CommmandItem.
            /// </summary>
            /// <param name="Text">The CommandItem text.</param>
            /// <param name="Width">The CommandItem width (pixel).</param>
            public CommandItem(string Text, float Width)
            {
                text = Text;

                commandText = new CanvasText("> " + Text, width = Width, 0, LayerConfig.TextFormat);

                textMetrics = commandText.Metrics;
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

                commandText.Reset("> " + Text, width, 0, LayerConfig.TextFormat);

                textMetrics = commandText.Metrics;
            }

            /// <summary>
            /// OnRender.
            /// </summary>
            public void OnRender()
            {
                Canvas.DrawText("> " + text, 0, 0, width, Height, LayerConfig.TextFormat, TextBrush);
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
            public float Height => textMetrics.Height + borderY * LayerConfig.TextFormat.Size * 2;
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
        /// DebugCommmand width (relative VisualLayer).
        /// </summary>
        private static float width;
        
        /// <summary>
        /// DebugCommand height (relative VisualLayer).
        /// </summary>
        private static float height;

        /// <summary>
        /// DebugCommand position-X (relative VisualLayer).
        /// </summary>
        private static float startPosX;

        /// <summary>
        /// DebugCommand position-Y (relative VisualLayer).
        /// </summary>
        private static float startPosY;

        /// <summary>
        /// DebugCommand width (pixel).
        /// </summary>
        private static float realWidth;

        /// <summary>
        /// DebugCommand height (pixel).
        /// </summary>
        private static float realHeight;

        /// <summary>
        /// DebugCommand position-X (pixel).
        /// </summary>
        private static float realStartPosX;

        /// <summary>
        /// DebugCommmand position-Y (pixel).
        /// </summary>
        private static float realStartPosY;

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
            CommandInputPadHeight, LayerConfig.TextFormat);

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
        /// See this: 
        /// <see cref="VisualLayer.VisualPad.currentContentStart"/>
        /// </summary>
        private static float currentCommandListStart;

        /// <summary>
        /// See this:
        /// <see cref="VisualLayer.VisualPad.currentContentEnd"/>
        /// </summary>
        private static float currentCommandListEnd;

        /// <summary>
        /// See this:
        /// <see cref="VisualLayer.VisualPad.maxItemHeight"/>
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
                float inputStartPosition = realHeight - CommandInputPadHeight;
                float offset = (CommandInputPadHeight - LayerConfig.TextFormat.Size) / 2.3f;

                Canvas.DrawLine(inputCommandRect.Left + cursorRealPosition, inputStartPosition + offset,
                    inputCommandRect.Left + cursorRealPosition, realHeight - offset, cursorBrush, LayerConfig.BorderSize);
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

                case "Watch":
                    if (Assert(commandParameters.Length != 2, CommandErrorType.CommandParametersCount) is true)
                        return true;

                    if (Assert(GlobalValue.Contains(commandParameters[1]) is false, CommandErrorType.CommandGlobalValueExist) is true)
                        return true;

                    DebugLayer.Watch(commandParameters[1]);

                    return true;

                case "UnWatch":
                    if (Assert(commandParameters.Length != 2, CommandErrorType.CommandParametersCount) is true)
                        return true;

                    if (Assert(GlobalValue.Contains(commandParameters[1]) is false, CommandErrorType.CommandGlobalValueExist) is true)
                        return true;

                    if (Assert(DebugLayer.WatchList.Contains(commandParameters[1]) is false, CommandErrorType.CommandWatchValueExist)
                        is true) return true;

                    DebugLayer.UnWatch(commandParameters[1]);

                    return true;

                case "Clear":
                    if (Assert(commandParameters.Length != 1, CommandErrorType.CommandParametersCount) is true)
                        return true;

                    ClearCommandList();

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
            float positionX = realPositionX - realStartPosX;
            float positionY = realPositionY - realStartPosY;

            return commandListRect.Contains(positionX, positionY);
        }

        /// <summary>
        /// On mouse scroll.
        /// </summary>
        /// <param name="offset">Offset</param>
        internal static void OnMouseScroll(float offset)
        {
            offset *= LayerConfig.OffsetSpeed;

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

                        inputCommand.Reset("", float.MaxValue, CommandInputPadHeight, LayerConfig.TextFormat);

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

                bool specialMode = Application.IsKeyDown(KeyCode.CapsLock) ^ Application.IsKeyDown(KeyCode.ShiftKey);

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
            Matrix3x2 transform = Matrix3x2.CreateTranslation(new Vector2(realStartPosX,
                realStartPosY));

            //Transform 
            Canvas.Transform = transform;

            //Render Command Border
            Canvas.DrawRectangle(0, 0, realWidth, realHeight, LayerConfig.BorderBrush, LayerConfig.BorderSize);

            //Render CommandText
            if (commandList.Count != 0)
            {
                Canvas.PushLayer(commandListRect.Left, commandListRect.Top,
                    commandListRect.Right, commandListRect.Bottom);

                Matrix3x2 currentTransform = transform * Matrix3x2.CreateTranslation(new Vector2(commandListRect.Left,
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

                Canvas.PopLayer();

                Canvas.Transform = transform;
            }


            float inputStartPosition = realHeight - CommandInputPadHeight;

            //Render Command Input Rect
            Canvas.DrawRectangle(0, inputStartPosition, realWidth,
                realHeight, LayerConfig.BorderBrush, LayerConfig.BorderSize);

            //Render CommandText
            Canvas.PushLayer(inputCommandRect.Left - LayerConfig.BorderSize,
                inputCommandRect.Top, inputCommandRect.Right + LayerConfig.BorderSize, inputCommandRect.Bottom);

            Canvas.DrawText(inputCommandRect.Left + inputCommandOffset, inputStartPosition,
                inputCommand, LayerConfig.TextBrush);

            //Render and Update Cursor
            CursorStateUpdate();

            Canvas.PopLayer();

            Canvas.Transform = Matrix3x2.Identity;
        }

        /// <summary>
        /// Set DebugCommand Size.
        /// </summary>
        /// <param name="Width">New width (relative VisualLayer).</param>
        /// <param name="Height">New height (relative VisualLayer).</param>
        internal static void SetArea(float Width, float Height)
        {
            width = Width;
            height = Height;

            realWidth = VisualLayer.Width * width;
            realHeight = VisualLayer.Height * height;

            realStartPosX = VisualLayer.Width * startPosX;
            realStartPosY = VisualLayer.Height * startPosY;

            commandListRect = new Rect(borderX * realWidth / 2, borderY * realHeight, realWidth,
                realHeight - CommandInputPadHeight);

            inputCommandRect = new Rect(borderX * realWidth, realHeight - CommandInputPadHeight,
                realWidth * (1 - borderX), realHeight);

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
        /// Set DebugCommand position.
        /// </summary>
        /// <param name="newStartPosX">New position-X (relative VisualLayer).</param>
        /// <param name="newStartPosY">New position-Y (relative VisualLayer).</param>
        internal static void SetPosition(float newStartPosX, float newStartPosY)
        {
            startPosX = newStartPosX;
            startPosY = newStartPosY;

            realStartPosX = VisualLayer.Width * startPosX;
            realStartPosY = VisualLayer.Height * startPosY;
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
            CommandItem newItem = new CommandItem(command, commandListRect.Right - commandListRect.Left);

            commandList.Add(newItem);

            maxCommandListHeight += newItem.Height;

            if (isGoBottom is true)
            {
                currentCommandListEnd = maxCommandListHeight;
                currentCommandListStart = currentCommandListEnd - (commandListRect.Bottom - commandListRect.Top);
            }
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
                WriteCommand("No Command Support!");
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
        private static float CommandInputPadHeight => LayerConfig.TextFormat.Size * 2.5f;

        /// <summary>
        /// Command Analyser.
        /// </summary>
        public static event CommandHandle CommandAnalyser;
    }
}
