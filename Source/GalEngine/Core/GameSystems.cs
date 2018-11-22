using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GameSystems
    {
        public static LogProvider LogProvider { get; }

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

            SystemScene.AddGameObject(LogProvider = new LogProvider("GameSystems"));

            //add log system
            AddBehaviorSystem(new ConsoleLogSystem());

            LogProvider.Log("[Log] [object] [time] : Finish Initialize GameSystems.");
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

            LogProvider.Log("[Log] [object] [time] : Add Behavior System [Name: {0}].",
                behaviorSystem.Name);
        }

        public static void RemoveBehaviorSystem(BehaviorSystem behaviorSystem)
        {
            BehaviorSystems.Remove(behaviorSystem);

            LogProvider.Log("[Log] [object] [time] : Remove Behavior System [Name: {0}].",
                behaviorSystem.Name);
        }
    }
}
