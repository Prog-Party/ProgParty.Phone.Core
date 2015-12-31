using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml.Controls;

namespace ProgParty.Core
{
    public class Config
    {
        public static Config Instance { get; set; }

        public Config(Page page)
        {
            Page = page;
        }
        
        public bool RegisterShowNoConnectionMessage { get; set; } = true;
        public bool RegisterReviewPopup { get; set; } = true;
        public bool RegisterPivotBackButton { get; set; } = true;

        public bool RegisterSetAds => Ad != null;

        public ConfigAd Ad { get; set; }

        public Page Page { get; private set; }
        public Pivot Pivot { get; set; }

        public LicenseInformation LicenseInformation { get; set; }

        public string AppName { get; set; }

        internal void SetLicenseInformation(LicenseInformation licenseInformation)
        {
            LicenseInformation = License.LicenseInfo.Instance.GetLicenseInformation(licenseInformation);
        }
    }

    public class ConfigAd
    {
        public Grid AdHolder { get; set; }
        public string AdApplicationId { get; set; }
        public string SmallAdUnitId { get; set; }
        public string MediumAdUnitId { get; set; }
        public string LargeAdUnitId { get; set; }
    }
}
