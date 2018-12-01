using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GameSystems
    {
        private static LogProvider mLogProvider { get; }
        private static PackageProvider mPackageProvider { get; }
        
        public static List<BehaviorSystem> BehaviorSystems { get; set; }

        public static GameScene MainScene { get; set; }
        public static GameScene SystemScene { get; }

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

                    if (subSystem.RequireComponents.IsPass(node) is true)
                        subSystem.Excute(node);

                    foreach (var child in node.Children)
                        SearchNode(child);
                }

                SearchNode(scene?.Root);
            }
        }

        static GameSystems()
        {
            BehaviorSystems = new List<BehaviorSystem>();
           
            IsExist = true;

            SystemScene = new GameScene("SystemScene");

            SystemScene.AddGameObject(mLogProvider = new LogProvider("GameSystems"));
            SystemScene.AddGameObject(mPackageProvider = new PackageProvider("Package Provider Root", "Package"));
            
            //add log system
            AddBehaviorSystem(new ConsoleLogSystem());

            mLogProvider.Log(StringGroup.Log + "[Initialize GameSystems Finish].");
        }

        public static void RunLoop()
        {
            while (IsExist is true)
            {
                //update time
                Time.Update();

                UpdateScene(SystemScene);
                UpdateScene(MainScene);
            }
        }

        public static void AddBehaviorSystem(BehaviorSystem behaviorSystem)
        {
            BehaviorSystems.Add(behaviorSystem);

            mLogProvider.Log(StringGroup.Log + "[Add Behavior System] [Name = {0}].",
                behaviorSystem.Name);
        }

        public static void RemoveBehaviorSystem(BehaviorSystem behaviorSystem)
        {
            BehaviorSystems.Remove(behaviorSystem);

            mLogProvider.Log(StringGroup.Log + "[Remove Behavior System] [Name = {0}].",
                behaviorSystem.Name);
        }
    }
}
