using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace StretchViewCS.Forms
{
    /// <summary>
    /// バージョン情報フォーム（DelphiのUVersionのC#版）
    /// </summary>
    public partial class frmVersion : Form
    {
        private Button okButton;
        private Panel panel1;
        private Label version;
        private TextBox edit1;
        private Label label1;
        private TextBox memo1;
        private TextBox memo2;

        public frmVersion()
        {
            InitializeComponent();
            LoadVersionInfo();
        }

        private void InitializeComponent()
        {
            this.okButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.version = new System.Windows.Forms.Label();
            this.edit1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.memo2 = new System.Windows.Forms.TextBox();
            this.memo1 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(297, 250);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.version);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(360, 50);
            this.panel1.TabIndex = 1;
            // 
            // version
            // 
            this.version.AutoSize = true;
            this.version.Location = new System.Drawing.Point(20, 20);
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(52, 12);
            this.version.TabIndex = 0;
            this.version.Text = "バージョン:";
            // 
            // edit1
            // 
            this.edit1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.edit1.Location = new System.Drawing.Point(12, 160);
            this.edit1.Name = "edit1";
            this.edit1.ReadOnly = true;
            this.edit1.Size = new System.Drawing.Size(360, 19);
            this.edit1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "説明:";
            // 
            // memo2
            // 
            this.memo2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.memo2.Location = new System.Drawing.Point(12, 190);
            this.memo2.Multiline = true;
            this.memo2.Name = "memo2";
            this.memo2.ReadOnly = true;
            this.memo2.Size = new System.Drawing.Size(360, 50);
            this.memo2.TabIndex = 5;
            // 
            // memo1
            // 
            this.memo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.memo1.Location = new System.Drawing.Point(12, 80);
            this.memo1.Multiline = true;
            this.memo1.Name = "memo1";
            this.memo1.ReadOnly = true;
            this.memo1.Size = new System.Drawing.Size(360, 60);
            this.memo1.TabIndex = 2;
            // 
            // frmVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 285);
            this.Controls.Add(this.memo2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edit1);
            this.Controls.Add(this.memo1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmVersion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "バージョン情報";
            this.Shown += new System.EventHandler(this.FrmVersion_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void LoadVersionInfo()
        {
            try
            {
                string exeName = Application.ExecutablePath;
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(exeName);

                if (versionInfo.FileVersion != null)
                {
                    version.Text = "バージョン: " + versionInfo.FileVersion;
                    memo1.Text = versionInfo.FileVersion;
                    edit1.Text = versionInfo.FileDescription ?? "";
                    memo2.Text = versionInfo.Comments ?? "";
                }
            }
            catch
            {
                version.Text = "バージョン: 不明";
            }
        }

        private void OkButton_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmVersion_Shown(object? sender, EventArgs e)
        {
            if (Owner != null)
            {
                TopMost = Owner.TopMost;
            }

            Activate();
        }
    }
}
