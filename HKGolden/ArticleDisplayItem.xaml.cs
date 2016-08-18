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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HKGolden
{
    public sealed partial class ArticleDisplayItem : UserControl
    {
        Article article;
        public Article displayArticle { get { return article; } set {
                article = value;
                featureImg.Source = new BitmapImage(new Uri(article.featurePhotoURL));
                txtAuthor.Text = article.author;
                txtTitle.Text = article.title;
                txtTime.Text = article.time;
            } }

        public ArticleDisplayItem()
        {
            this.InitializeComponent();
        }
    }
}
