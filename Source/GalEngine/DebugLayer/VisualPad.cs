using System;
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
        internal enum PadType
        {
            InformationPad,
            WarningPad,
            WatchPad
        }

        private class VisualPad
        {
            private const float borderX = 5;
            private const float borderY = 3;

            private class PadItem
            {
                private const float borderX = 0.1f;
                private const float borderY = 0.1f;

                private static CanvasBrush padItemBrush = new CanvasBrush(1, 0, 0, 1);
                
                private string text;
                private float width;

                private CanvasText canvasText;
                private TextMetrics textMetrics;

                public PadItem(string Text, float Width = 0)
                {
                    Reset(Text, Width);
                }

                public void Reset(string Text, float Width)
                {
                    if (text == Text && width == Width) return;

                    text = Text;
                    width = Width;

                    Utilities.Dipose(ref canvasText);

                    canvasText = new CanvasText(text, width, 0, textFormat);

                    textMetrics = canvasText.Metrics;
                }

                public void OnRender()
                {
                    Canvas.FillRectangle(0, 0, Width, Height, padItemBrush);

                    Canvas.DrawRectangle(0, 0, Width, Height, padBrush, 1f);

                    Canvas.DrawText(borderX * textFormat.Size, borderY * textFormat.Size,
                        canvasText, textBrush);
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

                public float Height => textMetrics.Height + borderY * textFormat.Size * 2;
            }

            private float width;
            private float height;

            private float realWidth;
            private float realHeight;

            private float contentWidth;
            private float contentHeight;

            private float startPosX;
            private float startPosY;

            private List<PadItem> itemList;
            private Dictionary<string, PadItem> itemIndex;

           
            public VisualPad()
            {
                itemList = new List<PadItem>();
                itemIndex = new Dictionary<string, PadItem>();
            }

            public void AddItem(string Tag, string Text)
            {
                PadItem padItem = new PadItem(Text, contentWidth);

                itemList.Add(padItem);
                itemIndex.Add(Tag, padItem);
            }

            public void RemoveItem(string Tag)
            {
                itemList.Remove(itemIndex[Tag]);
                itemIndex.Remove(Tag);
            }

            public void SetItem(string Tag, string Text)
            {
                if (itemIndex.ContainsKey(Tag) is false)
                {
                    AddItem(Tag, Text);
                    return;
                }

                itemIndex[Tag].Text = Text;
            }

            public void OnMouseScroll(int offset)
            {

            }

            public void OnRender()
            {
                float realStartPosX = startPosX * VisualLayer.width;
                float realStartPosY = startPosY * VisualLayer.height;

                //Trnasform Pad
                Matrix3x2 transform = Matrix3x2.CreateTranslation(new Vector2(realStartPosX,
                    realStartPosY));

                Canvas.Transform = transform;

                //Render Pad border
                Canvas.DrawRectangle(0, 0, realWidth, realHeight, padBrush, 2);

                if (itemList.Count != 0)
                {
                    //Get Content's area
                    Rect contentRect = new Rect(startPosX + borderX,
                        startPosY + borderY, startPosX + borderX + contentWidth,
                        startPosY + borderY + contentHeight);

                    //Render content layer
                    Canvas.PushLayer(contentRect.Left, contentRect.Top,
                        contentRect.Right, contentRect.Bottom);

                    Canvas.PopLayer();
                }

                Canvas.Transform = Matrix3x2.Identity;
            }

            public void SetArea(float Width, float Height)
            {
                width = Width;
                height = Height;

                realWidth = width * VisualLayer.width;
                realHeight = height * VisualLayer.height;

                contentWidth = realWidth - borderX * 2;
                contentHeight = realHeight - borderY * 2;

                //Update PadItem When Size Change
                for (int i = 0; i < itemList.Count; i++)
                    itemList[i].Reset(itemList[i].Text, contentWidth);


            }

            public void SetAreaKeeper(float baseValue, float targetAspectRatio, bool isWidth = true)
            {
                if (isWidth is true)
                {
                    SetArea(baseValue, baseValue / targetAspectRatio * aspectRatio);
                }
                else

                {
                    SetArea(baseValue * targetAspectRatio * aspectRatio, baseValue);
                }
            }

            public void SetPosition(float posX, float posY)
            {
                startPosX = posX;
                startPosY = posY;
            }

            public float Width => width;

            public float Height => height;

            public float StartPosX => startPosX;

            public float StartPosY => startPosY;
        }
    }
}
