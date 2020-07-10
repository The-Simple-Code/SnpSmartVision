using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Numerics;

using NationalInstruments.Vision.Acquisition.Imaqdx;
using NationalInstruments.Vision;
using NationalInstruments.Vision.WindowsForms;
using NationalInstruments.Vision.Analysis;

using WeifenLuo.WinFormsUI.Docking;
using SnpSystem.Vision.Acquisition;
using SnpSystem.Vision.Viewer;
using SnpSystem.Vision.VisionConfigurationHelper;
using SnpSystem.Vision.Parameters;

namespace SnpSmartVision
{
    // Capture화면에 Overlay Display여부를 설정
    [Flags] public enum OverLayDisplayMode
    {
        None        = 1<<0,
        Roi         = 1<<1,
        Outline     = 1<<2,
        Color1      = 1<<3,
        Color2      = 1<<4,
        Color3      = 1<<5,
        Color12     = Color1|Color2,
        Color13     = Color1|Color3,
        Color23     = Color2|Color3,
        Color123    = Color1|Color2|Color3,
        RoiOutline  = Roi|Outline,
        RoiColor1   = Roi|Color1,
        RoiColor2   = Roi|Color2,
        RoiColor3   = Roi|Color3,
        RoiColor12  = Roi|Color12,
        RoiColor13  = Roi|Color13,
        RoiColor23  = Roi|Color23,
        RoiColor123 = Roi|Color123,
    }

    // Capture화면에서 Rectangle Contour를 설정한후
    //이후 동작을 정의
    [Flags]public enum RectangleAction
    {
        None            = 1<<0,                 //아무것도 안함
        Histogram       = 1<<1,                 //히스토그램을 그린다
        MakeRoi         = 1<<2,                 //Roi를 입력한다
        MakeCalibration = 1<<3,
        Both            = Histogram|MakeRoi,    //Roi와 히스토그램을 그린다.
    }

    public partial class MdiForm : Form
    {
        MenuStrip menu;
        ToolStrip tool;

        //ImageSelectionForm selectionForm=null;

        Dictionary<string, ToolStripMenuItem> itmDCamera;
        Dictionary<string, ToolStripMenuItem> itmDCameraAdd;
        Dictionary<string, ToolStripMenuItem> itmDCameraRemove;

        ToolStripMenuItem itmFile;
        ToolStripMenuItem itmNew;

        ToolStripMenuItem itmOpen;
        ToolStripMenuItem itmSave;
        ToolStripMenuItem itmSaveAs;
        ToolStripMenuItem itmClose;
        ToolStripMenuItem itmCamera;
        ToolStripMenuItem itmRun;
        ToolStripMenuItem itmStop;

        ToolStripMenuItem itmSetting;
        ToolStripMenuItem itmInterval;

        ToolStripMenuItem itmRoiOffset;
        ToolStripMenuItem itmRoi;
        ToolStripMenuItem itmShowHisto;
        ToolStripMenuItem itmCameraAttr;
        ToolStripMenuItem itmColor;
        ToolStripMenuItem itmCalibration;


        ToolStripMenuItem itmView;
        ToolStripMenuItem itmResult;
        ToolStripMenuItem itmHistogram;
        ToolStripMenuItem itmCameraList;
        ToolStripMenuItem itmCameraAttrView;
        ToolStripMenuItem itmAlgorithmAttrView;

        ToolStripMenuItem itmRoiUser;
        ToolStripMenuItem itmRoiDynamic;

        ToolStripMenuItem itmStopWatch;
        ToolStripMenuItem itmDebugger;

        ToolStripMenuItem itmControl;

        ToolStripMenuItem itmWindow;
        ToolStripMenuItem itmCascade;

        ToolStripMenuItem itmHelp;
        ToolStripMenuItem itmInformation;

        ToolStripButton toolClose;
        ToolStripButton toolSave;
        ToolStripButton toolStop;
        ToolStripButton toolRun;
        ToolStripButton toolHisto;
        ToolStripButton toolColor;
        ToolStripButton toolSettings;
        ToolStripButton toolRoi;
        ToolStripButton toolResult;

        ToolStripButton toolLock;
        ToolStripButton toolUnlock;

        ToolStripButton toolOne;
        ToolStripButton toolTwo;
        ToolStripButton toolThree;
        ToolStripButton toolBlank;

        void UncheckedSettingMenu()
        {
            itmRoi.Checked=false;
            itmShowHisto.Checked=false;
            itmCameraAttr.Checked=false;
        }
        
