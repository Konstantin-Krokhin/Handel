using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HandelTSE.ViewModels
{
    // Class responsible for highlighting the TreeViewItem when DG row is dragged over it
    public static class TreeViewDropHighlighter
    {
        /// the TreeViewItem that is the current drop target
        private static TreeViewItem _currentItem = null;

        /// Indicates whether the current TreeViewItem is a possible drop target
        private static bool _dropPossible;

        /// Property key (since this is a read-only DP) for the IsPossibleDropTarget property.
        private static readonly DependencyPropertyKey IsPossibleDropTargetKey =
                                    DependencyProperty.RegisterAttachedReadOnly(
                                        "IsPossibleDropTarget",
                                        typeof(bool),
                                        typeof(TreeViewDropHighlighter),
                                        new FrameworkPropertyMetadata(null,
                                            new CoerceValueCallback(CalculateIsPossibleDropTarget)));


        /// Dependency Property IsPossibleDropTarget. Is true if the TreeViewItem is a possible drop target 
        /// (i.e., if it would receive the OnDrop event if the mouse button is released right now).
        public static readonly DependencyProperty IsPossibleDropTargetProperty = IsPossibleDropTargetKey.DependencyProperty;

        /// Getter for IsPossibleDropTarget
        public static bool GetIsPossibleDropTarget(DependencyObject obj) { return (bool)obj.GetValue(IsPossibleDropTargetProperty); }

        /// Coercion method which calculates the IsPossibleDropTarget property.
        private static object CalculateIsPossibleDropTarget(DependencyObject item, object value)
        {
            if ((item == _currentItem) && (_dropPossible))
                return true;
            else
                return false;
        }

        /// Initializes the <see cref="TreeViewDropHighlighter"/> class.
        static TreeViewDropHighlighter()
        {
            // Get all drag enter/leave events for TreeViewItem.
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewDragEnterEvent, new DragEventHandler(OnDragEvent), true);
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewDragLeaveEvent, new DragEventHandler(OnDragLeave), true);
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewDragOverEvent, new DragEventHandler(OnDragEvent), true);
        }

        /// Called when an item is dragged over the TreeViewItem.
        static void OnDragEvent(object sender, DragEventArgs args)
        {
            lock (IsPossibleDropTargetProperty)
            {
                _dropPossible = false;

                if (_currentItem != null)
                {
                    // Tell the item that previously had the mouse that it no longer does.
                    DependencyObject oldItem = _currentItem;
                    _currentItem = null;
                    oldItem.InvalidateProperty(IsPossibleDropTargetProperty);
                }

                if (args.Effects != DragDropEffects.None)
                {
                    _dropPossible = true;
                }

                TreeViewItem tvi = sender as TreeViewItem;
                if (tvi != null)
                {
                    _currentItem = tvi;
                    // Tell that item to re-calculate the IsPossibleDropTarget property
                    _currentItem.InvalidateProperty(IsPossibleDropTargetProperty);
                }
            }
        }

        /// Called when the drag cursor leaves the TreeViewItem
        static void OnDragLeave(object sender, DragEventArgs args)
        {
            lock (IsPossibleDropTargetProperty)
            {
                _dropPossible = false;

                if (_currentItem != null)
                {
                    // Tell the item that previously had the mouse that it no longer does.
                    DependencyObject oldItem = _currentItem;
                    _currentItem = null;
                    oldItem.InvalidateProperty(IsPossibleDropTargetProperty);
                }

                TreeViewItem tvi = sender as TreeViewItem;
                if (tvi != null)
                {
                    _currentItem = tvi;
                    tvi.InvalidateProperty(IsPossibleDropTargetProperty);
                }
            }
        }
    }
}
