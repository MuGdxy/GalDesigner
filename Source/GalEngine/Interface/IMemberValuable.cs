using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    interface IMemberValuable
    {
        void SetMemberValue(string memberName, object value);

        object GetMemberValue(string memberName);

        T GetMemberValue<T>(string memberName);
    }
}
