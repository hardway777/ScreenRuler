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
            contextMenuStrip1 = new ContextMenuStrip(components);
            saveSessionMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            CalibrationItem = new ToolStripMenuItem();
            alwaysOnTopMenuItem = new ToolStripMenuItem();
            helpMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            CloseItem = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            saveSessionToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { saveSessionMenuItem, toolStripMenuItem2, toolStripSeparator1, CalibrationItem, alwaysOnTopMenuItem, helpMenuItem, toolStripMenuItem1, CloseItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(181, 170);
            // 
            // saveSessionMenuItem
            // 
            saveSessionMenuItem.Name = "saveSessionMenuItem";
            saveSessionMenuItem.Size = new Size(180, 22);
            saveSessionMenuItem.Text = "Save Session...";
            saveSessionMenuItem.Click += saveSessionMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(180, 22);
            toolStripMenuItem2.Text = "Load Session...";
            toolStripMenuItem2.Click += loadSessionMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // CalibrationItem
            // 
            CalibrationItem.Name = "CalibrationItem";
            CalibrationItem.Size = new Size(180, 22);
            CalibrationItem.Text = "Calibration...";
            CalibrationItem.Click += Calibration_Click;
            // 
            // alwaysOnTopMenuItem
            // 
            alwaysOnTopMenuItem.CheckOnClick = true;
            alwaysOnTopMenuItem.Name = "alwaysOnTopMenuItem";
            alwaysOnTopMenuItem.Size = new Size(180, 22);
            alwaysOnTopMenuItem.Text = "Always On Top";
            alwaysOnTopMenuItem.Click += alwaysOnTopMenuItem_Click;
            // 
            // helpMenuItem
            // 
            helpMenuItem.Name = "helpMenuItem";
            helpMenuItem.Size = new Size(180, 22);
            helpMenuItem.Text = "Help";
            helpMenuItem.Click += helpMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(177, 6);
            // 
            // CloseItem
            // 
            CloseItem.Name = "CloseItem";
            CloseItem.Size = new Size(180, 22);
            CloseItem.Text = "Close";
            CloseItem.Click += Close_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(32, 19);
            // 
            // saveSessionToolStripMenuItem
            // 
            saveSessionToolStripMenuItem.Name = "saveSessionToolStripMenuItem";
            saveSessionToolStripMenuItem.Size = new Size(32, 19);
            saveSessionToolStripMenuItem.Text = "Save Session...";
            // 
            // RulerForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.LightYellow;
            ClientSize = new Size(800, 70);
            ContextMenuStrip = contextMenuStrip1;
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "RulerForm";
            Opacity = 0.75D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Ruler";
            Resize += RulerForm_Resize;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.ToolStripMenuItem saveSessionToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;

        private System.Windows.Forms.ToolStripMenuItem saveSessionMenuItem;

        private System.Windows.Forms.ToolStripMenuItem alwaysOnTopMenuItem;

        #endregion




        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem helpMenuItem;
    }
}
