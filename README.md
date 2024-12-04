# Component Icon In Hierarchy

[日本語版ReadMeはこちら](README-ja.md)

An Unity editor application that helps following requests:

- As we often say something like, "Where is the GameObjet which attached the Collider?" when the game object has many child game objects, it's helpful if we can see what component is attached to the game object in the hierarchy window.
- It's helpful if we can see as soon as possible that there is a game object which has a MISSING component or the component has MISSING fields such as materials.

![UsingImage](https://github.com/user-attachments/assets/9d4712e0-a645-41cb-9258-5fd4f07e62ef)

<!-- @import "[TOC]" {cmd="toc" depthFrom=2 depthTo=6 orderedList=false} -->

<!-- code_chunk_output -->

- [📦 How to Install](#-how-to-install)
  - [Unity Package Manager](#unity-package-manager)
  - [Open UPM](#open-upm)
  - [Download Files](#download-files)
- [⚙️ Features](#️-features)
  - [Component Icon](#component-icon)
  - [Missing Indicator](#missing-indicator)
  - [Switching Visibility](#switching-visibility)
  - [Frequency of Error Messages](#frequency-of-error-messages)
- [🔖 Unity Versions](#-unity-versions)
- [📗 Blogs](#-blogs)
- [👥 Help & Contribute](#-help--contribute)
- [🏢 Author Info](#-author-info)
- [📄 License](#-license)

<!-- /code_chunk_output -->

## 📦 How to Install

### Unity Package Manager

On Package Manager, select "Install package from git URL", and input "[https://github.com/lilytech-lab/ComponentIconInHierarchy.git](https://github.com/lilytech-lab/ComponentIconInHierarchy.git)" to the URL box.

### Open UPM

[Open UPM](https://openupm.com/packages/com.lilytech-lab.component-icon-in-hierarchy/) is also available.

```sh
openupm add com.lilytech-lab.component-icon-in-hierarchy
```

### Download Files

Download from [Releases](https://github.com/lilytech-lab/ComponentIconInHierarchy/releases).

## ⚙️ Features

### Component Icon

- Displays component icons attached to the game object on the hierarchy window in order with right aligned.
  - Excludes Transform and RectTransform icons.
  - Displays only one script icon if multiple scripts are attached.
  - Displays translucent icon when the component is not active.

### Missing Indicator

- Displays warning icon on the hierarchy window if the game object is in below status:
  - Attached component is missing.
    - Displays for a first component the app found per a game object.
  - SerializeField in attached component is missing.
- Puts error messages to the console window and notifies following information.
  - Existing a game object attached missing component(s).
  - Name of a missing field the app found first per a component.

### Switching Visibility

There are menus you can switch component icon and missing indicator visibilities in the context menu on the hierarchy window.

![ContextMenu](https://github.com/user-attachments/assets/79eb2c3d-0b3c-46dc-97da-ce087d01211f)

The GameObject menu on the menu bar have same menus, and you can see if the functions are active or not by tick.

![MenuBar](https://github.com/user-attachments/assets/3678d076-fa0f-428c-b7d0-d92b8bf885ce)

It might take a bit of time until the hierarchy window refreshed when the visibility has changed via the menu bar.

### Frequency of Error Messages

The error message tells missing status is put on `EditorApplication.hierarchyWindowItemOnGUI` event, so a new message is put every time the hierarchy window refreshed. It's pretty frequent, so the console window will look like the below image when operating the hierarchy window.

![TooManyErrorMessages](https://github.com/user-attachments/assets/54071315-52e4-42f7-aa21-a43e09f8bc0b)

We know it's too much, but it would be our relief if you think, 'It's OK because I would correct the error soon due to the loud messages.'

## 🔖 Unity Versions

This app is checked working on following Unity versions:

- 2020.3.5f1
- 6000.0.29f1

## 📗 Blogs

We wrote some points about the program. But sorry for written in Japanese.

- [【Unity】ヒエラルキーウィンドウにコンポーネントのアイコンを表示するEditor拡張](https://qiita.com/masamin/items/3c34c85cb3872ff10924)
- [【Unity】Missingチェックを行うEditor拡張](https://qiita.com/masamin/items/78ba7238dae7aacdc28c#comment-fd6816bf8e0241f0ff47)

## 👥 Help & Contribute

When you find some improvements, creating a pull request would be a great help.

## 🏢 Author Info

Lilytech Lab, Inc.  
[https://github.com/lilytech-lab](https://github.com/lilytech-lab)

A small IT engineering company in Japan.

## 📄 License

This application is under the MIT License.
