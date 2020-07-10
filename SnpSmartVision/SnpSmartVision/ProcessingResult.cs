using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SnpSystem.Vision.VisionConfigurationHelper;

using NationalInstruments.Vision;
using NationalInstruments.Vision.WindowsForms;
using NationalInstruments.Vision.Acquisition.Imaqdx;
using NationalInstruments.Vision.Analysis;

namespace SnpSmartVision
{
   
    public struct ProcessingValue
    {
        public int ColorRatio;
        public int AspectRatio;
        public int Size;
        public bool noFruit;
    }

    public class ProcessResultArgs : EventArgs
    {
        public ProcessingValue[] Results;
        public ProcessResultArgs(ProcessingValue[] results)
        {
            Results = new ProcessingValue[results.Length];
            Results = results;
        }
    }

    public class OverlayDisplayData
    {
        public RectangleContour[,] RContour;
        public PointContour[,] PContour;
        public double[,] Size;
        public double[,] Color;
        int size,lane;
        public OverlayDisplayData(int lane,int size)
        {
            this.size = size;
            this.lane = lane;
            RContour = new RectangleContour[lane,size];
            PContour = new PointContour[lane,size];
            Size = new double[lane,size];
            Color = new double[lane,size];
            Clear();
        }
        public void Clear()
        {
            for (int j = 0; j < lane; j++)
            {
                for (int i = 0; i < size; i++)
                {
                    Size[j, i] = 0;
                    Color[j, i] = 0;
                }
            }
        }
    }

    // color[SequenceNo,CarrierNo]

    public class ProcessingResult
    {
        
        public const int MAXIMUM_CAMERA_COUNT=3; 
        int cameraCount;
        double[,,] Color;
        double[,,] Size;
        //double[,,] ColorAccumulated;
        //double[,,] SizeAccumulated;
        int[,,] ColorRatio;
        int[,,] AspectRatio;
        //RectangleContour[,] Rect;
        //PointContour[,] Point;
        ProcessingValue PValue;
        int memSize;
        int colorRatio;
        public int CameraCount
        {
            get { return cameraCount; }
            set { cameraCount = value;}
        }

        public DataBindingType ColorRatioDataType
        {
            get;
            set;
        }

        public DataBindingType SizeDataType
        {
            get;
            set;
        }

        public DataBindingType AspectRatioDataType
        {
            get;
            set;
        }

        public ProcessingResult(int size,int colorRatio)
        {
            memSize = size;
            this.colorRatio = colorRatio;
            Color = new double[MAXIMUM_CAMERA_COUNT,size,size];
            Size = new double[MAXIMUM_CAMERA_COUNT,size,size];
            ColorRatio = new int[MAXIMUM_CAMERA_COUNT,size,size];            
            AspectRatio = new int[MAXIMUM_CAMERA_COUNT,size,size];          
            //ColorAccumulated = new double[MAXIMUM_CAMERA_COUNT, size, size];    //2017.7.17 
            //SizeAccumulated = new double[MAXIMUM_CAMERA_COUNT, size, size];     //2017.7.17

            //Rect = new RectangleContour[size, size];
            //Point = new PointContour[size, size];
            InitData();
        }
        
        // 처리결과를 활용하기 위해 빼내온다
        public string PokeColor(int camIndex, int sIndex)
        {
            string s = sIndex.ToString("D2")+":Color: ";
            for (int i = 0; i < memSize; i++) s+= string.Format("{0:00000}  ",Color[camIndex,sIndex,i]); 
            return s;
        }
        public string PokeColorRatio(int camIndex, int sIndex)
        {
            string s = sIndex.ToString("D2") + ":C.R.: ";
            for (int i = 0; i < memSize; i++) s += string.Format("{0:00000}  ", ColorRatio[camIndex, sIndex, i]);
            return s;
        }
        public string PokeSize(int camIndex, int sIndex)
        {
            string s = sIndex.ToString("D2")+":Size: ";
            for (int i = 0; i < memSize; i++) s += string.Format("{0:00000}  ", Size[camIndex, sIndex, i]);
            return s;
        }

