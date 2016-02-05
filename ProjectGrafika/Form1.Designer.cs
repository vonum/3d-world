namespace ProjectGrafika
{
    partial class Form1
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
            world.Dispose();
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
            this.openglControll = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.hangar_input = new System.Windows.Forms.NumericUpDown();
            this.anthenna_input = new System.Windows.Forms.NumericUpDown();
            this.doorspeed_input = new System.Windows.Forms.NumericUpDown();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hangar_input)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.anthenna_input)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.doorspeed_input)).BeginInit();
            this.SuspendLayout();
            // 
            // openglControll
            // 
            this.openglControll.AccumBits = ((byte)(0));
            this.openglControll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.openglControll.AutoCheckErrors = false;
            this.openglControll.AutoFinish = false;
            this.openglControll.AutoMakeCurrent = true;
            this.openglControll.AutoSwapBuffers = true;
            this.openglControll.BackColor = System.Drawing.Color.Black;
            this.openglControll.ColorBits = ((byte)(32));
            this.openglControll.DepthBits = ((byte)(16));
            this.openglControll.Location = new System.Drawing.Point(0, 0);
            this.openglControll.Name = "openglControll";
            this.openglControll.Size = new System.Drawing.Size(599, 600);
            this.openglControll.StencilBits = ((byte)(0));
            this.openglControll.TabIndex = 0;
            this.openglControll.Paint += new System.Windows.Forms.PaintEventHandler(this.openglControll_Paint);
            this.openglControll.KeyDown += new System.Windows.Forms.KeyEventHandler(this.openglControll_KeyDown);
            this.openglControll.Resize += new System.EventHandler(this.openglControll_Resize);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 150;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(597, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(188, 599);
            this.panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.doorspeed_input);
            this.groupBox1.Controls.Add(this.anthenna_input);
            this.groupBox1.Controls.Add(this.hangar_input);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(8, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(168, 191);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hangar height:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Anthenna length:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 118);
            this.label3.MaximumSize = new System.Drawing.Size(100, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 26);
            this.label3.TabIndex = 2;
            this.label3.Text = "Door openning speed:";
            // 
            // hangar_input
            // 
            this.hangar_input.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hangar_input.Location = new System.Drawing.Point(113, 33);
            this.hangar_input.Maximum = new decimal(new int[] {
            115,
            0,
            0,
            0});
            this.hangar_input.Minimum = new decimal(new int[] {
            85,
            0,
            0,
            0});
            this.hangar_input.Name = "hangar_input";
            this.hangar_input.Size = new System.Drawing.Size(49, 20);
            this.hangar_input.TabIndex = 3;
            this.hangar_input.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.hangar_input.ValueChanged += new System.EventHandler(this.hangar_input_ValueChanged);
            // 
            // anthenna_input
            // 
            this.anthenna_input.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.anthenna_input.Location = new System.Drawing.Point(113, 79);
            this.anthenna_input.Maximum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.anthenna_input.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.anthenna_input.Name = "anthenna_input";
            this.anthenna_input.Size = new System.Drawing.Size(49, 20);
            this.anthenna_input.TabIndex = 4;
            this.anthenna_input.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.anthenna_input.ValueChanged += new System.EventHandler(this.anthenna_input_ValueChanged);
            // 
            // doorspeed_input
            // 
            this.doorspeed_input.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doorspeed_input.Location = new System.Drawing.Point(113, 118);
            this.doorspeed_input.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.doorspeed_input.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.doorspeed_input.Name = "doorspeed_input";
            this.doorspeed_input.Size = new System.Drawing.Size(49, 20);
            this.doorspeed_input.TabIndex = 5;
            this.doorspeed_input.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.doorspeed_input.ValueChanged += new System.EventHandler(this.doorspeed_input_ValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 600);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.openglControll);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hangar_input)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.anthenna_input)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.doorspeed_input)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Tao.Platform.Windows.SimpleOpenGlControl openglControll;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown hangar_input;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown doorspeed_input;
        private System.Windows.Forms.NumericUpDown anthenna_input;

    }
}

