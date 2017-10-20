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
    static partial class VisualLayer
    {
        private const float borderX = 0.01f;
        private const float borderY = 0.01f;
        
        private static CanvasBrush padBrush = new CanvasBrush(0, 0, 0, 1);
        private static CanvasBrush textBrush = new CanvasBrush(0, 0, 0, 1);
        private static CanvasTextFormat textFormat = new CanvasTextFormat("Consolas", 10);

        private static CanvasBrush backGroundBrush = new CanvasBrush(1, 1, 1, 1);

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
            }else
            {
                throw new Exception("Not Support this Resolution");
            }
        }

        static VisualLayer()
        {
            infomationPad = new VisualPad("Information Pad");
            warningPad = new VisualPad("Waring Pad");
            watchPad = new VisualPad("Watch Pad");
        }

        internal static void OnResolutionChange(int newWidth, int newHeight)
        {
            aspectRatio = (width = newWidth) / (float)(height = newHeight);
            
            Utilities.Dipose(ref textFormat);

            textFormat = new CanvasTextFormat("Consolas", height * 0.027f);
            
            UpdatePads(width, height);
        }

        private static void OnRenderInformationPad()
        {
            //Update and Create Information PadItem
            infomationPad.SetItem(GlobalConfig.ApplicationName,
                "AppName : " + GlobalConfig.AppName);
            infomationPad.SetItem(GlobalConfig.WidthName,
                "Width : " + GlobalConfig.Width);
            infomationPad.SetItem(GlobalConfig.HeightName,
                "Height : " + GlobalConfig.Height);

            infomationPad.OnRender();
        }

        private static void OnRenderWarningPad()
        {  
            //We do not need update WarningPad Item

            warningPad.OnRender();
        }

        private static void OnRenderWatchPad()
        {
            //Update WatchPad Item When Text change
            foreach (var item in DebugLayer.WatchList)
            {
                watchPad.SetItem(item, item + " = " + GlobalValue.GetValue(item));
            }

            watchPad.OnRender();
        }

        public static void OnRender()
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

        public static void OnMouseScroll(int realMousePosX, int realMousePosY, int offset)
        {
            offset = -offset;

            if (infomationPad.Contains(realMousePosX, realMousePosY) is true)
            {
                infomationPad.OnMouseScroll(offset); return;
            }

            if (warningPad.Contains(realMousePosX, realMousePosY) is true)
            {
                warningPad.OnMouseScroll(offset); return;
            }

            if (watchPad.Contains(realMousePosX,realMousePosY) is true)
            {
                watchPad.OnMouseScroll(offset); return;
            }
        }

        public static void SetPadItem(string Tag, string Text, PadType PadType)
        {
            switch (PadType)
            {
                case PadType.InformationPad:
                    infomationPad.SetItem(Tag, Text);
                    break;
                case PadType.WarningPad:
                    warningPad.SetItem(Tag, Text);
                    break;
                case PadType.WatchPad:
                    watchPad.SetItem(Tag, Text);
                    break;
                default:
                    break;
            }
        }

        public static void RemovePadItem(string Tag,PadType PadType)
        {
            switch (PadType)
            {
                case PadType.InformationPad:
                    infomationPad.RemoveItem(Tag);
                    break;
                case PadType.WarningPad:
                    warningPad.RemoveItem(Tag);
                    break;
                case PadType.WatchPad:
                    watchPad.RemoveItem(Tag);
                    break;
                default:
                    break;
            }
        }

        public static bool IsEnable
        {
            set => isEnable = value;
            get => isEnable;
        }
    }
}
