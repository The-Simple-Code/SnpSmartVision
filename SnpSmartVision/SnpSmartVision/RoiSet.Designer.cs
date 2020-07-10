namespace SnpSmartVision
{
    partial class frmRoiSet
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCenter = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.boxTop = new IntTextBox.IntTextBox();
            this.boxLeft = new IntTextBox.IntTextBox();
            this.boxRight = new IntTextBox.IntTextBox();
            this.boxBottom = new IntTextBox.IntTextBox();
            this.cmbLane = new System.Windows.Forms.ComboBox();
            this.cmbSequence = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDirection = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.lblCenter);
            this.panel1.Controls.Add(this.shapeContainer1);
            this.panel1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panel1.Location = new System.Drawing.Point(79, 75);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(127, 119);
            this.panel1.TabIndex = 0;
            // 
            // lblCenter
            // 
            this.lblCenter.AutoSize = true;
            this.lblCenter.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCenter.Location = new System.Drawing.Point(46, 45);
            this.lblCenter.Name = "lblCenter";
            this.lblCenter.Size = new System.Drawing.Size(43, 17);
            this.lblCenter.TabIndex = 0;
            this.lblCenter.Text = "label1";
            this.lblCenter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape2,
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(127, 119);
            this.shapeContainer1.TabIndex = 1;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape2
            // 
            this.lineShape2.BorderStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = 65;
            this.lineShape2.X2 = 65;
            this.lineShape2.Y1 = 0;
            this.lineShape2.Y2 = 118;
            // 
            // lineShape1
            // 
            this.lineShape1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 1;
            this.lineShape1.X2 = 124;
            this.lineShape1.Y1 = 60;
            this.lineShape1.Y2 = 60;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(47, 277);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(98, 31);
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "추가";
            this.btnConfirm.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(196, 277);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(98, 31);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // boxTop
            // 
            this.boxTop.Caption = "Top";
            this.boxTop.CaptionVisible = true;
            this.boxTop.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.boxTop.Location = new System.Drawing.Point(79, 24);
            this.boxTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.boxTop.Name = "boxTop";
            this.boxTop.Size = new System.Drawing.Size(51, 47);
            this.boxTop.TabIndex = 15;
            this.boxTop.Value = 0;
            // 
            // boxLeft
            // 
            this.boxLeft.Caption = "Left";
            this.boxLeft.CaptionVisible = true;
            this.boxLeft.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.boxLeft.Location = new System.Drawing.Point(25, 54);
            this.boxLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.boxLeft.Name = "boxLeft";
            this.boxLeft.Size = new System.Drawing.Size(51, 62);
            this.boxLeft.TabIndex = 16;
            this.boxLeft.Value = 0;
            // 
            // boxRight
            // 
            this.boxRight.Caption = "Right";
            this.boxRight.CaptionVisible = true;
            this.boxRight.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.boxRight.Location = new System.Drawing.Point(209, 145);
            this.boxRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.boxRight.Name = "boxRight";
            this.boxRight.Size = new System.Drawing.Size(61, 49);
            this.boxRight.TabIndex = 17;
            this.boxRight.Value = 0;
            // 
            // boxBottom
            // 
            this.boxBottom.Caption = "Bottom";
            this.boxBottom.CaptionVisible = true;
            this.boxBottom.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.boxBottom.Location = new System.Drawing.Point(142, 198);
            this.boxBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.boxBottom.Name = "boxBottom";
            this.boxBottom.Size = new System.Drawing.Size(64, 49);
            this.boxBottom.TabIndex = 18;
            this.boxBottom.Value = 0;
            // 
            // cmbLane
            // 
            this.cmbLane.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbLane.FormattingEnabled = true;
            this.cmbLane.Items.AddRange(new object[] {
            "Lane1",
            "Lane2"});
            this.cmbLane.Location = new System.Drawing.Point(144, 21);
            this.cmbLane.Name = "cmbLane";
            this.cmbLane.Size = new System.Drawing.Size(73, 22);
            this.cmbLane.TabIndex = 19;
            this.cmbLane.Text = "Lane1";
            // 
            // cmbSequence
            // 
            this.cmbSequence.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSequence.FormattingEnabled = true;
            this.cmbSequence.Items.AddRange(new object[] {
            "SEQ0",
            "SEQ1",
            "SEQ2",
            "SEQ3",
            "SEQ4",
            "SEQ5",
            "SEQ6",
            "SEQ7",
            "SEQ8",
            "SEQ9",
            "SEQA",
            "SEQB"});
            this.cmbSequence.Location = new System.Drawing.Point(234, 21);
            this.cmbSequence.Name = "cmbSequence";
            this.cmbSequence.Size = new System.Drawing.Size(73, 22);
            this.cmbSequence.TabIndex = 20;
            this.cmbSequence.Text = "SEQ0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(141, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 14);
            this.label1.TabIndex = 21;
            this.label1.Text = "Lane";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(231, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 22;
            this.label2.Text = "Sequence";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(231, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 14);
            this.label3.TabIndex = 23;
            this.label3.Text = "Direction";
            this.label3.Visible = false;
            // 
            // cmbDirection
            // 
            this.cmbDirection.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDirection.FormattingEnabled = true;
            this.cmbDirection.Items.AddRange(new object[] {
            "Side1",
            "Side2",
            "Side3"});
            this.cmbDirection.Location = new System.Drawing.Point(234, 71);
            this.cmbDirection.Name = "cmbDirection";
            this.cmbDirection.Size = new System.Drawing.Size(73, 22);
            this.cmbDirection.TabIndex = 24;
            this.cmbDirection.Text = "Side1";
            this.cmbDirection.Visible = false;
            // 
            // frmRoiSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(334, 320);
            this.Controls.Add(this.cmbDirection);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbSequence);
            this.Controls.Add(this.cmbLane);
            this.Controls.Add(this.boxBottom);
            this.Controls.Add(this.boxRight);
            this.Controls.Add(this.boxLeft);
            this.Controls.Add(this.boxTop);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRoiSet";
            this.Text = "ROI SETTING";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public IntTextBox.IntTextBox boxTop;
        public IntTextBox.IntTextBox boxLeft;
        public IntTextBox.IntTextBox boxRight;
        public IntTextBox.IntTextBox boxBottom;
        public System.Windows.Forms.Label lblCenter;
        public System.Windows.Forms.Button btnConfirm;
        public System.Windows.Forms.Button btnCancel;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox cmbLane;
        public System.Windows.Forms.ComboBox cmbSequence;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox cmbDirection;
    }
}