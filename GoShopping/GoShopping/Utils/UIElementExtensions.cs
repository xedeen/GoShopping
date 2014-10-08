using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoShopping
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public static class UIElementExtensions
    {
        /// <summary>
        /// Gets the relative position of the given UIElement to this.
        /// </summary>
        public static Point GetRelativePosition(this UIElement element, UIElement other)
        {
            return element.TransformToVisual(other)
                .Transform(new Point(0, 0));
        }


    }
}
