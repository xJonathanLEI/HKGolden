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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HKGolden
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewPostPage : Page
    {
        public ViewPostPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            HKGoldenAPI.Manager mgr = new HKGoldenAPI.Manager();
            Post post = await mgr.LoadPost("12", "6505048");
            txtTitle.Text = post.postTitle;
            foreach (PostEntry entry in post.pages[0].postEntries)
            {
                SP.Children.Add(new PostEntryDisplayItem() { displayEntry = entry, Margin = new Thickness(3) });
            }
        }
    }
}
