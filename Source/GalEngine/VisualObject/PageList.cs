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

        internal static void AddPage(GenericPage page)
        {
            pageList[page.Tag] = page;
        }

        public static Dictionary<string, GenericPage> Element => pageList;
    }
}
