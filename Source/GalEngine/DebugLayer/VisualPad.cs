using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using Presenter;

namespace GalEngine
{
    ///Visual Layer Config
    using LayerConfig = VisualLayerConfig;

    static partial class VisualLayer
    {
        internal enum PadType
        {
            InformationPad,
            WarningPad,
            WatchPad
        }

        //relative visualLayer
        private class VisualPad
        {
            private const float borderX = 5;
            private const float borderY = 3;

            private class PadItem
            {
                private const float borderY = 0.1f;

                private static CanvasBrush padItemBrush = new CanvasBrush(192 / 255f, 192 / 255f, 192 / 255f, 1f);
                
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

                    canvasText = new CanvasText(text, width, 0, LayerConfig.TextFormat);

                    textMetrics = canvasText.Metrics;
                }

                public void OnRender()
                {
                    Canvas.FillRectangle(0, 0, Width, Height, padItemBrush);

                    Canvas.DrawRectangle(0, 0, Width, Height, LayerConfig.BorderBrush, LayerConfig.BorderSize / 2f);

                    Canvas.DrawText(text, LayerConfig.TitleOffsetX, 0, width, Height, LayerConfig.TextFormat, LayerConfig.TextBrush);
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

            private float width;
            private float height;

            private Rect realLayout;
            private float realTitleHeight;

            private float realWidth;
            private float realHeight;

            private float contentWidth;
            private float contentHeight;

            private float startPosX;
            private float startPosY;

            private float realStartPosX;
            private float realStartPosY;

            private string title;

            private List<PadItem> itemList;
            private Dictionary<string, PadItem> itemIndex;

            private float currentContentStart;
            private float currentContentEnd;
            private float maxItemHeight;

            public VisualPad(string Title)
            {
                itemList = new List<PadItem>();
                itemIndex = new Dictionary<string, PadItem>();

                maxItemHeight = 0;
                currentContentStart = 0;
                currentContentEnd = contentHeight;

                title = Title;
            }

            public void AddItem(string Tag, string Text)
            {
                PadItem padItem = new PadItem(Text, contentWidth);

                itemList.Add(padItem);
                itemIndex.Add(Tag, padItem);
                
                maxItemHeight += padItem.Height;
            }

            public void RemoveItem(string Tag)
            {
                maxItemHeight -= itemIndex[Tag].Height;

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

                float preHeight = itemIndex[Tag].Height;

                itemIndex[Tag].Text = Text;

                maxItemHeight = maxItemHeight - preHeight + itemIndex[Tag].Height;
            }

            public void OnMouseScroll(float offset)
            {
                offset *= LayerConfig.OffsetSpeed;

                currentContentStart += offset;
                currentContentEnd = currentContentStart + contentHeight;

                if (offset > 0) // Down
                {  
                    if (currentContentEnd > maxItemHeight)
                    {
                        currentContentEnd = maxItemHeight;
                        currentContentStart = currentContentEnd - contentHeight;
                    } 
                }else
                {
                    if (currentContentStart < 0)
                    {
                        currentContentStart = 0;
                        currentContentEnd = currentContentStart + contentHeight;
                    }
                }
            }

            public void OnRender()
            {
                //Trnasform Pad
                Matrix3x2 transform = Matrix3x2.CreateTranslation(new Vector2(realStartPosX,
                    realStartPosY));

                Canvas.Transform = transform;

                //Render Pad border
                Canvas.DrawRectangle(0, 0, realWidth, realHeight, 
                    LayerConfig.BorderBrush, LayerConfig.BorderSize);

                //Render Title BackGround
                Canvas.FillRectangle(0, 0, realWidth, realTitleHeight, LayerConfig.TitleBrush);

                //Render Title
                Canvas.DrawText(Title, LayerConfig.TitleOffsetX,
                    0, realWidth, realTitleHeight, LayerConfig.TitleFormat, LayerConfig.TextBrush);

                //Render Content
                transform *= Matrix3x2.CreateTranslation(new Vector2(0, realTitleHeight));

                if (itemList.Count != 0)
                {
                    //Get Content's area
                    Rect contentRect = new Rect(borderX, borderY + realTitleHeight, borderX + contentWidth,
                        borderY + contentHeight + realTitleHeight);

                    //Render content layer
                    Canvas.PushLayer(contentRect.Left, contentRect.Top,
                        contentRect.Right, contentRect.Bottom);

                    transform *= Matrix3x2.CreateTranslation(borderX, borderY);

                    float currentHeight = 0;

                    //Make sure the postion is right
                    if (maxItemHeight < contentHeight)
                    {
                        currentContentStart = 0;
                        currentContentEnd = contentHeight;
                    }

                    //Render items in pad
                    foreach (var item in itemList)
                    {
                        float height = item.Height;

                        if (currentHeight + height >= currentContentStart 
                            && currentHeight <= currentContentEnd)
                        {
                            Canvas.Transform = transform * Matrix3x2.CreateTranslation(0, currentHeight - currentContentStart);

                            item.OnRender();
                        }

                        currentHeight += height;
                    }

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

                realStartPosX = startPosX * VisualLayer.width;
                realStartPosY = startPosY * VisualLayer.height;

                realLayout = new Rect(realStartPosX, realStartPosY,
                    realStartPosX + realWidth, realStartPosY + realHeight);

                realTitleHeight = LayerConfig.TitleHeight;

                contentWidth = realWidth - borderX * 2;
                contentHeight = realHeight - borderY * 2 - realTitleHeight;

                //Update PadItem When Size Change
                maxItemHeight = 0;
                for (int i = 0; i < itemList.Count; i++)
                {
                    itemList[i].Reset(itemList[i].Text, contentWidth);
                    maxItemHeight += itemList[i].Height;
                }

                currentContentEnd = currentContentStart + contentHeight;
            }

            public void SetAreaKeeper(float baseValue, float targetAspectRatio, bool isWidth = true)
            {
                if (isWidth is true)
                {
                    SetArea(baseValue, baseValue / targetAspectRatio * aspectRatio);
                }
                else

                {
                    SetArea(baseValue * targetAspectRatio / aspectRatio, baseValue);
                }
            }

            public void SetPosition(float posX, float posY)
            {
                startPosX = posX;
                startPosY = posY;

                realStartPosX = startPosX * VisualLayer.width;
                realStartPosY = startPosY * VisualLayer.height;

                realLayout = new Rect(realStartPosX, realStartPosY,
                    realStartPosX + realWidth, realStartPosY + realHeight);
            }

            public bool Contains(float realPosX, float realPosY)
            {
                return realLayout.Contains(realPosX, realPosY);
            }

            public float Width => width;

            public float Height => height;

            public float StartPosX => startPosX;

            public float StartPosY => startPosY;

            public string Title
            {
                set => title = value;
                get => title;
            }
        }
    }
}
