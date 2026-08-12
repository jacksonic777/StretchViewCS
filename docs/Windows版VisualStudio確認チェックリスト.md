# Windows版 Visual Studio 確認チェックリスト

## 目的

StretchViewCS の Windows 版を `Visual Studio` で安定してビルドし、主要機能を手動で確認するためのチェックリストを残す。

本プロジェクトは `net48`、`WinForms`、`COMReference`、`Win32 API` を利用しているため、確認環境は `Visual Studio` を前提とする。

## 前提

- OS は Windows 10 または Windows 11
- `Visual Studio 2022` が導入済み
- `.NET Framework 4.8` のビルド環境が導入済み
- ソリューションは [StretchViewCS.sln](/C:/repos/StretchViewCS/StretchViewCS.sln) を使用する
- プロジェクト本体は [StretchViewCS.csproj](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/StretchViewCS.csproj:1)

## Visual Studio を優先する理由

- `dotnet build` では `COMReference` 解決に失敗する
- `WinForms` の実画面確認がしやすい
- デバッガで UI イベントや例外を追いやすい
- `CopyFromScreen`、ホットキー、印刷、クリップボードなどの確認に向いている

## 実行前チェック

- 作業ツリーに意図しない変更がないか確認する
- `Debug|Any CPU` で開いているか確認する
- スタートアッププロジェクトが `StretchViewCS` になっているか確認する
- 以前の実行プロセスが残っていないか確認する
- セキュリティソフトや権限制御で画面キャプチャが阻害されていないか確認する

## ビルド確認

### 最低限確認すること

- ソリューションを `Build` する
- `0 errors` で完了することを確認する
- 警告が増えていないか確認する
- 出力先に exe が生成されることを確認する

### 出力先

- `StretchViewCS\\StretchViewCS\\bin\\Debug\\net48\\StretchViewCS.exe`

### 確認観点

- 参照解決エラーが出ていない
- `IWshRuntimeLibrary` 関連で失敗していない
- リソース生成エラーが出ていない
- 日本語を含むリソースや文字列が壊れていない

## 起動確認

- `F5` で起動できる
- 起動直後にアプリが落ちない
- メイン画面が表示される
- ウィンドウサイズと位置が不自然でない
- タイトルバーやメニューが正しく表示される

## 基本表示確認

- メイン画面上で拡大表示が更新される
- マウス移動に追従して表示が更新される
- 表示が極端にちらつかない
- ウィンドウ移動、最小化、復元で異常がない
- 最前面表示が意図どおりに動く

## メニューとツールバー確認

- 各メニューが開ける
- ツールバーのボタンが押せる
- メニューとツールバーの状態が一致している
- チェック付き項目の ON/OFF が表示に反映される

## 拡大率確認

- 拡大率変更メニューが動く
- ツールバーの拡大、縮小が動く
- 連続操作で異常終了しない
- 極端な倍率で描画が破綻しない

## 回転、反転確認

- 左右反転が動く
- 上下反転が動く
- 回転が動く
- 回転後も描画位置が大きく崩れない
- リセット操作で状態が戻る

## 固定表示確認

- 範囲指定オーバーレイが表示される
- 範囲指定後に固定表示へ移行できる
- 固定表示解除ができる
- 固定中に位置や表示が不自然に飛ばない

## グリッド、補助表示確認

- グリッド表示の ON/OFF が動く
- グリッド間隔変更が反映される
- 中心線などの補助表示が意図どおりに出る

## 保存、クリップボード、印刷確認

- 画像保存ダイアログが開く
- 保存した画像が開ける
- 保存画像の内容が表示領域と一致する
- クリップボードコピー後にペイント等へ貼り付けできる
- 印刷ダイアログが開く
- プリンタ未接続時でも異常終了しない

## 設定確認

- 設定画面が開く
- スタートメニュー登録の ON/OFF が動く
- 設定反映後にアプリが異常終了しない
- 終了後、再起動して設定が復元される

## 補助機能確認

- カラーピッカーが開く
- 画面上の色を取得できる
- 色コードがクリップボードへコピーされる
- 定規機能が開く
- ドラッグしたサイズが表示される

## ホットキー確認

- アプリ起動中にホットキーが反応する
- 他アプリとの衝突がある場合に異常終了しない
- 最小化、復元後も登録解除と再登録が破綻しない

## エラー観察ポイント

- 例外ダイアログが出ていないか
- 何も起きない操作がないか
- 操作後に UI が固まらないか
- 高頻度操作で CPU 使用率が急上昇しないか
- 複数回の起動と終了で設定ファイルが壊れないか

## デバッグ時に確認するとよい設定

- `Exception Settings` で共通言語ランタイム例外を確認できるようにする
- `Output` ウィンドウでビルドログを確認する
- 問題箇所は `frmCap.cs`、`Win32API.cs`、`IniManager.cs` を優先して追う

## 特に注意して見るべき箇所

- [frmCap.cs](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmCap.cs:1)
- [Win32API.cs](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Native/Win32API.cs:1)
- [IniManager.cs](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Utils/IniManager.cs:1)
- [frmSetting.cs](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/frmSetting.cs:1)
- [ColorPickerForm.cs](/C:/repos/StretchViewCS/StretchViewCS/StretchViewCS/Forms/ColorPickerForm.cs:1)
- `RulerForm.cs` は画面定規機能の廃止によりビルド対象外

## 確認結果の残し方

毎回、少なくとも以下を記録する。

- 確認日
- 使用した Visual Studio のバージョン
- 使用したビルド構成
- エラー数、警告数
- 確認した機能
- 未確認の機能
- 再現した不具合
- 再現手順

## 推奨する運用

- まず `Build`
- 次に `F5` で通常起動
- 主要機能を上から順に手動確認
- 問題が出たらその場で再現条件を固定
- 確認結果を `docs/` または作業メモへ残す

## 次に整備したいもの

- Windows 版の回帰確認表
- 不具合報告テンプレート
- `frmCap` の責務分解メモ
- Visual Studio 用の確認シナリオ一覧
