using HtmlAgilityPack;
using System.Linq;

namespace ProgParty.Core.Element.HtmlToRtf
{
    public class XmlConvert
    {
        public string Convert(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.OptionFixNestedTags = true;
            doc.OptionOutputAsXml = true;
            doc.LoadHtml(html);
            return doc.DocumentNode.Descendants("html").First().InnerHtml;
        }
    }
}