        void initMenu()
        {

            #region Main Menu
            menu = new MenuStrip();
            menu.BackColor = System.Drawing.SystemColors.InactiveCaption;
            menu.GripStyle = ToolStripGripStyle.Hidden;
            menu.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            menu.AutoSize = true;
            menu.Dock = DockStyle.Top;
            menu.Parent = this;
            #endregion

            #region File
            itmFile = new ToolStripMenuItem();
            itmFile.Image = new Bitmap(Properties.Resources.mnFile);
            itmFile.Text = "파일";
            menu.Items.Add(itmFile);

            itmOpen = new ToolStripMenuItem();
            itmOpen.Image = new Bitmap(Properties.Resources.mnOpen);
            itmOpen.Text = "열기";
            itmOpen.Click += new EventHandler(itmOpen_Click);
            itmFile.DropDownItems.Add(itmOpen);

            ToolStripSeparator s11 = new ToolStripSeparator();
            itmFile.DropDownItems.Add(s11);

            itmNew = new ToolStripMenuItem();
            itmNew.Text = "새로만들기";
            itmNew.Image = new Bitmap(Properties.Resources.mnNew);
            itmNew.Click += new EventHandler(itmNew_Click);
            itmFile.DropDownItems.Add(itmNew);

            ToolStripSeparator s12 = new ToolStripSeparator();
            itmFile.DropDownItems.Add(s12);

            itmSave = new ToolStripMenuItem();
            itmSave.Text = "저장";
            itmSave.Image = new Bitmap(Properties.Resources.mnSave);
            itmSave.Click += new EventHandler(itmSave_Click);
            itmFile.DropDownItems.Add(itmSave);

            itmSaveAs = new ToolStripMenuItem();
            itmSaveAs.Text = "다른이름으로 저장";
            itmSaveAs.Image = new Bitmap(Properties.Resources.mnSaveAs);
            itmSaveAs.Click += new EventHandler(itmSaveAs_Click);
            itmFile.DropDownItems.Add(itmSaveAs);

            ToolStripSeparator s13 = new ToolStripSeparator();
            itmFile.DropDownItems.Add(s13);
            itmClose = new ToolStripMenuItem();
            itmClose.Image = new Bitmap(Properties.Resources.mnExit);
            itmClose.Text = "종료";
            itmClose.Click += new EventHandler(itmClose_Click);
            itmFile.DropDownItems.Add(itmClose);
           

            /*설정파일읽기
             *설정파일저장 
             */ 
            #endregion

            #region Cameras
            itmCamera = new ToolStripMenuItem();
            itmCamera.Image = new Bitmap(Properties.Resources.camera1);
            itmCamera.Text = "카메라";
            menu.Items.Add(itmCamera);

            /*ToolStripMenuItem itmSearch = new ToolStripMenuItem();
            itmSearch.Text = "카메라 찾기";
            itmCamera.DropDownItems.Add(itmSearch);
            itmSearch.Click += new EventHandler(itmSearch_Click);*/

            itmDCamera = new Dictionary<string, ToolStripMenuItem>();
            itmDCameraAdd = new Dictionary<string, ToolStripMenuItem>();
            itmDCameraRemove = new Dictionary<string, ToolStripMenuItem>();
            #endregion

            #region Setting
            itmSetting = new ToolStripMenuItem();
            itmSetting.Text = "설정";
            itmSetting.Image = new Bitmap(Properties.Resources.mnSettings);
            menu.Items.Add(itmSetting);

            itmInterval = new ToolStripMenuItem();
            itmInterval.Text = "타이머주기 설정";
            itmInterval.Image = new Bitmap(Properties.Resources.IntTrigger);
            itmSetting.DropDownItems.Add(itmInterval);
            itmInterval.Click += new EventHandler(itmInterval_Click);

            itmRoiOffset = new ToolStripMenuItem();
            itmRoiOffset.Text = "Roi옵셋설정";
            itmRoiOffset.Image = new Bitmap(Properties.Resources.mnRoiOffset);
            itmSetting.DropDownItems.Add(itmRoiOffset);
            itmRoiOffset.Click += new EventHandler(itmRoiOffset_Click);

            itmRoi = new ToolStripMenuItem();
            itmRoi.Text = "Roi설정";
            itmRoi.Image = new Bitmap(Properties.Resources.mnRoi);
            itmSetting.DropDownItems.Add(itmRoi);
            itmRoi.Click += new EventHandler(itmRoi_Click);

            itmShowHisto = new ToolStripMenuItem();
            itmShowHisto.Text = "색상스펙트럼설정";
            itmShowHisto.Image = new Bitmap(Properties.Resources.mnHisto);
            itmSetting.DropDownItems.Add(itmShowHisto);
            itmShowHisto.Click += new EventHandler(itmShowHisto_Click);

            itmCameraAttr = new ToolStripMenuItem();
            itmCameraAttr.Text = "카메라속성설정";
            itmCameraAttr.Image = new Bitmap(Properties.Resources.mnCameraSetting);
            itmSetting.DropDownItems.Add(itmCameraAttr);
            itmCameraAttr.Click += new EventHandler(itmCameraAttr_Click);

            itmColor = new ToolStripMenuItem();
            itmColor.Text = "기준색상설정";
            itmColor.Image = new Bitmap(Properties.Resources.mnColor);
            itmColor.Click += new EventHandler(itmColor_Click);
            itmSetting.DropDownItems.Add(itmColor);

            itmCalibration = new ToolStripMenuItem();
            itmCalibration.Text = "캘리브레이션";
            itmCalibration.Image = new Bitmap(Properties.Resources.mnCali);
            itmCalibration.Click += new EventHandler(itmCalibration_Click);
            itmSetting.DropDownItems.Add(itmCalibration);

            #endregion

            /* 실행 sub menu 
                       트리거    카메라추가/제거    booting후 동작조건
               선별     외부         불가           모든설정에 문제가 없으면 바로 선별모드로 들어감
               시험     내부         불가           선별/정지모드에서 명시적으로 시험모드를 선택하여 들어감 
               정지     내부         가능           설정에 문제가 있거나 선별/시험모드에서 명시적으로 선택하여 들어감
            */

            #region Control
            itmControl = new ToolStripMenuItem();
            itmControl.Text = "실행";
            itmControl.Image = new Bitmap(Properties.Resources.mnRun);
            menu.Items.Add(itmControl);

            itmRun = new ToolStripMenuItem();
            itmRun.Text = "선별";
            itmRun.Image = new Bitmap(Properties.Resources.mnStart);
            itmRun.Enabled = true;
            itmRun.Click += new EventHandler(itmRun_Click);
            itmControl.DropDownItems.Add(itmRun);
            /*
            itmTest = new ToolStripMenuItem();
            itmTest.Text = "시험";
            itmTest.Enabled = false;
            itmControl.DropDownItems.Add(itmTest);
            */
            itmStop = new ToolStripMenuItem();
            itmStop.Text = "정지";
            itmStop.Image = new Bitmap(Properties.Resources.mnStop);
            itmStop.Enabled = false;
            itmStop.Click += new EventHandler(itmStop_Click);
            itmControl.DropDownItems.Add(itmStop);
            #endregion

            #region View
            itmView = new ToolStripMenuItem();
            itmView.Text = "보기";
            itmView.Image = new Bitmap(Properties.Resources.mnShow);
            menu.Items.Add(itmView);

            itmResult = new ToolStripMenuItem();
            itmResult.Text = "선별결과보기";
            itmResult.Image = new Bitmap(Properties.Resources.mnResult);
            itmResult.Click += new EventHandler(itmResult_Click);
            itmView.DropDownItems.Add(itmResult);

            ToolStripSeparator s31 = new ToolStripSeparator();
            itmView.DropDownItems.Add(s31);

            itmHistogram = new ToolStripMenuItem();
            itmHistogram.Text = "히스토그램 챠트보기";
            itmHistogram.Image = new Bitmap(Properties.Resources.mnHisto);
            itmHistogram.Click += new EventHandler(itmHistogram_Click);
            itmView.DropDownItems.Add(itmHistogram);

            itmCameraList = new ToolStripMenuItem();
            itmCameraList.Text = "카메라목록 보기";
            itmCameraList.Image = new Bitmap(Properties.Resources.mnCameraList);
            itmCameraList.Click += new EventHandler(itmCameraList_Click);
            itmView.DropDownItems.Add(itmCameraList);

            itmCameraAttrView = new ToolStripMenuItem();
            itmCameraAttrView.Text = "카메라속성 보기";
            itmCameraAttrView.Image = new Bitmap(Properties.Resources.mnCameraInfo);
            itmCameraAttrView.Click += new EventHandler(itmCameraAttrView_Click);
            itmCameraAttrView.Enabled = false;
            itmView.DropDownItems.Add(itmCameraAttrView);

            itmAlgorithmAttrView = new ToolStripMenuItem();
            itmAlgorithmAttrView.Text = "화상처리정보 보기";
            itmAlgorithmAttrView.Image = new Bitmap(Properties.Resources.Color1);
            itmAlgorithmAttrView.Click += new EventHandler(itmAlgorithmAttrView_Click);
            itmAlgorithmAttrView.Enabled = true;
            itmView.DropDownItems.Add(itmAlgorithmAttrView);

            ToolStripSeparator s3 = new ToolStripSeparator();
            itmView.DropDownItems.Add(s3);

            itmRoiUser = new ToolStripMenuItem();
            itmRoiUser.Text = "사용자정의 ROI보기";
            itmRoiUser.Image = new Bitmap(Properties.Resources.mnUser);
            itmRoiUser.Click += new EventHandler(itmRoiUser_Click);
            itmView.DropDownItems.Add(itmRoiUser);

            itmRoiDynamic = new ToolStripMenuItem();
            itmRoiDynamic.Text = "다이나믹 ROI보기";
            itmRoiDynamic.Image = new Bitmap(Properties.Resources.mnDynamic);
            itmRoiDynamic.Click += new EventHandler(itmRoiDynamic_Click);
            itmView.DropDownItems.Add(itmRoiDynamic);

            ToolStripSeparator s4 = new ToolStripSeparator();
            itmView.DropDownItems.Add(s4);

            itmStopWatch = new ToolStripMenuItem();
            itmStopWatch.Text = "Stop Watch";
            itmStopWatch.Image = new Bitmap(Properties.Resources.mnClock);
            itmStopWatch.Click += new EventHandler(itmStopWatch_Click);
            itmView.DropDownItems.Add(itmStopWatch);

            itmDebugger = new ToolStripMenuItem();
            itmDebugger.Text = "Debugger";
            itmDebugger.Image = new Bitmap(Properties.Resources.mnFind);
            itmDebugger.Click += new EventHandler(itmDebugger_Click);
            itmView.DropDownItems.Add(itmDebugger);
            #endregion

            #region Window
            itmWindow = new ToolStripMenuItem();
            itmWindow.Text = "창";
            itmWindow.Image = new Bitmap(Properties.Resources.mnWindow);
            menu.Items.Add(itmWindow);
            #endregion

            #region Cascade
            itmCascade = new ToolStripMenuItem();
            itmCascade.Text = "균등정리";
            itmCascade.Image = new Bitmap(Properties.Resources.mnCascade);
            itmCascade.Click += new EventHandler(itmCascade_Click);
            itmWindow.DropDownItems.Add(itmCascade);
            #endregion


            #region Help
            itmHelp = new ToolStripMenuItem();
            itmHelp.Text = "도움말";
            itmHelp.Image = new Bitmap(Properties.Resources.mnHelp);
            menu.Items.Add(itmHelp);

            itmInformation = new ToolStripMenuItem();
            itmInformation.Text= "프로그램정보";
            itmInformation.Image = new Bitmap(Properties.Resources.mnInfo);
            itmInformation.Click += new EventHandler(itmInformation_Click);
            itmHelp.DropDownItems.Add(itmInformation);
            #endregion
        }

