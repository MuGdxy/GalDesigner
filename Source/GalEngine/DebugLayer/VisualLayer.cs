﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using Presenter;

namespace GalEngine
{
    static partial class VisualLayer
    {
        private const float borderX = 0.01f;
        private const float borderY = 0.01f;
        
        private static CanvasBrush padBrush = new CanvasBrush(0, 0, 0, 1);
        private static CanvasBrush textBrush = new CanvasBrush(0, 0, 0, 1);
        private static CanvasTextFormat textFormat = new CanvasTextFormat("Consolas", 10);

        private static CanvasBrush backGroundBrush = new CanvasBrush(192 / 255f, 192 / 255f, 192 / 255f, 1f);

        private static VisualPad infomationPad;
        private static VisualPad warningPad;
        private static VisualPad watchPad;

        private static int width;
        private static int height;

        private static float aspectRatio;

        private static bool isEnable = false;

        private static float opacity = 0.5f;

        private static void UpdatePads(int newWidth, int newHeight)
        {
            if (newWidth > newHeight)
            {
                //const
                const float leftAspectRatio = 10f / 16f;

                //Width > Height

                //Update Information Pad
                infomationPad.SetAreaKeeper(0.2f, leftAspectRatio, false);
                infomationPad.SetPosition(borderX, borderY);

                //Update Warning Pad 
                warningPad.SetArea(infomationPad.Width, 0.75f);
                warningPad.SetPosition(borderX, borderY + infomationPad.Height + borderY);

                watchPad.SetArea(0.95f - infomationPad.Width,
                    infomationPad.Height + borderY + warningPad.Height);
                watchPad.SetPosition(infomationPad.Width + borderX * 2, borderY);
            }
            else
            {

            }
        }

        static VisualLayer()
        {
            infomationPad = new VisualPad();
            warningPad = new VisualPad();
            watchPad = new VisualPad();

            infomationPad.AddItem(GlobalConfig.ApplicationName, "AppName : " +
                GlobalConfig.AppName);
            infomationPad.AddItem(GlobalConfig.WidthName, "Width   : " +
                GlobalConfig.Width);
            infomationPad.AddItem(GlobalConfig.HeightName, "Height  : " +
                GlobalConfig.Height);
        }

        internal static void OnResolutionChange(int newWidth, int newHeight)
        {
            aspectRatio = (width = newWidth) / (float)(height = newHeight);
            
            Utilities.Dipose(ref textFormat);

            textFormat = new CanvasTextFormat("Consolas", height * 0.03f);
            
            UpdatePads(width, height);
        }

        private static void OnRenderInformationPad()
        {
            infomationPad.OnRender();
        }

        private static void OnRenderWarningPad()
        {
            warningPad.OnRender();
        }

        private static void OnRenderWatchPad()
        {
            watchPad.OnRender();
        }

        internal static void OnRender()
        {
            //Render Object 
            Canvas.PushLayer(0, 0, width, height, opacity);
            
            //Render BackGround
            Canvas.ClearBuffer(backGroundBrush.Red, backGroundBrush.Green, backGroundBrush.Blue);

            OnRenderInformationPad();

            OnRenderWarningPad();

            OnRenderWatchPad();

            Canvas.PopLayer();
        }

        public static void Enable()
        {
            DebugLayer.IsEnableVisualLayer = true;
            isEnable = true;
        }

        public static void UnEnable()
        {
            DebugLayer.IsEnableVisualLayer = false;
            isEnable = false;
        }

        public static bool IsEnable
        {
            set => isEnable = DebugLayer.IsEnableVisualLayer = value;
            get => isEnable;
        }
    }
}
