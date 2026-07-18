using System;
using System.Drawing;
using System.Windows.Forms;
using StretchViewCS.Native;

namespace StretchViewCS.Forms
{
    /// <summary>
    /// 範囲指定中の「矩形・開始点・寸法」を画面上に描画する透明オーバーレイ。
    /// TransparencyKey で背景だけ透過し、描画部分だけ見えるようにする。
    /// </summary>
    internal class RulerDrawForm : Form
    {
        private Rectangle _rect;
        private Point _startPoint;
        private string _sizeText = "";
        private bool _hasRect;
        private bool _hasStart;

        private static readonly Color TransparentKey = Color.Magenta;

        public RulerDrawForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            TopMost = false;
            Bounds = Screen.PrimaryScreen.Bounds;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(0, 0);
            BackColor = TransparentKey;
            TransparencyKey = TransparentKey;
            ShowInTaskbar = false;
            DoubleBuffered = true;
        }

        public void SetDraw(Rectangle rect, Point startPoint, string sizeText)
        {
            _rect = rect;
            _startPoint = startPoint;
            _sizeText = sizeText ?? "";
            _hasRect = rect.Width >= 1 && rect.Height >= 1;
            _hasStart = true;
            Invalidate();
        }

        public void Clear()
        {
            _hasRect = false;
            _hasStart = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (_hasStart)
            {
                const int r = 8;
                using (var pen = new Pen(Color.Lime, 2))
                using (var brush = new SolidBrush(Color.FromArgb(180, 0, 255, 0)))
                {
                    g.FillEllipse(brush, _startPoint.X - r, _startPoint.Y - r, r * 2, r * 2);
                    g.DrawEllipse(pen, _startPoint.X - r, _startPoint.Y - r, r * 2, r * 2);
                }
            }

            if (_hasRect)
            {
                using (var pen = new Pen(Color.Red, 2))
                using (var brush = new SolidBrush(Color.FromArgb(50, 255, 0, 0)))
                {
                    g.FillRectangle(brush, _rect);
                    g.DrawRectangle(pen, _rect);
                }

                if (!string.IsNullOrEmpty(_sizeText))
                {
                    using (var font = new Font("Meiryo UI", 11f, FontStyle.Bold))
                    using (var bgBrush = new SolidBrush(Color.FromArgb(240, 255, 255, 220)))
                    using (var textBrush = new SolidBrush(Color.DarkBlue))
                    using (var borderPen = new Pen(Color.FromArgb(200, 0, 0, 128), 1))
                    {
                        var size = g.MeasureString(_sizeText, font);
                        int pad = 6;
                        int w = (int)size.Width + pad * 2;
                        int h = (int)size.Height + pad * 2;
                        int x = _rect.Left;
                        int y = _rect.Bottom + 4;
                        if (y + h > Bounds.Bottom) y = _rect.Top - h - 4;
                        if (x + w > Bounds.Right) x = Bounds.Right - w - 4;
                        if (x < 4) x = 4;
                        if (y < 4) y = 4;
                        var box = new Rectangle(x, y, w, h);
                        g.FillRectangle(bgBrush, box);
                        g.DrawRectangle(borderPen, box);
                        g.DrawString(_sizeText, font, textBrush, x + pad, y + pad);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 寸法表示用の独立ウィンドウ（オーバーレイの透過の影響を受けない）
    /// </summary>
    internal class RulerInfoForm : Form
    {
        private readonly Label _lblMain;
        private readonly Label _lblSub;

        public RulerInfoForm()
        {
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            Size = new Size(340, 110);
            BackColor = Color.FromArgb(255, 255, 240);
            ShowInTaskbar = false;
            Text = "画面定規 - 寸法";
            Padding = new Padding(10);

            _lblMain = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 36,
                Font = new Font("Meiryo UI", 16f, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "幅 --- px  ×  高さ --- px"
            };
            _lblSub = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Meiryo UI", 10f),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "左ドラッグで範囲を指定　Esc/右クリックで終了"
            };
            // DockStyle.Top は後から追加した方が上に来る
            Controls.Add(_lblSub);
            Controls.Add(_lblMain);
        }

        public void SetSize(int widthPx, int heightPx, string mmText)
        {
            _lblMain.Text = $"幅 {widthPx} px  ×  高さ {heightPx} px";
            _lblSub.Text = string.IsNullOrEmpty(mmText) ? "左ドラッグで範囲を指定　Esc/右クリックで終了" : mmText;
        }

        public void SetInstruction(string text)
        {
            _lblMain.Text = text;
            _lblSub.Text = "Esc または 右クリック で終了";
        }
    }

    /// <summary>
    /// デスクトップ画面上の幅・高さをピクセル（および mm）で計測する画面定規オーバーレイ。
    /// ドラッグで矩形を描き、寸法を別ウィンドウで明確に表示する。
    /// </summary>
    public class RulerForm : Form
    {
        private Point _startPt;
        private Point _currentPt;
        private bool _dragging;
        private Rectangle _fixedRect;
        private RulerInfoForm _infoForm;
        private RulerDrawForm _drawForm;
        private bool _cursorRestored;

        public RulerForm()
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

            _infoForm = new RulerInfoForm();
            _infoForm.SetInstruction("左ドラッグで計測したい範囲を指定");
            _drawForm = new RulerDrawForm();

            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                    Close();
            };
            MouseDown += RulerForm_MouseDown;
            MouseMove += RulerForm_MouseMove;
            MouseUp += RulerForm_MouseUp;
            Paint += RulerForm_Paint;
            Load += RulerForm_Load;
            FormClosing += RulerForm_FormClosing;
        }

        private void RulerForm_Load(object? sender, EventArgs e)
        {
            _infoForm.Location = new Point(Bounds.Left + 24, Bounds.Top + 24);
            _infoForm.Show();
            _drawForm.Show();
        }

        private void RulerForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            RestoreCursor();
            _drawForm?.Close();
            _drawForm = null!;
            _infoForm?.Close();
            _infoForm = null!;
        }

        protected override void Dispose(bool disposing)
        {
            RestoreCursor();
            base.Dispose(disposing);
        }

        private void RulerForm_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Close();
                return;
            }
            if (e.Button != MouseButtons.Left)
                return;
            _dragging = true;
            _fixedRect = Rectangle.Empty;
            _startPt = e.Location;
            _currentPt = _startPt;
            _drawForm?.SetDraw(Rectangle.Empty, _startPt, "");
            Invalidate();
        }

        private void RulerForm_MouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                return;
            if (_dragging)
            {
                _currentPt = e.Location;
                var r = NormalizedRect(_startPt, _currentPt);
                UpdateSizeDisplay(r);
                _drawForm?.SetDraw(r, _startPt, $"{r.Width} × {r.Height} px");
                Invalidate();
            }
            else if (_fixedRect != Rectangle.Empty)
            {
                PositionInfoForm(_fixedRect);
            }
        }

        private void RulerForm_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            _dragging = false;
            Rectangle r = NormalizedRect(_startPt, _currentPt);
            if (r.Width >= 2 && r.Height >= 2)
            {
                _fixedRect = r;
                UpdateSizeDisplay(_fixedRect);
                PositionInfoForm(_fixedRect);
                _drawForm?.SetDraw(_fixedRect, _startPt, $"{r.Width} × {r.Height} px");
            }
            else
            {
                _fixedRect = Rectangle.Empty;
                _infoForm?.SetInstruction("左ドラッグで計測したい範囲を指定");
                _drawForm?.Clear();
            }
            Invalidate();
        }

        private void RulerForm_Paint(object? sender, PaintEventArgs e)
        {
            Rectangle toDraw = _dragging ? NormalizedRect(_startPt, _currentPt) : _fixedRect;
            if (toDraw.IsEmpty || (toDraw.Width < 1 && toDraw.Height < 1))
                return;

            using (var pen = new Pen(Color.Red, 2))
            using (var brush = new SolidBrush(Color.FromArgb(40, 255, 0, 0)))
            {
                e.Graphics.FillRectangle(brush, toDraw);
                e.Graphics.DrawRectangle(pen, toDraw);
            }
        }

        private static Rectangle NormalizedRect(Point a, Point b)
        {
            int x = Math.Min(a.X, b.X);
            int y = Math.Min(a.Y, b.Y);
            int w = Math.Abs(b.X - a.X);
            int h = Math.Abs(b.Y - a.Y);
            return new Rectangle(x, y, w, h);
        }

        private void UpdateSizeDisplay(Rectangle r)
        {
            int w = r.Width;
            int h = r.Height;
            string mmText = "";
            IntPtr hDesk = Win32API.GetDesktopWindow();
            IntPtr hdc = Win32API.GetWindowDC(hDesk);
            try
            {
                int dpiX = Win32API.GetDeviceCaps(hdc, Win32API.LOGPIXELSX);
                int dpiY = Win32API.GetDeviceCaps(hdc, Win32API.LOGPIXELSY);
                if (dpiX > 0 && dpiY > 0)
                {
                    double mmW = w * 25.4 / dpiX;
                    double mmH = h * 25.4 / dpiY;
                    mmText = $"{mmW:F1} mm × {mmH:F1} mm";
                }
            }
            finally
            {
                Win32API.ReleaseDC(hDesk, hdc);
            }
            _infoForm?.SetSize(w, h, mmText);
        }

        private void PositionInfoForm(Rectangle r)
        {
            if (_infoForm == null) return;
            int margin = 12;
            int x = r.Right + margin;
            int y = r.Top;
            if (x + _infoForm.Width > Bounds.Right)
                x = r.Left - _infoForm.Width - margin;
            if (y + _infoForm.Height > Bounds.Bottom)
                y = r.Bottom - _infoForm.Height;
            if (x < Bounds.Left)
                x = Bounds.Left + margin;
            if (y < Bounds.Top)
                y = Bounds.Top + margin;
            _infoForm.Location = new Point(x, y);
        }

        private void RestoreCursor()
        {
            if (_cursorRestored)
                return;

            _cursorRestored = true;
            Capture = false;
            Cursor = Cursors.Default;
            Cursor.Current = Cursors.Default;
        }
    }
}
