using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SnpSmartVision
{
    public partial class Debugger : Form
    {

        public bool IsLoaded
        {
            get;
            set;
        }

        public bool Pause
        {
            get;
            set;
        }

        public bool ExitRequest
        {
            get;
            set;
        }

        new Dictionary<string, bool> Enabled;
        public bool FSize = false;
        public bool FColor = false;
        public bool FAspect = false;
        public bool FColorRatio = false;
        public bool FResult = false;
        public bool OutPacket = false;

        string[] cameraList;
        public System.Windows.Forms.ToolStripMenuItem[] selectToolStripMenuItem;


        public Debugger(string[] cameraList)
        {
            InitializeComponent();
            this.cameraList = cameraList;
            Enabled = new Dictionary<string, bool>();
            Pause = true;
            pauseToolStripMenuItem.Checked = true;
            ExitRequest = false;
            makeCameraSelectionMenuItems();
        }

        public bool IsCameraSelected(string cameraName)
        {
            return Enabled[cameraName];
        }

        void makeCameraSelectionMenuItems()
        {
            selectToolStripMenuItem = new ToolStripMenuItem[cameraList.Length];
            int i = 0;
            foreach (string name in cameraList)
            {
                selectToolStripMenuItem[i] = new ToolStripMenuItem();
                selectToolStripMenuItem[i].Name = name;
                selectToolStripMenuItem[i].Text = name;
                selectToolStripMenuItem[i].Click += new EventHandler(Debugger_Click);
                cameraToolStripMenuItem.DropDownItems.Add(selectToolStripMenuItem[i]);
                Enabled.Add(name, false);
            }
        }

        void Debugger_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            item.Checked = !item.Checked;
            Enabled[item.Text] = item.Checked;
        }

        public void DisplayProcessingResult(string message)
        {
            List<string> text = new List<string>();
            text = textBox1.Lines.ToList();
            text.Add(message);
            textBox1.Lines = text.ToArray();
            if (textBox1.Lines.Length > 500)
            {
                text.Clear();
                textBox1.Lines = text.ToArray();
            }
            textBox1.Select(textBox1.Text.Length, 0);
        }

        void txFrameToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            item.Checked = !item.Checked;
            OutPacket = item.Checked;
        }

        void aspectRatioToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            item.Checked = !item.Checked;
            FAspect = item.Checked;
        }

        void colorRatioToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            item.Checked = !item.Checked;
            FColorRatio = item.Checked;
        }

        void colorToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            item.Checked = !item.Checked;
            FColor = item.Checked;
        }

        void sizeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            item.Checked = !item.Checked;
            FSize = item.Checked;
        }
        private void resultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            item.Checked = !item.Checked;
            FResult = item.Checked;
        }
        void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Pause = false;
            ExitRequest = true;
        }
        
        void pauseToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            pauseToolStripMenuItem.Checked = !pauseToolStripMenuItem.Checked;
            runToolStripMenuItem.Checked = !pauseToolStripMenuItem.Checked;
            Pause = pauseToolStripMenuItem.Checked;
        }
        
        void clearToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            List<string> str = new List<string>();
            str.Clear();
            textBox1.Lines = str.ToArray();
        }

        void runToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            runToolStripMenuItem.Checked = !runToolStripMenuItem.Checked;
            pauseToolStripMenuItem.Checked = !runToolStripMenuItem.Checked;
            Pause = !runToolStripMenuItem.Checked;
        }

        void Debugger_Load(object sender, System.EventArgs e)
        {
            IsLoaded = true;
        }

        void Debugger_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            IsLoaded = false;
        }

        private void Debugger_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !this.ExitRequest;
        }

       
    }
}
