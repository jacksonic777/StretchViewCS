# frmCap責務分解メモ

## 目的

[frmCap.cs](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1) に集中している責務を整理し、以下の 2 つに役立てる。

- Windows 版を完成させる際の確認、改修の優先順位付け
- 将来の macOS 対応やクロスプラットフォーム化に向けた分離設計

## 結論

`frmCap` は単なるメイン画面ではなく、以下をまとめて抱えている。

- 画面キャプチャ処理
- 描画更新処理
- ズーム、回転、反転の状態管理
- グローバルホットキー処理
- マウス入力の中継
- 対象ウィンドウ固定処理
- 範囲指定
- 当たり判定レイヤ
- 画像保存、クリップボード、印刷
- メニュー、ツールバーの UI 配線
- 設定保存と復元の呼び出し

このため、`frmCap` は「フォーム」でありながら、実質的にはアプリケーション本体のオーケストレータ兼、Windows 固有機能の集約場所になっている。

## 大分類

### 1. ライフサイクル管理

主なメソッド:

- [frmCap()](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:102)
- [frmCap_Load](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:119)
- [frmCap_FormClosing](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:170)
- [frmCap_FormDestroy](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:193)
- [frmCap_Resize](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1390)
- [FormActivate](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:2119)
- [MyOnMinimize](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:2124)
- [MyOnRestore](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:2130)

役割:

- 初期値設定
- INI からの設定復元
- タイマー起動
- ホットキー登録、解除
- 終了時の設定保存
- 最小化、復元時の状態切替

問題点:

- 画面設定、ホットキー、表示更新準備がフォーム初期化に密結合
- 設定保存の責務が UI イベントに直結している

### 2. 画面キャプチャと描画更新

主なメソッド:

- [tim_Tick](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:200)
- [CaptureAndDisplay](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:281)
- [DrawOptions](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:396)
- [frmCap_Paint](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:2307)

役割:

- マウス位置の取得
- キャプチャ矩形の計算
- `BitBlt` による画面取得
- 拡大、回転、反転後のビットマップ生成
- グリッド、クロス、選択枠などのオーバーレイ描画
- 表示用ビットマップの更新と再描画要求

依存:

- `Win32API.GetWindowDC`
- `Win32API.BitBlt`
- `Screen.PrimaryScreen`
- `XRotateBitmap`

問題点:

- 画面取得、画像変換、描画状態更新が 1 本の流れで強結合
- Windows 依存が強く、UI と同じクラスにある
- 高頻度タイマー処理のため、負荷観点の影響が大きい

### 3. ホットキー処理

主なメソッド:

- [WndProc](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:459)
- [HandleHotKey](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:469)
- [RegisterMyHotkeys](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:532)
- [RegisterHotKeyOne](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:564)
- [UnregisterMyHotkeys](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:576)

役割:

- グローバルホットキーの登録
- `WM_HOTKEY` 受信
- 各操作へのディスパッチ

依存:

- `Win32API.RegisterHotKey`
- `Win32API.UnregisterHotKey`
- `WndProc`

問題点:

- Windows 固有機能
- ホットキー入力が直接 UI 操作に結びついている
- 今後のクロスプラットフォーム化では抽象化必須

### 4. マウス入力と操作投影

主なメソッド:

- [frmCap_MouseDown](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:611)
- [frmCap_MouseMove](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:649)
- [HandleMouseCapture](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:699)
- [frmCap_MouseUp](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:818)
- [CalculateXY](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:922)
- [TranslateXY](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:952)

役割:

- メイン画面上のマウス操作を受ける
- 固定表示範囲の選択
- 操作投影モード時のターゲット選択
- ターゲットウィンドウへの座標変換
- 仮想マウス入力の送出

依存:

- `mouse_event`
- `PostMessage`
- `ScreenToClient`
- 対象ウィンドウ情報

問題点:

- 最も Windows 依存が強い責務の 1 つ
- UI マウスイベント、座標変換、他アプリ操作が同居している
- 将来的な macOS 対応では仕様差が出やすい

### 5. ウィンドウ操作とターゲット追跡

主なメソッド:

- [GetTargetWindow](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1024)
- [SetAbsoluteForegroundWindow](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1050)
- [FixMode](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1080)
- [FixModeByKey](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1162)
- [FixView](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1437)

役割:

- カーソル下ウィンドウの取得
- 対象ウィンドウの前面化
- ウィンドウ固定モード
- 範囲中心での固定表示

依存:

- `WindowFromPoint`
- `ChildWindowFromPoint`
- `GetWindowRect`
- `SetForegroundWindow`

問題点:

- Windows API の前提が強い
- 「対象ウィンドウ固定」と「画面上の固定表示」が同じ文脈で混在している

### 6. 表示状態の変更

主なメソッド:

- [MoveRange](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1195)
- [ChgCapRate](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1222)
- [RotateAngle](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1252)
- [FlipHV](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1268)
- [UpdateTransFromClientSize](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1310)
- [ChgWindowSize](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1336)
- [SwitchTopMost](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1417)
- [UpdateCaption](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1472)

