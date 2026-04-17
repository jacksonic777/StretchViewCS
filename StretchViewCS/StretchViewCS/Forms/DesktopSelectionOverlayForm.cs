using System;
using System.Drawing;
using System.Windows.Forms;
using StretchViewCS.Native;

namespace StretchViewCS.Forms
{
    internal sealed class DesktopSelectionOverlayForm : Form
    {
        private const int WsExTransparent = 0x20;
        private const int WsExLayered = 0x80000;
        private const int WsExToolWindow = 0x80;
        private const int WsExNoActivate = 0x8000000;
        private const uint SwpNoActivate = 0x0010;
        private readonly Color transparentColorKey = Color.Magenta;
        private Rectangle selectionRect = Rectangle.Empty;

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
                cp.ExStyle |= WsExTransparent | WsExLayered | WsExToolWindow | WsExNoActivate;
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
                Invalidate(invalidRect);
            }

            Update();
        }

        public void HideSelection()
        {
            selectionRect = Rectangle.Empty;
            Hide();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (selectionRect.IsEmpty)
            {
                return;
            }

            Rectangle drawRect = ToClientRectangle(selectionRect);

            using (Pen pen = new Pen(Color.OrangeRed, 2))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                e.Graphics.DrawRectangle(pen, drawRect);
            }
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
    }
}
