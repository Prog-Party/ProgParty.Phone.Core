using ProgParty.Core.Review;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ProgParty.Core.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Contact : Page
    {
        public Contact()
        {
            this.InitializeComponent();
            Track.Telemetry.Instance.PageVisit(this);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif
        }

#if WINDOWS_PHONE_APP
        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }
#endif
        
        private async void ButtonReview_Click(object sender, RoutedEventArgs e)
        {
            Track.Telemetry.Instance.ReviewButtonClicked();

            await new Review.Review().Execute();
        }

        private void FotoJensDennis_Loaded(object sender, RoutedEventArgs e)
        {
            FotoJensDennis.Width = Window.Current.Bounds.Width;
        }

        private async void DennisMail_Click(object sender, RoutedEventArgs e)
        {
#if WINDOWS_PHONE_APP
            //predefine Recipient
            Windows.ApplicationModel.Email.EmailRecipient sendTo = new Windows.ApplicationModel.Email.EmailRecipient()
            {
                Address = "dennis.rosenbaum@outlook.com"
            };
            Windows.ApplicationModel.Email.EmailRecipient sendCc = new Windows.ApplicationModel.Email.EmailRecipient()
            {
                Address = "jens.denbraber@gmail.com"
            };

            //generate mail object
            Windows.ApplicationModel.Email.EmailMessage mail = new Windows.ApplicationModel.Email.EmailMessage();
            mail.Subject = "Hi Prog Party!";
            mail.Body = "You are great";

            //add recipients to the mail object
            mail.To.Add(sendTo);
            mail.CC.Add(sendCc);

            //open the share contract with Mail only:
            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(mail);
#endif
        }
    }
}
