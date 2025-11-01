using System;
using System.Windows.Forms;

namespace ScreenRuler
{
    public partial class CalibrationForm : Form
    {
        private readonly int _currentRulerWidth;
        
        public CalibrationForm(int currentRulerWidth)
        {
            InitializeComponent();
            _currentRulerWidth = currentRulerWidth;
            LoadSettings();
        }
        
        private void LoadSettings()
        {
            numPixels.Value = _currentRulerWidth;
            
            double oldPixels = CalibrationSettings.Pixels;
            double oldUnits = CalibrationSettings.UnitsValue;

            if (oldPixels > 0)
            {
                decimal correspondingUnits = (decimal)((_currentRulerWidth / oldPixels) * oldUnits);
                numUnitsValue.Value = correspondingUnits;
            }
            else
            {
                numUnitsValue.Value = (decimal)CalibrationSettings.UnitsValue;
            }
            
            txtUnitName.Text = CalibrationSettings.UnitName;
        }
        
        private void btnOk_Click(object sender, EventArgs e)
        {
            CalibrationSettings.Pixels = (double)numPixels.Value;
            CalibrationSettings.UnitsValue = (double)numUnitsValue.Value;
            CalibrationSettings.UnitName = txtUnitName.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}