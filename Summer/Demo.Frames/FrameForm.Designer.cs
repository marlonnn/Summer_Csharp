namespace Demo.Frames
{
    partial class FrameForm
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
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.buttonGraphEdit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.buttonLoop = new System.Windows.Forms.Button();
            this.buttonThru = new System.Windows.Forms.Button();
            this.buttonFramestep = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonSetmedia = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.movieFiles = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.snapShots = new Demo.Frames.SnapShots();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Width = 10;
            // 
            // buttonGraphEdit
            // 
            this.buttonGraphEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGraphEdit.Location = new System.Drawing.Point(420, 448);
            this.buttonGraphEdit.Name = "buttonGraphEdit";
            this.buttonGraphEdit.Size = new System.Drawing.Size(85, 23);
            this.buttonGraphEdit.TabIndex = 12;
            this.buttonGraphEdit.Text = "GraphEdit";
            this.buttonGraphEdit.Click += new System.EventHandler(this.buttonGraphEdit_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 28);
            this.label1.TabIndex = 7;
            this.label1.Text = "Duration: 00m:00s";
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.Location = new System.Drawing.Point(115, 37);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(382, 30);
            this.trackBar1.TabIndex = 6;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // buttonLoop
            // 
            this.buttonLoop.Location = new System.Drawing.Point(399, 10);
            this.buttonLoop.Name = "buttonLoop";
            this.buttonLoop.Size = new System.Drawing.Size(88, 24);
            this.buttonLoop.TabIndex = 5;
            this.buttonLoop.Text = "Loop";
            this.buttonLoop.Click += new System.EventHandler(this.buttonLoop_Click);
            // 
            // buttonThru
            // 
            this.buttonThru.Location = new System.Drawing.Point(300, 10);
            this.buttonThru.Name = "buttonThru";
            this.buttonThru.Size = new System.Drawing.Size(88, 24);
            this.buttonThru.TabIndex = 4;
            this.buttonThru.Text = "Thru";
            this.buttonThru.Click += new System.EventHandler(this.buttonThru_Click);
            // 
            // buttonFramestep
            // 
            this.buttonFramestep.Enabled = false;
            this.buttonFramestep.Location = new System.Drawing.Point(200, 10);
            this.buttonFramestep.Name = "buttonFramestep";
            this.buttonFramestep.Size = new System.Drawing.Size(88, 24);
            this.buttonFramestep.TabIndex = 3;
            this.buttonFramestep.Text = "Framestep";
            this.buttonFramestep.Click += new System.EventHandler(this.buttonFramestep_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(100, 10);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(88, 24);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "Stop";
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(126, 452);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(194, 20);
            this.textBox1.TabIndex = 10;
            // 
            // buttonSetmedia
            // 
            this.buttonSetmedia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSetmedia.Location = new System.Drawing.Point(8, 448);
            this.buttonSetmedia.Name = "buttonSetmedia";
            this.buttonSetmedia.Size = new System.Drawing.Size(110, 23);
            this.buttonSetmedia.TabIndex = 9;
            this.buttonSetmedia.Text = "Set media directory";
            this.buttonSetmedia.Click += new System.EventHandler(this.buttonSetmedia_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(6, 10);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(88, 24);
            this.buttonPlay.TabIndex = 0;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 694);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(1283, 22);
            this.statusBar1.SizingGrip = false;
            this.statusBar1.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(128, 94);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(341, 331);
            this.panel1.TabIndex = 11;
            // 
            // movieFiles
            // 
            this.movieFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.movieFiles.Location = new System.Drawing.Point(8, 96);
            this.movieFiles.Name = "movieFiles";
            this.movieFiles.Size = new System.Drawing.Size(110, 329);
            this.movieFiles.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.trackBar1);
            this.groupBox1.Controls.Add(this.buttonLoop);
            this.groupBox1.Controls.Add(this.buttonThru);
            this.groupBox1.Controls.Add(this.buttonFramestep);
            this.groupBox1.Controls.Add(this.buttonStop);
            this.groupBox1.Controls.Add(this.buttonPlay);
            this.groupBox1.Location = new System.Drawing.Point(8, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 88);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(326, 448);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 25);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save Frame";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(120, 68);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(368, 16);
            this.progressBar1.TabIndex = 15;
            // 
            // snapShots
            // 
            this.snapShots.AutoScroll = true;
            this.snapShots.BackColor = System.Drawing.Color.LightGray;
            this.snapShots.HorizontalMode = true;
            this.snapShots.Location = new System.Drawing.Point(8, 492);
            this.snapShots.Name = "snapShots";
            this.snapShots.Size = new System.Drawing.Size(500, 148);
            this.snapShots.TabIndex = 15;
            this.snapShots.Text = "snapShots1";
            // 
            // FrameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1283, 716);
            this.Controls.Add(this.snapShots);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.buttonGraphEdit);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonSetmedia);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.movieFiles);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrameForm";
            this.Text = "FrameForm";
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusBarPanel statusBarPanel1;
        private System.Windows.Forms.Button buttonGraphEdit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button buttonLoop;
        private System.Windows.Forms.Button buttonThru;
        private System.Windows.Forms.Button buttonFramestep;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonSetmedia;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox movieFiles;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ProgressBar progressBar1;
        private SnapShots snapShots;
    }
}