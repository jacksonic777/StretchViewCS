using System;
using System.Windows.Forms;
using StretchViewCS.Utils;

namespace StretchViewCS.Forms
{
    /// <summary>
    /// ヘルプフォーム（DelphiのUfrmHelpのC#版）
    /// </summary>
    public partial class frmHelp : Form
    {
        private GroupBox groupBox1;
        private Label label3;
        private Label label2;
        private GroupBox groupBox2;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private GroupBox groupBox3;
        private Label lblFix;
        private Button btnOK;

        public frmHelp()
        {
            InitializeComponent();
            ApplyLocalization();
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.label3 = new Label();
            this.label2 = new Label();
            this.groupBox2 = new GroupBox();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.groupBox3 = new GroupBox();
            this.lblFix = new Label();
            this.btnOK = new Button();
            this.SuspendLayout();

            // groupBox1
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 80);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基本操作";

            // label2, label3
            this.label2.Location = new System.Drawing.Point(20, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(330, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "マウスで操作";

            this.label3.Location = new System.Drawing.Point(20, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(330, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "メニューから各種機能を選択";

            // groupBox2
            this.groupBox2.Location = new System.Drawing.Point(12, 100);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(360, 120);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ショートカットキー";

            // label4-7
            this.label4.Location = new System.Drawing.Point(20, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(330, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Ctrl+A: ズームアップ";

            this.label5.Location = new System.Drawing.Point(20, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(330, 20);
            this.label5.TabIndex = 1;
            this.label5.Text = "Ctrl+S: ズームダウン";

            this.label6.Location = new System.Drawing.Point(20, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(330, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = "Ctrl+D: 左右反転";

            this.label7.Location = new System.Drawing.Point(20, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(330, 20);
            this.label7.TabIndex = 3;
            this.label7.Text = "Ctrl+F: 上下反転";

            // groupBox3
            this.groupBox3.Location = new System.Drawing.Point(12, 230);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(360, 100);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "固定表示モード";

            // lblFix
            this.lblFix.Location = new System.Drawing.Point(20, 25);
            this.lblFix.Name = "lblFix";
            this.lblFix.Size = new System.Drawing.Size(330, 70);
            this.lblFix.TabIndex = 0;
            this.lblFix.Text = "表示対象ウィンドウにフォーカスを当てると、\r\n" +
                             "そのウィンドウに固定表示する機能です。\r\n" +
                             "固定したいウィンドウにフォーカスを当てた状態で、\r\n" +
                             "「Ctrl+E」を押すと、そのウィンドウが固定され、\r\n" +
                             "常に表示されるようになります。\r\n" +
                             "モードを解除したい場合は、再度「Ctrl+E」を押して、\r\n" +
                             "チェックを外してください。";

            // btnOK
            this.btnOK.Location = new System.Drawing.Point(297, 340);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += BtnOK_Click;

            // frmHelp
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 375);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.lblFix);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHelp";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "ヘルプ";
            this.ResumeLayout(false);
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplyLocalization()
        {
            this.Text = LocalizationManager.Text("Help.Title");
            groupBox1.Text = LocalizationManager.Text("Help.Basic");
            label2.Text = LocalizationManager.Text("Help.Mouse");
            label3.Text = LocalizationManager.Text("Help.Menu");
            groupBox2.Text = LocalizationManager.Text("Help.Shortcuts");
            label4.Text = LocalizationManager.Text("Help.ZoomUp");
            label5.Text = LocalizationManager.Text("Help.ZoomDown");
            label6.Text = LocalizationManager.Text("Help.FlipH");
            label7.Text = LocalizationManager.Text("Help.FlipV");
            groupBox3.Text = LocalizationManager.Text("Help.FixedMode");
            lblFix.Text = LocalizationManager.Text("Help.FixedText");
            btnOK.Text = "OK";
        }
    }
}
