using System.Net.NetworkInformation;

namespace PrologCoder.Analysis
{
    public class PrologKnowledge
    {
        public static readonly string[] BuiltInPredicates =
        {
            "write",
            "writeln",
            "read",
            "readln",
            "get_char",
            "get_code",
            "put_char",
            "put_code",

            "var",
            "nonvar",
            "atom",
            "atomic",
            "number",
            "integer",
            "float",
            "compound",
            "callable",

            "atom_chars",
            "atom_codes",
            "number_chars",
            "number_codes",

            "assert",
            "asserta",
            "assertz",
            "retract",
            "abolish",

            "findall",
            "bagof",
            "setof",

            "member",
            "append",
            "length",
            "sort",
            "msort",

            "functor",
            "arg",
            "univ",

            "compare",
            "sort",

            "nl",

            "consult",
            "ensure_loaded",
            "use_module",

            "op",
            "current_predicate",
            "predicate_property"
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
