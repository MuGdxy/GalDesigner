using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    interface IValuable
    {
        void SetValue(string valueName, object value);

        object GetValue(string valueName);

        T GetValue<T>(string valueName) where T : class;
    }
}
