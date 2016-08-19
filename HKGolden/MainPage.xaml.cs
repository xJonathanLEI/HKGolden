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
using Windows.UI.Xaml.Media.Imaging;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HKGolden
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            HKGoldenAPI.Manager mgr = new HKGoldenAPI.Manager();
            await mgr.LoadHomePage();
            foreach (GalleryItem g in mgr.homepageGallery)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(g.imageURL));
                FV.Items.Add(new Image() { Source = bitmap, Stretch = Stretch.UniformToFill  });
            }
            foreach (Article a in mgr.homepageArticles)
            {
                GridViewItem GVI = new GridViewItem() { Margin = new Thickness(5, 0, 2, 15), Content = new ArticleDisplayItem() { displayArticle = a} };
                articleGV.Items.Add(GVI);
            }
            Page_SizeChanged(null, null);
            await mgr.LoadPost("12", "6505048");
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (articleGV.Items.Count != 0)
            {
                ((GridViewItem)articleGV.Items[0]).Width = (this.ActualWidth >= 310 ? (this.ActualWidth - 10) / (Math.Floor(this.ActualWidth / 310)) - 5 : this.ActualWidth - 10);
                FV.Width = Math.Min(this.ActualWidth, 500);
                FV.Height = 297.0 / 445 * FV.Width;
            }
        }
    }
}
