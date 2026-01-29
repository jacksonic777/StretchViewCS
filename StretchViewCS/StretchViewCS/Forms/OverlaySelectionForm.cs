using System;
using System.Drawing;
using System.Windows.Forms;

namespace StretchViewCS.Forms
{
    /// <summary>
    /// デスクトップ全面に被せるオーバーレイウィンドウ。
    /// 半透明の暗い背景の上に、マウスドラッグで矩形選択を行う。
    /// 選択完了後、選択矩形の中心座標をコールバックで返す。
    /// </summary>
    public class OverlaySelectionForm : Form
    {
        private bool isDragging = false;
        private Point dragStart;
        private Rectangle selectionRect = Rectangle.Empty;

        /// <summary>
        /// 選択が完了したときに呼び出される。引数は画面座標での矩形中心。
        /// </summary>
        public event Action<Point>? SelectionCompleted;

        public OverlaySelectionForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;

            // 画面全体を覆う
            Bounds = Screen.PrimaryScreen.Bounds;

            // 半透明の黒で背景を塗る（やや暗くする）
            BackColor = Color.Black;
            Opacity = 0.25;

            Cursor = Cursors.Cross;

            // ダブルバッファでちらつき軽減
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            MouseDown += OverlaySelectionForm_MouseDown;
            MouseMove += OverlaySelectionForm_MouseMove;
            MouseUp += OverlaySelectionForm_MouseUp;
            KeyDown += OverlaySelectionForm_KeyDown;
        }

        private void OverlaySelectionForm_KeyDown(object? sender, KeyEventArgs e)
        {
            // Esc でキャンセル
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void OverlaySelectionForm_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            isDragging = true;
            dragStart = e.Location;
            selectionRect = new Rectangle(e.Location, Size.Empty);
            Invalidate();
        }

        private void OverlaySelectionForm_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!isDragging)
                return;

            int x1 = Math.Min(dragStart.X, e.X);
            int y1 = Math.Min(dragStart.Y, e.Y);
            int x2 = Math.Max(dragStart.X, e.X);
            int y2 = Math.Max(dragStart.Y, e.Y);

            selectionRect = Rectangle.FromLTRB(x1, y1, x2, y2);
            Invalidate();
        }

        private void OverlaySelectionForm_MouseUp(object? sender, MouseEventArgs e)
        {
            if (!isDragging || e.Button != MouseButtons.Left)
                return;

            isDragging = false;

            if (selectionRect.Width > 5 && selectionRect.Height > 5)
            {
                // 選択矩形の中心を計算（画面座標）
                Point center = new Point(
                    selectionRect.Left + selectionRect.Width / 2,
                    selectionRect.Top + selectionRect.Height / 2);

                SelectionCompleted?.Invoke(center);
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }

            Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Pen pen = new Pen(Color.Red, 2))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                if (!selectionRect.IsEmpty)
                {
                    e.Graphics.DrawRectangle(pen, selectionRect);
                }
            }
        }
    }
}

