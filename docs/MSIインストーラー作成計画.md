# MSIインストーラー作成計画

## 目的

`StretchViewCS` の Windows 版について、Visual Studio から手動で作成・保守できる `MSI` インストーラーの作成方針を整理する。

本計画は、既存の `WinForms + .NET Framework 4.8` 構成を前提とし、配布方式は `Microsoft Visual Studio Installer Projects` 拡張を使った `Setup Project` を第一候補とする。

## 前提

- 対象アプリは `C:\repos\StretchViewCS\StretchViewCS\StretchViewCS\StretchViewCS.csproj` の `net48` WinForms アプリである。
- アセンブリ名は `StretchViewCS` である。
- 出力フォルダには `StretchViewCS.exe` に加えて `StretchView.exe` が混在する場合があるため、配布対象は明示的に選別する必要がある。
- `dotnet build` ではなく、Visual Studio 付属の `MSBuild.exe` または Visual Studio 本体でのビルド確認を前提とする。

## インストーラー方式

第一候補:

- `Microsoft Visual Studio Installer Projects` による `Setup Project`

採用理由:

- `.NET Framework 4.8` の既存 WinForms アプリに合わせやすい。
- Visual Studio 上で手動保守しやすい。
- `MSI` と `Setup.exe` の両方を生成できる。
- スタートメニュー登録やアンインストールを標準的な Windows アプリ配布として扱える。

今回は `MSIX` は第一候補にしない。

理由:

- 現状は Windows 専用デスクトップアプリであり、既存構成を大きく変えずに配布するには `MSI` のほうが現実的である。
- `MSIX` は将来検討してもよいが、現段階では導入コストに対して効果が小さい。

## 配布対象ファイル

現時点で優先して含める対象:

- `StretchViewCS.exe`
- `StretchViewCS.exe.config`
- `System.Configuration.ConfigurationManager.dll`

必要に応じて含める対象:

- `StretchView.hlp`
- アプリで外部ファイルとして参照している画像や設定テンプレート

原則として含めない対象:

- `StretchView.exe`
- `StretchView.pdb`
- `StretchViewCS.pdb`
- 開発中のみ使う一時ファイル
- `Thumbs.db`

## 配布対象の判断メモ

### StretchViewCS.exe を正式対象にする

`StretchViewCS.csproj` の `AssemblyName` は `StretchViewCS` であるため、正式な配布対象は `StretchViewCS.exe` とみなす。

出力フォルダにある `StretchView.exe` は、現時点では旧成果物または別構成の残存物として扱い、インストーラーに含めない。

### PDB は通常配布から除外する

通常利用者向けの配布では `.pdb` は不要であるため除外する。

必要なら、障害解析用の社内配布や開発者向けパッケージを別途用意する。

### StretchView.hlp は非推奨寄りの要判断

`frmCap.cs` では `Application.StartupPath` 配下の `StretchView.hlp` を探して開く実装がある。

そのため、ヘルプ機能を有効に保つなら、インストーラーで実行ファイルと同じフォルダに `StretchView.hlp` を配置する必要がある。

ただし現在のリポジトリでは、`StretchView.hlp` は `src_Delphi` 配下に存在し、`csproj` で自動コピー設定されていない。

Microsoft のサポート情報では、Windows Help は Windows 10 と Windows Server 2012 以降で非サポートであり、開発者に対して `.hlp` から `CHM`、`HTML`、`XML` などへの移行を強く推奨している。

そのため、現時点の推奨方針は次の通りとする。

- 新規インストーラーでは `.hlp` を既定で同梱しない。
- ヘルプが必要な場合は、まず `HTML` ベースの代替ドキュメントへ移行する。
- `.hlp` を暫定同梱する場合でも、正式な長期運用形式とはみなさない。

判断ポイント:

- Windows ヘルプファイルを暫定配布するか
- ヘルプ機能を未提供扱いにするか
- 将来 `html`、`chm`、`pdf` のどれに置き換えるか

この点は、インストーラー作成前にユーザー判断が必要である。

## スタートメニュー登録の扱い

`frmSetting.cs` には、アプリ実行中にスタートメニュー登録を行う機能が残っている。

インストーラー側でもショートカットを作る場合、登録経路が二重になる可能性がある。

そのため、初期方針は次の通りとする。

- インストーラー側でスタートメニューショートカットを作成する。
- アプリ側の「スタートメニューに登録」機能は当面そのまま残す。
- ただし運用上は二重管理になるため、将来的にはどちらを正式経路にするか整理する。

