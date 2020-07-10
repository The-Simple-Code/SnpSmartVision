/*
 *   화상처리 핵심 루틴 
 *   화상처리 목적에 따라 이 루틴의 내용 바꿔 사용할 수 있도록한다.
 *   
 *   아래 Function을 목적에 맞도록 수정하여 사용할것. 
 *   사용하지 않을경우 function전체를 Remark시킬것
 *
 *   void highPriorityThread(string cameraName) 
 *   
 *   Acquisition Class에서 취득한 이미지가 최초 handling되는 루틴
 *
 */



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using WeifenLuo.WinFormsUI.Docking;

using NationalInstruments.Vision;
using NationalInstruments.Vision.WindowsForms;
using NationalInstruments.Vision.Acquisition.Imaqdx;
using NationalInstruments.Vision.Analysis;

using SnpSystem.Vision.Acquisition;
using SnpSystem.Vision.CameraTrigger;
using SnpSystem.Vision.VisionConfigurationHelper;
using SnpSystem.Vision.Etc;
using SnpSystem.Vision.Viewer;

using SnpSystem.Vision.Parameters;

using System.Collections.ObjectModel;


namespace SnpSmartVision
{
    
    //멀티쓰레드중 어느것이 마스터인지 지정
    //마스터는 가장먼저 쓰레드로 모든쓰레드에서 필요한 연산이 끝날때 까지 기다린후
    //결과를 취합해서 Display,Tx동작을 개시하도록 함
    public enum TaskShape
    {
        Master,  //데이타처리 및 전송을 위한 쓰레드를 동작시킬 권한을 갖는다
        Slave,  
    }

    public partial class MdiForm : Form
    {
        #region 리포트에서 제일큰 파티클이있는 인덱스를 찾는다
        int findMaximumSizeInParticleReport(Collection<ParticleReport> report)
        {
            double max=0.0;
            int ix = -1;
            int maxIndex = -1; ;
            foreach (ParticleReport r in report)
            {
                ix++;
                if (r.Area > max)
                {
                    max = r.Area;
                    maxIndex = ix;
                }
            }
            return maxIndex;
        }
        #endregion
        #region 각 Particle의 좌표를 통합한다
        RectangleContour compositeRectangle(RectangleContour r1, RectangleContour r2)
        {
            double ri1,ri2,ri0;
            double bo1,bo2,bo0;
            
            RectangleContour r = new RectangleContour();

            ri1 = r1.Width + r1.Left;
            ri2 = r2.Width + r2.Left;
            
            bo1 = r1.Height + r1.Top;
            bo2 = r2.Height + r2.Top;

            ri0 = ri1 >= ri2 ? ri1 : ri2;
            bo0 = bo1 >= bo2 ? bo1 : bo2;

            r.Left   = r1.Left <= r2.Left ? r1.Left : r2.Left;
            r.Top    = r1.Top <= r2.Top ? r1.Top : r2.Top;
            r.Width  = ri0 - r.Left;
            r.Height = bo0 - r.Top;
                    
            return r;
        }
        #endregion
        #region 하나의 리포트에 기록된 모든 파티클을 통합한다

        //과일의 외형을 감지할때 사용한다
        //파티클이 분리되었다 하더라도 하나의 과일이므로 화면상에 파티클사이 빈 공간도
        //과일영역이라 간주함
        //과일영역은 다음과 같이 정의함
        //Left = min Xi, Top = min Yi, Right = max Xi, Bottom = max Yi
        //Width = Right-Left, Height = Bottom - Top;
        ParticleReport integrateParticleReport(Collection<ParticleReport> ire)
        {
            double r1, r2, b1, b2;
            ParticleReport p = new ParticleReport();
            int count = 0;
            p.Area = 0;
            for (int i = 0; i < ire.Count; i++)
            {

                ParticleReport report= ire[i];
                   if (report.Area < rootFile.minOutlineParticle) continue;
                   if (count == 0)
                   {
                       p = report; count = 1;
                   }
                   else
                   {
                       p.Area += report.Area;
                       b1 = p.BoundingRect.Top + p.BoundingRect.Height;
                       r1 = p.BoundingRect.Left + p.BoundingRect.Width;

                       b2 = report.BoundingRect.Top + report.BoundingRect.Height;
                       r2 = report.BoundingRect.Left + report.BoundingRect.Width;

                       p.BoundingRect.Left = p.BoundingRect.Left <= report.BoundingRect.Left ? p.BoundingRect.Left : report.BoundingRect.Left;
                       p.BoundingRect.Top = p.BoundingRect.Top <= report.BoundingRect.Top ? p.BoundingRect.Top : report.BoundingRect.Top;

                       if (r1 > r2) p.BoundingRect.Width = r1 - p.BoundingRect.Left;
                       else p.BoundingRect.Width = r2 - p.BoundingRect.Left;

                       if (b1 > b2) p.BoundingRect.Height = b1 - p.BoundingRect.Top;
                       else p.BoundingRect.Height = b2 - p.BoundingRect.Top;

                       p.CenterOfMass.X = p.BoundingRect.Left + (p.BoundingRect.Width / 2);
                       p.CenterOfMass.Y = p.BoundingRect.Top + (p.BoundingRect.Height / 2);
                   }
            }
            return p;
        }
        #endregion
        #region 시리얼통신으로 결과를 전송한다(쓰레드)

        /*void ResultSendingRoutine(object result)
        {
            if (rootFile.txProtocol == TxProtocol.Standard) serialOut.SendDatabyStandardProtocol((ProcessingValue[])result);
            else if(rootFile.txProtocol == TxProtocol.Extended) serialOut.SendDatabyExtendedProtocol((ProcessingValue[])result);
        }*/