        void itmCascade_Click(object sender, EventArgs e)
        {
           /* Size size = this.ClientSize;
            int sizeW = size.Width;
            int sizeH = size.Height;
            cameras["cam0"].camForm.Width = size.Width;
            cameras["cam0"].resultForm.Width = size.Width;
            cameras["cam0"].camForm.Height = size.Height / 2;
            cameras["cam0"].resultForm.Height = size.Height / 2;*/

        }

        void itmRun_Click(object sender, EventArgs e)
        {
            toolRun.PerformClick();
        }

        void itmStop_Click(object sender, EventArgs e)
        {
            toolStop.PerformClick();
        }

        void itmInformation_Click(object sender, EventArgs e)
        {
            AboutForm f = new AboutForm();
            f.label2.Text = Version;
            f.StartPosition = FormStartPosition.CenterScreen;
            f.Show();
        }

        void itmInterval_Click(object sender, EventArgs e)
        {
            IntervalForm form = new IntervalForm();
            if (rootFile.InternalTriggerPeriod == 0) form.knob1.Value=1;
            else form.knob1.Value = (double)1000 / rootFile.InternalTriggerPeriod;
            form.knob1.ValueChanged += new EventHandler(knob1_ValueChanged);
            form.buttonIntervalExit.Click += new EventHandler(buttonIntervalExit_Click);
            form.buttonUp.Click += new EventHandler(buttonUp_Click);
            form.buttonDown.Click += new EventHandler(buttonDown_Click);
            form.Show();

        }

        void buttonDown_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            IntervalForm form = (IntervalForm)b.Parent;
            double val = form.knob1.Value;
            if (form.checkBox1.Checked) val -= 0.1;
            else   val -= 0.01;
            if (val < 1) val = 1;
            form.knob1.Value = val;

        }

