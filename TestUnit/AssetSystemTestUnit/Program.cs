using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace AssetSystemTestUnit
{
    class StringAsset : Asset
    {
        protected override object ConvertBytesToInstance(byte[] bytes, List<AssetReference> dependentAssets)
        {
            var context = Encoding.UTF8.GetString(bytes);

            foreach (var asset in dependentAssets)
            {
                context += "\n[DependentAsset = " + asset.Instance + "] ";
            }

            return context;
        }

        protected override void DisposeInstance(ref object instance)
        {
            instance = null;
        }

        public StringAsset(string name, int size = -1) : base(name, size)
        {
        }
    }

    class Program
    {
        static List<AssetReference> mAssetReferences; 

        static void InitializePackage()
        {
            var systemScene = GameSystems.SystemScene;
            var assetSystem = GameSystems.AssetSystem;

            var packageRoot = systemScene.Root.GetChild(StringProperty.PackageRoot) as PackageProvider;

            packageRoot.AddChild(new PackageProvider("Package1", "Package1"));
            packageRoot.AddChild(new PackageProvider("Package2", "Package2"));
            packageRoot.GetChild("Package2").AddChild(new PackageProvider("Package3", "Package3"));
            packageRoot.GetChild("Package2").AddChild(new PackageProvider("Package4", "Package4"));

            assetSystem.AddAsset(packageRoot, new StringAsset("Asset1"));
            assetSystem.AddAsset(packageRoot, new StringAsset("Asset2"));
            assetSystem.AddAsset(packageRoot.GetChild("Package2") as PackageProvider, new StringAsset("Asset1"));
            assetSystem.AddAsset(packageRoot.GetChild("Package2") as PackageProvider, new StringAsset("Asset2"));

            var assets = new List<Asset>();

            assets.Add(packageRoot.GetAsset("Asset2"));
            assets.Add((packageRoot.GetChild("Package2") as PackageProvider).GetAsset("Asset1"));

            assetSystem.AddAssetDependencies(packageRoot.GetAsset("Asset1"), assets);
        }

        static void LoadAsset()
        {
            mAssetReferences = new List<AssetReference>();

            var systemScene = GameSystems.SystemScene;
            var assetSystem = GameSystems.AssetSystem;

            var packageRoot = systemScene.Root.GetChild(StringProperty.PackageRoot) as PackageProvider;

            mAssetReferences.Add(assetSystem.LoadAsset(packageRoot.GetAsset("Asset1")));
            mAssetReferences.Add(assetSystem.LoadAsset(packageRoot.GetAsset("Asset2")));
            mAssetReferences.Add(assetSystem.LoadAsset((packageRoot.GetChild("Package2") as PackageProvider).GetAsset("Asset1")));
            mAssetReferences.Add(assetSystem.LoadAsset((packageRoot.GetChild("Package2") as PackageProvider).GetAsset("Asset2")));
        }

        static void UnLoadAsset()
        {
            var assetSystem = GameSystems.AssetSystem;

            foreach (var asset in mAssetReferences)
            {
                var reference = asset;

                assetSystem.UnLoadAsset(ref reference);
            }

            mAssetReferences.Clear();
        }

        static void Main(string[] args)
        {
            GameSystems.Initialize();
            
            InitializePackage();

            LoadAsset();
            foreach (var asset in mAssetReferences)
                Console.WriteLine(asset.Instance);
            UnLoadAsset();
            
            GameSystems.RunLoop();
        }
    }
}
