using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using Presenter;

namespace GalEngine
{
    ///Visual Layer Config.
    using LayerConfig = VisualLayerConfig;

    static partial class VisualLayer
    {
        /// <summary>
        /// Pad Type.
        /// </summary>
        internal enum PadType
        {
            InformationPad,
            WarningPad,
            WatchPad
        }
        
        /// <summary>
        /// Visual Pad.
        /// </summary>
        private class VisualPad
        {
            /// <summary>
            /// The distance from Pad content's border-X to Pad's border-X (pixel).
            /// </summary>
            private const float borderX = 5;
            /// <summary>
            /// The distance from Pad content's border-Y to Pad's border-Y (pixel).
            /// </summary>
            private const float borderY = 3;

            /// <summary>
            /// Item in PadList.
            /// </summary>
            private class PadItem
            {
                /// <summary>
                /// The distance from text border-Y to PadItem's border-Y (relative text's size).
                /// </summary>
                private const float borderY = 0.1f;

                /// <summary>
                /// The PadItem's background brush.
                /// </summary>
                private static CanvasBrush padItemBrush = new CanvasBrush(192 / 255f, 192 / 255f, 192 / 255f, 1f);
                
                /// <summary>
                /// The PadItem's text.
                /// </summary>
                private string text;

                /// <summary>
                /// The PadItem's width (pixel).
                /// </summary>
                private float width;

                /// <summary>
                /// The PadItem's text instance for render text.
                /// </summary>
                private CanvasText canvasText;
                
                /// <summary>
                /// The information about our text.
                /// </summary>
                private TextMetrics textMetrics;

                /// <summary>
                /// Create a PadItem.
                /// </summary>
                /// <param name="Text">The PadItem's text.</param>
                /// <param name="Width">The PadItem's width (pixel).</param>
                public PadItem(string Text, float Width = 0)
                {
                    //create text instance
                    canvasText = new CanvasText(text = Text, width = Width, 0, LayerConfig.TextFormat);

                    //get text metrics
                    textMetrics = canvasText.Metrics;
                }

                /// <summary>
                /// Reset the PadItem.
                /// </summary>
                /// <param name="Text">The PadItem's text.</param>
                /// <param name="Width">The PadItem's width (pixel).</param>
                public void Reset(string Text, float Width)
                {
                    //if no change, we do not need to reset it.
                    if (text == Text && width == Width) return;

                    //reset
                    canvasText.Reset(text = Text, width = Width, 0);
                    
                    //get text metrics
                    textMetrics = canvasText.Metrics;
                }

                public void OnRender()
                {
                    //render background
                    Canvas.FillRectangle(0, 0, Width, Height, padItemBrush);

                    //render border
                    Canvas.DrawRectangle(0, 0, Width, Height, LayerConfig.BorderBrush, LayerConfig.BorderSize / 2f);

                    //render text
                    Canvas.DrawText(text, LayerConfig.TitleOffsetX, 0, width, Height, LayerConfig.TextFormat, LayerConfig.TextBrush);
                }

                /// <summary>
                /// The PadItem's text.
                /// </summary>
                public string Text
                {
                    set => Reset(value, width);
                    get => text;
                }

                /// <summary>
                /// The PadItem's width (pixel).
                /// </summary>
                public float Width
                {
                    set => Reset(text, value);
                    get => width;
                }

                /// <summary>
                /// The PadItem's height (pixel). It is decided to width and text.
                /// </summary>
                public float Height => textMetrics.Height + borderY * LayerConfig.TextFormat.Size * 2;
            }

            /// <summary>
            /// The Pad's width (relative VisualLayer).
            /// </summary>
            private float width;

            /// <summary>
            /// The Pad's height (relative VisualLayer).
            /// </summary>
            private float height;

            /// <summary>
            /// The Pad's scope (pixel).
            /// </summary>
            private Rect realLayout;
            
            /// <summary>
            /// The Pad's title height (pixel).
            /// </summary>
            private float realTitleHeight;

            /// <summary>
            /// The Pad's width (pixel).
            /// </summary>
            private float realWidth;

            /// <summary>
            /// The Pad's height (pixel).
            /// </summary>
            private float realHeight;

            /// <summary>
            /// The Pad content's width (pixel).
            /// </summary>
            private float contentWidth;

            /// <summary>
            /// The Pad content's height (pixel). It only defines the scope that we will render.
            /// </summary>
            private float contentHeight;

            /// <summary>
            /// The Pad's position-X (relative VisualLayer).
            /// </summary>
            private float startPosX;
            
            /// <summary>
            /// The Pad's position-Y (relative VisualLayer).
            /// </summary>
            private float startPosY;

            /// <summary>
            /// The Pad's position-X (pixel).
            /// </summary>
            private float realStartPosX;

            /// <summary>
            /// The Pad's position-Y (pixel).
            /// </summary>
            private float realStartPosY;

            /// <summary>
            /// The Pad's title.
            /// </summary>
            private string title;

            /// <summary>
            /// List to save the PadItem.
            /// </summary>
            private List<PadItem> itemList;

