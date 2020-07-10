namespace SnpSmartVision
{
    partial class CamAttrForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CamAttrForm));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.tabControl = new System.Windows.Forms.TabControl();
            this.emptyPage = new System.Windows.Forms.TabPage();
            this.label16 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.colorThresholdPage = new System.Windows.Forms.TabPage();
            this.buttonColor = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboImageType = new System.Windows.Forms.ComboBox();
            this.medianFilterPage = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericHeight = new System.Windows.Forms.NumericUpDown();
            this.numericWidth = new System.Windows.Forms.NumericUpDown();
            this.particleRemovingPage = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.comboConnectivity = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboSizeToKeep = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numericErosion = new System.Windows.Forms.NumericUpDown();
            this.filterOptionPage = new System.Windows.Forms.TabPage();
            this.checkFillHole = new System.Windows.Forms.CheckBox();
            this.checkRejectBorder = new System.Windows.Forms.CheckBox();
            this.checkRejectMatch = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboConnectivity1 = new System.Windows.Forms.ComboBox();
            this.filterCriteriaPage = new System.Windows.Forms.TabPage();
            this.buttonCriteria = new System.Windows.Forms.Button();
            this.reportPage = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.comboConnectivity2 = new System.Windows.Forms.ComboBox();
            this.displayColorPage = new System.Windows.Forms.TabPage();
            this.panelColor = new System.Windows.Forms.Panel();
            this.extraPage = new System.Windows.Forms.TabPage();
            this.comboComposit = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.panelExtraColor = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.comboExtraImageType = new System.Windows.Forms.ComboBox();
            this.buttonXColor = new System.Windows.Forms.Button();
            this.checkEnabled = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboContained = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboCombined = new System.Windows.Forms.ComboBox();
            this.tabControl.SuspendLayout();
            this.emptyPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.colorThresholdPage.SuspendLayout();
            this.medianFilterPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericWidth)).BeginInit();
            this.particleRemovingPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericErosion)).BeginInit();
            this.filterOptionPage.SuspendLayout();
            this.filterCriteriaPage.SuspendLayout();
            this.reportPage.SuspendLayout();
            this.displayColorPage.SuspendLayout();
            this.extraPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(401, 932);
            this.treeView1.TabIndex = 0;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "camera1.ico");
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.emptyPage);
            this.tabControl.Controls.Add(this.colorThresholdPage);
            this.tabControl.Controls.Add(this.medianFilterPage);
            this.tabControl.Controls.Add(this.particleRemovingPage);
            this.tabControl.Controls.Add(this.filterOptionPage);
            this.tabControl.Controls.Add(this.filterCriteriaPage);
            this.tabControl.Controls.Add(this.reportPage);
            this.tabControl.Controls.Add(this.displayColorPage);
            this.tabControl.Controls.Add(this.extraPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(0, 788);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(401, 144);
            this.tabControl.TabIndex = 14;
            // 
            // emptyPage
            // 
            this.emptyPage.Controls.Add(this.label16);
            this.emptyPage.Controls.Add(this.label3);
            this.emptyPage.Controls.Add(this.label1);
            this.emptyPage.Controls.Add(this.pictureBox1);
            this.emptyPage.Location = new System.Drawing.Point(4, 23);
            this.emptyPage.Name = "emptyPage";
            this.emptyPage.Padding = new System.Windows.Forms.Padding(3);
            this.emptyPage.Size = new System.Drawing.Size(393, 117);
            this.emptyPage.TabIndex = 6;
            this.emptyPage.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(129, 54);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(90, 19);
            this.label16.TabIndex = 4;
            this.label16.Text = "Ver.1.0.5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(78, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(252, 19);
            this.label3.TabIndex = 3;
            this.label3.Text = "Copyright(c) by S&&P SYSTEM ";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(129, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "SNP SMART VISION";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SnpSmartVision.Properties.Resources.rainbow_47311;
            this.pictureBox1.Location = new System.Drawing.Point(22, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(29, 27);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // colorThresholdPage
            // 
            this.colorThresholdPage.Controls.Add(this.buttonColor);
            this.colorThresholdPage.Controls.Add(this.label2);
            this.colorThresholdPage.Controls.Add(this.comboImageType);
            this.colorThresholdPage.Location = new System.Drawing.Point(4, 23);
            this.colorThresholdPage.Name = "colorThresholdPage";
            this.colorThresholdPage.Size = new System.Drawing.Size(393, 117);
            this.colorThresholdPage.TabIndex = 1;
            this.colorThresholdPage.Text = "Color Threshold";
            this.colorThresholdPage.UseVisualStyleBackColor = true;
            // 
            // buttonColor
            // 
            this.buttonColor.Location = new System.Drawing.Point(219, 27);
            this.buttonColor.Name = "buttonColor";
            this.buttonColor.Size = new System.Drawing.Size(119, 30);
            this.buttonColor.TabIndex = 2;
            this.buttonColor.Text = "Color";
            this.buttonColor.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Image Type";
            // 
            // comboImageType
            // 
            this.comboImageType.FormattingEnabled = true;
            this.comboImageType.Items.AddRange(new object[] {
            "RGB",
            "HSL",
            "HSI",
            "HSV"});
            this.comboImageType.Location = new System.Drawing.Point(48, 32);
            this.comboImageType.Name = "comboImageType";
            this.comboImageType.Size = new System.Drawing.Size(107, 22);
            this.comboImageType.TabIndex = 0;
            // 
            // medianFilterPage
            // 
            this.medianFilterPage.Controls.Add(this.label5);
            this.medianFilterPage.Controls.Add(this.label4);
            this.medianFilterPage.Controls.Add(this.numericHeight);
            this.medianFilterPage.Controls.Add(this.numericWidth);
            this.medianFilterPage.Location = new System.Drawing.Point(4, 23);
            this.medianFilterPage.Name = "medianFilterPage";
            this.medianFilterPage.Size = new System.Drawing.Size(393, 117);
            this.medianFilterPage.TabIndex = 2;
            this.medianFilterPage.Text = "Median Filter";
            this.medianFilterPage.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(226, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 14);
            this.label5.TabIndex = 3;
            this.label5.Text = "Height";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 14);
            this.label4.TabIndex = 2;
            this.label4.Text = "Width";
            // 
            // numericHeight
            // 
            this.numericHeight.Location = new System.Drawing.Point(229, 40);
            this.numericHeight.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericHeight.Name = "numericHeight";
            this.numericHeight.Size = new System.Drawing.Size(98, 22);
            this.numericHeight.TabIndex = 1;
            this.numericHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericWidth
            // 
            this.numericWidth.Location = new System.Drawing.Point(48, 40);
            this.numericWidth.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericWidth.Name = "numericWidth";
            this.numericWidth.Size = new System.Drawing.Size(98, 22);
            this.numericWidth.TabIndex = 0;
            this.numericWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // particleRemovingPage
            // 
            this.particleRemovingPage.Controls.Add(this.label8);
            this.particleRemovingPage.Controls.Add(this.comboConnectivity);
            this.particleRemovingPage.Controls.Add(this.label7);
            this.particleRemovingPage.Controls.Add(this.comboSizeToKeep);
            this.particleRemovingPage.Controls.Add(this.label6);
            this.particleRemovingPage.Controls.Add(this.numericErosion);
            this.particleRemovingPage.Location = new System.Drawing.Point(4, 23);
            this.particleRemovingPage.Name = "particleRemovingPage";
            this.particleRemovingPage.Padding = new System.Windows.Forms.Padding(3);
            this.particleRemovingPage.Size = new System.Drawing.Size(393, 117);
            this.particleRemovingPage.TabIndex = 3;
            this.particleRemovingPage.Text = "Partcle Removing";
            this.particleRemovingPage.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(252, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 14);
            this.label8.TabIndex = 7;
            this.label8.Text = "Connectivity";
            // 
            // comboConnectivity
            // 
            this.comboConnectivity.FormattingEnabled = true;
            this.comboConnectivity.Items.AddRange(new object[] {
            "Connectivity8",
            "Connectivity4"});
            this.comboConnectivity.Location = new System.Drawing.Point(255, 34);
            this.comboConnectivity.Name = "comboConnectivity";
            this.comboConnectivity.Size = new System.Drawing.Size(99, 22);
            this.comboConnectivity.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(129, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 14);
            this.label7.TabIndex = 5;
            this.label7.Text = "Size to Keep";
            // 
            // comboSizeToKeep
            // 
            this.comboSizeToKeep.FormattingEnabled = true;
            this.comboSizeToKeep.Items.AddRange(new object[] {
            "KeepLarge",
            "KeepSmall"});
            this.comboSizeToKeep.Location = new System.Drawing.Point(132, 34);
            this.comboSizeToKeep.Name = "comboSizeToKeep";
            this.comboSizeToKeep.Size = new System.Drawing.Size(99, 22);
            this.comboSizeToKeep.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 14);
            this.label6.TabIndex = 3;
            this.label6.Text = "Erosion";
            // 
            // numericErosion
            // 
            this.numericErosion.Location = new System.Drawing.Point(8, 34);
            this.numericErosion.Name = "numericErosion";
            this.numericErosion.Size = new System.Drawing.Size(102, 22);
            this.numericErosion.TabIndex = 1;
            this.numericErosion.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // filterOptionPage
            // 
            this.filterOptionPage.Controls.Add(this.checkFillHole);
            this.filterOptionPage.Controls.Add(this.checkRejectBorder);
            this.filterOptionPage.Controls.Add(this.checkRejectMatch);
            this.filterOptionPage.Controls.Add(this.label9);
            this.filterOptionPage.Controls.Add(this.comboConnectivity1);
            this.filterOptionPage.Location = new System.Drawing.Point(4, 23);
            this.filterOptionPage.Name = "filterOptionPage";
            this.filterOptionPage.Size = new System.Drawing.Size(393, 117);
            this.filterOptionPage.TabIndex = 4;
            this.filterOptionPage.Text = "Particle Filter Option";
            this.filterOptionPage.UseVisualStyleBackColor = true;
            // 
            // checkFillHole
            // 
            this.checkFillHole.AutoSize = true;
            this.checkFillHole.Location = new System.Drawing.Point(267, 23);
            this.checkFillHole.Name = "checkFillHole";
            this.checkFillHole.Size = new System.Drawing.Size(89, 18);
            this.checkFillHole.TabIndex = 11;
            this.checkFillHole.Text = "Fill Hole";
            this.checkFillHole.UseVisualStyleBackColor = true;
            // 
            // checkRejectBorder
            // 
            this.checkRejectBorder.AutoSize = true;
            this.checkRejectBorder.Location = new System.Drawing.Point(142, 47);
            this.checkRejectBorder.Name = "checkRejectBorder";
            this.checkRejectBorder.Size = new System.Drawing.Size(117, 18);
            this.checkRejectBorder.TabIndex = 10;
            this.checkRejectBorder.Text = "Reject Border";
            this.checkRejectBorder.UseVisualStyleBackColor = true;
            // 
            // checkRejectMatch
            // 
            this.checkRejectMatch.AutoSize = true;
            this.checkRejectMatch.Location = new System.Drawing.Point(142, 23);
            this.checkRejectMatch.Name = "checkRejectMatch";
            this.checkRejectMatch.Size = new System.Drawing.Size(110, 18);
            this.checkRejectMatch.TabIndex = 9;
            this.checkRejectMatch.Text = "Reject Match";
            this.checkRejectMatch.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 14);
            this.label9.TabIndex = 8;
            this.label9.Text = "Connectivity";
            // 
            // comboConnectivity1
            // 
            this.comboConnectivity1.FormattingEnabled = true;
            this.comboConnectivity1.Items.AddRange(new object[] {
            "Connectivity8",
            "Connectivity4"});
            this.comboConnectivity1.Location = new System.Drawing.Point(9, 40);
            this.comboConnectivity1.Name = "comboConnectivity1";
            this.comboConnectivity1.Size = new System.Drawing.Size(99, 22);
            this.comboConnectivity1.TabIndex = 7;
            // 
            // filterCriteriaPage
            // 
            this.filterCriteriaPage.Controls.Add(this.buttonCriteria);
            this.filterCriteriaPage.Location = new System.Drawing.Point(4, 23);
            this.filterCriteriaPage.Name = "filterCriteriaPage";
            this.filterCriteriaPage.Size = new System.Drawing.Size(393, 117);
            this.filterCriteriaPage.TabIndex = 5;
            this.filterCriteriaPage.Text = "Particle Filter Criteria";
            this.filterCriteriaPage.UseVisualStyleBackColor = true;
            // 
            // buttonCriteria
            // 
            this.buttonCriteria.Location = new System.Drawing.Point(123, 28);
            this.buttonCriteria.Name = "buttonCriteria";
            this.buttonCriteria.Size = new System.Drawing.Size(130, 27);
            this.buttonCriteria.TabIndex = 0;
            this.buttonCriteria.Text = "Criteria";
            this.buttonCriteria.UseVisualStyleBackColor = true;
            // 
            // reportPage
            // 
            this.reportPage.Controls.Add(this.label10);
            this.reportPage.Controls.Add(this.comboConnectivity2);
            this.reportPage.Location = new System.Drawing.Point(4, 23);
            this.reportPage.Name = "reportPage";
            this.reportPage.Size = new System.Drawing.Size(393, 117);
            this.reportPage.TabIndex = 7;
            this.reportPage.Text = "Report";
            this.reportPage.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(128, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(91, 14);
            this.label10.TabIndex = 10;
            this.label10.Text = "Connectivity";
            // 
            // comboConnectivity2
            // 
            this.comboConnectivity2.FormattingEnabled = true;
            this.comboConnectivity2.Items.AddRange(new object[] {
            "Connectivity8",
            "Connectivity4"});
            this.comboConnectivity2.Location = new System.Drawing.Point(131, 40);
            this.comboConnectivity2.Name = "comboConnectivity2";
            this.comboConnectivity2.Size = new System.Drawing.Size(99, 22);
            this.comboConnectivity2.TabIndex = 9;
            // 
            // displayColorPage
            // 
            this.displayColorPage.Controls.Add(this.panelColor);
            this.displayColorPage.Location = new System.Drawing.Point(4, 23);
            this.displayColorPage.Name = "displayColorPage";
            this.displayColorPage.Size = new System.Drawing.Size(393, 117);
            this.displayColorPage.TabIndex = 8;
            this.displayColorPage.Text = "Display Color";
            this.displayColorPage.UseVisualStyleBackColor = true;
            // 
            // panelColor
            // 
            this.panelColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColor.Location = new System.Drawing.Point(168, 35);
            this.panelColor.Name = "panelColor";
            this.panelColor.Size = new System.Drawing.Size(45, 42);
            this.panelColor.TabIndex = 0;
            // 
            // extraPage
            // 
            this.extraPage.Controls.Add(this.comboComposit);
            this.extraPage.Controls.Add(this.label15);
            this.extraPage.Controls.Add(this.label14);
            this.extraPage.Controls.Add(this.panelExtraColor);
            this.extraPage.Controls.Add(this.label13);
            this.extraPage.Controls.Add(this.comboExtraImageType);
            this.extraPage.Controls.Add(this.buttonXColor);
            this.extraPage.Controls.Add(this.checkEnabled);
            this.extraPage.Controls.Add(this.label12);
            this.extraPage.Controls.Add(this.comboContained);
            this.extraPage.Controls.Add(this.label11);
            this.extraPage.Controls.Add(this.comboCombined);
            this.extraPage.Location = new System.Drawing.Point(4, 23);
            this.extraPage.Name = "extraPage";
            this.extraPage.Size = new System.Drawing.Size(393, 117);
            this.extraPage.TabIndex = 9;
            this.extraPage.Text = "Extra Parameter";
            this.extraPage.UseVisualStyleBackColor = true;
            // 
            // comboComposit
            // 
            this.comboComposit.FormattingEnabled = true;
            this.comboComposit.Items.AddRange(new object[] {
            "NotComposit",
            "XimageComposit",
            "Image2Composit"});
            this.comboComposit.Location = new System.Drawing.Point(128, 72);
            this.comboComposit.Name = "comboComposit";
            this.comboComposit.Size = new System.Drawing.Size(97, 22);
            this.comboComposit.TabIndex = 21;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(125, 55);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(112, 14);
            this.label15.TabIndex = 20;
            this.label15.Text = "Composit Method";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(255, 65);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(98, 14);
            this.label14.TabIndex = 19;
            this.label14.Text = "Display Color";
            // 
            // panelExtraColor
            // 
            this.panelExtraColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelExtraColor.Location = new System.Drawing.Point(273, 82);
            this.panelExtraColor.Name = "panelExtraColor";
            this.panelExtraColor.Size = new System.Drawing.Size(38, 29);
            this.panelExtraColor.TabIndex = 18;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 55);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 14);
            this.label13.TabIndex = 17;
            this.label13.Text = "Image Type";
            // 
            // comboExtraImageType
            // 
            this.comboExtraImageType.FormattingEnabled = true;
            this.comboExtraImageType.Items.AddRange(new object[] {
            "RGB",
            "HSL",
            "HSI",
            "HSV"});
            this.comboExtraImageType.Location = new System.Drawing.Point(8, 72);
            this.comboExtraImageType.Name = "comboExtraImageType";
            this.comboExtraImageType.Size = new System.Drawing.Size(107, 22);
            this.comboExtraImageType.TabIndex = 16;
            // 
            // buttonXColor
            // 
            this.buttonXColor.Location = new System.Drawing.Point(258, 25);
            this.buttonXColor.Name = "buttonXColor";
            this.buttonXColor.Size = new System.Drawing.Size(75, 30);
            this.buttonXColor.TabIndex = 15;
            this.buttonXColor.Text = "Color";
            this.buttonXColor.UseVisualStyleBackColor = true;
            // 
            // checkEnabled
            // 
            this.checkEnabled.AutoSize = true;
            this.checkEnabled.Location = new System.Drawing.Point(10, 100);
            this.checkEnabled.Name = "checkEnabled";
            this.checkEnabled.Size = new System.Drawing.Size(75, 18);
            this.checkEnabled.TabIndex = 14;
            this.checkEnabled.Text = "Enabled";
            this.checkEnabled.UseVisualStyleBackColor = true;
            this.checkEnabled.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(122, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(105, 14);
            this.label12.TabIndex = 13;
            this.label12.Text = "Contained Type";
            // 
            // comboContained
            // 
            this.comboContained.FormattingEnabled = true;
            this.comboContained.Items.AddRange(new object[] {
            "Include",
            "Exclude"});
            this.comboContained.Location = new System.Drawing.Point(128, 30);
            this.comboContained.Name = "comboContained";
            this.comboContained.Size = new System.Drawing.Size(99, 22);
            this.comboContained.TabIndex = 12;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 13);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 14);
            this.label11.TabIndex = 11;
            this.label11.Text = "Combined Target";
            // 
            // comboCombined
            // 
            this.comboCombined.FormattingEnabled = true;
            this.comboCombined.Items.AddRange(new object[] {
            "Process1",
            "Process2",
            "All"});
            this.comboCombined.Location = new System.Drawing.Point(8, 30);
            this.comboCombined.Name = "comboCombined";
            this.comboCombined.Size = new System.Drawing.Size(107, 22);
            this.comboCombined.TabIndex = 1;
            // 
            // CamAttrForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(401, 932);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.treeView1);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight;
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CamAttrForm";
            this.Text = "Form1";
            this.tabControl.ResumeLayout(false);
            this.emptyPage.ResumeLayout(false);
            this.emptyPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.colorThresholdPage.ResumeLayout(false);
            this.colorThresholdPage.PerformLayout();
            this.medianFilterPage.ResumeLayout(false);
            this.medianFilterPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericWidth)).EndInit();
            this.particleRemovingPage.ResumeLayout(false);
            this.particleRemovingPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericErosion)).EndInit();
            this.filterOptionPage.ResumeLayout(false);
            this.filterOptionPage.PerformLayout();
            this.filterCriteriaPage.ResumeLayout(false);
            this.reportPage.ResumeLayout(false);
            this.reportPage.PerformLayout();
            this.displayColorPage.ResumeLayout(false);
            this.extraPage.ResumeLayout(false);
            this.extraPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList;
        public System.Windows.Forms.TabControl tabControl;
        public System.Windows.Forms.TabPage emptyPage;
        public System.Windows.Forms.TabPage colorThresholdPage;
        public System.Windows.Forms.TabPage medianFilterPage;
        public System.Windows.Forms.TabPage particleRemovingPage;
        public System.Windows.Forms.TabPage filterOptionPage;
        public System.Windows.Forms.TabPage filterCriteriaPage;
        private System.Windows.Forms.TabPage reportPage;
        private System.Windows.Forms.TabPage displayColorPage;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button buttonColor;
        public System.Windows.Forms.ComboBox comboImageType;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.NumericUpDown numericHeight;
        public System.Windows.Forms.NumericUpDown numericWidth;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.NumericUpDown numericErosion;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.ComboBox comboConnectivity;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.ComboBox comboSizeToKeep;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.ComboBox comboConnectivity1;
        public System.Windows.Forms.CheckBox checkFillHole;
        public System.Windows.Forms.CheckBox checkRejectBorder;
        public System.Windows.Forms.CheckBox checkRejectMatch;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.ComboBox comboConnectivity2;
        public System.Windows.Forms.Panel panelColor;
        public System.Windows.Forms.Button buttonCriteria;
        private System.Windows.Forms.TabPage extraPage;
        public System.Windows.Forms.Button buttonXColor;
        public System.Windows.Forms.CheckBox checkEnabled;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.ComboBox comboContained;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.ComboBox comboCombined;
        public System.Windows.Forms.Panel panelExtraColor;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.ComboBox comboExtraImageType;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.ComboBox comboComposit;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.Label label16;


    }
}