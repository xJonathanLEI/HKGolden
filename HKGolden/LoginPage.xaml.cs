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
using HKGoldenAPI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HKGolden
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            Manager mgr = new Manager();
            await mgr.Login("14111812d@connect.polyu.hk", "k813pcy");
        }
    }
}
