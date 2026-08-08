using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using PrologCoder.Models;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace PrologCoder.Completion
{
    public class CompletionData : ICompletionData
    {
        public ImageSource? Image => null;
        public string Text { get; }
        public object Content => Arity.HasValue && Arity > 0 ? $"{Text}/{Arity}" : Text;
        public int? Arity { get; }
        public CompletionType Type { get; }
        public object Description { get; }
        public double Priority => 0;

        public CompletionData(
            string text,
            string description = "",
            int? arity = null,
            CompletionType type = CompletionType.Unknown)
        {
            Text = text;
            Description = description;
            Arity = arity;
            Type = type;
        }

        public void Complete(TextArea textArea,
                             ISegment completionSegment,
                             EventArgs insertionRequestEventArgs)
        {
            int offset = textArea.Caret.Offset;
            string text = textArea.Document.Text;

            int start = offset;

            while (start > 0)
            {
                char c = text[start - 1];

                if (!char.IsLetterOrDigit(c) && c != '_')
                    break;

                start--;
            }

            if (Type == CompletionType.Predicate && Arity > 0)
            {
                textArea.Document.Replace(start, offset - start, $"{Text}()");

                textArea.Caret.Offset--;
                return;
            }

            textArea.Document.Replace(start, offset - start, Text);
        }
    }
}
