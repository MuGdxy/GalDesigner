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
        private LogProvider mLogProvider { get; }

        public string Name { get; set; }

        public GameObject Root { get; }

        public GameScene(string name)
        {
            Name = name;

            Root = new GameObject("Root");

            AddGameObject(mLogProvider = new LogProvider(name));

            mLogProvider.Log(StringGroup.Log + "[Initialize GameScene Finish].");
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
