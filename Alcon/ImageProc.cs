using System;
using System.Collections.Generic;
using OpenCvSharp;

namespace Alcon
{
    class ImageProc
    {
        // データ名
        string fileName;
        // T:時間総数　 Z:Z軸総数
        int T, Z;
        int all_count = 0;
        int count_1 = 0; // 前フレームの分裂候補数

        List<List<Mat>> Binary = new List<List<Mat>>();
        //List<List<Mat>> Result = new List<List<Mat>>();
        Mat[][] Result;

        //分裂候補情報を全て保存する配列
        List<int> all_x = new List<int>();
        List<int> all_y = new List<int>();
        List<int> all_w = new List<int>();
        List<int> all_h = new List<int>();
        List<int> all_z = new List<int>();
        List<int> all_t = new List<int>();

        public ImageProc(string[] ary)
        {
            fileName = ary[0];
            T = int.Parse(ary[1]);
            Z = int.Parse(ary[2]);
            Result = new Mat[T][];
            for(int t = 0; t < T; t++)
            {
                Result[t] = new Mat[Z];
            }
        }

        public void Exec()
        {
            //分裂候補を探すための配列
            List<int> count_x_1 = new List<int>();
            List<int> count_y_1 = new List<int>();
            for (int t = 0; t < T; t++)
            {
                for (int z = 0; z < Z; z++)
                {
                    string name = $"{fileName}/t{t + 1:000}/{fileName}_t{t + 1:000}_page_{z + 1:0000}.tif";
                    Result[t][z] = Cv2.ImRead(name, ImreadModes.Color);
                    Mat tmp = Cv2.ImRead(name, 0);
                    Mat bin = new Mat();
                    Cv2.Threshold(tmp, bin, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
                    Detect(bin, ref count_x_1, ref count_y_1, t, z);
                }
            }
        }

        public void Detect(Mat bin, ref List<int> count_x_1, ref List<int> count_y_1, int t, int z)
        {
            List<int> count_x = new List<int>();
            List<int> count_y = new List<int>();
            List<int> count_w = new List<int>();
            List<int> count_h = new List<int>();
            List<int> count_z = new List<int>();
            List<int> count_t = new List<int>();

             // ラベリング
             Mat status = new Mat();
             Mat center = new Mat();
             Mat labelTmp = new Mat();
             int nLabels = Cv2.ConnectedComponentsWithStats(bin, labelTmp, status, center, PixelConnectivity.Connectivity8, MatType.CV_32SC1);
             int count = 0; // 現在のフレームの分裂候補数
             List<Scalar> colors = new List<Scalar>();

             var param = status.GetGenericIndexer<int>();
             for (int label = 0; label < nLabels; label++)
             {
                 int x = param[label, 0];
                 int y = param[label, 1];
                 int width = param[label, 2];
                 int height = param[label, 3];
                 int area = param[label, 4];
                 double occupancy = (double)area / (height * width);
                 double aspect = (double)height / width;

                 //ラべリング情報による閾値設定(閾値以内なら分裂候補、以外ならノイズ)
                 if (CheckInRange(width, height, area, occupancy, aspect))
                 {
                     //現在フレームにおける分裂候補を数える
                     count++;
                     //現在フレームにおける分裂候補の情報を保存
                     count_x.Add(x);
                     count_y.Add(y);
                     count_w.Add(width);
                     count_h.Add(height);
                     count_z.Add(z);
                     count_t.Add(t);
                     //分裂候補なので白で描画
                     colors.Add(Scalar.White);
                 }
                 else
                 {
                     //分裂候補でないので黒で描画（ノイズ）
                     colors.Add(Scalar.Black);
                 }
             }
             for (int c = 0; c < count; c++)
             {
                 for (int v = 0; v < count_1; v++)
                 {
                     //候補座標同士の差が絶対値10px以下ならば分裂候補として再確定
                     if (Math.Abs(count_x[c] - count_x_1[v]) <= 10 && Math.Abs(count_y[c] - count_y_1[v]) <= 10 && count_y[c] != 0)
                     {
                         Console.WriteLine("時間：{0}、Z軸：{1}、(x,y)=({2},{3})", count_t[count - 1], count_z[count - 1], count_x[c], count_y[c]);
                         //分裂候補の情報を保存する
                         all_x.Add(count_x[c]);
                         all_y.Add(count_y[c]);
                         all_w.Add(count_w[c]);
                         all_h.Add(count_h[c]);
                         all_z.Add(count_z[c]);
                         all_t.Add(count_t[c]);
                         //分裂候補の総数を数える
                         all_count++;
                         Result[t][z].Rectangle(new Rect(count_x[c], count_y[c], count_w[c], count_h[c]), new Scalar(0, 0, 255), 1);
                     }
                 }
             }

            //1フレームごとに適用させるために配列の初期化を行う
            //過去候補配列を初期化
            count_x_1.Clear();
            count_y_1.Clear();

            //過去候補配列に現在フレームの分裂候補結果を保存
            for (int c = 0; c < count; c++)
             {
                 count_x_1.Add(count_x[c]);
                 count_y_1.Add(count_y[c]);
             }

             count_1 = count;
        }

        // 閾値以内か確認
        bool CheckInRange(int width, int height, int area, double occupancy, double aspect)
        {
            int tmp = 0;
            if (50 <= area && area <= 550) tmp++;  //面積
            if (0.75 <= aspect && aspect <= 1.25) tmp++; //縦横比
            if (10 <= height && height <= 50 && 10 <= width && width <= 50) tmp++; //縦幅横幅
            if (0.1 <= occupancy && occupancy <= 0.4) tmp++; //占有率

            if (tmp == 4) return true;
            return false;
        }

        public void ShowImage()
        {
            Cv2.NamedWindow("結果", WindowMode.AutoSize);
            for (int i = 0; i < T; i++)
            {
                for (int j = 0; j < Z; j++)
                {
                    Cv2.ImShow("結果", Result[i][j]);
                    Cv2.WaitKey(50);
                }
            }
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

    }
}
