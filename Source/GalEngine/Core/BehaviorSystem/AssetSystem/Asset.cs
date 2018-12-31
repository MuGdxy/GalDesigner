using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Asset
    {
        public int Size { get; }

        public object Instance { get; internal set; }

        public bool IsExisted { get; internal set; }
        public bool IsReference { get; }
        public AssetDescription AssetDescription { get; }

        internal Asset(object instance, int size, bool isReference, 
            AssetDescription assetDescription)
        {
            Size = size; Instance = instance; IsReference = isReference;
            AssetDescription = assetDescription; IsExisted = true;
        }
    }

    public delegate void AssetCreateFunction(out object instance, byte[] bytes, List<Asset> dependentAssets);
    public delegate void AssetDestoryFunction(ref object instance);

    public class AssetDescription
    {
        private object mInstance;

        private readonly AssetCreateFunction mCreateFunction;
        private readonly AssetDestoryFunction mDestoryFunction;

        internal object Instance { get => mInstance; }
        internal AssetCreateFunction CreateFunction { get => mCreateFunction; }
        internal AssetDestoryFunction DestoryFunction { get => mDestoryFunction; }
        internal List<Asset> DependentAssets { get; private set; }

        public string Name { get; }
        public string Type { get; }

        public int Size { get; private set; }
        public int Reference { get; private set; }
        public bool IsKeepDependentAssets { get; }

        public PackageProvider Package { get; internal set; }

        internal Asset IncreaseAssetReference(byte[] bytes, List<Asset> dependentAssets)
        {
            //increase the reference of asset
            //if the reference is 0, we will create it by using the input data(bytes and dependentAssets)
            //if the reference is not 0, the bytes and dependentAssets is null
            //each increase asset reference call need to match the decrease asset reference call
            if (Reference++ == 0)
            {
                RuntimeException.Assert(mInstance == null);
                RuntimeException.Assert(Size == bytes.Length || Size <= 0);

                //create instance and keep the dependent assets
                mCreateFunction(out mInstance, bytes, DependentAssets = dependentAssets);

                LogEmitter.Apply(LogLevel.Information, "[Load and Create Asset] [Name = {0}] [Type = {1}] from [{2}]", Name, Type, Package.Name);

                //create reference asset
                return new Asset(mInstance, Size = bytes.Length, true, this);
            }

            RuntimeException.Assert(mInstance != null);

            return new Asset(mInstance, Size, true, this);
        }

        internal Asset CreateIndependentAsset(byte[] bytes, List<Asset> dependentAssets)
        {
            //create independent asset(without reference count)
            //we do not have any reference count for independent asset
            //bytes and dependentAssets can not to be null
            //the create independent asset call need to match the destory independent asset call
            //note: the dependent assets will be destoryed after this
            mCreateFunction(out object instance, bytes, dependentAssets);

            LogEmitter.Apply(LogLevel.Information, "[Load and Create Independent Asset] [Name = {0}] [Type = {1}] from [{2}]", Name, Type, Package.Name);

            return new Asset(instance, bytes.Length, false, this);
        }

        internal Asset DecreaseAssetReference(Asset asset)
        {
            //decrease the reference of asset
            //if the reference is 0, error
            //if the reference is not 0, we dispose the asset
            //set the asset's Instance to "null" and IsExisted to false
            //each increase asset reference call need to match the decrease asset reference call
            RuntimeException.Assert(asset.IsReference == true && asset.IsExisted == true);
            RuntimeException.Assert(asset.AssetDescription == this);

            RuntimeException.Assert(Reference > 0);

            //destory instance if reference is 0
            if (--Reference == 0)
            {
                //destory 
                mDestoryFunction(ref mInstance);

                LogEmitter.Apply(LogLevel.Information, "[Destory Asset] [Name = {0}] [Type = {1}] from [{2}]", Name, Type, Package.Name);
            }

            asset.Instance = null;
            asset.IsExisted = false;

            return asset;
        }

        internal Asset DestoryIndependentAsset(Asset asset)
        {
            //destory independent asset(without reference count)
            //the create independent asset call need to match the destory independent asset call
            RuntimeException.Assert(asset.IsReference == false && asset.IsExisted == true);
            RuntimeException.Assert(asset.AssetDescription == this);

            //destory instance
            var instance = asset.Instance;
            mDestoryFunction(ref instance);

            LogEmitter.Apply(LogLevel.Information, "[Destory Independent Asset] [Name = {0}] [Type = {1}] from [{2}]", Name, Type, Package.Name);

            asset.Instance = null;
            asset.IsExisted = false;

            return asset;
        }

        public AssetDescription(string name, string type, int size, 
            AssetCreateFunction createFunction, 
            AssetDestoryFunction destoryFunction, 
            bool isKeepDependentAssets = false)
        {
            Name = name; Type = type; Size = size;

            mInstance = null;
            mCreateFunction = createFunction;
            mDestoryFunction = destoryFunction;

            IsKeepDependentAssets = isKeepDependentAssets;
        }
    }
}
