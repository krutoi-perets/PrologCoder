using System.Text.RegularExpressions;

namespace PrologCoder.Analysis
{
    public class PrologParser
    {
        public List<PredicateInfo> GetPredicates(string text)
        {
            List<PredicateInfo> result = new();

            string pattern = @"^\s*([a-z][A-Za-z0-9_]*)\s*(\((.*?)\))?\s*:-";

            foreach (Match match in Regex.Matches(text, pattern, RegexOptions.Multiline))
            {
                string name = match.Groups[1].Value;

                int arity = 0;

                if (match.Groups[3].Success)
                {
                    string args = match.Groups[3].Value;

                    if (!string.IsNullOrWhiteSpace(args))
                        arity = args.Split(',').Length;
                }

                result.Add(new PredicateInfo
                {
                    Name = name,
                    Arity = arity
                });
            }

            return result;
        }
    }
}
