using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class BrushScript : Script
    {
        private float red;
        private float green;
        private float blue;

        private float alpha;

        private BrushView brushView;

        public float Red => red;
        public float Green => green;
        public float Blue => blue;
        public float Alpha => alpha;

        private void ProcessSentence(string left, string right)
        {
            switch (left)
            {
                case SystemProperty.Name:
                    name = right;

                    break;
                default:
                    break;
            }
        }

        public BrushScript(string Content, int Line, string FilePath) : base(Content, Line, FilePath)
        {
            var script = this;

            ScriptProcessUnit.Default.ProcessScript(this);
        }

        public override object GetMemberValue(string memberName)
        {
            switch (memberName)
            {
                case SystemProperty.Red:
                    return AsObject(Red);
                case SystemProperty.Green:
                    return AsObject(Green);
                case SystemProperty.Blue:
                    return AsObject(Blue);
                case SystemProperty.Alpha:
                    return AsObject(Alpha);
                default:
                    break;
            }

            return base.GetMemberValue(memberName);
        }

        internal override void SetPrivateMemberValue(string memberName, object value)
        {
            switch (memberName)
            {
                case SystemProperty.Red:
                    red = Convert.ToSingle(value);
                    break;
                case SystemProperty.Green:
                    green = Convert.ToSingle(value);
                    break;
                case SystemProperty.Blue:
                    blue = Convert.ToSingle(value);
                    break;
                case SystemProperty.Alpha:
                    alpha = Convert.ToSingle(value);
                    break;
                default:
                    break;
            }

            base.SetPrivateMemberValue(memberName, value);
        }
    }
}
