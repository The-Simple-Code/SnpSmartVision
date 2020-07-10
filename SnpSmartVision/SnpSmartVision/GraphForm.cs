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
    public partial class GraphForm : DockContent
    {
        public GraphForm()
        {
            InitializeComponent();
            chart.Dock = DockStyle.Fill;
            chart.Series["plane1"].Points.Clear();
            chart.Series["plane2"].Points.Clear();
            chart.Series["plane3"].Points.Clear();
        }

        public void DrawGraph(int[] hue, int[] sat, int[] lum,string[] legendName)
        {
            chart.Series["plane1"].Points.Clear();
            chart.Series["plane2"].Points.Clear();
            chart.Series["plane3"].Points.Clear();

            foreach(int v in hue)
            {
                chart.Series["plane1"].Points.AddY(v);
                chart.Series["plane1"].LegendText=legendName[0];
            }
            foreach (int v in sat)
            {
                chart.Series["plane2"].Points.AddY(v);
                chart.Series["plane2"].LegendText = legendName[1];
            }
            foreach (int v in lum)
            {
                chart.Series["plane3"].Points.AddY(v);
                chart.Series["plane3"].LegendText = legendName[2];
            }
        }

        public void DrawGraph(string seriesName,string legendName, int[] value)
        {
            chart.Series[seriesName].Points.Clear();
            chart.Series[seriesName].LegendText = legendName;
            foreach (int v in value)
            {
                chart.Series[seriesName].Points.AddY(v);
            }
        }
        public String colorMode;
        public String selectedCam;

    }
}
