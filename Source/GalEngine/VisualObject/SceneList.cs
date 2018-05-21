using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    internal static class SceneList
    {
        private static Dictionary<string, GenericScene> sceneList = new Dictionary<string, GenericScene>();

        internal static void AddScene(GenericScene scene)
        {
            sceneList[scene.Name] = scene;
        }

        public static Dictionary<string, GenericScene> Element => sceneList;
    }
}
