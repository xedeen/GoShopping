using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GoShopping.Controls;
using GoShopping.ViewModels;
using LinqToVisualTree;
using Microsoft.Xna.Framework.Audio;

namespace GoShopping.Interactions
{
    /// <summary>
    /// An interaction that allows the user to add new items by dragging them down from
    /// the top of the screen;
    /// </summary>
    public class PullDownToAddNewInteraction : InteractionBase
    {
      private static readonly double ToDoItemHeight = 182;//147;
        private static readonly double trackDistance = 90;

        private TapEditInteraction _editInteraction;
        private PullDownItem _pullDownItem;
        private double _distance = 0;

        private bool _effectPlayed = false;
        private SoundEffect _popSound;

        public PullDownToAddNewInteraction(TapEditInteraction editInteraction, PullDownItem pullDownItem)
        {
            _editInteraction = editInteraction;
            _pullDownItem = pullDownItem;

            //_popSound = SoundEffect.FromStream(Microsoft.Xna.Framework.TitleContainer.OpenStream("Sounds/pop.wav"));
        }

        protected override void ScrollViewerLocated(ScrollViewer scrollViewer)
        {
            scrollViewer.MouseMove += ScrollViewer_MouseMove;
            scrollViewer.MouseLeftButtonUp += ScrollViewer_MouseLeftButtonUp;
        }


        private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("scrollViewer_MouseMove");
            if (!IsEnabled)
                return;

            // determine whether the user is pulling the list down by inspecting the ScrollViewer.Content abd
            // looking for the required transform.
            UIElement scrollContent = (UIElement) _scrollViewer.Content;
            CompositeTransform ct = scrollContent.RenderTransform as CompositeTransform;
            if (ct != null && ct.TranslateY > 0)
            {
                IsActive = true;

                // offset the pull-down element, set its text and opacity
                _distance = ct.TranslateY;
                _pullDownItem.VerticalOffset = _distance - ToDoItemHeight;

                if (_distance > trackDistance && !_effectPlayed)
                {
                    _effectPlayed = true;
                    //_popSound.Play();
                }

                _pullDownItem.Text = _distance > trackDistance
                    ? "Чтобы добавить, отпустите"
                    : "Чтобы добавить, тяните вниз";

                _pullDownItem.Opacity = Math.Min(1.0, _distance/trackDistance);
            }
        }

        private void ScrollViewer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("scrollViewer_MouseLeftButtonUp");

            if (!IsActive)
                return;

            // hide the pull down item by locating it off screen
            _pullDownItem.VerticalOffset = -ToDoItemHeight;

            // if the list was pulled down far enough, add a new item
            if (_distance > trackDistance)
            {
                var newItem = new ShoppingItemViewModel("");
                _todoItems.Insert(0, newItem);

                // when the new item has been rendered, use the edit interaction to place the UI
                // into edit mode
                _todoList.InvokeOnNextLayoutUpdated(() => _editInteraction.EditItem(newItem));
            }

            IsActive = false;
            _effectPlayed = false;
        }
    }
}
