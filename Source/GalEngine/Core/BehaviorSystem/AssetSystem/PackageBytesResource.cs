using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    /// <summary>
    /// package's resource(bytes)
    /// </summary>
    public class PackageBytesResource
    {
        public byte[] Bytes { get; internal set; }
        public int Reference { get; private set; }
        public string Name { get; private set; }

        public int Size => Bytes.Length;

        private PackageBytesResource(string name)
        {
            Name = name;
            Reference = 0;
        }

        internal PackageBytesResource IncreaseReference()
        {
            Reference++; return this;
        }

        internal PackageBytesResource DecreaseReference()
        {
            Reference--; return this;
        }

        public PackageBytesResource(string name, byte[] bytes) : this(name)
        {
            Bytes = bytes;
        }

        public PackageBytesResource(string name, int size) : this(name)
        {
            Bytes = new byte[size];
        }
    }
}
