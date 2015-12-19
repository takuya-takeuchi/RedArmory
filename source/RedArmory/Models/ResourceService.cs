using System.Globalization;
using GalaSoft.MvvmLight;
using RedArmory.Properties;

namespace RedArmory.Models
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

        private readonly Resources _Resources = new Resources();

        /// <summary>
        /// 多言語化されたリソースを取得します。
        /// </summary>
        public Resources Resources
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
            Resources.Culture = CultureInfo.GetCultureInfo(name);
            this.RaisePropertyChanged("Resources");
        }

        #endregion

    }

}
