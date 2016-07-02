using System.Globalization;
using GalaSoft.MvvmLight;

namespace Ouranos.RedArmory.Models
{
    /// <summary>
    /// 多言語化されたリソースと、言語の切り替え機能を提供します。
    /// </summary>
    public sealed class ResourceService : ViewModelBase
    {

        #region コンストラクタ

        static ResourceService()
        { }

        private ResourceService()
        {
        }

        #endregion

        #region プロパティ

        private static ResourceService _Instance;

        public static ResourceService Instance
        {
            get
            {
                return _Instance ?? (_Instance = new ResourceService());
            }
        }

        private readonly Properties.Resources _Resources = new Properties.Resources();

        /// <summary>
        /// 多言語化されたリソースを取得します。
        /// </summary>
        public Properties.Resources Resources
        {
            get
            {
                return this._Resources;
            }
        }

        #endregion

        #region メソッド

        public void ChangeCulture(string name)
        {
            Properties.Resources.Culture = CultureInfo.GetCultureInfo(name);
            this.RaisePropertyChanged("Resources");
        }

        #endregion

    }

}
