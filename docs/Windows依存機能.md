# Windows依存機能

## 対象

このドキュメントは、現行のWindows版 `StretchViewCS` に含まれるWindows依存機能を整理する。

## 結論

現行のWindows版は `.NET Framework 4.8` の WinForms アプリであり、複数の機能が Win32 API に依存している。

特に次の機能は、macOS / Linux へそのまま移植できない。

- デスクトップ画面のキャプチャ
- 対象ウィンドウの取得と範囲追跡
- 操作投影モード
- グローバルホットキー
- デスクトップ上の色取得
- デスクトップ再描画
- 最前面制御
- Inno Setup インストーラー

## 廃止済み機能

次の機能は現行版では廃止済みであり、UIと配布ドキュメントから除外している。

- 表面レイヤ機能
- 画面定規機能

表面レイヤの描画バッファや画面定規フォームのソースは履歴参照のため一部残しているが、現行機能としては扱わない。

## 機能別の整理

| 機能 | Windows依存度 | 主な依存箇所 | 移植時の考え方 |
| --- | --- | --- | --- |
| メイン画面表示 | 中 | WinForms / System.Drawing | UIフレームワークをAvalonia等へ置き換える |
| デスクトップキャプチャ | 高 | `GetDesktopWindow`, `GetWindowDC`, `BitBlt`, `ReleaseDC` | OS別の画面キャプチャAPIへ差し替える |
| 対象範囲の選択 | 高 | `WindowFromPoint`, `ChildWindowFromPoint`, `GetWindowRect` | OS別のウィンドウ列挙・座標取得APIが必要 |
| 対象範囲の枠線表示 | 高 | 透明/最前面フォーム、`SetWindowPos` | OS別のオーバーレイウィンドウ実装が必要 |
| 操作投影モード | 高 | `SendMessage`, `mouse_event`, `SetCapture`, `ReleaseCapture` | OSごとの入力イベント注入または別設計が必要 |
| グローバルホットキー | 高 | `RegisterHotKey`, `UnregisterHotKey`, `WM_HOTKEY` | OS別のグローバルショートカット機構が必要 |
| カラーピッカー | 高 | `GetDesktopWindow`, `GetWindowDC`, `GetPixel` | OS別の画面ピクセル取得APIが必要 |
| 最前面表示 | 高 | `SetWindowPos`, `HWND_TOPMOST` | UIフレームワークまたはOS別APIで実装する |
| ヘルプ表示 | 中 | `ShellExecute` | クロスプラットフォームなURL/ファイル起動処理へ置換する |
| スタートメニュー登録 | 高 | Windowsショートカット/COM参照 | macOS/Linuxでは各OSの配布方式へ置換する |
| インストーラー | 高 | Inno Setup | macOSは `.app` / `.dmg`、LinuxはAppImage等を検討する |

## 主なWin32 API

現行コードでは `Native/Win32API.cs` にWindows APIのP/Invoke定義を集約している。

- `gdi32.dll`
  - `BitBlt`
  - `StretchBlt`
  - `GetDeviceCaps`
  - `GetPixel`
  - `CreateCompatibleDC`
  - `SelectObject`
  - `DeleteDC`
  - `DeleteObject`
- `user32.dll`
  - `GetDesktopWindow`
  - `GetWindowDC`
  - `ReleaseDC`
  - `RegisterHotKey`
  - `UnregisterHotKey`
  - `WindowFromPoint`
  - `ChildWindowFromPoint`
  - `GetWindowRect`
  - `GetWindowText`
  - `GetClassName`
  - `SetWindowPos`
  - `SetCursorPos`
  - `SendMessage`
  - `PostMessage`
  - `mouse_event`
  - `SetCapture`
  - `ReleaseCapture`
  - `InvalidateRect`
  - `SystemParametersInfo`
- `shell32.dll`
  - `ShellExecute`
  - `SHGetSpecialFolderLocation`
  - `SHGetPathFromIDList`

## クロスプラットフォーム化時の方針

macOS / Linux 対応を進める場合は、Windows依存機能を一括移植しようとせず、次のように分離する。

- 画像表示、拡大縮小、回転、反転は共通機能として切り出す。
- デスクトップキャプチャ、ウィンドウ取得、入力転送、ホットキーはOS別アダプターに分ける。
- 操作投影モードはOSごとのセキュリティ制約が強いため、最初のクロスプラットフォーム版では対象外または実験機能として扱う。
- インストーラーはOSごとの配布形式に分ける。

## 関連ファイル

- `StretchViewCS/StretchViewCS/Native/Win32API.cs`
- `StretchViewCS/StretchViewCS/Forms/frmCap.cs`
- `StretchViewCS/StretchViewCS/Forms/DesktopSelectionOverlayForm.cs`
- `StretchViewCS/StretchViewCS/Forms/ColorPickerForm.cs`
- `StretchViewCS/StretchViewCS/Forms/RulerForm.cs`（廃止済み、ビルド対象外）
- `installer/StretchViewCS.iss`
