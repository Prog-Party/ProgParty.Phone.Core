using Windows.UI.Xaml.Documents;

namespace ProgParty.Core.Element.HtmlToRtf
{
    internal interface ITextContainer
    {
        void Add(Inline text);
        void Add(Block paragraph);
        Paragraph AddToParentParagraph(Inline paragraph);
    }
}
