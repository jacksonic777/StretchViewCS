using System;
using System.Windows.Forms;

namespace StretchViewCS.Forms
{
    /// <summary>
    /// ライセンスフォーム（DelphiのUfrmLisenceのC#版）
    /// </summary>
    public partial class frmLicense : Form
    {
        private TextBox edit1;

        public frmLicense()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.edit1 = new TextBox();
            this.SuspendLayout();

            // edit1
            this.edit1.BorderStyle = BorderStyle.FixedSingle;
            this.edit1.Location = new System.Drawing.Point(12, 12);
            this.edit1.Name = "edit1";
            this.edit1.Size = new System.Drawing.Size(360, 19);
            this.edit1.TabIndex = 0;

            // frmLicense
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 45);
            this.Controls.Add(this.edit1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLicense";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "ライセンス";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
