using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System.Windows.Media;
using System.Text.RegularExpressions;
using PrologCoder.Analysis;

namespace PrologCoder.Highlighting
{
    public class PrologColorizer : DocumentColorizingTransformer
    {
        public List<PredicateInfo> UserPredicates { get; set; } = [];
        private readonly string[] _builtInPredicates = PrologKnowledge.BuiltInPredicates;
        private readonly string[] _keywords = PrologKnowledge.Keywords;

        protected override void ColorizeLine(DocumentLine line)
        {
            string text = CurrentContext.Document.GetText(line);

            // Переменные
            ColorRegex(line, text, @"\b[A-ZА-Я_][A-Za-zА-Яа-я0-9_]*\b", Brushes.Blue);

            // Атомы
            ColorRegex(line, text, @"\b[a-zа-я][A-Za-zА-Яа-я0-9_]*\b", Brushes.DarkCyan);

            // Встроенные предикаты
            foreach (var predicate in _builtInPredicates)
            {
                ColorRegex(line, text, $@"\b{predicate}\b", Brushes.Goldenrod);
            }

            // Пользовательские предикаты
            foreach (var predicate in UserPredicates)
            {
                ColorRegex(line, text, $@"\b{Regex.Escape(predicate.Name)}\b", Brushes.Goldenrod);
            }

            // Ключевые слова
            foreach (var keyword in _keywords)
            {
                ColorRegex(line, text, $@"\b{Regex.Escape(keyword)}\b", Brushes.HotPink);
            }
            
            // Атомы в одинарных кавычках
            ColorRegex(line, text, @"'([^'\\]|\\.)*'", Brushes.DarkOrange);
            
            // Строки в кавычках
            ColorRegex(line, text, @"""([^""\\]|\\.)*""", Brushes.OrangeRed);
            
            // Комментарии
            ColorRegex(line, text, @"%.*$", Brushes.Green);
        }

        private void ColorRegex(DocumentLine line, string text, string pattern, Brush brush)
        {
            foreach (Match match in Regex.Matches(text, pattern))
            {
                ChangeLinePart(line.Offset + match.Index,
                               line.Offset + match.Index + match.Length,
                               element =>
                               {
                                   element.TextRunProperties.SetForegroundBrush(brush);
                               });
            }
        }
    }
}
