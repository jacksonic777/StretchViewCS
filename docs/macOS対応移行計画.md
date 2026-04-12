# macOS対応移行計画

## 目的

StretchViewCS を Windows 専用の `WinForms + Win32 API` 構成から、macOS でも利用可能なアプリケーションへ段階的に移行する。

本ドキュメントは、現状の制約、候補技術、推奨方針、段階的な移行手順を整理し、今後の実装判断の基準を残すことを目的とする。

## 結論

現状の StretchViewCS は、単純な `TargetFramework` の変更や `WinForms` の置き換えだけでは macOS 対応できない。

理由は以下の通り。

- `net48` を前提にしている
- `System.Windows.Forms` に強く依存している
- `user32.dll` / `gdi32.dll` / `shell32.dll` を直接呼び出している
- 画面キャプチャ、グローバルホットキー、他ウィンドウ追跡、入力注入が Windows API 前提で実装されている

移行先の第一候補は `Avalonia + .NET 8/9` とする。

理由は以下の通り。

- デスクトップアプリ向けで、Windows と macOS の両方を現実的に狙える
- 既存の C# ロジックを再利用しやすい
- `.NET MAUI` より本プロジェクトの用途に近い
- `Swift + AppKit` より全面作り直しになりにくい

## 現状の技術的制約

### プロジェクト構成

- 現行プロジェクトは `net48` の Windows Forms アプリ
- 出力種別は `WinExe`
- COM 参照として `IWshRuntimeLibrary` を利用

### Windows 固有依存

主な Windows 依存は以下。

- `BitBlt` による画面キャプチャ
- `GetWindowDC`, `GetWindowRect`, `GetPixel`
- `RegisterHotKey`, `UnregisterHotKey`
- `mouse_event`
- `PostMessage`, `SendMessage`
- `Environment.SpecialFolder.Programs` を使ったスタートメニュー登録
- `Screen.PrimaryScreen`
- `Graphics.CopyFromScreen`

### UI 依存

UI は `frmCap` を中心とした `System.Windows.Forms` 依存で構築されている。

- メイン表示
- 設定画面
- ヘルプ画面
- カラーピッカー
- 定規オーバーレイ
- 範囲指定オーバーレイ

特に `frmCap.cs` に機能が集中しており、UI ロジックと OS 依存処理が強く結合している。

## 候補技術の比較

### 1. Avalonia

概要:
クロスプラットフォームのデスクトップ UI フレームワーク。Windows / macOS / Linux を対象にしやすい。

長所:

- C# を継続利用できる
- Windows / macOS の 2 系統をまとめやすい
- デスクトップアプリの設計と相性が良い
- UI を MVVM 的に整理しやすい

短所:

- WinForms 画面は作り直しになる
- Windows API 呼び出しはそのまま使えない
- macOS 固有処理は別実装が必要

適性:

- 最もバランスが良い
- 将来的に Linux も視野に入れやすい

### 2. Swift + AppKit

概要:
macOS ネイティブ実装として別アプリを作る方針。

長所:

- macOS ネイティブ機能との親和性が高い
- 画面キャプチャや権限処理を素直に実装しやすい
- macOS らしい UI にしやすい

短所:

- C# 資産の再利用が少ない
- Windows 版と別系統で保守する負担が大きい
- 開発体験が現在の .NET ベースから大きく変わる

適性:

- macOS 品質最優先なら有力
- 共通コード維持には向きにくい

### 3. .NET MAUI

概要:
Microsoft 公式のクロスプラットフォーム UI フレームワーク。macOS は `Mac Catalyst` 経由。

長所:

- .NET を継続利用できる
- 公式サポートの選択肢である
- 将来的にモバイル系も視野に入れられる

短所:

- 本件のようなデスクトップ常駐ツールにはやや相性が弱い
- `Mac Catalyst` 前提の制約を受ける
- Windows / macOS の低レベル OS 依存処理は別実装が必要

適性:

- モバイル展開も検討するなら候補
- 本件では第一候補ではない

## 推奨方針

### 推奨案

`Avalonia + .NET 8/9` をベースに、新しいクロスプラットフォーム構成を別プロジェクトとして追加し、既存 Windows 版と並行して段階移行する。

### 推奨理由

- 既存ロジックの再利用余地がある
- UI の作り直しを前提にしても、C# と .NET の知識を継続利用できる
- macOS 対応後も Windows 版の保守を統合しやすい
- 現行実装の Windows 固有コードを、Windows 専用実装として残しやすい

## 移行時の考え方

### 重要な前提

現行コードをそのまま移植するのではなく、以下の 3 層に分解して整理する。

1. 共通ドメインロジック
2. OS 依存サービス
3. UI

### 共通化しやすい領域

