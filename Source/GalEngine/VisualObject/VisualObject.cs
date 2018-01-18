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
    public class VisualObject : IMemberValuable
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
            Internal.ResourceList.BlackBrush,
        };

        private CanvasResource[] memberResource = new CanvasResource[(int)MemberResource.Count];

        private string[] memberResourceTag = new string[(int)MemberResource.Count];

        private Dictionary<string, object> memberValueList = new Dictionary<string, object>();

        private bool isActive = false;
        private bool isPresented = false;

        private int width;
        private int height;

        private int positionX = 0;
        private int positionY = 0;

        private float borderSize = 1;

        private float opacity = 1;

        private string text = "";
        
        private List<VisualObject> children = new List<VisualObject>();

        private CanvasText textInstance;

        private void UpdateLayOut(string newText, int newWidth, int newHeight)
        {
            text = newText;
            width = newWidth;
            height = newHeight;

            if (isActive is true)
            {
                textInstance.Reset(text, width, height);
            }
        }

        private object AsObject<T>(T value)
        {
            return Convert.ChangeType(value, value.GetType());
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
                if (memberResourceTag[i] is null)
                    memberResource[i] = defaultResource[i];
                else memberResource[i] = GlobalResource.GetValue<ResourceTag>(memberResourceTag[i]).Use() as CanvasResource;
            }

            Utilities.Dipose(ref textInstance);

            textInstance = new CanvasText(text, width, height, memberResource[(int)MemberResource.TextFormat] as CanvasTextFormat);
        }

        internal void OnRender()
        {
            Active();

            Matrix3x2 oldMatrix = Canvas.Transform;

            Canvas.Transform *= Matrix3x2.CreateTranslation(new Vector2(positionX, positionY));

            if (opacity != 1.0f)
                Canvas.PushLayer(0, 0, width, height, opacity);

            Canvas.DrawRectangle(0, 0, width, height, memberResource[(int)MemberResource.BorderBrush] as CanvasBrush, borderSize);

            if (memberResourceTag[(int)MemberResource.BackGroundImage] is null)
                Canvas.FillRectangle(0, 0, width, height, memberResource[(int)MemberResource.BackGroundBrush] as CanvasBrush);
            else
                Canvas.DrawImage(0, 0, width, height, memberResource[(int)MemberResource.BackGroundImage] as CanvasImage);

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
            OnKeyEvent(sender, e);

            KeyEvent?.Invoke(sender, e);
        }

        internal void PrivateOnMouseClick(object sender, MouseClickEventArgs e)
        {
            OnMouseClick(sender, e);

            MouseClick?.Invoke(sender, e);
        }

        internal void PrivateOnMouseMove(object sender, MouseMoveEventArgs e)
        {
            OnMouseMove(sender, e);

            MouseMove?.Invoke(sender, e);
        }

        internal void PrivateOnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            OnMouseWheel(sender, e);

            MouseWheel?.Invoke(sender, e);
        }

        internal void PrivateOnUpdate(object sender)
        {
            OnUpdate(sender);

            Update?.Invoke(sender);

            OnRender();
        }

        public VisualObject(int Width, int Height)
        {
            width = Width;
            height = Height;
        }

        public void Dispose()
        {
            if (isActive is false) return;

            isActive = false;

            for (int i = 0; i < (int)MemberResource.Count; i++)
            {
                if (memberResourceTag[i] != null)
                {
                    GlobalResource.GetValue<ResourceTag>(memberResourceTag[i]).UnUse();
                    memberResourceTag[i] = null;
                }
            }

            Utilities.Dipose(ref textInstance);

            foreach (var item in children)
            {
                Utilities.Dipose(ref item.children);
            }
        }

        public object GetMemberValue(string memberName)
        {
            return GetMemberValue<object>(memberName);
        }

        public virtual T GetMemberValue<T>(string memberName)
        {
            switch (memberName)
            {
                case "Width":
                    return (T)AsObject(width);

                case "Height":
                    return (T)AsObject(height);

                case "Text":
                    return (T)(text as object);

                case "PositionX":
                    return (T)AsObject(positionX);

                case "PositionY":
                    return (T)AsObject(positionY);

                case "BorderSize":
                    return (T)AsObject(borderSize);

                case "Opacity":
                    return (T)AsObject(opacity);

                case "TextBrush":
                case "TextFormat":
                case "BorderBrush":
                case "BackGroundImage":
                case "BackGroundBrush":
                    return (T)(memberResource[(int)Enum.Parse(typeof(MemberResource), memberName)] as object);
            }

            if (memberValueList.ContainsKey(memberName) is false)
                throw DebugLayer.GetErrorException(ErrorType.InvaildMemberValueName, memberName);
            else return (T)memberValueList[memberName];
        }

        public virtual void SetMemberValue(string memberName, object value)
        {
            switch (memberName)
            {
                case "Width":
                    UpdateLayOut(text, (int)value, height);
                    return;

                case "Height":
                    UpdateLayOut(text, width, (int)value);
                    return;

                case "Text":
                    UpdateLayOut((string)value, width, height);
                    return;

                case "PositionX":
                    positionX = (int)value;
                    return;

                case "PositionY":
                    positionY = (int)value;
                    return;

                case "BorderSize":
                    borderSize = (float)value;
                    return;

                case "Opacity":
                    opacity = (float)value;
                    return;

                case "TextBrush":
                case "TextFormat":
                case "BorderBrush":
                case "BackGroundImage":
                case "BackGroundBrush":
                    var which = Enum.Parse(typeof(MemberResource), memberName);

                    if (memberResourceTag[(int)which] != null)
                        GlobalResource.GetValue<ResourceTag>(memberResourceTag[(int)which]).UnUse();

                    memberResourceTag[(int)which] = value as string;

                    if (value is null)
                        memberResource[(int)which] = defaultResource[(int)which];
                    else
                        memberResource[(int)which] = GlobalResource.GetValue<ResourceTag>(value as string).Use() as CanvasResource;
                    
                    return;

            }
        }

        public virtual void OnKeyEvent(object sender, KeyEventArgs e) { }
        public virtual void OnMouseClick(object sender, MouseClickEventArgs e) { }
        public virtual void OnMouseMove(object sender, MouseMoveEventArgs e) { }
        public virtual void OnMouseWheel(object sender, MouseWheelEventArgs e) { }
        public virtual void OnUpdate(object sender) { }

        public bool Contains(int pointX,int pointY)
        {
            return pointX >= positionX && pointX < positionX + width &&
                pointY > positionY && pointY < positionY + height;
        }

        public event UpdateHandler Update;
        public event MouseMoveHandler MouseMove;
        public event MouseClickHandler MouseClick;
        public event MouseWheelHandler MouseWheel;
        public event KeyEventHandler KeyEvent;

        public bool IsActive => isActive;

        public bool IsPresented => isPresented;

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

        public string Text
        {
            set => UpdateLayOut(value, width, height);
            get => text;
        }
    }
}
