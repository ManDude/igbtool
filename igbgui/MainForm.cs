using OpenTK.Windowing.Common;
using OpenTK.WinForms;
using System;
using System.Windows.Forms;
using System.IO;

namespace igbgui
{
    public partial class MainForm : Form
    {
        private IGB currentIGB;
        private IGBViewerSettings viewerSettings;

        public MainForm()
        {
            InitializeComponent();
            viewerSettings = new()
            {
                DisplayAABBs = true,
                DisplayOBBs = true
            };
            var settings = new GLControlSettings()
            {
                Flags = ContextFlags.Debug,
                API = ContextAPI.OpenGL,
                APIVersion = Version.Parse("4.3.0.0"),
                Profile = ContextProfile.Core
            };
            var viewer = new IGBViewer(viewerSettings, settings, () => { return currentIGB; }) { Dock = DockStyle.Fill };
            Controls.Add(viewer);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new()
            {
                Filter = "Intrinsic Graphics binary files|*.igb",
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    currentIGB = IGB.Load(File.ReadAllBytes(dialog.FileName));
                }
            }
        }

        private void viewerSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newform = new ViewerSettingsForm(viewerSettings);
            newform.ShowDialog();
        }
    }
}
