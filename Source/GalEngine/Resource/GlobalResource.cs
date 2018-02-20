using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    static class GlobalResource
    {
        private static Dictionary<string, ResourceTag> resourceTagList = new Dictionary<string, ResourceTag>();

        public static void SetValue(ResourceTag resourceTag)
        {
            resourceTagList[resourceTag.Tag] = resourceTag;
        }

        public static ResourceTag GetValue(string Tag)
        {
            return GetValue<ResourceTag>(Tag);
        }

        public static T GetValue<T>(string Tag) where T : ResourceTag
        {
            DebugLayer.Assert(resourceTagList.ContainsKey(Tag) is false, ErrorType.InvaildTag, "GlobalResource");

            return resourceTagList[Tag] as T;
        }
    }
}
