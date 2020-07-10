namespace SnpSmartVision
{
    partial class RoiOffsetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoiOffsetForm));
            this.buttonRoiOffsetCancel = new System.Windows.Forms.Button();
            this.buttonRoiOffsetOk = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericEdit1 = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.numericEdit2 = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericEdit2)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonRoiOffsetCancel
            // 
            this.buttonRoiOffsetCancel.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRoiOffsetCancel.Location = new System.Drawing.Point(26, 155);
            this.buttonRoiOffsetCancel.Name = "buttonRoiOffsetCancel";
            this.buttonRoiOffsetCancel.Size = new System.Drawing.Size(92, 34);
            this.buttonRoiOffsetCancel.TabIndex = 0;
            this.buttonRoiOffsetCancel.Text = "Close";
            this.buttonRoiOffsetCancel.UseVisualStyleBackColor = true;
            // 
            // buttonRoiOffsetOk
            // 
            this.buttonRoiOffsetOk.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRoiOffsetOk.Location = new System.Drawing.Point(176, 155);
            this.buttonRoiOffsetOk.Name = "buttonRoiOffsetOk";
            this.buttonRoiOffsetOk.Size = new System.Drawing.Size(92, 34);
            this.buttonRoiOffsetOk.TabIndex = 1;
            this.buttonRoiOffsetOk.Text = "Set";
            this.buttonRoiOffsetOk.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(16, 44);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(102, 23);
            this.comboBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Cameras";
            // 
            // numericEdit1
            // 
            this.numericEdit1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericEdit1.Location = new System.Drawing.Point(189, 46);
            this.numericEdit1.Name = "numericEdit1";
            this.numericEdit1.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange;
            this.numericEdit1.Range = new NationalInstruments.UI.Range(-200D, 200D);
            this.numericEdit1.Size = new System.Drawing.Size(92, 26);
            this.numericEdit1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(186, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Offset";
            // 
            // numericEdit2
            // 
            this.numericEdit2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericEdit2.Location = new System.Drawing.Point(189, 83);
            this.numericEdit2.Name = "numericEdit2";
            this.numericEdit2.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange;
            this.numericEdit2.Range = new NationalInstruments.UI.Range(-200D, 200D);
            this.numericEdit2.Size = new System.Drawing.Size(92, 26);
            this.numericEdit2.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(158, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "X";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(158, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Y";
            // 
            // RoiOffsetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 224);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericEdit2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericEdit1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.buttonRoiOffsetOk);
            this.Controls.Add(this.buttonRoiOffsetCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RoiOffsetForm";
            this.Text = "ROI OFFSET";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.numericEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericEdit2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        public NationalInstruments.UI.WindowsForms.NumericEdit numericEdit1;
        private System.Windows.Forms.Label label2;
        public NationalInstruments.UI.WindowsForms.NumericEdit numericEdit2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Button buttonRoiOffsetCancel;
        public System.Windows.Forms.Button buttonRoiOffsetOk;
    }
}