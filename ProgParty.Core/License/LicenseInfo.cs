using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Storage;

namespace ProgParty.Core.License
{
    public class LicenseInfo
    {
        public static LicenseInfo Instance = new LicenseInfo();

        private bool ProxyFileIsLoaded = false;

        public LicenseInformation GetLicenseInformation(LicenseInformation license, bool isDebug)
        {
            if (!isDebug)
                return license;

            var licenseInfo = CurrentAppSimulator.LicenseInformation;
            Task.Run(() => LoadInAppPurchaseProxyFileAsync());
            return licenseInfo;
        }

        private async Task LoadInAppPurchaseProxyFileAsync()
        {
            if (ProxyFileIsLoaded)
                return;

            try
            {
                StorageFolder coreFolder = await Package.Current.InstalledLocation.GetFolderAsync("Core");
                StorageFolder licenseFolder = await coreFolder.GetFolderAsync("License");
                StorageFile proxyFile = await licenseFolder.GetFileAsync("WindowsStoreProxy.xml");
                await CurrentAppSimulator.ReloadSimulatorAsync(proxyFile);
                ProxyFileIsLoaded = true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }
    }
}
