using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Utility
{
    public class FileUtility
    {
        public static void WriteTextToDataFile(string text, string path)
        {
            using (StreamWriter writetext = new StreamWriter(path))
            {
                writetext.Write(text);
            }
        }
    }
}
