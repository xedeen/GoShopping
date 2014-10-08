﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoShopping.ViewModels;
using LinqToVisualTree;
using System.Collections.Generic;

namespace GoShopping.Interactions
{
    /// <summary>
    /// An interaction that allows a user to edit an item by tapping on it.
    /// </summary>
    public class TapEditInteraction : InteractionBase
    {
        private TextBlock _taskText;
        private TextBox _taskTextEdit;
        private Grid _taskEditGrid;
        private string _originalText;
        private ShoppingItemViewModel _editItem;

        public override void Initialise(ItemsControl todoList, ResettableObservableCollection<ShoppingItemViewModel> todoItems)
        {
            base.Initialise(todoList, todoItems);

            todoList.KeyUp += ItemsControl_KeyUp;
        }

        public override void AddElement(FrameworkElement element)
        {
            element.Tap += Element_Tap;
        }

        private void Element_Tap(object sender, GestureEventArgs e)
        {
            if (!IsEnabled)
                return;

            IsActive = true;

            // find the edit and static text controls
            var border = sender as Border;
            EditItem(border.DataContext as ShoppingItemViewModel);
        }

        public void EditItem(ShoppingItemViewModel editItem)
        {
            _editItem = editItem;

            // find the edit and static text controls
            var container = _todoList.ItemContainerGenerator.ContainerFromItem(editItem);
            _taskTextEdit = FindNamedDescendant<TextBox>(container, "taskTextEdit");
            _taskEditGrid = FindNamedDescendant<Grid>(container, "editGrid");
            _taskText = FindNamedDescendant<TextBlock>(container, "taskText");

            // store the original text to allow undo
            _originalText = _taskTextEdit.Text;

            EditFieldVisible(true);

            // set the caret position to the end of the text field
            _taskTextEdit.Focus();
            _taskTextEdit.Select(_originalText.Length, 0);
            _taskEditGrid.LostFocus += _taskEditGrid_LostFocus;
            //_taskTextEdit.LostFocus += TaskTextEdit_LostFocus;

            // fade out all other items
            ((FrameworkElement)_todoList.ItemContainerGenerator.ContainerFromItem(_editItem)).Opacity = 1;
            var elements = _todoItems.Where(i => i != _editItem)
                                     .Select(i => _todoList.ItemContainerGenerator.ContainerFromItem(i))
                                     .Cast<FrameworkElement>();
            foreach (var el in elements)
            {
                el.Animate(1.0, 0.7, FrameworkElement.OpacityProperty, 800, 0);
            }
        }

        private void EditFieldVisible(bool visible)
        {
            //_taskTextEdit.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            _taskEditGrid.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            _taskText.Visibility = visible ? Visibility.Collapsed : Visibility.Visible;

            if (visible == false)
            {
                var elements = _todoItems.Select(i => _todoList.ItemContainerGenerator.ContainerFromItem(i))
                                         .Cast<FrameworkElement>();
                foreach (var el in elements)
                {
                    if (null != el)
                        el.Animate(null, 1.0, FrameworkElement.OpacityProperty, 800, 0);
                }
            }
        }

        private void ItemsControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EndEdit();
            }
        }

        private void EndEdit()
        {
            //_taskTextEdit.LostFocus -= TaskTextEdit_LostFocus;
            _taskEditGrid.LostFocus -= _taskEditGrid_LostFocus;

            EditFieldVisible(false);
            IsActive = false;
        }

        /*private void TaskTextEdit_LostFocus(object sender, RoutedEventArgs e)
        {
            EndEdit();
        }*/

        void _taskEditGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            EndEdit();
        }

        private T FindNamedDescendant<T>(DependencyObject element, string name)
          where T : FrameworkElement
        {
            return element.Descendants()
                          .OfType<T>()
                          .Where(i => i.Name == name)
                          .Single();
        }
    }
}