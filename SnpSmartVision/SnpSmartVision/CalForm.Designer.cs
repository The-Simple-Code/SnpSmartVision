namespace SnpSmartVision
{
    partial class CalForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalForm));
            this.numericX = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelXpt = new System.Windows.Forms.Label();
            this.labelYpt = new System.Windows.Forms.Label();
            this.labelOutX = new System.Windows.Forms.Label();
            this.buttonCalExit = new System.Windows.Forms.Button();
            this.buttonCalOk = new System.Windows.Forms.Button();
            this.buttonCalMeasure = new System.Windows.Forms.Button();
            this.buttonCalDo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericX)).BeginInit();
            this.SuspendLayout();
            // 
            // numericX
            // 
            this.numericX.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericX.Location = new System.Drawing.Point(171, 40);
            this.numericX.Name = "numericX";
            this.numericX.Size = new System.Drawing.Size(100, 26);
            this.numericX.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(168, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "X[mm]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(34, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "X[pt]";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(34, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 18);
            this.label4.TabIndex = 5;
            this.label4.Text = "Y[pt]";
            // 
            // labelXpt
            // 
            this.labelXpt.AutoSize = true;
            this.labelXpt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelXpt.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXpt.Location = new System.Drawing.Point(34, 40);
            this.labelXpt.Name = "labelXpt";
            this.labelXpt.Size = new System.Drawing.Size(18, 20);
            this.labelXpt.TabIndex = 6;
            this.labelXpt.Text = "0";
            // 
            // labelYpt
            // 
            this.labelYpt.AutoSize = true;
            this.labelYpt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelYpt.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelYpt.Location = new System.Drawing.Point(34, 96);
            this.labelYpt.Name = "labelYpt";
            this.labelYpt.Size = new System.Drawing.Size(18, 20);
            this.labelYpt.TabIndex = 7;
            this.labelYpt.Text = "0";
            // 
            // labelOutX
            // 
            this.labelOutX.AutoSize = true;
            this.labelOutX.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutX.Location = new System.Drawing.Point(31, 188);
            this.labelOutX.Name = "labelOutX";
            this.labelOutX.Size = new System.Drawing.Size(14, 15);
            this.labelOutX.TabIndex = 9;
            this.labelOutX.Text = "0";
            // 
            // buttonCalExit
            // 
            this.buttonCalExit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCalExit.Location = new System.Drawing.Point(49, 219);
            this.buttonCalExit.Name = "buttonCalExit";
            this.buttonCalExit.Size = new System.Drawing.Size(81, 30);
            this.buttonCalExit.TabIndex = 10;
            this.buttonCalExit.Text = "Exit";
            this.buttonCalExit.UseVisualStyleBackColor = true;
            // 
            // buttonCalOk
            // 
            this.buttonCalOk.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCalOk.Location = new System.Drawing.Point(158, 219);
            this.buttonCalOk.Name = "buttonCalOk";
            this.buttonCalOk.Size = new System.Drawing.Size(81, 30);
            this.buttonCalOk.TabIndex = 11;
            this.buttonCalOk.Text = "Ok";
            this.buttonCalOk.UseVisualStyleBackColor = true;
            // 
            // buttonCalMeasure
            // 
            this.buttonCalMeasure.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCalMeasure.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCalMeasure.Image = global::SnpSmartVision.Properties.Resources.Measure;
            this.buttonCalMeasure.Location = new System.Drawing.Point(220, 136);
            this.buttonCalMeasure.Name = "buttonCalMeasure";
            this.buttonCalMeasure.Size = new System.Drawing.Size(51, 43);
            this.buttonCalMeasure.TabIndex = 13;
            this.buttonCalMeasure.UseVisualStyleBackColor = true;
            // 
            // buttonCalDo
            // 
            this.buttonCalDo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCalDo.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCalDo.Image = global::SnpSmartVision.Properties.Resources.Calibration;
            this.buttonCalDo.Location = new System.Drawing.Point(220, 77);
            this.buttonCalDo.Name = "buttonCalDo";
            this.buttonCalDo.Size = new System.Drawing.Size(51, 39);
            this.buttonCalDo.TabIndex = 12;
            this.buttonCalDo.UseVisualStyleBackColor = true;
            // 
            // CalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 261);
            this.Controls.Add(this.buttonCalMeasure);
            this.Controls.Add(this.buttonCalDo);
            this.Controls.Add(this.buttonCalOk);
            this.Controls.Add(this.buttonCalExit);
            this.Controls.Add(this.labelOutX);
            this.Controls.Add(this.labelYpt);
            this.Controls.Add(this.labelXpt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericX);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalForm";
            this.Text = "Calibration";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.numericX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public NationalInstruments.UI.WindowsForms.NumericEdit numericX;
        public System.Windows.Forms.Label labelXpt;
        public System.Windows.Forms.Label labelYpt;
        public System.Windows.Forms.Label labelOutX;
        public System.Windows.Forms.Button buttonCalExit;
        public System.Windows.Forms.Button buttonCalOk;
        public System.Windows.Forms.Button buttonCalDo;
        public System.Windows.Forms.Button buttonCalMeasure;
    }
}