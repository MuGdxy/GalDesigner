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
            resourceTagList.Add(resourceTag.Tag, resourceTag);
        }

        public static ResourceTag GetValue(string valueName) => resourceTagList[valueName]; 
    }
}
