using System;
using System.Windows.Forms;

namespace igbgui
{
    public partial class ViewerSettingsForm : Form
    {
        private IGBViewerSettings settings;

        public ViewerSettingsForm(IGBViewerSettings settings)
        {
            this.settings = settings;
            InitializeComponent();
            chkDisplayAABB.Checked = settings.DisplayAABBs;
            chkDisplayOBB.Checked = settings.DisplayOBBs;

            chkDisplayAABB.CheckedChanged += chkDisplayAABB_CheckedChanged;
            chkDisplayOBB.CheckedChanged += chkDisplayOBB_CheckedChanged;
        }

        private void chkDisplayAABB_CheckedChanged(object sender, EventArgs e)
        {
            settings.DisplayAABBs = chkDisplayAABB.Checked;
        }

        private void chkDisplayOBB_CheckedChanged(object sender, EventArgs e)
        {
            settings.DisplayOBBs = chkDisplayOBB.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
