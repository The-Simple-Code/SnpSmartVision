using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Diagnostics;

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
    public partial class MdiForm : Form
    {

        String Version = "Ver.2017.2.1.1.c";

        delegate void ProcessingTimeComsumptionDelegate();
        #region 화상처리용 버퍼/class의 정의

        static int MAXIMUM_LANE_COUNT = 2;
        //static int MAXIMUM_SEQUENCE_COUNT = 12;

        delegate void ResultDisplayDelegate(ProcessingValue[] result);
        delegate void DebuggerDisplayDelegate(string[] s);
        //delegate void ResultSendingDelegate(ProcessingValue[] result);
        bool[] toolState;

        Timer searchingTimer;

        SearchingForm sForm;

        RoiOffsetForm roiOffsetForm;

        SerialOut serialOut;

        ControlPort controlPort;

        ProcessingValue[] yesterData;

        DisplayResultForm ResultForm;

        TimeTableForm timeTable;

        AlgorithmStopWatch stopWatch;

        RootConfigFile rootFile;
        
        //화상처리 Config Data
        ImageParameterFile parameterFile;

        //과일의 외형처리를 위해 Particle Measurement Report를 생성하기 위한 조건을 저장하는 버퍼
        Collection<MeasurementType> MeasurementTypeCollection;
        int MeasurementTypeCount;
        
        //SearchDirection direction;
        EdgeOptions edgeOptions;

        StraightEdgeOptions straightEdgeOptions;

        Debugger DebuggerForm;

        //가상의 카메라
        Dictionary<string, Camera> cameras;

        //이미지취득 완료 알리 델리게이션 및 플래그
        //Dictionary<string, FlagDelegation> flagDelegations;
        Dictionary<string, bool> acqCompleteFlag;
        
        //카메라 목록을 보여주는 Form
        CamAttrForm camAttrForm;

        //프로그램설정을 보여주는 Form
        CamAttrForm algorithmAttrForm;
  
        //화상의 히스토그램을 보여주는 Form
        GraphForm graphForm;
        
        //Image Grab용 트리거신호 발생용 class
        VisionTrigger visionTrigger;
        
        //처리색상 모드 
        //ColorMode extractionColorMode;

        //ROI LIST DATA
        RoiDataControl roiData;
        
        //ROI 목록을 저장하는 폼(리스트뷰를 가지고 있음)
        RoiListView roiView;

        //Vision Image
        Dictionary<string, VisionImage[]> processImage8;
        Dictionary<string, VisionImage[]> processImage32;
        Dictionary<string, VisionImage> processXImage8;
        Dictionary<string, VisionImage> processXImage32;


        //VisionImage extractImage;

        
        //ROI를 화면에 그릴것인지 결정
        //RoiDisplayMode roiDisplayMode;
        bool userRoiDisplay = false;
        bool dynamicRoiDisplay = false;
        
        //화상처리 결과를 표시하는 플래그
        //displayImage[0]는 항상 true
        bool[] displayImage;


        //시험용
        //VisionConfigurationHelper visionHelper;

        RectangleAction rectangleAction;

        ImageList imageList;

        MakingTreeList mTree;

        //Image Processing 속성설정용 Page control
        bool pageChange=false;

        ProcessingResult[] Result;

        //Image Processing thread안에서 사용
        //Image Processing이 완료되면 flag를 true로 만듬
        //<cameraName,flag>
        Dictionary<string, bool> processingCompletionFlag;

        //임시 색상데이타
        //Dictionary<string, Dictionary<string,Range>> colorSpace;

        //트리거 플래그(시험용)
        //Dictionary<string, bool> flagTrigger;
        //Dictionary<string, bool> flagAcqusition;

        #endregion
        
        #region Class의 시작점
        public MdiForm()
        {
            InitializeComponent();

            //initMeasurementOptions();
            //initEdgeOptions();
            //Processing Image Display Flag
            displayImage = new bool[3] { true, false, false };
            toolState = new bool[20];
            stopWatch = new AlgorithmStopWatch();
            //stopWatch.StopWatchHandler += new EventHandler(stopWatch_StopWatchHandler);

            processingCompletionFlag = new Dictionary<string,bool>();
            
            //root file을 읽어온다
            //사용자,설정파일명 및 경로,레인수,화상처리수
            rootFile = new RootConfigFile();
            rootFile.Read();
            yesterData = new ProcessingValue[2];

            Result = new ProcessingResult[MAXIMUM_LANE_COUNT];
            for (int i = 0; i < MAXIMUM_LANE_COUNT; i++)
            {
                Result[i] = new ProcessingResult(rootFile.Carrier,rootFile.laneBalanceColorFactor);
                Result[i].AspectRatioDataType = rootFile.AspectBindingType;
                Result[i].ColorRatioDataType = rootFile.ColorBindingType;
                Result[i].SizeDataType = rootFile.SizeBindingType;
            }

            processImage8 = new Dictionary<string, VisionImage[]>();
            processImage32 = new Dictionary<string, VisionImage[]>();
            processXImage8 = new Dictionary<string, VisionImage>();
            processXImage32 = new Dictionary<string, VisionImage>();

           // extractImage = new VisionImage();

            //설정파일을 읽어온다
            parameterFile = new ImageParameterFile();
            parameterFile.InitParameters(rootFile.DefaultCam);
            //parameterFile=parameterFile.Default("cam1");
            parameterFile=parameterFile.ReadFile(rootFile.Root);
            
            //메인화면 타이틀 설정
            this.Text = "S&P SMART VISION["+rootFile.User+"]";
            dockPanel.Dock = DockStyle.Fill;
            
            //카메라 Drag & Drop용 Event Handler
            this.DragDrop += new DragEventHandler(MdiForm_DragDrop);
            this.DragEnter += new DragEventHandler(MdiForm_DragEnter);
            
            // 사용할 카메라 목록저장용
            cameras = new Dictionary<string, Camera>();
            
            acqCompleteFlag = new Dictionary<string, bool>();
            //flagDelegations = new Dictionary<string, FlagDelegation>();
   
            //roi data저장
            //roiData = new RoiDataControl();
            roiData = parameterFile.roiData;

            // visionHelper = new VisionConfigurationHelper();

            #region Color Histogram Form
            //컬러 히스토그램 표시용 폼
            graphForm = new GraphForm();
            graphForm.CloseButtonVisible = false;
            graphForm.Show(dockPanel, DockState.DockBottomAutoHide);
            graphForm.Hide();
            #endregion

            #region config tree list
            //프로그램 상세 설정정보를 보여주는 폼
            algorithmAttrForm = new CamAttrForm("시스템정보");
            algorithmAttrForm.CloseButtonVisible = false;
            algorithmAttrForm.Show(dockPanel, DockState.DockRightAutoHide);
            algorithmAttrForm.Hide();
            algorithmAttrForm.tabControl.Selecting += new TabControlCancelEventHandler(tabControl_Selecting);
            algorithmAttrForm.buttonColor.Click += new EventHandler(buttonColor_Click);
            algorithmAttrForm.comboImageType.SelectedValueChanged += new EventHandler(comboImageType_SelectedValueChanged);
            algorithmAttrForm.numericWidth.ValueChanged += new EventHandler(numericWidth_ValueChanged);
            algorithmAttrForm.numericHeight.ValueChanged += new EventHandler(numericHeight_ValueChanged);
            algorithmAttrForm.numericErosion.ValueChanged += new EventHandler(numericErosion_ValueChanged);
            algorithmAttrForm.comboSizeToKeep.SelectedValueChanged += new EventHandler(comboSizeToKeep_SelectedValueChanged);
            algorithmAttrForm.comboConnectivity.SelectedValueChanged += new EventHandler(comboConnectivity0_SelectedValueChanged);
            algorithmAttrForm.comboConnectivity1.SelectedValueChanged += new EventHandler(comboConnectivity1_SelectedValueChanged);
            algorithmAttrForm.checkFillHole.CheckedChanged += new EventHandler(checkFillHole_CheckedChanged);
            algorithmAttrForm.checkRejectBorder.CheckedChanged += new EventHandler(checkRejectBorder_CheckedChanged);
            algorithmAttrForm.checkRejectMatch.CheckedChanged += new EventHandler(checkRejectMatch_CheckedChanged);
            algorithmAttrForm.comboConnectivity2.SelectedValueChanged += new EventHandler(comboConnectivity2_SelectedValueChanged);
            algorithmAttrForm.panelColor.Click += new EventHandler(panelColor_Click);
            algorithmAttrForm.buttonCriteria.Click += new EventHandler(buttonCriteria_Click);
            algorithmAttrForm.checkEnabled.CheckedChanged += new EventHandler(checEnabled_CheckedChanged);
            algorithmAttrForm.comboContained.SelectedValueChanged += new EventHandler(comboContained_SelectedValueChanged);
            algorithmAttrForm.comboCombined.SelectedValueChanged += new EventHandler(comboCombined_SelectedValueChanged);
            algorithmAttrForm.buttonXColor.Click += new EventHandler(buttonXColor_Click);
            algorithmAttrForm.comboExtraImageType.SelectedValueChanged += new EventHandler(comboExtraImageType_SelectedValueChanged);
            algorithmAttrForm.panelExtraColor.Click += new EventHandler(panelExtraColor_Click);
            algorithmAttrForm.comboComposit.SelectedValueChanged += new EventHandler(comboComposit_SelectedValueChanged);
            algorithmAttrForm.label16.Text = Version;
            #endregion
            #region tree list display
            //프로그램 상세정보를 Tree Viewer에 표시하기 위한 Class 
            mTree = new MakingTreeList(algorithmAttrForm.treeView1);
            mTree.treeView.AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);
            mTree.SetRootConfigFile(VisionFilePath.General, rootFile);
            #endregion

            #region trigger control
            //트리거관련 설정
            //flagTrigger = new Dictionary<string, bool>();
            //flagAcqusition = new Dictionary<string, bool>();

            visionTrigger = new VisionTrigger(rootFile.InternalTriggerPeriod, rootFile.Inport);//visionHelper.TriggerInfo.TriggerChannel);
            visionTrigger.OnTriggerActivate += new EventHandler(visionTrigger_OnTriggerActivate);
            visionTrigger.Trigger=false;  //internal trigger on
            //visionTrigger.DataSending += ResultSendingRoutine;
            #endregion

            #region serial control for output
            //visionTrigger.Enabled = false;
            //extractionColorMode = visionHelper.OutLine.ColorType;
            serialOut = new SerialOut(rootFile);
            serialOut.PortOpen();
            #endregion

            #region serial control for program status
            controlPort = new ControlPort(rootFile);
            controlPort.PortOpen();
            #endregion

            #region main form display
            initMenu();
            initTools();
            initStatus();
            #endregion

            //PC에 연결된 카메라정보를 보여주는 폼
            ToolsForInit();
            if (checkCameraInformation()) setLastCondition();
            else
            {
                searchingTimer = new Timer();
                searchingTimer.Enabled = false;
                searchingTimer.Interval = 1000;
                searchingTimer.Tick += new EventHandler(searchingTimer_Tick);
                searchingTimer.Tag = 0;
                searchingTimer.Start();
                sForm = new SearchingForm();
                sForm.progressBar1.Value = 0;
                sForm.buttonSearchingCancel.Click += new EventHandler(buttonSearchingCancel_Click);
                sForm.StartPosition = FormStartPosition.CenterScreen;
                sForm.Show();
            }
            dockPanel.BringToFront();
        }

        void buttonSearchingCancel_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            SearchingForm f = (SearchingForm)b.Parent;
            refrashCameraList();
            ToolForForceStop();
            dockPanel.BringToFront();
            f.Close();

        }

        void searchingTimer_Tick(object sender, EventArgs e)
        {
            int count=sForm.progressBar1.Value;
            count++;
            if (count > 100) count = 0;
            sForm.progressBar1.Value = count;

            if (checkCameraInformation())
            {
                setLastCondition();
                searchingTimer.Enabled = false;
                sForm.Close();
            }
        }
        #endregion

        void refrashCameraList()
        {
            camAttrForm = new CamAttrForm();
            camAttrForm.CloseButtonVisible = false;
            camAttrForm.Show(dockPanel, DockState.DockRightAutoHide);
            camAttrForm.Hide();
            addCameras(camAttrForm.CamInfo);
            //PC에 연결된 카메라 목록과 설정파일의 카메라목록을 조사
            checkCameraList();
            
        }

        void setLastCondition()
        {
            refrashCameraList();
            insertCameraList();
            //serialOut.PortOpen();

            if (rootFile.UserMode == "SUPERVISOR") ToolsForSuperStop();
            else ToolsForCustomerStop();
           

            if (rootFile.AutoStart)
            {
                Timer tm = new Timer();
                tm.Enabled = false;
                tm.Interval = 2000;
                tm.Tick += new EventHandler(tm_Tick);
                tm.Start();
            }
        }

        void tm_Tick(object sender, EventArgs e)
        {
            Timer tm = (Timer)sender;
            tm.Stop();
            toolRun.PerformClick();
        }

        private bool checkCameraInformation()
        {
            ImaqdxCameraInformation[] cameraList = ImaqdxSystem.GetCameraInformation(true);
            int len = cameraList.Length;
            bool matched=false;
            foreach (string camera in parameterFile.CameraList)
            {
                for (int i = 0; i < len; i++)
                {
                    if (camera == cameraList[i].Name)
                    {
                        matched = true;
                        break;
                    }
                    matched = false;
                }
                if (matched == false) return false;
            }
            return true;
        }

        void DebuggerTxFrame(string[] s)
        {
            string message = serialOut.SendingPacket;
            int count = serialOut.PacketSize;
            DebuggerForm.DisplayProcessingResult("Size= "+count.ToString()+", Packet = "+ message);
            if (DebuggerForm.ExitRequest) DebuggerForm.Close();
 
        }
        void DebuggerDataUpdate(string[] s)
        {
            string cameraName = s[0];
            int seq = int.Parse(s[1]);
            string message = s[2];
            
            DebuggerForm.DisplayProcessingResult("["+cameraName+"] "+message);
            if (DebuggerForm.ExitRequest)  DebuggerForm.Close();
 
        }
        void ResultDataUpdate(ProcessingValue[] result)
        {
            
            //poDisplayProcessingResult(string message)
            if (ResultForm == null) return;
            if (Result[0].AspectRatioDataType == DataBindingType.Minimum)       ResultForm.labelARType.Text="Min.";
            else if (Result[0].AspectRatioDataType == DataBindingType.Maximum)  ResultForm.labelARType.Text="Max.";
            else if (Result[0].AspectRatioDataType == DataBindingType.Average)  ResultForm.labelARType.Text ="Ave.";
            else                                                                ResultForm.labelARType.Text ="Sum.";

            if (Result[0].ColorRatioDataType == DataBindingType.Minimum) ResultForm.labelColorType.Text = "Min.";
            else if (Result[0].ColorRatioDataType == DataBindingType.Maximum) ResultForm.labelColorType.Text = "Max.";
            else if (Result[0].ColorRatioDataType == DataBindingType.Average) ResultForm.labelColorType.Text = "Ave.";
            else if (Result[0].ColorRatioDataType == DataBindingType.LastAverage) ResultForm.labelColorType.Text = "Lav.";
            else ResultForm.labelColorType.Text = "Sum.";

            if (Result[0].SizeDataType == DataBindingType.Minimum) ResultForm.labelSizeType.Text = "Min.";
            else if (Result[0].SizeDataType == DataBindingType.Maximum) ResultForm.labelSizeType.Text = "Max.";
            else if (Result[0].SizeDataType == DataBindingType.Average) ResultForm.labelSizeType.Text = "Ave.";
            else ResultForm.labelSizeType.Text = "Sum.";
            
            int count = result.Length;
            if (result[0].noFruit == false)
            {
                ResultForm.label1.Text = string.Format("{0}", result[0].Size);
                ResultForm.label2.Text = string.Format("{0:F1}", result[0].ColorRatio / 10);
                ResultForm.label3.Text = string.Format("{0:F1}", result[0].AspectRatio / 10);
            }
            
            // 과일이 감지되지 않으면 마지막으로 감지된 과일 정보를 유지 2017.7.
            /*
            else
            {
                ResultForm.label1.Text = string.Format("{0}", 0);
                ResultForm.label2.Text = string.Format("{0:F1}", 0);
                ResultForm.label3.Text = string.Format("{0:F1}", 0);
            }
            */
            
            if (rootFile.Lane == 2)
            {
                if (result[1].noFruit == false)
                {
                    ResultForm.label4.Text = string.Format("{0}", result[1].Size);
                    ResultForm.label5.Text = string.Format("{0:F1}", result[1].ColorRatio / 10);
                    ResultForm.label6.Text = string.Format("{0:F1}", result[1].AspectRatio / 10);

                }
                
                /*
                else
                {
                    ResultForm.label4.Text = string.Format("{0}", 0);
                    ResultForm.label5.Text = string.Format("{0:F1}", 0);
                    ResultForm.label6.Text = string.Format("{0:F1}", 0);
                }*/
            }

            if (DebuggerForm != null)
            {
                if (DebuggerForm.FResult)
                {
                    for (int i = 0; i < rootFile.Lane; i++)
                    {
                        if (result[i].noFruit == false)
                        {
                            string s = string.Format("{0}-{1}-{2}-{3}", i + 1, result[i].Size, result[i].ColorRatio, result[i].AspectRatio);
                            DebuggerForm.DisplayProcessingResult(s);
                        }
                        if (DebuggerForm.ExitRequest) DebuggerForm.Close();
                    }

                }
            }

        }
        void initMeasurementOptions()
        {
            // Particle Measurement Report에서 Report할 아이템을 설정
            MeasurementTypeCollection = new Collection<MeasurementType>();
            MeasurementTypeCollection.Add(MeasurementType.MaxFeretDiameterStartX);
            MeasurementTypeCollection.Add(MeasurementType.MaxFeretDiameterStartY);
            MeasurementTypeCollection.Add(MeasurementType.MaxFeretDiameterEndX);
            MeasurementTypeCollection.Add(MeasurementType.MaxFeretDiameterEndX);
            MeasurementTypeCollection.Add(MeasurementType.MaxFeretDiameter);
            MeasurementTypeCollection.Add(MeasurementType.MaxFeretDiameterOrientation);
            MeasurementTypeCollection.Add(MeasurementType.CenterOfMassX);
            MeasurementTypeCollection.Add(MeasurementType.CenterOfMassY);
            MeasurementTypeCollection.Add(MeasurementType.ParticleAndHolesArea);
            MeasurementTypeCount = MeasurementTypeCollection.Count;
        }
        void initEdgeOptions()
        {
            //direction = SearchDirection.LeftToRight;

            // Fill in the edge options structure from the controls on the form.
            edgeOptions = new EdgeOptions();
            edgeOptions.ColumnProcessingMode = ColumnProcessingMode.Average;
            edgeOptions.InterpolationType = InterpolationMethod.ZeroOrder;
            edgeOptions.KernelSize = 3;
            edgeOptions.MinimumThreshold = 3;
            edgeOptions.Polarity = EdgePolaritySearchMode.All;
            edgeOptions.Width = 3;

            // Fill in the straight edge options structure from the controls on the form.
            straightEdgeOptions = new StraightEdgeOptions();
            straightEdgeOptions.AngleRange = 10;
            straightEdgeOptions.AngleTolerance = 1;
            straightEdgeOptions.HoughIterations = 5;
            straightEdgeOptions.ScoreRange.Initialize(0,1000);
            straightEdgeOptions.MinimumCoverage = 25;
            straightEdgeOptions.MinimumSignalToNoiseRatio = 0;
            straightEdgeOptions.NumberOfLines = 1;
            straightEdgeOptions.Orientation = 6;
            straightEdgeOptions.SearchMode = StraightEdgeSearchMode.FirstProjectionEdge;
            straightEdgeOptions.StepSize = 3;
        }
        void buttonTimeTableExit_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            TimeTableForm f = (TimeTableForm)b.FindForm();
            stopWatch.StopRecord();
            f.Close();
        }
        void buttonTimeTableMeas_Click(object sender, EventArgs e)
        {
            string name = timeTable.comboBox1.Text;
            if (name == "") return;
            stopWatch.ReadyRecord(name);
        }
        void showProcessingTimeConsumption()
        {
            if (timeTable.listView1.InvokeRequired)
            {
                ProcessingTimeComsumptionDelegate p= new ProcessingTimeComsumptionDelegate(showProcessingTimeConsumption);
                this.Invoke(p,new object[] {});
            }
            else
            {
                timeTable.listView1.Items.Clear();
                double sum = 0;
                double time;
                foreach (string str in stopWatch.Record.Keys)
                {
                    ListViewItem item = new ListViewItem(str);
                    
                    string value;
                    time = stopWatch.Record[str];
                    if (time == 0)
                    {
                        value = "1>";
                        time = 1;
                    }
                    else value = time.ToString();
                    sum += time;
                    item.SubItems.Add(value);
                    timeTable.listView1.Items.Add(item);
                }
                ListViewItem item2 = new ListViewItem("Total");
                item2.SubItems.Add(sum.ToString());
                timeTable.listView1.Items.Add(item2);
            }
        }
        void stopWatch_StopWatchHandler(object sender, EventArgs e)
        {
            try
            {
                timeTable.listView1.Items.Clear();
                double sum = 0;
                double time;
                foreach (string str in stopWatch.Record.Keys)
                {
                    ListViewItem item = new ListViewItem(str);
                    string value;
                    time = stopWatch.Record[str];
                    if (time == 0) value = "<0";
                    else value = time.ToString();
                    sum += time;
                    item.SubItems.Add(value);
                    timeTable.listView1.Items.Add(item);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }
        }
        void insertCameraList()
        {
            foreach (string cam in parameterFile.CameraList)
            {
                if (cam == "") continue;
                makeImageViewForm(cam);
            }
        }
        bool checkCameraList()
        {
            Dictionary<string, string> cameras = new Dictionary<string, string>();
            int len = camAttrForm.CamInfo.Length;
            if (len <= 0) return false;

            for (int i = 0; i < len; i++)
                cameras.Add(camAttrForm.CamInfo[i].Name, camAttrForm.CamInfo[i].Name);

            foreach (string camera in parameterFile.CameraList)
            {
                if (camera == "") continue;
                if (cameras.ContainsKey(camera) == false) return false;
            }
            return true;
        }
  
                
        #region Program Attribute Control(어트리뷰제어)
       

        void buttonCriteria_Click(object sender, EventArgs e)
        {
            ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
            ParticleFilterCriteriaParameters p = imageParameter.particleFilterParameters.criteria;
            SnpSystem.Vision.Etc.CriteriaForm form = new SnpSystem.Vision.Etc.CriteriaForm();
            form.buttonDelete.Tag = (object)form;
            //form.buttonDelete.Click += new EventHandler(buttonDelete_Click);
            //form.buttonInsert.Click += new EventHandler(buttonInsert_Click);
            form.para = p;
            form.Tag = (string)algorithmAttrForm.tabControl.TabPages[5].Tag;
            form.DataChanged += new EventHandler(form_DataChanged);
            form.Show();
        }

        void form_DataChanged(object sender, EventArgs e)
        {
            SnpSystem.Vision.Etc.CriteriaForm form = (SnpSystem.Vision.Etc.CriteriaForm)sender;
            
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[5].Tag, rootFile.Count, parameterFile);
        }
        /*
        void buttonInsert_Click(object sender, EventArgs e)
        {
            SnpSystem.Vision.Etc.CriteriaForm form = (SnpSystem.Vision.Etc.CriteriaForm)sender;
        }

        void buttonDelete_Click(object sender, EventArgs e)
        {
            SnpSystem.Vision.Etc.CriteriaForm form = (SnpSystem.Vision.Etc.CriteriaForm)sender;
        }
        */

        void comboComposit_SelectedValueChanged(object sender, EventArgs e)
        {
            ExtraProcessingParameters imageParameter = (ExtraProcessingParameters)algorithmAttrForm.tabControl.Tag;
            if (algorithmAttrForm.comboComposit.Text == "XimageComposit")
            {
                toolThree.Enabled = true;
                imageParameter.Composit = CompositMethod.XimageComposit;
            }
            else if (algorithmAttrForm.comboComposit.Text == "Image2Composit")
            {
                toolThree.Enabled = false;
                imageParameter.Composit = CompositMethod.Image2Composit;
            }
            else
            {
                toolThree.Enabled = false;
                imageParameter.Composit = CompositMethod.NotComposit;
            }

            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[8].Tag, rootFile.Count, parameterFile);
        }
        void panelExtraColor_Click(object sender, EventArgs e)
        {
            ExtraProcessingParameters imageParameter = (ExtraProcessingParameters)algorithmAttrForm.tabControl.Tag;
            DisplayColorParameter p = imageParameter.DisplayColor;
            ColorDialog dia = new ColorDialog();
            dia.AllowFullOpen = true;
            dia.Color = p.DisplayColor;
            dia.AnyColor = true;
            dia.SolidColorOnly = true;
            if (DialogResult.OK == dia.ShowDialog())
            {
                algorithmAttrForm.panelColor.BackColor = dia.Color;
                p.DisplayColor = algorithmAttrForm.panelColor.BackColor;
                mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[8].Tag, rootFile.Count, parameterFile);
            }
        }

        void comboExtraImageType_SelectedValueChanged(object sender, EventArgs e)
        {
            ExtraProcessingParameters imageParameter = (ExtraProcessingParameters)algorithmAttrForm.tabControl.Tag;
            if (algorithmAttrForm.comboExtraImageType.Text == "HSL") imageParameter.colorMode = ColorMode.Hsl;
            else if (algorithmAttrForm.comboExtraImageType.Text == "HSI") imageParameter.colorMode = ColorMode.Hsi;
            else if (algorithmAttrForm.comboExtraImageType.Text == "HSV") imageParameter.colorMode = ColorMode.Hsv;
            else imageParameter.colorMode = ColorMode.Rgb;
            
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[8].Tag, rootFile.Count, parameterFile);
        }

       
        
        void comboCombined_SelectedValueChanged(object sender, EventArgs e)
        {
            ExtraProcessingParameters p = (ExtraProcessingParameters)algorithmAttrForm.tabControl.Tag;
            string val = algorithmAttrForm.comboCombined.Text;
            if (val == ExtraProcessingValidArea.All.ToString()) p.CombinedImage = ExtraProcessingValidArea.All;
            else if (val == ExtraProcessingValidArea.Process1.ToString()) p.CombinedImage = ExtraProcessingValidArea.Process1;
            else p.CombinedImage = ExtraProcessingValidArea.Process2;
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[8].Tag, rootFile.Count, parameterFile);
        }

        void comboContained_SelectedValueChanged(object sender, EventArgs e)
        {
            ExtraProcessingParameters p = (ExtraProcessingParameters)algorithmAttrForm.tabControl.Tag;
            string val = algorithmAttrForm.comboContained.Text;
            if (val == ContainedProcessingMode.Exclude.ToString()) p.ContainedMode = ContainedProcessingMode.Exclude;
            else p.ContainedMode = ContainedProcessingMode.Include;
            
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[8].Tag, rootFile.Count, parameterFile);
        }

        void checEnabled_CheckedChanged(object sender, EventArgs e)
        {
            ExtraProcessingParameters p = (ExtraProcessingParameters)algorithmAttrForm.tabControl.Tag;
            p.Enable = algorithmAttrForm.checkEnabled.Checked;
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[8].Tag, rootFile.Count, parameterFile);
        }


        void panelColor_Click(object sender, EventArgs e)
        {
            ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
            DisplayColorParameter p = imageParameter.displayColorParameters;
            ColorDialog dia = new ColorDialog();
            dia.AllowFullOpen = true;
            dia.Color = p.DisplayColor;
            dia.AnyColor = true;
            dia.SolidColorOnly = true;
            if (DialogResult.OK == dia.ShowDialog())
            {
                algorithmAttrForm.panelColor.BackColor = dia.Color;
                p.DisplayColor = algorithmAttrForm.panelColor.BackColor;
                mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[7].Tag, rootFile.Count, parameterFile);
            }
        }
        
        void comboConnectivity2_SelectedValueChanged(object sender, EventArgs e)
        {
            ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
            ParticleReportParameters p = imageParameter.particleReportParameters;
            if (algorithmAttrForm.comboConnectivity2.Text == "Connectivity8") p.connectivity = Connectivity.Connectivity8;
            else p.connectivity = Connectivity.Connectivity4;
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[6].Tag, rootFile.Count, parameterFile);
        }
        void checkFillHole_CheckedChanged(object sender, EventArgs e)
        {
            ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
            ParticleFilterOptionParameters p = imageParameter.particleFilterParameters.option;
            p.fillHoles = algorithmAttrForm.checkFillHole.Checked;
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[4].Tag, rootFile.Count, parameterFile);
        }
        void checkRejectBorder_CheckedChanged(object sender, EventArgs e)
        {
            ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
            ParticleFilterOptionParameters p = imageParameter.particleFilterParameters.option;
            p.rejectBorder = algorithmAttrForm.checkRejectBorder.Checked;
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[4].Tag, rootFile.Count, parameterFile);
        }
        void checkRejectMatch_CheckedChanged(object sender, EventArgs e)
        {
            ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
            ParticleFilterOptionParameters p = imageParameter.particleFilterParameters.option;
            p.rejectMatches = algorithmAttrForm.checkRejectMatch.Checked;
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[4].Tag, rootFile.Count, parameterFile);
        }
        void comboConnectivity1_SelectedValueChanged(object sender, EventArgs e)
        {
            ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
            ParticleFilterOptionParameters p = imageParameter.particleFilterParameters.option;
            if (algorithmAttrForm.comboConnectivity1.Text == "Connectivity8") p.connectivity = Connectivity.Connectivity8;
            else p.connectivity = Connectivity.Connectivity4;
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[4].Tag, rootFile.Count, parameterFile);
        }
        void numericErosion_ValueChanged(object sender, EventArgs e)
        {
            ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
            RemoveParticleParameters p = imageParameter.removeParticleParameter;
            p.erosion = (int)algorithmAttrForm.numericErosion.Value;
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[3].Tag, rootFile.Count, parameterFile);
        }
        void comboSizeToKeep_SelectedValueChanged(object sender, EventArgs e)
        {
            ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
            RemoveParticleParameters p = imageParameter.removeParticleParameter;
            if (algorithmAttrForm.comboSizeToKeep.Text == "KeepLarge") p.sizeToKeep = SizeToKeep.KeepLarge;
            else p.sizeToKeep = SizeToKeep.KeepSmall;
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[3].Tag, rootFile.Count, parameterFile);
        }

        void comboConnectivity0_SelectedValueChanged(object sender, EventArgs e)
        {
            ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
            RemoveParticleParameters p = imageParameter.removeParticleParameter;
            if (algorithmAttrForm.comboConnectivity.Text == "Connectivity8") p.connectivity = Connectivity.Connectivity8;
            else p.connectivity = Connectivity.Connectivity4;
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[3].Tag, rootFile.Count, parameterFile);
        }

        void numericWidth_ValueChanged(object sender, EventArgs e)
        {
            ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
            MedianFilterParameters p = imageParameter.medianFilterParameter;
            p.width = (int)algorithmAttrForm.numericWidth.Value;
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[2].Tag, rootFile.Count, parameterFile);
        }
        void numericHeight_ValueChanged(object sender, EventArgs e)
        {
            ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
            MedianFilterParameters p = imageParameter.medianFilterParameter;
            p.height = (int)algorithmAttrForm.numericHeight.Value;
            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[2].Tag, rootFile.Count, parameterFile);
        }
        void comboImageType_SelectedValueChanged(object sender, EventArgs e)
        {
            ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
            ColorThresholdParameters p = imageParameter.colorParameter;
            if(algorithmAttrForm.comboImageType.Text == "HSL") p.colorMode = ColorMode.Hsl;
            else if(algorithmAttrForm.comboImageType.Text == "HSI") p.colorMode = ColorMode.Hsi;
            else if(algorithmAttrForm.comboImageType.Text == "HSV") p.colorMode = ColorMode.Hsv;
            else  p.colorMode = ColorMode.Rgb;

            mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[1].Tag, rootFile.Count, parameterFile);
        }

        void buttonXColor_Click(object sender, EventArgs e)
        {

            //Button b = (Button)sender;
            //ImageSelectionForm form = (ImageSelectionForm)b.FindForm();
            //String cam = form.comboCamera.Text;
            
            ExtraProcessingParameters p = (ExtraProcessingParameters)algorithmAttrForm.tabControl.Tag;
            string camera = p.owner;
            SnpSystem.Vision.Viewer.ColorRangeForm colorXRangeForm = new SnpSystem.Vision.Viewer.ColorRangeForm((ColorThresholdParameters)p);
            colorXRangeForm.Text = "Color Setting Panel["+camera+",Extra Image]";
            colorXRangeForm.ValueChanged += new EventHandler(colorXRangeForm_ValueChanged);
            colorXRangeForm.Show();

        }

        void buttonColor_Click(object sender, EventArgs e)
        {
           //Button b = (Button)sender;
           //ImageSelectionForm form = (ImageSelectionForm)b.FindForm();
           String img = "image" + (algorithmAttrForm.SelectedIndex+1).ToString(); //form.comboImage.Text; 

           ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
           String cam = imageParameter.CameraName; 

           SnpSystem.Vision.Viewer.ColorRangeForm colorRangeForm=new SnpSystem.Vision.Viewer.ColorRangeForm(imageParameter.colorParameter);
           colorRangeForm.ValueChanged += new EventHandler(colorRangeForm_ValueChanged);
           colorRangeForm.Text = "Color Setting Panel["+cam+","+img+"]";
           colorRangeForm.Tag=1; 
           colorRangeForm.Show();
           
        }
        void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            e.Cancel = pageChange == true ? false : true;
            pageChange = false;
        }
        void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView v = (TreeView)sender;
            TreeNode node = v.SelectedNode;

            pageChange = true;

            try
            {

                if (node.Tag is ExtraProcessingParameters)
                {
                    ExtraProcessingParameters p = (ExtraProcessingParameters)node.Tag;
                    TreeDataCheck h = new TreeDataCheck(node.FullPath);
                    algorithmAttrForm.tabControl.Tag = (object)parameterFile.ExtraData[h.CameraName];
                    algorithmAttrForm.comboCombined.Text = parameterFile.ExtraData[h.CameraName].CombinedImage.ToString();
                    algorithmAttrForm.comboContained.Text = parameterFile.ExtraData[h.CameraName].ContainedMode.ToString();
                    algorithmAttrForm.checkEnabled.Checked = parameterFile.ExtraData[h.CameraName].Enable;
                    algorithmAttrForm.panelExtraColor.BackColor = parameterFile.ExtraData[h.CameraName].DisplayColor.DisplayColor;
                    algorithmAttrForm.comboExtraImageType.Text = parameterFile.ExtraData[h.CameraName].colorMode.ToString().ToUpper();
                    algorithmAttrForm.comboComposit.Text = parameterFile.ExtraData[h.CameraName].Composit.ToString();
                    algorithmAttrForm.tabControl.TabPages[8].Tag = (object)node.FullPath;
                    algorithmAttrForm.showTab(algorithmAttrForm.tabControl.TabPages[8]);
                }
                else if (node.Tag is ColorThresholdParameters)
                {
                    ColorThresholdParameters p = (ColorThresholdParameters)node.Tag;
                    TreeDataCheck h = new TreeDataCheck(node.FullPath);
                    ImageProcessingParameters[] para = parameterFile.ProcessingParameter[h.CameraName];
                    algorithmAttrForm.tabControl.Tag = (object)para[h.ImageNumber];
                    algorithmAttrForm.comboImageType.Text = para[h.ImageNumber].colorParameter.colorMode.ToString().ToUpper();
                    algorithmAttrForm.tabControl.TabPages[1].Tag = (object)node.FullPath;
                    algorithmAttrForm.showTab(algorithmAttrForm.tabControl.TabPages[1]);
                }
                else if (node.Tag is MedianFilterParameters)
                {
                    MedianFilterParameters p = (MedianFilterParameters)node.Tag;
                    TreeDataCheck h = new TreeDataCheck(node.FullPath);
                    ImageProcessingParameters[] para = parameterFile.ProcessingParameter[h.CameraName];
                    algorithmAttrForm.tabControl.Tag = (object)para[h.ImageNumber];
                    algorithmAttrForm.numericWidth.Value = para[h.ImageNumber].medianFilterParameter.width == 0 ? 5 : para[h.ImageNumber].medianFilterParameter.width;
                    algorithmAttrForm.numericHeight.Value = para[h.ImageNumber].medianFilterParameter.height == 0 ? 5 : para[h.ImageNumber].medianFilterParameter.height;
                    algorithmAttrForm.tabControl.TabPages[2].Tag = (object)node.FullPath;
                    algorithmAttrForm.showTab(algorithmAttrForm.tabControl.TabPages[2]);
                }
                else if (node.Tag is RemoveParticleParameters)
                {
                    RemoveParticleParameters p = (RemoveParticleParameters)node.Tag;
                    TreeDataCheck h = new TreeDataCheck(node.FullPath);
                    ImageProcessingParameters[] para = parameterFile.ProcessingParameter[h.CameraName];
                    algorithmAttrForm.tabControl.Tag = (object)para[h.ImageNumber];
                    algorithmAttrForm.numericErosion.Value = para[h.ImageNumber].removeParticleParameter.erosion;
                    algorithmAttrForm.comboSizeToKeep.Text = para[h.ImageNumber].removeParticleParameter.sizeToKeep.ToString();
                    algorithmAttrForm.comboConnectivity.Text = para[h.ImageNumber].removeParticleParameter.connectivity.ToString();
                    algorithmAttrForm.tabControl.TabPages[3].Tag = (object)node.FullPath;
                    algorithmAttrForm.showTab(algorithmAttrForm.tabControl.TabPages[3]);
                }
                else if (node.Tag is ParticleFilterOptionParameters)
                {
                    ParticleFilterOptionParameters p = (ParticleFilterOptionParameters)node.Tag;
                    TreeDataCheck h = new TreeDataCheck(node.FullPath);
                    ImageProcessingParameters[] para = parameterFile.ProcessingParameter[h.CameraName];
                    algorithmAttrForm.tabControl.Tag = (object)para[h.ImageNumber];
                    algorithmAttrForm.comboConnectivity1.Text = para[h.ImageNumber].particleFilterParameters.option.connectivity.ToString();
                    algorithmAttrForm.checkFillHole.Checked = para[h.ImageNumber].particleFilterParameters.option.fillHoles;
                    algorithmAttrForm.checkRejectBorder.Checked = para[h.ImageNumber].particleFilterParameters.option.rejectBorder;
                    algorithmAttrForm.checkRejectMatch.Checked = para[h.ImageNumber].particleFilterParameters.option.rejectMatches;
                    algorithmAttrForm.tabControl.TabPages[4].Tag = (object)node.FullPath;
                    algorithmAttrForm.showTab(algorithmAttrForm.tabControl.TabPages[4]);
                }
                else if (node.Tag is ParticleFilterCriteriaParameters)
                {
                    ParticleFilterCriteriaParameters p = (ParticleFilterCriteriaParameters)node.Tag;
                    TreeDataCheck h = new TreeDataCheck(node.FullPath);
                    ImageProcessingParameters[] para = parameterFile.ProcessingParameter[h.CameraName];
                    algorithmAttrForm.tabControl.Tag = (object)para[h.ImageNumber];
                    algorithmAttrForm.tabControl.TabPages[5].Tag = (object)node.FullPath;
                    algorithmAttrForm.showTab(algorithmAttrForm.tabControl.TabPages[5]);
                }
                else if (node.Tag is ParticleReportParameters)
                {
                    ParticleReportParameters p = (ParticleReportParameters)node.Tag;
                    TreeDataCheck h = new TreeDataCheck(node.FullPath);
                    ImageProcessingParameters[] para = parameterFile.ProcessingParameter[h.CameraName];
                    algorithmAttrForm.tabControl.Tag = (object)para[h.ImageNumber];
                    algorithmAttrForm.comboConnectivity2.Text = para[h.ImageNumber].particleReportParameters.connectivity.ToString();
                    algorithmAttrForm.tabControl.TabPages[6].Tag = (object)node.FullPath;
                    algorithmAttrForm.showTab(algorithmAttrForm.tabControl.TabPages[6]);
                }
                else if (node.Tag is DisplayColorParameter)
                {
                    DisplayColorParameter p = (DisplayColorParameter)node.Tag;
                    TreeDataCheck h = new TreeDataCheck(node.FullPath);
                    ImageProcessingParameters[] para = parameterFile.ProcessingParameter[h.CameraName];
                    algorithmAttrForm.tabControl.Tag = (object)para[h.ImageNumber];
                    algorithmAttrForm.panelColor.BackColor = para[h.ImageNumber].displayColorParameters.DisplayColor;
                    algorithmAttrForm.tabControl.TabPages[7].Tag = (object)node.FullPath;
                    algorithmAttrForm.showTab(algorithmAttrForm.tabControl.TabPages[7]);
                }

                //else if (node.Tag is IEnumerable<RoiData>)
                //{
                //    IEnumerable<RoiData> p = (IEnumerable<RoiData>)node.Tag;
                //    TreeDataCheck h = new TreeDataCheck(node.FullPath);

                //}
                else
                {
                    algorithmAttrForm.showTab(algorithmAttrForm.tabControl.TabPages[0]);
                    pageChange = false;
                }
            }
            catch
            {

            }
        }
        #endregion

        #region 1대의 카메라가 연결될때 필요한 자원들을 생성함

        void makeImageViewForm(string cameraName)
        {
            if (checkActiveViewerForm(cameraName)) return;
            visionTrigger.Enabled = false;

            if (parameterFile.CameraList.Contains(cameraName) == false) parameterFile = parameterFile.Default(cameraName);
            //if(!flagAcqusition.ContainsKey(cameraName)) flagAcqusition.Add(cameraName,false);
            //if(!flagTrigger.ContainsKey(cameraName)) flagTrigger.Add(cameraName,false);
            //Camera cam = new Camera(cameraName, 1, dockPanel, highPriorityThread,flagTrigger,flagAcqusition);
      
            Camera cam = new Camera(cameraName, 1, dockPanel, highPriorityThread);

            VisionImage[] image8 = new VisionImage[rootFile.Count];
            VisionImage[] image32 = new VisionImage[rootFile.Count]; 

            for (int i = 0; i < rootFile.Count; i++)
            {
                image8[i] = new VisionImage(ImageType.U8);
                image32[i] = new VisionImage(ImageType.Rgb32); 
            }
            //VisionImage ximage8 = new VisionImage(ImageType.U8);
            //VisionImage ximage32 = new VisionImage(ImageType.Rgb32);

            processImage8.Add(cameraName, image8);
            processXImage8.Add(cameraName, new VisionImage(ImageType.U8));
            processImage32.Add(cameraName, image32);
            processXImage32.Add(cameraName, new VisionImage(ImageType.Rgb32));

            cam.parameterFile = parameterFile;
            cam.camForm.Viewer.RoiChanged += new EventHandler<ContoursChangedEventArgs>(Viewer_RoiChanged);
            
            cameras.Add(cameraName, cam);
            cameras[cameraName].acquisitionImage.OnGrabImage += new EventHandler(MdiForm_OnGrabImage);
            visionTrigger.flagDelegation.RegisterDelegation(cameraName, setAcquisitionFlag);

            ItemConnection(cameraName, false);

            //visionTrigger.Enabled = true;
            //itmCameraAttrView.Enabled = true;
            addCameraStatus(cameraName);
            //colorSpace.Remove("cameraName");
            UncheckedSettingMenu();
            if (roiView != null) roiView.Close();
            mTree.SetSelectedImageParameterFile(cameraName, rootFile.Count, parameterFile);

            if (processingCompletionFlag.ContainsKey(cameraName)) processingCompletionFlag[cameraName] = false;
            else processingCompletionFlag.Add(cameraName, false);

            for (int i = 0; i < MAXIMUM_LANE_COUNT; i++) Result[i].CameraCount++;
            visionTrigger.Enabled = true;
            itmCameraAttrView.Enabled = true;
            remakeMenuEnableState();
            //config file name & path를 status bar에표시
            addConfigFileStatus();
        }
        #endregion

        #region 1대 카메라를 제거할때 함께 자원을 제거함
        void deleteImageViewForm(string cameraName,bool removeSettings)
        {
            if (!checkActiveViewerForm(cameraName)) return;
            if (processingCompletionFlag.ContainsKey(cameraName)) processingCompletionFlag.Remove(cameraName);
            visionTrigger.Enabled = false;
            visionTrigger.flagDelegation.RemoveDelegation(cameraName);

            //if (visionTrigger.flagTrigger.ContainsKey(cameraName)) visionTrigger.flagTrigger.Remove(cameraName);

            cameras[cameraName].Dispose();
            cameras.Remove(cameraName);
            for (int i = 0; i < MAXIMUM_LANE_COUNT; i++) Result[i].CameraCount--;
            ItemConnection(cameraName, true);
           
            visionTrigger.Enabled = true;

            if (cameras.Count == 0)
            {
                itmCameraAttrView.Checked = false;
                itmCameraAttrView.Enabled = false;
            }
            removeCameraStatus(cameraName);

            processImage8.Remove(cameraName);
            processXImage8.Remove(cameraName);
            processImage32.Remove(cameraName);
            //processXImage32.Remove(cameraName);

            if(removeSettings==true)parameterFile = parameterFile.RemoveImageParameterFile(cameraName);
            mTree.RemoveSelectedImageParameterFile(cameraName);
            //removeImageProcessingAttribute(cameraName);
            remakeMenuEnableState();
        }
        #endregion

        void remakeMenuEnableState()
        {
            if (cameras.Count > 0)
            {
                if (rootFile.UserMode == "SUPERVISOR") ToolsForSuperStop();
                else ToolsForCustomerStop();
            }
        }

        #region 모든 Viewer Form을 제거함
        void deleteAllImageViewForm()
        {
            List<string> camName = new List<string>();
            foreach (string name in cameras.Keys) camName.Add(name);
            foreach (string key in camName) deleteImageViewForm(key,true);
        }

        void closeAllImageViewForm()
        {
            List<string> camName = new List<string>();
            foreach (string name in cameras.Keys) camName.Add(name);
            foreach (string key in camName) deleteImageViewForm(key, false);
        }

        #endregion

        void initStatus()
        {
            statusStrip1.Items.Clear();
            
            imageList = new ImageList();
            imageList.Images.Add("meter",new Bitmap(Properties.Resources.meter));
            imageList.Images.Add("flag",new Bitmap(Properties.Resources.flag));
            imageList.Images.Add("seperator",new Bitmap(Properties.Resources.seperator));
            imageList.Images.Add("video",new Bitmap(Properties.Resources.Video));
            //imageList.Images.Add("ledGreen",new Bitmap(Properties.Resources.ledDarkGreen));
            //imageList.Images.Add("ledDarkGreen",new Bitmap(Properties.Resources.ledGreen));
            imageList.Images.Add("customer", new Bitmap(Properties.Resources.customer));
            imageList.Images.Add("supervisor", new Bitmap(Properties.Resources.supervisor));
            imageList.Images.Add("link", new Bitmap(Properties.Resources.connect));
            imageList.Images.Add("dislink", new Bitmap(Properties.Resources.disconnect));
            imageList.Images.Add("file", new Bitmap(Properties.Resources.Save));

           /*
            ToolStripStatusLabel led = new ToolStripStatusLabel("", imageList.Images["ledDarkGreen"]);
            led.ImageTransparentColor = Color.White;
            led.Text = "  ";
            led.Name = "led";
            statusStrip1.Items.Add(led);*/
           

            ToolStripStatusLabel label1 = new ToolStripStatusLabel("", imageList.Images["meter"]);
            label1.ImageTransparentColor = Color.White;
            //label1.BorderSides = ToolStripStatusLabelBorderSides.All;
            label1.Text = "No Speed";
            label1.Name = "labelSpeedMeter";
            statusStrip1.Items.Add(label1);

            ToolStripStatusLabel sp1 = new ToolStripStatusLabel("", imageList.Images["seperator"]);
            sp1.ImageTransparentColor = Color.White;
            //label2.BorderSides = ToolStripStatusLabelBorderSides.All;
            sp1.Text = "";
            sp1.Name = "sp1";
            statusStrip1.Items.Add(sp1);

            ToolStripStatusLabel label2 = new ToolStripStatusLabel("", imageList.Images["flag"]);
            label2.ImageTransparentColor = Color.White;
            //label2.BorderSides = ToolStripStatusLabelBorderSides.All;
            label2.Text = "Internal Trigger";
            label2.Name = "labelTriggerMode";
            statusStrip1.Items.Add(label2);

            ToolStripStatusLabel sp2 = new ToolStripStatusLabel("", imageList.Images["seperator"]);
            sp2.ImageTransparentColor = Color.White;
            sp2.Text = "";
            sp2.Name = "sp2";
            statusStrip1.Items.Add(sp2);

            ToolStripStatusLabel label3 = new ToolStripStatusLabel("", rootFile.UserMode == "SUPERVISOR" ? imageList.Images["supervisor"] : imageList.Images["customer"]);
            label3.ImageTransparentColor = Color.White;
            //label2.BorderSides = ToolStripStatusLabelBorderSides.All;
            label3.Text = "";
            label3.Name = "userMode";
            statusStrip1.Items.Add(label3);

            ToolStripStatusLabel sp3 = new ToolStripStatusLabel("", imageList.Images["seperator"]);
            sp3.ImageTransparentColor = Color.White;
            sp3.Text = "";
            sp3.Name = "sp3";
            statusStrip1.Items.Add(sp3);


            ToolStripStatusLabel label4 = new ToolStripStatusLabel("",visionTrigger.triggerPortName == "Not Assigned" ? imageList.Images["dislink"] : imageList.Images["link"]);
            label4.ImageTransparentColor = Color.White;
            //label2.BorderSides = ToolStripStatusLabelBorderSides.All;
            label4.Text = "IN:" + rootFile.Inport;
            label4.Name = "IN";
            statusStrip1.Items.Add(label4);

            ToolStripStatusLabel sp4 = new ToolStripStatusLabel("", imageList.Images["seperator"]);
            sp4.ImageTransparentColor = Color.White;
            sp4.Text = "";
            sp4.Name = "sp4";
            statusStrip1.Items.Add(sp4);

            ToolStripStatusLabel label5 = new ToolStripStatusLabel("",serialOut.port==null?imageList.Images["dislink"]:imageList.Images["link"]);
            label5.ImageTransparentColor = Color.White;
            //label2.BorderSides = ToolStripStatusLabelBorderSides.All;
            label5.Text = "OUT:" + rootFile.Outport;
            label5.Name = "OUT";
            statusStrip1.Items.Add(label5);

            ToolStripStatusLabel sp5 = new ToolStripStatusLabel("", imageList.Images["seperator"]);
            sp5.ImageTransparentColor = Color.White;
            sp5.Text = "";
            sp5.Name = "sp5";
            statusStrip1.Items.Add(sp5);

            ToolStripStatusLabel label6 = new ToolStripStatusLabel("", rootFile.Alarm==false ? imageList.Images["dislink"] : imageList.Images["link"]);
            label6.ImageTransparentColor = Color.White;
            //label2.BorderSides = ToolStripStatusLabelBorderSides.All;
            label6.Text = "ALARM:" + rootFile.Controlport;
            label6.Name = "ALARM";
            statusStrip1.Items.Add(label6);

            ToolStripStatusLabel sp6 = new ToolStripStatusLabel("", imageList.Images["seperator"]);
            sp6.ImageTransparentColor = Color.White;
            sp6.Text = "";
            sp6.Name = "sp6";
            statusStrip1.Items.Add(sp6);

        }

        void addConfigFileStatus()
        {

            removeConfigFileStatus();
            
            ToolStripStatusLabel label7 = new ToolStripStatusLabel("",imageList.Images["file"]);
            label7.ImageTransparentColor = Color.White;
            //label2.BorderSides = ToolStripStatusLabelBorderSides.All;
            label7.Text = "config file:" + rootFile.Root;
            label7.Name = "FILE";
            statusStrip1.Items.Add(label7);

            ToolStripStatusLabel sp7 = new ToolStripStatusLabel("", imageList.Images["seperator"]);
            sp7.ImageTransparentColor = Color.White;
            sp7.Text = "";
            sp7.Name = "sp5";
            statusStrip1.Items.Add(sp7);
        }

        void removeConfigFileStatus()
        {
            if (statusStrip1.Items.ContainsKey("FILE") == true)
            {
                statusStrip1.Items.RemoveByKey("FILE");
            }
        }


        void statusLedOn()
        {
            ToolStripStatusLabel led = (ToolStripStatusLabel)statusStrip1.Items["led"];
            led.Image = imageList.Images["ledGreen"];
        }
        void statusLedOff()
        {
            ToolStripStatusLabel led = (ToolStripStatusLabel)statusStrip1.Items["led"];
            led.Image = imageList.Images["ledDarkGreen"];
        }

        void addCameraStatus(string cameraName)
        {
            if (statusStrip1.Items.ContainsKey(cameraName) == true)
            {
                statusStrip1.Items.RemoveByKey(cameraName);
            }
            ToolStripStatusLabel label = new ToolStripStatusLabel(cameraName, imageList.Images[3]);
            //label.BorderSides = ToolStripStatusLabelBorderSides.All;
            label.ImageTransparentColor = Color.White;
            label.Text = cameraName;
            label.Name = cameraName;
            statusStrip1.Items.Add(label);

            ToolStripStatusLabel s = new ToolStripStatusLabel("", imageList.Images["seperator"]);
            s.ImageTransparentColor = Color.White;
            s.Text = "";
            s.Name = "s";
            statusStrip1.Items.Add(s);
        }

        void removeCameraStatus(string cameraName)
        {
            if (statusStrip1.Items.ContainsKey(cameraName) == true)
            {
                statusStrip1.Items.RemoveByKey(cameraName);
            }
        }

        /*     
        void removeImageProcessingAttribute(string cameraName)
        {
            TreeView tree = algorithmAttrForm.treeView1;
            if (tree.Nodes.ContainsKey(cameraName) == true)
            {
                tree.Nodes.RemoveByKey(cameraName);
            }
        }
        */
        #region 카메라목록에서 Drag & Drop
        void MdiForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        void MdiForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
               
                TreeNode tn = ((MdiForm)sender).camAttrForm.treeView1.SelectedNode;
                if (tn == null) return;
                if (tn.Text == "") return;
                makeImageViewForm(tn.Text);
            }
        }
        #endregion
       
        #region 생성된 Viewer Form이 있는지 조사
        bool checkActiveViewerForm(string cameraName)
        {
            try
            {
                CamForm form = cameras[cameraName].camForm;
                if (form == null) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        private bool setAcquisitionFlag(string name)
        {
            acqCompleteFlag[name] = true;
            return acqCompleteFlag[name];
        }

        #region 일괄 Form생성
        void initVisionImageViewer()
        {/*

            try
            {

                int camCount =visionHelper.GetCameraCount();
 
                //viewerForm = new CamForm[camCount];
                //acq = new Acquisition[camCount]; 
                //outLine = new List<VisionImage>();
               
                for (int i = 0; i < camCount; i++)
                {
                    string camName = visionHelper.Camers[i].Name;
                    viewerForm[i] = new CamForm();
                    viewerForm[i].Show(dockPanel, DockState.DockTop);
                    viewerImage.Add(new VisionImage());
                    viewerForm[i].Image = viewerImage[i];
                    viewerForm[i].Tag = camName;
                    viewerForm[i].Text = camName;
                    viewerForm[i].Viewer.Tag = camName;
                    viewerForm[i].Viewer.ShowScrollbars = true;
                    //viewerForm[i].ShowImageViewerToolBar = true;
                    viewerForm[i].Buttons =ButtonViewer.JustRoiSet| ButtonViewer.PictureAdj|ButtonViewer.Seperator1|ButtonViewer.Seperator2;
                    viewerForm[i].Viewer.RoiChanged += new EventHandler<ContoursChangedEventArgs>(Viewer_RoiChanged);
                    

                    acq[i] = new Acquisition(viewerForm[i].Viewer, camName);
                    acq[i].startBackgroundWorker();
                    acq[i].OnGrabImage += new EventHandler(MdiForm_OnGrabImage);

                    // outline 처리용 buffer
                    outLine.Add(new VisionImage(ImageType.U8));
                    
                }
                //resultForm.Image[0] = outLine[1];



            }
            catch
            {
                MessageBox.Show("Image Screen 초기화에 실패하였습니다.");
            }*/
        }
        #endregion

        // 링크된 카메라로부터 1Frame 화면을 Grab하도록 명령을 내림
        void visionTrigger_OnTriggerActivate(object sender, EventArgs e)
        {
            //statusLedOn();

            //ResultSendingRoutine(yesterData);
            TriggerEventArgs arg = (TriggerEventArgs)e;
            ToolStripStatusLabel label1 = (ToolStripStatusLabel)statusStrip1.Items["labelSpeedMeter"];
            label1.Text = string.Format("{0,04}[msec]", arg.triggerInterval);
            ToolStripStatusLabel label2 = (ToolStripStatusLabel)statusStrip1.Items["labelTriggerMode"];
            label2.Text = arg.triggerSource.ToString();
        }
       
        // 1Frame 화면을 Grab하였음을 통지받음
        void MdiForm_OnGrabImage(object sender, EventArgs e)
        {
            GrabCompletedEventArgs arg = (GrabCompletedEventArgs)e;
            int fCount = arg.FrameCount;
            string cName = arg.CamName;
        }

        #region 선택한 Roi를 가지고 히스토그램을 그린다
        void DoRectangleContour(string viewerOwner,RectangleContour contour)
        {
            if (rectangleAction == RectangleAction.Histogram || rectangleAction == RectangleAction.Both)
            {
                CamForm viewerForm = cameras[viewerOwner].camForm;
                VisionImage extImage = new VisionImage(ImageType.Rgb32);
                Algorithms.Extract(viewerForm.Viewer.Image, extImage, viewerForm.Viewer.Roi);

                String ct = graphForm.colorMode;
                String sc = graphForm.selectedCam;
                ColorMode cm = new ColorMode();

                ImageProcessingParameters[] p = parameterFile.ProcessingParameter[viewerOwner];
                if (ct == "Image1") cm = p[0].colorParameter.colorMode;
                else if (ct == "Image2") cm = p[1].colorParameter.colorMode;
                else cm = parameterFile.ExtraData[sc].colorMode;
                
                ColorHistogramReport report = Algorithms.ColorHistogram(extImage, 256, cm);

                string[] legendName = new string[3];
                makeLegendName(ref legendName, cm);
                int[] array = new int[256];

                report.Plane1.Histogram.CopyTo(array, 0);
                graphForm.DrawGraph("plane1", legendName[0], array);

                report.Plane2.Histogram.CopyTo(array, 0);
                graphForm.DrawGraph("plane2", legendName[1], array);

                report.Plane3.Histogram.CopyTo(array, 0);
                graphForm.DrawGraph("plane3", legendName[2], array);
            }
        }

        void DoNonRectangleContour(Contour contour, string viewerOwner)
        {

            if (contour.Type != ContourType.Line) return;
            CalForm form = (CalForm)itmCalibration.Tag;
            if (form == null) return;
            form.Tag = (object)viewerOwner;
            form.Text = "Calibration[" + viewerOwner + "]";
            if (parameterFile.CalibrationFactor != null)
            {
                form.labelOutX.Text = string.Format("Cal Factor:{0:F5}[mm/pt]", parameterFile.CalibrationFactor[viewerOwner]);
                if(form.buttonCalDo.Tag==null) form.buttonCalDo.Tag = (object)parameterFile.CalibrationFactor[viewerOwner];
            }

            LineContour line = (LineContour)contour.Shape;
            Double X = line.End.X-line.Start.X;
            Double Y = line.End.Y-line.Start.Y;
            if (X < 0) X = -X;
            if (Y < 0) Y = -Y;
            form.labelXpt.Tag = (object)(new System.Numerics.Complex(X, Y)); 
            form.labelXpt.Text = X.ToString();
            form.labelYpt.Text = Y.ToString();
        }
        #endregion

        #region Get Image ROI
        void Viewer_RoiChanged(object sender, ContoursChangedEventArgs e)
        {
            ImageViewer viewer = (ImageViewer)sender;
            string viewerName = (string)viewer.Tag;
            if (viewerName == "") return;

            CamForm viewerForm = cameras[viewerName].camForm;
            graphForm.Text = "Graph[" + viewerName + "]";

            if (e.NewItems.Count == 0) return;
            Contour contour = viewerForm.Viewer.Roi.GetContour(0);

            if (contour.Type == ContourType.Rectangle)
            {
                if (rectangleAction == RectangleAction.Histogram || rectangleAction == RectangleAction.Both)
                    DoRectangleContour(viewerName, viewerForm.Viewer.Roi.GetBoundingRectangle());
                if (rectangleAction == RectangleAction.MakeRoi || rectangleAction == RectangleAction.Both)
                    ExtractRoi(viewerName, viewerForm.Viewer.Roi.GetBoundingRectangle());
            }
            else DoNonRectangleContour(contour, viewerName);
        }

        void ExtractRoi(string viewerOwner, RectangleContour contour)
        {
            ShowRoiWindow(viewerOwner, contour);
        }

        void ShowRoiWindow(string viewerOwner, RectangleContour contour)
        {
            frmRoiSet frm = new frmRoiSet();
            frm.Owner = viewerOwner;
            frm.SetContour(contour);
            frm.btnConfirm.Text = "추가";

            frm.cmbLane.Items.Clear();
            for (int i = 0; i < rootFile.Lane; i++) frm.cmbLane.Items.Add(((ImageLane)i).ToString());

            frm.cmbSequence.Items.Clear();
            for (int i = 0; i < rootFile.Carrier; i++) frm.cmbSequence.Items.Add(((ImageSequence)i).ToString());

            frm.cmbDirection.Items.Clear();
            for (int i = 0; i < 3; i++) frm.cmbDirection.Items.Add(((ImageDirection)i).ToString());

            frm.btnConfirm.Click += new EventHandler(btnConfirm_Click);
            frm.btnCancel.Click += new EventHandler(btnCancel_Click);
            frm.ShowDialog();
        }
        #region ROI설정창 버튼동작
        //Roi설정창 Cancel Button
        void btnCancel_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            frmRoiSet frm = (frmRoiSet)btn.Parent;
            frm.Close();
        }

        //Roi설정창 Confirm Button
        void btnConfirm_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            frmRoiSet frm = (frmRoiSet)btn.Parent;

            string cameraName = frm.Owner;
            double left = (double)frm.boxLeft.Value;
            double top = (double)frm.boxTop.Value;
            double width = (double)frm.boxRight.Value - (double)frm.boxLeft.Value;
            double height = (double)frm.boxBottom.Value - (double)frm.boxTop.Value;
            if (width < 0) width = 0;
            if (height < 0) height = 0;

            roiData.Update(new RoiData(cameraName, frm.GetLane(), frm.GetSequence(),new RectangleContour(left, top, width, height)));
            roiView.Update();
            frm.Close();
            parameterFile.roiData = roiData;
            mTree.SetRoiData(VisionFilePath.Process + cameraName + "::Roi", roiData.GetContainedData(cameraName));

        }
        #endregion

        #endregion

        #region 히스토그램의 범례설정
        void makeLegendName(ref string[] legendName, ColorMode mode)
        {
            switch (mode)
            {
                case ColorMode.CieLab:
                    legendName[0] = "CIE *L";
                    legendName[1] = "CIE *a";
                    legendName[2] = "CIE *b";
                    break;
                case ColorMode.CieXyz:
                    legendName[0] = "CIE X";
                    legendName[1] = "CIE y";
                    legendName[2] = "CIE z";
                    break;
                case ColorMode.Hsi:
                    legendName[0] = "Hue";
                    legendName[1] = "Saturation";
                    legendName[2] = "Intensity";
                    break;
                case ColorMode.Hsl:
                    legendName[0] = "Hue";
                    legendName[1] = "Saturation";
                    legendName[2] = "Luminence";
                    break;
                case ColorMode.Hsv:
                    legendName[0] = "Hue";
                    legendName[1] = "Saturation";
                    legendName[2] = "Value";
                    break;
                case ColorMode.Rgb:
                    legendName[0] = "Red";
                    legendName[1] = "Green";
                    legendName[2] = "Blue";
                    break;
            }
        }
        #endregion

        #region Static & Dynamic ROI Drawing
        //User Defined ROI를 그린다
        void DrwaOverlayRectangle(string cameraName)
        {
            if (userRoiDisplay == false) return;
            if (roiData == null) return;
            IEnumerable<RoiData> rois = roiData.GetContainedData(cameraName);
            if (rois == null) return;
            CamForm viewerForm = cameras[cameraName].camForm;
            foreach (RoiData roi in rois)
            {
                viewerForm.Image.Overlays.Default.AddRectangle(
                                new RectangleContour(roi.Left+roi.Offset.X, roi.Top+roi.Offset.Y, roi.Width, roi.Height),
                                Rgb32Value.GreenColor,
                                DrawingMode.DrawValue
                            );
                viewerForm.Image.Overlays.Default.AddText(
                                roi.Lane.ToString() + "," + roi.Sequence.ToString(),
                                new PointContour(roi.Left+roi.Offset.X, roi.Top+20),
                                new Rgb32Value(Color.White),
                                new OverlayTextOptions("Consolas", 14)
                               );
            }
        }

        void DrwaOverlayRectangle(string cameraName,CamForm viewerForm)
        {
            if (userRoiDisplay == false) return;
            if (roiData == null) return;
            IEnumerable<RoiData> rois = roiData.GetContainedData(cameraName);
            if (rois == null) return;
            foreach (RoiData roi in rois)
            {
                viewerForm.Image.Overlays.Default.AddRectangle(
                                new RectangleContour(roi.Left + roi.Offset.X, roi.Top + roi.Offset.Y, roi.Width, roi.Height),
                                Rgb32Value.GreenColor,
                                DrawingMode.DrawValue
                            );
                viewerForm.Image.Overlays.Default.AddText(
                                roi.Lane.ToString() + "," + roi.Sequence.ToString(),
                                new PointContour(roi.Left + roi.Offset.X, roi.Top+20),
                                new Rgb32Value(Color.White),
                                new OverlayTextOptions("Consolas", 14)
                               );
            }
        }

        RectangleContour modifyRectangle(ParticleReport report)
        {
            double left = report.CenterOfMass.X      - report.BoundingRect.Left;
            double right= report.BoundingRect.Width  + report.BoundingRect.Left-report.CenterOfMass.X;
            double up   = report.CenterOfMass.Y      - report.BoundingRect.Top;
            double down = report.BoundingRect.Height + report.BoundingRect.Top -report.CenterOfMass.Y;

            double cx   = left + report.BoundingRect.Width / 2;
            double cy   = Top + report.BoundingRect.Height / 2; 

            double x,y;
            bool x_left, y_up;

            if (left < right)
            {
                x = left;
                x_left = false;
            }
            else
            {
                x = right;
                x_left = true;
            }

            if (up < down)
            {
                y = up;
                y_up = false;
            }
            else
            {
                y = down;
                y_up = true;
            }

            double xdiff = System.Math.Abs(left - right);
            double ydiff = System.Math.Abs(up - down);

            double cxdiff = System.Math.Abs(cx - report.CenterOfMass.X);
            double cydiff = System.Math.Abs(cy - report.CenterOfMass.Y);

            double rx = xdiff*100 / x;
            double ry = ydiff*100 / y;

            RectangleContour con = new RectangleContour(); 
            con= report.BoundingRect;
 
            //if (rx > 5)
            {
                if (x_left == true)
                {
                    con.Left = report.CenterOfMass.X - x;
                    con.Width = 2*x;
                }
                else
                {
                    con.Left = report.CenterOfMass.X-x ;
                    con.Width = 2*x;
                }
            }
            
            //if (ry > 5)
            {
                if (y_up == true)
                {
                    con.Top = report.CenterOfMass.Y - y;
                    con.Height = 2*y;
                }
                else
                {
                    con.Top = report.CenterOfMass.Y-y ;
                    con.Height = 2*y;
                }
            }
            return con;
        }

        void DrawDynamicOverlayRectangle(string cameraName)
        {
            if (dynamicRoiDisplay == false) return;
            VisionImage image = cameras[cameraName].resultImage;

            for (int j = 0; j < rootFile.Lane; j++)
            {
                for (int i = 0; i < rootFile.Carrier; i++)
                {
                    RectangleContour r = new RectangleContour();
                    double fsize = cameras[cameraName].DisplayData.Size[j,i];
                    if (fsize <= 0) continue;
                    r = cameras[cameraName].DisplayData.RContour[j,i];

                    if (r != null) image.Overlays.Default.AddRectangle(r, Rgb32Value.RedColor);

                    PointContour p = new PointContour();
                    p = cameras[cameraName].DisplayData.PContour[j,i];

                    if (p != null)
                    {
                        image.Overlays.Default.AddLine(
                                                            new LineContour(new PointContour(p.X, p.Y - 10), new PointContour(p.X, p.Y + 10)),
                                                             Rgb32Value.RedColor
                                                       );

                        image.Overlays.Default.AddLine(
                                                            new LineContour(new PointContour(p.X - 10, p.Y), new PointContour(p.X + 10, p.Y)),
                                                            Rgb32Value.RedColor
                                                       );
                    }

                    if (r == null) continue;
                    double size = cameras[cameraName].DisplayData.Size[j,i];
                    double color = cameras[cameraName].DisplayData.Color[j,i];
                    double height = r.Height;
                    double width = r.Width;

                    double cr = size != 0 ? (100 * color) / size : 0;
                    if (cr > 100) cr = 100;
                    
                    double ar;

                    if (height == 0 || width == 0) ar = 0;
                    else ar = width > height ? 100 * height / width : 100 * width / height;

                    //Lane No,Carrier No,Size,Aspect Ratio,color area,color ratio
                    string text = string.Format("L:{0},N:{1},S:{2},AR:{3:F1}\nCA:{4},CR:{5:F1}",j+1,i,size,ar,color,cr);
                    image.Overlays.Default.AddText(
                                                        text,
                                                        new PointContour(r.Left, r.Top + r.Height),
                                                        new Rgb32Value(Color.White),
                                                        new OverlayTextOptions("Consolas",14)
                                                   );
                }
            }
        }

        void DrawDynamicOverlayRectangle(int index, string cameraName)
        {
            if (dynamicRoiDisplay == false) return;
            VisionImage image = cameras[cameraName].resultImage;
            Collection<ParticleReport> reports = cameras[cameraName].imageSpec.GetParticleReport(index);

            foreach (ParticleReport report in reports)
            {
                
                //Dynamic Rectangle
                image.Overlays.Default.AddRectangle(report.BoundingRect, Rgb32Value.RedColor);
                
                //Displaying Center of Mass
                image.Overlays.Default.AddLine(
                                                    new LineContour(
                                                                        new PointContour(report.CenterOfMass.X,report.CenterOfMass.Y-10),
                                                                        new PointContour(report.CenterOfMass.X,report.CenterOfMass.Y+10)
                                                                    ),
                                                     Rgb32Value.RedColor
                                               );

                image.Overlays.Default.AddLine(
                                                    new LineContour(
                                                                        new PointContour(report.CenterOfMass.X-10, report.CenterOfMass.Y),
                                                                        new PointContour(report.CenterOfMass.X+10, report.CenterOfMass.Y)
                                                                    ), 
                                                    Rgb32Value.RedColor
                                               );
                //Modified Rectangle
                
                //image.Overlays.Default.AddRectangle(modifyRectangle(report), Rgb32Value.GreenColor);
               
                double laxis,saxis;
                if(report.BoundingRect.Width>report.BoundingRect.Height)
                {
                    laxis = report.BoundingRect.Width;
                    saxis = report.BoundingRect.Height;
                }
                else
                {
                    saxis = report.BoundingRect.Width;
                    laxis = report.BoundingRect.Height;
                }
                double ratio = laxis!=0?100*saxis/laxis:0; 
               

                string text = string.Format("{0} : {1:F1}",report.Area,ratio);
                image.Overlays.Default.AddText(
                                                    text, 
                                                    new PointContour(report.BoundingRect.Left,report.BoundingRect.Top + report.BoundingRect.Height),
                                                    new Rgb32Value(Color.White),
                                                    new OverlayTextOptions("Consolas",15)

                                               );
            }
        }
        #endregion

        void showDisplayForm()
        {
            if (rootFile.Lane == 1) ResultForm.Width=208;
            //ResultForm.Left = Screen.PrimaryScreen.Bounds.Width - ResultForm.Width;
            //ResultForm.Top = Screen.PrimaryScreen.Bounds.Height - ResultForm.Height - statusStrip1.Height;
            ResultForm.Opacity = 0.8D;
            ResultForm.Show();
            ResultForm.buttonRB.PerformClick();
        }

        private void MdiForm_Shown(object sender, EventArgs e)
        {
            ResultForm = new DisplayResultForm();
            ResultForm.buttonLB.Click += new EventHandler(buttonLB_Click);
            ResultForm.buttonLU.Click += new EventHandler(buttonLU_Click);
            ResultForm.buttonRB.Click += new EventHandler(buttonRB_Click);
            ResultForm.buttonRU.Click += new EventHandler(buttonRU_Click);
            ResultForm.Left = Screen.PrimaryScreen.Bounds.Width - ResultForm.Width;
            ResultForm.Top = Screen.PrimaryScreen.Bounds.Height - ResultForm.Height;
            ResultForm.Show();
            ResultForm.Hide();
        }
        void buttonLU_Click(object sender, EventArgs e)
        {
            if (rootFile.Lane == 1) ResultForm.Width = 208;
            ResultForm.Left = Screen.PrimaryScreen.Bounds.Left;
            ResultForm.Top = Screen.PrimaryScreen.Bounds.Top + tool.Height + menu.Height+ 60;
            ResultForm.buttonLU.Focus();
        }
        void buttonRU_Click(object sender, EventArgs e)
        {
            if (rootFile.Lane == 1) ResultForm.Width = 208;
            ResultForm.Left = Screen.PrimaryScreen.Bounds.Width - ResultForm.Width;
            ResultForm.Top = Screen.PrimaryScreen.Bounds.Top + tool.Height+menu.Height + 60;
            ResultForm.buttonRU.Focus();
        }
        void buttonLB_Click(object sender, EventArgs e)
        {
            if (rootFile.Lane == 1) ResultForm.Width = 208;
            ResultForm.Left = Screen.PrimaryScreen.Bounds.Left;
            ResultForm.Top = Screen.PrimaryScreen.Bounds.Height - ResultForm.Height - statusStrip1.Height;
            ResultForm.buttonLB.Focus();
        }
        void buttonRB_Click(object sender, EventArgs e)
        {
            if (rootFile.Lane == 1) ResultForm.Width = 208;
            ResultForm.Left = Screen.PrimaryScreen.Bounds.Width - ResultForm.Width;
            ResultForm.Top = Screen.PrimaryScreen.Bounds.Height - ResultForm.Height - statusStrip1.Height;
            ResultForm.buttonRB.Focus();
        }

    }
}
