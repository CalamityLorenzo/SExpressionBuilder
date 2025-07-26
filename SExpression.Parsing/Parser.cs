using Microsoft.Extensions.Logging;
using SExpressions;

namespace SExpression.Parsing
{

    // <program>       ::= <expression>*
    // <expression>    ::= <atom> | <list>
    // <atom>          ::= <number> | <symbol> | <string> | <boolean>
    // <number>        ::= [0-9]+
    // <symbol>        ::= [a-zA-Z_+\-*/=<>!?]+
    // <string>        ::= "\"" .* "\""
    // <boolean>       ::= "t" | "nil"
    // <list>          ::= "(" <expression>* ")"

    public class Parser(ILogger<Parser> logger)
    {
        private readonly ILogger<Parser> _logger = logger;

        public Stack<ScannerToken> Tokens { get; private set; }

        public void Parse(List<ScannerToken> tokens)
        {
            this.Tokens = new Stack<ScannerToken>(tokens);
            FindProgram();
        }

        private void FindProgram()
        {
            FindExpression();
        }

        private void FindExpression()
        {
            var root = this.Tokens.Peek();
            if (root.TokenType == Core.TokenType.OpenBracket)
            {
                FindList();
            }
            else
            {
                FindAtom();
            }
        }

        private void FindAtom()
        {
            throw new NotImplementedException();
        }

        private void FindList()
        {
            var BetterBeAOpenBracket = this.Tokens.Pop();
            if (BetterBeAOpenBracket.TokenType != Core.TokenType.OpenBracket)
            {
                var msg = $"Expected opening '(' for list instead found {BetterBeAOpenBracket.TokenType} at , ln:{BetterBeAOpenBracket.lineNumber}:{BetterBeAOpenBracket.column}";
                _logger.LogCritical(msg, nameof(FindList));
                throw new SExpression.ParserException(msg);
            }

            FindExpression();

        }
    }
}
