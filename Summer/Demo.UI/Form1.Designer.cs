
using System.Drawing;

namespace Demo.UI
{
    partial class Form1
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
            this.light1 = new TranspControl.Light();
            this.pmT_back1 = new Demo.UI.PMT_back();
            this.pmt1 = new Demo.UI.PMT();
            this.customPanel1 = new Demo.UI.OptionPanel();
            this.SuspendLayout();
            // 
            // light1
            // 
            this.light1.BackColor = System.Drawing.Color.Transparent;
            this.light1.BackImage = null;
            this.light1.FillColor = System.Drawing.Color.White;
            this.light1.GlassColor = System.Drawing.Color.Transparent;
            this.light1.GlassMode = true;
            this.light1.LightText = "VL8";
            this.light1.LineWidth = 2;
            this.light1.Location = new System.Drawing.Point(403, 502);
            this.light1.Name = "light1";
            this.light1.Opacity = 100;
            this.light1.Size = new System.Drawing.Size(18, 30);
            this.light1.TabIndex = 13;
            this.light1.TranspKey = System.Drawing.Color.White;
            // 
            // pmT_back1
            // 
            this.pmT_back1.Active = true;
            this.pmT_back1.ButtonText = "VLS";
            this.pmT_back1.Location = new System.Drawing.Point(278, 502);
            this.pmT_back1.Name = "pmT_back1";
            this.pmT_back1.PMTLight = Demo.UI.PMT_back.Light.VLS;
            this.pmT_back1.Size = new System.Drawing.Size(75, 30);
            this.pmT_back1.TabIndex = 12;
            // 
            // pmt1
            // 
            this.pmt1.Active = true;
            this.pmt1.ButtonText = "VLS BL4 RL1 YL3";
            this.pmt1.Location = new System.Drawing.Point(692, 546);
            this.pmt1.Name = "pmt1";
            this.pmt1.Size = new System.Drawing.Size(75, 30);
            this.pmt1.TabIndex = 11;
            // 
            // customPanel1
            // 
            this.customPanel1.ForeColor = System.Drawing.Color.Transparent;
            this.customPanel1.Location = new System.Drawing.Point(19, 52);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(709, 456);
            this.customPanel1.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 663);
            this.Controls.Add(this.light1);
            this.Controls.Add(this.pmT_back1);
            this.Controls.Add(this.pmt1);
            this.Controls.Add(this.customPanel1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private OptionPanel customPanel1;
        private PMT pmt1;
        private PMT_back pmT_back1;
        private TranspControl.Light light1;
    }
}

