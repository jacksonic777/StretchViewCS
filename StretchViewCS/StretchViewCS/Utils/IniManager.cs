using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace StretchViewCS.Utils
{
    /// <summary>
    /// INIファイル管理クラス（DelphiのTIniManagerのC#版）
    /// 簡易的なINIファイル読み書きを実装
    /// </summary>
    public class IniManager
    {
        private static IniManager? _instance;
        private readonly string _iniPath;

        // プロパティ
        public bool VFlip { get; set; }
        public bool HFlip { get; set; }
        public int Scale { get; set; }
        public bool FirstRun { get; set; }
        public int RunCount { get; set; }
        public string LicenseKey { get; set; } = "";
        public Rectangle BoundsRect { get; set; }
        public bool InfoVisible { get; set; }
        public bool CrossVisible { get; set; }
        public int SamplingRate { get; set; }
        public bool InfoIsHex { get; set; }
        public int ScaleWidth { get; set; }
        public int ScaleHeight { get; set; }
        public float CapRate { get; set; }
        public bool FixView { get; set; }
        public int FixViewX { get; set; }
        public int FixViewY { get; set; }

        // イベント
        public event EventHandler? OnChange;

        private IniManager()
        {
            _iniPath = Path.ChangeExtension(Application.ExecutablePath, ".ini");
            Read();
        }

        public static IniManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new IniManager();
                }
                return _instance;
            }
        }

        private string ReadString(string section, string key, string defaultValue)
        {
            if (!System.IO.File.Exists(_iniPath))
                return defaultValue;

            try
            {
                string[] lines = System.IO.File.ReadAllLines(_iniPath, Encoding.Default);
                bool inSection = false;

                foreach (string line in lines)
                {
                    string trimmed = line.Trim();
                    if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                    {
                        inSection = trimmed == "[" + section + "]";
                        continue;
                    }

                    if (inSection && trimmed.StartsWith(key + "="))
                    {
                        return trimmed.Substring(key.Length + 1);
                    }
                }
            }
            catch
            {
                // エラー時はデフォルト値を返す
            }

            return defaultValue;
        }

        private int ReadInteger(string section, string key, int defaultValue)
        {
            string value = ReadString(section, key, defaultValue.ToString());
            if (int.TryParse(value, out int result))
                return result;
            return defaultValue;
        }

        private bool ReadBool(string section, string key, bool defaultValue)
        {
            string value = ReadString(section, key, defaultValue.ToString());
            if (bool.TryParse(value, out bool result))
                return result;
            return defaultValue;
        }

        private void WriteString(string section, string key, string value)
        {
            try
            {
                StringBuilder content = new StringBuilder();
                bool sectionExists = false;
                bool keyExists = false;

                if (System.IO.File.Exists(_iniPath))
                {
                    string[] lines = System.IO.File.ReadAllLines(_iniPath, Encoding.Default);
                    bool inSection = false;

                    foreach (string line in lines)
                    {
                        string trimmed = line.Trim();
                        if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                        {
                            if (inSection && !keyExists)
                            {
                                content.AppendLine(key + "=" + value);
                                keyExists = true;
                            }
                            inSection = trimmed == "[" + section + "]";
                            if (inSection) sectionExists = true;
                            content.AppendLine(line);
                            continue;
                        }

                        if (inSection && trimmed.StartsWith(key + "="))
                        {
                            content.AppendLine(key + "=" + value);
                            keyExists = true;
                        }
                        else
                        {
                            content.AppendLine(line);
                        }
                    }

                    if (inSection && !keyExists)
                    {
                        content.AppendLine(key + "=" + value);
                        keyExists = true;
                    }
                }

                if (!sectionExists)
                {
                    content.AppendLine("[" + section + "]");
                    content.AppendLine(key + "=" + value);
                }

                System.IO.File.WriteAllText(_iniPath, content.ToString(), Encoding.Default);
            }
            catch
            {
                // エラー処理
            }
        }

        private void WriteInteger(string section, string key, int value)
        {
            WriteString(section, key, value.ToString());
        }

        private void WriteBool(string section, string key, bool value)
        {
            WriteString(section, key, value.ToString());
        }

        private void Read()
        {
            // BoundsRect
            BoundsRect = new Rectangle(
                ReadInteger("BoundsRect", "Left", 300),
                ReadInteger("BoundsRect", "Top", 100),
                ReadInteger("BoundsRect", "Right", 800) - ReadInteger("BoundsRect", "Left", 300),
                ReadInteger("BoundsRect", "Bottom", 600) - ReadInteger("BoundsRect", "Top", 100)
            );

            // Lenz
            Scale = ReadInteger("Lenz", "Scale", 4);
            HFlip = ReadBool("Lenz", "HFlip", false);
            VFlip = ReadBool("Lenz", "VFlip", false);
            ScaleWidth = ReadInteger("Lenz", "ScaleWidth", 300);
            ScaleHeight = ReadInteger("Lenz", "ScaleHeight", 300);

            int capRateInt = ReadInteger("Lenz", "CapRate", 10);
            CapRate = capRateInt / 10.0f;

            CrossVisible = ReadBool("Lenz", "CrossVisible", true);
            InfoIsHex = ReadBool("Lenz", "InfoIsHex", true);
            SamplingRate = ReadInteger("Lenz", "SamplingRate", 100);

            FixView = ReadBool("Lenz", "FixView", false);
            FixViewX = ReadInteger("Lenz", "FixViewX", 200);
            FixViewY = ReadInteger("Lenz", "FixViewY", 200);

            // Setting
            FirstRun = ReadBool("Setting", "FirstRun", true);
            LicenseKey = ReadString("Setting", "LicenseKey", "");
            RunCount = ReadInteger("Setting", "RunCount", 1);
        }

        public void Write()
        {
            // BoundsRect
            WriteInteger("BoundsRect", "Left", BoundsRect.Left);
            WriteInteger("BoundsRect", "Right", BoundsRect.Left + BoundsRect.Width);
            WriteInteger("BoundsRect", "Top", BoundsRect.Top);
            WriteInteger("BoundsRect", "Bottom", BoundsRect.Top + BoundsRect.Height);

            // Lenz
            WriteInteger("Lenz", "Scale", Scale);
            WriteBool("Lenz", "HFlip", HFlip);
            WriteBool("Lenz", "VFlip", VFlip);
            WriteInteger("Lenz", "ScaleWidth", ScaleWidth);
            WriteInteger("Lenz", "ScaleHeight", ScaleHeight);
            WriteInteger("Lenz", "CapRate", (int)(CapRate * 10));
            WriteBool("Lenz", "CrossVisible", CrossVisible);
            WriteBool("Lenz", "InfoVisible", InfoVisible);
            WriteBool("Lenz", "InfoIsHex", InfoIsHex);
            WriteInteger("Lenz", "SamplingRate", SamplingRate);
            WriteBool("Lenz", "FixView", FixView);
            WriteInteger("Lenz", "FixViewX", FixViewX);
            WriteInteger("Lenz", "FixViewY", FixViewY);

            // Setting
            WriteBool("Setting", "FirstRun", false);
            WriteString("Setting", "LicenseKey", LicenseKey);
            RunCount++;
            WriteInteger("Setting", "RunCount", RunCount);
        }

        private void CallChangeHandler()
        {
            OnChange?.Invoke(this, EventArgs.Empty);
        }

        public void SetBoundsRect(Rectangle value)
        {
            if (BoundsRect != value)
            {
                BoundsRect = value;
                Write();
            }
        }

        public void SetScale(int value)
        {
            if (Scale != value)
            {
                Scale = Math.Max(1, value);
                Write();
                CallChangeHandler();
            }
        }

        public void SetHFlip(bool value)
        {
            if (HFlip != value)
            {
                HFlip = value;
                Write();
                CallChangeHandler();
            }
        }

        public void SetVFlip(bool value)
        {
            if (VFlip != value)
            {
                VFlip = value;
                Write();
                CallChangeHandler();
            }
        }

        public void SetCrossVisible(bool value)
        {
            if (CrossVisible != value)
            {
                CrossVisible = value;
                Write();
                CallChangeHandler();
            }
        }

        public void SetCapRate(float value)
        {
            if (Math.Abs(CapRate - value) > 0.001f)
            {
                CapRate = value;
                Write();
                CallChangeHandler();
            }
        }

        public void SetScaleWidth(int value)
        {
            if (ScaleWidth != value)
            {
                ScaleWidth = value;
                Write();
                CallChangeHandler();
            }
        }

        public void SetScaleHeight(int value)
        {
            if (ScaleHeight != value)
            {
                ScaleHeight = value;
                Write();
                CallChangeHandler();
            }
        }

        public void SetSamplingRate(int value)
        {
            if (SamplingRate != value)
            {
                SamplingRate = value;
                Write();
                CallChangeHandler();
            }
        }

        public void SetInfoIsHex(bool value)
        {
            if (InfoIsHex != value)
            {
                InfoIsHex = value;
                Write();
                CallChangeHandler();
            }
        }

        ~IniManager()
        {
            Write();
        }
    }
}
