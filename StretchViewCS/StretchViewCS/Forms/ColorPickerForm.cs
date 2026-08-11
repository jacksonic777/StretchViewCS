using System;
using System.Drawing;
using System.Windows.Forms;
using StretchViewCS.Native;
using StretchViewCS.Utils;

namespace StretchViewCS.Forms
{
    /// <summary>
    /// デスクトップ全体を対象としたカラーピッカー。
    /// 全画面の透明オーバーレイで、クリックした位置の色を取得しクリップボードにコピーする。
    /// </summary>
    public class ColorPickerForm : Form
    {
        private Label _lblColor;
        private Panel _pnlPreview;
        public ColorPickerForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            TopMost = true;
            BackColor = Color.Black;
            Opacity = 0.01;
            Cursor = Cursors.Cross;
            KeyPreview = true;
            Bounds = Screen.PrimaryScreen.Bounds;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(0, 0);
            Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            DoubleBuffered = true;

            _pnlPreview = new Panel
            {
                Size = new Size(140, 60),
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.FixedSingle
            };
            _lblColor = new Label
            {
                AutoSize = false,
                Size = new Size(130, 40),
                Location = new Point(5, 5),
                Font = new Font("Consolas", 9f),
                Text = "#000000\r\nR=0 G=0 B=0"
            };
            _pnlPreview.Controls.Add(_lblColor);
            Controls.Add(_pnlPreview);

            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                    Close();
            };
            MouseMove += ColorPickerForm_MouseMove;
            MouseClick += ColorPickerForm_MouseClick;
        }

        private void ColorPickerForm_MouseMove(object? sender, MouseEventArgs e)
        {
            Point screenPt = PointToScreen(e.Location);
            UpdateColorLabel(screenPt.X, screenPt.Y);

            // ラベルをカーソル近くに表示（画面内に収める）
            int margin = 12;
            int x = screenPt.X + margin;
            int y = screenPt.Y + margin;
            if (x + _pnlPreview.Width > Bounds.Right)
                x = screenPt.X - _pnlPreview.Width - margin;
            if (y + _pnlPreview.Height > Bounds.Bottom)
                y = screenPt.Y - _pnlPreview.Height - margin;
            if (x < Bounds.Left)
                x = Bounds.Left + margin;
            if (y < Bounds.Top)
                y = Bounds.Top + margin;
            _pnlPreview.Location = new Point(x, y);
        }

        private void ColorPickerForm_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            Point screenPt = PointToScreen(e.Location);
            Color c = GetColorAtScreen(screenPt.X, screenPt.Y);
            string hex = $"#{c.R:X2}{c.G:X2}{c.B:X2}";
            try
            {
                Clipboard.SetText(hex);
            }
            catch
            {
                // クリップボードにアクセスできない場合
            }
            MessageBox.Show(
                LocalizationManager.Text("ColorPicker.CopiedBody") + $"\r\n{hex}\r\nR={c.R} G={c.G} B={c.B}",
                LocalizationManager.Text("ColorPicker.CopiedTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            Close();
        }

        private void UpdateColorLabel(int screenX, int screenY)
        {
            Color c = GetColorAtScreen(screenX, screenY);
            _lblColor.Text = $"#{c.R:X2}{c.G:X2}{c.B:X2}\r\nR={c.R} G={c.G} B={c.B}";
            _pnlPreview.BackColor = c;
            _lblColor.ForeColor = GetContrastColor(c);
        }

        private static Color GetContrastColor(Color c)
        {
            double luminance = (0.299 * c.R + 0.587 * c.G + 0.114 * c.B) / 255.0;
            return luminance > 0.5 ? Color.Black : Color.White;
        }

        /// <summary>
        /// 指定した画面座標のデスクトップ上の色を取得する。
        /// </summary>
        private static Color GetColorAtScreen(int screenX, int screenY)
        {
            IntPtr hDesk = Win32API.GetDesktopWindow();
            IntPtr hdc = Win32API.GetWindowDC(hDesk);
            try
            {
                uint cr = Win32API.GetPixel(hdc, screenX, screenY);
                if (cr == 0xFFFFFFFF)
                    return Color.Black;
                int r = (int)(cr & 0xFF);
                int g = (int)((cr >> 8) & 0xFF);
                int b = (int)((cr >> 16) & 0xFF);
                return Color.FromArgb(r, g, b);
            }
            finally
            {
                Win32API.ReleaseDC(hDesk, hdc);
            }
        }
    }
}
