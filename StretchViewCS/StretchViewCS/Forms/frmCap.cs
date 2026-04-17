using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Text;
using StretchViewCS.Native;
using StretchViewCS.Utils;

namespace StretchViewCS.Forms
{
    /// <summary>
    /// メインフォーム（DelphiのUfrmCapのC#完全版）
    /// </summary>
    public partial class frmCap : Form
    {
        // 定数
        private const string AppTitle = "StretchViewCS";
        private const int WM_HOTKEY = 0x0312;
        private const int RM_ZOOM = 110;
        private const int RM_ROTATE = 220;

        // ホットキーID
        private enum HotKeyID
        {
            MHK_ZOOMUP = 201,
            MHK_ZOOMDOWN = 202,
            MHK_MLeft = 211,
            MHK_MRight = 212,
            MHK_MUp = 213,
            MHK_MDown = 214,
            MHK_FLIPV = 215,
            MHK_FLIPH = 216,
            MHK_FixMode = 217,
            MHK_AtariMode = 218,
            MHK_AtariBlt = 219,
            MHK_FreeRotate = 220,
            MHK_Grid = 221,
            MHK_RRotate = 301,
            MHK_LRotate = 302
        }

        // 変数
        public bool bShowDlgBox = false;
        private float capRate = 1.0f;
        private bool bGrid = false;
        private bool bMdown = false;
        private int disX, disY;
        private int capSizeW, capSizeH;
        private bool bVFlip = false, bHFlip = false;
        private int transX, transY, transW, transH;
        private bool bFixed = false;
        private bool bFixedView = false;
        private int gAngle = 0;
        private int baseX, baseY;
        private int clientBaseX, clientBaseY;
        private IntPtr hTarget = IntPtr.Zero;
        private Win32API.RECT rW;
        private bool bMouseCap = false;
        private bool bStateUpdate = false;
        private Rectangle rcLastDraw;
        private Point pStart, pEnd;
        private Bitmap? bmpBackUp;
        private Bitmap? bmpAtari;
        private Bitmap? bmpDisplay; // 表示用ビットマップ
        private Bitmap? bmpCaptureWork;
        private Bitmap? bmpScaleWork;
        private bool bDrawedRect = false;
        private bool bGraph = false;
        private int iCutPixel = 60;
        private bool bBltAtari = false;
        private bool bAtariMode = false;
        private bool bPenDown = false;
        private Point pCStart;
        private bool bAtariBmpCreate = false;
        private Point pOverPoint;
        private bool bOverDrug = false;
        private bool bVirtualRDownEvent = false;
        private bool bVirtualLDownEvent = false;
        private string strWindowText = "";
        private string strClassName = "";
        private bool bRegistedMHK = false;
        private int iIncAngle = 10;
        private float sgIncCapRate = 0.1f;
        private Color MyAtariColor = Color.Blue;
        private int iRepeatAngle = 0;
        private float sgRepeatCapRate = 0.0f;
        private int iAtariLineWidth = 1;
        private IntPtr hLastTargetWindow = IntPtr.Zero;
        private bool bForFixViewCap = false;
        private bool bSrcNot = false;
        private int repeatMode = 0;
        private bool FFormShowing = false;
        private bool hotkeysEnabled = false;
        private readonly object _bmpDisplayLock = new object();
        /// <summary>直前のキャプチャ領域の左上（画面座標）。選択枠を自フォーム座標に変換するために使用</summary>
        private int lastCaptureLeft, lastCaptureTop;

        // メニュー項目とツールバー項目はDesignerファイルで定義されています
        // ツールバー項目の参照はDesigner.csで自動生成されます

        public frmCap()
        {
            InitializeComponent();

            // ダブルバッファリングを有効化（パフォーマンス向上）
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.DoubleBuffer |
                         ControlStyles.ResizeRedraw, true);

            // 初期値設定
            transX = 0;
            transY = 0;
            transW = 300;
            transH = 300;
        }

        private void frmCap_Load(object? sender, EventArgs e)
        {
            Application.ApplicationExit += OnApplicationExit;

            // 初期化
            gAngle = 0;
            bFixed = false;
            bDrawedRect = false;
            iCutPixel = 60;
            hLastTargetWindow = IntPtr.Zero;
            iIncAngle = 10;
            sgIncCapRate = 0.1f;
            iAtariLineWidth = 1;

            // タイマー設定
            tim.Interval = IniManager.Instance.SamplingRate;

            // メインウィンドウの位置・サイズを復元（BoundsRect を優先）
            this.Bounds = GetValidatedStartupBounds(IniManager.Instance.BoundsRect, this.Bounds);
            UpdateTransFromClientSize();

            // 拡大率などその他の設定
            capRate = IniManager.Instance.CapRate;
            hotkeysEnabled = IniManager.Instance.HotkeysEnabled;

            // 固定表示モード
            if (IniManager.Instance.FixView)
            {
                // 前回終了時の「範囲の指定」位置を復元
                FixView(true, IniManager.Instance.FixViewX, IniManager.Instance.FixViewY);
            }

            // ホットキー登録
            RegisterMyHotkeys();

            // 最前面表示
            SwitchTopMost(true);
            UpdateCaption();

            // タイマー開始
            tim.Enabled = true;

            // ツールバーの初期状態設定
            if (tbFlipH != null) tbFlipH.Checked = bHFlip;
            if (tbFlipV != null) tbFlipV.Checked = bVFlip;
            if (tbGrid != null) tbGrid.Checked = bGraph;
            if (tbAtariMode != null) tbAtariMode.Checked = bAtariMode;
            if (tbAtariVisible != null) tbAtariVisible.Checked = bBltAtari;
            if (tbFixSimu != null) tbFixSimu.Checked = bFixed;
            if (mmFixViewSw != null) mmFixViewSw.Checked = bFixedView;
        }

        private void frmCap_FormClosing(object? sender, FormClosingEventArgs e)
        {
            tim.Enabled = false;
            timRepeat.Enabled = false;
            Application.ApplicationExit -= OnApplicationExit;
            UnregisterMyHotkeys();
            bmpBackUp?.Dispose();
            if (bAtariBmpCreate) bmpAtari?.Dispose();
            bmpDisplay?.Dispose();
            bmpCaptureWork?.Dispose();
            bmpScaleWork?.Dispose();

            // 設定をメモリに反映してから INI に保存
            IniManager.Instance.CapRate = capRate;
            IniManager.Instance.BoundsRect = GetBoundsForPersistence();
            IniManager.Instance.ScaleWidth = transW > 0 ? transW : IniManager.Instance.ScaleWidth;
            IniManager.Instance.ScaleHeight = transH > 0 ? transH : IniManager.Instance.ScaleHeight;
            IniManager.Instance.FixView = bFixedView;
            IniManager.Instance.HotkeysEnabled = hotkeysEnabled;
            if (bFixedView)
            {
                int centerX = baseX + capSizeW / 2;
                int centerY = baseY + capSizeH / 2;
                IniManager.Instance.FixViewX = centerX;
                IniManager.Instance.FixViewY = centerY;
            }
            IniManager.Instance.Write();
        }

        private void frmCap_FormDestroy(object? sender, EventArgs e)
        {
            // FormClosing で保存済みのためここでは Write しない
            IniManager.Instance.CapRate = capRate;
            IniManager.Instance.BoundsRect = GetBoundsForPersistence();
        }

        private void OnApplicationExit(object? sender, EventArgs e)
        {
            UnregisterMyHotkeys();
        }

        private Rectangle GetBoundsForPersistence()
        {
            Rectangle boundsToSave = this.WindowState == FormWindowState.Normal ? this.Bounds : this.RestoreBounds;
            if (boundsToSave.Width < 100 || boundsToSave.Height < 100)
            {
                return this.Bounds;
            }

            return boundsToSave;
        }

        private Rectangle GetValidatedStartupBounds(Rectangle storedBounds, Rectangle defaultBounds)
        {
            if (storedBounds.Width < 100 || storedBounds.Height < 100)
            {
                return defaultBounds;
            }

            if (storedBounds.Left <= -30000 || storedBounds.Top <= -30000)
            {
                return defaultBounds;
            }

            Rectangle virtualScreen = SystemInformation.VirtualScreen;
            Rectangle paddedBounds = new Rectangle(
                storedBounds.Left,
                storedBounds.Top,
                Math.Max(storedBounds.Width, 100),
                Math.Max(storedBounds.Height, 100));

            if (!virtualScreen.IntersectsWith(paddedBounds))
            {
                return defaultBounds;
            }

            return storedBounds;
        }

