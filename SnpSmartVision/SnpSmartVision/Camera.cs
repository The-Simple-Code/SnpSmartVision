using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.ObjectModel;

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

namespace SnpSmartVision
{
    class ImageProcessingSpec
    {
        int imageCount;
        Dictionary<int,Dictionary<string, Range>> colorSpace;
        Dictionary<int,ParticleMeasurementsReport> particleMeasurementReports;
        Dictionary<int,System.Collections.ObjectModel.Collection<MeasurementType>> particleMeasurementType;
        Dictionary<int,Collection<ParticleReport>> particleReport;

        public ImageProcessingSpec(int imageCount)
        {
            this.imageCount = imageCount;
            colorSpace = new Dictionary<int, Dictionary<string, Range>>();
            particleMeasurementReports = new Dictionary<int, ParticleMeasurementsReport>();
            particleMeasurementType = new Dictionary<int, System.Collections.ObjectModel.Collection<MeasurementType>>();
            particleReport = new Dictionary<int, Collection<ParticleReport>>();
        }

        public Collection<ParticleReport> GetParticleReport(int imageNumber)
        {
            if (particleReport.ContainsKey(imageNumber) == true)  return particleReport[imageNumber];
            else return null;
        }

        public void SetParticleReport(int imageNumber, Collection<ParticleReport> report)
        {
            if (particleReport.ContainsKey(imageNumber) == true) particleReport.Remove(imageNumber);
            particleReport.Add(imageNumber, report);
        }

        public void SetParticleMeasurementReport(int imageNumber, ParticleMeasurementsReport report)
        {
            if (particleMeasurementReports.ContainsKey(imageNumber) == true) particleMeasurementReports.Remove(imageNumber);
            particleMeasurementReports.Add(imageNumber, report);
        }

        public ParticleMeasurementsReport GetParticleMeasurementReport(int imageNumber)
        {
            if (particleMeasurementReports.ContainsKey(imageNumber) == true) return particleMeasurementReports[imageNumber];
            else return null;
        }

        public void SetParticleMeasurementType(int imageNumber, System.Collections.ObjectModel.Collection<MeasurementType> type)
        {
            if (particleMeasurementType.ContainsKey(imageNumber) == true) particleMeasurementType.Remove(imageNumber);
            particleMeasurementType.Add(imageNumber, type);
        }

        public void SetParticleMeasurementDefaultType(int imageNumber)
        {
            if (particleMeasurementType.ContainsKey(imageNumber) == true) particleMeasurementType.Remove(imageNumber);
            System.Collections.ObjectModel.Collection<MeasurementType> type = new System.Collections.ObjectModel.Collection<MeasurementType>();
            type.Add(MeasurementType.BoundingRectLeft);
            type.Add(MeasurementType.BoundingRectTop);
            type.Add(MeasurementType.BoundingRectWidth);
            type.Add(MeasurementType.BoundingRectHeight);
            type.Add(MeasurementType.CenterOfMassX);
            type.Add(MeasurementType.CenterOfMassY);
            type.Add(MeasurementType.ParticleAndHolesArea);
            particleMeasurementType.Add(imageNumber, type);
        }

        public System.Collections.ObjectModel.Collection<MeasurementType> GetParticleMeasurementType(int imageNumber)
        {
            if (particleMeasurementType.ContainsKey(imageNumber) == true) return particleMeasurementType[imageNumber];
            else
            {
                System.Collections.ObjectModel.Collection<MeasurementType> type = new System.Collections.ObjectModel.Collection<MeasurementType>();
                type.Add(MeasurementType.BoundingRectLeft);
                type.Add(MeasurementType.BoundingRectTop);
                type.Add(MeasurementType.BoundingRectWidth);
                type.Add(MeasurementType.BoundingRectHeight);
                type.Add(MeasurementType.CenterOfMassX);
                type.Add(MeasurementType.CenterOfMassY);
                type.Add(MeasurementType.ParticleAndHolesArea);
                return type;
            }
        }
        
        public void SetColorSpace(int imageNumber, Range plane1, Range plane2, Range plane3)
        {
            if(colorSpace.ContainsKey(imageNumber)==true) colorSpace.Remove(imageNumber);
            Dictionary<string, Range> range = new Dictionary<string, Range>();
            range.Add("plane1", plane1);
            range.Add("plane2", plane2);
            range.Add("plane3", plane3);
            colorSpace.Add(imageNumber,range);
        }

