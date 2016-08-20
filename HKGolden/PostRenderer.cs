using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml;
using HKGoldenAPI;
using Windows.UI.Xaml.Media;
using HtmlAgilityPack;
using Windows.UI.Xaml.Media.Imaging;

namespace HKGolden
{
    public static class PostRenderer
    {
        public static RichTextBlock RenderHTML(string html)
        {
            RichTextBlock rtb = new RichTextBlock() { LineHeight = 0, Margin = new Thickness(10), FontSize = 18, Foreground = new SolidColorBrush() { Color = Windows.UI.Colors.White } };
            List<string> paraHtml = new List<string>();
            while (html.Contains("<br>"))
                paraHtml.Add(Utilities.ClearPostContent(Utilities.ReadUntil(ref html, "<br>")));
            if (html != "") paraHtml.Add(Utilities.ClearPostContent(html));
            foreach (string para in paraHtml)
            {
                Paragraph paragraph = new Paragraph() { Margin = new Thickness(0) , LineHeight = 0};
                HtmlDocument paragraphDocument = new HtmlDocument();
                paragraphDocument.LoadHtml(para);
                foreach (HtmlNode n in paragraphDocument.DocumentNode.ChildNodes)
                    switch (n.Name)
                    {
                        case "#text":
                            paragraph.Inlines.Add(new Run() { Text = n.InnerText });
                            break;
                        case "img":
                            if (n.Attributes.Contains("alt"))
                            {
                                string emoji = n.Attributes["src"].Value;
                                emoji = emoji.Substring(emoji.LastIndexOf("/") + 1);
                                paragraph.Inlines.Add(new InlineUIContainer() { Child = new Image() { Stretch = Stretch.None, Source = new BitmapImage(new Uri("ms-appx:///res/stickers/" + emoji, UriKind.Absolute)) } });
                            }
                            else
                                paragraph.Inlines.Add(new InlineUIContainer() { Child = new Image() { Stretch = Stretch.None, Source = new BitmapImage(new Uri(Constants.URL_HOMEPAGE + n.Attributes["src"].Value)) } });
                            break;
                        case "a":
                            if (n.Descendants("img").Count() == 0)
                            {
                                paragraph.Inlines.Add(new InlineUIContainer() { Child = new HyperlinkButton() { Padding = new Thickness(0, 0, 0, -3), Foreground = new SolidColorBrush() { Color = new Windows.UI.Color() { R = 0xAA, G = 0xFF, B = 0xAA, A = 0xFF} }, FontSize = 18, NavigateUri = new Uri(n.Attributes["href"].Value), Content = n.Attributes["href"].Value} });
                            }
                            else
                            {
                                paragraph.Inlines.Add(new InlineUIContainer() { Child = new Image() { Stretch = Stretch.UniformToFill, MaxWidth = 300, Source = new BitmapImage(new Uri(n.Descendants("img").ElementAt(0).Attributes["src"].Value)) } });
                            }
                            break;
                        default:
                            paragraph.Inlines.Add(new Run() { Text = "[UNKNOWN]" });
                            break;
                    }
                rtb.Blocks.Add(paragraph);
            }
            return rtb;
        }
    }
}
