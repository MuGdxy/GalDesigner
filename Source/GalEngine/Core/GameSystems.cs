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
        private static PackageProvider PackageProvider { get; set; }
        
        public static AssetSystem AssetSystem { get; private set; }
        public static GuiSystem GuiSystem { get; private set; }

        public static List<BehaviorSystem> BehaviorSystems { get; set; }

        public static GameScene MainScene { get; set; }
        public static GameScene SystemScene { get; private set; }
        public static EngineWindow EngineWindow { get; private set; }
        public static GraphicsDevice GraphicsDevice { get; private set; }


        public static string GameName { get; private set; }
        public static bool IsExist { get; set; }
        
        private static void UpdateScene(GameScene scene)
        {
            //update main scene
            scene?.Update(Time.DeltaSeconds);

            //process sub system
            foreach (var subSystem in BehaviorSystems)
            {
                List<GameObject> passedGameObjects = new List<GameObject>();

                //is active
                if (subSystem.IsActive is false) continue;

                //update sub system status
                subSystem.Update();

                //search node
                void SearchNode(GameObject node, ref List<GameObject> passedList)
                {
                    if (node is null) return;

                    //if node can not pass the requirement
                    //we will not to search the children of node
                    if (subSystem.RequireComponents.IsPass(node) is false) return;

                    passedList.Add(node);

                    foreach (var child in node.Children)
                        SearchNode(child, ref passedList);
                }

                SearchNode(scene?.Root, ref passedGameObjects);

                subSystem.Excute(passedGameObjects);
            }
        }

        private static void InitializeRuntime(GameStartInfo gameStartInfo)
        {
            var adapters = GraphicsAdapter.EnumerateGraphicsAdapter();

            LogEmitter.Assert(adapters.Count > 0, LogLevel.Error, "[Initialize Graphics Device Failed without Support Adapter] from [GameSystems]");

            GraphicsDevice = new GraphicsDevice(adapters[0]);

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
            
            SystemScene.AddGameObject(PackageProvider = new PackageProvider(StringProperty.PackageRoot, "Package"));

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

                UpdateScene(SystemScene);
                UpdateScene(MainScene);

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
