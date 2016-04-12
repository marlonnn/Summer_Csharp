namespace HW.HTUP
{
    partial class FrmMain
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.tmReferesh = new System.Windows.Forms.Timer(this.components);
            this.ribbonMenu = new RibbonLib.Ribbon();
            this.SuspendLayout();
            // 
            // lbLog
            // 
            this.lbLog.BackColor = System.Drawing.Color.Black;
            this.lbLog.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbLog.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbLog.FormattingEnabled = true;
            this.lbLog.ItemHeight = 20;
            this.lbLog.Location = new System.Drawing.Point(12, 602);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(1224, 84);
            this.lbLog.TabIndex = 0;
            // 
            // tmReferesh
            // 
            this.tmReferesh.Interval = 1000;
            this.tmReferesh.Tick += new System.EventHandler(this.tmReferesh_Tick);
            // 
            // ribbonMenu
            // 
            this.ribbonMenu.Location = new System.Drawing.Point(0, 0);
            this.ribbonMenu.Minimized = false;
            this.ribbonMenu.Name = "ribbonMenu";
            this.ribbonMenu.ResourceName = "HW.HTUP.RibbonMarkup.ribbon";
            this.ribbonMenu.ShortcutTableResourceName = null;
            this.ribbonMenu.Size = new System.Drawing.Size(1248, 150);
            this.ribbonMenu.TabIndex = 1;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 702);
            this.Controls.Add(this.ribbonMenu);
            this.Controls.Add(this.lbLog);
            this.Name = "FrmMain";
            this.Text = "HTUP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.Timer tmReferesh;
        private RibbonLib.Ribbon ribbonMenu;
    }
}

