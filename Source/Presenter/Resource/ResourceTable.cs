using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ResourceTable
    {
        private ResourceHeap resourceHeap;
        private int startPos;

        public ResourceTable(ResourceHeap heap, int start)
        {
            resourceHeap = heap;
            startPos = start;
        }

        public int Start => startPos;

        internal ResourceHeap WhcihHeap => resourceHeap;
    }
}
