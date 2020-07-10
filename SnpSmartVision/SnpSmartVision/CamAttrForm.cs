using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using SnpSystem.Vision.Acquisition;
using SnpSystem.Vision.CameraTrigger;

using NationalInstruments.Vision;
using NationalInstruments.Vision.WindowsForms;
using NationalInstruments.Vision.Acquisition.Imaqdx;

namespace SnpSmartVision
{
    public partial class CamAttrForm : DockContent
    {
        CameraInformation cameras;
        ImaqdxCameraInformation[] camInfo;
        int index;
        public ImaqdxCameraInformation[] CamInfo
        {
            get { return camInfo; }
        }

        public int SelectedIndex
        {
            get { return index; }
            set { index = value; }
        }
        //bool pageChange;
  
        // 카메라목록 보기
        public CamAttrForm()
        {
            InitializeComponent();
            this.Text = "카메라목록";
            cameras = new CameraInformation();
            SetCameraInformation();
        }

        //화상처리 파라미터 보기
        public CamAttrForm(string attr)
        {
            InitializeComponent();
            this.Text = attr;
            tabControl.Visible = true;
            treeView1.Dock = DockStyle.Fill;
            initTabControl();  
        }

        public void initTabControl()
        {
            tabControl.SelectedIndex = 0;
        }

        public void hideTabs()
        {
            foreach (TabPage page in tabControl.TabPages)
            {
                
            }
        }

        public void showTab(TabPage page)
        {
            //pageChange = true;
            tabControl.SelectedIndex = tabControl.TabPages.IndexOf(page);
        }
        public void SetCameraInformation()
        {
            camInfo=cameras.GetCameraInformation();
            cameras.Images = imageList;
            cameras.FillTreeView(treeView1);
            tabControl.Visible = false;
            treeView1.Dock = DockStyle.Fill;
            treeView1.AllowDrop = true;
            treeView1.ItemDrag += new ItemDragEventHandler(treeView1_ItemDrag);
        }

        void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Copy);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


    }
}
