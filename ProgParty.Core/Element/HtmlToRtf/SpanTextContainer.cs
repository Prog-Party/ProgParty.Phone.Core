using System;
using Windows.UI.Xaml.Documents;

namespace ProgParty.Core.Element.HtmlToRtf
{
    internal sealed class SpanTextContainer : ITextContainer
    {
        private readonly ITextContainer _parent;
        private readonly Span _span;

        public SpanTextContainer(ITextContainer parent, Span span)
        {
            _span = span;
            _parent = parent;
        }

        public void Add(Inline text)
        {
            _span.Inlines.Add(text);
        }

        public Paragraph AddToParentParagraph(Inline text) => _parent.AddToParentParagraph(text);

        public void Add(Block paragraph)
        {
            throw new NotSupportedException();
        }
    }
}
