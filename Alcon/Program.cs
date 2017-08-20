using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alcon
{
    class Program
    {
        static void Main(string[] args)
        {
            FileUtil fileUtil = new FileUtil();
            string[] inputData = fileUtil.ReadInputCSV();
            ImageProc imageProc = new ImageProc(inputData);
            imageProc.ReadImage();
            imageProc.ShowImage();
            //Console.ReadLine();
        }
    }
}
