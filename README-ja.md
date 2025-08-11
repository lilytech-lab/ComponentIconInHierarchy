# Component Icon In Hierarchy

[![openupm](https://img.shields.io/npm/v/com.lilytech-lab.component-icon-in-hierarchy?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.lilytech-lab.component-icon-in-hierarchy/)  [![Sponsor me on GitHub](https://img.shields.io/badge/Sponsor-❤️-ff69b4)](https://github.com/sponsors/lilytech-lab)  

[README in English](README.md)

下記の要望を満たすためのUnity Editor拡張です。

- 多階層構造のプレハブなどでは、「Colliderが付与されているGameObjectってどれ？」等よく探すことになるので、ヒエラルキーウィンドウのみで付与されているコンポーネントを判断したい。
- GameObjectに付与したスクリプトや、インスペクタウィンドウで設定したマテリアルや他のGameObjectへの参照がリンク切れになって「Missing」という表示になってしまったときに少しでも早く気付きたい。

![動作イメージ](https://github.com/user-attachments/assets/9d4712e0-a645-41cb-9258-5fd4f07e62ef)

<!-- @import "[TOC]" {cmd="toc" depthFrom=2 depthTo=6 orderedList=false} -->

<!-- code_chunk_output -->

- [📦 インストール方法](#-インストール方法)
  - [Unity Package Manager](#unity-package-manager)
  - [Open UPM](#open-upm)
  - [ファイルダウンロード](#ファイルダウンロード)
- [⚙️ 機能](#️-機能)
  - [コンポーネントのアイコン](#コンポーネントのアイコン)
  - [Missingアイコン](#missingアイコン)
  - [表示切り替え](#表示切り替え)
  - [Missingエラーメッセージ表示頻度](#missingエラーメッセージ表示頻度)
- [🔖 動作確認バージョン](#-動作確認バージョン)
- [📗 参考記事](#-参考記事)
- [👥 Help & Contribute](#-help--contribute)
- [🏢 Author Info](#-author-info)
- [📄 License](#-license)

<!-- /code_chunk_output -->

## 📦 インストール方法

### Unity Package Manager

PackageManagerにて「Install package from git URL」を選択し、URL欄に "[https://github.com/lilytech-lab/ComponentIconInHierarchy.git](https://github.com/lilytech-lab/ComponentIconInHierarchy.git)" と入力してください。

### Open UPM

[Open UPM](https://openupm.com/packages/com.lilytech-lab.component-icon-in-hierarchy/) にも対応しています。

```sh
openupm add com.lilytech-lab.component-icon-in-hierarchy
```

### ファイルダウンロード

[Releases](https://github.com/lilytech-lab/ComponentIconInHierarchy/releases) からダウンロードが可能です。

## ⚙️ 機能

### コンポーネントのアイコン

- コンポーネントの付与されている順に、ヒエラルキーウィンドウのGameObject名の右側にコンポーネントのアイコンを右揃えで表示します。
  - Transform、RectTransformのアイコンは除きます。
  - Scriptは複数付与されていてもアイコンは1つのみ表示します。
  - コンポーネントが非活性の場合は半透明でアイコンを表示します。

### Missingアイコン

- ヒエラルキーウィンドウに表示しているGameObjectに以下のものが存在する場合、対象GameObjectの行にワーニングアイコンを表示します。
  - 付与されているコンポーネントにMissingなものが存在する場合
    - GameObject単位で最初に発見したMissingコンポーネントに対してアイコンを表示
  - SerializeField設定項目の中にMissingなものが存在する場合
- 同時にコンソールにエラーメッセージで以下の情報を表示します。
  - GameObjectにMissingなコンポーネントが存在すること
  - コンポーネント単位で最初に発見したリンク切れ情報

### 表示切り替え

ヒエラルキーウィンドウのコンテキストメニューにコンポーネントアイコン、Missingアイコンそれぞれの表示/非表示を切り替えるメニューがあります。

![コンテキストメニュー](https://github.com/user-attachments/assets/79eb2c3d-0b3c-46dc-97da-ce087d01211f)

メニューバーの「GameObject」メニューの方にも同じメニューがあり、こちらではチェック状況で表示設定を確認できます。

![メニューバーのメニュー](https://github.com/user-attachments/assets/3678d076-fa0f-428c-b7d0-d92b8bf885ce)

メニューバーから表示を切り替えた場合は反映まで少し時間がかかることがあるようです。

### Missingエラーメッセージ表示頻度

`EditorApplication.hierarchyWindowItemOnGUI` イベントでエラーメッセージの表示を行なっているので、ヒエラルキービューの再描画処理が走る都度新しくエラーが出力されます。  
これがかなりの頻度なので、ヒエラルキーウィンドウを操作しているとすぐにこんな状態になってしまいます。

![エラーメッセージ出過ぎ](https://github.com/user-attachments/assets/54071315-52e4-42f7-aa21-a43e09f8bc0b)

もう少し控えめでも良いのですが、これだけ煩かったらすぐに解消したくなるからまあいいか、とご容赦いただければ…。

## 🔖 動作確認バージョン

下記のUnityのバージョンで動作確認しました。

- 2020.3.5f1
- 6000.0.29f1

## 📗 参考記事

下記の記事にソースコードのポイントなどを記しました。

- [【Unity】ヒエラルキーウィンドウにコンポーネントのアイコンを表示するEditor拡張](https://qiita.com/masamin/items/3c34c85cb3872ff10924)
- [【Unity】Missingチェックを行うEditor拡張](https://qiita.com/masamin/items/78ba7238dae7aacdc28c#comment-fd6816bf8e0241f0ff47)

## 👥 Help & Contribute

改善案を思い付いた方はPullRequestでお知らせいただけるとありがたいです。

## 🏢 Author Info

有限会社リリテックラボ  
https://www.lilytech-lab.com/  
[https://github.com/lilytech-lab](https://github.com/lilytech-lab)

大阪で小規模なITエンジニアリングを行っています。

もしお役に立てたなら [こちら](https://github.com/sponsors/lilytech-lab) からサポートいただけると大変喜びます。

## 📄 License

MITライセンス
