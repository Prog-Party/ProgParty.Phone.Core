using Microsoft.ApplicationInsights;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace ProgParty.Core.Track
{
    public class Telemetry
    {
        public static Telemetry Instance = new Telemetry();
        
        private TelemetryClient _client { get; set; } = new TelemetryClient();

        private string _appName = Config.Instance.AppName;

        public void SendEmail(string from, string message) => Action("Email", new Dictionary<string, string> { { "from", from }, { "message", message } });

        public void PageVisit(Page p) => Action($"{p.GetType().Name} page visited");
        public void ReviewButtonClicked() => Action("Review button clicked");

        public void Action(string action) => _client.TrackEvent($"{_appName}-{action}");
        
        public void Action(string action, Dictionary<string, string> properties) => _client.TrackEvent($"{_appName}-{action}", properties);
    }
}
