using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alcon
{
    class FileUtil
    {

        public void ReadInputCSV()
        {
            //System.IO.StreamReader file = new System.IO.StreamReader(@"Pre_Data01");
            string[] files = System.IO.Directory.GetDirectories(@"Pre_Data01");
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine(files[i]);
            }
            
        }

        public static List<string> getImageList(string path)
        {
            List<string> list = new List<string>();
            string[] files = System.IO.Directory.GetFiles(path);
            Regex regex = new Regex("^[a-zA-Z0-9].*\\.(jpg|bmp)$", RegexOptions.Compiled);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Remove(0, 7);
                if (regex.IsMatch(files[i]))
                {
                    list.Add(files[i]);
                }
            }
            return list;
        }

        public static List<string> getCascadeList(string path)
        {
            List<string> list = new List<string>();
            string[] files = System.IO.Directory.GetFiles(path);
            Regex regex = new Regex("^(haarcascade_).*\\.xml$", RegexOptions.Compiled);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Remove(0, 8);
                if (regex.IsMatch(files[i]))
                {
                    list.Add(files[i]);
                }
            }
            return list;
        }
    }
}
