using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ResourceHeap
    {
        private List<Resource> resouceList = new List<Resource>();

        private int maxCount;
        
        public ResourceHeap(int count)
        {
            maxCount = count;
        }

        public void AddResource<T>(ConstantBuffer<T> resource) where T : struct
        {
            resouceList.Add(resource);
        }

        public void AddResource(ShaderResource resource)
        {
            resouceList.Add(resource);
        }

        public int Count => resouceList.Count;

        public int MaxCount => maxCount;

        internal List<Resource> Elements => resouceList;
    }
}
