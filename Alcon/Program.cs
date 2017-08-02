using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace Alcon
{
    class Program
    {
        static void Main(string[] args)
        {
            FileUtil fileUtil = new FileUtil();
            fileUtil.ReadInputCSV();
            System.Console.ReadLine();
        }
    }
}
