using System;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace App1
{
    public sealed partial class FacebookDialog : UserControl
    {
        public FacebookDialog(int width, int height)
        {
            this.InitializeComponent();

            Height = height;
            Width = width;
        }

        public void InitialiseFeed(string feedBaseUri, string link, string picture, string source, string name, string caption, string description, string redirectUri, Grid parent)
        {
            // Escape all params

            Uri fullFeedUri = new Uri(string.Format("{0}&link={1}", feedBaseUri, Uri.EscapeUriString(link)));

            IFrame.NavigationStarting += (s, e) =>
            {
                if (e.Uri.ToString() == redirectUri)
                {
                    parent.Children.Remove(this);
                }
            };

            IFrame.Navigate(fullFeedUri);

            parent.Children.Add(this);
        }
    }
}