using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Ouranos.RedArmory.Behaviors
{

    internal sealed class ItemsControlDragAndDropBehavior : Behavior<ItemsControl>
    {

        #region �t�B�[���h

        private object _DraggedData;

        private int? _DraggedItemIndex;

        private Point? _InitialPosition;

        private IList _Items;

        #endregion

        #region �ˑ��֌W�v���p�e�B

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

        #region ���\�b�h

        #region �I�[�o�[���C�h

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

        #region �C�x���g�n���h��

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // �_�u���N���b�N�ɂ���āAItemsControl ���̃A�C�e���̍������ω����A
            // �X�N���[������������ȂǂŁA�ʒu������邱�Ƃɂ���āA�Ӑ}����
            // �h���b�O�A���h�h���b�v�̔�����h��
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

            // �`�F�b�N���Ȃ��ق����������ǂ����߃R�����g�A�E�g
            //var currentPos = itemsControl.PointToScreen(e.GetPosition(itemsControl));
            //if (!this.MovedEnoughForDrag((this._InitialPosition - currentPos).Value))
            //{
            //   return;
            //}

            // �h���b�v��̃A�C�e�����`�F�b�N
            var draggedItem = e.OriginalSource as FrameworkElement;
            if (draggedItem == null)
            {
                return;
            }

            // �h���b�v���Ɠ����Ȃ�L�����Z��
            // �L�����Z�����Ȃ��ƁA�A�C�e����̃{�^���Ȃǂ��������Ȃ�
            // (��� DragDrop.DoDragDrop ���������邽��)
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

            // �h���b�O��̏㔼���Ƀh���b�v���悤�Ƃ��Ă��邩
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

            // �A�C�e���� 2 �����ŁA 0 ���� 1 �Ƀh���b�v����ƁA�h���b�v��̈�O��
            // �}������邽�߁A�ړ��ł��Ȃ��悤�Ɍ�����
            // ���̂��߁A���̎������́A�����ɒǉ�����
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

        #region �w���p�[���\�b�h

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
            // �������Ȃ��ƃ^�C�~���O�ɂ���ăN���b�V������
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