using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    using Debug = System.Diagnostics.Debug;

    public class Asset
    {
        private object instance = null;

        public string Name { get; }
        public object Instance { get => instance; private set => instance = value; }
        public int Reference { get; private set; }
        public int Size { get; private set; }

        protected virtual object ConvertBytesToInstance(byte[] bytes)
        {
            throw new NotImplementedException("Convert bytes to instance failed.");
        }

        protected virtual void DisposeInstance(ref object instance)
        {
            throw new NotImplementedException("Dispose instance failed.");
        }

        internal Asset IncreaseReference()
        {
            Reference++; return this;
        }

        internal Asset DecreaseReference()
        {
            Debug.Assert(Reference > 0);

            Reference--; return this;
        }

        internal void Load(byte[] bytes)
        {
            Debug.Assert(bytes != null);
            Debug.Assert(bytes.Length == Size);

            if (Instance != null) DisposeInstance(ref instance); Instance= null;

            Size = bytes.Length;
            Instance = ConvertBytesToInstance(bytes);
        }

        internal void UnLoad()
        {
            if (Instance == null) return;

            DisposeInstance(ref instance); Instance = null;
        }

        public Asset(string name, int size)
        {
            Name = name; Size = size; Reference = 0;
            Instance = null;
        }
    }
}
