using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.UI.Popups;

// The WebView Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace Test2
{
    public sealed partial class MainPage : Page
    {
        // TODO: Replace with your URL here.
        private static readonly Uri HomeUri = new Uri("ms-appx-web:///Html/index.html", UriKind.Absolute);

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            WebViewControl.ScriptNotify +=WebViewControl_ScriptNotify;
        }

        private async void WebViewControl_ScriptNotify(object sender, NotifyEventArgs e)
        {
            Debug.WriteLine("Close App and open Rosary in Browser?");
            var messageDialog = new MessageDialog("Close App and open Rosary in Browser?");
            messageDialog.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler(this.OpenRosaryHandler)));
            messageDialog.Commands.Add(new UICommand("Close", new UICommandInvokedHandler(this.ReloadHandler)));

            messageDialog.DefaultCommandIndex = 0;

            messageDialog.CancelCommandIndex = 1;

            await messageDialog.ShowAsync();

        }

        private void OpenRosaryHandler(IUICommand command)
        {
            string uriToLaunch = @"http://icchurchborivali.org/m/prayers-rosary.html";
            var uri = new Uri(uriToLaunch);
            DefaultLaunch(uri);
        }

        async void DefaultLaunch(Uri abc)
        {
            // Launch the URI
            var success = await Windows.System.Launcher.LaunchUriAsync(abc);

            if (success)
            {
                // URI launched
            }
            else
            {
                // URI launch failed
            }
        }
 
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            WebViewControl.Navigate(HomeUri);

            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();

            // Hide the status bar
            await statusBar.HideAsync();

            HardwareButtons.BackPressed += this.MainPage_BackPressed;
        }

        /// <summary>
        /// Invoked when this page is being navigated away.
        /// </summary>
        /// <param name="e">Event data that describes how this page is navigating.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= this.MainPage_BackPressed;
        }

        /// <summary>
        /// Overrides the back button press to navigate in the WebView's back stack instead of the application's.
        /// </summary>
        private void MainPage_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (WebViewControl.CanGoBack)
            {
                WebViewControl.GoBack();
                e.Handled = true;
            }
        }

        private async void Browser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (!args.IsSuccess)
            {
                Debug.WriteLine("Navigation to this page failed, check your internet connection.");
                var messageDialog = new MessageDialog("No internet connection has been found.");
                messageDialog.Commands.Add(new UICommand("Refresh", new UICommandInvokedHandler(this.ReloadHandler)));
                messageDialog.Commands.Add(new UICommand( "Close", new UICommandInvokedHandler(this.CommandInvokedHandler)));

                messageDialog.DefaultCommandIndex = 0;

                messageDialog.CancelCommandIndex = 1;

                await messageDialog.ShowAsync();
            }
        }

        private void ReloadHandler(IUICommand command)
        {
            WebViewControl.Refresh();
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            if (WebViewControl.CanGoBack)
            {
                WebViewControl.GoBack();
            }
        }

        /// <summary>
        /// Navigates forward in the WebView's history.
        /// </summary>
        private void ForwardAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (WebViewControl.CanGoForward)
            {
                WebViewControl.GoForward();
            }
        }



        /// <summary>
        /// Navigates to the initial home page.
        /// </summary>
        //private void HomeAppBarButton_Click(object sender, RoutedEventArgs e)
        //{
        //    WebViewControl.Navigate(HomeUri);
        //}

        //private void PrayerAppBarButton_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void ContactAppBarButton_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void AnnouncementsAppBarButton_Click(object sender, RoutedEventArgs e)
        //{
            
        //    Frame.Navigate(typeof(External), AnnouncementsAppBarButton.Content);
        //}

        /// Hide top status bar
    }
}