役割:

- 表示倍率変更
- 回転、反転状態変更
- 固定範囲移動
- ウィンドウサイズ変更
- 最前面切替
- タイトル表示更新

問題点:

- 本来は共通状態として整理できる部分が多い
- UI チェック状態の更新と内部状態更新が混在している

### 7. 当たり判定モード

主なメソッド:

- [AtariMode](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1534)
- [MmAtariModeClick](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1937)
- [MmBltAtariClick](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1947)
- [MmClearAtariClick](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1956)

役割:

- 当たり判定レイヤの有効化
- レイヤ表示、非表示
- レイヤクリア

問題点:

- 機能自体は独立性が高いが、ビットマップ管理が `frmCap` に埋め込まれている

### 8. 外部出力と補助機能

主なメソッド:

- [MmSaveViewFileClick](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:2029)
- [MmCopyToClipBoardClick](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:2053)
- [MmPrintClick](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:2072)
- [OpenColorPicker](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:2003)
- [OpenRuler](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:2021)

役割:

- 画像保存
- クリップボードコピー
- 印刷
- カラーピッカー起動
- 定規起動

問題点:

- 保存や印刷の責務は将来的にサービス化しやすい
- 補助フォーム起動は UI 層に残してよいが、処理本体は分離できる

### 9. メニュー、ツールバー、イベント配線

主なメソッド:

- `MmRate*Click`
- `MmFlip*Click`
- `MmGraph*Click`
- `Tb*Click`
- `Tb*MouseDown`
- `Tb*MouseUp`
- [MmFixViewSwClick](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:2250)
- [MmRangeClick](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:2351)

役割:

- メニュー操作を内部機能へ接続
- ツールバー操作を内部機能へ接続
- チェック状態の同期

問題点:

- イベントハンドラ数が多く、可読性を落としている
- 本体ロジックと UI 配線が同じファイルに混ざっている

## 現時点での責務整理

### `frmCap` に残してよいもの

- WinForms の画面イベント受信
- 子フォームの起動
- 描画要求の最終受け口
- UI コントロールの見た目更新

### 分離優先度が高いもの

1. 画面キャプチャと描画パイプライン
2. ホットキー管理
3. 操作投影、入力中継
4. 固定表示とターゲット追跡
5. 表示状態モデル

### 比較的後回しにできるもの

- メニューイベントの整理
- ヘルプ、バージョン、ライセンス起動
- 補助ダイアログ起動配線

## Windows版完成に向けた優先視点

Windows 版を先に仕上げる場合、まず確認すべき責務は以下。

1. 画面キャプチャと描画更新
2. 固定表示
3. ホットキー
4. 保存、クリップボード、印刷
5. 当たり判定と補助表示

理由:

- ユーザー価値の中心に直結している
- Visual Studio 上での手動確認対象と一致している
- 将来分離するときも、この境界がそのまま重要になる

## 将来の分離候補

### 候補1: `CaptureCoordinator`

責務:

- キャプチャ範囲計算
- 画面取得
- 拡大、回転、反転パイプライン
- 表示ビットマップ生成

### 候補2: `ViewportState`

責務:

- 倍率
- 角度
- 左右、上下反転
- 表示範囲
- 固定状態

### 候補3: `HotkeyController`

責務:

- ホットキー登録、解除
- ホットキー操作のマッピング

### 候補4: `TargetWindowController`

責務:

- ターゲット取得
- 固定対象の追跡
- 画面座標とターゲット座標の変換

### 候補5: `InteractionRelayService`

責務:

- マウスイベント中継
- 他アプリへの操作投影

### 候補6: `ExportService`

責務:

- 画像保存
- クリップボードコピー
- 印刷

## 負荷観点で特に注意すべき箇所

本プロジェクトのルール上、負荷影響のある処理は注意が必要。

特に `frmCap` では以下が該当する。

- [tim_Tick](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:200)
- [timRepeat_Tick](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:442)
- [frmCap_MouseMove](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:649)
- [HandleMouseCapture](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:699)
- [frmCap_Paint](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:2307)

これらは呼び出し頻度が高く、軽微な変更でも描画負荷や応答性に影響しやすい。

## 今後のドキュメント連携

このメモは、以下とセットで使う。

- [Windows版VisualStudio確認チェックリスト.md](/C:/repos/StretchViewCS/docs/Windows版VisualStudio確認チェックリスト.md)
- [Windows版回帰確認表.md](/C:/repos/StretchViewCS/docs/Windows版回帰確認表.md)
- [macOS対応移行計画.md](/C:/repos/StretchViewCS/docs/macOS対応移行計画.md)

## 次に行うべき作業

優先順は以下。

1. `frmCap` の状態変数を分類する
2. 表示更新系と入力中継系の依存を洗い出す
3. Windows 版の修正で触る責務を明示する
4. 共通化候補だけを別メモに切り出す
