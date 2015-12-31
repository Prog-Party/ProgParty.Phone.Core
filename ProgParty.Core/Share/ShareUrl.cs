using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;

namespace ProgParty.Core.Share
{
    class ShareUrl
    {
        public async void RegisterForShare(MenuFlyoutItem menuFlyoutItem, string url)
        {
            //var saveImage = await new SaveImage().DoSaveImage(url);
            //_success = saveImage.Item1;
            //_imageFile = saveImage.Item2;

            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += ShareImageHandler;
            DataTransferManager.ShowShareUI();
        }

        private void ShareImageHandler(DataTransferManager sender, DataRequestedEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
