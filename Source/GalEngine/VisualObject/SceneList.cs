using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    internal static class SceneList
    {
        private static Dictionary<string, Scene> sceneList = new Dictionary<string, Scene>();

        internal static void AddScene(Scene scene)
        {
            sceneList.Add(scene.Name, scene);
        }

        public static Dictionary<string, Scene> Element => sceneList;
    }
}
