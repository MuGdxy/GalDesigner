using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public abstract class MemberValuable
    {
        protected Dictionary<string, object> memberValueList = new Dictionary<string, object>();

        protected static object AsObject<T>(T value)
        {
            return Convert.ChangeType(value, value.GetType());
        }

        internal virtual void SetPrivateMemberValue(string memberName, object value)
        {
            SetMemberValue(memberName, value);
        }

        internal virtual object GetPrivateMemberValue(string memberName)
        {
            return null;
        }

        public virtual void SetMemberValue(string memberName, object value)
        {

        }

        public virtual object GetMemberValue(string memberName)
        {
            return null;
        }

        public T GetMemberValue<T>(string memberName)
        {
            return (T)GetMemberValue(memberName);
        }
    }
}
