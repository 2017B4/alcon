using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace Alcon
{
    class ImageProc
    {
        // データ名
        string fileName;
        // T:時間総数　 Z:Z軸総数
        int T, Z;

        List<List<Mat>> Original = new List<List<Mat>>();
        List<List<Mat>> Binary = new List<List<Mat>>();
        List<List<Mat>> Label = new List<List<Mat>>();
        List<List<Mat>> Result = new List<List<Mat>>();

        public ImageProc(string[] ary)
        {
            fileName = ary[0];
            T = int.Parse(ary[1]);
            Z = int.Parse(ary[2]);
        }

        public void ReadImage()
        {
            for (int i = 1; i <= T; i++)
            {
                List<Mat> origin = new List<Mat>();
                List<Mat> bins = new List<Mat>();
                List<Mat> res = new List<Mat>();
                for (int j = 1; j <= Z; j++)
                {
                    string name = $"{fileName}/t{i:000}/{fileName}_t{i:000}_page_{j:0000}.tif";
                    Mat tmp = Cv2.ImRead(name, 0);
                    Mat bin = new Mat();
                    Cv2.Threshold(tmp, bin, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
                    origin.Add(tmp);
                    bins.Add(bin);
                    res.Add(Cv2.ImRead(name, ImreadModes.Color));
                }
                Original.Add(origin);
                Binary.Add(bins);
                Result.Add(res);
            }
        }

        public void ShowImage()
        {
            Cv2.NamedWindow("元画像", WindowMode.AutoSize);
            Cv2.NamedWindow("二値化", WindowMode.AutoSize);
            Cv2.NamedWindow("結果", WindowMode.AutoSize);
            for (int i = 1; i < T; i++) {
                for(int j = 1; j < Z; j++) {
                    Cv2.ImShow("元画像", Original[i][j]);
                    Cv2.ImShow("二値化", Binary[i][j]);
                    Cv2.ImShow("結果", Result[i][j]);
                    Cv2.WaitKey(50);
                }
            }
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

    }
}
