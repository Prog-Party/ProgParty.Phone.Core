using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Notifications;

namespace ProgParty.Core.Image
{
    public class SaveImage
    {
        public async void RegisterForSave(string url)
        {
            bool success = (await DoSaveImage(url)).Item1;

            if (success)
            {
                Track.Telemetry.Instance.Action($"Image saved");
                Notification.Toast.Instance.Notify("Image Saved", ToastTemplateType.ToastImageAndText02, 3.0);
            }
        }

        internal async Task<Tuple<bool, StorageFile>> DoSaveImage(string imageUrl)
        {
            try
            {
                string fileName = Path.GetFileName(imageUrl);
                var url = new Uri(imageUrl);

                StorageFolder storageFolder = await KnownFolders.PicturesLibrary.CreateFolderAsync(Config.Instance.AppName, CreationCollisionOption.OpenIfExists);

                var thumbnail = RandomAccessStreamReference.CreateFromUri(url);

                StorageFile remoteFile = await StorageFile.CreateStreamedFileFromUriAsync(fileName, url, thumbnail);
                await remoteFile.CopyAsync(storageFolder, fileName, NameCollisionOption.ReplaceExisting);

                return new Tuple<bool, StorageFile>(true, remoteFile);
            }
            catch (Exception e)
            {
                return new Tuple<bool, StorageFile>(false, null);
            }
        }
    }
}
