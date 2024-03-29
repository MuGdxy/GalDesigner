﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace AssetSystemTestUnit
{
    class StringAssetDescription : AssetDescription
    {
        private static void CreateFunction(out object instance, byte[] bytes, List<Asset> dependentAssets)
        {
            var context = Encoding.UTF8.GetString(bytes);

            foreach (var asset in dependentAssets) context += "\n[DependentAsset = " + asset.Instance + "] ";

            instance = context;
        }

        private static void DestoryFunction(ref object instance)
        {
            instance = null;
        }

        public StringAssetDescription(string name, int size = 0, 
            bool isKeepDependentAssets = true) : base(name, "string", size, CreateFunction, DestoryFunction, isKeepDependentAssets)
        {

        }
    }

    class Program
    {
        static List<Asset> mAssets; 

        static void InitializePackage()
        {
            var systemScene = GameSystems.SystemScene;
            var assetSystem = GameSystems.AssetSystem;

            var packageRoot = systemScene.Root.GetChild(StringProperty.PackageRoot) as Package;

            packageRoot.AddChild(new Package("Package1", "Package1"));
            packageRoot.AddChild(new Package("Package2", "Package2"));
            packageRoot.GetChild("Package2").AddChild(new Package("Package3", "Package3"));
            packageRoot.GetChild("Package2").AddChild(new Package("Package4", "Package4"));

            assetSystem.AddAssetDescription(packageRoot, new StringAssetDescription("Asset1"));
            assetSystem.AddAssetDescription(packageRoot, new StringAssetDescription("Asset2"));
            assetSystem.AddAssetDescription(packageRoot.GetChild("Package2") as Package, new StringAssetDescription("Asset1"));
            assetSystem.AddAssetDescription(packageRoot.GetChild("Package2") as Package, new StringAssetDescription("Asset2"));

            var assets = new List<AssetDescription>();

            assets.Add(packageRoot.GetAssetDescription("Asset2"));
            assets.Add((packageRoot.GetChild("Package2") as Package).GetAssetDescription("Asset1"));

            assetSystem.AddAssetDependencies(packageRoot.GetAssetDescription("Asset1"), assets);
        }

        static void LoadAsset()
        {
            mAssets = new List<Asset>();

            var systemScene = GameSystems.SystemScene;
            var assetSystem = GameSystems.AssetSystem;

            var packageRoot = systemScene.Root.GetChild(StringProperty.PackageRoot) as Package;

            mAssets.Add(assetSystem.CreateAsset(packageRoot.GetAssetDescription("Asset1")));
            mAssets.Add(assetSystem.CreateAsset(packageRoot.GetAssetDescription("Asset2")));
            mAssets.Add(assetSystem.CreateAsset((packageRoot.GetChild("Package2") as Package).GetAssetDescription("Asset1")));
            mAssets.Add(assetSystem.CreateAsset((packageRoot.GetChild("Package2") as Package).GetAssetDescription("Asset2")));
        }

        static void UnLoadAsset()
        {
            var assetSystem = GameSystems.AssetSystem;

            foreach (var asset in mAssets)
            {
                assetSystem.DestoryAsset(asset);
            }

            mAssets.Clear();
        }

        static void Main(string[] args)
        {
            GameSystems.Initialize(new GameStartInfo()
            {
                WindowName = "AssetSystemTestUnit",
                GameName = "AssetSystemTestUnit",
                IconName = "",
                WindowSize = new Size<int>(1920, 1080)
            });
            
            InitializePackage();

            LoadAsset();
            foreach (var asset in mAssets)
                Console.WriteLine(asset.Instance);
            UnLoadAsset();
            
            GameSystems.RunLoop();
        }
    }
}
