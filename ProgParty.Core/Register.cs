using System.Threading.Tasks;
using Windows.ApplicationModel.Store;

namespace ProgParty.Core
{
    public class Register
    {
        public static async Task Execute()
        {
            var config = Config.Instance;

            if(config.RegisterShowNoConnectionMessage)
                Connection.Instance.ShowNoConnectionMessage();

            if (config.RegisterReviewPopup)
                await Review.Review.Instance.SetReviewPopup();

            if(config.RegisterPivotBackButton)
                PivotBackButton.Instance.Register(config.Pivot, config.Page);
        }

        public static void RegisterOnLoaded()
        {
            var config = Config.Instance;

            if (config.RegisterSetAds)
                Ads.Instance.RegisterAll();
        }

        public static void RegisterOnNavigatedTo(LicenseInformation licenseInformation)
        {
            Config.Instance.LicenseInformation = licenseInformation;

            var config = Config.Instance;

            if(config.RegisterSetAds)
                Ads.Instance.SetRemoveAds();

            if(config.RegisterPivotBackButton)
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += PivotBackButton.Instance.HardwareButtons_BackPressed;
        }
    }
}