        private void tim_Tick(object? sender, EventArgs e)
        {
            if (bShowDlgBox) return;
            bStateUpdate = false;
            IntPtr desktopWindow = Win32API.GetDesktopWindow();
            IntPtr desktopDC = IntPtr.Zero;

            try
            {
                // マウス座標取得
                Point mousePos = Cursor.Position;
                int tmpMx = mousePos.X;
                int tmpMy = mousePos.Y;

                // デスクトップDC取得
                desktopDC = Win32API.GetWindowDC(desktopWindow);
                if (desktopDC == IntPtr.Zero)
                {
                    throw new InvalidOperationException("デスクトップ DC の取得に失敗しました。");
                }

                if (capRate == 0) capRate = 1;

                // キャプチャサイズ計算
                capSizeW = (int)(transW / capRate);
                capSizeH = (int)(transH / capRate);
                int halfCapW = capSizeW / 2;
                int halfCapH = capSizeH / 2;

                // 座標調整
                Rectangle desktopRect = Screen.PrimaryScreen.Bounds;
                if (tmpMx <= (desktopRect.Left + halfCapW))
                    tmpMx = desktopRect.Left + halfCapW;
                if (tmpMy <= halfCapH)
                    tmpMy = halfCapH;
                if (tmpMx + halfCapW >= desktopRect.Right)
                    tmpMx = desktopRect.Right - halfCapW;
                if (tmpMy + halfCapH >= desktopRect.Bottom)
                    tmpMy = desktopRect.Bottom - halfCapH;

                int rLeft = tmpMx - halfCapW;
                int rTop = tmpMy - halfCapH;

                // 固定表示モード
                if (bFixedView)
                {
                    rLeft = baseX;
                    rTop = baseY;
                }
                else if (bFixed)
                {
                    // ターゲットウィンドウが無効（閉じられた等）の場合は固定モードを解除し、
                    // GetWindowRect による UI スレッドのブロック・応答なしを防ぐ
                    if (hTarget == IntPtr.Zero || !Win32API.IsWindow(hTarget))
                    {
                        bFixed = false;
                        if (mmFix != null) mmFix.Checked = false;
                        if (tbFixSimu != null) tbFixSimu.Checked = false;
                        if (mmLeft != null) mmLeft.Enabled = false;
                        if (mmRight != null) mmRight.Enabled = false;
                        if (mmUpper != null) mmUpper.Enabled = false;
                        if (mmDowner != null) mmDowner.Enabled = false;
                        if (mmWndInfo != null) mmWndInfo.Enabled = false;
                        UpdateCaption();
                    }
                    else
                    {
                        Win32API.GetWindowRect(hTarget, out rW);
                        rLeft = baseX;
                        rTop = baseY;
                    }
                }

                lastCaptureLeft = rLeft;
                lastCaptureTop = rTop;

                // キャプチャ処理
                CaptureAndDisplay(desktopDC, rLeft, rTop, tmpMx, tmpMy);
            }
            catch
            {
                // エラー処理
            }
            finally
            {
                if (desktopDC != IntPtr.Zero)
                {
                    Win32API.ReleaseDC(desktopWindow, desktopDC);
                }
            }
        }