        void ResultSendingRoutine()
        {
            if (rootFile.txProtocol == TxProtocol.Standard) serialOut.SendDatabyStandardProtocol(yesterData);
            else if (rootFile.txProtocol == TxProtocol.Extended) serialOut.SendDatabyExtendedProtocol(yesterData);

            if (visionTrigger.Trigger == true) controlPort.SendAlarm();
        }
        
        #endregion
        #region 알고리즘 최종처리루틴(결과를전송,디스플레이한다,쓰레드)
        void LastProcessingRoutine(object count)
        {
            //Sequence Count
            int index = (int)count;
            
            //Processing Thread의 수를 파악
            int cameraCount = cameras.Count;            
            List<string>cameraList = new List<string>();

            //Processing Thread로 부터 완료신호가 도달했는지 검사
            //모든 완료신호가 도착할때까지 계속 검사
            bool start;
            for(int i=0;i<cameraCount;i++) cameraList.Add(cameras.ElementAt(i).Key);
            do
            {
                start = true;
                foreach(string cam in cameraList)
                {
                    start &= processingCompletionFlag[cam];
                }
            }
            while(start==false);

            ProcessingValue[] value = new ProcessingValue[2];
            for(int i=0;i<rootFile.Lane;i++)
            {

                   value[i]=Result[i].PokeResult(index);
                   yesterData[i] = value[i];
            }
            try
            {
                this.Invoke(new ResultDisplayDelegate(ResultDataUpdate), new object[] { value });
            }
            catch
            {
            }
        }
        #endregion

        #region 과일외형,색상계측 알고리즘(쓰레드)
        //이 쓰레드는 1대 카메라당 1개의 쓰레드가 생성됨
        //캡쳐된 화면 전체를 대상으로 처리를 수행함.
        //즉, 사용자가 8개의 ROI를 설정했다면, 처리 결과로 부터 각각의 ROI영역에 위치한
        //Prticle을 모아서 각 ROI의 외형,채색영역을 계산함-2017.7.19
        void highPriorityThread(string cameraName)
        {
            #region Trigger Synchronized
            //
            Collection<ParticleReport> reports = new Collection<ParticleReport>();
            if (!acqCompleteFlag.ContainsKey(cameraName)) return;
            
            bool trigger = acqCompleteFlag[cameraName];
            if (trigger == false)
            {
                return;
            }
            acqCompleteFlag[cameraName] = false;
            /*
            if (!flagAcqusition.ContainsKey(cameraName)) return;
            if (flagAcqusition.Count == 0) return;
            bool trigger = flagAcqusition[cameraName];
            if (trigger == false)
            {
                return;
            }
            flagAcqusition[cameraName] = false;
            */
            stopWatch.StartRecord(cameraName);
     
            // 이 쓰레드가 유일한 쓰레드인지, 복수개이면 마스터(첫번째)쓰레드인지 판단
            // Master는 복수의 쓰레드에서 제일먼저 Dictionary에 등록괸 카메라의 쓰레드임
            // Master쓰레드에서 결과를 전송할 권한을 가짐
            // 전송될 결과는 이전 시퀜스에서 얻은 결과임.
            TaskShape taskShape;// = new TaskShape();
            if (cameras.Count == 1) taskShape = TaskShape.Master;
            else
            {
                string camName=cameras.ElementAt(0).Key;
                if (camName == cameraName) taskShape = TaskShape.Master;
                else taskShape = TaskShape.Slave;
            }
            //if (taskShape == TaskShape.Master) ThreadPool.QueueUserWorkItem(ResultSendingRoutine, yesterData);
            
            //아래코드는 리마크
            //if (taskShape == TaskShape.Master) serialOut.SendData(yesterData);
            int cameraIndex=0;
            foreach(string key in cameras.Keys)
            {
                if (key == cameraName) break;
                cameraIndex++;
            }

            int count;
            if (rootFile.Carrier == 0) count = 1;
            else count = FlagDelegation.Count % rootFile.Carrier;
            if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":data sending & preprocessing");
           
            #endregion
            try
            {
                #region Image Capture from Acquisition Buffer
                //취득한 이미지를 임시버퍼에 Copy
                //Algorithms.Copy(cameras[cameraName].acquisitionImage.Image, image);
                       
                //또한 ImageView 버퍼에 저장
                //Algorithms.Copy(image, cameras[cameraName].camForm.Image);
                Algorithms.Copy(cameras[cameraName].acquisitionImage.Image, cameras[cameraName].camForm.Image);
                stopWatch.GetTime(cameraName + ":image Copy1");
                #endregion
            
                #region User Defined ROI Drawing
                //Overlay동작(ROI)를 그린다
                DrwaOverlayRectangle(cameraName);
                if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":user roi drawing");
                #endregion

                #region Processing Initialization
                //옵션대로 원 이미지에서 색상을 추출
                int index=0;
                VisionImage[] image     = processImage32[cameraName];
                VisionImage[] image1    = processImage8[cameraName];
                VisionImage ximage1     = processXImage8[cameraName];
                VisionImage ximage      = processXImage32[cameraName];    
                
                ImageProcessingParameters[] parameters  = parameterFile.ProcessingParameter[cameraName];   
                Dictionary<string,Range> colorRange1    = parameters[index].colorParameter.range;
                Dictionary<string, Range> colorRange2   = parameters[index+1].colorParameter.range;
                Dictionary<string, Range> colorRange3   = parameterFile.ExtraData[cameraName].range; 
                    
                //Extra Image Processing Parameter Reading
                //bool extraEnabled = parameterFile.ExtraData[cameraName].Enable;
               
