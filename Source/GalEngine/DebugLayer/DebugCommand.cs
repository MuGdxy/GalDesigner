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

    public delegate bool CommandHandle(string[] commandParameters);

    //relative VisualLayer
    public static class DebugCommand
    {
        private const float borderX = 0.01f; //relative DebugCommand 
        private const float borderY = 0.01f; //relative DebugCommand

        private const float cursorShowTimeSpan = 0.5f; //0.5s

        //relative VisualLayer
        private class CommandItem
        {
            private static CanvasBrush TextBrush => ResourceList.BlackBrush;
            private static CanvasBrush BorderBrush => ResourceList.BlackBrush;

            private const float borderY = 0.1f;

            private string text;
            private float width;

            private CanvasText commandText;
            private TextMetrics textMetrics;

            public CommandItem(string Text, float Width)
            {
                text = Text;

                commandText = new CanvasText("> " + Text, width = Width, 0, LayerConfig.TextFormat);

                textMetrics = commandText.Metrics;
            }

            public void Reset(string Text, float Width)
            {
                if (text == Text && width == Width) return;

                text = Text;
                width = Width;

                commandText.Reset("> " + Text, width, 0, LayerConfig.TextFormat);

                textMetrics = commandText.Metrics;
            }

            public void OnRender()
            {
                Canvas.DrawText("> " + text, 0, 0, width, Height, LayerConfig.TextFormat, TextBrush);
            }

            public string Text
            {
                set => Reset(value, width);
                get => text;
            }

            public float Width
            {
                set => Reset(text, value);
                get => width;
            }

            public float Height => textMetrics.Height + borderY * LayerConfig.TextFormat.Size * 2;
        }

        private enum CommandErrorType
        {
            CommandParametersCount,
            CommandParametersFormat,
            CommandGlobalValueExist,
            CommandWatchValueExist
        }

        private static float width;
        private static float height;

        private static float startPosX;
        private static float startPosY;

        private static float realWidth;
        private static float realHeight;

        private static float realStartPosX;
        private static float realStartPosY;

        private static int cursorPosition = 0;
        private static float cursorRealPosition = 0;
        private static float cursorPassTime = 0;
        private static bool cursorState = false; //true is showing, false is hiding

        private static CanvasBrush cursorBrush = new CanvasBrush(0, 0, 0, 1);

        private static CanvasText inputCommand = new CanvasText("", float.MaxValue,
            CommandInputPadHeight, LayerConfig.TextFormat);

        private static Rect inputCommandRect = new Rect(); //relative DebugCommand
        private static Rect commandListRect = new Rect(); //relative DebugCommand

        private static List<CommandItem> commandList = new List<CommandItem>();

        private static Dictionary<CommandErrorType, string> CommandErrorText = new Dictionary<CommandErrorType, string>();

        private static float currentCommandListStart;
        private static float currentCommandListEnd;
        private static float maxCommandListHeight;

        private static float inputCommandOffset = 0; //+ is right , - is left, relative InputCommand

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

        private static void CursorStateUpdate()
        {
            cursorPassTime += Time.DeltaSeconds;

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

        private static void Insert(char word)
        {
            inputCommand.Insert(word, cursorPosition);
            CursorMoveRight();
        }

        private static void Remove()
        {
            if (cursorPosition is 0) return;
            
            CursorMoveLeft();
            inputCommand.Remove(cursorPosition, 1);
        }

        private static void ReportCommandError(CommandErrorType type)
        {
            WriteCommand(CommandErrorText[type]);
        }

        private static bool Assert(bool test, CommandErrorType type)
        {
            if (test is true) { ReportCommandError(type); return true; }
            return false;
        }

        private static string[] GetCommandParameters(string command)
        {
            return command.Split(' ');
        }

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

                    ClearCommand();

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

        internal static bool Contains(int realPositionX, int realPositionY)
        {
            float positionX = realPositionX - realStartPosX;
            float positionY = realPositionY - realStartPosY;

            return commandListRect.Contains(positionX, positionY);
        }

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

        internal static void SetPosition(float newStartPosX, float newStartPosY)
        {
            startPosX = newStartPosX;
            startPosY = newStartPosY;

            realStartPosX = VisualLayer.Width * startPosX;
            realStartPosY = VisualLayer.Height * startPosY;
        }

        static DebugCommand()
        {
            WriteCommand("Hi! Welcome to use Command.");

            CommandErrorText.Add(CommandErrorType.CommandParametersCount, "There is wrong number of CommandParameters!");
            CommandErrorText.Add(CommandErrorType.CommandParametersFormat, "There is wrong format of Command!");
            CommandErrorText.Add(CommandErrorType.CommandGlobalValueExist, "The value is not in GlobalValue!");
            CommandErrorText.Add(CommandErrorType.CommandWatchValueExist, "The value is not in WatchList!");
        }

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

        public static void SendCommand(string command)
        {
            WriteCommand(command);

            bool result = AnalyseCommand(command);

            if (result is false)
                WriteCommand("No Command Support!");
        }

        public static void ClearCommand()
        {
            commandList.Clear();

            currentCommandListStart = 0;
            currentCommandListEnd = 0;
            maxCommandListHeight = 0;
        }

        private static float CommandInputPadHeight => LayerConfig.TextFormat.Size * 2.5f;

        public static event CommandHandle CommandAnalyser;
    }
}