            /// <summary>
            /// Dictionary to get the PadItem in itemList by string.
            /// </summary>
            private Dictionary<string, PadItem> itemIndex;

            /// <summary>
            /// The Pad content's start pos (relative content's real height). It tells us the start scope in content we will render.
            /// </summary>
            private float currentContentStart;

            /// <summary>
            /// The Pad content's end pos (relative content's real height). It tells us the end scope in content we will render.
            /// </summary>
            private float currentContentEnd;

            /// <summary>
            /// The Pad content's real height.
            /// </summary>
            private float maxItemHeight;

            /// <summary>
            /// Create a VisualPad.
            /// </summary>
            /// <param name="Title">VisualPad's title.</param>
            public VisualPad(string Title)
            {
                itemList = new List<PadItem>();
                itemIndex = new Dictionary<string, PadItem>();

                maxItemHeight = 0;
                currentContentStart = 0;
                currentContentEnd = contentHeight;

                title = Title;
            }

            /// <summary>
            /// Add a PadItem to VisualPad.
            /// </summary>
            /// <param name="Tag">PadItem's tag.</param>
            /// <param name="Text">PadItem's text.</param>
            public void AddItem(string Tag, string Text)
            {
                PadItem padItem = new PadItem(Text, contentWidth);

                itemList.Add(padItem);
                itemIndex.Add(Tag, padItem);
                
                //we add a item, so we need add the content real height.
                maxItemHeight += padItem.Height;
            }

            /// <summary>
            /// Remove a PadItem from VisualPad.
            /// </summary>
            /// <param name="Tag">PadItem's tag.</param>
            public void RemoveItem(string Tag)
            {
                maxItemHeight -= itemIndex[Tag].Height;

                itemList.Remove(itemIndex[Tag]);
                itemIndex.Remove(Tag);
            }

            /// <summary>
            /// Set a PadItem. We can use this function to update PadItem's text.
            /// </summary>
            /// <param name="Tag">PadItem's tag.</param>
            /// <param name="Text">PadItem's text.</param>
            public void SetItem(string Tag, string Text)
            {
                //if the PadItem is not in itemList, add it.
                if (itemIndex.ContainsKey(Tag) is false)
                {
                    AddItem(Tag, Text);
                    return;
                }

                float preHeight = itemIndex[Tag].Height;

                //update text.
                itemIndex[Tag].Text = Text;

                //update content's real height.
                maxItemHeight = maxItemHeight - preHeight + itemIndex[Tag].Height;
            }

            /// <summary>
            /// On Mouse Scroll.
            /// </summary>
            /// <param name="offset">The offset.</param>
            public void OnMouseScroll(float offset)
            {
                //real offset.
                offset *= LayerConfig.OffsetSpeed;

                //change the scope that we will render.
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

            /// <summary>
            /// Render.
            /// </summary>
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

            /// <summary>
            /// Set VisualPad's size.
            /// </summary>
            /// <param name="Width">width (relative VisualLayer).</param>
            /// <param name="Height">height (relative VisualLayer).</param>
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

                //get the content scope that we will render.
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

            /// <summary>
            /// Set VisualPad's size and keep the AspectRatio.
            /// </summary>
            /// <param name="baseValue">width or height (relative VisualLayer).</param>
            /// <param name="targetAspectRatio">The AspectRatio we want to keep.</param>
            /// <param name="isWidth">True is width. Flase is height.</param>
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

            /// <summary>
            /// Set VisualPad's position (relative VisualLayer).
            /// </summary>
            /// <param name="posX">Position-X.</param>
            /// <param name="posY">Position-Y.</param>
            public void SetPosition(float posX, float posY)
            {
                startPosX = posX;
                startPosY = posY;

                realStartPosX = startPosX * VisualLayer.width;
                realStartPosY = startPosY * VisualLayer.height;

                realLayout = new Rect(realStartPosX, realStartPosY,
                    realStartPosX + realWidth, realStartPosY + realHeight);
            }

            /// <summary>
            /// If Contains a point.
            /// </summary>
            /// <param name="realPosX">Point's position-X (pixel).</param>
            /// <param name="realPosY">Point's position-Y (pixel).</param>
            /// <returns>True means the point is contained. Fales means not.</returns>
            public bool Contains(float realPosX, float realPosY)
            {
                return realLayout.Contains(realPosX, realPosY);
            }

            /// <summary>
            /// VisualPad's width (relative VisualLayer).
            /// </summary>
            public float Width => width;

            /// <summary>
            /// VisualPad's height (relative VisualLayer).
            /// </summary>
            public float Height => height;

            /// <summary>
            /// VisualPad's position-X (relative VisualLayer).
            /// </summary>
            public float StartPosX => startPosX;

            /// <summary>
            /// VisualPad's position-Y (relative VisualLayer).
            /// </summary>
            public float StartPosY => startPosY;

            /// <summary>
            /// VisualPad's title.
            /// </summary>
            public string Title
            {
                set => title = value;
                get => title;
            }
        }
    }
}
