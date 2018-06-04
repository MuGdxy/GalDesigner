using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    static class ScriptManager
    {
        private static List<string>[] scripts = new List<string>[(int)ScriptType.Count];

        private static void ProcessScriptFile(ScriptFile script)
        {
            var buffer = new byte[script.Stream.Length];

            script.Stream.Read(buffer, 0, buffer.Length);

            string content = Encoding.UTF8.GetString(buffer);

            bool state = false; //true is in block, false is not.

            int line = 1;

            var head = "";
            var block = "";

            foreach (var item in content)
            {
                switch (item)
                {
                    case '{':

                        DebugLayer.Assert(state is true, ErrorType.InvaildScriptFormat, line, script.FileName);

                        state = true;

                        continue;
                    case '}':

                        DebugLayer.Assert(state is false, ErrorType.InvaildScriptFormat, line, script.FileName);

                        state = false;

                        block = "";
                        head = "";

                        continue;
                    case '\n':

                        line++;
                        break;
                    default:
                        break;
                }

                if (state is false && item != ' ') head += item;
            }
        }

        public static void Initialize()
        {
            foreach (var item in FileManager.Scripts)
            {
                ProcessScriptFile(item);
            }
        }
    }
}
