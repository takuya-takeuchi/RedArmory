Red Armory
--
[![Github All Releases](https://img.shields.io/github/downloads/takuya-takeuchi/RedArmory/total.svg)]()
[![Build status](https://ci.appveyor.com/api/projects/status/d340i8loqua7s20u?svg=true)](https://ci.appveyor.com/project/takuya-takeuchi/redarmory)
[![GitHub license](https://img.shields.io/github/license/mashape/apistatus.svg)]()

![Main Window of application](/web/images/main.png "Main Window of application")

###### English 
Red Armory is simple tool to easily backup and restore database, plugins, themes and attached files of Bitnami Redmine Stack which be distributed by Bitnami.

###### Japanese 
Red Armory は、Bitnami が配布している Bitnami Redmine Stack のデータベース、プラグイン、テーマ、添付ファイルのバックアップ、復元を簡単に実行するためのツールです。

## Main Features (主な機能)

###### English 
* Backup and Restore MySql database
  * Need not to input database password
* Backup and Restore plugins, themes and attached files.
* Change start up type for each Windows services.
* Support multi Bitnami Redmine Stack in system
* Reorder enumerations items
  * It is easy to reorder for before Redmine 3.3
* Add/Remove backup task for task scheduler
* Support Multi Languages
  * For now, only Japanese and English  


###### Japanese 
* MySql データベースのバックアップ、復元
  * パスワードの自動入力
* プラグイン、テーマ、添付ファイルのバックアップ、復元
* Windows サービスのスタートアップの種類変更
* システム内の複数の Bitnami Redmine Stack のサポート
* 列挙項目のソート
** Redmine 3.3 以前でもソートが簡単に実行可能
* タスクスケジューラへのバックアップタスクの登録、削除
* 多言語対応
  * 現在、日本語と英語のみ対応

## Screenshots (スクリーンショット)

![Edit backup task](/web/images/task.png "Edit backup task")

![Edit enumeration items](/web/images/enumerations.png "Edit enumeration items")

## System requirements (システム要件)

###### English 
* Windows 7, 8, 8.1 and 10
* .NET Framework 4.5.2
* Bitnami Redmine Stack 2.5 or Later


###### Japanese 
* Windows 7, 8, 8.1 または 10
* .NET Framework 4.5.2
* Bitnami Redmine Stack 2.5以降


## Development Environmental (開発環境)

* C# + Windows Presentation Foundation
* Windows 7 Ultimate Service Pack 1 64bit
* Visual Studio 2015 Professional Update 1


## License (ライセンス)

###### English 
* The MIT License (MIT)
* This software is allowed to publish under the MIT License due to FOSS License Exception of MySQL.
* Please refer http://www.mysql.com/about/legal/licensing/foss-exception/ if you want to know.


###### Japanese 
* MIT ライセンスの下で公開する、オープンソース、フリーソフトウェアです。
* 本ソフトはMySql.Dataをリンクしていますが、FOSSライセンス例外に従い、MIT ライセンスの下で公開することができます。
* FOSSライセンス例外については、http://www.mysql.com/about/legal/licensing/foss-exception/


## Dependencies Libraries and Products (依存ライブラリとプロダクト)

#### [Dapper](https://github.com/StackExchange/dapper-dot-net/)

> **License:** Apache License 2.0
>
> **Author:** Marc Gravell; Nick Craver
> 
> **Principal Use:** Object-relational mapping (ORM) product for the Microsoft .NET platform

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

#### [Mono.Options](https://www.nuget.org/packages/Mono.Options/)

> **License:** Xamarin License
> 
> **Author:** Xamarin
> 
> **Principal Use:** Command line parsing library

#### [MVVM Light Toolkit](http://www.mvvmlight.net/)

> **License:** The MIT License (MIT)
> 
> **Author:** Laurent Bugnion
> 
> **Principal Use:** Support to develop WPF application by using MVVM Software Architecture

#### [MySql.Data](http://www.mysql.com/)

> **License:** GNU GENERAL PUBLIC LICENSE Version 2
> 
> **Author:** Oracle Corporation
> 
> **Principal Use:** Restore database data to MySql database of Bitnami Redmine Stack

#### [NLog](http://nlog-project.org/)

> **License:** The BSD License (BSD)
> 
> **Author:** Julian Verdurmen (304NotModified); Daniel Gómez Didier (dnlgmzddr); Sreenath (Page-Not-Found); Uğur Aldanmaz
> 
> **Principal Use:** Logging application log

#### [Task Scheduler Managed Wrapper](http://taskscheduler.codeplex.com/)

> **License:** The Microsoft Public License (Ms-PL)
> 
> **Author:** David Hall
> 
> **Principal Use:** Wrapper library to manage task scheduler

#### [YamlSerializer for .NET](https://yamlserializer.codeplex.com/)

> **License:** The MIT License (MIT)
> 
> **Author:** Osamu TAKEUCHI
> 
> **Principal Use:** Read *.yaml files in Bitnami Redmine stack
