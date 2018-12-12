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

        public GameScene(string name)
        {
            Name = name;

            Root = new GameObject("Root");
        }

        public virtual void Update(float deltaTime)
        {
            Root.Update(deltaTime);
        }

        public void AddGameObject(GameObject gameObject)
        {
            Root.AddChild(gameObject);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            Root.RemoveChild(gameObject);
        }
    }
}
