using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HKGoldenAPI.Types;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HKGolden
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewPostPage : Page
    {
        Post post;

        public ViewPostPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            SV_ViewChanging(null, null);
        }

        bool loadingPage;
        bool noMorePage;
        private async Task LoadNextPage()
        {
            loadingPage = true;
            int loadedPageIndex = -1;
            if (post == null)
            {
                HKGoldenAPI.Manager mgr = new HKGoldenAPI.Manager();
                post = await mgr.LoadPost("12", "6507545");
                txtTitle.Text = post.postTitle;
                loadedPageIndex = 0;
            }else
            {
                for(int i = 0; i < post.pages.Count; i++)
                {
                    if (!post.pages[i].pageLoaded)
                    {
                        await post.LoadPost(i + 1);
                        loadedPageIndex = i;
                        break;
                    }
                }
            }
            if (loadedPageIndex == -1)
            {
                noMorePage = true;
                return;
            }
            foreach (PostEntry entry in post.pages[loadedPageIndex].postEntries)
            {
                SP.Children.Add(new PostEntryDisplayItem() { displayEntry = entry, Margin = new Thickness(3) , HorizontalAlignment = HorizontalAlignment.Stretch});
            }
            if (loadedPageIndex == post.pages.Count - 1)
            {
                noMorePage = true;
                SP.Children.Add(new TextBlock() { Text = "No More Entries", Foreground = new SolidColorBrush() { Color = new Windows.UI.Color() { R = 0xAA, G = 0xFF, B = 0xAA, A = 0xFF } }, HorizontalAlignment = HorizontalAlignment.Center });
            }
            loadingPage = false;
        }

        public T GetVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                DependencyObject v = (DependencyObject)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                    child = GetVisualChild<T>(v);
                if (child != null)
                    break;
            }
            return child;
        }

        private async void SV_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (loadingPage || noMorePage) return;
            SV.ViewChanging -= SV_ViewChanging;
            if (SV.VerticalOffset > SV.ScrollableHeight - 20)
                await LoadNextPage();
            SV.ViewChanging += SV_ViewChanging;
        }

        private void SV_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            SV_ViewChanging(null, null);
        }
    }
}
