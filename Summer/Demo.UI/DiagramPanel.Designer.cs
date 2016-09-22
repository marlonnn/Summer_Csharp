
namespace Demo.UI
{
    partial class DiagramPanel
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.spectroscope1 = new Demo.UI.SpectroscopeB();
            this.spectroscope2 = new Demo.UI.SpectroscopeB();
            this.SuspendLayout();
            // 
            // spectroscope1
            // 
            this.spectroscope1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.spectroscope1.Location = new System.Drawing.Point(61, 51);
            this.spectroscope1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.spectroscope1.Name = "spectroscope1";
            this.spectroscope1.Size = new System.Drawing.Size(43, 43);
            this.spectroscope1.TabIndex = 3;
            // 
            // spectroscope2
            // 
            this.spectroscope2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.spectroscope2.Location = new System.Drawing.Point(61, 171);
            this.spectroscope2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.spectroscope2.Name = "spectroscope2";
            this.spectroscope2.Size = new System.Drawing.Size(43, 43);
            this.spectroscope2.TabIndex = 4;

            // 
            // DiagramPanel
            // 
            this.Controls.Add(this.spectroscope1);
            this.Controls.Add(this.spectroscope2);
            this.Size = new System.Drawing.Size(428, 349);
            this.ResumeLayout(false);

        }

        #endregion

        private SpectroscopeB spectroscope1;
        private SpectroscopeB spectroscope2;
    }
}