        private void CaptureAndDisplay(IntPtr desktopDC, int rLeft, int rTop, int mmx, int mmy)
        {
            try
            {
                Bitmap? bmpRotated = null;
                Bitmap? bmpFlipped = null;
                bool bRotated = false;
                bool bFlipped = false;

                try
                {
                    // デバイスのビット深度を取得
                    PixelFormat pixelFormat = GetDesktopPixelFormat(desktopDC);
                    Bitmap captureBmp = GetOrCreateWorkBitmap(ref bmpCaptureWork, capSizeW, capSizeH, pixelFormat);
                    Bitmap scaledBmp = GetOrCreateWorkBitmap(ref bmpScaleWork, transW, transH, PixelFormat.Format32bppArgb);
                    Bitmap sourceBmp = captureBmp;

                    // デスクトップからキャプチャ
                    using (Graphics g = Graphics.FromImage(captureBmp))
                    {
                        IntPtr hdc = g.GetHdc();
                        try
                        {
                            uint rop = bSrcNot ? Win32API.NOTSRCCOPY : Win32API.SRCCOPY;
                            Win32API.BitBlt(hdc, 0, 0, capSizeW, capSizeH,
                                desktopDC, rLeft, rTop, rop);
                        }
                        finally
                        {
                            g.ReleaseHdc(hdc);
                        }
                    }

                    // 当たり判定レイヤーの合成
                    if (bBltAtari && bmpAtari != null)
                    {
                        using (Graphics g = Graphics.FromImage(captureBmp))
                        {
                            g.DrawImage(bmpAtari, 0, 0, capSizeW, capSizeH);
                        }
                    }

                    // 拡大
                    using (Graphics g = Graphics.FromImage(scaledBmp))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        g.DrawImage(captureBmp, 0, 0, transW, transH);
                    }
                    sourceBmp = scaledBmp;

                    // 回転・反転処理
                    if (bHFlip || bVFlip || gAngle != 0)
                    {
                        if ((bHFlip || bVFlip) && gAngle == 0)
                        {
                            // 反転のみ
                            bmpFlipped = XRotateBitmap.RotateBitmapX(sourceBmp, gAngle, (bHFlip || bVFlip), false);
                            bFlipped = true;
                            bmpRotated = bmpFlipped;
                        }
                        else if (gAngle != 0)
                        {
                            // 回転
                            if (bVFlip || bHFlip)
                            {
                                bmpFlipped = XRotateBitmap.RotateBitmapX(sourceBmp, 0, (bHFlip || bVFlip), false);
                                bFlipped = true;
                                bmpRotated = XRotateBitmap.RotateBitmapX(bmpFlipped, gAngle, false, false);
                            }
                            else
                            {
                                bmpRotated = XRotateBitmap.RotateBitmapX(sourceBmp, gAngle, false, false);
                            }
                            bRotated = true;
                        }
                    }

                    // オプション描画（グリッド、クロスなど）
                    Bitmap displayBmp = bmpRotated ?? sourceBmp;
                    DrawOptions(displayBmp, mmx, mmy, rLeft, rTop);

                    // 表示用ビットマップを更新（フォーム本体のロックは避けデッドロックを防止）
                    UpdateDisplayBitmap(displayBmp);

                    // 再描画を要求
                    this.Invalidate(new Rectangle(transX, transY, transW, transH));
                }
                finally
                {
                    if (bRotated) bmpRotated?.Dispose();
                    if (bFlipped) bmpFlipped?.Dispose();
                }
            }
            catch
            {
                // エラー処理
            }
        }

        private void DrawOptions(Bitmap bitmap, int mmx, int mmy, int rLeft, int rTop)
        {
            if (!(bGraph || bGrid) && (bFixed || bAtariMode)) return;

            try
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                    // クロス表示
                    if (bGrid)
                    {
                        int dx = transW / 2;
                        int dy = transH / 2;
                        g.DrawLine(pen, dx, 0, dx, transH);
                        g.DrawLine(pen, 0, dy, transW, dy);
                    }

                    // グリッド表示
                    if (bGraph)
                    {
                        int iWnum = transW / iCutPixel;
                        int iHnum = transH / iCutPixel;
                        for (int idx = 1; idx <= iWnum; idx++)
                        {
                            int dx = idx * iCutPixel;
                            g.DrawLine(pen, dx, 0, dx, transH);
                        }
                        for (int idx = 1; idx <= iHnum; idx++)
                        {
                            int dy = idx * iCutPixel;
                            g.DrawLine(pen, 0, dy, transW, dy);
                        }
                    }

                    // カーソル位置表示はPaintイベントで処理
                }
            }
            catch
            {
                // エラー処理
            }
        }

        private void timRepeat_Tick(object? sender, EventArgs e)
        {
            if (repeatMode == RM_ZOOM)
            {
                ChgCapRate(capRate + sgRepeatCapRate);
            }
            else if (repeatMode == RM_ROTATE)
            {
                gAngle += iRepeatAngle;
                RotateAngle(iRepeatAngle);
            }
        }

        // ============================================================
        // ホットキー処理
        // ============================================================

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                HandleHotKey(m.WParam.ToInt32());
                return;
            }
            base.WndProc(ref m);
        }

        private void HandleHotKey(int hotKeyId)
        {
            switch ((HotKeyID)hotKeyId)
            {
                case HotKeyID.MHK_ZOOMUP:
                    ChgCapRate(capRate + sgIncCapRate);
                    break;
                case HotKeyID.MHK_ZOOMDOWN:
                    ChgCapRate(capRate - sgIncCapRate);
                    break;
                case HotKeyID.MHK_LRotate:
                    gAngle += iIncAngle;
                    RotateAngle(iIncAngle);
                    break;
                case HotKeyID.MHK_RRotate:
                    gAngle -= iIncAngle;
                    RotateAngle(-iIncAngle);
                    break;
                case HotKeyID.MHK_FLIPH:
                    FlipHV(1);
                    break;
                case HotKeyID.MHK_FLIPV:
                    FlipHV(2);
                    break;
                case HotKeyID.MHK_AtariBlt:
                    if (mmBltAtari != null)
                    {
                        mmBltAtari.Checked = !bBltAtari;
                        MmBltAtariClick(this, EventArgs.Empty);
                    }
                    break;
                case HotKeyID.MHK_AtariMode:
                    if (mmAtariMode != null)
                    {
                        mmAtariMode.Checked = !bAtariMode;
                        MmAtariModeClick(this, EventArgs.Empty);
                    }
                    break;
                case HotKeyID.MHK_FixMode:
                    FixModeByKey();
                    break;
                case HotKeyID.MHK_FreeRotate:
                    MFlexFlipClick(this, EventArgs.Empty);
                    break;
                case HotKeyID.MHK_Grid:
                    bGraph = !bGraph;
                    if (tbGrid != null) tbGrid.Checked = bGraph;
                    break;
                case HotKeyID.MHK_MLeft:
                    if (bFixed) MoveRange("left");
                    break;
                case HotKeyID.MHK_MRight:
                    if (bFixed) MoveRange("right");
                    break;
                case HotKeyID.MHK_MUp:
                    if (bFixed) MoveRange("up");
                    break;
                case HotKeyID.MHK_MDown:
                    if (bFixed) MoveRange("down");
                    break;
            }
        }

        private void RegisterMyHotkeys()
        {
            if (!hotkeysEnabled) return;
            if (bRegistedMHK) return;
            if (!this.IsHandleCreated) return;

            try
            {
                // Delphi 版に合わせたホットキー登録（登録失敗時は無視して続行）
                RegisterHotKeyOne((int)HotKeyID.MHK_ZOOMUP, Win32API.MOD_CONTROL, 0x41);      // Ctrl+A
                RegisterHotKeyOne((int)HotKeyID.MHK_ZOOMDOWN, Win32API.MOD_CONTROL, 0x53);    // Ctrl+S
                RegisterHotKeyOne((int)HotKeyID.MHK_FLIPH, Win32API.MOD_CONTROL, 0x44);       // Ctrl+D
                RegisterHotKeyOne((int)HotKeyID.MHK_FLIPV, Win32API.MOD_CONTROL, 0x46);       // Ctrl+F
                RegisterHotKeyOne((int)HotKeyID.MHK_AtariMode, Win32API.MOD_CONTROL, Win32API.VK_F2);
                RegisterHotKeyOne((int)HotKeyID.MHK_AtariBlt, Win32API.MOD_CONTROL, Win32API.VK_F3);
                RegisterHotKeyOne((int)HotKeyID.MHK_FixMode, Win32API.MOD_CONTROL, 0x45);     // Ctrl+E
                RegisterHotKeyOne((int)HotKeyID.MHK_MLeft, Win32API.MOD_CONTROL, Win32API.VK_LEFT);
                RegisterHotKeyOne((int)HotKeyID.MHK_MRight, Win32API.MOD_CONTROL, Win32API.VK_RIGHT);
                RegisterHotKeyOne((int)HotKeyID.MHK_MUp, Win32API.MOD_CONTROL, Win32API.VK_UP);
                RegisterHotKeyOne((int)HotKeyID.MHK_MDown, Win32API.MOD_CONTROL, Win32API.VK_DOWN);
                RegisterHotKeyOne((int)HotKeyID.MHK_FreeRotate, Win32API.MOD_CONTROL, 0x54);  // Ctrl+T
                RegisterHotKeyOne((int)HotKeyID.MHK_Grid, Win32API.MOD_CONTROL, 0x47);        // Ctrl+G
                RegisterHotKeyOne((int)HotKeyID.MHK_LRotate, Win32API.MOD_SHIFT, Win32API.VK_LEFT);
                RegisterHotKeyOne((int)HotKeyID.MHK_RRotate, Win32API.MOD_SHIFT, Win32API.VK_RIGHT);

                bRegistedMHK = true;
            }
            catch
            {
                // 一部でも登録できていれば true のまま
            }
        }

        private void RegisterHotKeyOne(int id, uint mod, uint vk)
        {
            try
            {
                Win32API.RegisterHotKey(this.Handle, id, mod, vk);
            }
            catch
            {
                // 競合等で失敗しても続行
            }
        }

        private void UnregisterMyHotkeys()
        {
            if (!bRegistedMHK) return;
            if (!this.IsHandleCreated) return;

            try
            {
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_ZOOMUP);
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_ZOOMDOWN);
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_FLIPH);
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_FLIPV);
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_AtariMode);
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_AtariBlt);
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_FixMode);
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_MLeft);
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_MRight);
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_MUp);
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_MDown);
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_FreeRotate);
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_Grid);
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_LRotate);
                Win32API.UnregisterHotKey(this.Handle, (int)HotKeyID.MHK_RRotate);

                bRegistedMHK = false;
            }
            catch
            {
                bRegistedMHK = false;
            }
        }

        // ============================================================
        // マウスイベント処理
        // ============================================================

        private void frmCap_MouseDown(object? sender, MouseEventArgs e)
        {
            if (bAtariMode)
            {
                int x = e.X - transX;
                int y = e.Y - transY;
                CalculateXY(ref x, ref y);
                bPenDown = true;
                pCStart = new Point(x, y);
            }
            else if (bMouseCap)
            {
                pStart = Cursor.Position;
            }
            else
            {
                if ((e.Button == MouseButtons.Left && bVirtualLDownEvent) ||
                    (e.Button == MouseButtons.Right && bVirtualRDownEvent))
                    return;

                if (bFixed)
                {
                    // ターゲットを前面に出してからメッセージを送る（多くのアプリは前面でないとマウスメッセージを処理しない）
                    if (hTarget != IntPtr.Zero && Win32API.IsWindow(hTarget))
                        SetAbsoluteForegroundWindow(hTarget);

                    int iWM = (e.Button == MouseButtons.Left) ? Win32API.WM_LBUTTONDOWN : Win32API.WM_RBUTTONDOWN;
                    TranslateXY(e.X, e.Y, e.Button, Control.ModifierKeys, iWM);
                }
                else
                {
                    bMdown = true;
                    disX = Cursor.Position.X - this.Left;
                    disY = Cursor.Position.Y - this.Top;
                }
            }
        }

        private void frmCap_MouseMove(object? sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;

            if (bAtariMode && bPenDown)
            {
                int x = e.X - transX;
                int y = e.Y - transY;
                CalculateXY(ref x, ref y);

                if (bmpAtari != null)
                {
                    using (Graphics g = Graphics.FromImage(bmpAtari))
                    {
                        if (e.Button == MouseButtons.Left && (Control.ModifierKeys & Keys.Control) == 0)
                        {
                            using (Pen pen = new Pen(MyAtariColor, iAtariLineWidth))
                            {
                                g.DrawLine(pen, pCStart.X, pCStart.Y, x, y);
                            }
                        }
                        else if ((e.Button == MouseButtons.Left && (Control.ModifierKeys & Keys.Control) != 0) ||
                                 e.Button == MouseButtons.Right)
                        {
                            using (Pen pen = new Pen(Color.White, Math.Max(5, iAtariLineWidth * 4)))
                            {
                                g.DrawLine(pen, pCStart.X, pCStart.Y, x, y);
                            }
                        }
                    }
                    pCStart = new Point(x, y);
                    this.Invalidate();
                }
            }
            else if (bMouseCap)
            {
                // マウスキャプチャモードの処理
                HandleMouseCapture(e);
            }
            else if (bFixed)
            {
                TranslateXY(e.X, e.Y, e.Button, Control.ModifierKeys, Win32API.WM_MOUSEMOVE);
            }
            else
            {
                this.Cursor = Cursors.Hand;
                if (bMdown)
                {
                    this.Left = Cursor.Position.X - disX;
                    this.Top = Cursor.Position.Y - disY;
                }
            }
        }

        private void HandleMouseCapture(MouseEventArgs e)
        {
            try
            {
                if (!bForFixViewCap)
                {
                    // 操作投影モード：デスクトップには描画せず、カーソル下のウィンドウだけ記録して
                    // 自フォームの Paint で選択枠を描画する（デスクトップの描画乱れを防ぐ）
                    IntPtr hTmp = GetTargetWindow(Cursor.Position);
                    hLastTargetWindow = hTmp != IntPtr.Zero ? hTmp : IntPtr.Zero;
                    this.Invalidate();
                    return;
                }

                IntPtr desktopDC = Win32API.GetWindowDC(Win32API.GetDesktopWindow());
                try
                {
                    using (Graphics cvDesk = Graphics.FromHdc(desktopDC))
                    {
                        int mmx = Cursor.Position.X;
                        int mmy = Cursor.Position.Y;
                        int halfCapW = capSizeW / 2;
                        int halfCapH = capSizeH / 2;

                        // 固定表示範囲の選択（従来どおりデスクトップに枠描画）
                        {
                            Rectangle desktopRect = Screen.PrimaryScreen.Bounds;
                            if (mmx < desktopRect.Left + halfCapW)
                                Win32API.SetCursorPos(desktopRect.Left + halfCapW, mmy);
                            if (mmy < halfCapH)
                                Win32API.SetCursorPos(mmx, desktopRect.Top + halfCapH);
                            if (mmx > desktopRect.Right - halfCapW)
                                Win32API.SetCursorPos(desktopRect.Right - halfCapW, mmy);
                            if (mmy > desktopRect.Bottom - halfCapH)
                                Win32API.SetCursorPos(mmx, desktopRect.Bottom - halfCapH);

                            mmx = Cursor.Position.X;
                            mmy = Cursor.Position.Y;

                            // 前の矩形を復元
                            if (bDrawedRect && bmpBackUp != null)
                            {
                                RestoreDesktopBackup(cvDesk);
                            }

                            // 新しい矩形を設定
                            rcLastDraw = new Rectangle(mmx - halfCapW, mmy - halfCapH,
                                capSizeW, capSizeH);

                            // バックアップを取得
                            if (bmpBackUp != null)
                            {
                                bmpBackUp.Dispose();
                            }
                            bmpBackUp = new Bitmap(capSizeW + 4, capSizeH + 4);
                            CaptureDesktopBackup();

                            // 矩形を描画
                            using (Pen pen = new Pen(Color.Black, 1))
                            {
                                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                                cvDesk.DrawRectangle(pen, rcLastDraw.Left, rcLastDraw.Top,
                                    rcLastDraw.Width, rcLastDraw.Height);
                            }
                        }

                        bDrawedRect = true;
                    }
                }
                finally
                {
                    Win32API.ReleaseDC(Win32API.GetDesktopWindow(), desktopDC);
                }
            }
            catch
            {
                // エラー処理
            }
        }

        private void frmCap_MouseUp(object? sender, MouseEventArgs e)
        {
            if (bAtariMode)
            {
                bPenDown = false;
            }
            else if (bMouseCap)
            {
                // マウスキャプチャ終了処理
                if (bForFixViewCap)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        // 最後に描画した矩形を元に戻す
                        try
                        {
                            if (bDrawedRect && bmpBackUp != null)
                            {
                                IntPtr desktopDC = Win32API.GetWindowDC(Win32API.GetDesktopWindow());
                                try
                                {
                                    using (Graphics desktopGraphics = Graphics.FromHdc(desktopDC))
                                    {
                                        RestoreDesktopBackup(desktopGraphics);
                                    }
                                }
                                finally
                                {
                                    Win32API.ReleaseDC(Win32API.GetDesktopWindow(), desktopDC);
                                }
                            }
                        }
                        catch
                        {
                            // 復元失敗時はそのまま続行
                        }

                        bForFixViewCap = false;
                        FixView(true, Cursor.Position.X, Cursor.Position.Y);
                        Win32API.mouse_event(Win32API.MOUSEEVENTF_RIGHTUP, 0, 0, 0, Win32API.GetMessageExtraInfo());
                        Win32API.ReleaseCapture();
                        bMouseCap = false;
                    }
                    else
                    {
                        Win32API.ReleaseCapture();
                        bMouseCap = false;
                    }
                }
                else
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (tbFixSimu != null) tbFixSimu.Checked = true;
                        if (mmFix != null) mmFix.Checked = true;
                        FixMode(true);
                        Win32API.mouse_event(Win32API.MOUSEEVENTF_RIGHTUP, 0, 0, 0, Win32API.GetMessageExtraInfo());
                        pEnd = Cursor.Position;
                        Win32API.ReleaseCapture();
                        bMouseCap = false;
                    }
                    else
                    {
                        if (tbFixSimu != null) tbFixSimu.Checked = false;
                        if (mmFix != null) mmFix.Checked = false;
                        Win32API.ReleaseCapture();
                        bMouseCap = false;
                    }
                }
                UpdateCaption();
            }
            else
            {
                if ((e.Button == MouseButtons.Left && bVirtualLDownEvent) ||
                    (e.Button == MouseButtons.Right && bVirtualRDownEvent))
                    return;

                if (bFixed)
                {
                    int iWM = (e.Button == MouseButtons.Left) ? Win32API.WM_LBUTTONUP : Win32API.WM_RBUTTONUP;
                    TranslateXY(e.X, e.Y, e.Button, Control.ModifierKeys, iWM);
                }
                UpdateCaption();
                bMdown = false;
            }
        }

        // ============================================================
        // 座標変換・計算処理
        // ============================================================

        private void CalculateXY(ref int x, ref int y)
        {
            // 任意角度回転
            if (gAngle != 0)
            {
                x -= transW / 2;
                y -= transH / 2;
                int tmpx = x;
                int tmpy = y;

                double rTheta = (gAngle / 10.0) * Math.PI / 180.0;
                x = (int)(tmpx * Math.Cos(rTheta) - tmpy * Math.Sin(rTheta));
                y = (int)(tmpx * Math.Sin(rTheta) + tmpy * Math.Cos(rTheta));
                x += transW / 2;
                y += transH / 2;
            }

            // 左右反転
            if (bHFlip)
            {
                x = (transW / 2) - (x - (transW / 2));
            }

            // 上下反転
            if (bVFlip)
            {
                y = (transH / 2) - (y - (transH / 2));
            }
        }

        private void TranslateXY(int X, int Y, MouseButtons button, Keys modifiers, int iWM)
        {
            X += transX;
            Y -= transY;

            int iShift = 0;
            if ((button == MouseButtons.Left) && !bVirtualLDownEvent)
                iShift = Win32API.MK_LBUTTON;
            if ((button == MouseButtons.Right) && !bVirtualRDownEvent)
                iShift = Win32API.MK_RBUTTON;
            if (button == MouseButtons.Middle)
                iShift = Win32API.MK_MBUTTON;
            if ((modifiers & Keys.Control) != 0)
                iShift |= Win32API.MK_CONTROL;
            if ((modifiers & Keys.Shift) != 0)
                iShift |= Win32API.MK_SHIFT;

            // 仮想マウスイベント処理
            if (!bVirtualLDownEvent && !bVirtualRDownEvent && iWM == Win32API.WM_LBUTTONDOWN)
            {
                Win32API.mouse_event(Win32API.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, Win32API.GetMessageExtraInfo());
                bVirtualRDownEvent = true;
            }
            else if (!bVirtualLDownEvent && bVirtualRDownEvent && iWM == Win32API.WM_LBUTTONUP)
            {
                Win32API.mouse_event(Win32API.MOUSEEVENTF_RIGHTUP, 0, 0, 0, Win32API.GetMessageExtraInfo());
                bVirtualRDownEvent = false;
            }
            else if (!bVirtualRDownEvent && !bVirtualLDownEvent && iWM == Win32API.WM_RBUTTONDOWN)
            {
                Win32API.mouse_event(Win32API.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, Win32API.GetMessageExtraInfo());
                bVirtualLDownEvent = true;
            }
            else if (!bVirtualRDownEvent && bVirtualLDownEvent && iWM == Win32API.WM_RBUTTONUP)
            {
                Win32API.mouse_event(Win32API.MOUSEEVENTF_LEFTUP, 0, 0, 0, Win32API.GetMessageExtraInfo());
                bVirtualLDownEvent = false;
            }

            // 座標計算
            CalculateXY(ref X, ref Y);

            // 補正
            Y -= (int)Math.Floor(capRate / 2);
            X -= (int)Math.Floor(capRate / 2);

            // 特殊クラス対応
            if (strClassName == "MSAWT_Comp_Class")
            {
                X += (int)(1 * capRate);
            }

            // 表示座標 → 画面座標 → ターゲットのクライアント座標に変換
            // （ScreenToClient によりウィンドウ移動後も正しい座標になる）
            if (hTarget == IntPtr.Zero || !Win32API.IsWindow(hTarget))
                return;

            Point screenPt = new Point(
                baseX + (int)(X / capRate),
                baseY + (int)(Y / capRate));
            Win32API.ScreenToClient(hTarget, ref screenPt);

            IntPtr lParam = Win32API.MakeLParam(screenPt.X, screenPt.Y);
            // 操作投影モード中にターゲットウィンドウが落ちてもこちらが巻き込まれないよう、
            // 同期 SendMessage ではなく PostMessage を使用する
            Win32API.PostMessage(hTarget, (uint)iWM, (IntPtr)iShift, lParam);
        }

        // ============================================================
        // ウィンドウ操作
        // ============================================================

        private IntPtr GetTargetWindow(Point pt)
        {
            IntPtr hRes = Win32API.WindowFromPoint(pt);
            if (hRes == IntPtr.Zero) return IntPtr.Zero;

            Point clientPt = pt;
            Win32API.ScreenToClient(hRes, ref clientPt);
            IntPtr hCWnd = Win32API.ChildWindowFromPoint(hRes, clientPt);
            if (hCWnd != IntPtr.Zero)
                hRes = hCWnd;

            StringBuilder bufWT = new StringBuilder(71);
            StringBuilder bufCN = new StringBuilder(71);
            Win32API.GetWindowText(hRes, bufWT, 71);
            Win32API.GetClassName(hRes, bufCN, 71);

            strClassName = bufCN.ToString();
            strWindowText = bufWT.ToString();

            if (strClassName == "TfrmCap" || strClassName == "toolbarMain")
                return IntPtr.Zero;

            Win32API.GetWindowRect(hRes, out rW);
            return hRes;
        }

        private void SetAbsoluteForegroundWindow(IntPtr hwnd)
        {
            uint nForegroundID = Win32API.GetWindowThreadProcessId(Win32API.GetForegroundWindow(), IntPtr.Zero);
            uint nTargetID = Win32API.GetWindowThreadProcessId(hwnd, IntPtr.Zero);

            Win32API.AttachThreadInput(nTargetID, nForegroundID, true);

            uint sp_time = 0;
            IntPtr pTime = Marshal.AllocHGlobal(sizeof(uint));
            Win32API.SystemParametersInfo(Win32API.SPI_GETFOREGROUNDLOCKTIMEOUT, 0, pTime, 0);
            sp_time = (uint)Marshal.ReadInt32(pTime);

            IntPtr zero = IntPtr.Zero;
            Win32API.SystemParametersInfo(Win32API.SPI_SETFOREGROUNDLOCKTIMEOUT, 0, zero, 0);

            Win32API.SetForegroundWindow(hwnd);

            IntPtr pTime2 = Marshal.AllocHGlobal(sizeof(uint));
            Marshal.WriteInt32(pTime2, (int)sp_time);
            Win32API.SystemParametersInfo(Win32API.SPI_SETFOREGROUNDLOCKTIMEOUT, 0, pTime2, 0);
            Marshal.FreeHGlobal(pTime);
            Marshal.FreeHGlobal(pTime2);

            Win32API.AttachThreadInput(nTargetID, nForegroundID, false);
        }

        // ============================================================
        // 固定モード処理
        // ============================================================

        private bool FixMode(bool flg)
        {
            if (bAtariMode) bAtariMode = false;

            strClassName = "";
            strWindowText = "";

            bFixed = flg;
            if (mmFix != null) mmFix.Checked = bFixed;

            if (bFixed)
            {
                try
                {
                    Point pt = Cursor.Position;
                    Win32API.GetCursorPos(out pt);
                    hTarget = Win32API.WindowFromPoint(pt);

                    if (hTarget == IntPtr.Zero)
                    {
                        if (mmFix != null) mmFix.Checked = false;
                        bFixed = false;
                        bShowDlgBox = true;
                        MessageBox.Show("対象が取得できません " + hTarget, "対象取得");
                        bShowDlgBox = false;
                        return false;
                    }

                    Win32API.ScreenToClient(hTarget, ref pt);
                    IntPtr hCWnd = Win32API.ChildWindowFromPoint(hTarget, pt);
                    if (hCWnd != IntPtr.Zero)
                        hTarget = hCWnd;

                    StringBuilder bufWT = new StringBuilder(71);
                    StringBuilder bufCN = new StringBuilder(71);
                    Win32API.GetWindowText(hTarget, bufWT, 71);
                    Win32API.GetClassName(hTarget, bufCN, 71);

                    strClassName = bufCN.ToString();
                    strWindowText = bufWT.ToString();

                    if (strClassName == "TfrmCap")
                    {
                        if (mmFix != null) mmFix.Checked = false;
                        bFixed = false;
                        bShowDlgBox = true;
                        MessageBox.Show("自分自身を選択しています", "対象取得");
                        bShowDlgBox = false;
                        return false;
                    }

                    // ウィンドウ情報をメニューに表示
                    if (mmClassName != null)
                        mmClassName.Text = "<クラス名>\"" + strClassName + "\"";
                    if (mmWindowText != null)
                        mmWindowText.Text = "<ウィンドウテキスト>\"" + strWindowText + "\"";

                    Win32API.GetWindowRect(hTarget, out rW);
                    baseX = Cursor.Position.X - capSizeW / 2;
                    baseY = Cursor.Position.Y - capSizeH / 2;

                    Rectangle desktopRect = Screen.PrimaryScreen.Bounds;
                    if (baseX < desktopRect.Left) baseX = 0;
                    if (baseY < desktopRect.Top) baseY = 0;
                    if (baseX + capSizeW > desktopRect.Right)
                        baseX = desktopRect.Right - capSizeW;
                    if (baseY + capSizeH > desktopRect.Bottom)
                        baseY = desktopRect.Bottom - capSizeH;

                    clientBaseX = baseX - rW.Left;
                    clientBaseY = baseY - rW.Top;
                }
                catch
                {
                    // エラー処理
                }
            }

            UpdateCaption();
            return bFixed;
        }

        private bool FixModeByKey()
        {
            if (mmFix != null)
            {
                mmFix.Checked = !mmFix.Checked;
                if (mmFix.Checked)
                {
                    if (tbFixSimu != null) tbFixSimu.Checked = true;
                    return FixMode(true);
                }
                else
                {
                    FixMode(false);
                    if (tbFixSimu != null) tbFixSimu.Checked = false;
                    return false;
                }
            }
            return false;
        }

        private void StartMyCapture()
        {
            if (bAtariMode) bAtariMode = false;
            bMouseCap = true;
            UpdateCaption();
            Win32API.SetCapture(this.Handle);
            Win32API.mouse_event(Win32API.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, Win32API.GetMessageExtraInfo());
        }

        // ============================================================
        // 範囲移動処理
        // ============================================================

        private void MoveRange(string direction)
        {
            if (direction == "left")
                baseX -= 10;
            else if (direction == "right")
                baseX += 10;
            else if (direction == "up")
                baseY -= 10;
            else if (direction == "down")
                baseY += 10;

            Rectangle desktopRect = Screen.PrimaryScreen.Bounds;
            if (baseX < desktopRect.Left) baseX = desktopRect.Left;
            if (baseY < desktopRect.Top) baseY = desktopRect.Top;
            if (baseX + capSizeW > desktopRect.Right)
                baseX = desktopRect.Right - capSizeW;
            if (baseY + capSizeH > desktopRect.Bottom)
                baseY = desktopRect.Bottom - capSizeH;

            if (bFixed)
            {
                clientBaseX = baseX - rW.Left;
                clientBaseY = baseY - rW.Top;
            }

            if (bFixedView)
            {
                IniManager.Instance.FixViewX = baseX + capSizeW / 2;
                IniManager.Instance.FixViewY = baseY + capSizeH / 2;
            }

            this.Invalidate();
        }

        // ============================================================
        // ズーム率変更処理
        // ============================================================

        private void ChgCapRate(float iRate)
        {
            if (iRate <= 1.0f && iRate >= 0.95f) iRate = 1.0f;
            if (iRate >= 1.0f && iRate <= 16.0f)
            {
                if (bFixed)
                {
                    baseX += capSizeW / 2;
                    baseY += capSizeH / 2;
                    capRate = iRate;
                    capSizeW = (int)(transW / capRate);
                    capSizeH = (int)(transH / capRate);
                    baseX -= capSizeW / 2;
                    baseY -= capSizeH / 2;
                    clientBaseX = baseX - rW.Left;
                    clientBaseY = baseY - rW.Top;
                }
                else
                {
                    capRate = iRate;
                }
                bStateUpdate = true;
                UpdateCaption();
            }
        }

        // ============================================================
        // 回転処理
        // ============================================================

        private void RotateAngle(int iAngle)
        {
            if (mmFlipV != null) mmFlipV.Checked = false;
            if (mmFlipH != null) mmFlipH.Checked = false;
            bHFlip = false;
            bVFlip = false;
            if (mmFlexRotate != null) mmFlexRotate.Checked = true;
            UpdateCaption();
            if (bVFlip && mmFlipV != null) MmFlipVClick(this, EventArgs.Empty);
            if (bHFlip && mmFlipH != null) MmFlipHClick(this, EventArgs.Empty);
        }

        // ============================================================
        // 反転処理
        // ============================================================

        private bool FlipHV(int iHV)
        {
            if (iHV == 1) // 左右
            {
                bHFlip = !bHFlip;
                if (mmFlipH != null) mmFlipH.Checked = bHFlip;
                if (tbFlipH != null) tbFlipH.Checked = bHFlip;

                if (bHFlip)
                {
                    if (mmFlipV != null) mmFlipV.Checked = false;
                    if (tbFlipV != null) tbFlipV.Checked = false;
                    bVFlip = false;
                }
            }
            else // 上下
            {
                bVFlip = !bVFlip;
                if (mmFlipV != null) mmFlipV.Checked = bVFlip;
                if (tbFlipV != null) tbFlipV.Checked = bVFlip;

                if (bVFlip)
                {
                    if (mmFlipH != null) mmFlipH.Checked = false;
                    if (tbFlipH != null) tbFlipH.Checked = false;
                    bHFlip = false;
                }
            }

            bStateUpdate = true;
            UpdateCaption();
            return true;
        }

        // ============================================================
        // ウィンドウサイズ変更
        // ============================================================

        /// <summary>
        /// 現在のフォームの ClientSize から表示領域（transX, transY, transW, transH）と
        /// バックアップ用ビットマップを更新する。Bounds 復元後に呼ぶ。
        /// </summary>
        private void UpdateTransFromClientSize()
        {
            int cw = this.ClientSize.Width;
            int ch = this.ClientSize.Height;
            int menuHeight = mMainMenu?.Height ?? 24;
            int controlBarHeight = controlBar1 != null ? controlBar1.Height : 48;
            int statusBarHeight = sbMain?.Height ?? 22;
            int infoBarHeight = sbMain != null ? sbMain.Height : 0;

            transX = 0;
            transY = controlBarHeight + menuHeight;
            transW = cw;
            transH = ch - (statusBarHeight + infoBarHeight + transY);

            if (transW < 10) transW = 10;
            if (transH < 10) transH = 10;

            if (bmpBackUp != null)
            {
                bmpBackUp.Dispose();
            }
            bmpBackUp = new Bitmap(Math.Max(transW, 1), Math.Max(transH, 1));

            bStateUpdate = true;
        }

        private void ChgWindowSize(int iSizeW, int iSizeH)
        {
            int maxH = Screen.PrimaryScreen.Bounds.Height;
            int maxW = Screen.PrimaryScreen.Bounds.Width;

            try
            {
                if (iSizeW >= 10 && iSizeW <= maxW && iSizeH >= 10 && iSizeH <= maxH)
                {
                    this.ClientSize = new Size(iSizeW, iSizeH);
                    transX = 0;
                    int menuHeight = mMainMenu.Height;
                    int controlBarHeight = controlBar1 != null ? controlBar1.Height : 48;
                    transY = controlBarHeight + menuHeight; // ControlBar1.Height + Menu.Height
                    transW = iSizeW;
                    int statusBarHeight = sbMain.Height;
            int infoBarHeight = sbMain != null ? sbMain.Height : 0;
                    transH = iSizeH - (statusBarHeight + infoBarHeight + transY);

                    // 最小サイズチェック
                    if (transW < 10) transW = 10;
                    if (transH < 10) transH = 10;

                    if (bAtariBmpCreate)
                    {
                        if (bmpAtari != null)
                        {
                            bmpAtari.Dispose();
                        }
                        bmpAtari = new Bitmap(transW, transH);
                        bmpAtari.MakeTransparent();
                    }

                    // バックアップビットマップも更新
                    if (bmpBackUp != null)
                    {
                        bmpBackUp.Dispose();
                    }
                    bmpBackUp = new Bitmap(transW, transH);
                }
                else
                {
                    MessageBox.Show("値が大きすぎるか小さすぎます", "メッセージ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("値が大きすぎるか小さすぎます: " + ex.Message, "メッセージ");
            }
            bStateUpdate = true;
        }

        private FormWindowState lastWindowState = FormWindowState.Normal;

        private void frmCap_Resize(object? sender, EventArgs e)
        {
            // ウィンドウ状態の変更を検出
            if (this.WindowState != lastWindowState)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    MyOnMinimize(this, EventArgs.Empty);
                }
                else if (this.WindowState == FormWindowState.Normal && lastWindowState == FormWindowState.Minimized)
                {
                    MyOnRestore(this, EventArgs.Empty);
                }
                lastWindowState = this.WindowState;
            }

            if (this.WindowState == FormWindowState.Normal)
            {
                RegisterMyHotkeys();
                ChgWindowSize(this.ClientSize.Width, this.ClientSize.Height);
            }
        }

        // ============================================================
        // 最前面表示切替
        // ============================================================

        public void SwitchTopMost(bool onoff)
        {
            if (onoff)
            {
                Win32API.SetWindowPos(this.Handle, (IntPtr)Win32API.HWND_TOPMOST,
                    0, 0, 0, 0, Win32API.SWP_NOMOVE | Win32API.SWP_NOSIZE);
                if (mmTopMost != null) mmTopMost.Checked = true;
            }
            else
            {
                Win32API.SetWindowPos(this.Handle, (IntPtr)Win32API.HWND_NOTOPMOST,
                    0, 0, 0, 0, Win32API.SWP_NOMOVE | Win32API.SWP_NOSIZE);
                if (mmTopMost != null) mmTopMost.Checked = false;
            }
        }

        public void SetHotkeysEnabled(bool enabled)
        {
            hotkeysEnabled = enabled;
            IniManager.Instance.HotkeysEnabled = enabled;

            if (hotkeysEnabled)
            {
                RegisterMyHotkeys();
            }
            else
            {
                UnregisterMyHotkeys();
            }
        }

        // ============================================================
        // 固定表示モード
        // ============================================================

        public bool FixView(bool onoff, int x, int y)
        {
            if (onoff)
            {
                bFixedView = true;
                capSizeW = (int)(transW / capRate);
                capSizeH = (int)(transH / capRate);

                baseX = x - capSizeW / 2;
                baseY = y - capSizeH / 2;

                Rectangle desktopRect = Screen.PrimaryScreen.Bounds;
                if (baseX < desktopRect.Left) baseX = 0;
                if (baseY < desktopRect.Top) baseY = 0;
                if (baseX + capSizeW > desktopRect.Right)
                    baseX = desktopRect.Right - capSizeW;
                if (baseY + capSizeH > desktopRect.Bottom)
                    baseY = desktopRect.Bottom - capSizeH;

                IniManager.Instance.FixViewX = x;
                IniManager.Instance.FixViewY = y;
            }
            else
            {
                bFixedView = false;
            }

            IniManager.Instance.FixView = bFixedView;
            return bFixedView;
        }

        private void ApplyFixViewState(bool enabled, int x, int y)
        {
            FixView(enabled, x, y);
            if (mmFixViewSw != null) mmFixViewSw.Checked = bFixedView;
            if (mmLeft != null) mmLeft.Enabled = bFixed || bFixedView;
            if (mmRight != null) mmRight.Enabled = bFixed || bFixedView;
            if (mmUpper != null) mmUpper.Enabled = bFixed || bFixedView;
            if (mmDowner != null) mmDowner.Enabled = bFixed || bFixedView;
            UpdateCaption();
            this.Invalidate();
        }

        // ============================================================
        // キャプション更新
        // ============================================================

        private void UpdateCaption()
        {
            string state = "";
            if (bHFlip) state = "反転(左右)";
            if (bVFlip) state = "反転(上下)";
            if (gAngle != 0)
            {
                state = (gAngle / 10).ToString() + "度回転";
            }

            string state2 = "";
            if (bFixed)
            {
                state2 = "表示固定 & ウィンドウ固定モードです";
                if (sbMain.Items.Count > 3)
                    sbMain.Items[3].Text = ":" + strClassName;
            }
            if (bAtariMode)
            {
                state2 = "表示レイヤ描画モードです";
                if (sbMain.Items.Count > 3)
                    sbMain.Items[3].Text = "Ctrl+マウス:消去実行";
            }
            if (!bFixed && !bAtariMode && !bMouseCap)
            {
                state2 = "表示モード";
            }
            if (bMouseCap)
            {
                if (bForFixViewCap)
                {
                    state2 = "表示範囲を指定してください:左クリックで固定";
                }
                else
                {
                    state2 = "対象を選択してください:左クリックで固定";
                }
            }

            string state3 = bBltAtari ? "on" : "off";

            try
            {
                if (sbMain.Items.Count > 0)
                    sbMain.Items[0].Text = "表示レイヤ:" + state3;
                if (sbMain.Items.Count > 1)
                    sbMain.Items[1].Text = state;
                if (sbMain.Items.Count > 3)
                    sbMain.Items[3].Text = state2;

                this.Text = $"拡大鏡 StretchViewCS [倍率:{capRate:F1}倍 角度:{gAngle}度 状態:{state2}]{state}";
            }
            catch
            {
                // エラー処理
            }
        }

        // ============================================================
        // 当たり判定モード処理
        // ============================================================

        private void AtariMode(bool onoff)
        {
            bAtariMode = onoff;
            if (bAtariMode)
            {
                bBltAtari = true;
                if (tbAtariVisible != null) tbAtariVisible.Checked = true;
                if (bmpAtari != null)
                {
                    // 既存のビットマップを破棄して新しいサイズで再作成
                    bmpAtari.Dispose();
                    bmpAtari = new Bitmap(transW, transH);
                    bmpAtari.MakeTransparent();
                }
            }
        }

        // ============================================================
        // メニューイベントハンドラ（主要なもの）
        // ============================================================

        private void Exit1Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MmRate10Click(object? sender, EventArgs e)
        {
            ChgCapRate(1.0f);
        }

        private void MmRate15Click(object? sender, EventArgs e)
        {
            ChgCapRate(1.5f);
        }

        private void MmRate20Click(object? sender, EventArgs e)
        {
            ChgCapRate(2.0f);
        }

        private void MmRate25Click(object? sender, EventArgs e)
        {
            ChgCapRate(2.5f);
        }

        private void MmRate30Click(object? sender, EventArgs e)
        {
            ChgCapRate(3.0f);
        }

        private void Mm40Click(object? sender, EventArgs e)
        {
            ChgCapRate(4.0f);
        }

        private void Mm50Click(object? sender, EventArgs e)
        {
            ChgCapRate(5.0f);
        }

        private void MmRate60Click(object? sender, EventArgs e)
        {
            ChgCapRate(6.0f);
        }

        private void MmRate70Click(object? sender, EventArgs e)
        {
            ChgCapRate(7.0f);
        }

        private void MmRate80Click(object? sender, EventArgs e)
        {
            ChgCapRate(8.0f);
        }

        private void MmRate90Click(object? sender, EventArgs e)
        {
            ChgCapRate(9.0f);
        }

        private void MmRate100Click(object? sender, EventArgs e)
        {
            ChgCapRate(10.0f);
        }

        private void MmRate160Click(object? sender, EventArgs e)
        {
            ChgCapRate(16.0f);
        }

        private void MmUPClick(object? sender, EventArgs e)
        {
            ChgCapRate(capRate + 1.0f);
        }

        private void MmDownClick(object? sender, EventArgs e)
        {
            ChgCapRate(capRate - 1.0f);
        }

        private void MmFlipHClick(object? sender, EventArgs e)
        {
            FlipHV(1);
        }

        private void MmFlipVClick(object? sender, EventArgs e)
        {
            FlipHV(2);
        }

        private void MFlexFlipClick(object? sender, EventArgs e)
        {
            bShowDlgBox = true;
            SwitchTopMost(false);

            if (mmFlexRotate != null && mmFlexRotate.Checked)
            {
                gAngle = 0;
                if (mmFlexRotate != null) mmFlexRotate.Checked = false;
            }
            else
            {
                try
                {
                    string? instr = Microsoft.VisualBasic.Interaction.InputBox(
                        "任意角度回転", "角度を入力してください(1~359)", "");
                    if (!string.IsNullOrEmpty(instr))
                    {
                        int iAngle = int.Parse(instr);
                        if (iAngle > 0 && iAngle < 360)
                        {
                            gAngle = iAngle * 10;
                            if (mmFlipV != null) mmFlipV.Checked = false;
                            if (mmFlipH != null) mmFlipH.Checked = false;
                            bHFlip = false;
                            bVFlip = false;
                            if (mmFlexRotate != null) mmFlexRotate.Checked = true;
                        }
                    }

                    if (bVFlip && mmFlipV != null) MmFlipVClick(this, EventArgs.Empty);
                    if (bHFlip && mmFlipH != null) MmFlipHClick(this, EventArgs.Empty);
                }
                catch
                {
                    MessageBox.Show("不正な数値です", "");
                }
            }

            SwitchTopMost(true);
            bShowDlgBox = false;
            tim_Tick(this, EventArgs.Empty);
        }

        private void MmFlexRotateClick(object? sender, EventArgs e)
        {
            MFlexFlipClick(sender, e);
            UpdateCaption();
        }

        private void MmGridClick(object? sender, EventArgs e)
        {
            bGrid = !bGrid;
            if (mmGrid != null) mmGrid.Checked = bGrid;
            if (tbGrid != null) tbGrid.Checked = bGrid;
        }

        private void MmLeftClick(object? sender, EventArgs e)
        {
            if (bFixed || bFixedView) MoveRange("left");
        }

        private void MmRightClick(object? sender, EventArgs e)
        {
            if (bFixed || bFixedView) MoveRange("right");
        }

        private void MmUpperClick(object? sender, EventArgs e)
        {
            if (bFixed || bFixedView) MoveRange("up");
        }

        private void MmDownerClick(object? sender, EventArgs e)
        {
            if (bFixed || bFixedView) MoveRange("down");
        }

        private void MmFixClick(object? sender, EventArgs e)
        {
            bFixed = !bFixed;
            if (mmFix != null) mmFix.Checked = bFixed;
            if (tbFixSimu != null) tbFixSimu.Checked = bFixed;

            if (bFixed)
            {
                StartMyCapture();
                Win32API.SetCursorPos(this.Left, this.Top);
            }
            else
            {
                FixMode(false);
            }
        }

        private void MmRedrawDeskClick(object? sender, EventArgs e)
        {
            Win32API.InvalidateRect(IntPtr.Zero, IntPtr.Zero, true);
        }

        private void Mm300x300Click(object? sender, EventArgs e)
        {
            ChgWindowSize(300, 300);
        }

        private void Mm200x200Click(object? sender, EventArgs e)
        {
            ChgWindowSize(200, 200);
        }

        private void N400x3001Click(object? sender, EventArgs e)
        {
            ChgWindowSize(400, 300);
        }

        private void N400x4001Click(object? sender, EventArgs e)
        {
            ChgWindowSize(400, 400);
        }

        private void Mm300x400Click(object? sender, EventArgs e)
        {
            ChgWindowSize(300, 400);
        }

        private void MmChgSizeClick(object? sender, EventArgs e)
        {
            try
            {
                string? widthStr = Microsoft.VisualBasic.Interaction.InputBox(
                    "", "幅サイズを入力してください(50~400)", "0");
                string? heightStr = Microsoft.VisualBasic.Interaction.InputBox(
                    "", "高さサイズを入力してください(50~400)", "0");

                if (!string.IsNullOrEmpty(widthStr) && !string.IsNullOrEmpty(heightStr))
                {
                    int iSizeW = int.Parse(widthStr);
                    int iSizeH = int.Parse(heightStr);
                    ChgWindowSize(iSizeW, iSizeH);
                }
            }
            catch
            {
                MessageBox.Show("不正な数値です", "");
            }
        }

        private void Help1Click(object? sender, EventArgs e)
        {
            SwitchTopMost(false);
            if (mmTopMost != null) mmTopMost.Checked = false;

            Win32API.ShellExecute(IntPtr.Zero, null,
                "http://f29.aaa.livedoor.jp/~morg/wiki/index.php?StretchView%2FHelp",
                null, null, Win32API.SW_NORMAL);
        }

        private void MmSampRateClick(object? sender, EventArgs e)
        {
            bShowDlgBox = true;
            SwitchTopMost(false);

            FFormShowing = true;
            try
            {
                string? tmpStr = Microsoft.VisualBasic.Interaction.InputBox(
                    "サンプリングレート", "サンプリングレートを指定してください" + Environment.NewLine +
                    "(単位:ms デフォルト 100  範囲 10~400)", IniManager.Instance.SamplingRate.ToString());

                if (!string.IsNullOrEmpty(tmpStr))
                {
                    int tmpInt = int.Parse(tmpStr);
                    if (tmpInt >= 10 && tmpInt <= 400)
                    {
                        IniManager.Instance.SamplingRate = tmpInt;
                        tim.Interval = IniManager.Instance.SamplingRate;
                    }
                    else
                    {
                        MessageBox.Show("範囲外の数値が入力されました", "");
                    }
                }
            }
            finally
            {
                FFormShowing = false;
            }
            SwitchTopMost(true);
            bShowDlgBox = false;
        }

        private void MmGraph30Click(object? sender, EventArgs e)
        {
            if (mmGraph30 != null)
            {
                mmGraph30.Checked = !mmGraph30.Checked;
                bGraph = mmGraph30.Checked;
                if (mmGraph30.Checked)
                {
                    if (mmGraph40 != null) mmGraph40.Checked = false;
                    if (mmGraph50 != null) mmGraph50.Checked = false;
                    if (mmGraph60 != null) mmGraph60.Checked = false;
                    iCutPixel = 30;
                }
                if (mmGraph != null) mmGraph.Checked = bGraph;
                bStateUpdate = true;
            }
        }

        private void MmGraph40Click(object? sender, EventArgs e)
        {
            if (mmGraph40 != null)
            {
                mmGraph40.Checked = !mmGraph40.Checked;
                bGraph = mmGraph40.Checked;
                if (mmGraph40.Checked)
                {
                    if (mmGraph30 != null) mmGraph30.Checked = false;
                    if (mmGraph50 != null) mmGraph50.Checked = false;
                    if (mmGraph60 != null) mmGraph60.Checked = false;
                    iCutPixel = 40;
                }
                if (mmGraph != null) mmGraph.Checked = bGraph;
                bStateUpdate = true;
            }
        }

        private void MmGraph50Click(object? sender, EventArgs e)
        {
            if (mmGraph50 != null)
            {
                mmGraph50.Checked = !mmGraph50.Checked;
                bGraph = mmGraph50.Checked;
                if (mmGraph50.Checked)
                {
                    if (mmGraph30 != null) mmGraph30.Checked = false;
                    if (mmGraph40 != null) mmGraph40.Checked = false;
                    if (mmGraph60 != null) mmGraph60.Checked = false;
                    iCutPixel = 50;
                }
                if (mmGraph != null) mmGraph.Checked = bGraph;
                bStateUpdate = true;
            }
        }

        private void MmGraph60Click(object? sender, EventArgs e)
        {
            if (mmGraph60 != null)
            {
                mmGraph60.Checked = !mmGraph60.Checked;
                bGraph = mmGraph60.Checked;
                if (mmGraph60.Checked)
                {
                    if (mmGraph30 != null) mmGraph30.Checked = false;
                    if (mmGraph40 != null) mmGraph40.Checked = false;
                    if (mmGraph50 != null) mmGraph50.Checked = false;
                    iCutPixel = 60;
                }
                if (mmGraph != null) mmGraph.Checked = bGraph;
                bStateUpdate = true;
            }
        }

        private void MmGraphFlexClick(object? sender, EventArgs e)
        {
            bShowDlgBox = true;
            SwitchTopMost(false);

            if (mmGraphFlex != null && mmGraphFlex.Checked)
            {
                bGraph = false;
                return;
            }
            bGraph = true;

            try
            {
                string? tmpStr = Microsoft.VisualBasic.Interaction.InputBox(
                    "グリッド間隔", "間隔を入力してください(ピクセル)", "");
                if (!string.IsNullOrEmpty(tmpStr))
                {
                    int tmpInt = int.Parse(tmpStr);
                    iCutPixel = tmpInt;
                }
            }
            catch
            {
                // エラー処理
            }
            bShowDlgBox = false;
            SwitchTopMost(true);
        }

        private void MmAtariModeClick(object? sender, EventArgs e)
        {
            bAtariMode = !bAtariMode;
            if (mmAtariMode != null) mmAtariMode.Checked = bAtariMode;
            if (tbAtariMode != null) tbAtariMode.Checked = bAtariMode;
            AtariMode(bAtariMode);
            bStateUpdate = true;
            UpdateCaption();
        }

        private void MmBltAtariClick(object? sender, EventArgs e)
        {
            bBltAtari = !bBltAtari;
            if (mmBltAtari != null) mmBltAtari.Checked = bBltAtari;
            if (tbAtariVisible != null) tbAtariVisible.Checked = bBltAtari;
            bStateUpdate = true;
            UpdateCaption();
        }

        private void MmClearAtariClick(object? sender, EventArgs e)
        {
            if (bmpAtari != null)
            {
                using (Graphics g = Graphics.FromImage(bmpAtari))
                {
                    g.Clear(Color.Transparent);
                }
            }
        }

        private void MmTopMostClick(object? sender, EventArgs e)
        {
            if (mmTopMost != null)
            {
                mmTopMost.Checked = !mmTopMost.Checked;
                SwitchTopMost(mmTopMost.Checked);
            }
        }

        private void About1Click(object? sender, EventArgs e)
        {
            bShowDlgBox = true;
            frmVersion frm = new frmVersion();
            frm.ShowDialog();
            bShowDlgBox = false;
        }

        private void MmOptionClick(object? sender, EventArgs e)
        {
            bShowDlgBox = true;
            SwitchTopMost(false);
            frmSetting frm = new frmSetting();
            frm.ShowDialog();
            bShowDlgBox = false;
        }

        private void MmColorPickerClick(object? sender, EventArgs e)
        {
            OpenColorPicker();
        }

        private void TbColorPickerClick(object? sender, EventArgs e)
        {
            OpenColorPicker();
        }

        private void OpenColorPicker()
        {
            using (var picker = new ColorPickerForm())
            {
                picker.ShowDialog();
            }
        }

        private void MmRulerClick(object? sender, EventArgs e)
        {
            OpenRuler();
        }

        private void TbRulerClick(object? sender, EventArgs e)
        {
            OpenRuler();
        }

        private void OpenRuler()
        {
            using (var ruler = new RulerForm())
            {
                ruler.ShowDialog();
            }
        }

        private void MmSaveViewFileClick(object? sender, EventArgs e)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "画像ファイル|*.bmp;*.jpg;*.png|すべてのファイル|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bmpClip = new Bitmap(transW, transH);
                    using (Graphics g = Graphics.FromImage(bmpClip))
                    {
                        g.CopyFromScreen(this.PointToScreen(new Point(transX, transY)),
                            new Point(0, 0), new Size(transW, transH));
                    }
                    bmpClip.Save(dlg.FileName);
                    bmpClip.Dispose();
                }
            }
            catch
            {
                // エラー処理
            }
        }

        private void MmCopyToClipBoardClick(object? sender, EventArgs e)
        {
            try
            {
                Bitmap bmpClip = new Bitmap(transW, transH);
                using (Graphics g = Graphics.FromImage(bmpClip))
                {
                    g.CopyFromScreen(this.PointToScreen(new Point(transX, transY)),
                        new Point(0, 0), new Size(transW, transH));
                }
                Clipboard.SetImage(bmpClip);
                bmpClip.Dispose();
            }
            catch
            {
                // エラー処理
            }
        }

        private void MmPrintClick(object? sender, EventArgs e)
        {
            try
            {
                Bitmap bmpClip = new Bitmap(transW, transH);
                using (Graphics g = Graphics.FromImage(bmpClip))
                {
                    g.CopyFromScreen(this.PointToScreen(new Point(transX, transY)),
                        new Point(0, 0), new Size(transW, transH));
                }

                using (var printDoc = new PrintDocument())
                {
                    printDoc.PrintPage += (s, ev) =>
                    {
                        var bounds = ev.MarginBounds;
                        ev.Graphics?.DrawImage(bmpClip, bounds.X, bounds.Y, bounds.Width, bounds.Height);
                        ev.HasMorePages = false;
                    };

                    using (var dlg = new PrintDialog())
                    {
                        dlg.Document = printDoc;
                        if (dlg.ShowDialog() == DialogResult.OK)
                            printDoc.Print();
                    }
                }

                bmpClip.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("印刷に失敗しました: " + ex.Message, AppTitle);
            }
        }

        private void MmSrcNotClick(object? sender, EventArgs e)
        {
            bSrcNot = !bSrcNot;
        }

        private void MHelpClick(object? sender, EventArgs e)
        {
            frmHelp frm = new frmHelp();
            frm.ShowDialog();
        }

        private void FormActivate(object? sender, EventArgs e)
        {
            RegisterMyHotkeys();
        }

        private void MyOnMinimize(object? sender, EventArgs e)
        {
            tim.Enabled = false;
            UnregisterMyHotkeys();
        }

        private void MyOnRestore(object? sender, EventArgs e)
        {
            tim.Enabled = true;
            RegisterMyHotkeys();
        }

        private void frmCap_ResizeBegin(object? sender, EventArgs e)
        {
            // リサイズ開始時の処理
        }

        private void frmCap_ResizeEnd(object? sender, EventArgs e)
        {
            // リサイズ終了時の処理
            if (this.WindowState == FormWindowState.Normal)
            {
                ChgWindowSize(this.ClientSize.Width, this.ClientSize.Height);
            }
        }


        // ============================================================
        // ツールバーイベントハンドラ
        // ============================================================

        private void TbFlipHClick(object? sender, EventArgs e)
        {
            FlipHV(1);
        }

        private void TbFlipVClick(object? sender, EventArgs e)
        {
            FlipHV(2);
        }

        private void TbZoomUpMouseDown(object? sender, MouseEventArgs e)
        {
            repeatMode = RM_ZOOM;
            sgRepeatCapRate = sgIncCapRate;
            timRepeat.Enabled = true;
        }

        private void TbZoomUpMouseUp(object? sender, MouseEventArgs e)
        {
            timRepeat.Enabled = false;
        }

        private void TbZoomOutMouseDown(object? sender, MouseEventArgs e)
        {
            repeatMode = RM_ZOOM;
            sgRepeatCapRate = -sgIncCapRate;
            timRepeat.Enabled = true;
        }

        private void TbZoomOutMouseUp(object? sender, MouseEventArgs e)
        {
            timRepeat.Enabled = false;
        }

        private void TbLRotateMouseDown(object? sender, MouseEventArgs e)
        {
            repeatMode = RM_ROTATE;
            iRepeatAngle = -iIncAngle;
            timRepeat.Enabled = true;
        }

        private void TbLRotateMouseUp(object? sender, MouseEventArgs e)
        {
            timRepeat.Enabled = false;
        }

        private void TbRRotateMouseDown(object? sender, MouseEventArgs e)
        {
            repeatMode = RM_ROTATE;
            iRepeatAngle = iIncAngle;
            timRepeat.Enabled = true;
        }

        private void TbRRotateMouseUp(object? sender, MouseEventArgs e)
        {
            timRepeat.Enabled = false;
        }

        private void TbGridMouseDown(object? sender, MouseEventArgs e)
        {
            bGraph = !bGraph;
            if (tbGrid != null) tbGrid.Checked = bGraph;
            if (mmGrid != null) mmGrid.Checked = bGraph;
        }

        private void TbFixSimuClick(object? sender, EventArgs e)
        {
            MmFixClick(this, EventArgs.Empty);
        }

        private void TbAtariModeClick(object? sender, EventArgs e)
        {
            MmAtariModeClick(this, EventArgs.Empty);
        }

        private void TbAtariVisibleClick(object? sender, EventArgs e)
        {
            MmBltAtariClick(this, EventArgs.Empty);
        }

        private void TbResetClick(object? sender, EventArgs e)
        {
            gAngle = 0;
            UpdateCaption();
        }

        private void TbClearClick(object? sender, EventArgs e)
        {
            MmClearAtariClick(this, EventArgs.Empty);
        }

        // ============================================================
        // その他のイベントハンドラ
        // ============================================================

        private void MmFixViewSwClick(object? sender, EventArgs e)
        {
            // オーバーレイウィンドウ方式で範囲指定を行う
            if (!bFixedView)
            {
                try
                {
                    using (var overlay = new OverlaySelectionForm())
                    {
                        Point selectedCenter = Point.Empty;
                        overlay.SelectionCompleted += p => selectedCenter = p;

                        // メインウィンドウはそのまま（最前面指定は維持）
                        overlay.ShowDialog(this);

                        if (selectedCenter != Point.Empty)
                        {
                            ApplyFixViewState(true, selectedCenter.X, selectedCenter.Y);
                        }
                    }
                }
                catch
                {
                    // 失敗した場合は従来通りの状態に戻すだけ
                }
            }
            else
            {
                // すでに固定中なら固定表示を解除
                ApplyFixViewState(false, 0, 0);
            }
        }

        private void MmFixViewClick(object? sender, EventArgs e)
        {
            // 親メニューを開いたときに各項目の状態を更新するだけ
            if (mmFixViewSw != null) mmFixViewSw.Checked = bFixedView;
            if (mmLeft != null) mmLeft.Enabled = bFixed || bFixedView;
            if (mmRight != null) mmRight.Enabled = bFixed || bFixedView;
            if (mmUpper != null) mmUpper.Enabled = bFixed || bFixedView;
            if (mmDowner != null) mmDowner.Enabled = bFixed || bFixedView;
        }

        private void ChangeUIMode(int iMode)
        {
            // UIモード変更処理
        }

        // ============================================================
        // Paintイベントハンドラ
        // ============================================================

        private void frmCap_Paint(object? sender, PaintEventArgs e)
        {
            // 背景をクリア
            e.Graphics.Clear(this.BackColor);

            if (bmpDisplay != null)
            {
                lock (_bmpDisplayLock)
                {
                    e.Graphics.DrawImage(bmpDisplay, transX, transY);
                }
            }

            // カーソル位置表示（固定表示モードでない場合）
            if (!bFixed && !bAtariMode && !bMouseCap)
            {
                Point mousePos = this.PointToClient(Cursor.Position);
                if (mousePos.X >= transX && mousePos.X < transX + transW &&
                    mousePos.Y >= transY && mousePos.Y < transY + transH)
                {
                    using (Pen pen = new Pen(Color.White, 1))
                    {
                        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        int size = (int)(5 * capRate);
                        e.Graphics.DrawRectangle(pen, mousePos.X - size / 2, mousePos.Y - size / 2, size, size);
                    }
                }
            }

            // 操作投影モードのターゲット選択中：自フォーム内に選択枠のみ描画（デスクトップは触らない）
            if (bMouseCap && !bForFixViewCap && hLastTargetWindow != IntPtr.Zero && Win32API.IsWindow(hLastTargetWindow))
            {
                Win32API.GetWindowRect(hLastTargetWindow, out Win32API.RECT rSel);
                int x1 = transX + (int)((rSel.Left - lastCaptureLeft) * capRate);
                int y1 = transY + (int)((rSel.Top - lastCaptureTop) * capRate);
                int w = (int)((rSel.Right - rSel.Left) * capRate);
                int h = (int)((rSel.Bottom - rSel.Top) * capRate);
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawRectangle(pen, x1, y1, w, h);
                }
            }
        }

        private void MmRangeClick(object? sender, EventArgs e)
        {
            if (mmLeft != null) mmLeft.Enabled = bFixed || bFixedView;
            if (mmRight != null) mmRight.Enabled = bFixed || bFixedView;
            if (mmUpper != null) mmUpper.Enabled = bFixed || bFixedView;
            if (mmDowner != null) mmDowner.Enabled = bFixed || bFixedView;

            if (mmGraph != null) mmGraph.Checked = bGraph;
            if (mmGraph30 != null) mmGraph30.Checked = false;
            if (mmGraph40 != null) mmGraph40.Checked = false;
            if (mmGraph50 != null) mmGraph50.Checked = false;
            if (mmGraph60 != null) mmGraph60.Checked = false;
            if (mmGraphFlex != null) mmGraphFlex.Checked = false;

            if (bGraph)
            {
                if (iCutPixel == 30 && mmGraph30 != null) mmGraph30.Checked = true;
                else if (iCutPixel == 40 && mmGraph40 != null) mmGraph40.Checked = true;
                else if (iCutPixel == 50 && mmGraph50 != null) mmGraph50.Checked = true;
                else if (iCutPixel == 60 && mmGraph60 != null) mmGraph60.Checked = true;
                else if (mmGraphFlex != null) mmGraphFlex.Checked = true;
            }
        }

        private void MAtariClick(object? sender, EventArgs e)
        {
            if (mmBltAtari != null) mmBltAtari.Checked = bBltAtari;
            if (mmAtariMode != null) mmAtariMode.Checked = bAtariMode;
        }

        private void MmAtarisClick(object? sender, EventArgs e)
        {
            if (mmWndInfo != null) mmWndInfo.Enabled = bFixed;
            if (mmFix != null) mmFix.Enabled = !bAtariMode;
        }

        private void MmRateClick(object? sender, EventArgs e)
        {
            if (mmSrcNot != null) mmSrcNot.Checked = bSrcNot;
        }

        private void Help2Click(object? sender, EventArgs e)
        {
            SwitchTopMost(false);
            if (mmTopMost != null) mmTopMost.Checked = false;

            try
            {
                string helpFile = Path.Combine(Application.StartupPath, "help", "index.html");
                if (!System.IO.File.Exists(helpFile))
                {
                    throw new FileNotFoundException("ヘルプファイルが見つかりません。", helpFile);
                }

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = helpFile;
                startInfo.UseShellExecute = true;
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    "ヘルプを開けませんでした。\r\n" + ex.Message,
                    "StretchViewCS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void UpdateDisplayBitmap(Bitmap sourceBitmap)
        {
            lock (_bmpDisplayLock)
            {
                if (bmpDisplay == null ||
                    bmpDisplay.Width != sourceBitmap.Width ||
                    bmpDisplay.Height != sourceBitmap.Height ||
                    bmpDisplay.PixelFormat != sourceBitmap.PixelFormat)
                {
                    bmpDisplay?.Dispose();
                    bmpDisplay = new Bitmap(sourceBitmap);
                    return;
                }

                using (Graphics g = Graphics.FromImage(bmpDisplay))
                {
                    g.DrawImageUnscaled(sourceBitmap, 0, 0);
                }
            }
        }

        private PixelFormat GetDesktopPixelFormat(IntPtr desktopDC)
        {
            int bpp = Win32API.GetDeviceCaps(desktopDC, Win32API.BITSPIXEL);
            if (bpp == 32) return PixelFormat.Format32bppRgb;
            if (bpp == 24) return PixelFormat.Format24bppRgb;
            if (bpp == 16) return PixelFormat.Format16bppRgb565;
            if (bpp == 15) return PixelFormat.Format16bppRgb555;
            if (bpp == 8) return PixelFormat.Format8bppIndexed;
            return PixelFormat.Format24bppRgb;
        }

        private Bitmap GetOrCreateWorkBitmap(ref Bitmap? targetBitmap, int width, int height, PixelFormat pixelFormat)
        {
            if (targetBitmap == null ||
                targetBitmap.Width != width ||
                targetBitmap.Height != height ||
                targetBitmap.PixelFormat != pixelFormat)
            {
                targetBitmap?.Dispose();
                targetBitmap = new Bitmap(width, height, pixelFormat);
            }

            return targetBitmap;
        }

        private void CaptureDesktopBackup()
        {
            if (bmpBackUp == null)
            {
                throw new InvalidOperationException("バックアップ用ビットマップが初期化されていません。");
            }

            using (Graphics backupGraphics = Graphics.FromImage(bmpBackUp))
            {
                backupGraphics.CopyFromScreen(
                    rcLastDraw.Left,
                    rcLastDraw.Top,
                    0,
                    0,
                    new Size(rcLastDraw.Width + 4, rcLastDraw.Height + 4));
            }
        }

        private void RestoreDesktopBackup(Graphics desktopGraphics)
        {
            if (bmpBackUp == null)
            {
                throw new InvalidOperationException("復元対象のバックアップがありません。");
            }

            desktopGraphics.DrawImageUnscaled(bmpBackUp, rcLastDraw.Left, rcLastDraw.Top);
        }
    }
}
