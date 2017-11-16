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

    /// <summary>
    /// A surface to render debug message.
    /// </summary>
    static partial class VisualLayer
    {
        /// <summary>
        /// Information Pad.
        /// </summary>
        private static VisualPad infomationPad;
        
        /// <summary>
        /// Waring Message Pad.
        /// </summary>
        private static VisualPad warningPad;

        /// <summary>
        /// Watch Pad.
        /// </summary>
        private static VisualPad watchPad;

        /// <summary>
        /// VisualLayer width (pixel). It will be same as resolution-width.
        /// </summary>
        private static int width;
        
        /// <summary>
        /// VisualLayer height (pixel). It will be same as resolution-height.
        /// </summary>
        private static int height;

        /// <summary>
        /// The AspectRatio = width / height.
        /// </summary>
        private static float aspectRatio;

        /// <summary>
        /// Is enable VisualLayer.
        /// </summary>
        private static bool isEnable = false;

        /// <summary>
        /// The VisualLayer opacity.
        /// </summary>
        private static float opacity = 0.5f;

        /// <summary>
        /// Update VisualLayer's Pad size.
        /// </summary>
        /// <param name="newWidth">New VisualLayer Width (pixel).</param>
        /// <param name="newHeight">New VisualLayer Height (pixel).</param>
        private static void UpdatePads(int newWidth, int newHeight)
        {
            const float borderX = 0.01f;
            const float borderY = 0.01f;

            if (newWidth > newHeight)
            {
                //const
                const float leftAspectRatio = 16f / 10f;

                //Width > Height

                //Update Information Pad
                infomationPad.SetAreaKeeper(0.2f, leftAspectRatio, false);
                infomationPad.SetPosition(borderX, borderY);

                //Update Warning Pad 
                warningPad.SetArea(infomationPad.Width, 0.75f);
                warningPad.SetPosition(borderX, borderY + infomationPad.Height + borderY);

                //Update Watch Pad
                watchPad.SetArea(0.95f - infomationPad.Width,
                    infomationPad.Height + borderY + warningPad.Height - 0.5f);
                watchPad.SetPosition(infomationPad.Width + borderX * 2, borderY);

                //Update Command
                DebugCommand.SetArea(watchPad.Width, warningPad.Height +
                    warningPad.PositionY - (watchPad.PositionY + watchPad.Height + borderY));
                DebugCommand.SetPosition(watchPad.PositionX,
                    watchPad.PositionY + watchPad.Height + borderY);

            }else
            {
                throw new Exception("Not Support this Resolution!");
            }
        }

        /// <summary>
        /// Create VisualLayer.
        /// </summary>
        static VisualLayer()
        {
            infomationPad = new VisualPad("Information Pad");
            warningPad = new VisualPad("Waring Pad");
            watchPad = new VisualPad("Watch Pad");
        }

        /// <summary>
        /// On Resolution Change.
        /// </summary>
        /// <param name="newWidth">New VisualLayer Width (pixel).</param>
        /// <param name="newHeight">New VisualLayer Height (pixel).</param>
        internal static void OnResolutionChange(int newWidth, int newHeight)
        {
            aspectRatio = (width = newWidth) / (float)(height = newHeight);

            //Update Resource.
            LayerConfig.OnResizeResource(newWidth, newHeight);

            //Update Pad.
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
            infomationPad.SetItem("Fps", "Fps : " + GalEngine.GameWindow.Fps);

            //Render Information Pad.
            infomationPad.OnRender();
        }

        private static void OnRenderWarningPad()
        {  
            //We do not need update WarningPad Item

            //Render Waring Message Pad.
            warningPad.OnRender();
        }

        private static void OnRenderWatchPad()
        {
            //Update WatchPad Item When Text change
            foreach (var item in DebugLayer.WatchList)
            {
                watchPad.SetItem(item, item + " = " + GlobalValue.GetValue(item));
            }

            //Render Watch Pad.
            watchPad.OnRender();
        }

        /// <summary>
        /// Render.
        /// </summary>
        public static void OnRender()
        {
            //Set Layer to render.
            Canvas.PushLayer(0, 0, width, height, opacity);

            //Render BackGround
            Canvas.ClearBuffer(LayerConfig.BackGroundBrush.Red,
                LayerConfig.BackGroundBrush.Green, LayerConfig.BackGroundBrush.Blue);

            OnRenderInformationPad();

            OnRenderWarningPad();

            OnRenderWatchPad();

            //Render DebugCommand.
            DebugCommand.OnRender();

            //Finish Render and Render Layer to present surface.
            Canvas.PopLayer();
        }

        /// <summary>
        /// On mouse scroll
        /// </summary>
        /// <param name="realMousePosX">Mouse position-X (pixel).</param>
        /// <param name="realMousePosY">Mouse position-Y (pixel).</param>
        /// <param name="offset">Scroll offset.</param>
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

            if (DebugCommand.Contains(realMousePosX, realMousePosY) is true)
            {
                DebugCommand.OnMouseScroll(offset); return;
            }
        }

        /// <summary>
        /// On key event.
        /// </summary>
        /// <param name="e">Key event args.</param>
        public static void OnKeyEvent(KeyEventArgs e)
        {
            DebugCommand.OnKeyEvent(e);
        }

        /// <summary>
        /// Set PadItem.
        /// </summary>
        /// <param name="Tag">PadItem's tag.</param>
        /// <param name="Text">PadItem's text.</param>
        /// <param name="PadType">In which Pad.</param>
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

        /// <summary>
        /// Remove PadItem.
        /// </summary>
        /// <param name="Tag">PadItem's tag.</param>
        /// <param name="PadType">In which Pad.</param>
        public static void RemovePadItem(string Tag, PadType PadType)
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

        /// <summary>
        /// Is enable VisualLayer.
        /// </summary>
        public static bool IsEnable
        {
            set => isEnable = value;
            get => isEnable;
        }

        /// <summary>
        /// VisualLayer height (pixel). It will be same as resolution-height.
        /// </summary>
        public static int Height => height;


        /// <summary>
        /// VisualLayer width (pixel). It will be same as resolution-width.
        /// </summary>
        public static int Width => width;
    }
}
