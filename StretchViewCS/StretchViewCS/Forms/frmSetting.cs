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
        private Label lblSamplingRate;
        private Label lblLanguage;
        private NumericUpDown nudSamplingRate;
        private ComboBox cmbLanguage;
        private Button btnOK;
        private Button btnCancel;
        private GroupBox groupBox1;

        public frmSetting()
        {
            InitializeComponent();
            ApplyLocalization();
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkResistProgram = new System.Windows.Forms.CheckBox();
            this.chkHotkeysEnabled = new System.Windows.Forms.CheckBox();
            this.lblSamplingRate = new System.Windows.Forms.Label();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.nudSamplingRate = new System.Windows.Forms.NumericUpDown();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudSamplingRate)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudSamplingRate);
            this.groupBox1.Controls.Add(this.cmbLanguage);
            this.groupBox1.Controls.Add(this.lblLanguage);
            this.groupBox1.Controls.Add(this.lblSamplingRate);
            this.groupBox1.Controls.Add(this.chkHotkeysEnabled);
            this.groupBox1.Controls.Add(this.chkResistProgram);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 170);
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
            // lblSamplingRate
            // 
            this.lblSamplingRate.AutoSize = true;
            this.lblSamplingRate.Location = new System.Drawing.Point(20, 88);
            this.lblSamplingRate.Name = "lblSamplingRate";
            this.lblSamplingRate.Size = new System.Drawing.Size(114, 12);
            this.lblSamplingRate.TabIndex = 2;
            this.lblSamplingRate.Text = "サンプリングレート(ms)";
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new System.Drawing.Point(20, 118);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(53, 12);
            this.lblLanguage.TabIndex = 4;
            this.lblLanguage.Text = "表示言語";
            // 
            // nudSamplingRate
            // 
            this.nudSamplingRate.Location = new System.Drawing.Point(160, 84);
            this.nudSamplingRate.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.nudSamplingRate.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSamplingRate.Name = "nudSamplingRate";
            this.nudSamplingRate.Size = new System.Drawing.Size(80, 19);
            this.nudSamplingRate.TabIndex = 3;
            this.nudSamplingRate.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Location = new System.Drawing.Point(160, 114);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(150, 20);
            this.cmbLanguage.TabIndex = 5;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 190);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 190);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 225);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "設定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSetting_FormClosing);
            this.Shown += new System.EventHandler(this.FrmSetting_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.nudSamplingRate)).EndInit();
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
                    mainForm.RestoreTopMostSetting();
                    break;
                }
            }
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            frmCap? mainForm = FindMainForm();

            IniManager.Instance.HotkeysEnabled = chkHotkeysEnabled.Checked;
            IniManager.Instance.SamplingRate = Decimal.ToInt32(nudSamplingRate.Value);
            IniManager.Instance.SetLanguage(GetSelectedLanguage());
            ResisterStartMenu(false, chkResistProgram.Checked, "StretchViewCS");

            if (mainForm != null)
            {
                mainForm.SetHotkeysEnabled(chkHotkeysEnabled.Checked);
                mainForm.SetSamplingRate(Decimal.ToInt32(nudSamplingRate.Value));
                mainForm.ApplyLocalization();
            }

            this.Close();
        }

        private void FrmSetting_Shown(object? sender, EventArgs e)
        {
            chkResistProgram.Checked = ResisterStartMenu(true, false, "StretchViewCS");
            chkHotkeysEnabled.Checked = IniManager.Instance.HotkeysEnabled;
            nudSamplingRate.Value = IniManager.Instance.SamplingRate;
            SetSelectedLanguage(IniManager.Instance.Language);
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

        private void ApplyLocalization()
        {
            this.Text = LocalizationManager.Text("Settings.Title");
            groupBox1.Text = LocalizationManager.Text("Settings.Title");
            chkResistProgram.Text = LocalizationManager.Text("Settings.StartMenu");
            chkHotkeysEnabled.Text = LocalizationManager.Text("Settings.HotkeysEnabled");
            lblSamplingRate.Text = LocalizationManager.Text("Settings.SamplingRate");
            lblLanguage.Text = LocalizationManager.Text("Settings.Language");
            btnOK.Text = "OK";
            btnCancel.Text = LocalizationManager.Text("Settings.Cancel");
            FillLanguageItems();
        }

        private void FillLanguageItems()
        {
            cmbLanguage.Items.Clear();
            cmbLanguage.Items.Add(LocalizationManager.Text("Settings.LanguageJapanese"));
            cmbLanguage.Items.Add(LocalizationManager.Text("Settings.LanguageEnglish"));
        }

        private string GetSelectedLanguage()
        {
            if (cmbLanguage.SelectedIndex == 0)
            {
                return LocalizationManager.Japanese;
            }

            if (cmbLanguage.SelectedIndex == 1)
            {
                return LocalizationManager.English;
            }

            throw new InvalidOperationException("Language is not selected.");
        }

        private void SetSelectedLanguage(string language)
        {
            string normalizedLanguage = LocalizationManager.NormalizeLanguage(language);
            if (normalizedLanguage == LocalizationManager.Japanese)
            {
                cmbLanguage.SelectedIndex = 0;
                return;
            }

            if (normalizedLanguage == LocalizationManager.English)
            {
                cmbLanguage.SelectedIndex = 1;
                return;
            }

            throw new InvalidOperationException("Unsupported language: " + language);
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
