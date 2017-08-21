using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class ResourceLayout
    {
        public class Element
        {
            public ResourceType Type;
            public int Register;
            public int Count;

            public Element(ResourceType type, int register,
                int count = 1)
            {
                Type = type;
                Register = register;
                Count = count;
            }

        }
    }
}