                ExtraProcessingValidArea combined   = parameterFile.ExtraData[cameraName].CombinedImage;
                ContainedProcessingMode mode        = parameterFile.ExtraData[cameraName].ContainedMode;
                CompositMethod composition          = parameterFile.ExtraData[cameraName].Composit;
                bool[] DisplayMode                  = new bool[3]{displayImage[0],displayImage[1],displayImage[2]};

                bool extraEnabled = composition == CompositMethod.XimageComposit ? true : false;
                if (extraEnabled == false) DisplayMode[2] = false;

                int dMode = DisplayMode[0] ? 1 : 0;
                dMode += DisplayMode[1] ? 2 : 0;
                dMode += DisplayMode[2] ? 4 : 0;

                #endregion
             
                #region Image1 Color Threshod
                // Image1 for Outline
                // 카메라에서 화상을 취득해서 image1[0]에저장하여 outline추출에 사용
                Algorithms.ColorThreshold(
                            cameras[cameraName].camForm.Image,
                            image1[index],
                            parameters[index].colorParameter.colorMode,
                            128,
                            colorRange1["plane1"],
                            colorRange1["plane2"],
                            colorRange1["plane3"]
                            );
                if (stopWatch.enabled == true) stopWatch.GetTime(cameraName+":color threshold1");
                #endregion
             
                #region Image2 Color Threshod
                // Image2 for color extraction 
                // 카메라에서 화상을 취득해서 image1[1]에저장하여 색상추출에 사용
                Algorithms.ColorThreshold(
                            cameras[cameraName].camForm.Image,
                            image1[index + 1],
                            parameters[index + 1].colorParameter.colorMode,
                            128,
                            colorRange2["plane1"],
                            colorRange2["plane2"],
                            colorRange2["plane3"]
                            );
                if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":color threshold2");
                #endregion
             
