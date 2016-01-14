using System;

namespace ProgParty.Core.Store
{
    public class Routing
    {
        public static async void RouteToPublisher(string publisher = "Prog Party")
        {
            var appOverviewUri = new Uri($"zune:search?publisher={publisher}");                       //windows 8.x

            //var appOverviewUri = new Uri($"ms-windows-store://publisher/?name={publisher}");        //windows 10 

            await Windows.System.Launcher.LaunchUriAsync(appOverviewUri);
        }
    }
}
