# Windows版の多言語対応

Windows版の表示言語は、`StretchViewCS/StretchViewCS/Utils/LocalizationManager.cs` で管理する。

## 対応言語

- `ja`: 日本語
- `en`: 英語

## 設定保存

表示言語はINIファイルの `Setting` セクションに `Language` として保存する。

```ini
[Setting]
Language=ja
```

初回起動時は、WindowsのUIカルチャが日本語の場合は `ja`、それ以外は `en` を使用する。

Inno Setup インストーラーから導入した場合は、インストーラーで選択した言語をインストール先の `StretchViewCS.install.ini` に保存する。
ユーザーINIに `Language` がまだ存在しない場合は、このインストール時初期値をアプリの表示言語として使用する。
ユーザーINIに `Language` がある場合は、ユーザーINIを優先する。

## 文言追加ルール

- UI文言は `LocalizationManager.Text("Key.Name")` で取得する。
- 日本語と英語の両方に同じキーを追加する。
- キーが存在しない場合は例外を出し、未翻訳を見落とさないようにする。
- 既存フォームのDesignerに残る日本語初期値は、フォーム生成後の `ApplyLocalization()` で上書きする。

## 現在の接続範囲

- メインフォームのメニュー、ツールバー、主要チェックボックス
- 設定画面の表示言語選択
- ヘルプ画面
- バージョン情報画面
- 画面定規の案内表示（画面定規機能は現行版では廃止済み）
- カラーピッカーの完了メッセージ
