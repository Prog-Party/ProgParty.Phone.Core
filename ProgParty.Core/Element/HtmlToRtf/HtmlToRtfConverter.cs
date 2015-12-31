using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ProgParty.Core.Element.HtmlToRtf
{
    public class HtmlToRtfConverter
    {
        public static string GetHtml(DependencyObject obj)
        {
            return (string)obj.GetValue(HtmlProperty);
        }

        public static void SetHtml(DependencyObject obj, string value)
        {
            obj.SetValue(HtmlProperty, value);
        }

        // Using a DependencyProperty as the backing store for Html.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HtmlProperty =
            DependencyProperty.RegisterAttached("Html", typeof(string), typeof(HtmlToRtfConverter), new PropertyMetadata("", OnHtmlChanged));

        private static void OnHtmlChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            string html = (string)eventArgs.NewValue;
            RichTextBlock parent = (RichTextBlock)sender;
            parent.Blocks.Clear();
            
            Convert.FromHtml(parent, html);
        }
    }
}
