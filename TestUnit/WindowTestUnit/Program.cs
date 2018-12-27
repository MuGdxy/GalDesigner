using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace WindowTestUnit
{
    class Program
    {

        static void InitializeWindow()
        {
            GameSystems.EngineWindow = new EngineWindow("WindowTestUnit", "", new Size<int>(1920, 1080));
            GameSystems.EngineWindow.Show();

            GameSystems.EngineWindow.OnKeyBoardEvent += (object sender, KeyBoardEvent eventArg) =>
            {
                LogEmitter.Apply(LogLevel.Information, "[KeyBoardEvent] [KeyCode = {0}] [State = {1}]",
                    eventArg.KeyCode, eventArg.IsDown);
            };

            GameSystems.EngineWindow.OnMouseMoveEvent += (object sender, MouseMoveEvent eventArg) =>
            {
                LogEmitter.Apply(LogLevel.Information, "[MouseMoveEvent] [X = {0}] [Y = {1}]", eventArg.Position.X, eventArg.Position.Y);
            };

            GameSystems.EngineWindow.OnMouseClickEvent += (object sender, MouseClickEvent eventArg) =>
            {
                LogEmitter.Apply(LogLevel.Information, "[MouseClickEvent] [Button = {0}] [State = {1}]", eventArg.Button, eventArg.IsDown);
            };

            GameSystems.EngineWindow.OnMouseWhellEvent += (object sender, MouseWheelEvent eventArg) =>
            {
                LogEmitter.Apply(LogLevel.Information, "[MouseWheelEvent] [Offset = {0}]", eventArg.Offset);
            };

            GameSystems.EngineWindow.OnSizeChangeEvent += (object sender, SizeChangeEvent eventArg) =>
             {
                 LogEmitter.Apply(LogLevel.Information, "[SizeChangeEvent] [Width = {0}] [Height = {1}]",
                     eventArg.After.Width, eventArg.After.Height);
             };
        }

        static void Main(string[] args)
        { 
            GameSystems.Initialize();

            InitializeWindow();

            GameSystems.RunLoop();
        }
    }
}
