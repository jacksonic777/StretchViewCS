using System;
using System.IO;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using StretchViewCS.Utils;

namespace StretchViewCS.Forms
{
    /// <summary>
    /// 設定フォーム（DelphiのUfrmSettingのC#版）
    /// </summary>
    public partial class frmSetting : Form
    {
        private CheckBox chkResistProgram;
        private CheckBox chkHotkeysEnabled;
        private Button btnOK;
        private Button btnCancel;
        private GroupBox groupBox1;

        public frmSetting()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkResistProgram = new System.Windows.Forms.CheckBox();
            this.chkHotkeysEnabled = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkHotkeysEnabled);
            this.groupBox1.Controls.Add(this.chkResistProgram);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 110);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "設定";
            // 
            // chkResistProgram
            // 
            this.chkResistProgram.AutoSize = true;
            this.chkResistProgram.Location = new System.Drawing.Point(20, 25);
            this.chkResistProgram.Name = "chkResistProgram";
            this.chkResistProgram.Size = new System.Drawing.Size(127, 16);
            this.chkResistProgram.TabIndex = 0;
            this.chkResistProgram.Text = "スタートメニューに登録";
            // 
            // chkHotkeysEnabled
            // 
            this.chkHotkeysEnabled.AutoSize = true;
            this.chkHotkeysEnabled.Location = new System.Drawing.Point(20, 55);
            this.chkHotkeysEnabled.Name = "chkHotkeysEnabled";
            this.chkHotkeysEnabled.Size = new System.Drawing.Size(108, 16);
            this.chkHotkeysEnabled.TabIndex = 1;
            this.chkHotkeysEnabled.Text = "ホットキー有効";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 130);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 130);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 165);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "設定";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmSetting_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // メインフォームの処理を呼び出す
            foreach (Form form in Application.OpenForms)
            {
                if (form is frmCap mainForm)
                {
                    mainForm.bShowDlgBox = false;
                    mainForm.SwitchTopMost(true);
                    break;
                }
            }
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            frmCap? mainForm = FindMainForm();

            IniManager.Instance.HotkeysEnabled = chkHotkeysEnabled.Checked;
            ResisterStartMenu(false, chkResistProgram.Checked, "StretchViewCS");

            if (mainForm != null)
            {
                mainForm.SetHotkeysEnabled(chkHotkeysEnabled.Checked);
            }

            this.Close();
        }

        private void FrmSetting_Shown(object? sender, EventArgs e)
        {
            chkResistProgram.Checked = ResisterStartMenu(true, false, "StretchViewCS");
            chkHotkeysEnabled.Checked = IniManager.Instance.HotkeysEnabled;
        }

        private frmCap? FindMainForm()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is frmCap mainForm)
                {
                    return mainForm;
                }
            }

            return null;
        }

        /// <summary>
        /// スタートメニューへの登録/削除
        /// </summary>
        public bool ResisterStartMenu(bool checkOnly, bool onoff, string strLinkName)
        {
            try
            {
                string fileName = Application.ExecutablePath;
                string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                string linkFileName = Path.Combine(startMenuPath, strLinkName + ".lnk");
                bool bExists = System.IO.File.Exists(linkFileName);

                // チェックのみの場合
                if (checkOnly)
                {
                    return bExists;
                }

                // 登録/削除処理
                if (onoff)
                {
                    // 登録
                    if (!bExists)
                    {
                        WshShell shell = new WshShell();
                        IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(linkFileName);
                        shortcut.TargetPath = fileName;
                        shortcut.WorkingDirectory = Path.GetDirectoryName(fileName);
                        shortcut.Save();
                        return true;
                    }
                }
                else
                {
                    // 削除
                    if (bExists)
                    {
                        System.IO.File.Delete(linkFileName);
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
