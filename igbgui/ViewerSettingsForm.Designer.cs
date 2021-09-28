
namespace igbgui
{
    partial class ViewerSettingsForm
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
            this.chkDisplayAABB = new System.Windows.Forms.CheckBox();
            this.chkDisplayOBB = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkDisplayAABB
            // 
            this.chkDisplayAABB.AutoSize = true;
            this.chkDisplayAABB.Location = new System.Drawing.Point(12, 12);
            this.chkDisplayAABB.Name = "chkDisplayAABB";
            this.chkDisplayAABB.Size = new System.Drawing.Size(242, 21);
            this.chkDisplayAABB.TabIndex = 0;
            this.chkDisplayAABB.Text = "Display axis-aligned bounding boxes";
            this.chkDisplayAABB.UseVisualStyleBackColor = true;
            // 
            // chkDisplayOBB
            // 
            this.chkDisplayOBB.AutoSize = true;
            this.chkDisplayOBB.Location = new System.Drawing.Point(12, 39);
            this.chkDisplayOBB.Name = "chkDisplayOBB";
            this.chkDisplayOBB.Size = new System.Drawing.Size(221, 21);
            this.chkDisplayOBB.TabIndex = 1;
            this.chkDisplayOBB.Text = "Display oriented bounding boxes";
            this.chkDisplayOBB.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(172, 122);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 27);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ViewerSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 161);
            this.ControlBox = false;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkDisplayOBB);
            this.Controls.Add(this.chkDisplayAABB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ViewerSettingsForm";
            this.Text = "Viewer Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDisplayAABB;
        private System.Windows.Forms.CheckBox chkDisplayOBB;
        private System.Windows.Forms.Button btnOK;
    }
}
