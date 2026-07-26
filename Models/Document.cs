using System.IO;
using System.Windows.Forms;

namespace PrologCoder.Models
{
    public class Document
    {
        public string? FilePath { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsModified { get; set; }
        public string FileName => FilePath == null ? "Untitled.pl" : Path.GetFileName(FilePath);
    }
}
