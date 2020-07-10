using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SnpSystem.Vision.VisionConfigurationHelper;
using SnpSystem.Vision.Acquisition;

namespace SnpSmartVision
{
    
    public partial class RoiListView : Form
    {

        List<RoiData> _roiData;
        public MenuItem itmCopy, itmDelete, itmModify,itmClose;

        public RoiListView(List<RoiData> roiData)
        {
            _roiData = roiData;
            itmCopy = new System.Windows.Forms.MenuItem("복사");
            itmModify = new System.Windows.Forms.MenuItem("수정");
            itmDelete = new System.Windows.Forms.MenuItem("삭제");
            itmClose = new System.Windows.Forms.MenuItem("종료");

            //itmCopy.Click += new EventHandler(itmCopy_Click);
            //itmModify.Click += new EventHandler(itmModify_Click);
            //itmDelete.Click += new EventHandler(itmDelete_Click);
            
            MenuItem[] popUp = new MenuItem[] { itmCopy, itmModify, new MenuItem("-"), itmDelete,new MenuItem("-"),itmClose }; 
            this.ContextMenu = new ContextMenu(popUp);
            InitializeComponent();
            initListView();
            
        }

        void itmDelete_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection select = listView1.SelectedItems;
            foreach (ListViewItem item in select)
            {
               
            }
        }
        /*
        void itmModify_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void itmCopy_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }*/

        public new void Update()
        {
            listView1.Clear();
            initListView();
            foreach (RoiData roi in _roiData)
            {
                ListViewItem Roi1 = new ListViewItem(roi.CameraName);
                Roi1.SubItems.Add(roi.Lane.ToString());
                Roi1.SubItems.Add(roi.Sequence.ToString());
                //Roi1.SubItems.Add(roi.Direction.ToString());
                Roi1.SubItems.Add(roi.Left.ToString());
                Roi1.SubItems.Add(roi.Top.ToString());
                Roi1.SubItems.Add(roi.Right.ToString());
                Roi1.SubItems.Add(roi.Bottom.ToString());
                Roi1.SubItems.Add(roi.Width.ToString());
                Roi1.SubItems.Add(roi.Height.ToString());
                listView1.Items.Add(Roi1);
            }
            
        }
        void initListView()
        {

            this.Text = "ROI Table";
            this.Width = 650;
 
            listView1.Dock = DockStyle.Fill;
            listView1.View = View.Details;
            listView1.FullRowSelect = true;

            ColumnHeader camera = new ColumnHeader();
            camera.Text = "Camera";
            camera.TextAlign = HorizontalAlignment.Left;
            camera.Width = 70;

            ColumnHeader lane = new ColumnHeader();
            lane.Text = "Lane";
            lane.TextAlign = HorizontalAlignment.Left;
            lane.Width = 70;

            ColumnHeader number = new ColumnHeader();
            number.Text = "Number";
            number.TextAlign = HorizontalAlignment.Left;
            number.Width = 70;
            /*
            ColumnHeader direction = new ColumnHeader();
            direction.Text = "Direction";
            direction.TextAlign = HorizontalAlignment.Left;
            direction.Width = 90;
            */
            ColumnHeader left = new ColumnHeader();
            left.Text = "Left";
            left.TextAlign = HorizontalAlignment.Left;
            left.Width = 70;

            ColumnHeader top = new ColumnHeader();
            top.Text = "Top";
            top.TextAlign = HorizontalAlignment.Left;
            top.Width = 70;

            ColumnHeader right = new ColumnHeader();
            right.Text = "Right";
            right.TextAlign = HorizontalAlignment.Left;
            right.Width = 70;

            ColumnHeader bottom = new ColumnHeader();
            bottom.Text = "Bottom";
            bottom.TextAlign = HorizontalAlignment.Left;
            bottom.Width = 70;

            ColumnHeader width = new ColumnHeader();
            width.Text = "Width";
            width.TextAlign = HorizontalAlignment.Left;
            width.Width = 70;

            ColumnHeader height = new ColumnHeader();
            height.Text = "Height";
            height.TextAlign = HorizontalAlignment.Left;
            height.Width = 70;
            
            listView1.Columns.Add(camera);
            listView1.Columns.Add(lane);
            listView1.Columns.Add(number);
            //listView1.Columns.Add(direction);
            listView1.Columns.Add(left);
            listView1.Columns.Add(top);
            listView1.Columns.Add(right);
            listView1.Columns.Add(bottom);
            listView1.Columns.Add(width);
            listView1.Columns.Add(height);
        }
    }
}
