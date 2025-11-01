namespace ScreenRuler
{
    partial class CalibrationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalibrationForm));
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            numPixels = new System.Windows.Forms.NumericUpDown();
            numUnitsValue = new System.Windows.Forms.NumericUpDown();
            txtUnitName = new System.Windows.Forms.TextBox();
            btnCancel = new System.Windows.Forms.Button();
            btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)numPixels).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numUnitsValue).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(61, 25);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(36, 15);
            label1.TabIndex = 0;
            label1.Text = "Pixels";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(197, 25);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(15, 15);
            label2.TabIndex = 1;
            label2.Text = "=";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(307, 25);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(34, 15);
            label3.TabIndex = 2;
            label3.Text = "Units";
            // 
            // numPixels
            // 
            numPixels.Location = new System.Drawing.Point(103, 23);
            numPixels.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numPixels.Name = "numPixels";
            numPixels.Size = new System.Drawing.Size(76, 23);
            numPixels.TabIndex = 3;
            // 
            // numUnitsValue
            // 
            numUnitsValue.DecimalPlaces = 2;
            numUnitsValue.Location = new System.Drawing.Point(218, 23);
            numUnitsValue.Maximum = new decimal(new int[] { 20000, 0, 0, 0 });
            numUnitsValue.Name = "numUnitsValue";
            numUnitsValue.Size = new System.Drawing.Size(73, 23);
            numUnitsValue.TabIndex = 4;
            // 
            // txtUnitName
            // 
            txtUnitName.Location = new System.Drawing.Point(384, 22);
            txtUnitName.Name = "txtUnitName";
            txtUnitName.Size = new System.Drawing.Size(102, 23);
            txtUnitName.TabIndex = 5;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(388, 65);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(98, 36);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnOk
            // 
            btnOk.Location = new System.Drawing.Point(279, 65);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(103, 36);
            btnOk.TabIndex = 7;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // CalibrationForm
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(519, 113);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            Controls.Add(txtUnitName);
            Controls.Add(numUnitsValue);
            Controls.Add(numPixels);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            Text = "Calibration";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)numPixels).EndInit();
            ((System.ComponentModel.ISupportInitialize)numUnitsValue).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Label label2;
        private System.Windows.Forms.Label label3;
        private NumericUpDown numPixels;
        private NumericUpDown numUnitsValue;
        private System.Windows.Forms.TextBox txtUnitName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}