        public string PokeAspectRatio(int camIndex, int sIndex)
        {
            string s = sIndex.ToString("D2")+":A.R.: ";
            for (int i = 0; i < memSize; i++) s += string.Format("{0:00.0}  ", AspectRatio[camIndex, sIndex, i]/10.0);
            return s;
        }

        /*   if memSize =4, seq=2;
         *   i=0, index=2, cindex=3
         *   i=1, index=1, cindex=2
         *   i=2, index=0, cindex=1
         *   i=3, index=3, cindex=0
         */
        
        public void InitDiagonals(int seq)
        {
            int index = seq;
            int cindex;
            for (int ci = 0; ci < cameraCount; ci++)
            {
                for (int i = 0; i < memSize; i++)
                {
                    cindex = memSize - i - 1;
                    Color[ci,index,cindex]              = 0.0;
                    Size[ci,index,cindex]               = 0.0;
                    ColorRatio[ci,index,cindex]         = 0;
                    AspectRatio[ci,index,cindex]        = 0;
                    //SizeAccumulated[ci, index, cindex]  = 0;
                    //ColorAccumulated[ci, index, cindex] = 0;
                    index--;
                    if (index < 0) index = memSize - 1;
                }
            }
        }


        public void PokeColorRatio(int seq)
        {
            int index;
            int cindex;
            int sum=0,max=0,min=0;
            int value;
            double size,color;
            double colorTotal=0,sizeTotal=0;

            //sizeTotal=0;
            //colorTotal=0;
            //PValue.noFruit = false;
            
            for (int ci = 0; ci < cameraCount; ci++)
            {
                index = seq;
                for (int i = 0; i < memSize; i++)
                {
                    cindex = memSize - i - 1;
                    value = ColorRatio[ci,index,cindex];
                    size = Size[ci,index,cindex];
                    color = Color[ci, index, cindex];
        
                    if (i == 0 && ci==0)
                    {
                        min = value;
                        max = value;
                        sum = value;
                        colorTotal = color;
                        sizeTotal = size;
                    }
                    else
                    {
                        if (min > value) min = value;
                        if (max < value) max = value;
                        sum += value;
                        colorTotal += color;
                        sizeTotal += size;
                    }
                    index--;
                    if (index < 0) index = memSize - 1;
                }
            }

            if (ColorRatioDataType == DataBindingType.Average)
            {
                PValue.ColorRatio = memSize > 0 ? (int)(sum / memSize) : 0;
                if (PValue.ColorRatio >= 1000) PValue.ColorRatio = 1000;
            }
            else if (ColorRatioDataType == DataBindingType.Maximum)
            {
                PValue.ColorRatio = max;
                if (PValue.ColorRatio >= 1000) PValue.ColorRatio = 1000;
            }
            else if (ColorRatioDataType == DataBindingType.Minimum)
            {
                PValue.ColorRatio = min;
                if (PValue.ColorRatio >= 1000) PValue.ColorRatio = 1000;
            }
            else if (ColorRatioDataType == DataBindingType.LastAverage)  //2017.7.17 =>모든Size,Color를 더하여 마지막에 착색률을 계산  
            {
              if(sizeTotal<=0)
              {
                  PValue.ColorRatio = 0;
                  PValue.noFruit = true;
              }
              else
              {
                  if(colorTotal>=sizeTotal) 
                      PValue.ColorRatio = 1000;
                  else 
                      PValue.ColorRatio = (int)((colorTotal*1000) / sizeTotal);
              }
             }
             else PValue.ColorRatio = sum;
            
            //다조에서 각 레인별 색상값의 BALANCE를 맞추기위해 color factor를 사용함 2017.7.29
            if (colorRatio != 0)
            {
                float nColor = (float)PValue.ColorRatio;
                PValue.ColorRatio = (int)(colorRatio * nColor) / 1000;
                if (PValue.ColorRatio >= 1000) PValue.ColorRatio = 1000;
            }

        }

