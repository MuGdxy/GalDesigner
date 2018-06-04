using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    static class FileManager
    {
        private const string ScirptSuffixName = "gs";

        private static List<ScriptFile> scripts = new List<ScriptFile>();

        private static void ReadScriptFile(string directoryPath)
        {
            //Get files in this directory
            var files = Directory.GetFiles(directoryPath);

            foreach (var item in files)
            { 
                if (Utilities.GetFileSuffix(item) == ScirptSuffixName)
                {
                    scripts.Add(new ScriptFile(item, new FileStream(item, FileMode.Open)));
                }
            }

            //Get directorys in this directory
            var directorys = Directory.GetDirectories(directoryPath);

            foreach (var item in directorys)
            {
                ReadScriptFile(item);
            }
        }

        public static void Initialize()
        {
            ReadScriptFile(Directory.GetCurrentDirectory());
        }

        public static List<ScriptFile> Scripts => scripts;
    }
}
