using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    static class GlobalResource
    {
        private static Dictionary<string, ResourceView> resourceViewList = new Dictionary<string, ResourceView>();

        public static void SetValue(ResourceView resourceView)
        {
            resourceViewList[resourceView.Name] = resourceView;
        }

        public static ResourceView GetValue(string name)
        {
            return GetValue<ResourceView>(name);
        }

        public static T GetValue<T>(string name) where T : ResourceView
        {
            DebugLayer.Assert(resourceViewList.ContainsKey(name) is false, ErrorType.InvaildName, "GlobalResource");

            return resourceViewList[name] as T;
        }
    }
}
