﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alcon
{
    class FileUtil
    {

        // データ名
        string fileName;
        // T:時間総数　 Z:Z軸総数
        int T, Z;
        
        public string[] ReadInputCSV()
        {
            //System.IO.StreamReader file = new System.IO.StreamReader(@"Pre_Data01");
            //string[] files = System.IO.Directory.GetDirectories(@"Pre_Data05");
            //for (int i = 0; i < files.Length; i++)
            //{
            //    Console.WriteLine(files[i]);
            //}
            string line;
            int index = 0;
            string[] ary = new string[3];
            using (StreamReader file = new StreamReader(@"Pre_Data01/input.csv"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    ary[index++] = line;
                    Console.WriteLine(line);
                    if (index == 3) break;
                }
            }
            //StreamReader file = new StreamReader(@"Pre_Data01/input.csv");
            //while ((line = file.ReadLine()) != null)
            //{
            //    ary[index++] = line;
            //    Console.WriteLine(line);
            //    if (index == 3) break;
            //}
            fileName = ary[0];
            if (!int.TryParse(ary[1], out T))
            {
                Console.WriteLine("時間総数の取得に失敗しました");
            }

            if (!int.TryParse(ary[2], out Z))
            {
                Console.WriteLine("Z軸総数の取得に失敗しました");
            }

            Console.WriteLine("データ名:" + fileName);
            Console.WriteLine("時間:" + T);
            Console.WriteLine("Z軸:" + Z);
            return ary;
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
