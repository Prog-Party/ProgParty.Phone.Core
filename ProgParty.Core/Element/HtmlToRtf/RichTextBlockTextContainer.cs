using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace ProgParty.Core.Element.HtmlToRtf
{
    internal sealed class RichTextBlockTextContainer : ITextContainer
    {
        private readonly RichTextBlock _richTextBlock;

        public RichTextBlockTextContainer(RichTextBlock richTextBlock)
        {
            _richTextBlock = richTextBlock;
        }

        public void Add(Inline text)
        {
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(text);
            Add(paragraph);
        }

        public Paragraph AddToParentParagraph(Inline text)
        {
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(text);
            Add(paragraph);
            return paragraph;
        }

        public void Add(Block paragraph)
        {
            _richTextBlock.Blocks.Add(paragraph);
        }
    }
}
