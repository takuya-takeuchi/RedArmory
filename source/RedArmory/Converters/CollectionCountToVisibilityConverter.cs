using System;
using System.Collections;
using System.Windows;
using System.Windows.Data;

namespace Ouranos.RedArmory.Converters
{

    /// <summary>
    /// コレクションの要素数が指定した要素数以下の時、要素の Visibility 列挙値を指定した Visibility へ変換するコンバーターを表します。このクラスは継承できません。
    /// </summary>
    internal sealed class CollectionCountToVisibilityConverter : IValueConverter
    {

        private bool _IsHidden;

        public bool IsHidden
        {
            get
            {
                return this._IsHidden;
            }
            set
            {
                this._IsHidden = value;
            }
        }

        private int _TriggerCount;

        public int TriggerCount
        {
            get
            {
                return this._TriggerCount;
            }
            set
            {
                this._TriggerCount = value;
            }
        }

        /// <summary>
        /// 値を変換します。
        /// </summary>
        /// <param name="value">バインディング ソースによって生成された値。</param>
        /// <param name="targetType">バインディング ターゲット プロパティの型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        /// <returns>変換された値。メソッドが null を返す場合は、有効な null 値が使用されています。</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.GetVisibility(value);
        }

        /// <summary>
        /// 値を変換します。
        /// </summary>
        /// <param name="value">バインディング ターゲットによって生成される値。</param>
        /// <param name="targetType">変換後の型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        /// <returns>変換された値。 メソッドが null を返す場合は、有効な null 値が使用されています。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #region Helpers

        private object GetVisibility(object value)
        {
            var collection = value as ICollection;
            if (collection == null)
            {
                return Visibility.Collapsed;
            }

            if (collection.Count <= this._TriggerCount)
            {
                return this.IsHidden ? Visibility.Hidden : Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        #endregion

    }

}
