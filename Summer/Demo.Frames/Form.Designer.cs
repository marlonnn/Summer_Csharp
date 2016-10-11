namespace Demo.Frames
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.snapShots1 = new Demo.Frames.SnapShots();
            this.videoListView1 = new Demo.Frames.VideoListView();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(35, 316);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 21);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(175, 316);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 21);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(272, 21);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(362, 289);
            this.lblInfo.TabIndex = 5;
            // 
            // snapShots1
            // 
            this.snapShots1.AutoScroll = true;
            this.snapShots1.BackColor = System.Drawing.Color.LightGray;
            this.snapShots1.HorizontalMode = true;
            this.snapShots1.Location = new System.Drawing.Point(1, 400);
            this.snapShots1.Name = "snapShots1";
            this.snapShots1.Size = new System.Drawing.Size(846, 120);
            this.snapShots1.TabIndex = 4;
            this.snapShots1.Text = "snapShots1";
            // 
            // videoListView1
            // 
            this.videoListView1.FullRowSelect = true;
            this.videoListView1.Location = new System.Drawing.Point(35, 21);
            this.videoListView1.Name = "videoListView1";
            this.videoListView1.Size = new System.Drawing.Size(215, 289);
            this.videoListView1.TabIndex = 3;
            this.videoListView1.ThumbBorderColor = System.Drawing.Color.Black;
            this.videoListView1.ThumbNailSize = 32;
            this.videoListView1.UseCompatibleStateImageBehavior = false;
            this.videoListView1.View = System.Windows.Forms.View.List;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(298, 316);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save Frames";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 661);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.snapShots1);
            this.Controls.Add(this.videoListView1);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.MaximizeBox = false;
            this.Name = "Form";
            this.Text = "Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private VideoListView videoListView1;
        private SnapShots snapShots1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnSave;
    }
}

