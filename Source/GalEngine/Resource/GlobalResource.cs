using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    static class GlobalResource
    {
        private static Dictionary<string, ResourceView> resourceTagList = new Dictionary<string, ResourceView>();

        public static void SetValue(ResourceView resourceTag)
        {
            resourceTagList[resourceTag.Name] = resourceTag;
        }

        public static ResourceView GetValue(string Tag)
        {
            return GetValue<ResourceView>(Tag);
        }

        public static T GetValue<T>(string Tag) where T : ResourceView
        {
            DebugLayer.Assert(resourceTagList.ContainsKey(Tag) is false, ErrorType.InvaildName, "GlobalResource");

            return resourceTagList[Tag] as T;
        }
    }
}
