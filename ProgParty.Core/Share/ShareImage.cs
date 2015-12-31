using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace ProgParty.Core.Image
{
    internal class ShareImage
    {
        protected Uri ApplicationLink => GetApplicationLink(GetType().Name);

        public static Uri GetApplicationLink(string sharePageName) =>  new Uri("ms-sdk-sharesourcecs:navigate?page=" + sharePageName);

        private StorageFile _imageFile = null;
        private bool _success = false;

        public async void RegisterForShare(MenuFlyoutItem menuFlyoutItem, string url)
        {           
            //var saveImage = await new SaveImage().DoSaveImage(url);
            //_success = saveImage.Item1;
            //_imageFile = saveImage.Item2;

            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += ShareImageHandler;
            DataTransferManager.ShowShareUI();
        }

        private void ShareImageHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            if (_imageFile == null)
                return;

            DataRequest request = e.Request;
            request.Data.Properties.ApplicationName = Config.Instance.AppName;
            request.Data.Properties.Title = $"Share {Config.Instance.AppName} Image";
            request.Data.Properties.Description = $"Share {Config.Instance.AppName} Image";
            request.Data.Properties.ContentSourceApplicationLink = ApplicationLink;
            request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromFile(_imageFile);
            request.Data.SetDataProvider(StandardDataFormats.Bitmap, new DataProviderHandler(this.OnDeferredImageRequestedHandler));

            if (_success)
                Core.Track.Telemetry.Instance.Action($"Image shared");
        }

        private async void OnDeferredImageRequestedHandler(DataProviderRequest request)
        {
            // In this delegate we provide updated Bitmap data using delayed rendering.

            if (_imageFile != null)
            {
                // If the delegate is calling any asynchronous operations it needs to acquire
                // the deferral first. This lets the system know that you are performing some
                // operations that might take a little longer and that the call to SetData 
                // could happen after the delegate returns. Once you acquired the deferral object 
                // you must call Complete on it after your final call to SetData.
                DataProviderDeferral deferral = request.GetDeferral();
                InMemoryRandomAccessStream inMemoryStream = new InMemoryRandomAccessStream();

                // Make sure to always call Complete when finished with the deferral.
                try
                {
                    // Decode the image and re-encode it at 50% width and height.
                    IRandomAccessStream imageStream = await _imageFile.OpenAsync(FileAccessMode.Read);
                    BitmapDecoder imageDecoder = await BitmapDecoder.CreateAsync(imageStream);
                    BitmapEncoder imageEncoder = await BitmapEncoder.CreateForTranscodingAsync(inMemoryStream, imageDecoder);
                    imageEncoder.BitmapTransform.ScaledWidth = (uint)(imageDecoder.OrientedPixelWidth * 0.5);
                    imageEncoder.BitmapTransform.ScaledHeight = (uint)(imageDecoder.OrientedPixelHeight * 0.5);
                    await imageEncoder.FlushAsync();

                    request.SetData(RandomAccessStreamReference.CreateFromStream(inMemoryStream));
                }
                finally
                {
                    deferral.Complete();
                }
            }
        }
    }
}
