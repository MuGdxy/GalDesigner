using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GameSystems
    {
        private static PackageProvider mPackageProvider { get; set; }
        
        public static AssetSystem AssetSystem { get; private set; }

        public static List<BehaviorSystem> BehaviorSystems { get; set; }

        public static GameScene MainScene { get; set; }
        public static GameScene SystemScene { get; private set; }
        public static EngineWindow EngineWindow { get; set; }

        public static bool IsExist { get; set; }
        
        private static void UpdateScene(GameScene scene)
        {
            //update main scene
            scene?.Update(Time.DeltaSeconds);

            //process sub system
            foreach (var subSystem in BehaviorSystems)
            {
                //is active
                if (subSystem.IsActive is false) continue;

                //search node
                void SearchNode(GameObject node)
                {
                    if (node is null) return;

                    if (subSystem.RequireComponents.IsPass(node) is false) return;

                    subSystem.Excute(node);

                    foreach (var child in node.Children)
                        SearchNode(child);
                }

                SearchNode(scene?.Root);
            }
        }

        public static void Initialize()
        {
            BehaviorSystems = new List<BehaviorSystem>();

            IsExist = true;

            SystemScene = new GameScene("SystemScene");
            
            SystemScene.AddGameObject(mPackageProvider = new PackageProvider(StringProperty.PackageRoot, "Package"));

            //add system
            AddBehaviorSystem(AssetSystem = new AssetSystem());
            
            LogEmitter.Apply(LogLevel.Information, "[Initialize GameSystems Finish] from [GameSystems]");
        }
        
        public static void RunLoop()
        {
            while (IsExist is true)
            {
                //update time
                Time.Update();

                UpdateScene(SystemScene);
                UpdateScene(MainScene);

                if (EngineWindow != null && EngineWindow.IsExisted != false)
                    EngineWindow.Update(Time.DeltaSeconds);
            }
        }

        public static void AddBehaviorSystem(BehaviorSystem behaviorSystem)
        {
            BehaviorSystems.Add(behaviorSystem);
            
            LogEmitter.Apply(LogLevel.Information, "[Add Behavior System] [Name = {0}] from [GameSystems]", behaviorSystem.Name);
        }

        public static void RemoveBehaviorSystem(BehaviorSystem behaviorSystem)
        {
            BehaviorSystems.Remove(behaviorSystem);

            LogEmitter.Apply(LogLevel.Information, "[Remove Behavior System] [Name = {0}] from [GameSystems]", behaviorSystem.Name);
        }
    }
}
