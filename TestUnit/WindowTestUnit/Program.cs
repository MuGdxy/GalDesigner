﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace WindowTestUnit
{
    class Program
    {

        static void InitializeWindowEvent()
        {
            GameSystems.EngineWindow.OnKeyBoardEvent += (object sender, KeyBoardEventArg eventArg) =>
            {
                LogEmitter.Apply(LogLevel.Information, "[KeyBoardEvent] [KeyCode = {0}] [State = {1}]",
                    eventArg.KeyCode, eventArg.IsDown);
            };

            GameSystems.EngineWindow.OnMouseMoveEvent += (object sender, MouseMoveEventArg eventArg) =>
            {
                LogEmitter.Apply(LogLevel.Information, "[MouseMoveEvent] [X = {0}] [Y = {1}]", eventArg.Position.X, eventArg.Position.Y);
            };

            GameSystems.EngineWindow.OnMouseClickEvent += (object sender, MouseClickEventArg eventArg) =>
            {
                LogEmitter.Apply(LogLevel.Information, "[MouseClickEvent] [Button = {0}] [State = {1}]", eventArg.Button, eventArg.IsDown);
            };

            GameSystems.EngineWindow.OnMouseWhellEvent += (object sender, MouseWheelEventArg eventArg) =>
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
            GameSystems.Initialize(new GameStartInfo()
            {
                WindowName = "WindowTestUnit",
                GameName = "WindowTestUnit",
                IconName = "",
                WindowSize = new Size<int>(1920, 1080)
            });

            InitializeWindowEvent();

            GameSystems.RunLoop();
        }
    }
}
