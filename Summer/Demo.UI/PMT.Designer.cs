namespace Demo.UI
{
    partial class PMT
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.orientedTextLabel1 = new Summer.UI.Text.OrientedTextLabel();
            this.SuspendLayout();
            // 
            // orientedTextLabel1
            // 
            this.orientedTextLabel1.BackColor = System.Drawing.Color.Transparent;
            this.orientedTextLabel1.Location = new System.Drawing.Point(-3, 18);
            this.orientedTextLabel1.Name = "orientedTextLabel1";
            this.orientedTextLabel1.RotationAngle = 0D;
            this.orientedTextLabel1.Size = new System.Drawing.Size(63, 12);
            this.orientedTextLabel1.TabIndex = 0;
            this.orientedTextLabel1.TextDirection = Summer.UI.Text.Direction.Clockwise;
            this.orientedTextLabel1.TextOrientation = Summer.UI.Text.Orientation.Rotate;
            // 
            // PMT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.orientedTextLabel1);
            this.Size = new System.Drawing.Size(60, 30);
            this.ResumeLayout(false);

        }

        #endregion

        private Summer.UI.Text.OrientedTextLabel orientedTextLabel1;
    }
}
