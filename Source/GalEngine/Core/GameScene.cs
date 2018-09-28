using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class GameScene
    {
        private string name;

        private Camera defaultCamera;
        private Camera currentCamera;
        private GameObject root;

        private ValueManager<Size> resolution;

        private Bitmap renderTarget;

        private List<BehaviorSystem> behaviorSystems;

        private static GameScene mainScene = null;

        protected virtual void OnResolutionChange(object owner, Size oldValue, Size newValue)
        {
            GameScene scene = owner as GameScene;

            Utility.Dispose(ref scene.renderTarget);

            renderTarget = new Bitmap(newValue);

            defaultCamera.Area = new RectangleF(0, 0, newValue.Width, newValue.Height);
        }

        public GameScene(string sceneName, Size sceneResolution)
        {
            name = sceneName;
            resolution = new ValueManager<Size>(sceneResolution, OnResolutionChange);

            defaultCamera = new Camera(0, 0, sceneResolution.Width, sceneResolution.Height);
            currentCamera = defaultCamera;

            root = new GameObject("Root");

            renderTarget = new Bitmap(sceneResolution);

            behaviorSystems = new List<BehaviorSystem>();
        }

        public virtual void Update(float deltaTime)
        {
            resolution.Update(this, true);

            root?.Update(deltaTime);

            Systems.Graphics.BeginDraw(renderTarget);
            Systems.Graphics.Clear(new Color(1, 1, 1, 1));

            behaviorSystems.ForEach((BehaviorSystem system) => { system.Excute(); });

            Systems.Graphics.EndDraw();
        }

        public void SetDefaultCamera()
        {
            currentCamera = defaultCamera;
        }

        public void SetGameObject(GameObject gameObject)
        {
            root.SetChild(gameObject);
        }

        public void SetBehaviorSystem(BehaviorSystem behaviorSystem)
        {
            behaviorSystems.Add(behaviorSystem);
        }

        public void CancelGameObject(GameObject gameObject)
        {
            root.CancelChild(gameObject);
        }

        public void CancelBehaviorSystem(BehaviorSystem behaviorSystem)
        {
            behaviorSystems.Remove(behaviorSystem);
        }

        public string Name { get => name; }

        public Camera Camera { get => currentCamera; set => currentCamera = value; }
        public GameObject Root { get => root; }

        public Size Resolution { get => resolution.Value; set => resolution.Value = value; }

        public Bitmap RenderTarget { get => renderTarget; }

        public static GameScene Main { get => mainScene; set => mainScene = value; }
    }
}
