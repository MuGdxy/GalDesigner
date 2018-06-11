using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Script : MemberValuable
    {
        protected string name;

        private string code = "";
        private string filePath = "";
        private int line = 0;

        public Script(string Code, int Line, string FilePath)
        {
            code = Code;
            filePath = FilePath;

            line = Line;
        }

        public string Name
        {
            internal set => name = value;
            get => name;
        }

        public string Code => code;
        public string FilePath => filePath;
        public int Line => line;

        internal override void SetPrivateMemberValue(string memberName, object value)
        {
            switch (memberName)
            {
                case SystemProperty.Name:

                    Name = Convert.ToString(value);

                    break;
                default:
                    break;
            }

            base.SetPrivateMemberValue(memberName, value);
        }

        public override object GetMemberValue(string memberName)
        {
            switch (memberName)
            {
                case SystemProperty.Name:

                    return AsObject(Name);
                case SystemProperty.Code:

                    return AsObject(Code);
                case SystemProperty.FilePath:

                    return AsObject(FilePath);
                default:
                    return memberValueList[memberName];
            }
        }

        public override void SetMemberValue(string memberName, object value)
        {
            memberValueList[memberName] = value;
        }
    }
}
