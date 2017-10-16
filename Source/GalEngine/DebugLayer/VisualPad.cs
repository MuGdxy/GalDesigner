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
        private class VisualPad
        {
            private const float borderX = 3;
            private const float borderY = 2;

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

            private float startPosX;
            private float startPosY;

            private float currentTopItemOffset = -1;
            private float currentLastItemOffset = 0;
            private int currentTopItem = -1;
            private int currentLastItem = 0;

            private List<PadItem> itemList;

            private void SetItemAsTop(int itemID, float offset)
            {
                currentTopItem = itemID;
                currentTopItemOffset = offset;

                float contentHeight = ContentHeight;
                float contentWidth = ContentWidth;

                //height
                float height = itemList[itemID].Height + offset;

                for (int i = itemID + 1; i < itemList.Count; i++)
                {
                    height += itemList[i].Height;

                    if (height >= contentHeight)
                    {
                        currentLastItem = itemID;
                        currentLastItemOffset = contentHeight - height;
                    }
                }

                if (height < contentHeight)
                {
                    currentLastItem = itemList.Count - 1;
                    currentLastItemOffset = contentHeight - height;
                }
            }

            private void SetItemAsLast(int itemID, float offset)
            {
                currentLastItem = itemID;
                currentLastItemOffset = offset;

                float contenHeight = ContentHeight;

                float height = itemList[itemID].Height + offset;

                for (int i = itemID - 1; i >= 0; i--)
                {
                    height += itemList[i].Height;

                    if (height >= contenHeight)
                    {
                        currentTopItem = i;
                        currentTopItemOffset = contenHeight - height;
                    }
                }

                if (height < contenHeight)
                {
                    currentTopItem = 0;
                    currentTopItemOffset = contenHeight - height;
                }
            }

            public VisualPad()
            {
                itemList = new List<PadItem>();
                itemList.Add(new PadItem("t12"));
                itemList.Add(new PadItem("t23"));

                itemList.Add(new PadItem("t23"));
                itemList.Add(new PadItem("t23"));
                itemList.Add(new PadItem("t23"));
                itemList.Add(new PadItem("t23"));
                itemList.Add(new PadItem("t23"));
                itemList.Add(new PadItem("t23"));
                itemList.Add(new PadItem("t23"));

                SetItemAsTop(0, 0);
            }

            public void OnRender()
            {
                float realStartPosX = startPosX * VisualLayer.width;
                float realStartPosY = startPosY * VisualLayer.height;

                float realWidth = RealWidth;
                float realHeight = RealHeight;

                float contentWidth = ContentWidth;
                float contentHeight = ContentHeight;

                //Trnasform Pad
                Matrix3x2 transform = Matrix3x2.CreateTranslation(new Vector2(realStartPosX,
                    realStartPosY));

                Canvas.Transform = transform;

                //Render Pad border
                Canvas.DrawRectangle(0, 0, realWidth, realHeight, padBrush, 2);

                //Get Content's area
                Rect contentRect = new Rect(startPosX + borderX,
                    startPosY + borderY, startPosX + borderX + contentWidth,
                    startPosY + borderY + contentHeight);

                //Update PadItem
                for (int i = 0; i < itemList.Count; i++)
                    itemList[i].Reset(itemList[i].Text, contentWidth);

                //Render rontent layer
                Canvas.PushLayer(contentRect.Left, contentRect.Top,
                    contentRect.Right, contentRect.Bottom);

                Canvas.Transform *= Matrix3x2.CreateTranslation(new Vector2(borderX, borderY + currentTopItemOffset));

                for (int i = currentTopItem; i <= currentLastItem; i++)
                {
                    itemList[i].OnRender();

                    Canvas.Transform *= Matrix3x2.CreateTranslation(new Vector2(0, itemList[i].Height));
                }
                
                Canvas.PopLayer();

                Canvas.Transform = Matrix3x2.Identity;
            }

            public void SetArea(float Width, float Height)
            {
                width = Width;
                height = Height;

                SetItemAsTop(currentTopItem, currentTopItemOffset);
            }

            public void SetAreaKeeper(float baseValue, float targetAspectRatio, bool isWidth = true)
            {
                if (isWidth is true)
                {
                    width = baseValue;
                    height = width / targetAspectRatio * aspectRatio;
                }
                else

                {
                    height = baseValue;
                    width = height * targetAspectRatio * aspectRatio;
                }

                SetItemAsTop(currentTopItem, currentTopItemOffset);
            }

            public void SetPosition(float posX, float posY)
            {
                startPosX = posX;
                startPosY = posY;
            }

            public float RealWidth => width * VisualLayer.width;

            public float RealHeight => height * VisualLayer.height;

            public float ContentWidth => RealWidth - borderX * 2;

            public float ContentHeight => RealHeight - borderY * 2;

            public float Width => width;

            public float Height => height;

            public float StartPosX => startPosX;

            public float StartPosY => startPosY;
        }
    }
}
