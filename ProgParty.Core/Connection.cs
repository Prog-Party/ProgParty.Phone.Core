using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;

namespace ProgParty.Core
{
    public class Connection
    {
        public static bool HasInternetAccess { get; private set; }
        
        public static Connection Instance { get; } = new Connection();

        public Connection()
        {
            NetworkInformation.NetworkStatusChanged += NetworkInformationOnNetworkStatusChanged;
            CheckInternetAccess();
        }

        private void NetworkInformationOnNetworkStatusChanged(object sender)
        {
            CheckInternetAccess();
        }

        private void CheckInternetAccess()
        {
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            HasInternetAccess = (connectionProfile != null &&
                                 connectionProfile.GetNetworkConnectivityLevel() ==
                                 NetworkConnectivityLevel.InternetAccess);
        }

        internal async Task ShowNoConnectionMessage()
        {
            if (!HasInternetAccess)
                await new MessageDialog("Geen internet verbinding aanwezig :(").ShowAsync();

        }
    }
}
