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
        private const uint ColorReadFailed = 0xFFFFFFFF;

        private ColorPickerInfoForm? _infoForm;
        private Point _lastScreenPoint;
        private Color _lastColor;
        private bool _hasLastColor;

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

            KeyDown += ColorPickerForm_KeyDown;
            MouseMove += ColorPickerForm_MouseMove;
            MouseClick += ColorPickerForm_MouseClick;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            _infoForm = new ColorPickerInfoForm();
            _infoForm.Show(this);

            Point screenPt = Cursor.Position;
            UpdateColorInfo(screenPt, true);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_infoForm != null)
            {
                _infoForm.Close();
                _infoForm.Dispose();
                _infoForm = null;
            }

            base.OnFormClosed(e);
        }

        private void ColorPickerForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void ColorPickerForm_MouseMove(object? sender, MouseEventArgs e)
        {
            Point screenPt = PointToScreen(e.Location);
            UpdateColorInfo(screenPt, false);
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

        private void UpdateColorInfo(Point screenPt, bool forceUpdate)
        {
            Color color = GetColorAtScreen(screenPt.X, screenPt.Y);
            bool pointChanged = !_hasLastColor || _lastScreenPoint != screenPt;
            bool colorChanged = !_hasLastColor || _lastColor.ToArgb() != color.ToArgb();
            if (!forceUpdate && !pointChanged && !colorChanged)
            {
                return;
            }

            _lastScreenPoint = screenPt;
            _lastColor = color;
            _hasLastColor = true;

            if (_infoForm == null)
            {
                throw new InvalidOperationException("Color picker information form has not been initialized.");
            }

            _infoForm.UpdateInfo(screenPt, color, Bounds);
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
                if (cr == ColorReadFailed)
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
