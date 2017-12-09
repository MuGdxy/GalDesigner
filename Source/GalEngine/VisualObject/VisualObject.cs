using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using Presenter;

namespace GalEngine
{
    public class VisualObject : IActivable, IMemberValuable
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
            null,
            Internal.ResourceList.BlackBrush,
            null,
            Internal.ResourceList.DefaultTextFormat
        };

        private CanvasResource[] memberResource = new CanvasResource[(int)MemberResource.Count];

        private string[] memberResourceTag = new string[(int)MemberResource.Count];

        private Dictionary<string, object> memberValueList = new Dictionary<string, object>();

        private bool isActive = false;
        private bool isPresented = false;

        private int width;
        private int height;

        private int positionX;
        private int positionY;

        private float angle;

        private string text;

        private VisualObject parent;
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

        internal void OnRender()
        {

        }

        public VisualObject(int Width, int Height)
        {
            width = Width;
            height = Height;
        }

        public void Active()
        { 
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

        public void Dispose()
        {
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
                    return (T)Convert.ChangeType(width, width.GetType());

                case "Height":
                    return (T)Convert.ChangeType(height, height.GetType());

                case "Text":
                    return (T)(text as object);

                case "PositionX":
                    return (T)Convert.ChangeType(positionX, positionX.GetType());

                case "PositionY":
                    return (T)Convert.ChangeType(positionY, positionY.GetType());

                case "Angle":
                    return (T)Convert.ChangeType(angle, angle.GetType());

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

                case "Angle":
                    angle = (float)value;
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

        public bool IsActive => isActive;

        public bool IsPresented => isPresented;

        public VisualObject Parent
        {
            set => parent = value;
            get => parent;
        }

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

        public float Angle
        {
            set => angle = value;
            get => angle;
        }

        public string Text
        {
            set => UpdateLayOut(value, width, height);
            get => text;
        }
    }
}
