using Microsoft.Extensions.Logging;
using SExpression.Core.IR;
using SExpressions;
using System.Linq.Expressions;

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

        private Core.IR.SExpression FindExpression()
        {
            var root = this.Tokens.Peek();
            if (root.TokenType == Core.TokenType.OpenBracket)
            {
                return FindList();
            }
            else
            {
                return BuildAtom();
            }
        }

        private Core.IR.SExpression BuildAtom()
        {
            var current = this.Tokens.Peek();

            var Atom = current.TokenType switch
            {
                Core.TokenType.Number => AtomNumber(),
                Core.TokenType.String => AtomString(),
                Core.TokenType a when a == Core.TokenType.Keyword && (current.Value == "t" || current.Value == "nil") => AtomBoolean(),
                _ => AtomSymbol()
            };
            return Atom;

        }

        private Core.IR.SExpression AtomSymbol()
        {
            var symbolToken = this.Tokens.Pop();

            // Process the symbol token as needed
            return new SExpressionSymbol(symbolToken.Value, symbolToken.TokenType == Core.TokenType.Keyword);
        }

        private Core.IR.SExpression AtomBoolean()
        {
            var booleanToken = this.Tokens.Pop();
            if (booleanToken.TokenType != Core.TokenType.Keyword && (booleanToken.Value != "t" || booleanToken.Value != "nil"))
            {
                var msg = $"Expected a boolean but found {booleanToken.TokenType} at ln:{booleanToken.lineNumber}:{booleanToken.column}";
                _logger.LogCritical(msg, nameof(AtomBoolean));
                throw new SExpression.ParserException(msg);
            }
            // Process the boolean token as needed
            return new SExpressionBoolean(booleanToken.Value == "t");
        }

        private Core.IR.SExpression AtomString()
        {
            var stringToken = this.Tokens.Pop();
            if (stringToken.TokenType != Core.TokenType.String)
            {
                var msg = $"Expected a string but found {stringToken.TokenType} at ln:{stringToken.lineNumber}:{stringToken.column}";
                _logger.LogCritical(msg, nameof(AtomString));
                throw new SExpression.ParserException(msg);
            }
            // Process the string token as needed
            return new SExpressionString(stringToken.Value);

        }

        private Core.IR.SExpression AtomNumber()
        {
            var numberToken = this.Tokens.Pop();
            if (numberToken.TokenType != Core.TokenType.Number)
            {
                var msg = $"Expected a number but found {numberToken.TokenType} at ln:{numberToken.lineNumber}:{numberToken.column}";
                _logger.LogCritical(msg, nameof(AtomNumber));
                throw new SExpression.ParserException(msg);
            }
            // Process the number token as needed
            return new SExpressionNumber(numberToken.Value);
        }

        private Core.IR.SExpression FindList()
        {
            var BetterBeAOpenBracket = this.Tokens.Pop();
            if (BetterBeAOpenBracket.TokenType != Core.TokenType.OpenBracket)
            {
                var msg = $"Expected opening '(' for list instead found {BetterBeAOpenBracket.TokenType} at , ln:{BetterBeAOpenBracket.lineNumber}:{BetterBeAOpenBracket.column}";
                _logger.LogCritical(msg, nameof(FindList));
                throw new SExpression.ParserException(msg);
            }

            List<Core.IR.SExpression> expressions = new List<Core.IR.SExpression>();
            while (this.Tokens.Peek().TokenType != Core.TokenType.CloseBracket)
            {
                expressions.Add(FindExpression());
            }

            return new SExpressionList(expressions);
        }
    }
}
