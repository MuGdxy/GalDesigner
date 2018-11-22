using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    /// <summary>
    /// game scene
    /// </summary>
    public class GameScene
    {
        public string Name { get; set; }

        public GameObject Root { get; }

        public List<BehaviorSystem> BehaviorSystems { get; set; }

        public static GameScene Main { get; }

        static GameScene()
        {
            Main = new GameScene("MainScene");

            //console log system
            Main.AddBehaviorSystem(new ConsoleLogSystem());
        }

        public GameScene(string name)
        {
            Name = name;

            Root = new GameObject("Root");
            BehaviorSystems = new List<BehaviorSystem>();
        }

        public virtual void Update(float deltaTime)
        {
            Root.Update(deltaTime);

            foreach (var subSystem in BehaviorSystems)
            {
                void SearchNode(GameObject node)
                {
                    if (subSystem.RequireComponents.IsPass(node) is true)
                        subSystem.Excute(node);

                    foreach (var child in node.Children)
                        SearchNode(child);
                }

                SearchNode(Root);
            }
        }

        public void AddGameObject(GameObject gameObject)
        {
            Root.AddChild(gameObject);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            Root.RemoveChild(gameObject);
        }

        public void AddBehaviorSystem(BehaviorSystem behaviorSystem)
        {
            BehaviorSystems.Add(behaviorSystem);
        }

        public void RemoveBehaviorSystem(BehaviorSystem behaviorSystem)
        {
            BehaviorSystems.Remove(behaviorSystem);
        }
    }
}
