namespace SnpSmartVision
{
    partial class CamForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CamForm));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnPointer = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPoint = new System.Windows.Forms.ToolStripButton();
            this.btnLine = new System.Windows.Forms.ToolStripButton();
            this.btnRect = new System.Windows.Forms.ToolStripButton();
            this.btnRotateRect = new System.Windows.Forms.ToolStripButton();
            this.btnCircle = new System.Windows.Forms.ToolStripButton();
            this.btnDonut = new System.Windows.Forms.ToolStripButton();
            this.btnOpenPoly = new System.Windows.Forms.ToolStripButton();
            this.btnClosedPoly = new System.Windows.Forms.ToolStripButton();
            this.btnOpenFree = new System.Windows.Forms.ToolStripButton();
            this.toolClosedFree = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.btnFit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.btnHand = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolRun = new System.Windows.Forms.ToolStripButton();
            this.toolPause = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSave = new System.Windows.Forms.ToolStripButton();
            this.imageViewer = new NationalInstruments.Vision.WindowsForms.ImageViewer();
            this.btnClosedFree = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPointer,
            this.toolStripSeparator1,
            this.btnPoint,
            this.btnLine,
            this.btnRect,
            this.btnRotateRect,
            this.btnCircle,
            this.btnDonut,
            this.btnOpenPoly,
            this.btnClosedPoly,
            this.btnOpenFree,
            this.toolClosedFree,
            this.toolStripSeparator2,
            this.btnZoomIn,
            this.btnZoomOut,
            this.btnFit,
            this.toolStripButton2,
            this.btnHand,
            this.toolStripSeparator3,
            this.toolRun,
            this.toolPause,
            this.toolStripSeparator4,
            this.toolSave});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(596, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            this.toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip_ItemClicked);
            // 
            // btnPointer
            // 
            this.btnPointer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPointer.Image = global::SnpSmartVision.Properties.Resources.Pointer;
            this.btnPointer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPointer.Name = "btnPointer";
            this.btnPointer.Size = new System.Drawing.Size(23, 22);
            this.btnPointer.Tag = "0";
            this.btnPointer.Text = "기능선택";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator1.Tag = "99";
            // 
            // btnPoint
            // 
            this.btnPoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPoint.Image = global::SnpSmartVision.Properties.Resources.point1;
            this.btnPoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPoint.Name = "btnPoint";
            this.btnPoint.Size = new System.Drawing.Size(23, 22);
            this.btnPoint.Tag = "1";
            this.btnPoint.Text = "포인트찍기";
            // 
            // btnLine
            // 
            this.btnLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLine.Image = global::SnpSmartVision.Properties.Resources.line;
            this.btnLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLine.Name = "btnLine";
            this.btnLine.Size = new System.Drawing.Size(23, 22);
            this.btnLine.Tag = "2";
            this.btnLine.Text = "선그리기";
            // 
            // btnRect
            // 
            this.btnRect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRect.Image = global::SnpSmartVision.Properties.Resources.rect;
            this.btnRect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRect.Name = "btnRect";
            this.btnRect.Size = new System.Drawing.Size(23, 22);
            this.btnRect.Tag = "3";
            this.btnRect.Text = "사각형그리기";
            // 
            // btnRotateRect
            // 
            this.btnRotateRect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRotateRect.Image = global::SnpSmartVision.Properties.Resources.rotate_rect;
            this.btnRotateRect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRotateRect.Name = "btnRotateRect";
            this.btnRotateRect.Size = new System.Drawing.Size(23, 22);
            this.btnRotateRect.Tag = "4";
            this.btnRotateRect.Text = "회전가능사각형그리기";
            // 
            // btnCircle
            // 
            this.btnCircle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCircle.Image = global::SnpSmartVision.Properties.Resources.circle;
            this.btnCircle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCircle.Name = "btnCircle";
            this.btnCircle.Size = new System.Drawing.Size(23, 22);
            this.btnCircle.Tag = "5";
            this.btnCircle.Text = "타원그리기";
            // 
            // btnDonut
            // 
            this.btnDonut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDonut.Image = global::SnpSmartVision.Properties.Resources.donut;
            this.btnDonut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDonut.Name = "btnDonut";
            this.btnDonut.Size = new System.Drawing.Size(23, 22);
            this.btnDonut.Tag = "6";
            this.btnDonut.Text = "도넛형원 그리기";
            // 
            // btnOpenPoly
            // 
            this.btnOpenPoly.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpenPoly.Image = global::SnpSmartVision.Properties.Resources.open_poly;
            this.btnOpenPoly.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenPoly.Name = "btnOpenPoly";
            this.btnOpenPoly.Size = new System.Drawing.Size(23, 22);
            this.btnOpenPoly.Tag = "7";
            this.btnOpenPoly.Text = "열린다각형그리기";
            // 
            // btnClosedPoly
            // 
            this.btnClosedPoly.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClosedPoly.Image = global::SnpSmartVision.Properties.Resources.closed_poly;
            this.btnClosedPoly.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClosedPoly.Name = "btnClosedPoly";
            this.btnClosedPoly.RightToLeftAutoMirrorImage = true;
            this.btnClosedPoly.Size = new System.Drawing.Size(23, 22);
            this.btnClosedPoly.Tag = "8";
            this.btnClosedPoly.Text = "닫힌다각형그리기";
            // 
            // btnOpenFree
            // 
            this.btnOpenFree.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpenFree.Image = global::SnpSmartVision.Properties.Resources.open_free;
            this.btnOpenFree.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenFree.Name = "btnOpenFree";
            this.btnOpenFree.Size = new System.Drawing.Size(23, 22);
            this.btnOpenFree.Tag = "9";
            this.btnOpenFree.Text = "열린자유곡선그리기";
            // 
            // toolClosedFree
            // 
            this.toolClosedFree.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolClosedFree.Image = global::SnpSmartVision.Properties.Resources.closed_free;
            this.toolClosedFree.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolClosedFree.Name = "toolClosedFree";
            this.toolClosedFree.Size = new System.Drawing.Size(23, 22);
            this.toolClosedFree.Tag = "10";
            this.toolClosedFree.Text = "닫힌자유곡선그리기";
            this.toolClosedFree.ToolTipText = "닫힌자유곡선그리기";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator2.Tag = "99";
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomIn.Image = global::SnpSmartVision.Properties.Resources.ZoomIn;
            this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(23, 22);
            this.btnZoomIn.Tag = "11";
            this.btnZoomIn.Text = "이미지확대";
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomOut.Image = global::SnpSmartVision.Properties.Resources.ZoomOut;
            this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(23, 22);
            this.btnZoomOut.Tag = "12";
            this.btnZoomOut.Text = "이미지축소";
            // 
            // btnFit
            // 
            this.btnFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFit.Image = global::SnpSmartVision.Properties.Resources.fit;
            this.btnFit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFit.Name = "btnFit";
            this.btnFit.Size = new System.Drawing.Size(23, 22);
            this.btnFit.Tag = "13";
            this.btnFit.Text = "화면에 맞추기";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::SnpSmartVision.Properties.Resources.Normal;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Tag = "15";
            this.toolStripButton2.Text = "오리지날이미지";
            // 
            // btnHand
            // 
            this.btnHand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnHand.Image = global::SnpSmartVision.Properties.Resources.Breakpoint;
            this.btnHand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHand.Name = "btnHand";
            this.btnHand.Size = new System.Drawing.Size(23, 22);
            this.btnHand.Tag = "14";
            this.btnHand.Text = "이미지옮기기";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Tag = "99";
            // 
            // toolRun
            // 
            this.toolRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolRun.Image = ((System.Drawing.Image)(resources.GetObject("toolRun.Image")));
            this.toolRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolRun.Name = "toolRun";
            this.toolRun.Size = new System.Drawing.Size(23, 22);
            this.toolRun.Tag = "16";
            this.toolRun.Text = "화상취득개시";
            // 
            // toolPause
            // 
            this.toolPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolPause.Image = global::SnpSmartVision.Properties.Resources.Pause;
            this.toolPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPause.Name = "toolPause";
            this.toolPause.Size = new System.Drawing.Size(23, 22);
            this.toolPause.Tag = "17";
            this.toolPause.Text = "화상취득정지";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator4.Tag = "99";
            // 
            // toolSave
            // 
            this.toolSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolSave.Image = global::SnpSmartVision.Properties.Resources.Save1;
            this.toolSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSave.Name = "toolSave";
            this.toolSave.Size = new System.Drawing.Size(23, 22);
            this.toolSave.Tag = "18";
            this.toolSave.Text = "카메라설정 저장";
            // 
            // imageViewer
            // 
            this.imageViewer.AutoScroll = true;
            this.imageViewer.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.imageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageViewer.Location = new System.Drawing.Point(129, 107);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.ShowToolbar = true;
            this.imageViewer.Size = new System.Drawing.Size(354, 239);
            this.imageViewer.TabIndex = 0;
            this.imageViewer.RoiChanged += new System.EventHandler<NationalInstruments.Vision.WindowsForms.ContoursChangedEventArgs>(this.imageViewer_RoiChanged);
            // 
            // btnClosedFree
            // 
            this.btnClosedFree.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClosedFree.Image = global::SnpSmartVision.Properties.Resources.closed_free;
            this.btnClosedFree.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClosedFree.Name = "btnClosedFree";
            this.btnClosedFree.Size = new System.Drawing.Size(23, 22);
            this.btnClosedFree.Tag = "10";
            this.btnClosedFree.Text = "닫힌자유곡선그리기";
            // 
            // CamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(596, 415);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.imageViewer);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CamForm";
            this.Text = "Image Viewer";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton btnPointer;
        private System.Windows.Forms.ToolStripButton btnPoint;
        private System.Windows.Forms.ToolStripButton btnRect;
        private System.Windows.Forms.ToolStripButton btnRotateRect;
        private System.Windows.Forms.ToolStripButton btnCircle;
        private System.Windows.Forms.ToolStripButton btnDonut;
        private System.Windows.Forms.ToolStripButton btnOpenPoly;
        private System.Windows.Forms.ToolStripButton btnClosedPoly;
        private System.Windows.Forms.ToolStripButton btnOpenFree;
        private System.Windows.Forms.ToolStripButton btnZoomIn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnZoomOut;
        private System.Windows.Forms.ToolStripButton btnFit;
        private System.Windows.Forms.ToolStripButton btnHand;
        private System.Windows.Forms.ToolStripButton btnLine;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolClosedFree;
        private System.Windows.Forms.ToolStripButton btnClosedFree;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStrip toolStrip;
        public System.Windows.Forms.ToolStripButton toolRun;
        public System.Windows.Forms.ToolStripButton toolPause;
        public System.Windows.Forms.ToolStripButton toolSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        public NationalInstruments.Vision.WindowsForms.ImageViewer imageViewer;
    }
}