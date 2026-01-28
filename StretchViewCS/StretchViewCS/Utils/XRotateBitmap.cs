using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace StretchViewCS.Utils
{
    /// <summary>
    /// ビットマップ回転処理クラス（DelphiのXRotateBitmapのC#版）
    /// 注: 完全な実装には低レベルピクセル操作が必要ですが、
    ///     ここではGraphics.Transformを使用した簡易版を提供します
    /// </summary>
    public static class XRotateBitmap
    {
        /// <summary>
        /// ビットマップを任意の角度で回転（10度単位）
        /// </summary>
        /// <param name="bitmap">元のビットマップ</param>
        /// <param name="angleDeg">回転角度（10倍の値、例: 900 = 90度）</param>
        /// <param name="flgMirror">ミラー反転フラグ</param>
        /// <param name="flgCircum">外接矩形でサイズを決定するか</param>
        /// <returns>回転後のビットマップ</returns>
        public static Bitmap RotateBitmapX(Bitmap bitmap, int angleDeg, bool flgMirror, bool flgCircum)
        {
            if (bitmap == null || bitmap.Width == 0 || bitmap.Height == 0)
                throw new ArgumentException("Bitmap Size Error");

            // 角度を度に変換（10倍の値から）
            float angle = angleDeg / 10.0f;

            // 回転後のサイズを計算
            int newWidth, newHeight;
            if (flgCircum)
            {
                // 外接矩形でサイズを決定
                double rad = angle * Math.PI / 180.0;
                double cos = Math.Abs(Math.Cos(rad));
                double sin = Math.Abs(Math.Sin(rad));
                newWidth = (int)(bitmap.Width * cos + bitmap.Height * sin);
                newHeight = (int)(bitmap.Width * sin + bitmap.Height * cos);
            }
            else
            {
                // 元のサイズを維持
                newWidth = bitmap.Width;
                newHeight = bitmap.Height;
            }

            Bitmap result = new Bitmap(newWidth, newHeight, bitmap.PixelFormat);
            result.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // 回転の中心を設定
                g.TranslateTransform(newWidth / 2.0f, newHeight / 2.0f);

                // ミラー反転
                if (flgMirror)
                {
                    g.ScaleTransform(-1, 1);
                }

                // 回転
                g.RotateTransform(angle);

                // 元の画像を描画（中心を基準に）
                g.DrawImage(bitmap, -bitmap.Width / 2.0f, -bitmap.Height / 2.0f);
            }

            return result;
        }

        /// <summary>
        /// ビットマップを任意の角度で回転（Extended版）
        /// </summary>
        public static void RotateImage(Bitmap bitmap, double angle)
        {
            if (bitmap == null || bitmap.Width == 0 || bitmap.Height == 0)
                return;

            double radAngle = angle * Math.PI / 180.0;

            // 4隅の座標を回転
            Point[] corners = new Point[]
            {
                new Point(0, 0),
                new Point(bitmap.Width - 1, 0),
                new Point(0, bitmap.Height - 1),
                new Point(bitmap.Width - 1, bitmap.Height - 1)
            };

            Point[] rotatedCorners = new Point[4];
            for (int i = 0; i < 4; i++)
            {
                int x = corners[i].X;
                int y = corners[i].Y;
                rotatedCorners[i] = new Point(
                    (int)(Math.Cos(radAngle) * x - Math.Sin(radAngle) * y),
                    (int)(Math.Sin(radAngle) * x + Math.Cos(radAngle) * y)
                );
            }

            // 新しい矩形を計算
            int minX = Math.Min(Math.Min(rotatedCorners[0].X, rotatedCorners[1].X),
                               Math.Min(rotatedCorners[2].X, rotatedCorners[3].X));
            int minY = Math.Min(Math.Min(rotatedCorners[0].Y, rotatedCorners[1].Y),
                               Math.Min(rotatedCorners[2].Y, rotatedCorners[3].Y));
            int maxX = Math.Max(Math.Max(rotatedCorners[0].X, rotatedCorners[1].X),
                               Math.Max(rotatedCorners[2].X, rotatedCorners[3].X));
            int maxY = Math.Max(Math.Max(rotatedCorners[0].Y, rotatedCorners[1].Y),
                               Math.Max(rotatedCorners[2].Y, rotatedCorners[3].Y));

            int newWidth = maxX - minX + 1;
            int newHeight = maxY - minY + 1;

            Bitmap newBitmap = new Bitmap(newWidth, newHeight, bitmap.PixelFormat);
            newBitmap.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;

                // 逆回転で元の座標を計算
                radAngle = -radAngle;
                for (int newY = minY; newY <= maxY; newY++)
                {
                    for (int newX = minX; newX <= maxX; newX++)
                    {
                        int oldX = (int)(Math.Cos(radAngle) * newX - Math.Sin(radAngle) * newY);
                        int oldY = (int)(Math.Sin(radAngle) * newX + Math.Cos(radAngle) * newY);

                        if (oldX >= 0 && oldX < bitmap.Width && oldY >= 0 && oldY < bitmap.Height)
                        {
                            Color pixel = bitmap.GetPixel(oldX, oldY);
                            newBitmap.SetPixel(newX - minX, newY - minY, pixel);
                        }
                        else
                        {
                            newBitmap.SetPixel(newX - minX, newY - minY, Color.White);
                        }
                    }
                }
            }

            // 元のビットマップを置き換え
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                g.DrawImage(newBitmap, 0, 0);
            }

            newBitmap.Dispose();
        }
    }
}
