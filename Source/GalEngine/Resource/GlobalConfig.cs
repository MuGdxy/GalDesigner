using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GlobalConfig
    {
        private static Dictionary<string, object> valueList;
        
        static GlobalConfig()
        {
            valueList = new Dictionary<string, object>();
        }

        public static void SetValue(string Tag, object value)
        {

#if DEBUG
            DebugLayer.Assert(Utilities.IsBaseType(value) is false, ErrorType.InvalidValueType);
#endif
            valueList[Tag] = value;
        }

        public static object GetValue(string Tag) => valueList[Tag];

        public static T GetValue<T>(string Tag)
        {
            return (T)valueList[Tag];
        }
    }
}