- 拡大率の状態管理
- 回転角、反転状態の管理
- 固定表示座標の管理
- グリッドや表示モードの状態管理
- 設定値モデル
- 画像処理の一部ルール

### OS 別実装が必要な領域

- 画面キャプチャ
- グローバルホットキー
- カーソル位置取得
- 他ウィンドウの追跡
- 入力注入
- クリップボード
- 印刷
- ショートカット作成

## 推奨アーキテクチャ

### 構成案

- `src/StretchView.Core`
  - 共通モデル
  - 共通ユースケース
  - 共通設定モデル
- `src/StretchView.Platform.Abstractions`
  - OS 依存機能のインターフェイス
- `src/StretchView.Platform.Windows`
  - Win32 実装
- `src/StretchView.Platform.Mac`
  - macOS 実装
- `src/StretchView.App.Desktop`
  - Avalonia UI

### 抽象化したいインターフェイス例

- `IScreenCaptureService`
- `IGlobalHotkeyService`
- `IInputRelayService`
- `IWindowTrackingService`
- `IClipboardService`
- `IPrintService`
- `ISettingsStore`

## 機能ごとの移行難易度

### 低

- 設定保存
- 拡大率変更
- 回転、反転の状態管理
- グリッド表示
- 画像保存

### 中

- カラーピッカー
- 定規機能
- 範囲選択 UI
- 印刷

### 高

- グローバルホットキー
- 対象ウィンドウ固定
- 他アプリ操作の投影
- 他アプリへの入力注入
- Windows 固有のメッセージ送信ベース機能

## macOS 側で特に注意すべき点

- 画面キャプチャは macOS の権限が必要
- 他アプリ操作は Accessibility 権限が必要
- Windows の `PostMessage` 相当で同じ挙動がそのまま得られるとは限らない
- `Screen.PrimaryScreen` 前提の実装は、複数ディスプレイで見直しが必要
- `System.Drawing` 前提の画像処理は置き換えを検討する

## 段階的な移行計画

### フェーズ1: 調査と土台づくり

- 現行機能を一覧化する
- 各機能を共通化可能か、OS 依存かに分類する
- 新しいソリューション構成を決める
- 共通ライブラリと抽象インターフェイスを定義する

成果物:

- 機能一覧
- 層分割方針
- 新規ソリューション構成

### フェーズ2: 共通ロジック切り出し

- 状態管理と設定モデルを WinForms から分離する
- 画像表示に必要な共通ロジックを切り出す
- Windows 実装をラップする

成果物:

- `StretchView.Core`
- `StretchView.Platform.Abstractions`
- `StretchView.Platform.Windows`

### フェーズ3: 新 UI の最小実装

- Avalonia で最小の表示画面を作る
- 拡大率変更
- 回転、反転
- 設定保存
- 画像保存

成果物:

- Windows 上で動く新 UI の最小版

### フェーズ4: macOS 対応の最小版

- macOS での画面キャプチャ実装
- macOS での表示
- 基本操作の接続

成果物:

- macOS 上で動く最小版
- 拡大表示、回転、反転、保存

### フェーズ5: 高難度機能の移行

- ホットキー
- 定規
- カラーピッカー
- 固定表示
- 他アプリ操作

成果物:

- Windows 版との機能差分一覧
- macOS 版の権限制約を踏まえた仕様確定

## 初期リリースの現実的なスコープ

最初の macOS 版は、以下の機能に絞ることを推奨する。

- 画面拡大
- 回転
- 左右反転、上下反転
- 範囲指定
- グリッド表示
- 画像保存
- カラーピッカー
- 定規

後回し候補は以下。

- 他アプリへの入力注入
- Windows メッセージベースの操作投影
- 完全な同等ホットキー

## 未決定事項

以下は実装前に判断が必要。

- Windows 版との後方互換性をどの範囲まで維持するか
- macOS 版で同等機能を目指すか、仕様差を許容するか
- UI を既存に寄せるか、macOS に合わせて再設計するか
- 現行プロジェクトを改修するか、新規ソリューションを追加するか

## 次に行うべき作業

優先順は以下。

1. 現行機能一覧の作成
2. `frmCap` の責務分解
3. 共通ロジック候補の棚卸し
4. 新規ソリューション構成案の作成
5. Windows 実装と macOS 実装の責務境界の定義

## 当面の推奨判断

現時点では、以下の判断を推奨する。

- 既存 WinForms プロジェクトは維持する
- 新しいクロスプラットフォーム実装は別プロジェクトとして追加する
- 初期リリースでは macOS 版の機能を絞る
- 高難度の他アプリ操作は後段フェーズに分離する

この方針であれば、既存 Windows 版を壊さずに前進できる。
