using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;

namespace ProgParty.Core.Share
{
    internal class ShareUrl
    {
        protected Uri ApplicationLink => GetApplicationLink(GetType().Name);

        public static Uri GetApplicationLink(string sharePageName) => new Uri("ms-sdk-sharesourcecs:navigate?page=" + sharePageName);

        private Uri _url = null;
        private string _description = null;
        private bool _success = false;

        public void RegisterForShare(MenuFlyoutItem menuFlyoutItem, Uri url, string description = "")
        {
            _url = url;
            _description = description;
            
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += ShareUrlHandler;
            DataTransferManager.ShowShareUI();
        }

        private void ShareUrlHandler(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (_url == null)
                return;

            DataRequest request = args.Request;
            request.Data.Properties.ApplicationName = Config.Instance.AppName;
            request.Data.Properties.Title = $"Share {Config.Instance.AppName} url";
            request.Data.Properties.Description = $"Share {Config.Instance.AppName} url";
            request.Data.Properties.ContentSourceApplicationLink = ApplicationLink;
            request.Data.SetDataProvider(StandardDataFormats.WebLink, new DataProviderHandler(this.OnDeferredImageRequestedHandler));

            if (_success)
                Core.Track.Telemetry.Instance.Action($"Url shared");
        }

        private void OnDeferredImageRequestedHandler(DataProviderRequest request)
        {
            if (_url != null)
            {
                // If the delegate is calling any asynchronous operations it needs to acquire
                // the deferral first. This lets the system know that you are performing some
                // operations that might take a little longer and that the call to SetData 
                // could happen after the delegate returns. Once you acquired the deferral object 
                // you must call Complete on it after your final call to SetData.
                DataProviderDeferral deferral = request.GetDeferral();

                // Make sure to always call Complete when finished with the deferral.
                try
                {
                    request.SetData(_url.AbsolutePath);
                }
                finally
                {
                    deferral.Complete();
                }
            }
        }
    }
}
