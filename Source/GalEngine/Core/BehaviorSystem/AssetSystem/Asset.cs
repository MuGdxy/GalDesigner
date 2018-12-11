using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    using Debug = System.Diagnostics.Debug;

    public class AssetReference
    {
        internal object instance;

        public Asset Source { get; }
        public int Size { get; }
        public bool IsReference { get; }

        public object Instance { get => instance; private set => instance = value; }

        public AssetReference(Asset asset, object instance, int size, bool isReference = true)
        {
            Source = asset;
            Instance = instance;
            Size = size;
            IsReference = isReference;
        }
    }

    public class Asset
    {
        private object instance;

        public string Name { get; }
        public int Reference { get; private set; }
        public int Size { get; private set; }

        public PackageProvider Package { get; internal set; }

        protected virtual object ConvertBytesToInstance(byte[] bytes, List<AssetReference> dependentAssets)
        {
            throw new NotImplementedException("Convert bytes to instance failed.");
        }

        protected virtual void DisposeInstance(ref object instance)
        {
            throw new NotImplementedException("Dispose instance failed.");
        }

        internal AssetReference IncreaseReference()
        {
            Reference++; return new AssetReference(this, instance, Size, true);
        }

        internal void DecreaseReference()
        {
            Debug.Assert(Reference > 0); Reference--;
        }

        internal void Load(byte[] bytes, List<AssetReference> dependentAssets)
        {
            Debug.Assert(instance == null && Reference == 0);
            Debug.Assert(bytes.Length == Size || Size == -1);

            if (Size == -1) Size = bytes.Length;

            instance = ConvertBytesToInstance(bytes, dependentAssets);
        }

        internal AssetReference LoadIndependentReference(byte[] bytes, List<AssetReference> dependentAssets)
        {
            return new AssetReference(this, ConvertBytesToInstance(bytes, dependentAssets), bytes.Length, false);
        }

        internal void UnLoad()
        {
            Debug.Assert(instance != null && Reference == 0);

            DisposeInstance(ref instance);
        }

        internal void UnLoadIndependentReference(ref AssetReference independentReference)
        {
            Debug.Assert(independentReference.IsReference == false && independentReference.Source == this);
            Debug.Assert(independentReference.Instance != null);

            DisposeInstance(ref independentReference.instance);

            independentReference = null;
        }

        public Asset(string name, int size = -1)
        {
            Name = name; Size = size; Reference = 0;
            instance = null;
        }
    }
}
