using Windows.Data.Xml.Dom;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using System.Linq;
using Windows.UI.Xaml.Media;
using System;
using Windows.UI.Xaml.Media.Imaging;

namespace ProgParty.Core.Element.HtmlToRtf
{
    public class Convert
    {
        public static void FromHtml(RichTextBlock rtb, string html)
        {
            var oldHtml = html;
            html = System.Net.WebUtility.HtmlDecode(html);
            html = html.Replace("&bull;", "<br />    - ");
            html = html.Replace("&middot;", "<br />    - ");
            html = html.Replace("<li>", "    - ").Replace("</li>", "<br />").Replace("<ul>","<p>").Replace("</ul>","</p>");
            
            html = html.Replace("<br /><br /><p>", "<br /><p>");

            html = $"<html><body><p>{html}</p></body></html>";
            var v = new XmlConvert();
            var htmlFromDoc = v.Convert(html);
            htmlFromDoc = htmlFromDoc.Replace("<p />", "");
            XmlDocument document = new XmlDocument();
            document.LoadXml(htmlFromDoc);

            XmlElement elem = (XmlElement)(document.GetElementsByTagName("body")[0]);

            var container = new RichTextBlockTextContainer(rtb);
            
            ParseElement(elem, container);
        }
        
        static bool isInP = false;
        private static void ParseElement(XmlElement element, ITextContainer parent)
        {
            foreach (var child in element.ChildNodes)
            {
                var tagNameToUpper = (child as XmlElement)?.TagName?.ToUpper();

                if (!isInP
                    && !(child is XmlElement && (tagNameToUpper == "P" || tagNameToUpper == "DIV" || tagNameToUpper == "IMG")))
                {
                    var paragraph = new Paragraph();
                    parent.Add(paragraph);
                    isInP = true;
                    parent = new ParagraphTextContainer(parent, paragraph);
                }

                if (child is XmlText)
                {
                    var text = child.InnerText.Replace("\n", "").Trim();
                    if (string.IsNullOrEmpty(text))
                        continue;

                    text = text.Replace("</form>", "");
                    parent.Add(new Run { Text = System.Net.WebUtility.HtmlDecode(text), Foreground = new SolidColorBrush(Windows.UI.Colors.Black) });
                }
                else if (child is XmlElement)
                {
                    XmlElement e = (XmlElement)child;
                    switch (tagNameToUpper)
                    {
                        case "P":
                        case "DIV":
                            if (isInP)
                            {
                                if (tagNameToUpper == "P")
                                    parent.Add(new LineBreak());
                                ParseElement(e, parent);
                            }
                            else
                            {
                                var paragraph = new Paragraph();
                                parent.Add(paragraph);
                                isInP = true;
                                ParseElement(e, new ParagraphTextContainer(parent, paragraph));
                                isInP = false;
                            }
                            break;
                        case "B":
                        case "STRONG":
                            var bold = new Bold();
                            parent.Add(bold);
                            ParseElement(e, new SpanTextContainer(parent, bold));
                            break;
                        case "I":
                        case "EM":
                            var italic = new Italic();
                            parent.Add(italic);
                            ParseElement(e, new SpanTextContainer(parent, italic));
                            break;
                        case "U":
                            var underline = new Underline();
                            parent.Add(underline);
                            ParseElement(e, new SpanTextContainer(parent, underline));
                            break;
                        case "A":
                            var url = e.Attributes.FirstOrDefault(a => a.NodeName == "href")?.NodeValue;
                            Uri uri;
                            if (url != null && Uri.TryCreate(url.ToString(), UriKind.Absolute, out uri))
                            {
                                var link = new Hyperlink();
                                link.NavigateUri = uri;
                                parent.Add(link);
                                ParseElement(e, new SpanTextContainer(parent, link));
                            }
                            else
                            {
                                ParseElement(e, parent);
                            }

                            break;
                        case "BR":
                            parent.Add(new LineBreak());
                            break;
                        case "IMG":
                            var src = e.Attributes.FirstOrDefault(a => a.NodeName == "src")?.NodeValue;
                            Uri srcUri;
                            if (src != null && Uri.TryCreate(src.ToString(), UriKind.Absolute, out srcUri))
                            {
                                var img = new InlineUIContainer
                                {
                                    Child = new Windows.UI.Xaml.Controls.Image()
                                    {
                                        Source = new BitmapImage(srcUri)
                                    }
                                };

                                parent.AddToParentParagraph(img);
                            }
                            break;
                    }
                }
            }
        }
    }
}