                #region Composition image1 with image2
                //OUTLINE 추출영상과 색상용 영상을 합해서 OUTLINE으로 사용=>image1[0]
                if (composition == CompositMethod.Image2Composit)
                {
                    Algorithms.Or(image1[index], image1[index + 1], image1[index]);
                }
                if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":Image1,Image2 Composition");
                #endregion

                #region Extra Image Color Threshod & Composition with other Image
                //꼭지추출용 영상 =>  ximage1
                //outline 영상   =>  image1[0]=image1[0]+image1[1]
                //색상영상       =>  image1[1]  
                if (extraEnabled == true)  //composition ==CompositMethod.XImageComposit
                    {
                        //Extra Image for extra processing
                        Algorithms.ColorThreshold(
                                   cameras[cameraName].camForm.Image,
                                   ximage1,
                                   parameterFile.ExtraData[cameraName].colorMode,
                                   128,
                                   colorRange3["plane1"],
                                   colorRange3["plane2"],
                                   colorRange3["plane3"]
                                   );
                       

                        #region Lowpass Filter
                        Algorithms.LowPass(ximage1,ximage1,new LowPassOptions(5,5));
                        if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":ximage1 lowpass filter");
                        #endregion
                        #region Morphology
                        if (rootFile.ximageEnhance == true)
                        {
                            //Algorithms.GrayMorphology(ximage1, ximage1, MorphologyMethod.Close);
                            Algorithms.GrayMorphology(ximage1, ximage1, MorphologyMethod.Close);
                            if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":ximage1 Morphology");
                        }
                        #endregion
                        #region Remove Particle
                        Algorithms.RemoveParticle(
                                       ximage1,
                                       ximage1,
                                       5,
                                       SizeToKeep.KeepSmall,
                                       Connectivity.Connectivity4
                                       );
                        if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":particle removing1");

                        Algorithms.Cast(ximage1, ximage, ImageType.Rgb32);
                        if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":color threshold3");

                        #endregion
                        #region 추출된 꼭지를 처리
                        //꼭지를 OUTLINE영역에 대하여 처리
                        if (combined == ExtraProcessingValidArea.Process1)
                        {
                            if (mode == ContainedProcessingMode.Include)Algorithms.Or(ximage1, image1[index], image1[index]);
                            else                                        Algorithms.Xor(ximage1, image1[index], image1[index]);
                        }
                        //꼭지를 색상영역에 대하여 처리
                        else if (combined == ExtraProcessingValidArea.Process2) 
                        {
                            if (mode == ContainedProcessingMode.Include)Algorithms.Or(ximage1, image1[index+1], image1[index+1]);
                            else                                        Algorithms.Xor(ximage1, image1[index+1], image1[index+1]);
                        }
                        //꼭지를 OUTLINE과 색상영역에 처리
                        else
                        {
                            if (mode == ContainedProcessingMode.Include)
                            {                               
                                Algorithms.Or(ximage1, image1[index], image1[index]);
                                Algorithms.Or(ximage1, image1[index + 1], image1[index + 1]);
                            }
                            else
                            {
                                Algorithms.Xor(ximage1, image1[index], image1[index]);
                                Algorithms.Xor(ximage1, image1[index + 1], image1[index + 1]);
                            }
                        }
                       
                        if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":X image Composition");
                        #endregion
                    }
                #endregion
                
                #region Image Processing of Image1(과일의 외형 형상을 처리한다)

                #region Morphology
                if (rootFile.image1Enhance == true)
                {
                    Algorithms.GrayMorphology(image1[index], image1[index], MorphologyMethod.Close);
                    if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":image1 Morphology");
                }
                #endregion

                #region Median Filter
                Algorithms.MedianFilter(
                               image1[index],
                               image1[index],
                               parameters[index].medianFilterParameter.width,
                               parameters[index].medianFilterParameter.height
                               );
                if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":median filter1");
                #endregion
                
                #region Remove Particle(옵션)
                if (rootFile.outlineParticleRemover == true)
                {
                    Algorithms.RemoveParticle(
                                    image1[index],
                                    image1[index],
                                    parameters[index].removeParticleParameter.erosion,
                                    parameters[index].removeParticleParameter.sizeToKeep,
                                    parameters[index].removeParticleParameter.connectivity
                                    );
                    if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":particle removing1");
                }
                #endregion
               
                #region 디스플레이용 색상처리 영상을 저장
                //화면에 표시할 이미지를 대피시킨다.
                //Particle Filter에서 나온 이미지는 검정색으로 아무 이미지도 보이지않음(이유는 모름)
                Algorithms.Cast(image1[index], image[index], ImageType.Rgb32);
                if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":image cast1");
                #endregion

                #region Particle Filter
                Algorithms.ParticleFilter(
                                    image1[index],
                                    image1[index],
                                    parameters[index].particleFilterParameters.criteria.Criteria,
                                    parameters[index].particleFilterParameters.option.GetParticleFilterOptions()
                                    );

                 if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":particle filter1");
                 #endregion
                
                #region Accumulate Particle for outline
                 //줄기제거를 하지 않을경우 외형처리결과는 여기에서 완료)
                 cameras[cameraName].DisplayData.Clear();
                 #region 줄기처리 안하는 경우
                 if (rootFile.StemRemoving == false)
                 {
                     #region Particle Report
                     reports = Algorithms.ParticleReport(
                                            image1[index],
                                            parameters[index].particleReportParameters.connectivity
                                        );
                     if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":particle report1");
                     #endregion
                     foreach (ParticleReport pr in reports)
                     {
                         OwnerOfRoi oRoi = roiData.GetRoiOwner(cameraName, pr.CenterOfMass);
                         if (oRoi.Lane == -1) continue;
                         Result[oRoi.Lane].peekSize(cameraIndex, count, oRoi.Sequence, pr.Area);
                         Result[oRoi.Lane].peekAspectRatio(cameraIndex, count, oRoi.Sequence, pr.BoundingRect.Width, pr.BoundingRect.Height);
                         cameras[cameraName].DisplayData.RContour[oRoi.Lane,oRoi.Sequence] = pr.BoundingRect;
                         cameras[cameraName].DisplayData.PContour[oRoi.Lane, oRoi.Sequence] = pr.CenterOfMass;
                         cameras[cameraName].DisplayData.Size[oRoi.Lane, oRoi.Sequence] = pr.Area;
                     }
                 }
                 #endregion
                 #region 줄기처리 하는경우
                 else
                 {
                    Collection<ParticleReport> ire = new Collection<ParticleReport>();
                    using (VisionImage imx = new VisionImage(ImageType.U8))
                    { 
                        #region Outline 처리1.Maximum Size를 가지고 Outline을 처리=OutlineType.One
                        if(rootFile.outlineMethod==OutlineMethodType.One)
                        {
                             //int iter = 0;
                             #region Particle Report를 생성한다

                            //Image전체 영역에 대해 파티클 리포트를 만든다.
                             reports = Algorithms.ParticleReport(
                                                    image1[index],
                                                    parameters[index].particleReportParameters.connectivity
                                                );
                             if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":particle report1");
                             #endregion
                             #region  Particle Report를 검사한다
                             //개별리포트를 조사 
                            foreach (ParticleReport report in reports)
                             {
                                     //?RectangleContour rect = modifyRectangle(report);
                                     //?Algorithms.Extract(image1[index], imx, rect);
                                     //?ire = Algorithms.ParticleReport(imx, parameters[index].particleReportParameters.connectivity);
                                     //notice:
                                     //리포트는 collection type이라 복수의 리포트가 생성될 수 있으나
                                     //이 단계에서는 처리물체의 외형을 상대하므로 하나라고 간주
                                     //만약 여러개라면 이전단계에서 Color Threshold값을 조정해야함.
                                     //if (ire.Count == 0) continue;
                                     //?iter = findMaximumSizeInParticleReport(ire);
                                     //?if (iter < 0) continue;
                                     //if (ire[iter].Area < rootFile.MinimumSize) continue;

                                     if (report.Area < rootFile.MinimumSize) continue;

                                     /*
                                     ParticleReport pr = ire[iter];
                                     pr.BoundingRect.Left += rect.Left;
                                     pr.BoundingRect.Top += rect.Top;
                                     pr.CenterOfMass.X += rect.Left;
                                     pr.CenterOfMass.Y += rect.Top;
                                     //re.Add(pr);
                                     OwnerOfRoi oRoi = roiData.GetRoiOwner(cameraName, pr.CenterOfMass);
                                     */
                                     OwnerOfRoi oRoi = roiData.GetRoiOwner(cameraName, report.CenterOfMass);

                                     if (oRoi.Lane == -1) continue;
                                     //Result[oRoi.Lane].peekSize(count,oRoi.Sequence,pr.Area);
                                     if (Result[oRoi.Lane].peekSizeMaximum(cameraIndex, count, oRoi.Sequence, report.Area))
                                     {
                                         Result[oRoi.Lane].peekAspectRatio(cameraIndex, count, oRoi.Sequence, report.BoundingRect.Width, report.BoundingRect.Height);
                                         cameras[cameraName].DisplayData.RContour[oRoi.Lane, oRoi.Sequence] = report.BoundingRect;
                                         cameras[cameraName].DisplayData.PContour[oRoi.Lane, oRoi.Sequence] = report.CenterOfMass;
                                         cameras[cameraName].DisplayData.Size[oRoi.Lane, oRoi.Sequence] = report.Area;
                                     }
                              }//foreach의 끝
                             #endregion
                        }//if문의 끝
                             
                        #endregion
                        #region Outline 처리2.모든 파티클을 Outline처리에 사용=OutlineType.All
                        else if (rootFile.outlineMethod == OutlineMethodType.All)
                        {
                            /*
                             * 1. reports는 화면 전 영역에서 파티클을 찾아냄
                             * 2. 전체영역에서 n개의 파티클을 찾아낸다.
                             * 3. 각각의 파티클 대상으로, 어느 ROI에 속하는지 검사 
                             * 4. 같은 ROI에 대해서는 크기를 합산,영역은 합성한다.
                             */
                            #region 임시메모리 선언및 초기화
                            RectangleContour[,] rc = new RectangleContour[2, 10];
                            PointContour[,] pc = new PointContour[2, 10];
                            double[,] size = new double[2, 10] { 
                                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                                };

                            int i = 0;

                            for (int j = 0; j < rootFile.Lane; j++)
                            {
                                for (int k = 0; k < rootFile.Carrier; k++)
                                {
                                    rc[j, k] = new RectangleContour();
                                    pc[j, k] = new PointContour();
                                }
                            }
                            #endregion
                            #region Particle Report
                            reports = Algorithms.ParticleReport(
                                                   image1[index],
                                                   parameters[index].particleReportParameters.connectivity
                                               );
                            if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":particle report1");
                            #endregion
                            #region ROI영역안의 모든 파티클을 모은다
                            foreach (ParticleReport report in reports)
                            {
                                //RectangleContour rect = modifyRectangle(report);
                                //Algorithms.Extract(image1[index], imx, rect);        //2017.7.20

                                //아래함수는 불필요함(이중으로 파티클 리포트를 만들었음)  //2017.7.30
                                //ire = Algorithms.ParticleReport(imx, parameters[index].particleReportParameters.connectivity);
                                //if (ire.Count == 0) continue;
                                //ParticleReport pr = integrateParticleReport(ire);
                                /*if (pr.Area < rootFile.MinimumSize) continue;
                                  
                                pr.BoundingRect.Left += rect.Left;
                                pr.BoundingRect.Top += rect.Top;
                                pr.CenterOfMass.X += rect.Left;
                                pr.CenterOfMass.Y += rect.Top;*/
                                if (report.Area < rootFile.MinimumSize) continue;
                                OwnerOfRoi oRoi = roiData.GetRoiOwner(cameraName, report.CenterOfMass);
                                if (oRoi.Lane == -1) continue;
                                if (size[oRoi.Lane, oRoi.Sequence] == 0)
                                {
                                    size[oRoi.Lane, oRoi.Sequence] = report.Area;
                                    rc[oRoi.Lane, oRoi.Sequence].Left = report.BoundingRect.Left;
                                    rc[oRoi.Lane, oRoi.Sequence].Top = report.BoundingRect.Top;
                                    rc[oRoi.Lane, oRoi.Sequence].Width = report.BoundingRect.Width;
                                    rc[oRoi.Lane, oRoi.Sequence].Height = report.BoundingRect.Height;
                                }
                                else
                                {
                                    size[oRoi.Lane, oRoi.Sequence] += report.Area;
                                    rc[oRoi.Lane, oRoi.Sequence] = RectOperator.Add(rc[oRoi.Lane, oRoi.Sequence], report.BoundingRect);
                                }
                                /*
                                Result[oRoi.Lane].peekSize(cameraIndex, count, oRoi.Sequence, pr.Area);
                                Result[oRoi.Lane].peekAspectRatio(cameraIndex, count, oRoi.Sequence, pr.BoundingRect.Width, pr.BoundingRect.Height);
                                cameras[cameraName].DisplayData.RContour[oRoi.Sequence] = pr.BoundingRect;
                                cameras[cameraName].DisplayData.PContour[oRoi.Sequence] = pr.CenterOfMass;
                                cameras[cameraName].DisplayData.Size[oRoi.Sequence] = pr.Area;*/
                            }
                            #endregion
                            #region 처리결과를 전송,디스플레이 하기위해 버퍼에 저장
                            for (int j = 0; j < rootFile.Lane; j++)
                            {
                                for (int k = 0; k < rootFile.Carrier; k++)
                                {
                                    if (size[j, k] == 0) continue;
                                    Result[j].peekSize(cameraIndex, count, k, size[j, k]);
                                    Result[j].peekAspectRatio(cameraIndex, count, k, rc[j, k].Width, rc[j, k].Height);
                                    cameras[cameraName].DisplayData.Size[j, k] = size[j, k];
                                    cameras[cameraName].DisplayData.RContour[j, k] = new RectangleContour(
                                                                                                            rc[j, k].Left,
                                                                                                            rc[j, k].Top,
                                                                                                            rc[j, k].Width,
                                                                                                            rc[j, k].Height
                                                                                                            );
                                    cameras[cameraName].DisplayData.PContour[j, k] = new PointContour(

                                                                                                       rc[j, k].Left + (rc[j, k].Width / 2),
                                                                                                       rc[j, k].Top + (rc[j, k].Height / 2)

                                                                                                             );
                                }
                            }
                            #endregion
                        }
                        #endregion
                        #region Outline 처리3.USER ROI영역을 미리 짤라내고 각 영역에서 최대 파티클을 가지고 외형을 추출=OutlineType.MaxBySeperated
                        else if(rootFile.outlineMethod==OutlineMethodType.MaxBySeperated)
                        {
                           for (int i = 0; i < rootFile.Lane; i++)
                            {
                                for (int j = 0; j < rootFile.Carrier; j++)
                                {
                                    #region User ROI하나를 불러서 그 영역만큼 이미지를 추출
                                    //저장된 ROI를 하나 불러온다
                                    RoiData roi = roiData.getPointedRoi(cameraName, i+1, j);
                                    if (roi == null) continue;
                                    RectangleContour rect = new RectangleContour(
                                                                                roi.Left,
                                                                                roi.Top,
                                                                                roi.Width,
                                                                                roi.Height);
                                    //ROI만큼 이미지를 잘라낸다
                                    Algorithms.Extract(image1[index], imx, rect);
                                    #endregion
                                    #region 자른 영역에 대한 Particle Report를 생성하고, 가장큰 Particle을 찾는다
                                    ire = Algorithms.ParticleReport(imx, parameters[index].particleReportParameters.connectivity);
                                    if (ire.Count <= 0) continue;
                                    int iter = findMaximumSizeInParticleReport(ire);
                                    if (iter < 0) continue;
                                    if (ire[iter].Area < rootFile.MinimumSize) continue;
                                    #endregion
                                    #region 가장큰 파티클의 좌표와 가로,세로영역을
                                    ParticleReport pr = ire[iter];
                                    pr.BoundingRect.Left += rect.Left;
                                    pr.BoundingRect.Top += rect.Top;
                                    pr.CenterOfMass.X += rect.Left;
                                    pr.CenterOfMass.Y += rect.Top;
                                    #endregion
                                    #region 결과전송,디스플레이를 위해 버퍼에 결과저장
                                    Result[i].peekSize(cameraIndex, count, j, pr.Area);
                                    Result[i].peekAspectRatio(cameraIndex, count, j, pr.BoundingRect.Width, pr.BoundingRect.Height);
                                    cameras[cameraName].DisplayData.RContour[i, j] = pr.BoundingRect;
                                    cameras[cameraName].DisplayData.PContour[i, j] = pr.CenterOfMass;
                                    cameras[cameraName].DisplayData.Size[i, j] = pr.Area;
                                    #endregion
                                }
                            }
                        }
                        #endregion
                        #region Outline 처리4.USER ROI영역을 미리 짤라내고 각 영역에서 모든파티클을 합산하여 외형을 추출=OutlineType.AllBySeperated
                        else if (rootFile.outlineMethod == OutlineMethodType.AllBySeperated)
                        {
                            #region 임시메모리 선언및 초기화

                            //최대 2Lane,10개의 캐리어 까지 처리할 수 있음
                            //영역내 모든 파티클은 크기와 영역이 합산됨
                            //합산된 영역을 저장할 수 있는 버퍼를 정의함
                            RectangleContour[,] rc = new RectangleContour[2, 10];
                            PointContour[,] pc = new PointContour[2, 10];
                            double[,] size = new double[2, 10] { 
                                                                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                                                               };
                            #endregion
                            for (int i = 0; i < rootFile.Lane; i++)
                            {
                                for (int j = 0; j < rootFile.Carrier; j++)
                                {
                                    rc[i, j] = new RectangleContour();
                                    pc[i, j] = new PointContour();

                                    #region User ROI를 불러서 그 크기만큼 이미지를 추출
                                    //저장된 ROI를 하나 불러온다
                                    RoiData roi = roiData.getPointedRoi(cameraName, i + 1, j);
                                    if (roi == null) continue;
                                    RectangleContour rect = new RectangleContour(
                                                                                roi.Left,
                                                                                roi.Top,
                                                                                roi.Width,
                                                                                roi.Height);
                                    //ROI만큼 이미지를 잘라낸다
                                    Algorithms.Extract(image1[index], imx, rect);
                                    #endregion
                                    #region Particle Report생성
                                    ire = Algorithms.ParticleReport(imx, parameters[index].particleReportParameters.connectivity);
                                    if (ire.Count <= 0) continue;
                                    int x = 0;
                                    #endregion
                                    #region 모든 파티클을 합한다(크기+영역)
                                    foreach (ParticleReport report in ire)
                                    {
                                        if (report.Area < rootFile.MinimumSize) continue;
                                        if (x == 0)
                                        {
                                            size[i, j] = report.Area;
                                            rc[i, j].Left = report.BoundingRect.Left+roi.Left;
                                            rc[i, j].Top = report.BoundingRect.Top+roi.Top;
                                            rc[i, j].Width = report.BoundingRect.Width;
                                            rc[i, j].Height = report.BoundingRect.Height;
                                            x++;
                                        }
                                        else
                                        {
                                            size[i, j] += report.Area;
                                            RectangleContour ct = new RectangleContour(
                                                                            report.BoundingRect.Left+roi.Left,
                                                                            report.BoundingRect.Top+roi.Top,
                                                                            report.BoundingRect.Width,
                                                                            report.BoundingRect.Height
                                                                            );
                                            rc[i, j] = RectOperator.Add(rc[i, j], ct);
                                        }

                                    }
                                    #endregion
                                    #region 결과를 전송,디스플레이 하기위해 버퍼에 저장
                                    if (size[i, j] == 0) continue;
                                    Result[i].peekSize(cameraIndex, count, j, size[i, j]);
                                    Result[i].peekAspectRatio(cameraIndex, count, j, rc[i, j].Width, rc[i, j].Height);
                                    cameras[cameraName].DisplayData.Size[i, j] = size[i, j];
                                    cameras[cameraName].DisplayData.RContour[i, j] = new RectangleContour(
                                                                                                            rc[i, j].Left,
                                                                                                            rc[i, j].Top,
                                                                                                            rc[i, j].Width,
                                                                                                            rc[i, j].Height
                                                                                                            );
                                    cameras[cameraName].DisplayData.PContour[i, j] = new PointContour(

                                                                                                       rc[i, j].Left + (rc[i,j].Width / 2),
                                                                                                       rc[i, j].Top + (rc[i, j].Height / 2)

                                                                                                             );


                                    #endregion
                                }
                            }
                        }
                        #endregion
                    }  //using문의 끝
                     if(stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":stem removing");
                  }//else문의 끝
                 #endregion
                 #endregion
                #endregion

                #region Image Processing of Image2(과일의 색상을 처리한다)
                #region Median Filter
                 Algorithms.MedianFilter(
                                image1[index+1],
                                image1[index+1],
                                parameters[index+1].medianFilterParameter.width,
                                parameters[index+1].medianFilterParameter.height
                                );
                if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":median filter2");
                #endregion
                #region Remove Particle(옵션)
                if (rootFile.particleRemover == true)
                {
                    Algorithms.RemoveParticle(
                                    image1[index + 1],
                                    image1[index + 1],
                                    parameters[index + 1].removeParticleParameter.erosion,
                                    parameters[index + 1].removeParticleParameter.sizeToKeep,
                                    parameters[index + 1].removeParticleParameter.connectivity
                                    );
                    if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":particle removing2");
                }
                #endregion
                #region Morphology(옵션)
                if (rootFile.image2Enhance == true)
                {
                    Algorithms.GrayMorphology(image1[index + 1], image1[index + 1], MorphologyMethod.Close);
                    if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":image2 Morphology");
                }
                #endregion
                //Algorithms.LowPass(image1[index + 1], image1[index + 1], new LowPassOptions(7, 7));
                #region 디스플레이용 색상처리 영상을 저장
                Algorithms.Cast(image1[index + 1], image[index + 1], ImageType.Rgb32);
                if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":image cast2");
                #endregion
                #region Particle Filter(옵션)
                
                if (rootFile.particleFilter == true)
                {
                    Algorithms.ParticleFilter(
                                        image1[index + 1],
                                        image1[index + 1],
                                        parameters[index + 1].particleFilterParameters.criteria.Criteria,
                                        parameters[index + 1].particleFilterParameters.option.GetParticleFilterOptions()
                                        );

                    if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":particle filter2");
                }
                #endregion
                #region ROI 영역안에있는 파티클을 카운트 
                //outline을 추출한 결과,새로운 ROI에 대한 Image를 추출
                //추출한 이미지의 파티클리포트를 만들고
                //리포트의 파티클정보를 반영함
                using (VisionImage imx = new VisionImage(ImageType.U8))
                {
                    for (int i = 0; i < rootFile.Lane; i++)
                    {
                        for (int j = 0; j < rootFile.Carrier; j++)
                        {
                            //RoiData roi = roiData.getPointedRoi(cameraName, i + 1, j);
                            //if (roi == null) continue;

                            RectangleContour rx = new RectangleContour();
                            rx=cameras[cameraName].DisplayData.RContour[i, j];
                            if (rx == null) continue;
                            
                            /*
                            RectangleContour rect = new RectangleContour(
                                                                        roi.Left,
                                                                        roi.Top,
                                                                        roi.Width,
                                                                        roi.Height);
                            
                             */

                            RectangleContour rect = new RectangleContour(
                                                                        rx.Left,
                                                                        rx.Top,
                                                                        rx.Width,
                                                                        rx.Height);
                            
                            //ROI만큼 이미지를 잘라낸다
                            Algorithms.Extract(image1[index+1], imx, rect);

                            #region Particle Report
                            reports = Algorithms.ParticleReport(
                                                          imx,
                                                          parameters[index + 1].particleReportParameters.connectivity
                                                         );

                            #endregion
                            #region Accumulate Particle
                            foreach (ParticleReport r in reports)
                            {
                                //OwnerOfRoi oRoi = roiData.GetRoiOwner(cameraName, r.CenterOfMass);
                                //if (oRoi.Lane == -1) continue;
                                Result[i].peekColorAccumulated(cameraIndex, count, j, r.Area);
                                cameras[cameraName].DisplayData.Color[i, j] += r.Area;
                            }
                            if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":particle report2");
                            #endregion
                           
                        }
                    }
                }
                #endregion
                 #endregion

                //if(DebuggerForm.IsLoaded) DebuggerForm.DisplayProcessingResult(Result[0].PokeSize(0, count));

                #region Debugger Process
                if (DebuggerForm != null)
                {
                    if (DebuggerForm.IsLoaded && DebuggerForm.Pause == false && DebuggerForm.IsCameraSelected(cameraName))
                    {
                        for (int i = 0; i < rootFile.Lane; i++)
                        {
                            if (DebuggerForm.FSize)
                            {
                                string[] s = new string[] { cameraName, count.ToString(), Result[i].PokeSize(cameraIndex, count) };
                                this.Invoke(new DebuggerDisplayDelegate(DebuggerDataUpdate), new object[] { s });
                            }
                            if (DebuggerForm.FColor)
                            {
                                string[] s = new string[] { cameraName, count.ToString(), Result[i].PokeColor(cameraIndex, count) };
                                this.Invoke(new DebuggerDisplayDelegate(DebuggerDataUpdate), new object[] { s });
                            }
                            if (DebuggerForm.FColorRatio)
                            {
                                string[] s = new string[] { cameraName, count.ToString(), Result[i].PokeColorRatio(cameraIndex, count) };
                                this.Invoke(new DebuggerDisplayDelegate(DebuggerDataUpdate), new object[] { s });
                            }
                            if (DebuggerForm.FAspect)
                            {
                                string[] s = new string[] { cameraName, count.ToString(), Result[i].PokeAspectRatio(cameraIndex, count) };
                                this.Invoke(new DebuggerDisplayDelegate(DebuggerDataUpdate), new object[] { s });
                            }
                        }

                        if (DebuggerForm.OutPacket)
                        {
                            string[] s = new string[] { cameraName, count.ToString(), "Packet Debugger" };
                            this.Invoke(new DebuggerDisplayDelegate(DebuggerTxFrame), new object[] { s });
                        }
/*
                        if (DebuggerForm.FResult)
                        {
                            string[] s = new string[] { cameraName, count.ToString(), "Record Result" };
                            this.Invoke(new DebuggerDisplayDelegate(DebuggerTxFrame), new object[] { s });
                        }*/
                    }
                }

                #endregion

                #region 화상처리가 완료되었음을 이후 생성될 쓰레드에게 알리고 디스플레이용 쓰레드를 생성
                
                lock (this)
                {
                    if (processingCompletionFlag.ContainsKey(cameraName)) 
                                    processingCompletionFlag[cameraName] = true;
                }
                // 쓰레드풀 방식으로 쓰레드를 생성한다
                // 생성된 쓰레드는 처리결과를 디스플레이하는데 사용됨
                if (taskShape == TaskShape.Master)
                {
                    //이전시퀜스에서 생성한 선별데이타를 전송
                    ResultSendingRoutine();
                    //현재시퀜스에서 수집한 데이타로 선별데이타를 만든다.
                    ThreadPool.QueueUserWorkItem(LastProcessingRoutine, count);
                }
                #endregion

                #region Image Display
                //추출된 이미지에 설정한 색을입혀 화면에 표시

                switch (dMode)
                {
                    case 1: //display only image1
                            Algorithms.And(
                                        image[index],
                                        new PixelValue(new Rgb32Value(parameters[index].displayColorParameters.DisplayColor)),
                                        cameras[cameraName].resultForm.Viewer.Image
                                      );
                                    break;
                    case 2: //display only image2
                            Algorithms.And(
                                        image[index+1],
                                        new PixelValue(new Rgb32Value(parameters[index+1].displayColorParameters.DisplayColor)),
                                        cameras[cameraName].resultForm.Viewer.Image
                                      );
                                    break;
                    case 3: //display image1 + image2
                            Algorithms.And(
                                           image[index],
                                           new PixelValue(new Rgb32Value(parameters[index].displayColorParameters.DisplayColor)),
                                           image[index]
                                         );
                            Algorithms.And(
                                           image[index+1],
                                           new PixelValue(new Rgb32Value(parameters[index+1].displayColorParameters.DisplayColor)),
                                           image[index+1]
                                         );
                            Algorithms.Or(image[index], image[index + 1], cameras[cameraName].resultForm.Viewer.Image);
                                    break;
                    case 4: //display only ximage
                            Algorithms.And(
                                        ximage,
                                        new PixelValue(new Rgb32Value(parameterFile.ExtraData[cameraName].DisplayColor.DisplayColor)),
                                        cameras[cameraName].resultForm.Viewer.Image
                                       );
                                    break;
                    case 5: //display image1 + ximage
                            Algorithms.And(
                                           image[index],
                                           new PixelValue(new Rgb32Value(parameters[index].displayColorParameters.DisplayColor)),
                                           image[index]
                                         );
                            Algorithms.And(
                                           ximage,
                                           new PixelValue(new Rgb32Value(parameterFile.ExtraData[cameraName].DisplayColor.DisplayColor)),
                                           ximage
                                         );
                            Algorithms.Or(image[index], ximage, cameras[cameraName].resultForm.Viewer.Image);
                                    break;
                    case 6: //display image2+ximage
                            Algorithms.And(
                                           image[index+1],
                                           new PixelValue(new Rgb32Value(parameters[index+1].displayColorParameters.DisplayColor)),
                                           image[index+1]
                                         );
                            Algorithms.And(
                                           ximage,
                                           new PixelValue(new Rgb32Value(parameterFile.ExtraData[cameraName].DisplayColor.DisplayColor)),
                                           ximage
                                         );
                            Algorithms.Or(image[index+1], ximage, cameras[cameraName].resultForm.Viewer.Image);
                                    break;
                    case 7: //display all
                            Algorithms.And(
                                           image[index],
                                           new PixelValue(new Rgb32Value(parameters[index].displayColorParameters.DisplayColor)),
                                           image[index]
                                         );
                            Algorithms.And(
                                           image[index + 1],
                                           new PixelValue(new Rgb32Value(parameters[index+1].displayColorParameters.DisplayColor)),
                                           image[index + 1]
                                         );
                            Algorithms.And(
                                           ximage,
                                           new PixelValue(new Rgb32Value(parameterFile.ExtraData[cameraName].DisplayColor.DisplayColor)),
                                           ximage
                                         );

                            Algorithms.Or(image[index], image[index + 1], image[index]);
                            Algorithms.Or(image[index], ximage, cameras[cameraName].resultForm.Viewer.Image);
                                    break;
                    default: //display only image1 for default
                            Algorithms.And(
                                        image[index],
                                        new PixelValue(new Rgb32Value(parameters[index].displayColorParameters.DisplayColor)),
                                        cameras[cameraName].resultForm.Viewer.Image
                                      );
                                    break;
                }
                if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":display image composition");
                #endregion

                #region User Defined ROI Drawing
                //Overlay동작(ROI)를 그린다
                DrwaOverlayRectangle(cameraName,cameras[cameraName].resultForm);
                if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":user roi drawing");
                #endregion

                #region Dynamic ROI DRAWING & Stop stop watch
                     //DrawDynamicOverlayRectangle(index, cameraName);
                     DrawDynamicOverlayRectangle(cameraName);
                     if (stopWatch.enabled == true) stopWatch.GetTime(cameraName + ":drawing dynamic roi");
                     if (stopWatch.enabled == true)
                     {
                         stopWatch.StopRecord();
                         showProcessingTimeConsumption();
                     }
                 #endregion

            } //try의 끝

            catch (VisionException ex)
            {
                string str = ex.Message;
            }
        }
        //System.Threading.Thread.Sleep(0);
        #endregion 변산농협 양파외형 계측 알고리즘의 끝
    }
    
}
