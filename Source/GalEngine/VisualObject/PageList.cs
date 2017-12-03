using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    internal static class PageList
    {
        private static Dictionary<string, GenericPage> pageList = new Dictionary<string, GenericPage>();

        public static void AddPage(string tag, GenericPage page)
        {
            pageList[tag] = page;
        }

        public static Dictionary<string, GenericPage> Element => pageList;
    }
}
