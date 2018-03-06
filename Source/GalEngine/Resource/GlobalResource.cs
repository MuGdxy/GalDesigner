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

        public static void SetValue(ResourceView resourceTag)
        {
            resourceViewList[resourceTag.Name] = resourceTag;
        }

        public static ResourceView GetValue(string Tag)
        {
            return GetValue<ResourceView>(Tag);
        }

        public static T GetValue<T>(string Tag) where T : ResourceView
        {
            DebugLayer.Assert(resourceViewList.ContainsKey(Tag) is false, ErrorType.InvaildName, "GlobalResource");

            return resourceViewList[Tag] as T;
        }
    }
}
