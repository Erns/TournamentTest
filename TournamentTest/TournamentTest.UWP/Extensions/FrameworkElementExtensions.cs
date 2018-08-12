using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentTest.UWP.Extensions
{
    using global::Windows.UI.Xaml;
    using global::Windows.UI.Xaml.Media;

    /// <summary>
    /// Extensions for <see cref="FrameworkElement"/>
    /// </summary>
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// Finds the first instance of <see cref="{T}"/> while iterating
        /// through each <see cref="FrameworkElement.Parent"/>.
        /// </summary>
        /// <typeparam name="T">The type of element we are looking for.</typeparam>
        /// <param name="element">The starting point element.</param>
        /// <returns>The first instance of <see cref="{T}"/> or null if not found.</returns>
        public static T FindAncestorElement<T>(this FrameworkElement element) where T : FrameworkElement
        {
            var parent = (FrameworkElement)VisualTreeHelper.GetParent(element);
            if (parent == null)
            {
                return null;
            }

            return parent as T ?? FindAncestorElement<T>(parent);
        }

        /// <summary>
        /// Finds the named instance of <see cref="{T}"/> that is a sub view of the element.
        /// This is not very optimized so it's best if used for something that you are
        /// sure there is only one instance of, otherwise it should be run asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of element to search for.</typeparam>
        /// <param name="element">The element whose children we are searching through.</param>
        /// <param name="name">The name of the element we are searching for.</param>
        /// <returns>The named instance of <see cref="{T}"/> found in the view tree.</returns>
        public static T FindNamedElement<T>(this FrameworkElement element, string name) where T : FrameworkElement
        {
            if (element == null)
            {
                return null;
            }

            T childElement = null;

            var childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;
                if (child == null)
                {
                    continue;
                }

                var castedChild = child as T;
                if (castedChild != null && castedChild.Name == name)
                {
                    childElement = castedChild;
                    break;
                }

                childElement = FindNamedElement<T>(child, name);
                if (childElement != null)
                {
                    break;
                }
            }

            return childElement;
        }

        /// <summary>
        /// Finds the first instance of <see cref="{T}"/> that is a sub view of the element.
        /// This is not very optimized so it's best if used for something that you are
        /// sure there is only one instance of, otherwise it should be run asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of element to search for.</typeparam>
        /// <param name="element">The element whose children we are searching through.</param>
        /// <returns>The first instance of <see cref="{T}"/> found in the view tree.</returns>
        public static T FindFirstElementByType<T>(this FrameworkElement element) where T : FrameworkElement
        {
            if (element == null)
            {
                return null;
            }

            T childElement = null;

            var childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;
                if (child == null)
                {
                    continue;
                }

                var castedChild = child as T;
                if (castedChild != null)
                {
                    childElement = castedChild;
                    break;
                }

                childElement = child as T ?? FindFirstElementByType<T>(child);
                if (childElement != null)
                {
                    break;
                }
            }

            return childElement;
        }
    }
}
