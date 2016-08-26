namespace Demo.Animation
{
    partial class Form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblFps = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.hscrFps = new System.Windows.Forms.HScrollBar();
            this.picFrame = new System.Windows.Forms.PictureBox();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.tmrNextFrame = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFps
            // 
            this.lblFps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFps.Location = new System.Drawing.Point(333, 21);
            this.lblFps.Name = "lblFps";
            this.lblFps.Size = new System.Drawing.Size(30, 17);
            this.lblFps.TabIndex = 5;
            this.lblFps.Text = "50";
            this.lblFps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "FPS";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // hscrFps
            // 
            this.hscrFps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hscrFps.Location = new System.Drawing.Point(46, 21);
            this.hscrFps.Maximum = 109;
            this.hscrFps.Minimum = 1;
            this.hscrFps.Name = "hscrFps";
            this.hscrFps.Size = new System.Drawing.Size(284, 17);
            this.hscrFps.TabIndex = 3;
            this.hscrFps.Value = 50;
            this.hscrFps.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscrFps_Scroll);
            // 
            // picFrame
            // 
            this.picFrame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picFrame.Location = new System.Drawing.Point(19, 85);
            this.picFrame.Name = "picFrame";
            this.picFrame.Size = new System.Drawing.Size(100, 50);
            this.picFrame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picFrame.TabIndex = 7;
            this.picFrame.TabStop = false;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(19, 56);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 23);
            this.btnStartStop.TabIndex = 6;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // tmrNextFrame
            // 
            this.tmrNextFrame.Interval = 20;
            this.tmrNextFrame.Tick += new System.EventHandler(this.tmrNextFrame_Tick);
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 457);
            this.Controls.Add(this.picFrame);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.lblFps);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hscrFps);
            this.Name = "Form";
            this.Text = "Frame Animation";
            this.Load += new System.EventHandler(this.Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picFrame)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFps;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.HScrollBar hscrFps;
        private System.Windows.Forms.PictureBox picFrame;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Timer tmrNextFrame;
    }
}

