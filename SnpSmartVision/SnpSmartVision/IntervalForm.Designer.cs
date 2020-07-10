namespace SnpSmartVision
{
    partial class IntervalForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IntervalForm));
            this.knob1 = new NationalInstruments.UI.WindowsForms.Knob();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonIntervalExit = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.knob1)).BeginInit();
            this.SuspendLayout();
            // 
            // knob1
            // 
            this.knob1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.knob1.KnobStyle = NationalInstruments.UI.KnobStyle.RaisedWithThinNeedle3D;
            this.knob1.Location = new System.Drawing.Point(12, 12);
            this.knob1.Name = "knob1";
            this.knob1.Range = new NationalInstruments.UI.Range(1D, 10D);
            this.knob1.Size = new System.Drawing.Size(183, 171);
            this.knob1.TabIndex = 0;
            this.knob1.Value = 1D;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(36, 186);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 19);
            this.label1.TabIndex = 1;
            // 
            // buttonIntervalExit
            // 
            this.buttonIntervalExit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonIntervalExit.Location = new System.Drawing.Point(183, 223);
            this.buttonIntervalExit.Name = "buttonIntervalExit";
            this.buttonIntervalExit.Size = new System.Drawing.Size(75, 27);
            this.buttonIntervalExit.TabIndex = 2;
            this.buttonIntervalExit.Text = "Exit";
            this.buttonIntervalExit.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(211, 23);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(47, 19);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "X10";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // buttonUp
            // 
            this.buttonUp.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUp.Location = new System.Drawing.Point(211, 48);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(45, 30);
            this.buttonUp.TabIndex = 4;
            this.buttonUp.Text = "UP";
            this.buttonUp.UseVisualStyleBackColor = true;
            // 
            // buttonDown
            // 
            this.buttonDown.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDown.Location = new System.Drawing.Point(211, 84);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(45, 30);
            this.buttonDown.TabIndex = 5;
            this.buttonDown.Text = "DN";
            this.buttonDown.UseVisualStyleBackColor = true;
            // 
            // IntervalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.buttonIntervalExit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.knob1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IntervalForm";
            this.Text = "Trigger Interval";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.knob1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public NationalInstruments.UI.WindowsForms.Knob knob1;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button buttonIntervalExit;
        public System.Windows.Forms.CheckBox checkBox1;
        public System.Windows.Forms.Button buttonUp;
        public System.Windows.Forms.Button buttonDown;
    }
}