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
        public string Name { get; private set; }
        public byte[] Bytes { get; private set; }
        public int Reference { get; private set; }

        public int Size => Bytes.Length;

        private PackageBytesResource()
        {
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

        public PackageBytesResource(string name, byte[] bytes) : this()
        {
            Name = name;
            Bytes = bytes;
        }

        public PackageBytesResource(string name, int size) : this()
        {
            Name = name;
            Bytes = new byte[size];
        }
    }
}