        public Dictionary<string, Range> GetColorSpace(int imageNumber)
        {
            if (colorSpace.ContainsKey(imageNumber) == true) return colorSpace[imageNumber];
            else
            {
                Range range = new Range(0, 255);
                Dictionary<string, Range> dRange = new Dictionary<string, Range>();
                dRange.Add("plane1", range);
                dRange.Add("plane2", range);
                dRange.Add("plane3", range);
                return dRange;
            }
        }
    }
    
    class Camera : IDisposable
    {
        string  cameraName;
        int     imageCount;
        WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        static int MAXIMUM_LANE_COUNT = 2;
        static int MAXIMUM_SEQUENCE_COUNT = 12;
        public CamForm          camForm;
        public CamForm          resultForm;
        public CamAttrTreeView  attrForm;
        public Acquisition      acquisitionImage;

        //오리지날 이미지를 저장할 버퍼의 집합
        public VisionImage      viewerImage;

        //화상처리결과를 저장할 버퍼의 집합
        public VisionImage      resultImage;
        
        //Extra Image Buffer
        //public VisionImage      xtraImage;

        MainThread.ParentThreadFunction pFunction;

        public MainThread               mainThread;

        public ImageProcessingSpec      imageSpec;

        public ImageParameterFile parameterFile=null;

        public OverlayDisplayData DisplayData;

        public void Dispose()
        {
            mainThread.EndThread = true;
            mainThread.Dispose();
            acquisitionImage.StopSession();
            acquisitionImage.Dispose();
            camForm.Close();
            camForm.Dispose();
            resultForm.Close();
            resultForm.Dispose();
            attrForm.Close();
            attrForm.Dispose();
        }

        public string CameraName
        {
            get { return cameraName; }
            private set { cameraName = value; }
        }

        public Camera(string cameraName, int imageCount, DockPanel dockPanel, SnpSystem.Vision.Etc.MainThread.ParentThreadFunction pFunction)
        {
            imageSpec = new ImageProcessingSpec(imageCount);
            CameraName = cameraName;
            this.imageCount = imageCount;
            this.dockPanel = dockPanel;
            this.pFunction = pFunction;
            DisplayData = new OverlayDisplayData(MAXIMUM_LANE_COUNT,MAXIMUM_SEQUENCE_COUNT);
            initOriginalImageForm();
            initResultImageForm();
            initAttrTreeViewForm();
            initAcquisition();
            initThread();
        }
 
        void initOriginalImageForm()
        {
            camForm = new CamForm();
            viewerImage = new VisionImage(ImageType.Rgb32);
            camForm.Image = viewerImage;
            camForm.Tag = cameraName;
            camForm.Text = cameraName;
            camForm.Viewer.Tag = cameraName;
            camForm.Viewer.ShowScrollbars = true;
            camForm.Buttons = ButtonViewer.PictureAdj;
            camForm.CloseButtonVisible = false;
            camForm.Show(dockPanel, DockState.DockTop | DockState.Document);
        }

        void initResultImageForm()
        {
            Icon myIcon = new System.Drawing.Icon("icon.ico");
            resultImage = new VisionImage(ImageType.Rgb32);
            resultForm = new CamForm();
            resultForm.Text = "처리영상[" + cameraName + "]";
            resultForm.Icon = myIcon;
            resultForm.Viewer.Tag = cameraName;
            resultForm.Viewer.ShowScrollbars = true;
            resultForm.Image = resultImage;
            resultForm.Buttons = ButtonViewer.PictureAdj;
            resultForm.CloseButtonVisible = false;
            resultForm.Show(dockPanel, DockState.Document);
        }

        void initAttrTreeViewForm()
        {
            attrForm = new CamAttrTreeView();
            attrForm.Text = "카메라속성[" + cameraName + "]";
            attrForm.Tag = cameraName;
            attrForm.CloseButtonVisible = false;
            attrForm.TopMost = false;
            attrForm.Show(dockPanel, DockState.DockLeftAutoHide);
            attrForm.Hide();        
        }

        void initAcquisition()
        {
            acquisitionImage = new Acquisition(attrForm, camForm.Viewer, cameraName, camForm.toolStrip);
        }

        void initThread()
        {
            mainThread = new MainThread(cameraName, pFunction);
        }
    }
}
