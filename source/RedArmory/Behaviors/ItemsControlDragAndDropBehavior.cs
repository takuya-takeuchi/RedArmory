using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Ouranos.RedArmory.Behaviors
{

    internal sealed class ItemsControlDragAndDropBehavior : Behavior<ItemsControl>
    {

        #region フィールド

        private object _DraggedData;

        private int? _DraggedItemIndex;

        private Point? _InitialPosition;

        private IList _Items;

        #endregion

        #region 依存関係プロパティ

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.RegisterAttached(
                "Items",
                typeof(IList),
                typeof(ItemsControlDragAndDropBehavior),
                new UIPropertyMetadata(null, OnItemsChanged));

        public static IList GetItems(ItemsControl itemsControl)
        {
            return (IList)itemsControl.GetValue(ItemsProperty);
        }

        public static void SetItems(ItemsControl itemsControl, IList value)
        {
            itemsControl.SetValue(ItemsProperty, value);
        }

        private static void OnItemsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var behavior = dependencyObject as ItemsControlDragAndDropBehavior;
            if (behavior == null)
            {
                return;
            }

            behavior._Items = e.NewValue as IList;
        }

        public static readonly DependencyProperty IsDraggingProperty = DependencyProperty.RegisterAttached(
                "IsDragging",
                typeof(bool),
                typeof(ItemsControlDragAndDropBehavior),
                new UIPropertyMetadata(null));

        public static bool GetIsDragging(ItemsControl itemsControl)
        {
            return (bool)itemsControl.GetValue(IsDraggingProperty);
        }

        public static void SetIsDragging(ItemsControl itemsControl, bool value)
        {
            itemsControl.SetValue(IsDraggingProperty, value);
        }

        #endregion

        #region メソッド

        #region オーバーライド

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewMouseLeftButtonDown += this.OnPreviewMouseLeftButtonDown;
            this.AssociatedObject.PreviewMouseMove += this.OnPreviewMouseMove;
            this.AssociatedObject.PreviewDrop += this.OnPreviewDrop;
            this.AssociatedObject.PreviewMouseUp += this.OnPreviewMouseUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PreviewMouseLeftButtonDown -= this.OnPreviewMouseLeftButtonDown;
            this.AssociatedObject.PreviewMouseMove -= this.OnPreviewMouseMove;
            this.AssociatedObject.PreviewDrop -= this.OnPreviewDrop;
            this.AssociatedObject.PreviewMouseUp -= this.OnPreviewMouseUp;
        }

        #endregion

        #region イベントハンドラ

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // ダブルクリックによって、ItemsControl 内のアイテムの高さが変化し、
            // スクロールが発生するなどで、位置がずれることによって、意図せぬ
            // ドラッグアンドドロップの発生を防ぐ
            if (e.ClickCount > 1)
            {
                return;
            }

            var itemsControl = sender as ItemsControl;
            if (itemsControl == null)
            {
                return;
            }

            var draggedItem = e.OriginalSource as FrameworkElement;
            if (draggedItem == null)
            {
                return;
            }

            this._DraggedData = this.GetItemData(itemsControl, draggedItem);
            if (this._DraggedData == null)
            {
                return;
            }

            this._InitialPosition = itemsControl.PointToScreen(e.GetPosition(itemsControl));
            //_mouseOffsetFromItem = itemsControl.PointToItem(draggedItem, this._initialPosition.Value);
            this._DraggedItemIndex = this.GetItemIndex(itemsControl, this._DraggedData);
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this._DraggedData == null || this._InitialPosition == null)
            {
                return;
            }

            var itemsControl = sender as ItemsControl;
            if (itemsControl == null)
            {
                return;
            }

            // チェックしないほうが反応が良いためコメントアウト
            //var currentPos = itemsControl.PointToScreen(e.GetPosition(itemsControl));
            //if (!this.MovedEnoughForDrag((this._InitialPosition - currentPos).Value))
            //{
            //   return;
            //}

            // ドロップ先のアイテムをチェック
            var draggedItem = e.OriginalSource as FrameworkElement;
            if (draggedItem == null)
            {
                return;
            }

            // ドロップ元と同じならキャンセル
            // キャンセルしないと、アイテム上のボタンなどが反応しない
            // (先の DragDrop.DoDragDrop が発動するため)
            var current = this.GetItemData(itemsControl, draggedItem);
            if (this._DraggedData == current)
            {
                return;
            }

            SetIsDragging(this.AssociatedObject, true);

            DragDrop.DoDragDrop(itemsControl, this._DraggedData, DragDropEffects.Move);
            this.CleanUpData();
        }

        private void OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            SetIsDragging(this.AssociatedObject, false);
            this.CleanUpData();
        }

        private void OnPreviewDrop(object sender, DragEventArgs e)
        {
            var itemsControl = sender as ItemsControl;
            if (itemsControl == null)
            {
                return;
            }

            FrameworkElement container;
            var dropTargetData = this.GetItemData(itemsControl, e.OriginalSource as DependencyObject, out container);
            if (dropTargetData == null)
            {
                return;
            }

            // ドラッグ先の上半分にドロップしようとしているか
            var point = e.GetPosition(container);
            var next = container.ActualHeight / 2 < point.Y;

            var index = this.GetItemIndex(itemsControl, dropTargetData);
            if (index == null)
            {
                return;
            }

            this.DropItemAt(index + (next ? 1 : 0));
        }

        private void DropItemAt(int? droppedItemIndex)
        {
            if (this._Items == null)
            {
                return;
            }

            var data = this._DraggedData;
            this._Items.Remove(data);

            // アイテムが 2 個だけで、 0 から 1 にドロップすると、ドロップ先の一つ前に
            // 挿入されるため、移動できないように見える
            // そのため、この時だけは、末尾に追加する
            if (this._Items.Count == 1 && droppedItemIndex != null && droppedItemIndex == 1)
            {
                this._Items.Add(data);
                return;
            }

            if (droppedItemIndex != null)
            {
                droppedItemIndex -= droppedItemIndex > this._DraggedItemIndex ? 1 : 0;
                this._Items.Insert((int)droppedItemIndex, data);
            }
            else
            {
                this._Items.Add(data);
            }
        }

        #endregion

        #region ヘルパーメソッド

        private void CleanUpData()
        {
            this._InitialPosition = null;
            this._DraggedData = null;
            this._DraggedItemIndex = null;
        }

        private FrameworkElement GetItemContainer(ItemsControl itemsControl, DependencyObject item)
        {
            if (itemsControl == null || item == null)
            {
                return null;
            }

            return itemsControl.ContainerFromElement(item) as FrameworkElement;
        }

        private object GetItemData(ItemsControl itemsControl, DependencyObject item)
        {
            var container = this.GetItemContainer(itemsControl, item);
            return container?.DataContext;
        }

        private object GetItemData(ItemsControl itemsControl, DependencyObject item, out FrameworkElement container)
        {
            container = this.GetItemContainer(itemsControl, item);
            return container?.DataContext;
        }

        private int? GetItemIndex(ItemsControl itemsControl, object item)
        {
            var items = itemsControl.ItemsSource as IList;
            if (items == null)
            {
                return null;
            }

            // NOTE
            // ここがないとタイミングによってクラッシュする
            if (item == null)
            {
                return null;
            }

            var index = items.IndexOf(item);
            return index != -1 ? index : (int?)null;
        }

        //private bool MovedEnoughForDrag(Vector delta)
        //{
        //    return Math.Abs(delta.X) > SystemParameters.MinimumHorizontalDragDistance &&
        //           Math.Abs(delta.Y) > SystemParameters.MinimumVerticalDragDistance;
        //}

        #endregion

        #endregion

    }

}