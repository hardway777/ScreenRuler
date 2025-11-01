namespace ScreenRuler
{
    partial class RulerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem CalibrationItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem CloseItem;
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            CalibrationItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            CloseItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { CalibrationItem, toolStripMenuItem1, CloseItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(150, 54);
            // 
            // CalibrationItem
            // 
            CalibrationItem.Name = "CalibrationItem";
            CalibrationItem.Size = new Size(149, 22);
            CalibrationItem.Text = "Калибровка...";
            CalibrationItem.Click += Calibration_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(146, 6);
            // 
            // CloseItem
            // 
            CloseItem.Name = "CloseItem";
            CloseItem.Size = new Size(149, 22);
            CloseItem.Text = "Закрыть";
            CloseItem.Click += Close_Click;
            // 
            // RulerForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.LightYellow;
            ClientSize = new Size(800, 70);
            ContextMenuStrip = contextMenuStrip1;
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "RulerForm";
            Opacity = 0.75D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            TopMost = true;
            Resize += RulerForm_Resize;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion




    }
}
