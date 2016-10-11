namespace Demo.Frames
{
    partial class SnapShots
    {

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmMakeDefaultImage = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmMakeDefaultImage});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 26);
            // 
            // tsmMakeDefaultImage
            // 
            this.tsmMakeDefaultImage.Name = "tsmMakeDefaultImage";
            this.tsmMakeDefaultImage.Size = new System.Drawing.Size(180, 22);
            this.tsmMakeDefaultImage.Text = "Make Default Image";
            this.tsmMakeDefaultImage.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsmMakeDefaultImage_DropDownItemClicked);
            // 
            // SnapShots
            // 
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Size = new System.Drawing.Size(148, 148);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmMakeDefaultImage;
    }
}
