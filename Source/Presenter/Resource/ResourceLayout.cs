using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class ResourceLayout
    { 
        private Element[] layoutElements;

        private StaticSampler[] layoutStaticSamplers;

        private int staticSamplerCount = 0;

        public ResourceLayout(Element[] elements = null, StaticSampler[] staticSamplers = null)
        {
            layoutElements = elements;

            layoutStaticSamplers = staticSamplers;
        }

        public Element[] Elements => layoutElements;

        public StaticSampler[] StaticSamplers => layoutStaticSamplers;

        public int SlotCount => layoutElements.Length;

        public int StaticSamplerCount => staticSamplerCount;
    }
}
