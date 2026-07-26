using PrologCoder.Services;
using System.Windows.Input;

namespace PrologCoder
{
    public class PrologTerminal : ICSharpCode.AvalonEdit.TextEditor
    {
        private int _inputStartOffset;
        private const string _prompt = "> ";

        public event Action<string>? InputSended;

        public PrologTerminal()
        {
            ShowLineNumbers = false;
            IsReadOnly = true;

            TextArea.TextEntering += TextArea_TextEntering;
            TextArea.PreviewKeyDown += TextArea_PreviewKeyDown;
        }

        public void StartSession()
        {
            IsReadOnly = false;
            AppendPrompt();
        }

        public void StopSession()
        {
            IsReadOnly = true;
            RemovePrompt();
        }

        private void TextArea_TextEntering(object? sender, TextCompositionEventArgs e)
        {
            if (CaretOffset < _inputStartOffset)
                CaretOffset = Document.TextLength;

            ScrollToEnd();
        }

        private void TextArea_PreviewKeyDown(object? sender, KeyEventArgs e)
        {
            if (IsReadOnly) return;

            if (e.Key == Key.Back && CaretOffset <= _inputStartOffset)
            {
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Delete && CaretOffset < _inputStartOffset)
            {
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                string input = Document.Text.Substring(_inputStartOffset);

                Document.Insert(Document.TextLength, Environment.NewLine);
                AppendPrompt();

                InputSended?.Invoke(input);
            }
        }

        public void AppendOutput(string text)
        {
            RemovePrompt();

            Document.Insert(Document.TextLength, text);
            CaretOffset = Document.TextLength;

            AppendPrompt();
        }

        private void AppendPrompt()
        {
            if (!Document.Text.EndsWith(_prompt))
            {
                Document.Insert(Document.TextLength, _prompt);

                _inputStartOffset = Document.TextLength;
                CaretOffset = Document.TextLength;
                ScrollToEnd();
            }
        }

        private void RemovePrompt()
        {
            if (Document.Text.EndsWith(_prompt))
                Document.Remove(Document.TextLength - _prompt.Length, _prompt.Length);
        }

        public void ClearTerminal()
        {
            Text = "";
        }
    }
}
