using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GlobalValue
    {
        private static Dictionary<string, object> valueList = new Dictionary<string, object>();

        public static void SetValue(string valueName, object value)
        {
            valueList[valueName] = value;
        }

        public static object GetValue(string valueName) => valueList[valueName];

        public static T GetValue<T>(string valueName) where T : class
            => (valueList[valueName] as T);
        

    }
}
