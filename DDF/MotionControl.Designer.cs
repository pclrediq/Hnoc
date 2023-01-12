
namespace DDF
{
    partial class MotionControl
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
            this.comboBox_Emergency = new System.Windows.Forms.ComboBox();
            this.comboBox_YAlarm = new System.Windows.Forms.ComboBox();
            this.comboBox_XAlarm = new System.Windows.Forms.ComboBox();
            this.button_SetUp = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.DDF = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.num_YAcc = new System.Windows.Forms.NumericUpDown();
            this.num_YStartSpeed = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.num_YMax = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.num_YRatio = new System.Windows.Forms.NumericUpDown();
            this.num_YDec = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.num_XMax = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.num_XDec = new System.Windows.Forms.NumericUpDown();
            this.num_XAcc = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.num_XStartSpeed = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.num_XRatio = new System.Windows.Forms.NumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.btn_Reset = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_YAcc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_YStartSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_YMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_YRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_YDec)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_XMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_XDec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_XAcc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_XStartSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_XRatio)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox_Emergency
            // 
            this.comboBox_Emergency.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.comboBox_Emergency.FormattingEnabled = true;
            this.comboBox_Emergency.IntegralHeight = false;
            this.comboBox_Emergency.Items.AddRange(new object[] {
            "NO",
            "NC"});
            this.comboBox_Emergency.Location = new System.Drawing.Point(150, 296);
            this.comboBox_Emergency.Name = "comboBox_Emergency";
            this.comboBox_Emergency.Size = new System.Drawing.Size(105, 23);
            this.comboBox_Emergency.TabIndex = 139;
            this.comboBox_Emergency.Text = "NO";
            // 
            // comboBox_YAlarm
            // 
            this.comboBox_YAlarm.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.comboBox_YAlarm.FormattingEnabled = true;
            this.comboBox_YAlarm.Items.AddRange(new object[] {
            "NC",
            "NO"});
            this.comboBox_YAlarm.Location = new System.Drawing.Point(127, 167);
            this.comboBox_YAlarm.Name = "comboBox_YAlarm";
            this.comboBox_YAlarm.Size = new System.Drawing.Size(105, 23);
            this.comboBox_YAlarm.TabIndex = 138;
            this.comboBox_YAlarm.Text = "NC";
            // 
            // comboBox_XAlarm
            // 
            this.comboBox_XAlarm.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.comboBox_XAlarm.FormattingEnabled = true;
            this.comboBox_XAlarm.Items.AddRange(new object[] {
            "NC",
            "NO"});
            this.comboBox_XAlarm.Location = new System.Drawing.Point(122, 166);
            this.comboBox_XAlarm.Name = "comboBox_XAlarm";
            this.comboBox_XAlarm.Size = new System.Drawing.Size(105, 23);
            this.comboBox_XAlarm.TabIndex = 137;
            this.comboBox_XAlarm.Text = "NC";
            // 
            // button_SetUp
            // 
            this.button_SetUp.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button_SetUp.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_SetUp.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button_SetUp.Location = new System.Drawing.Point(280, 287);
            this.button_SetUp.Name = "button_SetUp";
            this.button_SetUp.Size = new System.Drawing.Size(105, 39);
            this.button_SetUp.TabIndex = 114;
            this.button_SetUp.Text = "SetUp";
            this.button_SetUp.UseVisualStyleBackColor = false;
            this.button_SetUp.Click += new System.EventHandler(this.button_SetUp_Click);
            // 
            // btn_close
            // 
            this.btn_close.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_close.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_close.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btn_close.Location = new System.Drawing.Point(391, 287);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(105, 39);
            this.btn_close.TabIndex = 151;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // DDF
            // 
            this.DDF.AutoSize = true;
            this.DDF.Font = new System.Drawing.Font("맑은 고딕", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.DDF.Location = new System.Drawing.Point(12, 9);
            this.DDF.Name = "DDF";
            this.DDF.Size = new System.Drawing.Size(255, 45);
            this.DDF.TabIndex = 153;
            this.DDF.Text = "Motion Control";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.num_YAcc);
            this.groupBox1.Controls.Add(this.num_YStartSpeed);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.comboBox_YAlarm);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.num_YMax);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.num_YRatio);
            this.groupBox1.Controls.Add(this.num_YDec);
            this.groupBox1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(20, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 198);
            this.groupBox1.TabIndex = 154;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Screw Motor(Y)";
            // 
            // num_YAcc
            // 
            this.num_YAcc.DecimalPlaces = 3;
            this.num_YAcc.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_YAcc.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.num_YAcc.Location = new System.Drawing.Point(126, 86);
            this.num_YAcc.Name = "num_YAcc";
            this.num_YAcc.Size = new System.Drawing.Size(106, 23);
            this.num_YAcc.TabIndex = 164;
            this.num_YAcc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_YStartSpeed
            // 
            this.num_YStartSpeed.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_YStartSpeed.Location = new System.Drawing.Point(126, 59);
            this.num_YStartSpeed.Name = "num_YStartSpeed";
            this.num_YStartSpeed.Size = new System.Drawing.Size(106, 23);
            this.num_YStartSpeed.TabIndex = 165;
            this.num_YStartSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label19.Location = new System.Drawing.Point(12, 61);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(72, 15);
            this.label19.TabIndex = 146;
            this.label19.Text = "Start Speed:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.Location = new System.Drawing.Point(12, 142);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(70, 15);
            this.label17.TabIndex = 145;
            this.label17.Text = "Max Speed:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label16.Location = new System.Drawing.Point(12, 115);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 15);
            this.label16.TabIndex = 144;
            this.label16.Text = "Deceleration:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(12, 88);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(66, 15);
            this.label15.TabIndex = 143;
            this.label15.Text = "Accelation:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(12, 170);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(42, 15);
            this.label14.TabIndex = 142;
            this.label14.Text = "Alarm:";
            // 
            // num_YMax
            // 
            this.num_YMax.DecimalPlaces = 3;
            this.num_YMax.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_YMax.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.num_YMax.Location = new System.Drawing.Point(126, 140);
            this.num_YMax.Name = "num_YMax";
            this.num_YMax.Size = new System.Drawing.Size(106, 23);
            this.num_YMax.TabIndex = 158;
            this.num_YMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(12, 34);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 15);
            this.label12.TabIndex = 140;
            this.label12.Text = "Unit Per Pulse:";
            // 
            // num_YRatio
            // 
            this.num_YRatio.DecimalPlaces = 7;
            this.num_YRatio.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_YRatio.Increment = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            this.num_YRatio.Location = new System.Drawing.Point(126, 30);
            this.num_YRatio.Name = "num_YRatio";
            this.num_YRatio.Size = new System.Drawing.Size(106, 23);
            this.num_YRatio.TabIndex = 156;
            this.num_YRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_YDec
            // 
            this.num_YDec.DecimalPlaces = 3;
            this.num_YDec.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_YDec.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.num_YDec.Location = new System.Drawing.Point(126, 113);
            this.num_YDec.Name = "num_YDec";
            this.num_YDec.Size = new System.Drawing.Size(106, 23);
            this.num_YDec.TabIndex = 157;
            this.num_YDec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.num_XMax);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.num_XDec);
            this.groupBox2.Controls.Add(this.num_XAcc);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.num_XStartSpeed);
            this.groupBox2.Controls.Add(this.label23);
            this.groupBox2.Controls.Add(this.num_XRatio);
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Controls.Add(this.label26);
            this.groupBox2.Controls.Add(this.comboBox_XAlarm);
            this.groupBox2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.Location = new System.Drawing.Point(268, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(241, 198);
            this.groupBox2.TabIndex = 155;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Piston Motor(X)";
            // 
            // num_XMax
            // 
            this.num_XMax.DecimalPlaces = 3;
            this.num_XMax.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_XMax.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.num_XMax.Location = new System.Drawing.Point(122, 140);
            this.num_XMax.Name = "num_XMax";
            this.num_XMax.Size = new System.Drawing.Size(106, 23);
            this.num_XMax.TabIndex = 163;
            this.num_XMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label20.Location = new System.Drawing.Point(9, 61);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(72, 15);
            this.label20.TabIndex = 153;
            this.label20.Text = "Start Speed:";
            // 
            // num_XDec
            // 
            this.num_XDec.DecimalPlaces = 3;
            this.num_XDec.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_XDec.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.num_XDec.Location = new System.Drawing.Point(122, 113);
            this.num_XDec.Name = "num_XDec";
            this.num_XDec.Size = new System.Drawing.Size(106, 23);
            this.num_XDec.TabIndex = 161;
            this.num_XDec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_XAcc
            // 
            this.num_XAcc.DecimalPlaces = 3;
            this.num_XAcc.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_XAcc.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.num_XAcc.Location = new System.Drawing.Point(122, 86);
            this.num_XAcc.Name = "num_XAcc";
            this.num_XAcc.Size = new System.Drawing.Size(106, 23);
            this.num_XAcc.TabIndex = 162;
            this.num_XAcc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label21.Location = new System.Drawing.Point(9, 142);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(70, 15);
            this.label21.TabIndex = 152;
            this.label21.Text = "Max Speed:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label22.Location = new System.Drawing.Point(9, 115);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(77, 15);
            this.label22.TabIndex = 151;
            this.label22.Text = "Deceleration:";
            // 
            // num_XStartSpeed
            // 
            this.num_XStartSpeed.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_XStartSpeed.Location = new System.Drawing.Point(122, 59);
            this.num_XStartSpeed.Name = "num_XStartSpeed";
            this.num_XStartSpeed.Size = new System.Drawing.Size(106, 23);
            this.num_XStartSpeed.TabIndex = 160;
            this.num_XStartSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label23.Location = new System.Drawing.Point(9, 88);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(66, 15);
            this.label23.TabIndex = 150;
            this.label23.Text = "Accelation:";
            // 
            // num_XRatio
            // 
            this.num_XRatio.DecimalPlaces = 7;
            this.num_XRatio.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_XRatio.Increment = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            this.num_XRatio.Location = new System.Drawing.Point(122, 32);
            this.num_XRatio.Name = "num_XRatio";
            this.num_XRatio.Size = new System.Drawing.Size(106, 23);
            this.num_XRatio.TabIndex = 159;
            this.num_XRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label24.Location = new System.Drawing.Point(9, 169);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(42, 15);
            this.label24.TabIndex = 149;
            this.label24.Text = "Alarm:";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label26.Location = new System.Drawing.Point(9, 34);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(85, 15);
            this.label26.TabIndex = 147;
            this.label26.Text = "Unit Per Pulse:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.Location = new System.Drawing.Point(35, 299);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(69, 15);
            this.label18.TabIndex = 146;
            this.label18.Text = "Emergency:";
            // 
            // btn_Reset
            // 
            this.btn_Reset.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Reset.Location = new System.Drawing.Point(434, 43);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new System.Drawing.Size(75, 23);
            this.btn_Reset.TabIndex = 156;
            this.btn_Reset.Text = "Reset";
            this.btn_Reset.UseVisualStyleBackColor = true;
            this.btn_Reset.Click += new System.EventHandler(this.btn_Reset_Click);
            // 
            // MotionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(529, 345);
            this.Controls.Add(this.btn_Reset);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.DDF);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.button_SetUp);
            this.Controls.Add(this.comboBox_Emergency);
            this.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "MotionControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MotionControl";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_YAcc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_YStartSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_YMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_YRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_YDec)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_XMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_XDec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_XAcc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_XStartSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_XRatio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBox_Emergency;
        private System.Windows.Forms.ComboBox comboBox_YAlarm;
        private System.Windows.Forms.ComboBox comboBox_XAlarm;
        private System.Windows.Forms.Button button_SetUp;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label DDF;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label26;
        public System.Windows.Forms.NumericUpDown num_YRatio;
        public System.Windows.Forms.NumericUpDown num_YDec;
        public System.Windows.Forms.NumericUpDown num_YMax;
        public System.Windows.Forms.NumericUpDown num_XRatio;
        public System.Windows.Forms.NumericUpDown num_XStartSpeed;
        public System.Windows.Forms.NumericUpDown num_XDec;
        public System.Windows.Forms.NumericUpDown num_XAcc;
        public System.Windows.Forms.NumericUpDown num_XMax;
        public System.Windows.Forms.NumericUpDown num_YAcc;
        public System.Windows.Forms.NumericUpDown num_YStartSpeed;
        private System.Windows.Forms.Button btn_Reset;
    }
}