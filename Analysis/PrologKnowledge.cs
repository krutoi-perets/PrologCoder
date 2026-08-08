using System.Net.NetworkInformation;

namespace PrologCoder.Analysis
{
    public class PrologKnowledge
    {
        public static readonly PredicateInfo[] BuiltInPredicates =
        {
            new() { Name = "write", Arity = 1, Description = "Writes a term" },
            new() { Name = "writeln", Arity = 1, Description = "Writes a term followed by a newline" },
            new() { Name = "read", Arity = 1, Description = "Reads a term" },
            new() { Name = "readln", Arity = 1, Description = "Reads a line as a string" },
            new() { Name = "get_char", Arity = 1, Description = "Reads one character" },
            new() { Name = "get_code", Arity = 1, Description = "Reads one character code" },
            new() { Name = "put_char", Arity = 1, Description = "Writes one character" },
            new() { Name = "put_code", Arity = 1, Description = "Writes a character code" },

            new() { Name = "var", Arity = 1, Description = "Tests whether a term is an unbound variable" },
            new() { Name = "nonvar", Arity = 1, Description = "Tests whether a term is not a variable" },
            new() { Name = "atom", Arity = 1, Description = "Tests whether a term is an atom" },
            new() { Name = "atomic", Arity = 1, Description = "Tests whether a term is atomic" },
            new() { Name = "number", Arity = 1, Description = "Tests whether a term is a number" },
            new() { Name = "integer", Arity = 1, Description = "Tests whether a term is an integer" },
            new() { Name = "float", Arity = 1, Description = "Tests whether a term is a floating-point number" },
            new() { Name = "compound", Arity = 1, Description = "Tests whether a term is compound" },
            new() { Name = "callable", Arity = 1, Description = "Tests whether a term is callable" },

            new() { Name = "atom_chars", Arity = 2, Description = "Converts between an atom and a character list" },
            new() { Name = "atom_codes", Arity = 2, Description = "Converts between an atom and a code list" },
            new() { Name = "number_chars", Arity = 2, Description = "Converts between a number and a character list" },
            new() { Name = "number_codes", Arity = 2, Description = "Converts between a number and a code list" },

            new() { Name = "assert", Arity = 1, Description = "Adds a clause to the database" },
            new() { Name = "asserta", Arity = 1, Description = "Adds a clause at the beginning of the database" },
            new() { Name = "assertz", Arity = 1, Description = "Adds a clause at the end of the database" },
            new() { Name = "retract", Arity = 1, Description = "Removes a matching clause from the database" },
            new() { Name = "abolish", Arity = 1, Description = "Removes all clauses of a predicate" },

            new() { Name = "findall", Arity = 3, Description = "Collects all solutions into a list" },
            new() { Name = "bagof", Arity = 3, Description = "Collects solutions into a bag grouped by free variables" },
            new() { Name = "setof", Arity = 3, Description = "Collects and sorts unique solutions into a set" },

            new() { Name = "member", Arity = 2, Description = "Tests or generates list membership" },
            new() { Name = "append", Arity = 3, Description = "Concatenates lists" },
            new() { Name = "length", Arity = 2, Description = "Relates a list to its length" },
            new() { Name = "sort", Arity = 2, Description = "Sorts a list and removes duplicates" },
            new() { Name = "msort", Arity = 2, Description = "Sorts a list while preserving duplicates" },

            new() { Name = "functor", Arity = 3, Description = "Decomposes or constructs a compound term" },
            new() { Name = "arg", Arity = 3, Description = "Unifies an argument of a compound term" },
            new() { Name = "univ", Arity = 2, Description = "Converts between a compound term and a list representation" },

            new() { Name = "compare", Arity = 3, Description = "Compares two terms using standard order" },

            new() { Name = "nl", Arity = 0, Description = "Writes a newline" },

            new() { Name = "consult", Arity = 1, Description = "Loads a Prolog source file" },
            new() { Name = "ensure_loaded", Arity = 1, Description = "Loads a file if it has not already been loaded" },
            new() { Name = "use_module", Arity = 1, Description = "Loads a Prolog module" },

            new() { Name = "op", Arity = 3, Description = "Declares an operator" },
            new() { Name = "current_predicate", Arity = 1, Description = "Tests or enumerates currently defined predicates" },
            new() { Name = "predicate_property", Arity = 2, Description = "Queries properties of a predicate" }
        };

        public static readonly string[] Keywords =
        {
            "is",
            "mod",
            "not",
            "fail",
            "true",
            "halt"
        };
    }
}
