# StretchView.CrossPlatform

`StretchView.CrossPlatform` は、既存の Windows Forms 版とは別系統で進める macOS 対応版の土台です。

## 構成

- `StretchView.CrossPlatform.sln`
- `StretchView.Core`
  - OS に依存しない状態管理や設定モデルを置く
- `StretchView.App.Desktop`
  - Avalonia ベースの Windows / macOS 向けデスクトップ UI

## 方針

- 既存の WinForms 版は壊さず残す
- Windows API 前提の機能は新アプリへ直接持ち込まない
- まずは小さな MVP から始める
- 操作投影や他ウィンドウ操作は対象外にする

## 初期 MVP 候補

- 画像/画面キャプチャの表示
- 拡大縮小
- 回転、反転
- グリッド表示
- 画像保存
- アプリ内注釈レイヤ

## ビルド

```powershell
dotnet build .\StretchView.CrossPlatform.sln
```
