using System;
using System.Windows.Data;
using RedArmory.Models.Services;

namespace RedArmory.Converters
{

    /// <summary>
    /// <see cref="ServiceStartupType"/> から <see cref="string"/> への変換を行うコンバーターを表します。このクラスは継承できません。
    /// </summary>
    public sealed class ServiceStartupTypeToStringConverter : IValueConverter
    {

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
            var status = (ServiceStartupType)value;
            switch (status)
            {
                case ServiceStartupType.Disabled:
                    return Properties.Resources.Word_ServiceDisabled;
                case ServiceStartupType.Manual:
                    return Properties.Resources.Word_ServiceManual;
                case ServiceStartupType.Automatic:
                    return Properties.Resources.Word_ServiceAutomatic;
                case ServiceStartupType.DelayStart:
                    return Properties.Resources.Word_ServiceDelayedStart;
            }

            return null;
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
    }

}
