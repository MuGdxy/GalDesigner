﻿using System;
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
        
        public static ConsoleLogSystem ConsoleLogSystem { get; }
        public static AssetSystem AssetSystem { get; }

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
            SystemScene.AddGameObject(mPackageProvider = new PackageProvider(StringProperty.PackageRoot, "Package"));

            //add system
            AddBehaviorSystem(ConsoleLogSystem = new ConsoleLogSystem());
            AddBehaviorSystem(AssetSystem = new AssetSystem());

            mLogProvider.Log(StringProperty.Log + "[Initialize GameSystems Finish].");
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

            mLogProvider.Log(StringProperty.Log + "[Add Behavior System] [Name = {0}].", LogLevel.Information,
                behaviorSystem.Name);
        }

        public static void RemoveBehaviorSystem(BehaviorSystem behaviorSystem)
        {
            BehaviorSystems.Remove(behaviorSystem);

            mLogProvider.Log(StringProperty.Log + "[Remove Behavior System] [Name = {0}].", LogLevel.Information,
                behaviorSystem.Name);
        }
    }
}
