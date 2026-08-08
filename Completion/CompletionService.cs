using PrologCoder.Analysis;

namespace PrologCoder.Completion
{
    public class CompletionService
    {
        public IEnumerable<CompletionData> GetCompletions(
            string prefix,
            IEnumerable<PredicateInfo> userPredicates)
        {
            if (string.IsNullOrEmpty(prefix)) return [];

            var predicates = PrologKnowledge.BuiltInPredicates.Concat(userPredicates);

            var result = predicates.Where(x => x.Name.StartsWith(
                                          prefix, StringComparison.OrdinalIgnoreCase))
                                   .Select(x => new CompletionData(x.Name,
                                                    x.Description ?? "",
                                                    x.Arity,
                                                    Models.CompletionType.Predicate));

            var keywords = PrologKnowledge.Keywords.Where(x => x.StartsWith(
                                                          prefix, StringComparison.OrdinalIgnoreCase))
                                                   .Select(x => new CompletionData(x,
                                                                    type: Models.CompletionType.Keyword));

            return result.Concat(keywords);
        }
    }
}
