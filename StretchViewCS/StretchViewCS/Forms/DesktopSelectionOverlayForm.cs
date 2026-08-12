using System;
using System.Drawing;
using System.Windows.Forms;
using StretchViewCS.Native;

namespace StretchViewCS.Forms
{
    internal sealed class DesktopSelectionOverlayForm : Form
    {
        private const int WsExLayered = 0x80000;
        private const int WsExToolWindow = 0x80;
        private const int WsExNoActivate = 0x8000000;
        private const uint SwpNoActivate = 0x0010;
        private const int WM_NCHITTEST = 0x0084;
        private const int HTTRANSPARENT = -1;
        private const int SelectionBorderWidth = 1;
        private const int InvalidateMargin = SelectionBorderWidth + 4;
        private readonly Color transparentColorKey = Color.Magenta;
        private Rectangle selectionRect = Rectangle.Empty;
        private bool isDragging = false;
        private Point dragStartScreen = Point.Empty;
        private Rectangle dragStartRect = Rectangle.Empty;

        public event Action<Rectangle>? SelectionRectChanged;

        public DesktopSelectionOverlayForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            TopMost = true;
            BackColor = transparentColorKey;
            TransparencyKey = transparentColorKey;
            DoubleBuffered = true;
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WsExLayered | WsExToolWindow | WsExNoActivate;
                return cp;
            }
        }

        public void ShowSelection(Rectangle desktopRect)
        {
            Rectangle virtualScreen = SystemInformation.VirtualScreen;
            Rectangle previousRect = selectionRect;
            selectionRect = desktopRect;
            Bounds = virtualScreen;

            if (!Visible)
            {
                Show();
            }

            Win32API.SetWindowPos(
                Handle,
                (IntPtr)Win32API.HWND_TOPMOST,
                virtualScreen.Left,
                virtualScreen.Top,
                virtualScreen.Width,
                virtualScreen.Height,
                SwpNoActivate);

            if (previousRect.IsEmpty)
            {
                Invalidate();
            }
            else
            {
                Rectangle invalidRect = Rectangle.Union(
                    ToClientRectangle(previousRect),
                    ToClientRectangle(selectionRect));
                invalidRect.Inflate(InvalidateMargin, InvalidateMargin);
                Invalidate(invalidRect);
            }

            Update();
        }

        public void HideSelection()
        {
            isDragging = false;
            selectionRect = Rectangle.Empty;
            Hide();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(transparentColorKey);

            if (selectionRect.IsEmpty)
            {
                return;
            }

            Rectangle drawRect = ToClientRectangle(selectionRect);
            drawRect.Width -= 1;
            drawRect.Height -= 1;

            using (Pen pen = new Pen(Color.LimeGreen, SelectionBorderWidth))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                e.Graphics.DrawRectangle(pen, drawRect);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCHITTEST)
            {
                Point screenPoint = new Point(
                    (short)((int)m.LParam & 0xFFFF),
                    (short)(((int)m.LParam >> 16) & 0xFFFF));

                if (!CanStartDrag(screenPoint))
                {
                    m.Result = (IntPtr)HTTRANSPARENT;
                    return;
                }
            }

            base.WndProc(ref m);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Point screenPoint = PointToScreen(e.Location);
            if (!CanStartDrag(screenPoint))
            {
                return;
            }

            isDragging = true;
            dragStartScreen = screenPoint;
            dragStartRect = selectionRect;
            Capture = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!isDragging)
            {
                return;
            }

            Point screenPoint = PointToScreen(e.Location);
            int offsetX = screenPoint.X - dragStartScreen.X;
            int offsetY = screenPoint.Y - dragStartScreen.Y;

            Rectangle nextRect = dragStartRect;
            nextRect.Offset(offsetX, offsetY);
            nextRect = ClampToVirtualScreen(nextRect);

            if (nextRect == selectionRect)
            {
                return;
            }

            ShowSelection(nextRect);
            SelectionRectChanged?.Invoke(selectionRect);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            isDragging = false;
            Capture = false;
        }

        private Rectangle ToClientRectangle(Rectangle desktopRect)
        {
            Rectangle virtualScreen = Bounds;
            return new Rectangle(
                desktopRect.Left - virtualScreen.Left,
                desktopRect.Top - virtualScreen.Top,
                desktopRect.Width,
                desktopRect.Height);
        }

        private bool CanStartDrag(Point screenPoint)
        {
            return !selectionRect.IsEmpty && selectionRect.Contains(screenPoint);
        }

        private Rectangle ClampToVirtualScreen(Rectangle desktopRect)
        {
            Rectangle virtualScreen = SystemInformation.VirtualScreen;
            Rectangle clampedRect = desktopRect;

            if (clampedRect.Left < virtualScreen.Left)
            {
                clampedRect.X = virtualScreen.Left;
            }

            if (clampedRect.Top < virtualScreen.Top)
            {
                clampedRect.Y = virtualScreen.Top;
            }

            if (clampedRect.Right > virtualScreen.Right)
            {
                clampedRect.X = virtualScreen.Right - clampedRect.Width;
            }

            if (clampedRect.Bottom > virtualScreen.Bottom)
            {
                clampedRect.Y = virtualScreen.Bottom - clampedRect.Height;
            }

            return clampedRect;
        }
    }
}
