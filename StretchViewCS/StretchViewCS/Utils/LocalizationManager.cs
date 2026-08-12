using System;
using System.Collections.Generic;
using System.Globalization;

namespace StretchViewCS.Utils
{
    public static class LocalizationManager
    {
        public const string Japanese = "ja";
        public const string English = "en";

        private static readonly Dictionary<string, Dictionary<string, string>> Resources =
            new Dictionary<string, Dictionary<string, string>>
            {
                [Japanese] = new Dictionary<string, string>
                {
                    ["Main.FixView"] = "対象範囲の選択",
                    ["Main.FixViewMenu"] = "対象範囲の選択(&S)",
                    ["Main.FixViewToggle"] = "範囲の指定←→解除",
                    ["Main.MoveLeft"] = "← 表示範囲を左に移動",
                    ["Main.MoveRight"] = "→ 表示範囲を右に移動",
                    ["Main.MoveUp"] = "↑表示範囲を上に移動",
                    ["Main.MoveDown"] = "↓表示範囲を下に移動",
                    ["Main.Transform"] = "拡大・反転・回転(&E)",
                    ["Main.ZoomUp"] = "倍率を上げる",
                    ["Main.ZoomDown"] = "倍率を下げる",
                    ["Main.FlipH"] = "表示を左右反転する",
                    ["Main.FlipV"] = "表示を上下反転する",
                    ["Main.RotateCustom"] = "指定角度回転する",
                    ["Main.InvertColor"] = "色反転する",
                    ["Main.RedrawDesktop"] = "デスクトップの再描画",
                    ["Main.Exit"] = "終了",
                    ["Main.View"] = "表示(&V)",
                    ["Main.TopMost"] = "最前面表示",
                    ["Main.Tools"] = "ツール(&T)",
                    ["Main.ColorPicker"] = "カラーピッカー",
                    ["Main.SettingsMenu"] = "設定..",
                    ["Main.Ruler"] = "画面定規（幅・高さ計測）",
                    ["Main.SaveImage"] = "画像の保存",
                    ["Main.SaveBitmap"] = "画像をビットマップ形式で保存する...",
                    ["Main.CopyClipboard"] = "画像をクリップボードにコピーする",
                    ["Main.Print"] = "簡易印刷...",
                    ["Main.Extensions"] = "拡張機能(&X)",
                    ["Main.SurfaceLayer"] = "表面レイヤ",
                    ["Main.SurfaceDrawMode"] = "表面レイヤへの描き込みモード ON/OFF",
                    ["Main.SurfaceVisible"] = "表面レイヤの表示/非表示",
                    ["Main.SurfaceClear"] = "表面レイヤのクリア",
                    ["Main.Help"] = "ヘルプ(&H)",
                    ["Main.OpenHelp"] = "ヘルプを開く",
                    ["Main.About"] = "バージョン情報",
                    ["Main.ZoomInButton"] = "拡大",
                    ["Main.ZoomOutButton"] = "縮小",
                    ["Main.ResetZoom"] = "拡縮リセット",
                    ["Main.FlipHButton"] = "左右反転",
                    ["Main.FlipVButton"] = "上下反転",
                    ["Main.RotateLeft"] = "左回転",
                    ["Main.RotateRight"] = "右回転",
                    ["Main.ResetRotation"] = "回転リセット",
                    ["Main.RulerButton"] = "画面定規",
                    ["Main.SettingsButton"] = "設定",
                    ["Main.Hotkeys"] = "ホットキー有効",
                    ["Main.GridButton"] = "方眼",
                    ["Main.Clear"] = "クリア",
                    ["Main.Graph"] = "方眼表示",
                    ["Main.Graph30"] = "30ピクセル線",
                    ["Main.Graph40"] = "40ピクセル線",
                    ["Main.Graph50"] = "50ピクセル線",
                    ["Main.Graph60"] = "60ピクセル線",
                    ["Main.Custom"] = "任意",
                    ["Main.CenterLine"] = "中心線表示",
                    ["Main.WindowPreset"] = "ウィンドウ定型サイズ",
                    ["Main.LayerStatusOff"] = "表示レイヤ:off",
                    ["Message.TargetTitle"] = "対象取得",
                    ["Message.TargetNotFound"] = "対象が取得できません",
                    ["Message.TargetSelf"] = "自分自身を選択しています",
                    ["Message.ValueOutOfRange"] = "値が大きすぎるか小さすぎます",
                    ["Message.Title"] = "メッセージ",
                    ["Message.InvalidNumber"] = "不正な数値です",
                    ["Message.OutOfRangeNumber"] = "範囲外の数値が入力されました",
                    ["Message.SaveImageFailed"] = "画像保存に失敗しました:",
                    ["Message.CopyClipboardFailed"] = "クリップボードへのコピーに失敗しました:",
                    ["Message.PrintFailed"] = "印刷に失敗しました:",
                    ["Message.HelpFileNotFound"] = "ヘルプファイルが見つかりません。",
                    ["Message.OpenHelpFailed"] = "ヘルプを開けませんでした。",
                    ["Input.RotateTitle"] = "任意角度回転",
                    ["Input.RotatePrompt"] = "角度を入力してください(1~359)",
                    ["Input.WidthPrompt"] = "幅サイズを入力してください(50~400)",
                    ["Input.HeightPrompt"] = "高さサイズを入力してください(50~400)",
                    ["Input.SamplingTitle"] = "サンプリングレート",
                    ["Input.SamplingPrompt"] = "サンプリングレートを指定してください\r\n(単位:ms デフォルト 400  範囲 10~400)",
                    ["Input.GridTitle"] = "グリッド間隔",
                    ["Input.GridPrompt"] = "間隔を入力してください(ピクセル)",
                    ["Settings.Title"] = "設定",
                    ["Settings.StartMenu"] = "スタートメニューに登録",
                    ["Settings.HotkeysEnabled"] = "ホットキー有効",
                    ["Settings.RestoreFixViewOnStartup"] = "対象範囲の選択を起動時に復元",
                    ["Settings.SamplingRate"] = "サンプリングレート(ms)",
                    ["Settings.Language"] = "表示言語",
                    ["Settings.LanguageJapanese"] = "日本語",
                    ["Settings.LanguageEnglish"] = "English",
                    ["Settings.Cancel"] = "キャンセル",
                    ["Help.Title"] = "ヘルプ",
                    ["Help.Basic"] = "基本操作",
                    ["Help.Mouse"] = "マウスで操作",
                    ["Help.Menu"] = "メニューから各種機能を選択",
                    ["Help.Shortcuts"] = "ショートカットキー",
                    ["Help.ZoomUp"] = "Ctrl+A: ズームアップ",
                    ["Help.ZoomDown"] = "Ctrl+S: ズームダウン",
                    ["Help.FlipH"] = "Ctrl+D: 左右反転",
                    ["Help.FlipV"] = "Ctrl+F: 上下反転",
                    ["Help.FixedMode"] = "固定表示モード",
                    ["Help.FixedText"] = "対象範囲を指定すると、選択した画面範囲を固定して表示できます。\r\n対象範囲の選択メニューまたはチェックボックスから指定と解除を切り替えます。",
                    ["Version.Title"] = "バージョン情報",
                    ["Version.Label"] = "バージョン:",
                    ["Version.Unknown"] = "バージョン: 不明",
                    ["Version.Description"] = "説明:",
                    ["Ruler.Title"] = "画面定規 - 寸法",
                    ["Ruler.EmptySize"] = "幅 --- px  ×  高さ --- px",
                    ["Ruler.WidthLabel"] = "幅",
                    ["Ruler.HeightLabel"] = "高さ",
                    ["Ruler.Instruction"] = "左ドラッグで範囲を指定　Esc/右クリックで終了",
                    ["Ruler.ExitInstruction"] = "Esc または 右クリック で終了",
                    ["Ruler.StartInstruction"] = "左ドラッグで計測したい範囲を指定",
                    ["ColorPicker.CopiedTitle"] = "色をコピーしました",
                    ["ColorPicker.CopiedBody"] = "色コードをクリップボードにコピーしました。",
                    ["ColorPicker.InfoTitle"] = "カラーピッカー情報",
                    ["ColorPicker.PositionLabel"] = "座標",
                    ["ColorPicker.HexLabel"] = "HEX",
                    ["ColorPicker.RgbLabel"] = "RGB",
                    ["ColorPicker.InfoInstruction"] = "左クリックでコピー / Escで終了",
                },
                [English] = new Dictionary<string, string>
                {
                    ["Main.FixView"] = "Selection Range(&S)",
                    ["Main.FixViewMenu"] = "Selection Range(&S)",
                    ["Main.FixViewToggle"] = "Select / Clear Range",
                    ["Main.MoveLeft"] = "Move view range left",
                    ["Main.MoveRight"] = "Move view range right",
                    ["Main.MoveUp"] = "Move view range up",
                    ["Main.MoveDown"] = "Move view range down",
                    ["Main.Transform"] = "Zoom / Flip / Rotate (&E)",
                    ["Main.ZoomUp"] = "Increase zoom",
                    ["Main.ZoomDown"] = "Decrease zoom",
                    ["Main.FlipH"] = "Flip horizontally",
                    ["Main.FlipV"] = "Flip vertically",
                    ["Main.RotateCustom"] = "Rotate by angle",
                    ["Main.InvertColor"] = "Invert colors",
                    ["Main.RedrawDesktop"] = "Redraw desktop",
                    ["Main.Exit"] = "Exit",
                    ["Main.View"] = "View (&V)",
                    ["Main.TopMost"] = "Always on top",
                    ["Main.Tools"] = "Tools (&T)",
                    ["Main.ColorPicker"] = "Color Picker",
                    ["Main.SettingsMenu"] = "Settings...",
                    ["Main.Ruler"] = "Screen ruler (width / height)",
                    ["Main.SaveImage"] = "Save image",
                    ["Main.SaveBitmap"] = "Save image as bitmap...",
                    ["Main.CopyClipboard"] = "Copy image to clipboard",
                    ["Main.Print"] = "Quick print...",
                    ["Main.Extensions"] = "Extensions (&X)",
                    ["Main.SurfaceLayer"] = "Surface layer",
                    ["Main.SurfaceDrawMode"] = "Surface layer drawing mode ON/OFF",
                    ["Main.SurfaceVisible"] = "Show / hide surface layer",
                    ["Main.SurfaceClear"] = "Clear surface layer",
                    ["Main.Help"] = "Help (&H)",
                    ["Main.OpenHelp"] = "Open help",
                    ["Main.About"] = "About",
                    ["Main.ZoomInButton"] = "Zoom in",
                    ["Main.ZoomOutButton"] = "Zoom out",
                    ["Main.ResetZoom"] = "Reset zoom",
                    ["Main.FlipHButton"] = "Flip H",
                    ["Main.FlipVButton"] = "Flip V",
                    ["Main.RotateLeft"] = "Rotate left",
                    ["Main.RotateRight"] = "Rotate right",
                    ["Main.ResetRotation"] = "Reset rotation",
                    ["Main.RulerButton"] = "Ruler",
                    ["Main.SettingsButton"] = "Settings",
                    ["Main.Hotkeys"] = "Hotkeys",
                    ["Main.GridButton"] = "Grid",
                    ["Main.Clear"] = "Clear",
                    ["Main.Graph"] = "Grid lines",
                    ["Main.Graph30"] = "30 px lines",
                    ["Main.Graph40"] = "40 px lines",
                    ["Main.Graph50"] = "50 px lines",
                    ["Main.Graph60"] = "60 px lines",
                    ["Main.Custom"] = "Custom",
                    ["Main.CenterLine"] = "Center lines",
                    ["Main.WindowPreset"] = "Window size presets",
                    ["Main.LayerStatusOff"] = "Layer: off",
                    ["Message.TargetTitle"] = "Target selection",
                    ["Message.TargetNotFound"] = "Could not get the target",
                    ["Message.TargetSelf"] = "StretchView itself is selected",
                    ["Message.ValueOutOfRange"] = "The value is too large or too small",
                    ["Message.Title"] = "Message",
                    ["Message.InvalidNumber"] = "Invalid number",
                    ["Message.OutOfRangeNumber"] = "The entered value is out of range",
                    ["Message.SaveImageFailed"] = "Failed to save image:",
                    ["Message.CopyClipboardFailed"] = "Failed to copy to clipboard:",
                    ["Message.PrintFailed"] = "Failed to print:",
                    ["Message.HelpFileNotFound"] = "Help file was not found.",
                    ["Message.OpenHelpFailed"] = "Could not open help.",
                    ["Input.RotateTitle"] = "Custom rotation",
                    ["Input.RotatePrompt"] = "Enter an angle (1-359)",
                    ["Input.WidthPrompt"] = "Enter width size (50-400)",
                    ["Input.HeightPrompt"] = "Enter height size (50-400)",
                    ["Input.SamplingTitle"] = "Sampling rate",
                    ["Input.SamplingPrompt"] = "Enter the sampling rate\r\n(Unit: ms, default 400, range 10-400)",
                    ["Input.GridTitle"] = "Grid interval",
                    ["Input.GridPrompt"] = "Enter the interval (pixels)",
                    ["Settings.Title"] = "Settings",
                    ["Settings.StartMenu"] = "Register in Start menu",
                    ["Settings.HotkeysEnabled"] = "Enable hotkeys",
                    ["Settings.RestoreFixViewOnStartup"] = "Restore selection range on startup",
                    ["Settings.SamplingRate"] = "Sampling rate (ms)",
                    ["Settings.Language"] = "Display language",
                    ["Settings.LanguageJapanese"] = "Japanese",
                    ["Settings.LanguageEnglish"] = "English",
                    ["Settings.Cancel"] = "Cancel",
                    ["Help.Title"] = "Help",
                    ["Help.Basic"] = "Basic operations",
                    ["Help.Mouse"] = "Use the mouse to operate",
                    ["Help.Menu"] = "Choose features from the menu",
                    ["Help.Shortcuts"] = "Shortcut keys",
                    ["Help.ZoomUp"] = "Ctrl+A: Zoom in",
                    ["Help.ZoomDown"] = "Ctrl+S: Zoom out",
                    ["Help.FlipH"] = "Ctrl+D: Flip horizontally",
                    ["Help.FlipV"] = "Ctrl+F: Flip vertically",
                    ["Help.FixedMode"] = "Fixed display mode",
                    ["Help.FixedText"] = "After selecting a target range, StretchView keeps that screen area fixed in the viewer.\r\nUse the Selection Range menu or checkbox to select or clear the range.",
                    ["Version.Title"] = "About",
                    ["Version.Label"] = "Version:",
                    ["Version.Unknown"] = "Version: Unknown",
                    ["Version.Description"] = "Description:",
                    ["Ruler.Title"] = "Screen ruler - Size",
                    ["Ruler.EmptySize"] = "Width --- px  x  Height --- px",
                    ["Ruler.WidthLabel"] = "Width",
                    ["Ruler.HeightLabel"] = "Height",
                    ["Ruler.Instruction"] = "Drag with left button to select a range. Esc/right-click to exit.",
                    ["Ruler.ExitInstruction"] = "Esc or right-click to exit",
                    ["Ruler.StartInstruction"] = "Drag with left button to measure a range",
                    ["ColorPicker.CopiedTitle"] = "Color copied",
                    ["ColorPicker.CopiedBody"] = "The color code has been copied to the clipboard.",
                    ["ColorPicker.InfoTitle"] = "Color Picker Info",
                    ["ColorPicker.PositionLabel"] = "Position",
                    ["ColorPicker.HexLabel"] = "HEX",
                    ["ColorPicker.RgbLabel"] = "RGB",
                    ["ColorPicker.InfoInstruction"] = "Left-click to copy / Esc to exit",
                },
            };

        public static string CurrentLanguage { get; private set; } = Japanese;

        public static string NormalizeLanguage(string language)
        {
            if (language == Japanese || language == English)
            {
                return language;
            }

            throw new ArgumentException("Unsupported language: " + language, nameof(language));
        }

        public static string DetectDefaultLanguage()
        {
            string twoLetterName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            if (twoLetterName == Japanese)
            {
                return Japanese;
            }

            return English;
        }

        public static void SetLanguage(string language)
        {
            CurrentLanguage = NormalizeLanguage(language);
        }

        public static string Text(string key)
        {
            Dictionary<string, string> languageResources = Resources[CurrentLanguage];
            if (!languageResources.TryGetValue(key, out string value))
            {
                throw new KeyNotFoundException("Localization key was not found: " + key);
            }

            return value;
        }
    }
}
