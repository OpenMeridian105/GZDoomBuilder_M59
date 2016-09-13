namespace CodeImp.DoomBuilder.Windows
{
	partial class SectorEditFormMeridian
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
			if(disposing && (components != null))
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
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Label label9;
			System.Windows.Forms.GroupBox groupfloorceiling;
			System.Windows.Forms.Label label8;
			System.Windows.Forms.Label label5;
			System.Windows.Forms.Label label6;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label4;
			System.Windows.Forms.Label label10;
			this.sectortag = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.textureyoffset = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.label7 = new System.Windows.Forms.Label();
			this.texturexoffset = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.texXlabel = new System.Windows.Forms.Label();
			this.ceilingheight = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.sectorheightlabel = new System.Windows.Forms.Label();
			this.sectorheight = new System.Windows.Forms.Label();
			this.floortex = new CodeImp.DoomBuilder.Controls.FlatSelectorControl();
			this.floorheight = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.ceilingtex = new CodeImp.DoomBuilder.Controls.FlatSelectorControl();
			this.brightness = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.cancel = new System.Windows.Forms.Button();
			this.apply = new System.Windows.Forms.Button();
			this.flatSelectorControl2 = new CodeImp.DoomBuilder.Controls.FlatSelectorControl();
			this.flatSelectorControl1 = new CodeImp.DoomBuilder.Controls.FlatSelectorControl();
			this.panel1 = new System.Windows.Forms.Panel();
			this.slopebox = new System.Windows.Forms.GroupBox();
			this.floorvert1 = new System.Windows.Forms.ComboBox();
			this.floorvert1height = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.label14 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.ceiltexrot = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.label12 = new System.Windows.Forms.Label();
			this.floortexrot = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.label11 = new System.Windows.Forms.Label();
			this.depthbox = new System.Windows.Forms.GroupBox();
			this.depthvery = new System.Windows.Forms.RadioButton();
			this.depthdeep = new System.Windows.Forms.RadioButton();
			this.depthshallow = new System.Windows.Forms.RadioButton();
			this.depthnone = new System.Windows.Forms.RadioButton();
			this.Scrolling = new System.Windows.Forms.GroupBox();
			this.scrollceiling = new System.Windows.Forms.CheckBox();
			this.scrollfloor = new System.Windows.Forms.CheckBox();
			this.scrolldirection = new System.Windows.Forms.GroupBox();
			this.SE = new System.Windows.Forms.RadioButton();
			this.E = new System.Windows.Forms.RadioButton();
			this.NE = new System.Windows.Forms.RadioButton();
			this.S = new System.Windows.Forms.RadioButton();
			this.N = new System.Windows.Forms.RadioButton();
			this.SW = new System.Windows.Forms.RadioButton();
			this.W = new System.Windows.Forms.RadioButton();
			this.NW = new System.Windows.Forms.RadioButton();
			this.scrollspeed = new System.Windows.Forms.GroupBox();
			this.Fast = new System.Windows.Forms.RadioButton();
			this.Slow = new System.Windows.Forms.RadioButton();
			this.Medium = new System.Windows.Forms.RadioButton();
			this.None = new System.Windows.Forms.RadioButton();
			this.groupEffects = new System.Windows.Forms.GroupBox();
			this.animationspeed = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.noambientbox = new System.Windows.Forms.CheckBox();
			this.flickerbox = new System.Windows.Forms.CheckBox();
			this.tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.floorvert2 = new System.Windows.Forms.ComboBox();
			this.floorvert2height = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.floorvert3 = new System.Windows.Forms.ComboBox();
			this.floorvert3height = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.ceilvert3 = new System.Windows.Forms.ComboBox();
			this.ceilvert3height = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.ceilvert2 = new System.Windows.Forms.ComboBox();
			this.ceilvert2height = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.ceilvert1 = new System.Windows.Forms.ComboBox();
			this.ceilvert1height = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label9 = new System.Windows.Forms.Label();
			groupfloorceiling = new System.Windows.Forms.GroupBox();
			label8 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			label10 = new System.Windows.Forms.Label();
			groupfloorceiling.SuspendLayout();
			this.panel1.SuspendLayout();
			this.slopebox.SuspendLayout();
			this.depthbox.SuspendLayout();
			this.Scrolling.SuspendLayout();
			this.scrolldirection.SuspendLayout();
			this.scrollspeed.SuspendLayout();
			this.groupEffects.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.Location = new System.Drawing.Point(271, 18);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(83, 16);
			label1.TabIndex = 15;
			label1.Text = "Floor";
			label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label3
			// 
			label3.Location = new System.Drawing.Point(363, 18);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(83, 16);
			label3.TabIndex = 14;
			label3.Text = "Ceiling";
			label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label9
			// 
			label9.Location = new System.Drawing.Point(-6, 26);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(78, 14);
			label9.TabIndex = 2;
			label9.Text = "Light level:";
			label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupfloorceiling
			// 
			groupfloorceiling.BackColor = System.Drawing.Color.Transparent;
			groupfloorceiling.Controls.Add(this.sectortag);
			groupfloorceiling.Controls.Add(label8);
			groupfloorceiling.Controls.Add(this.textureyoffset);
			groupfloorceiling.Controls.Add(this.label7);
			groupfloorceiling.Controls.Add(this.texturexoffset);
			groupfloorceiling.Controls.Add(this.texXlabel);
			groupfloorceiling.Controls.Add(label5);
			groupfloorceiling.Controls.Add(label6);
			groupfloorceiling.Controls.Add(this.ceilingheight);
			groupfloorceiling.Controls.Add(label2);
			groupfloorceiling.Controls.Add(this.sectorheightlabel);
			groupfloorceiling.Controls.Add(label4);
			groupfloorceiling.Controls.Add(this.sectorheight);
			groupfloorceiling.Controls.Add(this.floortex);
			groupfloorceiling.Controls.Add(this.floorheight);
			groupfloorceiling.Controls.Add(this.ceilingtex);
			groupfloorceiling.Location = new System.Drawing.Point(3, 3);
			groupfloorceiling.Name = "groupfloorceiling";
			groupfloorceiling.Size = new System.Drawing.Size(460, 211);
			groupfloorceiling.TabIndex = 0;
			groupfloorceiling.TabStop = false;
			groupfloorceiling.Text = "Floor and Ceiling ";
			// 
			// sectortag
			// 
			this.sectortag.AllowDecimal = false;
			this.sectortag.AllowNegative = true;
			this.sectortag.AllowRelative = true;
			this.sectortag.ButtonStep = 8;
			this.sectortag.ButtonStepBig = 16F;
			this.sectortag.ButtonStepFloat = 1F;
			this.sectortag.ButtonStepSmall = 1F;
			this.sectortag.ButtonStepsUseModifierKeys = true;
			this.sectortag.ButtonStepsWrapAround = false;
			this.sectortag.Location = new System.Drawing.Point(99, 173);
			this.sectortag.Name = "sectortag";
			this.sectortag.Size = new System.Drawing.Size(73, 24);
			this.sectortag.StepValues = null;
			this.sectortag.TabIndex = 30;
			// 
			// label8
			// 
			label8.Location = new System.Drawing.Point(9, 178);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(78, 14);
			label8.TabIndex = 29;
			label8.Text = "Sector tag:";
			label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textureyoffset
			// 
			this.textureyoffset.AllowDecimal = false;
			this.textureyoffset.AllowNegative = true;
			this.textureyoffset.AllowRelative = true;
			this.textureyoffset.ButtonStep = 8;
			this.textureyoffset.ButtonStepBig = 16F;
			this.textureyoffset.ButtonStepFloat = 1F;
			this.textureyoffset.ButtonStepSmall = 1F;
			this.textureyoffset.ButtonStepsUseModifierKeys = true;
			this.textureyoffset.ButtonStepsWrapAround = false;
			this.textureyoffset.Location = new System.Drawing.Point(99, 143);
			this.textureyoffset.Name = "textureyoffset";
			this.textureyoffset.Size = new System.Drawing.Size(88, 24);
			this.textureyoffset.StepValues = null;
			this.textureyoffset.TabIndex = 28;
			this.textureyoffset.WhenTextChanged += new System.EventHandler(this.texYOffset_OnValuesChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 148);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(85, 13);
			this.label7.TabIndex = 27;
			this.label7.Text = "Texture Y offset:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// texturexoffset
			// 
			this.texturexoffset.AllowDecimal = false;
			this.texturexoffset.AllowNegative = true;
			this.texturexoffset.AllowRelative = true;
			this.texturexoffset.ButtonStep = 8;
			this.texturexoffset.ButtonStepBig = 16F;
			this.texturexoffset.ButtonStepFloat = 1F;
			this.texturexoffset.ButtonStepSmall = 1F;
			this.texturexoffset.ButtonStepsUseModifierKeys = true;
			this.texturexoffset.ButtonStepsWrapAround = false;
			this.texturexoffset.Location = new System.Drawing.Point(99, 113);
			this.texturexoffset.Name = "texturexoffset";
			this.texturexoffset.Size = new System.Drawing.Size(88, 24);
			this.texturexoffset.StepValues = null;
			this.texturexoffset.TabIndex = 26;
			this.texturexoffset.WhenTextChanged += new System.EventHandler(this.texXOffset_OnValuesChanged);
			// 
			// texXlabel
			// 
			this.texXlabel.AutoSize = true;
			this.texXlabel.Location = new System.Drawing.Point(9, 118);
			this.texXlabel.Name = "texXlabel";
			this.texXlabel.Size = new System.Drawing.Size(85, 13);
			this.texXlabel.TabIndex = 25;
			this.texXlabel.Text = "Texture X offset:";
			this.texXlabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label5
			// 
			label5.Location = new System.Drawing.Point(16, 35);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(78, 14);
			label5.TabIndex = 17;
			label5.Text = "Floor height:";
			label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label6
			// 
			label6.Location = new System.Drawing.Point(16, 65);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(78, 14);
			label6.TabIndex = 19;
			label6.Text = "Ceiling height:";
			label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// ceilingheight
			// 
			this.ceilingheight.AllowDecimal = false;
			this.ceilingheight.AllowNegative = true;
			this.ceilingheight.AllowRelative = true;
			this.ceilingheight.ButtonStep = 8;
			this.ceilingheight.ButtonStepBig = 16F;
			this.ceilingheight.ButtonStepFloat = 1F;
			this.ceilingheight.ButtonStepSmall = 1F;
			this.ceilingheight.ButtonStepsUseModifierKeys = true;
			this.ceilingheight.ButtonStepsWrapAround = false;
			this.ceilingheight.Location = new System.Drawing.Point(99, 60);
			this.ceilingheight.Name = "ceilingheight";
			this.ceilingheight.Size = new System.Drawing.Size(88, 24);
			this.ceilingheight.StepValues = null;
			this.ceilingheight.TabIndex = 22;
			this.ceilingheight.WhenTextChanged += new System.EventHandler(this.ceilingheight_TextChanged);
			// 
			// label2
			// 
			label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			label2.Location = new System.Drawing.Point(196, 16);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(114, 16);
			label2.TabIndex = 15;
			label2.Text = "Floor";
			label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// sectorheightlabel
			// 
			this.sectorheightlabel.Location = new System.Drawing.Point(16, 91);
			this.sectorheightlabel.Name = "sectorheightlabel";
			this.sectorheightlabel.Size = new System.Drawing.Size(78, 14);
			this.sectorheightlabel.TabIndex = 20;
			this.sectorheightlabel.Text = "Sector height:";
			this.sectorheightlabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			label4.Location = new System.Drawing.Point(329, 16);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(114, 16);
			label4.TabIndex = 14;
			label4.Text = "Ceiling";
			label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// sectorheight
			// 
			this.sectorheight.AutoSize = true;
			this.sectorheight.Location = new System.Drawing.Point(100, 91);
			this.sectorheight.Name = "sectorheight";
			this.sectorheight.Size = new System.Drawing.Size(13, 13);
			this.sectorheight.TabIndex = 21;
			this.sectorheight.Text = "0";
			// 
			// floortex
			// 
			this.floortex.Location = new System.Drawing.Point(196, 35);
			this.floortex.MultipleTextures = false;
			this.floortex.Name = "floortex";
			this.floortex.Size = new System.Drawing.Size(114, 138);
			this.floortex.TabIndex = 2;
			this.floortex.TextureName = "";
			this.floortex.UsePreviews = true;
			this.floortex.OnValueChanged += new System.EventHandler(this.floortex_OnValueChanged);
			// 
			// floorheight
			// 
			this.floorheight.AllowDecimal = false;
			this.floorheight.AllowNegative = true;
			this.floorheight.AllowRelative = true;
			this.floorheight.ButtonStep = 8;
			this.floorheight.ButtonStepBig = 16F;
			this.floorheight.ButtonStepFloat = 1F;
			this.floorheight.ButtonStepSmall = 1F;
			this.floorheight.ButtonStepsUseModifierKeys = true;
			this.floorheight.ButtonStepsWrapAround = false;
			this.floorheight.Location = new System.Drawing.Point(99, 30);
			this.floorheight.Name = "floorheight";
			this.floorheight.Size = new System.Drawing.Size(88, 24);
			this.floorheight.StepValues = null;
			this.floorheight.TabIndex = 23;
			this.floorheight.WhenTextChanged += new System.EventHandler(this.floorheight_TextChanged);
			// 
			// ceilingtex
			// 
			this.ceilingtex.Location = new System.Drawing.Point(329, 35);
			this.ceilingtex.MultipleTextures = false;
			this.ceilingtex.Name = "ceilingtex";
			this.ceilingtex.Size = new System.Drawing.Size(114, 138);
			this.ceilingtex.TabIndex = 3;
			this.ceilingtex.TextureName = "";
			this.ceilingtex.UsePreviews = true;
			this.ceilingtex.OnValueChanged += new System.EventHandler(this.ceilingtex_OnValueChanged);
			// 
			// label10
			// 
			label10.Location = new System.Drawing.Point(14, 79);
			label10.Name = "label10";
			label10.Size = new System.Drawing.Size(89, 14);
			label10.TabIndex = 27;
			label10.Text = "Animation speed:";
			label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// brightness
			// 
			this.brightness.AllowDecimal = false;
			this.brightness.AllowNegative = false;
			this.brightness.AllowRelative = true;
			this.brightness.ButtonStep = 8;
			this.brightness.ButtonStepBig = 16F;
			this.brightness.ButtonStepFloat = 1F;
			this.brightness.ButtonStepSmall = 1F;
			this.brightness.ButtonStepsUseModifierKeys = true;
			this.brightness.ButtonStepsWrapAround = false;
			this.brightness.Location = new System.Drawing.Point(78, 21);
			this.brightness.Name = "brightness";
			this.brightness.Size = new System.Drawing.Size(73, 24);
			this.brightness.StepValues = null;
			this.brightness.TabIndex = 24;
			this.brightness.WhenTextChanged += new System.EventHandler(this.brightness_WhenTextChanged);
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(661, 459);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(112, 25);
			this.cancel.TabIndex = 2;
			this.cancel.Text = "Cancel";
			this.cancel.UseVisualStyleBackColor = true;
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			// 
			// apply
			// 
			this.apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.apply.Location = new System.Drawing.Point(543, 459);
			this.apply.Name = "apply";
			this.apply.Size = new System.Drawing.Size(112, 25);
			this.apply.TabIndex = 1;
			this.apply.Text = "OK";
			this.apply.UseVisualStyleBackColor = true;
			this.apply.Click += new System.EventHandler(this.apply_Click);
			// 
			// flatSelectorControl2
			// 
			this.flatSelectorControl2.Location = new System.Drawing.Point(271, 37);
			this.flatSelectorControl2.MultipleTextures = false;
			this.flatSelectorControl2.Name = "flatSelectorControl2";
			this.flatSelectorControl2.Size = new System.Drawing.Size(83, 105);
			this.flatSelectorControl2.TabIndex = 13;
			this.flatSelectorControl2.TextureName = "";
			this.flatSelectorControl2.UsePreviews = true;
			// 
			// flatSelectorControl1
			// 
			this.flatSelectorControl1.Location = new System.Drawing.Point(363, 37);
			this.flatSelectorControl1.MultipleTextures = false;
			this.flatSelectorControl1.Name = "flatSelectorControl1";
			this.flatSelectorControl1.Size = new System.Drawing.Size(83, 105);
			this.flatSelectorControl1.TabIndex = 12;
			this.flatSelectorControl1.TextureName = "";
			this.flatSelectorControl1.UsePreviews = true;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Window;
			this.panel1.Controls.Add(this.slopebox);
			this.panel1.Controls.Add(this.depthbox);
			this.panel1.Controls.Add(this.Scrolling);
			this.panel1.Controls.Add(this.groupEffects);
			this.panel1.Controls.Add(groupfloorceiling);
			this.panel1.Location = new System.Drawing.Point(12, 10);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(761, 443);
			this.panel1.TabIndex = 3;
			// 
			// slopebox
			// 
			this.slopebox.Controls.Add(this.ceilvert3);
			this.slopebox.Controls.Add(this.ceilvert3height);
			this.slopebox.Controls.Add(this.ceilvert2);
			this.slopebox.Controls.Add(this.ceilvert2height);
			this.slopebox.Controls.Add(this.ceilvert1);
			this.slopebox.Controls.Add(this.ceilvert1height);
			this.slopebox.Controls.Add(this.label15);
			this.slopebox.Controls.Add(this.label16);
			this.slopebox.Controls.Add(this.floorvert3);
			this.slopebox.Controls.Add(this.floorvert3height);
			this.slopebox.Controls.Add(this.floorvert2);
			this.slopebox.Controls.Add(this.floorvert2height);
			this.slopebox.Controls.Add(this.floorvert1);
			this.slopebox.Controls.Add(this.floorvert1height);
			this.slopebox.Controls.Add(this.label14);
			this.slopebox.Controls.Add(this.label13);
			this.slopebox.Controls.Add(this.ceiltexrot);
			this.slopebox.Controls.Add(this.label12);
			this.slopebox.Controls.Add(this.floortexrot);
			this.slopebox.Controls.Add(this.label11);
			this.slopebox.Location = new System.Drawing.Point(263, 228);
			this.slopebox.Name = "slopebox";
			this.slopebox.Size = new System.Drawing.Size(489, 212);
			this.slopebox.TabIndex = 53;
			this.slopebox.TabStop = false;
			this.slopebox.Text = "Slopes";
			// 
			// floorvert1
			// 
			this.floorvert1.FormattingEnabled = true;
			this.floorvert1.Location = new System.Drawing.Point(11, 96);
			this.floorvert1.Name = "floorvert1";
			this.floorvert1.Size = new System.Drawing.Size(103, 21);
			this.floorvert1.TabIndex = 8;
			this.floorvert1.SelectedValueChanged += new System.EventHandler(this.floorvertexes_OnValuesChanged);
			// 
			// floorvert1height
			// 
			this.floorvert1height.AllowDecimal = false;
			this.floorvert1height.AllowNegative = false;
			this.floorvert1height.AllowRelative = false;
			this.floorvert1height.ButtonStep = 1;
			this.floorvert1height.ButtonStepBig = 10F;
			this.floorvert1height.ButtonStepFloat = 1F;
			this.floorvert1height.ButtonStepSmall = 0.1F;
			this.floorvert1height.ButtonStepsUseModifierKeys = false;
			this.floorvert1height.ButtonStepsWrapAround = false;
			this.floorvert1height.Location = new System.Drawing.Point(128, 96);
			this.floorvert1height.Name = "floorvert1height";
			this.floorvert1height.Size = new System.Drawing.Size(72, 24);
			this.floorvert1height.StepValues = null;
			this.floorvert1height.TabIndex = 7;
			this.floorvert1height.WhenTextChanged += new System.EventHandler(this.floorvertexheight_OnValuesChanged);
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(142, 68);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(38, 13);
			this.label14.TabIndex = 5;
			this.label14.Text = "Height";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(44, 68);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(47, 13);
			this.label13.TabIndex = 4;
			this.label13.Text = "Vertex #";
			// 
			// ceiltexrot
			// 
			this.ceiltexrot.AllowDecimal = false;
			this.ceiltexrot.AllowNegative = false;
			this.ceiltexrot.AllowRelative = false;
			this.ceiltexrot.ButtonStep = 1;
			this.ceiltexrot.ButtonStepBig = 10F;
			this.ceiltexrot.ButtonStepFloat = 1F;
			this.ceiltexrot.ButtonStepSmall = 0.1F;
			this.ceiltexrot.ButtonStepsUseModifierKeys = false;
			this.ceiltexrot.ButtonStepsWrapAround = false;
			this.ceiltexrot.Location = new System.Drawing.Point(385, 27);
			this.ceiltexrot.Name = "ceiltexrot";
			this.ceiltexrot.Size = new System.Drawing.Size(84, 24);
			this.ceiltexrot.StepValues = null;
			this.ceiltexrot.TabIndex = 3;
			this.ceiltexrot.WhenTextChanged += new System.EventHandler(this.ceilTexRot_OnValuesChanged);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(265, 30);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(114, 13);
			this.label12.TabIndex = 2;
			this.label12.Text = "Ceiling texture rotation:";
			// 
			// floortexrot
			// 
			this.floortexrot.AllowDecimal = false;
			this.floortexrot.AllowNegative = false;
			this.floortexrot.AllowRelative = false;
			this.floortexrot.ButtonStep = 1;
			this.floortexrot.ButtonStepBig = 10F;
			this.floortexrot.ButtonStepFloat = 1F;
			this.floortexrot.ButtonStepSmall = 0.1F;
			this.floortexrot.ButtonStepsUseModifierKeys = false;
			this.floortexrot.ButtonStepsWrapAround = false;
			this.floortexrot.Location = new System.Drawing.Point(128, 25);
			this.floortexrot.Name = "floortexrot";
			this.floortexrot.Size = new System.Drawing.Size(84, 24);
			this.floortexrot.StepValues = null;
			this.floortexrot.TabIndex = 1;
			this.floortexrot.WhenTextChanged += new System.EventHandler(this.floorTexRot_OnValuesChanged);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(8, 30);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(106, 13);
			this.label11.TabIndex = 0;
			this.label11.Text = "Floor texture rotation:";
			// 
			// depthbox
			// 
			this.depthbox.Controls.Add(this.depthvery);
			this.depthbox.Controls.Add(this.depthdeep);
			this.depthbox.Controls.Add(this.depthshallow);
			this.depthbox.Controls.Add(this.depthnone);
			this.depthbox.Location = new System.Drawing.Point(12, 228);
			this.depthbox.Name = "depthbox";
			this.depthbox.Size = new System.Drawing.Size(244, 90);
			this.depthbox.TabIndex = 52;
			this.depthbox.TabStop = false;
			this.depthbox.Text = "Depth";
			// 
			// depthvery
			// 
			this.depthvery.AutoSize = true;
			this.depthvery.Location = new System.Drawing.Point(100, 54);
			this.depthvery.Name = "depthvery";
			this.depthvery.Size = new System.Drawing.Size(73, 17);
			this.depthvery.TabIndex = 3;
			this.depthvery.TabStop = true;
			this.depthvery.Text = "Very deep";
			this.depthvery.UseVisualStyleBackColor = true;
			// 
			// depthdeep
			// 
			this.depthdeep.AutoSize = true;
			this.depthdeep.Location = new System.Drawing.Point(6, 54);
			this.depthdeep.Name = "depthdeep";
			this.depthdeep.Size = new System.Drawing.Size(51, 17);
			this.depthdeep.TabIndex = 2;
			this.depthdeep.TabStop = true;
			this.depthdeep.Text = "Deep";
			this.depthdeep.UseVisualStyleBackColor = true;
			// 
			// depthshallow
			// 
			this.depthshallow.AutoSize = true;
			this.depthshallow.Location = new System.Drawing.Point(100, 20);
			this.depthshallow.Name = "depthshallow";
			this.depthshallow.Size = new System.Drawing.Size(62, 17);
			this.depthshallow.TabIndex = 1;
			this.depthshallow.TabStop = true;
			this.depthshallow.Text = "Shallow";
			this.depthshallow.UseVisualStyleBackColor = true;
			// 
			// depthnone
			// 
			this.depthnone.AutoSize = true;
			this.depthnone.Location = new System.Drawing.Point(6, 20);
			this.depthnone.Name = "depthnone";
			this.depthnone.Size = new System.Drawing.Size(51, 17);
			this.depthnone.TabIndex = 0;
			this.depthnone.TabStop = true;
			this.depthnone.Text = "None";
			this.depthnone.UseVisualStyleBackColor = true;
			// 
			// Scrolling
			// 
			this.Scrolling.Controls.Add(this.scrollceiling);
			this.Scrolling.Controls.Add(this.scrollfloor);
			this.Scrolling.Controls.Add(this.scrolldirection);
			this.Scrolling.Controls.Add(this.scrollspeed);
			this.Scrolling.Location = new System.Drawing.Point(469, 3);
			this.Scrolling.Name = "Scrolling";
			this.Scrolling.Size = new System.Drawing.Size(283, 211);
			this.Scrolling.TabIndex = 51;
			this.Scrolling.TabStop = false;
			this.Scrolling.Text = "Scrolling";
			// 
			// scrollceiling
			// 
			this.scrollceiling.AutoSize = true;
			this.scrollceiling.Location = new System.Drawing.Point(13, 48);
			this.scrollceiling.Name = "scrollceiling";
			this.scrollceiling.Size = new System.Drawing.Size(85, 17);
			this.scrollceiling.TabIndex = 52;
			this.scrollceiling.Text = "Scroll ceiling";
			this.scrollceiling.UseVisualStyleBackColor = true;
			// 
			// scrollfloor
			// 
			this.scrollfloor.AutoSize = true;
			this.scrollfloor.Location = new System.Drawing.Point(13, 25);
			this.scrollfloor.Name = "scrollfloor";
			this.scrollfloor.Size = new System.Drawing.Size(75, 17);
			this.scrollfloor.TabIndex = 51;
			this.scrollfloor.Text = "Scroll floor";
			this.scrollfloor.UseVisualStyleBackColor = true;
			// 
			// scrolldirection
			// 
			this.scrolldirection.Controls.Add(this.SE);
			this.scrolldirection.Controls.Add(this.E);
			this.scrolldirection.Controls.Add(this.NE);
			this.scrolldirection.Controls.Add(this.S);
			this.scrolldirection.Controls.Add(this.N);
			this.scrolldirection.Controls.Add(this.SW);
			this.scrolldirection.Controls.Add(this.W);
			this.scrolldirection.Controls.Add(this.NW);
			this.scrolldirection.Location = new System.Drawing.Point(6, 110);
			this.scrolldirection.Name = "scrolldirection";
			this.scrolldirection.Size = new System.Drawing.Size(265, 90);
			this.scrolldirection.TabIndex = 49;
			this.scrolldirection.TabStop = false;
			this.scrolldirection.Text = "Scroll Direction";
			// 
			// SE
			// 
			this.SE.AutoSize = true;
			this.SE.Location = new System.Drawing.Point(190, 66);
			this.SE.Name = "SE";
			this.SE.Size = new System.Drawing.Size(39, 17);
			this.SE.TabIndex = 7;
			this.SE.TabStop = true;
			this.SE.Text = "SE";
			this.SE.UseVisualStyleBackColor = true;
			// 
			// E
			// 
			this.E.AutoSize = true;
			this.E.Location = new System.Drawing.Point(190, 43);
			this.E.Name = "E";
			this.E.Size = new System.Drawing.Size(46, 17);
			this.E.TabIndex = 6;
			this.E.TabStop = true;
			this.E.Text = "East";
			this.E.UseVisualStyleBackColor = true;
			// 
			// NE
			// 
			this.NE.AutoSize = true;
			this.NE.Location = new System.Drawing.Point(190, 20);
			this.NE.Name = "NE";
			this.NE.Size = new System.Drawing.Size(40, 17);
			this.NE.TabIndex = 5;
			this.NE.TabStop = true;
			this.NE.Text = "NE";
			this.NE.UseVisualStyleBackColor = true;
			// 
			// S
			// 
			this.S.AutoSize = true;
			this.S.Location = new System.Drawing.Point(99, 66);
			this.S.Name = "S";
			this.S.Size = new System.Drawing.Size(53, 17);
			this.S.TabIndex = 4;
			this.S.TabStop = true;
			this.S.Text = "South";
			this.S.UseVisualStyleBackColor = true;
			// 
			// N
			// 
			this.N.AutoSize = true;
			this.N.Location = new System.Drawing.Point(99, 20);
			this.N.Name = "N";
			this.N.Size = new System.Drawing.Size(51, 17);
			this.N.TabIndex = 3;
			this.N.TabStop = true;
			this.N.Text = "North";
			this.N.UseVisualStyleBackColor = true;
			// 
			// SW
			// 
			this.SW.AutoSize = true;
			this.SW.Location = new System.Drawing.Point(7, 66);
			this.SW.Name = "SW";
			this.SW.Size = new System.Drawing.Size(43, 17);
			this.SW.TabIndex = 2;
			this.SW.TabStop = true;
			this.SW.Text = "SW";
			this.SW.UseVisualStyleBackColor = true;
			// 
			// W
			// 
			this.W.AutoSize = true;
			this.W.Location = new System.Drawing.Point(7, 43);
			this.W.Name = "W";
			this.W.Size = new System.Drawing.Size(50, 17);
			this.W.TabIndex = 1;
			this.W.TabStop = true;
			this.W.Text = "West";
			this.W.UseVisualStyleBackColor = true;
			// 
			// NW
			// 
			this.NW.AutoSize = true;
			this.NW.Location = new System.Drawing.Point(7, 20);
			this.NW.Name = "NW";
			this.NW.Size = new System.Drawing.Size(44, 17);
			this.NW.TabIndex = 0;
			this.NW.TabStop = true;
			this.NW.Text = "NW";
			this.NW.UseVisualStyleBackColor = true;
			// 
			// scrollspeed
			// 
			this.scrollspeed.Controls.Add(this.Fast);
			this.scrollspeed.Controls.Add(this.Slow);
			this.scrollspeed.Controls.Add(this.Medium);
			this.scrollspeed.Controls.Add(this.None);
			this.scrollspeed.Location = new System.Drawing.Point(142, 14);
			this.scrollspeed.Name = "scrollspeed";
			this.scrollspeed.Size = new System.Drawing.Size(129, 90);
			this.scrollspeed.TabIndex = 50;
			this.scrollspeed.TabStop = false;
			this.scrollspeed.Text = "Scroll Speed";
			// 
			// Fast
			// 
			this.Fast.AutoSize = true;
			this.Fast.Location = new System.Drawing.Point(64, 54);
			this.Fast.Name = "Fast";
			this.Fast.Size = new System.Drawing.Size(45, 17);
			this.Fast.TabIndex = 3;
			this.Fast.TabStop = true;
			this.Fast.Text = "Fast";
			this.Fast.UseVisualStyleBackColor = true;
			// 
			// Slow
			// 
			this.Slow.AutoSize = true;
			this.Slow.Location = new System.Drawing.Point(6, 54);
			this.Slow.Name = "Slow";
			this.Slow.Size = new System.Drawing.Size(48, 17);
			this.Slow.TabIndex = 2;
			this.Slow.TabStop = true;
			this.Slow.Text = "Slow";
			this.Slow.UseVisualStyleBackColor = true;
			// 
			// Medium
			// 
			this.Medium.AutoSize = true;
			this.Medium.Location = new System.Drawing.Point(64, 20);
			this.Medium.Name = "Medium";
			this.Medium.Size = new System.Drawing.Size(62, 17);
			this.Medium.TabIndex = 1;
			this.Medium.TabStop = true;
			this.Medium.Text = "Medium";
			this.Medium.UseVisualStyleBackColor = true;
			// 
			// None
			// 
			this.None.AutoSize = true;
			this.None.Location = new System.Drawing.Point(6, 20);
			this.None.Name = "None";
			this.None.Size = new System.Drawing.Size(51, 17);
			this.None.TabIndex = 0;
			this.None.TabStop = true;
			this.None.Text = "None";
			this.None.UseVisualStyleBackColor = true;
			// 
			// groupEffects
			// 
			this.groupEffects.Controls.Add(label10);
			this.groupEffects.Controls.Add(this.animationspeed);
			this.groupEffects.Controls.Add(this.noambientbox);
			this.groupEffects.Controls.Add(this.flickerbox);
			this.groupEffects.Controls.Add(label9);
			this.groupEffects.Controls.Add(this.brightness);
			this.groupEffects.Location = new System.Drawing.Point(3, 324);
			this.groupEffects.Name = "groupEffects";
			this.groupEffects.Size = new System.Drawing.Size(253, 116);
			this.groupEffects.TabIndex = 1;
			this.groupEffects.TabStop = false;
			this.groupEffects.Text = "Effects";
			// 
			// animationspeed
			// 
			this.animationspeed.AllowDecimal = false;
			this.animationspeed.AllowNegative = true;
			this.animationspeed.AllowRelative = true;
			this.animationspeed.ButtonStep = 8;
			this.animationspeed.ButtonStepBig = 16F;
			this.animationspeed.ButtonStepFloat = 1F;
			this.animationspeed.ButtonStepSmall = 1F;
			this.animationspeed.ButtonStepsUseModifierKeys = true;
			this.animationspeed.ButtonStepsWrapAround = false;
			this.animationspeed.Location = new System.Drawing.Point(109, 74);
			this.animationspeed.Name = "animationspeed";
			this.animationspeed.Size = new System.Drawing.Size(73, 24);
			this.animationspeed.StepValues = null;
			this.animationspeed.TabIndex = 28;
			// 
			// noambientbox
			// 
			this.noambientbox.AutoSize = true;
			this.noambientbox.Location = new System.Drawing.Point(19, 51);
			this.noambientbox.Name = "noambientbox";
			this.noambientbox.Size = new System.Drawing.Size(194, 17);
			this.noambientbox.TabIndex = 26;
			this.noambientbox.Text = "Not affected by room\'s ambient light";
			this.noambientbox.UseVisualStyleBackColor = true;
			// 
			// flickerbox
			// 
			this.flickerbox.AutoSize = true;
			this.flickerbox.Location = new System.Drawing.Point(160, 25);
			this.flickerbox.Name = "flickerbox";
			this.flickerbox.Size = new System.Drawing.Size(87, 17);
			this.flickerbox.TabIndex = 25;
			this.flickerbox.Text = "Flicker effect";
			this.tooltip.SetToolTip(this.flickerbox, "Software renderer only");
			this.flickerbox.UseVisualStyleBackColor = true;
			// 
			// tooltip
			// 
			this.tooltip.AutomaticDelay = 10;
			this.tooltip.AutoPopDelay = 10000;
			this.tooltip.InitialDelay = 10;
			this.tooltip.ReshowDelay = 100;
			// 
			// floorvert2
			// 
			this.floorvert2.FormattingEnabled = true;
			this.floorvert2.Location = new System.Drawing.Point(11, 129);
			this.floorvert2.Name = "floorvert2";
			this.floorvert2.Size = new System.Drawing.Size(103, 21);
			this.floorvert2.TabIndex = 10;
			this.floorvert2.SelectedValueChanged += new System.EventHandler(this.floorvertexes_OnValuesChanged);
			// 
			// floorvert2height
			// 
			this.floorvert2height.AllowDecimal = false;
			this.floorvert2height.AllowNegative = false;
			this.floorvert2height.AllowRelative = false;
			this.floorvert2height.ButtonStep = 1;
			this.floorvert2height.ButtonStepBig = 10F;
			this.floorvert2height.ButtonStepFloat = 1F;
			this.floorvert2height.ButtonStepSmall = 0.1F;
			this.floorvert2height.ButtonStepsUseModifierKeys = false;
			this.floorvert2height.ButtonStepsWrapAround = false;
			this.floorvert2height.Location = new System.Drawing.Point(128, 126);
			this.floorvert2height.Name = "floorvert2height";
			this.floorvert2height.Size = new System.Drawing.Size(72, 24);
			this.floorvert2height.StepValues = null;
			this.floorvert2height.TabIndex = 9;
			this.floorvert2height.WhenTextChanged += new System.EventHandler(this.floorvertexheight_OnValuesChanged);
			// 
			// floorvert3
			// 
			this.floorvert3.FormattingEnabled = true;
			this.floorvert3.Location = new System.Drawing.Point(11, 159);
			this.floorvert3.Name = "floorvert3";
			this.floorvert3.Size = new System.Drawing.Size(103, 21);
			this.floorvert3.TabIndex = 12;
			this.floorvert3.SelectedValueChanged += new System.EventHandler(this.floorvertexes_OnValuesChanged);
			// 
			// floorvert3height
			// 
			this.floorvert3height.AllowDecimal = false;
			this.floorvert3height.AllowNegative = false;
			this.floorvert3height.AllowRelative = false;
			this.floorvert3height.ButtonStep = 1;
			this.floorvert3height.ButtonStepBig = 10F;
			this.floorvert3height.ButtonStepFloat = 1F;
			this.floorvert3height.ButtonStepSmall = 0.1F;
			this.floorvert3height.ButtonStepsUseModifierKeys = false;
			this.floorvert3height.ButtonStepsWrapAround = false;
			this.floorvert3height.Location = new System.Drawing.Point(128, 156);
			this.floorvert3height.Name = "floorvert3height";
			this.floorvert3height.Size = new System.Drawing.Size(72, 24);
			this.floorvert3height.StepValues = null;
			this.floorvert3height.TabIndex = 11;
			this.floorvert3height.WhenTextChanged += new System.EventHandler(this.floorvertexheight_OnValuesChanged);
			// 
			// ceilvert3
			// 
			this.ceilvert3.FormattingEnabled = true;
			this.ceilvert3.Location = new System.Drawing.Point(268, 162);
			this.ceilvert3.Name = "ceilvert3";
			this.ceilvert3.Size = new System.Drawing.Size(103, 21);
			this.ceilvert3.TabIndex = 20;
			this.ceilvert3.SelectedValueChanged += new System.EventHandler(this.ceilvertexes_OnValuesChanged);
			// 
			// ceilvert3height
			// 
			this.ceilvert3height.AllowDecimal = false;
			this.ceilvert3height.AllowNegative = false;
			this.ceilvert3height.AllowRelative = false;
			this.ceilvert3height.ButtonStep = 1;
			this.ceilvert3height.ButtonStepBig = 10F;
			this.ceilvert3height.ButtonStepFloat = 1F;
			this.ceilvert3height.ButtonStepSmall = 0.1F;
			this.ceilvert3height.ButtonStepsUseModifierKeys = false;
			this.ceilvert3height.ButtonStepsWrapAround = false;
			this.ceilvert3height.Location = new System.Drawing.Point(385, 159);
			this.ceilvert3height.Name = "ceilvert3height";
			this.ceilvert3height.Size = new System.Drawing.Size(72, 24);
			this.ceilvert3height.StepValues = null;
			this.ceilvert3height.TabIndex = 19;
			this.ceilvert3height.WhenTextChanged += new System.EventHandler(this.ceilvertexheight_OnValuesChanged);
			// 
			// ceilvert2
			// 
			this.ceilvert2.FormattingEnabled = true;
			this.ceilvert2.Location = new System.Drawing.Point(268, 132);
			this.ceilvert2.Name = "ceilvert2";
			this.ceilvert2.Size = new System.Drawing.Size(103, 21);
			this.ceilvert2.TabIndex = 18;
			this.ceilvert2.SelectedValueChanged += new System.EventHandler(this.ceilvertexes_OnValuesChanged);
			// 
			// ceilvert2height
			// 
			this.ceilvert2height.AllowDecimal = false;
			this.ceilvert2height.AllowNegative = false;
			this.ceilvert2height.AllowRelative = false;
			this.ceilvert2height.ButtonStep = 1;
			this.ceilvert2height.ButtonStepBig = 10F;
			this.ceilvert2height.ButtonStepFloat = 1F;
			this.ceilvert2height.ButtonStepSmall = 0.1F;
			this.ceilvert2height.ButtonStepsUseModifierKeys = false;
			this.ceilvert2height.ButtonStepsWrapAround = false;
			this.ceilvert2height.Location = new System.Drawing.Point(385, 129);
			this.ceilvert2height.Name = "ceilvert2height";
			this.ceilvert2height.Size = new System.Drawing.Size(72, 24);
			this.ceilvert2height.StepValues = null;
			this.ceilvert2height.TabIndex = 17;
			this.ceilvert2height.WhenTextChanged += new System.EventHandler(this.ceilvertexheight_OnValuesChanged);
			// 
			// ceilvert1
			// 
			this.ceilvert1.FormattingEnabled = true;
			this.ceilvert1.Location = new System.Drawing.Point(268, 99);
			this.ceilvert1.Name = "ceilvert1";
			this.ceilvert1.Size = new System.Drawing.Size(103, 21);
			this.ceilvert1.TabIndex = 16;
			this.ceilvert1.SelectedValueChanged += new System.EventHandler(this.ceilvertexes_OnValuesChanged);
			// 
			// ceilvert1height
			// 
			this.ceilvert1height.AllowDecimal = false;
			this.ceilvert1height.AllowNegative = false;
			this.ceilvert1height.AllowRelative = false;
			this.ceilvert1height.ButtonStep = 1;
			this.ceilvert1height.ButtonStepBig = 10F;
			this.ceilvert1height.ButtonStepFloat = 1F;
			this.ceilvert1height.ButtonStepSmall = 0.1F;
			this.ceilvert1height.ButtonStepsUseModifierKeys = false;
			this.ceilvert1height.ButtonStepsWrapAround = false;
			this.ceilvert1height.Location = new System.Drawing.Point(385, 99);
			this.ceilvert1height.Name = "ceilvert1height";
			this.ceilvert1height.Size = new System.Drawing.Size(72, 24);
			this.ceilvert1height.StepValues = null;
			this.ceilvert1height.TabIndex = 15;
			this.ceilvert1height.WhenTextChanged += new System.EventHandler(this.ceilvertexheight_OnValuesChanged);
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(399, 71);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(38, 13);
			this.label15.TabIndex = 14;
			this.label15.Text = "Height";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(301, 71);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(47, 13);
			this.label16.TabIndex = 13;
			this.label16.Text = "Vertex #";
			// 
			// SectorEditFormMeridian
			// 
			this.AcceptButton = this.apply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(785, 490);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.apply);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SectorEditFormMeridian";
			this.Opacity = 0;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Sector";
			this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.SectorEditForm_HelpRequested);
			groupfloorceiling.ResumeLayout(false);
			groupfloorceiling.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.slopebox.ResumeLayout(false);
			this.slopebox.PerformLayout();
			this.depthbox.ResumeLayout(false);
			this.depthbox.PerformLayout();
			this.Scrolling.ResumeLayout(false);
			this.Scrolling.PerformLayout();
			this.scrolldirection.ResumeLayout(false);
			this.scrolldirection.PerformLayout();
			this.scrollspeed.ResumeLayout(false);
			this.scrollspeed.PerformLayout();
			this.groupEffects.ResumeLayout(false);
			this.groupEffects.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Button apply;
		private CodeImp.DoomBuilder.Controls.FlatSelectorControl floortex;
		private CodeImp.DoomBuilder.Controls.FlatSelectorControl ceilingtex;
		private CodeImp.DoomBuilder.Controls.FlatSelectorControl flatSelectorControl2;
		private CodeImp.DoomBuilder.Controls.FlatSelectorControl flatSelectorControl1;
		private System.Windows.Forms.Label sectorheight;
		private System.Windows.Forms.Label sectorheightlabel;
		private CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox ceilingheight;
		private CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox floorheight;
		private CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox brightness;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolTip tooltip;
		private Controls.ButtonsNumericTextbox sectortag;
		private Controls.ButtonsNumericTextbox textureyoffset;
		private System.Windows.Forms.Label label7;
		private Controls.ButtonsNumericTextbox texturexoffset;
		private System.Windows.Forms.Label texXlabel;
		private System.Windows.Forms.GroupBox groupEffects;
		private Controls.ButtonsNumericTextbox animationspeed;
		private System.Windows.Forms.CheckBox noambientbox;
		private System.Windows.Forms.CheckBox flickerbox;
		private System.Windows.Forms.GroupBox slopebox;
		private System.Windows.Forms.GroupBox depthbox;
		private System.Windows.Forms.RadioButton depthvery;
		private System.Windows.Forms.RadioButton depthdeep;
		private System.Windows.Forms.RadioButton depthshallow;
		private System.Windows.Forms.RadioButton depthnone;
		private System.Windows.Forms.GroupBox Scrolling;
		private System.Windows.Forms.CheckBox scrollceiling;
		private System.Windows.Forms.CheckBox scrollfloor;
		private System.Windows.Forms.GroupBox scrolldirection;
		private System.Windows.Forms.RadioButton SE;
		private System.Windows.Forms.RadioButton E;
		private System.Windows.Forms.RadioButton NE;
		private System.Windows.Forms.RadioButton S;
		private System.Windows.Forms.RadioButton N;
		private System.Windows.Forms.RadioButton SW;
		private System.Windows.Forms.RadioButton W;
		private System.Windows.Forms.RadioButton NW;
		private System.Windows.Forms.GroupBox scrollspeed;
		private System.Windows.Forms.RadioButton Fast;
		private System.Windows.Forms.RadioButton Slow;
		private System.Windows.Forms.RadioButton Medium;
		private System.Windows.Forms.RadioButton None;
		private System.Windows.Forms.Label label11;
		private Controls.ButtonsNumericTextbox floortexrot;
		private Controls.ButtonsNumericTextbox ceiltexrot;
		private System.Windows.Forms.Label label12;
		private Controls.ButtonsNumericTextbox floorvert1height;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.ComboBox floorvert1;
		private System.Windows.Forms.ComboBox ceilvert3;
		private Controls.ButtonsNumericTextbox ceilvert3height;
		private System.Windows.Forms.ComboBox ceilvert2;
		private Controls.ButtonsNumericTextbox ceilvert2height;
		private System.Windows.Forms.ComboBox ceilvert1;
		private Controls.ButtonsNumericTextbox ceilvert1height;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.ComboBox floorvert3;
		private Controls.ButtonsNumericTextbox floorvert3height;
		private System.Windows.Forms.ComboBox floorvert2;
		private Controls.ButtonsNumericTextbox floorvert2height;
	}
}