        public void PokeAspectRatio(int seq)
        {
            int index;
            int cindex;
            int sum = 0, max = 0, min = 0;
            int value;
            for (int ci = 0; ci < cameraCount; ci++)
            {
                index = seq;
                for (int i = 0; i < memSize; i++)
                {
                    cindex = memSize - i - 1;
                    value = AspectRatio[ci,index, cindex];
                    if (i == 0 && ci==0)
                    {
                        min = value;
                        max = value;
                        sum = value;
                    }
                    else
                    {
                        if (min > value) min = value;
                        if (max < value) max = value;
                        sum += value;
                    }
                    index--;
                    if (index < 0) index = memSize - 1;
                }
            }
            if (AspectRatioDataType == DataBindingType.Average) PValue.AspectRatio = memSize > 0 ? (int)(sum / memSize) : 0;
            else if (AspectRatioDataType == DataBindingType.Maximum) PValue.AspectRatio = max;
            else if (AspectRatioDataType == DataBindingType.Minimum) PValue.AspectRatio = min;
            else PValue.AspectRatio = sum;
        }

        public void PokeSize(int seq)
        {
            int index;
            int cindex;
            bool noFruit = false;
            double sum = 0, max = 0, min = 0;
            double value;

            for (int ci = 0; ci < cameraCount; ci++)
            {
                index = seq;
                for (int i = 0; i < memSize; i++)
                {
                    cindex = memSize - i - 1;
                    value = Size[ci,index, cindex];
                    Size[ci, index, cindex] = 0;
                    if(i == 0 && ci==0)
                    {
                        if (value == 0) noFruit = true;
                        min = value;
                        max = value;
                        sum = value;
                    }
                    else
                    {
                        if (min > value) min = value;
                        if (max < value) max = value;
                        sum += value;

                    }
                    index--;
                    if (index < 0) index = memSize - 1;
                }
            }
            if (SizeDataType == DataBindingType.Average) PValue.Size = memSize > 0 ? (int)(sum / memSize) : 0;
            else if (SizeDataType == DataBindingType.Maximum) PValue.Size = (int)max;
            else if (SizeDataType == DataBindingType.Minimum) PValue.Size = (int)min;
            else PValue.Size = (int)sum;

            PValue.noFruit = noFruit;
        }

  
        public ProcessingValue PokeResult(int seq)
        {
            PokeColorRatio(seq);
            PokeAspectRatio(seq);
            PokeSize(seq);
            InitDiagonals(seq);
            return PValue;
        }


        //화상처리 결과 연산값을 임시메모리에 저장한다

        public void InitData()
        {
            for (int ci = 0; ci < cameraCount; ci++)
            {
                for (int i = 0; i < memSize; i++)
                {
                    for (int j = 0; j < memSize; j++)
                    {
                        Color[ci,i,j] = 0.0;
                        Size[ci,i,j] = 0.0;
                        ColorRatio[ci,i,j] = 0;
                        AspectRatio[ci,i,j] = 0;
                        //ColorAccumulated[ci,i,j] = 0;
                        //SizeAccumulated[ci,i,j] = 0;
                    }
                }
            }
        }

        public void peekColor(int camNo,int seq,int car, double colorValue,double sizeValue)
        {
            Color[camNo,seq, car] = colorValue;
            Size[camNo, seq, car] = sizeValue;

            //ColorAccumulated[camNo] += colorValue;
            //SizeAccumulated[camNo] += sizeValue;

            if (sizeValue == 0) ColorRatio[camNo, seq, car] = 0;
            else if (colorValue > sizeValue) ColorRatio[camNo, seq, car] = 1000;
            else ColorRatio[camNo, seq, car] = (int)((1000 * colorValue) / sizeValue);
        }

