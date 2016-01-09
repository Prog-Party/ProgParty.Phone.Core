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

        public static Func<Task<StorageFile>> GetProxyFile = () => null;

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
                StorageFile proxyFile = await GetProxyFile();
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