        void buttonUp_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            IntervalForm form = (IntervalForm)b.Parent;
            double val = form.knob1.Value;
            if (form.checkBox1.Checked) val += 0.1;
            else  val += 0.01;
            if (val >10) val = 10;
            form.knob1.Value = val;
        }
        void buttonIntervalExit_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            IntervalForm form = (IntervalForm)b.Parent;
            form.Close();
        }

        void knob1_ValueChanged(object sender, EventArgs e)
        {
            NationalInstruments.UI.WindowsForms.Knob k = (NationalInstruments.UI.WindowsForms.Knob)sender;
            IntervalForm form = (IntervalForm)k.Parent; 
            double t = 1000.0/k.Value;
             form.label1.Text = string.Format("T={0}msec", t);
             rootFile.InternalTriggerPeriod = (int)t;
             visionTrigger.timerInterval = (int)t;
        }

        void itmRoiOffset_Click(object sender, EventArgs e)
        {
            roiOffsetForm = new RoiOffsetForm();
            roiOffsetForm.buttonRoiOffsetCancel.Click += new EventHandler(buttonRoiOffsetCancel_Click);
            roiOffsetForm.buttonRoiOffsetOk.Click += new EventHandler(buttonRoiOffsetOk_Click);
           
            roiOffsetForm.comboBox1.Items.Clear();
            foreach (string cam in cameras.Keys) roiOffsetForm.comboBox1.Items.Add(cam);
            roiOffsetForm.comboBox1.Text = (string)roiOffsetForm.comboBox1.Items[0];
            Point point = roiData.GetRoiOffset(roiOffsetForm.comboBox1.Text);
            roiOffsetForm.numericEdit1.Value = (double)point.X;
            roiOffsetForm.numericEdit2.Value = (double)point.Y;
            roiOffsetForm.comboBox1.SelectedValueChanged += new EventHandler(comboBox1_SelectedValueChanged);
            roiOffsetForm.numericEdit1.ValueChanged += new EventHandler(numericEdit1_ValueChanged);
            roiOffsetForm.numericEdit2.ValueChanged += new EventHandler(numericEdit1_ValueChanged);
            roiOffsetForm.Show();
        }

        void numericEdit1_ValueChanged(object sender, EventArgs e)
        {
            string s = roiOffsetForm.comboBox1.Text;
            if (s != "")
            {
                int x = (int)roiOffsetForm.numericEdit1.Value;
                int y = (int)roiOffsetForm.numericEdit2.Value;
                roiData.SetRoiOffset(s, new Point(x, y));
            }
        }

        void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            Point point = roiData.GetRoiOffset(roiOffsetForm.comboBox1.Text);
            roiOffsetForm.numericEdit1.Value = (double)point.X;
            roiOffsetForm.numericEdit2.Value = (double)point.Y;
        }

        void buttonRoiOffsetOk_Click(object sender, EventArgs e)
        {
            string s = roiOffsetForm.comboBox1.Text;
            if (s != "")
            {
                int x = (int)roiOffsetForm.numericEdit1.Value;
                int y = (int)roiOffsetForm.numericEdit2.Value;
                roiData.SetRoiOffset(s, new Point(x,y));
            }
   
        }

        void buttonRoiOffsetCancel_Click(object sender, EventArgs e)
        {
            roiOffsetForm.Close();   
        }

        void itmDebugger_Click(object sender, EventArgs e)
        {
            List<string> cameraList = new List<string>();
            foreach (string name in cameras.Keys) cameraList.Add(name);
            DebuggerForm = new Debugger(cameraList.ToArray());
            DebuggerForm.Show();
        }

        void itmResult_Click(object sender, EventArgs e)
        {
            itmResult.Checked = !itmResult.Checked;
            if (itmResult.Checked) showDisplayForm();
            else ResultForm.Hide();
        }

        void itmStopWatch_Click(object sender, EventArgs e)
        {

            timeTable = new TimeTableForm();
            timeTable.buttonExit.Click += new EventHandler(buttonTimeTableExit_Click);
            timeTable.buttonMeas.Click += new EventHandler(buttonTimeTableMeas_Click);
            timeTable.InitTimeTable(parameterFile.CameraList);
            timeTable.Show();
        }

        void itmColor_Click(object sender, EventArgs e)
        {

            ImageSelectionForm selectionForm    = new ImageSelectionForm();
            selectionForm.buttonOk.Click        += new EventHandler(buttonOk_Click);
            selectionForm.buttonCancel.Click    += new EventHandler(buttonCancel_Click);
            selectionForm.comboCamera.Items.Clear();
            selectionForm.Tag = 0;
            foreach(string cam in parameterFile.CameraList)
            {
                selectionForm.comboCamera.Items.Add(cam);
            }
            selectionForm.comboCamera.Text = (string)selectionForm.comboCamera.Items[0];

            selectionForm.comboImage.Items.Clear();
            for (int i = 1; i <= rootFile.Count; i++)
            {
                selectionForm.comboImage.Items.Add("Image" + i.ToString());
            }
            selectionForm.comboImage.Items.Add("ExtraImage");
            selectionForm.comboImage.Text = (string)selectionForm.comboImage.Items[0];
            selectionForm.Show();
        }

        void itmHistogram_Click(object sender, EventArgs e)
        {
            if (graphForm.IsHidden == false)
            {
                graphForm.Hide();
                itmHistogram.Checked = false;
                return;
            }
               
            ImageSelectionForm selectionForm = new ImageSelectionForm();
            selectionForm.buttonOk.Click += new EventHandler(buttonOkForHisto_Click);
            selectionForm.buttonCancel.Click += new EventHandler(buttonCancelForHisto_Click);
            selectionForm.comboCamera.Items.Clear();
            selectionForm.Tag = 1;

            foreach (string cam in parameterFile.CameraList)
            {
                selectionForm.comboCamera.Items.Add(cam);
            }
            selectionForm.comboCamera.Text = (string)selectionForm.comboCamera.Items[0];

            selectionForm.comboImage.Items.Clear();
            for (int i = 1; i <= rootFile.Count; i++)
            {
                selectionForm.comboImage.Items.Add("Image" + i.ToString());
            }
            selectionForm.comboImage.Items.Add("ExtraImage");
            selectionForm.comboImage.Text = (string)selectionForm.comboImage.Items[0];
            selectionForm.Show();
        }


        void buttonCancel_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ImageSelectionForm selectionForm = (ImageSelectionForm)btn.FindForm();
            selectionForm.Close();
        }

        void buttonCancelForHisto_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ImageSelectionForm selectionForm = (ImageSelectionForm)btn.FindForm();
            selectionForm.Close();
        }

        void buttonOkForHisto_Click(object sender, EventArgs e)
        {
            itmHistogram.Checked = true;
            Button btn = (Button)sender;
            ImageSelectionForm selectionForm = (ImageSelectionForm)btn.FindForm();
            graphForm.colorMode = selectionForm.comboImage.Text;
            graphForm.selectedCam = selectionForm.comboCamera.Text;
            graphForm.Show();
            selectionForm.Close();
        }

        void buttonOk_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ImageSelectionForm selectionForm = (ImageSelectionForm)btn.FindForm();
            string camera = selectionForm.comboCamera.Text;
            if (camera == "") return;
            string image = selectionForm.comboImage.Text;
            string root;
            ColorThresholdParameters color;
            string path;
            TreeDataCheck h;

            int index;
            if (image == "Image1")
            {
                index = 0;
                root = "\\Image Processing Parameter1";
            }
            else if (image == "Image2")
            {
                index = 1;
                root = "\\Image Processing Parameter2";
            }
            else if (image == "Image3")
            {
                index = 2;
                root = "\\Image Processing Parameter3";
            }
            else if (image == "ExtraImage")
            {
                root = "\\Extra Processing Parameter";
                index = 3;
            }
            else return;
            
            algorithmAttrForm.SelectedIndex = index;
            if (index < 3)
            {
                ImageProcessingParameters[] para = parameterFile.ProcessingParameter[camera];
                color = para[index].colorParameter;
                path = "화상처리정보\\" + camera + root + "\\Color Threshold\\Image Type\\" + color.colorMode.ToString();
                h = new TreeDataCheck(path);
 
                if (itmAlgorithmAttrView.Checked == false) itmAlgorithmAttrView.PerformClick();
                pageChange = true;
                
                algorithmAttrForm.tabControl.Tag = (object)para[index];
                algorithmAttrForm.comboImageType.Text = para[index].colorParameter.colorMode.ToString();
                algorithmAttrForm.tabControl.TabPages[1].Tag = (object)path;
                algorithmAttrForm.showTab(algorithmAttrForm.tabControl.TabPages[1]);
                algorithmAttrForm.buttonColor.PerformClick();
            }
            else
            {
                color = (ColorThresholdParameters)parameterFile.ExtraData[camera];
                path = "화상처리정보\\" + camera + root + "\\Color Threshold\\Enabled\\" + color.colorMode.ToString();
                h = new TreeDataCheck(path);

                if (itmAlgorithmAttrView.Checked == false) itmAlgorithmAttrView.PerformClick();
                pageChange = true;

                algorithmAttrForm.tabControl.Tag = (object)parameterFile.ExtraData[camera];
                algorithmAttrForm.tabControl.TabPages[8].Tag = (object)path;
                algorithmAttrForm.checkEnabled.Checked = parameterFile.ExtraData[camera].Enable;
                algorithmAttrForm.comboCombined.Text = parameterFile.ExtraData[camera].CombinedImage.ToString();
                algorithmAttrForm.comboContained.Text = parameterFile.ExtraData[camera].ContainedMode.ToString();
                algorithmAttrForm.showTab(algorithmAttrForm.tabControl.TabPages[8]);
                algorithmAttrForm.buttonXColor.PerformClick();
            }
            
            selectionForm.Close();
        }

        void itmNew_Click(object sender, EventArgs e)
        {
            deleteAllImageViewForm();
        }

        void itmSave_Click(object sender, EventArgs e)
        {
            if (rootFile.Root == "") itmSaveAs.PerformClick();
            else
            {
                parameterFile.WriteFile(rootFile.Root);
                rootFile.Root = parameterFile.fileName;
                rootFile.Write();
            }
        }

        void itmSaveAs_Click(object sender, EventArgs e)
        {
            parameterFile.WriteFileAs();
            rootFile.Root = parameterFile.fileName;
            rootFile.Write();
            addConfigFileStatus();
        }

        void itmAlgorithmAttrView_Click(object sender, EventArgs e)
        {
            itmAlgorithmAttrView.Checked = !itmAlgorithmAttrView.Checked;
            if (itmAlgorithmAttrView.Checked == true) algorithmAttrForm.Show();
            else algorithmAttrForm.Hide();
        }

        void itmOpen_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(
            ImageParameterFile newFile = new ImageParameterFile();
            newFile = parameterFile.ReadFileAs();
            DialogResult result = new System.Windows.Forms.DialogResult();

            result = MessageBox.Show("새로운 설정파일 " + newFile.fileName + "을 읽어왔습니다.\n설정을 적용하려면 프로그램을 다시 시작해야 합니다.\n다시 시작할까요?","알림",

                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                rootFile.Root = newFile.fileName;  //parameterFile.fileName;
                rootFile.Write();
                //itmClose.PerformClick();
                if (serialOut != null) serialOut.PortClose();
                if (controlPort != null) controlPort.PortClose();
                closeAllImageViewForm();
                visionTrigger.Dispose();

                Application.Restart();

            }
        }

        void colorXRangeForm_ValueChanged(object sender, EventArgs e)
        {
            if (e is ColorRangeChangedArg)
            {
                ColorRangeChangedArg a = e as ColorRangeChangedArg;
                ColorThresholdParameters para = (ColorThresholdParameters)algorithmAttrForm.tabControl.Tag;
                para.range["plane1"] = a.Value["plane1"];
                para.range["plane2"] = a.Value["plane2"];
                para.range["plane3"] = a.Value["plane3"];
                mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[8].Tag, rootFile.Count, parameterFile);
            }
        }

        void colorRangeForm_ValueChanged(object sender, EventArgs e)
        {
            if (e is ColorRangeChangedArg)
            {
                ColorRangeChangedArg a = e as ColorRangeChangedArg;
                ImageProcessingParameters imageParameter = (ImageProcessingParameters)algorithmAttrForm.tabControl.Tag;
                ColorThresholdParameters para = imageParameter.colorParameter;
                para.range["plane1"] = a.Value["plane1"];
                para.range["plane2"] = a.Value["plane2"];
                para.range["plane3"] = a.Value["plane3"];
                mTree.UpdateCamTree((string)algorithmAttrForm.tabControl.TabPages[1].Tag, rootFile.Count, parameterFile);
            }
        }

        void itmRoiDynamic_Click(object sender, EventArgs e)
        {
            itmRoiDynamic.Checked = !itmRoiDynamic.Checked;
            if (itmRoiDynamic.Checked == true)
            {
                //itmRoiUser.Checked = false;
                dynamicRoiDisplay = true;
            }
            else
            {
                dynamicRoiDisplay = false;
            }
        }

        void itmRoiUser_Click(object sender, EventArgs e)
        {
            itmRoiUser.Checked = !itmRoiUser.Checked;
            if (itmRoiUser.Checked == true)
            {
                //itmRoiDynamic.Checked = false;
                userRoiDisplay = true;
            }
            else
            {
                userRoiDisplay = false;
            }
        }
        void itmSearch_Click(object sender, EventArgs e)
        {
            camAttrForm.SetCameraInformation();
            addCameras(camAttrForm.CamInfo);
        }

        void itmCameraAttrView_Click(object sender, EventArgs e)
        {

            foreach (string key in cameras.Keys)
            {
                if (cameras[key].attrForm.IsHidden)
                {
                    cameras[key].attrForm.Show();
                    itmCameraAttrView.Checked = true;
                }
                else
                {
                    cameras[key].attrForm.Hide();
                    itmCameraAttrView.Checked = false;
                }
            }
        }

        void itmCameraList_Click(object sender, EventArgs e)
        {
            if (camAttrForm.IsHidden) camAttrForm.Show();
            else camAttrForm.Hide();
            itmCameraList.Checked = !camAttrForm.IsHidden;
        }
                
        public void ItemConnection(string name,bool conn)
        {
                itmDCameraAdd[name].Enabled = conn;
                itmDCameraRemove[name].Enabled = !conn;
        }

        void initViewerButtons()
        {
            ButtonViewer btn = ButtonViewer.PictureAdj;
            foreach (string key in cameras.Keys)
            {
                if (itmRoi.Checked || itmShowHisto.Checked) btn |= ButtonViewer.JustRoiSet;
                else if (itmCalibration.Checked) btn |= ButtonViewer.Calibration; 
                if (itmCameraAttr.Checked) btn |= ButtonViewer.SetAttr;
                cameras[key].camForm.Buttons = btn;
            }
        }

        //Roi설정
        void itmRoi_Click(object sender, EventArgs e)
        {
            itmRoi.Checked = !itmRoi.Checked;
            if (itmRoi.Checked == true)
            {
                if (itmCalibration.Checked == true) itmCalibration.PerformClick();
                
                if (itmShowHisto.Checked == true) rectangleAction = RectangleAction.Both;
                else rectangleAction = RectangleAction.MakeRoi;

                itmRoiUser.Checked = true;
                itmRoiDynamic.Checked = false;
                userRoiDisplay = true;

                roiView = new RoiListView(roiData.roiData);
                roiView.itmCopy.Click += new EventHandler(itmCopy_Click);
                roiView.itmModify.Click += new EventHandler(itmModify_Click);
                roiView.itmDelete.Click += new EventHandler(itmDelete_Click);
                roiView.itmClose.Click +=new EventHandler(itmRoiClose_Click);
                roiView.Show();
                roiView.Update();
            }
            else
            {
                if (itmShowHisto.Checked == true) rectangleAction = RectangleAction.Histogram;
                else rectangleAction = RectangleAction.None;
                itmRoiUser.Checked = false;
                userRoiDisplay = false;
                roiView.Close();
            }
            initViewerButtons();
        }

        #region Roi ListView Popup Menu
        void itmDelete_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = roiView.listView1.SelectedItems;
            string cameraName=null;
            foreach (ListViewItem item in items)
            {
                cameraName = item.SubItems[0].Text;
                roiData.Remove(
                        item.SubItems[0].Text, 
                        EnumConv.ToLane(item.SubItems[1].Text), 
                        EnumConv.ToSequence(item.SubItems[2].Text)
                        );
                roiView.Update();
            }
            mTree.SetRoiData(cameraName + "::Roi", roiData.GetContainedData(cameraName));
        }
        
        void itmRoiClose_Click(object sender, EventArgs e)
        {
            if (itmRoi.Checked == true) itmRoi.PerformClick();

        }

        void itmModify_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = roiView.listView1.SelectedItems;
            if (items.Count != 1) return;
            ListViewItem item = items[0];
            frmRoiSet frm = new frmRoiSet();

            frm.Owner = item.SubItems[0].Text;
            frm.cmbLane.Text = item.SubItems[1].Text;
            frm.cmbSequence.Text = item.SubItems[2].Text;
            frm.boxLeft.Value = Convert.ToInt32(item.SubItems[3].Text);
            frm.boxTop.Value = Convert.ToInt32(item.SubItems[4].Text);
            frm.boxRight.Value = Convert.ToInt32(item.SubItems[5].Text);
            frm.boxBottom.Value = Convert.ToInt32(item.SubItems[6].Text);

            frm.cmbLane.Enabled = false;
            frm.cmbSequence.Enabled = false;
            frm.Text = item.SubItems[0].Text+" ROI[수정]"; 
            frm.btnConfirm.Text = "수정";
            frm.btnConfirm.Click += new EventHandler(btnModify_Click); 
            frm.btnCancel.Click += new EventHandler(btnModifyCancel_Click);
            frm.ShowDialog();
        }
        
        void btnModify_Click(object sender, EventArgs e)
        {
            Button  btn = (Button)sender;
            frmRoiSet frm = (frmRoiSet)btn.Parent;
            string cameraName = frm.Owner;
            double left = (double)frm.boxLeft.Value;
            double top = (double)frm.boxTop.Value;
            double width = (double)frm.boxRight.Value - (double)frm.boxLeft.Value;
            double height = (double)frm.boxBottom.Value - (double)frm.boxTop.Value;
            if (width < 0) width = 0;
            if (height < 0) height = 0;

            roiData.Update(new RoiData(cameraName, frm.GetLane(), frm.GetSequence(), new RectangleContour(left, top, width, height)));
            roiView.Update();
            mTree.SetRoiData(cameraName + "\\Roi", roiData.GetContainedData(cameraName));
            frm.Close();
        }

        void btnModifyCancel_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            frmRoiSet frm = (frmRoiSet)btn.Parent;
            frm.Close();
        }

        void itmCopy_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = roiView.listView1.SelectedItems;
            if (items.Count != 1) return;
            ListViewItem item = items[0];

            string cameraName = item.SubItems[0].Text;
            string lane = item.SubItems[1].Text;
            string sequence = item.SubItems[2].Text;

            int iSeq = EnumConv.ToIntSequence(sequence);
            iSeq++;
            if (iSeq > 9) return;

            string dir = item.SubItems[3].Text;
            
            ImageLane imageLane = EnumConv.ToLane(lane);
            ImageSequence imageSeq = EnumConv.ToSequence(iSeq);
            ImageDirection imageDir = EnumConv.ToDirection(dir);

            int left = Convert.ToInt32(item.SubItems[3].Text);
            int top = Convert.ToInt32(item.SubItems[4].Text);
            int right = Convert.ToInt32(item.SubItems[5].Text);
            int bottom = Convert.ToInt32(item.SubItems[6].Text);
            int width = Convert.ToInt32(item.SubItems[7].Text);
            int height = Convert.ToInt32(item.SubItems[8].Text);

            roiData.Update(new RoiData(cameraName, imageLane, imageSeq, new RectangleContour(right+1,top,width,height)));
            roiView.Update();
            mTree.SetRoiData(VisionFilePath.Process + cameraName + "::Roi", roiData.GetContainedData(cameraName));
        }

        #endregion
        //Color Spectrum
        void itmShowHisto_Click(object sender, EventArgs e)
        {
            itmShowHisto.Checked = !itmShowHisto.Checked;
            if (itmShowHisto.Checked == true)
            {
                if (itmRoi.Checked == true) rectangleAction = RectangleAction.Both;
                else rectangleAction = RectangleAction.Histogram;

                if (itmHistogram.Checked == false) itmHistogram.PerformClick();
                if (itmCalibration.Checked == true) itmCalibration.PerformClick();
              
            }
            else
            {
                if (itmRoi.Checked == true) rectangleAction = RectangleAction.MakeRoi;
                else rectangleAction = RectangleAction.None;
                
                if (itmHistogram.Checked == true) itmHistogram.PerformClick();
            }
            initViewerButtons();
        }


        void itmCalibration_Click(object sender, EventArgs e)
        {
            itmCalibration.Checked = !itmCalibration.Checked;
            CalForm calForm;
            if (itmCalibration.Checked == true)
            {
                calForm = new CalForm();
                itmCalibration.Tag = (object)calForm;
                calForm.labelOutX.Visible = false;
                calForm.buttonCalDo.Tag = null;
                calForm.buttonCalDo.Click += new EventHandler(buttonCalDo_Click);
                calForm.buttonCalExit.Click += new EventHandler(buttonCalExit_Click);
                calForm.buttonCalMeasure.Click += new EventHandler(buttonCalMeasure_Click);
                calForm.buttonCalOk.Click += new EventHandler(buttonCalOk_Click);

                //calForm.labelOutX.Text = string.Format("Cal Factor:{0:F5}[mm/pt]", parameterFile.CalibrationFactor[]);
                //calForm.buttonCalDo.Tag = (object)xCal;
                
                calForm.Show();

                if (itmHistogram.Checked == true) itmHistogram.PerformClick();
                if (itmRoi.Checked == true) itmRoi.PerformClick();

                rectangleAction = RectangleAction.MakeCalibration;
            }
            else
            {
                calForm = (CalForm)itmCalibration.Tag;
                if (calForm != null) calForm.Close();
                rectangleAction = RectangleAction.None;
            }
            initViewerButtons();
        }

        void buttonCalMeasure_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            CalForm form = (CalForm)btn.FindForm();
            if (form.buttonCalDo.Tag == null) return;
            double xCal =  (Double)form.buttonCalDo.Tag;
           if(xCal<=0) return;
           System.Numerics.Complex complex = (System.Numerics.Complex)form.labelXpt.Tag;
           Double length = xCal * complex.Magnitude;
           form.labelOutX.Text = string.Format("Length: {0:F2}", length);
           form.labelOutX.Visible = true;
        }

        void buttonCalOk_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            CalForm form = (CalForm)btn.FindForm();
            string cam = (string)form.Tag;
            if (cam != "")
            {
                if (parameterFile.CalibrationFactor.ContainsKey(cam) == true) 
                    parameterFile.CalibrationFactor[cam] = (double)form.buttonCalDo.Tag;
                else
                    parameterFile.CalibrationFactor.Add(cam,(double)form.buttonCalDo.Tag);

                mTree.UpdateCamTree("화상처리정보\\"+cam+"\\Calibration Factor", rootFile.Count, parameterFile);
            }
            itmCalibration.Checked = false;
            initViewerButtons();
            form.Close();
        }

        void buttonCalExit_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            CalForm form = (CalForm)btn.FindForm();
            itmCalibration.Checked = false;
            initViewerButtons();
            form.Close();
        }

        void buttonCalDo_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            CalForm form = (CalForm)btn.FindForm();
            System.Numerics.Complex complex = (System.Numerics.Complex)form.labelXpt.Tag;

            Double mag = complex.Magnitude;
            Double xCal;
            Double xmm = form.numericX.Value;
            if (mag > 0)
            {
                xCal = xmm / mag;
                form.labelOutX.Text = string.Format("Cal Factor:{0:F5}[mm/pt]",xCal);
                form.buttonCalDo.Tag = (object)xCal;
            }
            form.labelOutX.Visible = true;
        }

        //카메라속성설정
        void itmCameraAttr_Click(object sender, EventArgs e)
        {
            itmCameraAttr.Checked = !itmCameraAttr.Checked;
            if (itmCameraAttr.Checked == true)
            {
                if (itmCameraAttrView.Checked == false) itmCameraAttrView.PerformClick();
            }
            else
            {
                if (itmCameraAttrView.Checked == true) itmCameraAttrView.PerformClick();
            }
            initViewerButtons();
        }

    

        public void initTools()
        {
            tool = new ToolStrip();
            tool.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tool.GripStyle = ToolStripGripStyle.Hidden;
            tool.Dock = DockStyle.Top;
            tool.ShowItemToolTips = true;
            tool.Parent = this;
            tool.BringToFront();

            toolClose = new ToolStripButton();
            toolClose.Text = "종료";
            toolClose.ToolTipText = "프로그램을 종료합니다";
            toolClose.ImageScaling = ToolStripItemImageScaling.None;
            //toolClose.ImageTransparentColor = Color.FromArgb(0, 255, 0);
            toolClose.Image = new Bitmap(Properties.Resources.ToolExit);
            tool.Items.Add(toolClose);
            toolClose.Click += new EventHandler(toolClose_Click);

            ToolStripSeparator s1 = new ToolStripSeparator();
            tool.Items.Add(s1);

            toolSave = new ToolStripButton();
            toolSave.Text = "저장";
            toolSave.ToolTipText = "설정값을 일괄 저장합니다";
            toolSave.ImageScaling = ToolStripItemImageScaling.None;
            //toolSave.ImageTransparentColor = Color.FromArgb(0, 255, 0);
            toolSave.Image = new Bitmap(Properties.Resources.ToolSave);
            toolSave.Click += new EventHandler(toolSave_Click);
            tool.Items.Add(toolSave);
            
            ToolStripSeparator s2 = new ToolStripSeparator();
            tool.Items.Add(s2);

            toolSettings = new ToolStripButton();
            toolSettings.Text = "설정보기";
            toolSettings.ToolTipText = "화상처리설정내용을 보여줍니다";
            toolSettings.ImageScaling = ToolStripItemImageScaling.None;
            toolSettings.Image = new Bitmap(Properties.Resources.ToolConfig);
            toolSettings.Click += new EventHandler(toolSettings_Click);
            tool.Items.Add(toolSettings);

            toolRoi = new ToolStripButton();
            toolRoi.Text = "영역설정";
            toolRoi.ToolTipText = "화상처리영역을 설정합니다";
            toolRoi.ImageScaling = ToolStripItemImageScaling.None;
            toolRoi.Image = new Bitmap(Properties.Resources.ToolRoi);
            toolRoi.Click += new EventHandler(toolRoi_Click);
            tool.Items.Add(toolRoi);

            toolHisto = new ToolStripButton();
            toolHisto.Text = "색상분석";
            toolHisto.ToolTipText = "화상처리 색상을 분석합니다";
            toolHisto.ImageScaling = ToolStripItemImageScaling.None;
            toolHisto.Image = new Bitmap(Properties.Resources.ToolMeasurement);
            toolHisto.Click += new EventHandler(toolHisto_Click);
            tool.Items.Add(toolHisto);

            toolColor = new ToolStripButton();
            toolColor.Text = "색상설정";
            toolColor.ToolTipText = "화상처리 색상범위를 설정합니다";
            toolColor.ImageScaling = ToolStripItemImageScaling.None;
            toolColor.Image = new Bitmap(Properties.Resources.ToolColor);
            toolColor.Click += new EventHandler(toolColor_Click);
            tool.Items.Add(toolColor);

            ToolStripSeparator s3 = new ToolStripSeparator();
            tool.Items.Add(s3);

            toolRun = new ToolStripButton();
            toolRun.Text = "선별시작";
            toolRun.ToolTipText = "선별을 시작합니다";
            toolRun.ImageScaling = ToolStripItemImageScaling.None;
            toolRun.Image = new Bitmap(Properties.Resources.ToolStart);
            toolRun.Click += new EventHandler(toolRun_Click);
            tool.Items.Add(toolRun);

            toolStop = new ToolStripButton();
            toolStop.Text = "선별중지";
            toolStop.ToolTipText = "선별을 중지합니다";
            toolStop.ImageScaling = ToolStripItemImageScaling.None;
            toolStop.Image = new Bitmap(Properties.Resources.ToolStop);
            toolStop.Click += new EventHandler(toolStop_Click);
            tool.Items.Add(toolStop);

            ToolStripSeparator s4 = new ToolStripSeparator();
            tool.Items.Add(s4);

            toolResult = new ToolStripButton();
            toolResult.Text = "결과보기";
            toolResult.ToolTipText = "선별결과를 보여줍니다";
            toolResult.ImageScaling = ToolStripItemImageScaling.None;
            toolResult.Image = new Bitmap(Properties.Resources.ToolResult);
            toolResult.Click += new EventHandler(toolResult_Click);
            tool.Items.Add(toolResult);
            toolResult.Enabled = false;

            ToolStripSeparator s5 = new ToolStripSeparator();
            tool.Items.Add(s5);

            toolThree = new ToolStripButton();
            //toolOne.Text = "선별중지";
            toolThree.ToolTipText = "예외처리화면을 봅니다";
            toolThree.ImageScaling = ToolStripItemImageScaling.None;
            toolThree.Image = new Bitmap(Properties.Resources.apple3);
            toolThree.Alignment = ToolStripItemAlignment.Right;
            toolThree.Click += new EventHandler(toolThree_Click);
            tool.Items.Add(toolThree);

            toolTwo = new ToolStripButton();
            //toolOne.Text = "선별중지";
            toolTwo.ToolTipText = "색상처리화면을 봅니다";
            toolTwo.ImageScaling = ToolStripItemImageScaling.None;
            toolTwo.Image = new Bitmap(Properties.Resources.apple2);
            toolTwo.Alignment = ToolStripItemAlignment.Right;
            toolTwo.Click += new EventHandler(toolTwo_Click);
            tool.Items.Add(toolTwo);
            
            toolOne = new ToolStripButton();
            //toolOne.Text = "선별중지";
            toolOne.ToolTipText = "외형처리화면을 봅니다";
            toolOne.ImageScaling = ToolStripItemImageScaling.None;
            toolOne.Image = new Bitmap(Properties.Resources.apple1);
            toolOne.Checked = true;
            toolOne.Alignment = ToolStripItemAlignment.Right;
            toolOne.Click += new EventHandler(toolOne_Click);
            tool.Items.Add(toolOne);

            ToolStripSeparator s7 = new ToolStripSeparator();
            s7.Alignment = ToolStripItemAlignment.Right;
            tool.Items.Add(s7);

            toolBlank = new ToolStripButton();
            toolBlank.Alignment = ToolStripItemAlignment.Right;
            tool.Items.Add(toolBlank);
            toolBlank.Enabled = false;
            toolLock = new ToolStripButton();
            //toolOne.Text = "선별중지";
            toolLock.ToolTipText = "화면을 잠금니다";
            toolLock.ImageScaling = ToolStripItemImageScaling.None;
            toolLock.Image = new Bitmap(Properties.Resources.ToolLock);
            toolLock.Alignment = ToolStripItemAlignment.Right;
            toolLock.Click += new EventHandler(toolLock_Click);
            tool.Items.Add(toolLock);

            toolUnlock = new ToolStripButton();
            //toolOne.Text = "선별중지";
            toolUnlock.ToolTipText = "화면잠금을 해제합니다";
            toolUnlock.ImageScaling = ToolStripItemImageScaling.None;
            toolUnlock.Image = new Bitmap(Properties.Resources.ToolUnlock);
            toolUnlock.Alignment = ToolStripItemAlignment.Right;
            toolUnlock.Visible = false;
            toolUnlock.Click += new EventHandler(toolUnlock_Click);
            tool.Items.Add(toolUnlock);
 
        }

        void toolUnlock_Click(object sender, EventArgs e)
        {
            toolLock.Visible = true;
            toolUnlock.Visible = false;
            ToolsUnLock();
        }

        void toolLock_Click(object sender, EventArgs e)
        {
            toolUnlock.Visible = true;
            toolLock.Visible = false;
            ToolsLock();
        }
        
        void toolStop_Click(object sender, EventArgs e)
        {
            visionTrigger.Trigger = false;
            toolRun.Enabled = true;
            toolStop.Enabled = false;
            itmOpen.Enabled = true;
            if (itmResult.Checked) itmResult.PerformClick();
      
            if (rootFile.UserMode == "SUPERVISOR") ToolsForSuperStop();
            else ToolsForCustomerStop();
         }

        void toolRun_Click(object sender, EventArgs e)
        {
            visionTrigger.Trigger = true;
            toolStop.Enabled = true;
            toolRun.Enabled = false;
            itmOpen.Enabled = false;
            if (rootFile.UserMode == "SUPERVISOR") ToolsForSuperRun();
            else ToolsForCustomerRun();
        }

        void toolResult_Click(object sender, EventArgs e)
        {
            itmResult.PerformClick();
        }

        void toolOne_Click(object sender, EventArgs e)
        {
            toolOne.Checked = !toolOne.Checked;
            displayImage[0] = toolOne.Checked;
            if (toolOne.Checked == false && toolTwo.Checked == false && toolThree.Checked == false) toolOne.Checked = true;
        }

        void toolTwo_Click(object sender, EventArgs e)
        {
            toolTwo.Checked = !toolTwo.Checked;
            displayImage[1] = toolTwo.Checked;

            if (toolOne.Checked == false && toolTwo.Checked == false && toolThree.Checked == false) toolOne.Checked = true;
        }

        void toolThree_Click(object sender, EventArgs e)
        {
            toolThree.Checked = !toolThree.Checked;
            displayImage[2] = toolThree.Checked;
            if (toolOne.Checked == false && toolTwo.Checked == false && toolThree.Checked == false) toolOne.Checked = true;
        }

        void toolSettings_Click(object sender, EventArgs e)
        {
            itmAlgorithmAttrView.PerformClick();
        }
        void toolColor_Click(object sender, EventArgs e)
        {
            itmColor.PerformClick();
        }
        void toolHisto_Click(object sender, EventArgs e)
        {
            //itmHistogram.PerformClick();
            itmShowHisto.PerformClick();
        }

        void toolRoi_Click(object sender, EventArgs e)
        {
            itmRoi.PerformClick();
        }

        void toolSave_Click(object sender, EventArgs e)
        {
            itmSave.PerformClick();
        }

        void toolClose_Click(object sender, EventArgs e)
        {
            //closeAllImageViewForm();
            itmClose.PerformClick();
        }

        void addCameras(ImaqdxCameraInformation[] camInfo)
        {
            foreach (ImaqdxCameraInformation info in camInfo)
            {
                ToolStripMenuItem cam = new ToolStripMenuItem();
                cam.Text = info.Name;
                if (itmDCamera.ContainsKey(info.Name) == true) continue;
                cam.Image = new Bitmap(Properties.Resources.Video);
                itmDCamera.Add(info.Name, cam);
                itmCamera.DropDownItems.Add(cam);

                ToolStripMenuItem camConnect = new ToolStripMenuItem();
                camConnect.Text = cam.Text + "연결";
                camConnect.Image = new Bitmap(Properties.Resources.mnRun);
                camConnect.Tag = cam.Text;
                itmDCameraAdd.Add(cam.Text, camConnect);
                cam.DropDownItems.Add(camConnect);
                camConnect.Click += new EventHandler(camConnect_Click);
                ToolStripMenuItem camClose = new ToolStripMenuItem();
                camClose.Text = cam.Text + "제거";
                camClose.Image = new Bitmap(Properties.Resources.mnExit);
                camClose.Tag = cam.Text;
                camClose.Enabled = false;
                itmDCameraRemove.Add(cam.Text, camClose);
                cam.DropDownItems.Add(camClose);
                camClose.Click += new EventHandler(camClose_Click);
            }
            tool.BringToFront();
        }

        void camConnect_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            makeImageViewForm((string)item.Tag);
            item.Enabled = false;
        }
                
        void camClose_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            deleteImageViewForm((string)item.Tag,true);
            item.Enabled = false;
        }

        void itmClose_Click(object sender, EventArgs e)
        {
            if(serialOut!=null) serialOut.PortClose();
            if(controlPort != null) controlPort.PortClose();
            closeAllImageViewForm();
            visionTrigger.Dispose();

            Close();
        }

        void ToolsForSuperRun()
        {
            itmNew.Enabled=false;
            itmOpen.Enabled = false;
            itmSave.Enabled = true;
            itmSaveAs.Enabled = true;
            itmClose.Enabled=false;
            itmCamera.Enabled=false;

            itmRun.Enabled = false;
            itmStop.Enabled = true;

            itmSetting.Enabled = true;
            itmInterval.Enabled = true;
            itmRoiOffset.Enabled = true;
            itmRoi.Enabled = true;
            itmShowHisto.Enabled = true;
            itmCameraAttr.Enabled = false;
            itmColor.Enabled = true;
            itmCalibration.Enabled = false;

            itmView.Enabled = true;
            itmResult.Enabled = true;
            itmHistogram.Enabled = true;
            itmCameraList.Enabled = true;
            itmCameraAttrView.Enabled = true;
            itmAlgorithmAttrView.Enabled = true;

            itmRoiUser.Enabled = true;
            itmRoiDynamic.Enabled = true;

            itmStopWatch.Enabled = true;
            itmDebugger.Enabled = true;

            itmControl.Enabled = true;
            toolClose.Enabled = false;
            toolSave.Enabled = true;
            toolStop.Enabled = true;
            toolRun.Enabled = false;
            toolHisto.Enabled = true;
            toolColor.Enabled = true;
            toolSettings.Enabled = true;
            toolRoi.Enabled = true;
            toolResult.Enabled = true;

            itmWindow.Visible = false;

        }

        void ToolsForSuperStop()
        {
            itmNew.Enabled = true;
            itmOpen.Enabled = true;
            itmSave.Enabled = true;
            itmSaveAs.Enabled = true;
            itmClose.Enabled = true;

            itmCamera.Enabled = true;

            itmRun.Enabled = true;
            itmStop.Enabled = false;

            itmSetting.Enabled = true;
            itmInterval.Enabled = true;
            itmRoiOffset.Enabled = true;
            itmRoi.Enabled = true;
            itmShowHisto.Enabled = true;
            itmCameraAttr.Enabled = true;
            itmColor.Enabled = true;
            itmCalibration.Enabled = true;

            itmView.Enabled = true;
            itmResult.Enabled = true;
            itmHistogram.Enabled = true;
            itmCameraList.Enabled = true;
            itmCameraAttrView.Enabled = true;
            itmAlgorithmAttrView.Enabled = true;

            itmRoiUser.Enabled = true;
            itmRoiDynamic.Enabled = true;

            itmStopWatch.Enabled = true;
            itmDebugger.Enabled = true;

            itmControl.Enabled = true;
            toolClose.Enabled = true;
            toolSave.Enabled = true;
            toolStop.Enabled = false;
            toolRun.Enabled = true;
            toolHisto.Enabled = true;
            toolColor.Enabled = true;
            toolSettings.Enabled = true;
            toolRoi.Enabled = true;
            toolResult.Enabled = false;

            itmWindow.Visible = false;
        }

        void ToolsForCustomerRun()
        {
            itmNew.Enabled = false;
            itmOpen.Enabled = true;
            itmSave.Enabled = true;
            itmSaveAs.Enabled = true;
            itmClose.Enabled = false;
            itmCamera.Enabled = false;

            itmRun.Enabled = false;
            itmStop.Enabled = true;

            itmSetting.Enabled = true;
            itmInterval.Enabled = false;
            itmRoiOffset.Enabled = true;
            itmRoi.Enabled = true;
            itmShowHisto.Enabled = true;
            itmCameraAttr.Enabled = false;
            itmColor.Enabled = true;
            itmCalibration.Enabled = false;

            itmView.Enabled = true;
            itmResult.Enabled = true;
            itmHistogram.Enabled = true;
            itmCameraList.Enabled = true;
            itmCameraAttrView.Enabled = false;
            itmAlgorithmAttrView.Enabled = true;

            itmRoiUser.Enabled = true;
            itmRoiDynamic.Enabled = true;

            itmStopWatch.Enabled = false;
            itmDebugger.Enabled = false;
            itmControl.Enabled = true;

            toolClose.Enabled = false;
            toolSave.Enabled = true;
            toolStop.Enabled = true;
            toolRun.Enabled = false;
            toolHisto.Enabled = true;
            toolColor.Enabled = true;
            toolSettings.Enabled = true;
            toolRoi.Enabled = true;
            toolResult.Enabled = true;

            itmWindow.Visible = false;
        }

        void ToolsForCustomerStop()
        {
            itmNew.Enabled = false;
            itmOpen.Enabled = true;
            itmSave.Enabled = true;
            itmSaveAs.Enabled = true;
            itmClose.Enabled = true;

            itmCamera.Enabled = false;

            itmRun.Enabled = true;
            itmStop.Enabled = false;

            itmSetting.Enabled = true;
            itmInterval.Enabled = false;
            itmRoiOffset.Enabled = true;
            itmRoi.Enabled = true;
            itmShowHisto.Enabled = true;
            itmCameraAttr.Enabled = false;
            itmColor.Enabled = true;
            itmCalibration.Enabled = true;

            itmView.Enabled = true;
            itmResult.Enabled = true;
            itmHistogram.Enabled = true;
            itmCameraList.Enabled = true;
            itmCameraAttrView.Enabled = true;
            itmAlgorithmAttrView.Enabled = true;

            itmRoiUser.Enabled = true;
            itmRoiDynamic.Enabled = true;

            itmStopWatch.Enabled = false;
            itmDebugger.Enabled = false;

            itmControl.Enabled = true;
            toolClose.Enabled = true;
            toolSave.Enabled = true;
            toolStop.Enabled = false;
            toolRun.Enabled = true;
            toolHisto.Enabled = true;
            toolColor.Enabled = true;
            toolSettings.Enabled = true;
            toolRoi.Enabled = true;
            toolResult.Enabled = false;

            itmWindow.Visible = false;
        }
        
        void ToolForForceStop()
        {
            itmNew.Enabled = true;
            itmOpen.Enabled = true;
            itmSaveAs.Enabled = true;
            itmClose.Enabled = true;
            itmCamera.Enabled = true;
        }

        void ToolsForInit()
        {
            itmNew.Enabled = false;
            itmOpen.Enabled = false;
            itmSave.Enabled = false;
            itmSaveAs.Enabled = false;
            itmClose.Enabled = true;

            itmCamera.Enabled = false;

            itmRun.Enabled = false;
            itmStop.Enabled = false;

            itmSetting.Enabled = false;
            itmInterval.Enabled = false;
            itmRoiOffset.Enabled = false;
            itmRoi.Enabled = false;
            itmShowHisto.Enabled = false;
            itmCameraAttr.Enabled = false;
            itmColor.Enabled = false;
            itmCalibration.Enabled = false;

            itmView.Enabled = false;
            itmResult.Enabled = false;
            itmHistogram.Enabled = false;
            itmCameraList.Enabled = false;
            itmCameraAttrView.Enabled = false;
            itmAlgorithmAttrView.Enabled = false;

            itmRoiUser.Enabled = false;
            itmRoiDynamic.Enabled = false;

            itmStopWatch.Enabled = false;
            itmDebugger.Enabled = false;

            itmControl.Enabled = false;
            toolClose.Enabled = true;
            toolSave.Enabled = false;
            toolStop.Enabled = false;
            toolRun.Enabled = false;
            toolHisto.Enabled = false;
            toolColor.Enabled = false;
            toolSettings.Enabled = false;
            toolRoi.Enabled = false;
            toolResult.Enabled = false;

            itmWindow.Visible = false;
        }

        void ToolsLock()
        {
            toolState[0] = itmFile.Enabled;
            toolState[1] = itmCamera.Enabled;
            toolState[2] = itmSetting.Enabled;
            toolState[3] = itmControl.Enabled;
            toolState[4] = itmView.Enabled;
            toolState[5] = itmWindow.Enabled;
            toolState[6] = itmHelp.Enabled;

            toolState[7] = toolClose.Enabled;
            toolState[8] = toolSave.Enabled;
            toolState[9] = toolSettings.Enabled;
            toolState[10] = toolRoi.Enabled;
            toolState[11] = toolHisto.Enabled;
            toolState[12] = toolColor.Enabled;
            toolState[13] = toolRun.Enabled;
            toolState[14] = toolStop.Enabled;
            toolState[15] = toolResult.Enabled;


            itmFile.Enabled=false;
            itmCamera.Enabled = false;
            itmSetting.Enabled = false;
            itmControl.Enabled = false;
            itmView.Enabled =false;
            itmWindow.Enabled = false;
            itmHelp.Enabled = false;

            toolClose.Enabled = false;
            toolSave.Enabled = false;
            toolSettings.Enabled = false;
            toolRoi.Enabled = false;
            toolHisto.Enabled = false;
            toolColor.Enabled = false;
            toolRun.Enabled = false;
            toolStop.Enabled = false;
            toolResult.Enabled = false;

            toolOne.Enabled = false;
            toolTwo.Enabled = false;
            toolThree.Enabled = false;
            
        }

        void ToolsUnLock()
        {
            itmFile.Enabled = toolState[0];
            itmCamera.Enabled = toolState[1];
            itmSetting.Enabled = toolState[2];
            itmControl.Enabled = toolState[3];
            itmView.Enabled = toolState[4];
            itmWindow.Enabled = toolState[5];
            itmHelp.Enabled = toolState[6];

            toolClose.Enabled = toolState[7];
            toolSave.Enabled = toolState[8];
            toolSettings.Enabled = toolState[9];
            toolRoi.Enabled = toolState[10];
            toolHisto.Enabled = toolState[11];
            toolColor.Enabled = toolState[12];
            toolRun.Enabled = toolState[13];
            toolStop.Enabled = toolState[14];
            toolResult.Enabled = toolState[15];

            toolOne.Enabled = true;
            toolTwo.Enabled = true;
            toolThree.Enabled = true;
        }

    }
}