## Visual Studio での作成手順

### 1. 拡張をインストールする

Visual Studio に `Microsoft Visual Studio Installer Projects` 拡張を入れる。

### 2. Setup Project を追加する

ソリューションに新規プロジェクトとして `Setup Project` を追加する。

想定プロジェクト名:

- `StretchViewCS.Setup`

### 3. Application Folder に Primary output を追加する

セットアッププロジェクトの `Application Folder` に対して、`StretchViewCS` プロジェクトの `Primary output` を追加する。

これにより、主実行ファイルと参照 DLL をまとめて取り込める。

### 4. 明示的に同梱するファイルを追加する

必要に応じて次を `Application Folder` に追加する。

- `StretchViewCS.exe.config`
- `StretchView.hlp`

注意:

- `StretchView.hlp` を追加する場合は、リポジトリ上のどのファイルを正式採用するか決めてから登録する。
- `Thumbs.db` や旧成果物の `StretchView.exe` は追加しない。

### 5. ショートカットを作成する

`Primary output` からショートカットを作り、以下に配置する。

- `User's Programs Menu`
- 任意選択で `User's Desktop`

表示名は `StretchViewCS` を基本とする。

デスクトップショートカットは、インストール時オプションとして選べる構成を目標にする。

初期方針:

- スタートメニューショートカットは既定で作成する。
- デスクトップショートカットは既定で未選択にする。
- 利用者が明示的に選んだ場合だけ作成する。

注意:

- `Setup Project` 単体では、任意チェック付き UI の作り込みに制約がある可能性がある。
- まずは `User Interface` エディタで実現可能か確認する。
- 難しい場合は、インストーラー方式を `WiX Toolset` や `Inno Setup` まで広げて再評価する。

### 6. アイコンを設定する

必要に応じて `appIcon.ico` をショートカットや製品アイコンに割り当てる。

### 7. 製品情報を設定する

最低限、以下を設定する。

- `ProductName`
- `Manufacturer`
- `Version`
- `Author`
- `Title`

`Version` を上げる場合、Windows Installer 上の更新動作に影響するため、ルールを事前に決める。

### 8. ビルドして生成物を確認する

生成物として少なくとも以下を確認する。

- `Setup.exe`
- `.msi`

## テスト観点

最低限、次を確認する。

### インストール

- 管理者権限不要の標準インストールで完了するか
- インストール先に `StretchViewCS.exe` と必要ファイルだけが配置されるか
- 不要な `StretchView.exe` が混入していないか

### 起動

- スタートメニューショートカットから起動できるか
- 実行ファイル直接起動と同じ動作になるか
- 初回起動時に設定ファイル生成で失敗しないか

### 機能

- 画面キャプチャ
- 保存
- 印刷ダイアログ
- 設定画面
- ホットキー有効化後の動作
- ヘルプ起動

### アンインストール

- コントロールパネルから削除できるか
- スタートメニューショートカットが除去されるか
- デスクトップショートカットを作成した場合に除去されるか
- 実行ファイル本体が削除されるか

## リスク

### ヘルプファイル未整理

`StretchView.hlp` は複数箇所に存在しており、どれを正式採用するか未確定である。

加えて、`.hlp` 自体が現在の Windows 利用環境に適さない可能性が高い。

### 旧成果物混入

ビルド出力フォルダの `StretchView.exe` を誤って同梱すると、配布物の構成が不明瞭になる。

### スタートメニュー登録の二重化

インストーラーとアプリ内機能の両方でショートカットを扱うため、将来的に整理が必要である。

### 更新手順未確定

製品バージョンを変更したときのアップグレード手順や旧版からの上書き方針は、まだ決まっていない。

## 作成前に決めること

1. 配布名を `StretchViewCS` で統一するか。
2. `StretchView.hlp` を暫定同梱するか、それとも代替ヘルプへ移行するか。
3. デスクトップショートカットを任意選択にするか。
4. アプリ内のスタートメニュー登録機能を今後どう扱うか。
5. バージョン更新時に上書き更新を許可するか。

## 次に行う作業

1. Visual Studio に `Microsoft Visual Studio Installer Projects` が入っているか確認する。
2. `StretchViewCS.Setup` を追加する。
3. `Primary output` と必須ファイルを登録する。
4. デスクトップショートカットを任意選択にできるか確認する。
5. `StretchView.hlp` を同梱しない前提でヘルプ方針を決める。
6. `Debug` または `Release` 生成物からテストインストールを行う。
