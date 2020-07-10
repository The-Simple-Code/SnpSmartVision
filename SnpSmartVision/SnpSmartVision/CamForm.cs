using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using NationalInstruments.Vision;
using NationalInstruments.Vision.WindowsForms;
using NationalInstruments.Vision.Acquisition.Imaqdx;

using SnpSystem.Vision.Acquisition;
using SnpSystem.Vision.CameraTrigger;


namespace SnpSmartVision
{

    [Flags] public  enum ButtonViewer :uint
    {
    
        None            =0,
        Selection       =1<<0,
        Seperator1      =1<<1, 
        Point           =1<<2,
        Line            =1<<3,
        Rectangle       =1<<4,
        RotateRectangle =1<<5,
        Oval            =1<<6,
        Annula          =1<<7,
        OpenPoly        =1<<8,
        ClosedPoly      =1<<9,
        OpenFree        =1<<10,
        ClosedFree      =1<<11,
        Seperator2      =1<<12, 
        ZoomIn          =1<<13,
        ZoomOut         =1<<14,
        Fit             =1<<15,
        Original        =1<<16,
        Hand            =1<<17,
        Seperator3      =1<<18,
        Run             =1<<19,
        Pause           =1<<20,
        Seperator4      =1<<21,
        Save            =1<<22,
        Over            =1<<23,
        Seperator5      =1<<24,
        Color           =1<<25,
        PictureAdj      =ZoomIn|ZoomOut|Fit|Original|Hand,
        JustRoiSet      =Selection|Seperator1|Rectangle,
        ClosedRoi       =Rectangle|RotateRectangle|Oval|ClosedPoly|ClosedFree,
        SetAttr         =Run|Pause|Seperator4|Save,
        Calibration     =Selection|Seperator1|Line,
    }
    
    public partial class CamForm : DockContent
    {
        const int MaximumBottonCount = 23; 
        private ButtonViewer _buttons=ButtonViewer.None;
        public CamForm()
        {
            InitializeComponent();
            this.Text = "이미지보기";
            imageViewer.Dock = DockStyle.Fill;
            imageViewer.ShowToolbar = false;
            imageViewer.ActiveTool = ViewerTools.Selection;
            imageViewer.Roi.MaximumContours = 1;
            toolStrip.Visible = false;
        }

        public ButtonViewer Buttons
        {
            get { return _buttons; }
            set
            {
                if (value == ButtonViewer.None) ShowImageViewerToolBar = false;
                else
                {
                    ShowImageViewerToolBar = true;
                    uint val = (uint)value;
                    for (int i = 0; i < MaximumBottonCount; i++)
                    {
                        //op=1<<i;
                        uint v2 = (uint)1 & val;
                        toolStrip.Items[i].Visible = v2 == 0 ? false : true;
                        val >>=1;
                    }
                }
            }

        }
        public ImageViewer Viewer
        {
            get { return imageViewer; }
        }

        public bool ShowImageViewerToolBar
        {
            get { return toolStrip.Visible; }
            set { toolStrip.Visible = value; }
        }

        public VisionImage Image
        {
            set { imageViewer.Attach(value); }
            get { return imageViewer.Image; }
        }

        private void unchekedToolButton()
        {
            foreach (ToolStripItem itm in toolStrip.Items)
            {
                if (itm is ToolStripButton)
                {
                    ToolStripButton btn = itm as ToolStripButton;
                    btn.Checked = false;
                }
            }
        }

        

        private void doButtonAction(int index)
        {
            
            switch (index)
            {
                case 0: imageViewer.ActiveTool = ViewerTools.Selection; break;
                case 1: imageViewer.ActiveTool = ViewerTools.Point; break;
                case 2: imageViewer.ActiveTool = ViewerTools.Line; break;
                case 3: imageViewer.ActiveTool = ViewerTools.Rectangle; break;
                case 4: imageViewer.ActiveTool = ViewerTools.RotatedRectangle; break;
                case 5: imageViewer.ActiveTool = ViewerTools.Oval; break;
                case 6: imageViewer.ActiveTool = ViewerTools.Annulus; break;
                case 7: imageViewer.ActiveTool = ViewerTools.Polyline; break;
                case 8: imageViewer.ActiveTool = ViewerTools.Polygon; break;
                case 9: imageViewer.ActiveTool = ViewerTools.Freehand; break;
                case 10: imageViewer.ActiveTool = ViewerTools.ClosedFreehand; break;
                case 11: imageViewer.ActiveTool = ViewerTools.ZoomIn; break;
                case 12: imageViewer.ActiveTool = ViewerTools.ZoomOut; break;
                case 13: imageViewer.ZoomToFit = true; break;
                case 14: imageViewer.ActiveTool = ViewerTools.Pan; break;
                case 15: imageViewer.ZoomInfo.Initialize(1.0, 1.0); break;
               // case 16:imageViewer.ActiveTool = ViewerTools.
                default: imageViewer.ActiveTool = ViewerTools.Selection; break; 
            }
            
        }
        private void toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            if (item is ToolStripButton)
            {
                unchekedToolButton();
                ToolStripButton button = item as ToolStripButton;
                button.Checked = true;
                if (button.Tag == null) return;
                int index = int.Parse((string)button.Tag);
                doButtonAction(index);
            }

        }

        private void imageViewer_RoiChanged(object sender, ContoursChangedEventArgs e)
        {

        }

    }
}