        public void peekColor(int camNo,int seq, int car, double colorValue)
        {
            Color[camNo, seq, car] = colorValue;
            //ColorAccumulated[camNo] += colorValue;

            double size = Size[camNo, seq, car];
            if (size == 0) ColorRatio[camNo, seq, car] = 0;
            else if (colorValue > size) ColorRatio[camNo, seq, car] = 1000;
            else ColorRatio[camNo, seq, car] = (int)((1000 * colorValue) / size);
        }
        public void peekColorAccumulated(int camNo,int seq, int car, double colorValue)
        {
            Color[camNo, seq, car] += colorValue;
            //ColorAccumulated[camNo] += colorValue;

            double size = Size[camNo, seq, car];
            if (size == 0) ColorRatio[camNo, seq, car] = 0;
            else if (size < Color[camNo, seq, car]) ColorRatio[camNo, seq, car] = 1000;
            else ColorRatio[camNo, seq, car] = (int)((1000*colorValue) / size);
        }
        public void peekSize(int camNo,int seq, int car, double size)
        {
            double color = Color[camNo, seq, car];
            Size[camNo, seq, car] = size;
            //SizeAccumulated[camNo] += size;
            
            /*if (size == 0) ColorRatio[camNo, seq, car] = 0;
            else if (size < color) ColorRatio[camNo, seq, car] = 1000;
            else ColorRatio[camNo, seq, car] = (int)(color / size * 1000);*/
        }
        public void peekSizeAccumulated(int camNo, int seq, int car, double size)
        {
            Size[camNo, seq, car] += size;
            double color = Color[camNo, seq, car];
            //ColorAccumulated[camNo] += colorValue;

            /*if (Size[camNo, seq, car] == 0) ColorRatio[camNo, seq, car] = 0;
            else if (Size[camNo, seq, car] < Color[camNo, seq, car]) ColorRatio[camNo, seq, car] = 1000;
            else ColorRatio[camNo, seq, car] = (int)(color / size * 1000);*/
        }
        public bool peekSizeMaximum(int camNo, int seq, int car, double size)
        {
            bool rts = true;
            double color = Color[camNo, seq, car];
 
            //SizeAccumulated[camNo] += size;
            if (Size[camNo, seq, car] < size) Size[camNo, seq, car] = size;
            else rts = false;

            /*if (size == 0) ColorRatio[camNo, seq, car] = 0;
            else if (size < color) ColorRatio[camNo, seq, car] = 1000;
            else ColorRatio[camNo, seq, car] = (int)(color / size * 1000);*/
            return rts;
        }

        public void peekAspectRatio(int camNo, int seq, int car, double aspect)
        {
            if (aspect == 0) AspectRatio[camNo, seq, car] = 0;
            else if (aspect > 1) AspectRatio[camNo, seq, car] = (int)(1 / aspect * 1000);
            else AspectRatio[camNo, seq, car] = (int)(aspect * 1000);
        }

        public void peekAspectRatio(int camNo, int seq, int car, double width, double height)
        {
            if (width == 0 || height == 0) AspectRatio[camNo, seq, car] = 0;
            else
            {
                if (width > height) AspectRatio[camNo, seq, car] = (int)(1000 * height / width);
                else AspectRatio[camNo, seq, car] = (int)(1000 * width / height);
            }
        }

        public void peekAll(int camNo, int seq, int car, double colorValue, double sizeValue, double aspect)
        {
            //ColorAccumulated[camNo, seq, car] += colorValue;
            //SizeAccumulated[camNo, seq, car] += sizeValue;
            
            if (sizeValue == 0) ColorRatio[camNo, seq, car] = 0;
            else ColorRatio[camNo, seq, car] = (int)(colorValue / sizeValue * 1000);

            Size[camNo, seq, car] = sizeValue;

            if (aspect == 0) AspectRatio[camNo, seq, car] = 0;
            else AspectRatio[camNo, seq, car] = (int)(1 / aspect * 1000);

        }
    }
}
