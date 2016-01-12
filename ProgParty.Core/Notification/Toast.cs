using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace ProgParty.Core.Notification
{
    public class Toast
    {
        private ToastNotifier _toastNotifier { get; set; } = ToastNotificationManager.CreateToastNotifier();

        public static Toast Instance = new Toast();

        public void Notify(string text, ToastTemplateType toastTemplateType, double duration, string imageUrl = null)
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplateType);

            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");          //Text Notification    
            toastTextElements[0].InnerText = text;

            if(imageUrl != null)
            {
                XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
                ((XmlElement)toastImageAttributes[0]).SetAttribute("src", imageUrl);
                ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", text);
            }

            IXmlNode toastNode = toastXml.SelectSingleNode("/toast"); //Create toast node so you can add separete audio and duration    

            ((XmlElement)toastNode).SetAttribute("duration", "long");     //Toast Duration short or long [optional]    

            ToastNotification toast = new ToastNotification(toastXml);                      //create object of toast notificaion     

            toast.ExpirationTime = DateTimeOffset.UtcNow.AddSeconds(duration);                  //Auto remove Notificaiton [Optional]    

            _toastNotifier.Show(toast);
        }
    }
}
