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
            imageProc.Exec();
            imageProc.ShowImage();

            //ImageProc2 imageProc2 = new ImageProc2(inputData);
            //imageProc2.ReadImage();
            //imageProc2.ExecDetection();
            //imageProc2.ShowImage();
        }
    }
}
