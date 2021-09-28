using OpenTK.Windowing.Common;
using OpenTK.WinForms;
using System;
using System.Windows.Forms;
using System.IO;

namespace igbgui
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            var settings = new GLControlSettings()
            {
                Flags = ContextFlags.Debug,
                API = ContextAPI.OpenGL,
                APIVersion = Version.Parse("4.3.0.0"),
                Profile = ContextProfile.Core
            };
            var viewer = new GLViewer(settings) { Dock = DockStyle.Fill };
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
                    var igb = IGB.Load(File.ReadAllBytes(dialog.FileName));
                }
            }
        }
    }
}
