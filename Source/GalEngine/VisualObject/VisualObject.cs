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
    public class VisualObject : MemberValuable
    {
        private enum MemberResource : int
        {
            TextBrush,
            TextFormat,
            BorderBrush,
            BackGroundImage,
            BackGroundBrush,
            Count
        }

        static CanvasResource[] defaultResource = new CanvasResource[]
        {
            Internal.ResourceList.BlackBrush,
            Internal.ResourceList.DefaultTextFormat,
            Internal.ResourceList.BlackBrush,
            null,
            Internal.ResourceList.WhiteBrush,
        };

        private CanvasResource[] memberResource = new CanvasResource[(int)MemberResource.Count];
        private ResourceView[] memberResourceView = new ResourceView[(int)MemberResource.Count];

        private bool isActive = false;
        private bool isPresented = true;
        private bool isFocus = false;
        private bool isMouseHover = false;

        private int width = 0;
        private int height = 0;

        private int positionX = 0;
        private int positionY = 0;
        private int positionZ = 1;

        private float borderSize = 0;

        private float opacity = 1;

        private float angle = 0;

        private float scaleX = 1;

        private float scaleY = 1;

        private string text = "";

        private string name = "";
        
        private List<VisualObject> children = new List<VisualObject>();

        private CanvasText textInstance;

        public bool IsFocus => isFocus;

        public bool IsMouseHover => isMouseHover;

        public string Name => name;

        public List<VisualObject> Children
        {
            get => children;
        }

        public int Width
        {
            set => UpdateLayOut(text, value, height);
            get => width;
        }

        public int Height
        {
            set => UpdateLayOut(text, width, value);
            get => height;
        }

        public int PositionX
        {
            set => positionX = value;
            get => positionX;
        }

        public int PositionY
        {
            set => positionY = value;
            get => positionY;
        }

        public int PositionZ
        {
            get => positionZ;
            set => positionZ = value;
        }

        public float BorderSize
        {
            set => borderSize = value;
            get => borderSize;
        }

        public float Opacity
        {
            set => opacity = value;
            get => opacity;
        }

        public bool IsPresented
        {
            set
            {
                isPresented = value;
                if (isPresented is false)
                {
                    isFocus = false;
                    isMouseHover = false;
                }
            }
            get => isPresented;
        }

        public string Text
        {
            set => UpdateLayOut(value, width, height);
            get => text;
        }

        public float Angle
        {
            set => angle = value;
            get => angle;
        }

        public float ScaleX
        {
            set => scaleX = value;
            get => scaleX;
        }

        public float ScaleY
        {
            set => scaleY = value;
            get => scaleY;
        }

        private void UpdateLayOut(string newText, int newWidth, int newHeight)
        {
            text = newText;
            width = newWidth;
            height = newHeight;

            if (isActive is true)
            {
                textInstance.Reset(text, width, height, memberResource[(int)MemberResource.TextFormat] as CanvasTextFormat);
            }
        }

        /// <summary>
        /// Active resource. We will active them on render.
        /// </summary>
        private void Active()
        {
            if (isActive is true) return;

            isActive = true;

            for (int i = 0; i < (int)MemberResource.Count; i++)
            {
                if (memberResourceView[i] is null)
                    memberResource[i] = defaultResource[i];
                else memberResource[i] = memberResourceView[i].Resource as CanvasResource;
            }

            Utilities.Dipose(ref textInstance);

            textInstance = new CanvasText(text, width, height, memberResource[(int)MemberResource.TextFormat] as CanvasTextFormat);
        }

        internal void OnRender()
        {
            if (isPresented is false) return;

            if (width == 0 || height == 0) return;

            Active();

            Matrix3x2 oldMatrix = Canvas.Transform;

            var center = new Vector2(width / 2, height / 2);

            Canvas.Transform = Matrix3x2.CreateScale(scaleX, scaleY, center) * Matrix3x2.CreateRotation(angle, center)
                * Matrix3x2.CreateTranslation(new Vector2(positionX, positionY)) * oldMatrix;

            if (opacity != 1.0f)
                Canvas.PushLayer(0, 0, width, height, opacity);

            if (borderSize != 0f)
                Canvas.DrawRectangle(0, 0, width, height, memberResource[(int)MemberResource.BorderBrush] as CanvasBrush, borderSize);

            if (memberResourceView[(int)MemberResource.BackGroundImage] is null)
                Canvas.FillRectangle(0, 0, width, height, memberResource[(int)MemberResource.BackGroundBrush] as CanvasBrush);
            else
            {
                if ((memberResourceView[(int)MemberResource.BackGroundImage] as ImageView).Size is 0)
                    Canvas.DrawImage(0, 0, width, height, memberResource[(int)MemberResource.BackGroundImage] as CanvasImage);
                else
                {
                    var resourceView = memberResourceView[(int)MemberResource.BackGroundImage] as ImageView;

                    Canvas.DrawImage(0, 0, width, height, resourceView.Source, resourceView.Left,
                        resourceView.Top, resourceView.Right, resourceView.Bottom);
                }
            }

            if (text != "")
                Canvas.DrawText(0, 0, textInstance, memberResource[(int)MemberResource.TextBrush] as CanvasBrush);

            foreach (var item in children)
            {
                item.OnRender();
            }

            if (opacity != 1.0f)
                Canvas.PopLayer();

            Canvas.Transform = oldMatrix;
        }

        internal void PrivateOnKeyEvent(object sender, KeyEventArgs e)
        {
            if (isFocus is false) return;

            OnKeyEvent(sender, e);

            KeyEvent?.Invoke(sender, e);

            foreach (var item in children)
            {
                item.PrivateOnKeyEvent(item, e);
            }
        }

        internal void PrivateOnMouseClick(object sender, MouseClickEventArgs e)
        {
            if (isMouseHover is false)
            {
                if (e.Which is MouseButton.LeftButton)
                    isFocus = false;

                return;
            }

            if (e.Which is MouseButton.LeftButton)
                isFocus = true;

            OnMouseClick(sender, e);

            MouseClick?.Invoke(sender, e);

            foreach (var item in children)
            {
                item.PrivateOnMouseClick(item, e);
            }
        }

        internal void PrivateOnMouseMove(object sender, MouseMoveEventArgs e)
        {
            if (isPresented is false) return;

            if (Contains(e.X, e.Y) is true)
                isMouseHover = true;
            else
            {
                isMouseHover = false;

                return;
            }

            OnMouseMove(sender, e);

            MouseMove?.Invoke(sender, e);

            foreach (var item in children)
            {
                item.PrivateOnMouseMove(item, e);
            }
        }

        internal void PrivateOnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (isMouseHover is false) return;

            OnMouseWheel(sender, e);

            MouseWheel?.Invoke(sender, e);

            foreach (var item in children)
            {
                item.PrivateOnMouseWheel(item, e);
            }
        }

        internal void PrivateOnUpdate(object sender)
        {
            if (isPresented is false) return;

            OnUpdate(sender);

            Update?.Invoke(sender);

            foreach (var item in children)
            {
                item.OnUpdate(item);
            }
        }

        public VisualObject(string Name, int Width, int Height)
        {
            name = Name;

            width = Width;
            height = Height;

            VisualObjectList.Add(this);
        }

        public void Dispose()
        {
            if (isActive is false) return;

            isActive = false;

            for (int i = 0; i < (int)MemberResource.Count; i++)
            {
                if (memberResourceView[i] != null)
                {
                    memberResourceView[i] = null;
                }
            }

            Utilities.Dipose(ref textInstance);

            VisualObjectList.Remove(this);

            foreach (var item in children)
            {
                Utilities.Dipose(ref item.children);
            }
        }

        public override object GetMemberValue(string memberName)
        {
            return GetMemberValue<object>(memberName);
        }

        public override void SetMemberValue(string memberName, object value)
        {
            switch (memberName)
            {
                case "Width":
                    Width = Convert.ToInt32(value);
                    return;

                case "Height":
                    Height = Convert.ToInt32(value);
                    return;

                case "Text":
                    Text = Convert.ToString(value);
                    return;

                case "PositionX":
                    PositionX = Convert.ToInt32(value);
                    return;

                case "PositionY":
                    PositionY = Convert.ToInt32(value);
                    return;

                case "PositionZ":
                    positionZ = Convert.ToInt32(value);
                    return;

                case "BorderSize":
                    BorderSize = Convert.ToSingle(value);
                    return;

                case "Opacity":
                    Opacity = Convert.ToSingle(value);
                    return;

                case "IsPresented":
                    IsPresented = Convert.ToBoolean(value);
                    return;

                case "Angle":
                    angle = Convert.ToSingle(value);
                    return;

                case "ScaleX":
                    scaleX = Convert.ToSingle(value);
                    return;

                case "ScaleY":
                    scaleY = Convert.ToSingle(value);
                    return;

                case "TextBrush":
                case "TextFormat":
                case "BorderBrush":
                case "BackGroundImage":
                case "BackGroundBrush":
                    var which = Enum.Parse(typeof(MemberResource), memberName);

                    memberResourceView[(int)which] = GlobalResource.GetValue(value as string);

                    if (value is null)
                        memberResource[(int)which] = defaultResource[(int)which];
                    else
                        memberResource[(int)which] = memberResourceView[(int)which].Resource as CanvasResource; ;

                    if ((MemberResource)which== MemberResource.TextFormat)
                        UpdateLayOut(text, width, height);

                    return;
            }

            memberValueList[memberName] = value;
        }

        internal virtual void OnKeyEvent(object sender, KeyEventArgs e) { }
        internal virtual void OnMouseClick(object sender, MouseClickEventArgs e) { }
        internal virtual void OnMouseMove(object sender, MouseMoveEventArgs e) { }
        internal virtual void OnMouseWheel(object sender, MouseWheelEventArgs e) { }
        internal virtual void OnUpdate(object sender) { }

        public bool Contains(int pointX,int pointY)
        {
            return pointX >= positionX && pointX < positionX + width &&
                pointY > positionY && pointY < positionY + height;
        }

        public void AddChildren(string name)
        {
            children.Add(VisualObjectList.GetVisualObject(name));
        }

        public void RemoveChildren(string name)
        {
            children.Remove(VisualObjectList.GetVisualObject(name));
        }

        public event UpdateHandler Update;
        public event MouseMoveHandler MouseMove;
        public event MouseClickHandler MouseClick;
        public event MouseWheelHandler MouseWheel;
        public event KeyEventHandler KeyEvent;
    }
}
