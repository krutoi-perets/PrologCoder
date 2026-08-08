using PrologCoder.Models;

namespace PrologCoder.Analysis
{
    public class PredicateInfo
    {
        public string Name { get; set; } = string.Empty;
        public int Arity { get; set; }
        public string? Description { get; set; }
    }
}
