using Microsoft.Advertising.Mobile.UI;
using System;
using Img = Windows.UI.Xaml.Controls.Image;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace ProgParty.Core
{
    public class Ads
    {
        public static Ads Instance = new Ads();
        private bool HideAllAds { get; set; } = false;

        private AdControl AdControlSmall;
        private AdControl AdControlMedium;
        private AdControl AdControlLarge;
        private Img ProgPartyBanner;


        public void RegisterAll()
        {
            if (!Config.Instance.RegisterSetAds)
                return;

            var ads = Config.Instance.Ad;
            var adParent = ads.AdHolder;

            AdControlSmall = new AdControl(ads.AdApplicationId, ads.SmallAdUnitId, true)
            {
                Width = 300,
                Height = 50,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            AdControlSmall.ErrorOccurred += AdControlSmall_ErrorOccurred;
            AdControlSmall.AdRefreshed += AdControlSmall_Refreshed;
            AdControlSmall.Loaded += AdControlSmall_Loaded;

            AdControlMedium = new AdControl(ads.AdApplicationId, ads.MediumAdUnitId, true)
            {
                Width = 480,
                Height = 80,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            AdControlMedium.ErrorOccurred += AdControlMedium_ErrorOccurred;
            AdControlMedium.AdRefreshed += AdControlMedium_Refreshed;
            AdControlMedium.Loaded += AdControlMedium_Loaded;

            AdControlLarge = new AdControl(ads.AdApplicationId, ads.LargeAdUnitId, true)
            {
                Width = 640,
                Height = 100,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            AdControlLarge.ErrorOccurred += AdControlLarge_ErrorOccurred;
            AdControlLarge.AdRefreshed += AdControlLarge_Refreshed;
            AdControlLarge.Loaded += AdControlLarge_Loaded;

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri("ms-appx:///Core/Assets/banner_Jens_Dennis.png");

            ProgPartyBanner = new Img()
            {
                Source = bitmapImage,
                Width = 480,
                Height = 80,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            ProgPartyBanner.PointerReleased += ProgPartyBanner_PointerReleased;

            adParent.Children.Add(AdControlSmall);
            adParent.Children.Add(AdControlMedium);
            adParent.Children.Add(AdControlLarge);
            adParent.Children.Add(ProgPartyBanner);
        }

        private async void ProgPartyBanner_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var appOverviewUri = new Uri("zune:search?publisher=Prog Party");                       //windows 8.x

            //var appOverviewUri = new Uri("ms-windows-store://publisher/?name=Prog Party");        //windows 10 

            await Windows.System.Launcher.LaunchUriAsync(appOverviewUri);
        }

        internal void SetRemoveAds()
        {
            var config = Config.Instance;
            if (config.LicenseInformation.ProductLicenses[InAppPurchase.TokenRemoveAdvertisement].IsActive)
            {
                HideAllAds = true;
                AdControlSmall.Visibility = Visibility.Collapsed;
                AdControlMedium.Visibility = Visibility.Collapsed;
                AdControlLarge.Visibility = Visibility.Collapsed;
                ProgPartyBanner.Visibility = Visibility.Collapsed;
            }
            else
            {
                HideAllAds = false;
            }
        }

        bool addControlMediumError = false;
        bool addControlSmallError = false;
        bool addControlLargeError = false;
        bool addControlMediumLoaded = false;
        bool addControlSmallLoaded = false;
        bool addControlLargeLoaded = false;

        private void AdControlMedium_ErrorOccurred(object sender, Microsoft.Advertising.Mobile.Common.AdErrorEventArgs e)
        {
            addControlMediumError = true;
            HideAdControllers();
        }

        private void AdControlSmall_ErrorOccurred(object sender, Microsoft.Advertising.Mobile.Common.AdErrorEventArgs e)
        {
            addControlSmallError = true;
            HideAdControllers();
        }
        private void AdControlLarge_ErrorOccurred(object sender, Microsoft.Advertising.Mobile.Common.AdErrorEventArgs e)
        {
            addControlLargeError = true;
            HideAdControllers();
        }

        private void AdControlMedium_Loaded(object sender, RoutedEventArgs e)
        {
            addControlMediumLoaded = true;
            HideAdControllers();
        }
        private void AdControlSmall_Loaded(object sender, RoutedEventArgs e)
        {
            addControlSmallLoaded = true;
            HideAdControllers();
        }
        private void AdControlLarge_Loaded(object sender, RoutedEventArgs e)
        {
            addControlLargeLoaded = true;
            HideAdControllers();
        }

        private void AdControlMedium_Refreshed(object sender, RoutedEventArgs e)
        {
            addControlMediumError = false;
            HideAdControllers();
        }

        private void AdControlSmall_Refreshed(object sender, RoutedEventArgs e)
        {
            addControlSmallError = false;
            HideAdControllers();
        }

        private void AdControlLarge_Refreshed(object sender, RoutedEventArgs e)
        {
            addControlLargeError = false;
            HideAdControllers();
        }

        private void HideAdControllers()
        {
            if (HideAllAds)
            {
                AdControlSmall.Visibility = Visibility.Collapsed;
                AdControlMedium.Visibility = Visibility.Collapsed;
                AdControlLarge.Visibility = Visibility.Collapsed;
                ProgPartyBanner.Visibility = Visibility.Collapsed;
                Config.Instance.Ad.AdHolder.Visibility = Visibility.Collapsed;
                return;
            }

            if (!addControlMediumLoaded || !addControlSmallLoaded || !addControlLargeLoaded)
            {
                ProgPartyBanner.Visibility = Visibility.Visible;
                return;
            }
                
            AdControlSmall.Visibility = Visibility.Collapsed;
            AdControlMedium.Visibility = Visibility.Collapsed;
            AdControlLarge.Visibility = Visibility.Collapsed;
            ProgPartyBanner.Visibility = Visibility.Collapsed;

            if (addControlMediumError && addControlSmallError && addControlLargeError)
            {
                ProgPartyBanner.Width = Window.Current.Bounds.Width;
                ProgPartyBanner.Visibility = Visibility.Visible;
            }
            else
            if (Window.Current.Bounds.Width < 480)
            {
                if (!addControlSmallError)
                    AdControlSmall.Visibility = Visibility.Visible;
                else if (!addControlMediumError)
                    AdControlMedium.Visibility = Visibility.Visible;
                else if (!addControlLargeError)
                    AdControlLarge.Visibility = Visibility.Visible;
                else
                {
                    ProgPartyBanner.Width = Window.Current.Bounds.Width;
                    ProgPartyBanner.Visibility = Visibility.Visible;
                }
            }
            else if (Window.Current.Bounds.Width < 640)
            {
                if (!addControlMediumError)
                    AdControlMedium.Visibility = Visibility.Visible;
                else if (!addControlSmallError)
                    AdControlSmall.Visibility = Visibility.Visible;
                else if (!addControlLargeError)
                    AdControlLarge.Visibility = Visibility.Visible;
                else
                {
                    ProgPartyBanner.Width = Window.Current.Bounds.Width;
                    ProgPartyBanner.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (!addControlLargeError)
                    AdControlLarge.Visibility = Visibility.Visible;
                else if (!addControlMediumError)
                    AdControlMedium.Visibility = Visibility.Visible;
                else if (!addControlSmallError)
                    AdControlSmall.Visibility = Visibility.Visible;
                else
                {
                    ProgPartyBanner.Width = Window.Current.Bounds.Width;
                    ProgPartyBanner.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
