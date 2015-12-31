using System;
using Windows.UI.Xaml.Documents;

namespace ProgParty.Core.Element.HtmlToRtf
{
    internal sealed class ParagraphTextContainer : ITextContainer
    {
        private readonly ITextContainer _parent;
        private readonly Paragraph _block;

        public ParagraphTextContainer(ITextContainer parent, Paragraph block)
        {
            _block = block;
            _parent = parent;
        }

        public void Add(Inline text)
        {
            _block.Inlines.Add(text);
        }

        public Paragraph AddToParentParagraph(Inline text) {
            Add(text);
            return _block;
        }

        public void Add(Block paragraph)
        {
            throw new NotSupportedException();
        }
    }
}
