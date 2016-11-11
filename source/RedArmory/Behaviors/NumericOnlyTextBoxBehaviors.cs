using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Ouranos.RedArmory.Behaviors
{

    internal sealed class NumericOnlyTextBoxBehaviors : Behavior<TextBox>
    {

        #region 依存関係プロパティ

        public static readonly DependencyProperty DefaultNumericProperty = DependencyProperty.RegisterAttached(
            "DefaultNumeric",
            typeof(int),
            typeof(NumericOnlyTextBoxBehaviors),
            new UIPropertyMetadata(0));

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static int GetDefaultNumeric(DependencyObject obj)
        {
            return (int)obj.GetValue(DefaultNumericProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static void SetDefaultNumeric(DependencyObject obj, int value)
        {
            obj.SetValue(DefaultNumericProperty, value);
        }

        public static readonly DependencyProperty IsNumericProperty = DependencyProperty.RegisterAttached(
            "IsNumeric",
            typeof(bool),
            typeof(NumericOnlyTextBoxBehaviors),
            new UIPropertyMetadata(false));

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetIsNumeric(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsNumericProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static void SetIsNumeric(DependencyObject obj, bool value)
        {
            obj.SetValue(IsNumericProperty, value);
        }

        #endregion

        #region メソッド

        #region オーバーライド

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.KeyDown += this.OnKeyDown;
            this.AssociatedObject.TextChanged += this.OnTextChanged;
            DataObject.AddPastingHandler(this.AssociatedObject, this.OnTextBoxPastingEventHandler);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.KeyDown -= this.OnKeyDown;
            this.AssociatedObject.TextChanged -= this.OnTextChanged;
            DataObject.RemovePastingHandler(this.AssociatedObject, this.OnTextBoxPastingEventHandler);
        }

        #endregion

        #region イベントハンドラ

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!GetIsNumeric(this))
            {
                return;
            }

            var textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }

            if ((Key.D0 <= e.Key && e.Key <= Key.D9) ||
                (Key.NumPad0 <= e.Key && e.Key <= Key.NumPad9) ||
                (Key.Delete == e.Key) || (Key.Back == e.Key) || (Key.Tab == e.Key))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!GetIsNumeric(this))
            {
                return;
            }

            var textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = GetDefaultNumeric(this).ToString();
            }
        }

        private void OnTextBoxPastingEventHandler(object sender, DataObjectPastingEventArgs e)
        {
            if (!GetIsNumeric(this))
            {
                return;
            }

            var textBox = sender as TextBox;
            var clipboard = e.DataObject.GetData(typeof(string)) as string;
            clipboard = ValidateValue(clipboard);
            if (textBox != null && !string.IsNullOrEmpty(clipboard))
            {
                textBox.Text = clipboard;
            }

            e.CancelCommand();
            e.Handled = true;
        }

        #endregion

        #region ヘルパーメソッド

        private static string ValidateValue(string text)
        {
            var returntext = "";
            foreach (var c in text)
            {
                if (Regex.Match(c.ToString(), "^[-0-9]$").Success)
                {
                    returntext += c.ToString();
                }
                else if (c == '.')
                {
                    returntext += c.ToString();
                }
            }

            return returntext;
        }

        #endregion

        #endregion

    }

}
