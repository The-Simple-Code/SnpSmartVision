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
    public partial class TimeTableForm : Form
    {
        public TimeTableForm()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.Columns.Add("Task", 210, HorizontalAlignment.Left);
            listView1.Columns.Add("Time Elapsed[msec]", 130, HorizontalAlignment.Left);
        }

        public void InitTimeTable(List<string> cameraList)
        {
            comboBox1.Items.Clear();
            foreach (string str in cameraList)
            {
                comboBox1.Items.Add(str);
            }
            comboBox1.Text = (string)comboBox1.Items[0];
            listView1.Items.Clear();
        }
    }
}
