using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RedArmory.Models;
using RedArmory.Models.Services;

namespace RedArmory.ViewModels
{

    public sealed class AboutViewModel : ViewModelBase
    {

        #region フィールド

        private readonly ILoggerService _LoggerService;

        #endregion

        #region コンストラクタ

        public AboutViewModel(ILoggerService loggerService)
        {
            this._LoggerService = loggerService;

            this.Copyright = AssemblyProperty.Copyright;
            this.Product = AssemblyProperty.Product;
            this.Version = AssemblyProperty.Version;

            this.TwitterCommand = new RelayCommand(() => System.Diagnostics.Process.Start(this.Twitter), () => true);
            this.FacebookCommand = new RelayCommand(() => System.Diagnostics.Process.Start(this.Facebook), () => true);
            this.GitHubCommand = new RelayCommand(() => System.Diagnostics.Process.Start(this.GitHub), () => true);
            this.WebSiteCommand = new RelayCommand(() => System.Diagnostics.Process.Start(this.WebSite), () => true);

            this.GitHub = "https://github.com/takuya-takeuchi/";
            this.Twitter = "https://twitter.com/takuya_takeuchi/";
            this.Facebook = "https://www.facebook.com/takuya.takeuchi.sns";
            this.WebSite = "http://ouranos.sakura.ne.jp/wordpress/";

            var acknowledgmentModels = new[]
            {
                new AcknowledgmentModel
                {
                    Name = "MahApps.Metro",
                    Author = "Paul Jenkins; Jake Ginnivan; Brendan Forster (shiftkey); Alex Mitchell (Amrykid); Dennis Daume (flagbug); Jan Karger (punker76)",
                    Description = "A toolkit for creating metro-style WPF applications. Lots of goodness out-of-the box.",
                    Licence = "MahApps.Metro is distributed under the Microsoft Public License(Ms-PL).",
                    Url = "http://mahapps.com/"
                },
                new AcknowledgmentModel
                {
                    Name = "Material Design In XAML Toolkit",
                    Author = "James Willock (ButchersBoy)",
                    Description = "An open source library to providing the Material Design palette, plenty of control themes, and new custom controls to bring this popular design language to your desktop applications.",
                    Licence = "Material Design In XAML Toolkit is distributed under the Microsoft Public License(Ms-PL).",
                    Url = "http://materialdesigninxaml.net/"
                },
                new AcknowledgmentModel
                {
                    Name = "Material icons",
                    Author = "Google",
                    Description = "Material icons are beautifully crafted, delightful, and easy to use in your web, Android, and iOS projects. Learn more about material design and our process for making these icons in the system icons section of the material design guidelines.",
                    Licence = "Modern UI Icons is distributed under the Creative Commons 4.0 (CC BY 4.0).",
                    Url = "https://www.google.com/design/icons/"
                },
                new AcknowledgmentModel
                {
                    Name = "Modern UI (Metro) Charts for Windows 8, WPF, Silverlight",
                    Author = "Torsten Mandelkow",
                    Description = "This library provides a small library to display charts in Modern UI Style (formerly known as Metro) in WPF, Silverlight and Windows 8 applications.",
                    Licence = "Modern UI (Metro) Charts for Windows 8, WPF, Silverlight is distributed under the Microsoft Public License(Ms-PL).",
                    Url = "https://modernuicharts.codeplex.com/"
                },
                new AcknowledgmentModel
                {
                    Name = "MVVM Light Toolkit",
                    Author = "Laurent Bugnion",
                    Description = "The MVVM Light Toolkit helps you to separate your View from your Model which creates applications that are cleaner and easier to maintain and extend.",
                    Licence = "MVVM Light Toolkit is distributed under the Massachusetts Institute of Technology License (MIT).",
                    Url = "http://www.mvvmlight.net/"
                },
                new AcknowledgmentModel
                {
                    Name = "NLog",
                    Author = "Julian Verdurmen (304NotModified); Daniel Gómez Didier (dnlgmzddr); Sreenath (Page-Not-Found); Uğur Aldanmaz",
                    Description = "This Modern UI Icons project was started back in October 2011 with the goal of allowing developers a single solution for icons.",
                    Licence = "NLog is distributed under the Berkeley Software Distribution License (BSD).",
                    Url = "http://nlog-project.org/"
                },
                new AcknowledgmentModel
                {
                    Name = "YamlSerializer for .NET",
                    Author = "Osamu TAKEUCHI",
                    Description = "This class library supports YAML file manipulation in two different contexts.\nOne is to serialize / deserialize C# native objects into / from YAML 1.2 text files.\nThe other is to manipulate generic YAML 1.2 documents that does not represent C# object but does some unknown logical data.",
                    Licence = "YamlSerializer for .NET is distributed under the Massachusetts Institute of Technology License (MIT).",
                    Url = "https://yamlserializer.codeplex.com/"
                },
            };

            this.Acknowledgments = new ObservableCollection<AcknowledgmentModel>(acknowledgmentModels);
        }

        #endregion

        #region プロパティ

        public RelayCommand TwitterCommand
        {
            get;
            private set;
        }

        public RelayCommand FacebookCommand
        {
            get;
            private set;
        }

        public RelayCommand GitHubCommand
        {
            get;
            private set;
        }

        public RelayCommand WebSiteCommand
        {
            get;
            private set;
        }

        private ObservableCollection<AcknowledgmentModel> _Acknowledgments;

        public ObservableCollection<AcknowledgmentModel> Acknowledgments
        {
            get
            {
                return this._Acknowledgments;
            }
            set
            {
                this._Acknowledgments = value;
                this.RaisePropertyChanged();
            }
        }

        private string _Copyright;

        public string Copyright
        {
            get
            {
                return this._Copyright;
            }
            set
            {
                this._Copyright = value;
                this.RaisePropertyChanged();
            }
        }

        private string _Product;

        public string Product
        {
            get
            {
                return this._Product;
            }
            set
            {
                this._Product = value;
                this.RaisePropertyChanged();
            }
        }

        private string _Version;

        public string Version
        {
            get
            {
                return this._Version;
            }
            set
            {
                this._Version = value;
                this.RaisePropertyChanged();
            }
        }

        private string _Facebook;

        public string Facebook
        {
            get
            {
                return this._Facebook;
            }
            set
            {
                this._Facebook = value;
                this.RaisePropertyChanged();
            }
        }

        private string _GitHub;

        public string GitHub
        {
            get
            {
                return this._GitHub;
            }
            set
            {
                this._GitHub = value;
                this.RaisePropertyChanged();
            }
        }

        private string _Twitter;

        public string Twitter
        {
            get
            {
                return this._Twitter;
            }
            set
            {
                this._Twitter = value;
                this.RaisePropertyChanged();
            }
        }

        private string _WebSite;

        public string WebSite
        {
            get
            {
                return this._WebSite;
            }
            set
            {
                this._WebSite = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }

}
