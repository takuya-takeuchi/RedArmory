Red Armory
--
[![License](https://img.shields.io/github/license/Grabacr07/KanColleViewer.svg?style=flat-square)](https://github.com/takuya-takeuchi/RedArmory/blob/master/LICENSE.txt)

![Main Window of application](/web/images/main.png "Main Window of application")

Red Armory is simple tool to easily backup and restore database, plugins, themes and attached files of Bitnami Redmine Stack which be distributed by Bitnami.

Red Armory は、Bitnami が配布している Bitnami Redmine Stack のデータベース、プラグイン、テーマ、添付ファイルのバックアップ、復元を簡単に実行するためのツールです。

## Main Fuatures (主な機能)

* Backup and Restore MySql database
  * Need not to input database password
* Backup and Restore plugins, themes and attached files.
* Change start up type for each Windows services.
* Support multi Bitnami Redmine Stack in system
* Support Multi Languages
  * For now, only Japanese and English  


* MySql データベースのバックアップ、復元
   * パスワードの自動入力
* プラグイン、テーマ、添付ファイルのバックアップ、復元
* Windows サービスのスタートアップの種類変更
* システム内の複数の Bitnami Redmine Stack のサポート
* 多言語対応
  * 現在、日本語と英語のみ対応

## System requirements (システム要件)

* Windows 7, 8, 8.1 and 10
* .NET Framework 4.5.2
* Bitnami Redmine Stack 2.5 or Later
   * Maybe Red Armory supports Bitnami Redmine Stack 2 or Later. But I did not test.


* Windows 7, 8, 8.1 または 10
* .NET Framework 4.5.2
* Bitnami Redmine Stack 2.5以降
   * Bitnami Redmine Stack 2以降も対応していると思いますが、テストしていません。


## Development Environmental (開発環境)

* C# + Windows Presentation Framework
* Windows 7 Ultimate Service Pack 1 64bit
* Visual Studio 2015 Professional Update 1


## License (ライセンス)

* The MIT License (MIT)

* MIT ライセンスの下で公開する、オープンソース、フリーソフトウェアです。


## Dependencies Libraries and Products (依存ライブラリとプロダクト)

#### [MahApps.Metro](http://mahapps.com/)

> **License:** The Microsoft Public License (Ms-PL)
> 
> **Author:** Paul Jenkins; Jake Ginnivan; Brendan Forster (shiftkey); Alex Mitchell (Amrykid); Dennis Daume (flagbug); Jan Karger (punker76)
> 
> **Principal Use:** Apply Metro style for application window

#### [Material Design In XAML Toolkit](http://materialdesigninxaml.net/)

> **License:** The Microsoft Public License (Ms-PL)
> 
> **Author:** James Willock (ButchersBoy)
> 
> **Principal Use:** Apply Google Material Design for application window

#### [Material icons](https://www.google.com/design/icons/)

> **License:** The Creative Commons 4.0 (CC BY 4.0)
> 
> **Author:** James Willock (ButchersBoy)
> 
> **Principal Use:** Icons. All icons are re-created by Blend for Visual Studio 2015.

#### [Modern UI (Metro) Charts for Windows 8, WPF, Silverlight](https://modernuicharts.codeplex.com/)

> **License:** The Microsoft Public License (Ms-PL)
> 
> **Author:** Torsten Mandelkow
> 
> **Principal Use:** Donut Chart to display storage space

#### [MVVM Light Toolkit](http://www.mvvmlight.net/)

> **License:** The MIT License (MIT)
> 
> **Author:** Laurent Bugnion
> 
> **Principal Use:** Support to develop WPF application by using MVVM Software Architecture

#### [NLog](http://nlog-project.org/)

> **License:** The BSD License (BSD)
> 
> **Author:** Julian Verdurmen (304NotModified); Daniel Gómez Didier (dnlgmzddr); Sreenath (Page-Not-Found); Uğur Aldanmaz
> 
> **Principal Use:** Logging application log

#### [YamlSerializer for .NET](http://nlog-project.org/)

> **License:** The MIT License (MIT)
> 
> **Author:** Osamu TAKEUCHI
> 
> **Principal Use:** Read *.yaml files in Bitnami Redmine stack
