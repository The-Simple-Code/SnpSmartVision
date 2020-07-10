using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NationalInstruments.Vision;
using SnpSystem.Vision.Acquisition;
using SnpSystem.Vision.VisionConfigurationHelper;

namespace SnpSmartVision
{
    
    public partial class frmRoiSet : Form
    {
        string _owner;
        ImageRectangle rectangle=new ImageRectangle(0,0,0,0);
 
        public new string Owner
        {
            set 
            { 
                _owner = value;
                this.Text = "ROI SETTING[" + value + "]";
            }
            get { return _owner; }
        }

        public ImageRectangle Rectangle
        {
            get { return rectangle; }
        }
        public void SetContour(RectangleContour contour)
        {
            this.boxTop.Value = (int)contour.Top;
            this.boxLeft.Value = (int)contour.Left;
            this.boxBottom.Value = (int)contour.Top + (int)contour.Height;
            this.boxRight.Value = (int)contour.Width + (int)contour.Left;
            this.lblCenter.Text = string.Format("{0},{1}", (int)(contour.Left + contour.Width / 2), (int)(contour.Top + contour.Height / 2));

            rectangle.LeftTop.X = (int)contour.Left;
            rectangle.LeftTop.Y = (int)contour.Top;
            rectangle.WidthHeight.Width = (int)contour.Width;
            rectangle.WidthHeight.Height = (int)contour.Height;
        }

        public frmRoiSet()
        {
            InitializeComponent();
            
        }
        public ImageDirection GetDirection()
        {
            ImageDirection dir = new ImageDirection();
            switch (cmbDirection.Text)
            {
                case "Side1": dir = ImageDirection.Side1; break;
                case "Side2": dir = ImageDirection.Side2; break;
                case "Side3": dir = ImageDirection.Side3; break;
            }
            return dir;
        }
        public ImageLane GetLane()
        {
            ImageLane lane = new ImageLane();
            switch (cmbLane.Text)
            {
                case "Lane1": lane = ImageLane.Lane1;break;
                case "Lane2": lane = ImageLane.Lane2;break;
            }
            return lane;
        }
        public ImageSequence GetSequence()
        {
            ImageSequence sequence = new ImageSequence();
            switch (cmbSequence.Text.ToUpper())
            {
                case "SEQ0": sequence = ImageSequence.Seq0;break;
                case "SEQ1": sequence = ImageSequence.Seq1;break;
                case "SEQ2": sequence = ImageSequence.Seq2;break;
                case "SEQ3": sequence = ImageSequence.Seq3;break;
                case "SEQ4": sequence = ImageSequence.Seq4;break;
                case "SEQ5": sequence = ImageSequence.Seq5;break;
                case "SEQ6": sequence = ImageSequence.Seq6;break;
                case "SEQ7": sequence = ImageSequence.Seq7;break;
                case "SEQ8": sequence = ImageSequence.Seq8;break;
                case "SEQ9": sequence = ImageSequence.Seq9;break;
                case "SEQA": sequence = ImageSequence.SeqA; break;
                case "SEQB": sequence = ImageSequence.SeqB; break;
            }
            return sequence;
        }
    }
}
