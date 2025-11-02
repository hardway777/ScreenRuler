using System;
using System.Windows.Forms;

namespace ScreenRuler.Dialogs
{
    public partial class InputDialog : Form
    {
        public string InputText { get; private set; }

        public InputDialog(string title, string prompt)
        {
            InitializeComponent();
            this.Text = title;
            lblPrompt.Text = prompt;
            this.InputText = "";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.InputText = txtInput.Text;
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