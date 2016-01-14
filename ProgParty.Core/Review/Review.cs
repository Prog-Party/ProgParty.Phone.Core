using ProgParty.Core.Storage;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.UI.Popups;

namespace ProgParty.Core.Review
{
    public class Review
    {
        public static Review Instance = new Review();
        public Uri ReviewUri { get; private set; }

        public Review()
        {
            var appId = CurrentApp.AppId.ToString();
            var reviewApp = "ms-windows-store:reviewapp";
            var uri = new Uri($"{reviewApp}?appid={appId}");
            ReviewUri = uri;
        }

        public async Task Execute()
        {
            var storage = new Storage.Storage();
            storage.StoreInRoaming(StorageKeys.ReviewDone, true);

            await Windows.System.Launcher.LaunchUriAsync(ReviewUri);
        }

        public async Task SetReviewPopup()
        {
            var storage = new Storage.Storage();

            bool reviewDoneValue = storage.LoadBoolFromRoaming(StorageKeys.ReviewDone);
            if (reviewDoneValue)
                return;

            int reviewValue = storage.LoadIntFromRoaming(StorageKeys.Review);

            if (reviewValue % 4 == 3)
            {
                string name = Config.Instance.AppName;
                MessageDialog reviewPopup;

                if (Config.Instance.language == Language.Dutch)
                    reviewPopup = new MessageDialog($"Vind je de {name} app een 5 sterren review waard, druk dan op 'Ja' en laat een review achter in de store. Dit helpt anderen om de {name} app makkelijker te vinden, want hoe beter de reviews hoe gemakkelijker de app te vinden is in de store.", $"Rate & Review {name}");
                else
                    reviewPopup = new MessageDialog($"Do you think this {name} app is worth 5 stars? Please press 'yes' and leave a review in the store. This helps other people to find the {name} app easier. The more and better the reviews are, the easier it is to find.", $"Rate & Review {name}");

                // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers

                if (Config.Instance.language == Language.Dutch)
                {
                    reviewPopup.Commands.Add(new UICommand("Ja", new UICommandInvokedHandler(this.CommandInvokedHandler), commandId: 0));
                    reviewPopup.Commands.Add(new UICommand("Misschien later"));
                }
                else
                {
                    reviewPopup.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(this.CommandInvokedHandler), commandId: 0));
                    reviewPopup.Commands.Add(new UICommand("Maybe later"));
                }
                
                // Set the command that will be invoked by default
                reviewPopup.DefaultCommandIndex = 0;

                // Set the command to be invoked when escape is pressed
                reviewPopup.CancelCommandIndex = 1;

                // Show the message dialog
                await reviewPopup.ShowAsync();
            }

            storage.StoreInRoaming(StorageKeys.Review, reviewValue + 1);
        }
        
        private async void CommandInvokedHandler(IUICommand command)
        {
            if (command.Id.ToString() == "0")
            {
                await Execute();
            }
        }
    }
}
