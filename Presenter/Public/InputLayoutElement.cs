using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class InputLayout
    {
        public class Element
        {
            public ElementSize Size;
            public string Tag;

            public Element(string tag, ElementSize size)
            {
                Tag = tag;
                Size = size;
            }
        }
    }
}
