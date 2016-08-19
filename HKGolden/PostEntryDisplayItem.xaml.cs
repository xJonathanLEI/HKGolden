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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HKGolden
{
    public sealed partial class PostEntryDisplayItem : UserControl
    {
        PostEntry entry;
        public PostEntry displayEntry { get { return entry; } set {
                entry = value;
                txtUsername.Text = entry.entryAuthor.nickname;
                txtTime.Text = entry.postTime.ToString("yyyy-MM-dd HH:mm");
                SP.Children.Add(PostRenderer.RenderHTML(entry.entryContent));
            } }

        public PostEntryDisplayItem()
        {
            this.InitializeComponent();
        }
    }
}
