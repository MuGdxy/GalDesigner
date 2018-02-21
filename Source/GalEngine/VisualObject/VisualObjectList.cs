using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class VisualObjectList
    {
        private static Dictionary<string, VisualObject> visualObjectList = new Dictionary<string, VisualObject>();

        internal static void Add(VisualObject visualObject)
        {
            visualObjectList[visualObject.Tag] = visualObject;
        }

        internal static void Remove(VisualObject visualObject)
        {
            visualObjectList.Remove(visualObject.Tag);
        }

        public static Dictionary<string, VisualObject> Element => visualObjectList;
    }
}
