[assembly: Xamarin.Forms.Platform.UWP.ExportRenderer(
    typeof(TournamentTest.Views.ListView),
    typeof(TournamentTest.UWP.Renderers.ListViewRenderer))]
namespace TournamentTest.UWP.Renderers
{
    using Xamarin.Forms.Platform.UWP;
    using Xaml = global::Windows.UI.Xaml;
    using PortableListView = Views.ListView;
    using WindowsListView = global::Windows.UI.Xaml.Controls.ListView;
    using WindowsScrollViewer = global::Windows.UI.Xaml.Controls.ScrollViewer;
    using Extensions;
    using Xamarin.Forms;

    /// <summary>
    /// Windows renderer for a <see cref="Views.ListView"/>.
    /// </summary>
    public class ListViewRenderer : Xamarin.Forms.Platform.UWP.ListViewRenderer
    {
        #region Fields

        private WindowsListView _control;
        private WindowsScrollViewer _scrollViewer;

        #endregion

        #region Protected Methods

        /// <summary>
        /// Called when the element changes.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            if (e.OldElement != null)
            {
                _control.Loaded -= OnLoaded;
            }

            _control = Control as WindowsListView;
            _control.Loaded += OnLoaded;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Called when the <see cref="WindowsListView"/> has been constructed and added to the object tree.
        /// </summary>
        /// <param name="sender">The sender that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLoaded(object sender, Xaml.RoutedEventArgs e)
        {
            _scrollViewer = _control.FindFirstElementByType<WindowsScrollViewer>();
            if (_scrollViewer == null)
            {
                return;
            }

            BindScrolledEvent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Bind the scrolled event to the portable event handler.
        /// </summary>
        private void BindScrolledEvent()
        {
            var element = Element as PortableListView;

            _scrollViewer.RegisterPropertyChangedCallback(WindowsScrollViewer.VerticalOffsetProperty, (sender, property) =>
            {
                var args = new ScrolledEventArgs(0, _scrollViewer.VerticalOffset);
                element?.OnScrolled(args);
            });
        }

        #endregion
    }
}
