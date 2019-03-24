using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine.Runtime.Graphics;

namespace GalEngine
{
    public class GameStartInfo
    {
        public string WindowName { get; set; }
        public string GameName { get; set; }
        public string IconName { get; set; }

        public Size<int> WindowSize { get; set; }
    }

    public static class GameSystems
    {
        private static Package Package { get; set; }
        private static GuiControl GuiControl { get; set; }
        
        public static AssetSystem AssetSystem { get; private set; }
        public static GuiSystem GuiSystem { get; private set; }

        public static List<BehaviorSystem> BehaviorSystems { get; set; }

        public static GameScene MainScene { get; set; }
        public static GameScene SystemScene { get; private set; }
        public static EngineWindow EngineWindow { get; private set; }
        public static GpuDevice GraphicsDevice { get; private set; }


        public static string GameName { get; private set; }
        public static bool IsExist { get; set; }
        
        private static void UpdateBehaviorSystem(BehaviorSystem system)
        {
            if (system.IsActive == false) return;

            //update system
            system.Update();

            //build scenes
            GameScene[] scenes = new GameScene[2]
            {
                SystemScene, MainScene
            };

            foreach (var scene in scenes)
            {
                if (scene == null) continue;

                //create passed gameobject list
                List<GameObject> passedGameObjects = new List<GameObject>();

                void SearchNode(GameObject node, ref List<GameObject> passedList)
                {
                    if (node is null) return;

                    //if node can not pass the requirement
                    //we will not to search the children of node
                    if (system.RequireComponents.IsPass(node) is false) return;

                    passedList.Add(node);

                    foreach (var child in node.Children)
                        SearchNode(child, ref passedList);
                }

                foreach (var child in scene.Root.Children)
                    SearchNode(child, ref passedGameObjects);

                system.Excute(passedGameObjects);
            }
        }

        private static void InitializeRuntime(GameStartInfo gameStartInfo)
        {
            var adapters = GpuAdapter.EnumerateGraphicsAdapter();

            LogEmitter.Assert(adapters.Count > 0, LogLevel.Error, "[Initialize Graphics Device Failed without Support Adapter] from [GameSystems]");

            GraphicsDevice = new GpuDevice(adapters[0]);

            EngineWindow = new EngineWindow(
                gameStartInfo.WindowName, 
                gameStartInfo.IconName, 
                gameStartInfo.WindowSize);
            EngineWindow.Show();
        }

        public static void Initialize(GameStartInfo gameStartInfo)
        {
            GameName = gameStartInfo.GameName;

            BehaviorSystems = new List<BehaviorSystem>();

            IsExist = true;

            SystemScene = new GameScene("SystemScene");
            
            SystemScene.AddGameObject(Package = new Package(StringProperty.PackageRoot, "Package"));
            SystemScene.AddGameObject(GuiControl = new GuiControl(StringProperty.GuiControlRoot));

            InitializeRuntime(gameStartInfo);

            //add system
            AddBehaviorSystem(AssetSystem = new AssetSystem());
            AddBehaviorSystem(GuiSystem = new GuiSystem(GraphicsDevice, new Rectangle<int>(0, 0, EngineWindow.Size.Width, EngineWindow.Size.Height)));
            
            LogEmitter.Apply(LogLevel.Information, "[Initialize GameSystems Finish] from [GameSystems]");
        }
        
        public static void RunLoop()
        {
            while (IsExist is true)
            {
                //update time
                Time.Update();

                SystemScene?.Update(Time.DeltaSeconds);
                MainScene?.Update(Time.DeltaSeconds);

                foreach (var subSystem in BehaviorSystems) UpdateBehaviorSystem(subSystem);

                if (EngineWindow != null && EngineWindow.IsExisted != false)
                    EngineWindow.Update(Time.DeltaSeconds);

                if (EngineWindow != null && EngineWindow.IsExisted == false)
                    IsExist = false;
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
