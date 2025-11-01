namespace ScreenRuler
{
    partial class RulerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CalibrationItem;
        private ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem CloseItem;
        /// <summary>
        ///  Clean up any resources being used.
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RulerForm));
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            CalibrationItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            CloseItem = new System.Windows.Forms.ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { CalibrationItem, toolStripMenuItem1, CloseItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(142, 54);
            // 
            // CalibrationItem
            // 
            CalibrationItem.Name = "CalibrationItem";
            CalibrationItem.Size = new System.Drawing.Size(141, 22);
            CalibrationItem.Text = "Calibration...";
            CalibrationItem.Click += Calibration_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(138, 6);
            // 
            // CloseItem
            // 
            CloseItem.Name = "CloseItem";
            CloseItem.Size = new System.Drawing.Size(141, 22);
            CloseItem.Text = "Close";
            CloseItem.Click += Close_Click;
            // 
            // RulerForm
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = System.Drawing.Color.LightYellow;
            ClientSize = new System.Drawing.Size(800, 70);
            ContextMenuStrip = contextMenuStrip1;
            DoubleBuffered = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
            Opacity = 0.75D;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Ruler";
            Resize += RulerForm_Resize;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion




    }
}
