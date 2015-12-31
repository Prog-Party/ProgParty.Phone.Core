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

        public static LicenseInformation LicenseInformation;

        private bool ProxyFileIsLoaded = false;

        public LicenseInformation GetLicenseInformation(LicenseInformation license)
        {
#if DEBUG
            var licenseInfo = CurrentAppSimulator.LicenseInformation;
            Task.Run(() => LoadInAppPurchaseProxyFileAsync());
            return licenseInfo;
#else
            return license;
#endif
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
