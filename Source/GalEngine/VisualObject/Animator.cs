using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
   
    public class Animator
    {
        private object parent = null;

        private string name;

        public object Parent
        {
            internal set => parent = value;
            get => parent;
        }

        public Animator(string Name)
        {
            name = Name;
        }
    }
}
