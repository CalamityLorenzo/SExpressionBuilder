using Microsoft.Extensions.Logging;
using SExpression.Core;
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
        private List<Core.IR.SExpression> _SExpressions;

        public Stack<ScannerToken> Tokens { get; private set; }

        public List<Core.IR.SExpression> Parse(List<ScannerToken> tokens)
        {
            this.Tokens = new Stack<ScannerToken>(tokens.Reverse<ScannerToken>());
            return FindProgram();
        }

        private List<Core.IR.SExpression> FindProgram()
        {
            this._SExpressions = new List<Core.IR.SExpression>();
            while (this.Tokens.TryPeek(out var peekedToken))
            {
                _SExpressions.Add(BuildSExpression());

            }
            return _SExpressions;
        }

        private Core.IR.SExpression BuildSExpression()
        {
            var root = this.Tokens.Peek();

            if (root.TokenType == Core.TokenType.OpenBracket)
            {
                return BuildList();
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

            // can be

            // A Keyword
            return symbolToken.TokenType switch
            {
                Core.TokenType.Keyword => new SExpressionSymbolKeyword(symbolToken.Value),
                Core.TokenType.Identifier => new SExpressionSymbolIdentifier(symbolToken.Value),
                Core.TokenType ops when LookUps.Operators.ContainsKey(symbolToken.SourceValue) => new SExpressionSymbolOperator(symbolToken.SourceValue),
                _ => throw new ParserException($"Atom Symbol type not found {symbolToken.Value}")
            };

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
            if (stringToken.SourceValue == "\"\"") // The empty string case
            {
                return new SExpressionString(String.Empty);
            }
            else
            {
                return new SExpressionString(stringToken.SourceValue.AsSpan(1, stringToken.SourceValue.Length - 2).ToString());
            }

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

        private Core.IR.SExpression BuildList()
        {
            var BetterBeAOpenBracket = this.Tokens.Pop();
            if (BetterBeAOpenBracket.TokenType != Core.TokenType.OpenBracket)
            {
                var msg = $"Expected opening '(' for list instead found {BetterBeAOpenBracket.TokenType} at , ln:{BetterBeAOpenBracket.lineNumber}:{BetterBeAOpenBracket.column}";
                _logger.LogCritical(msg, nameof(BuildList));
                throw new SExpression.ParserException(msg);
            }

            List<Core.IR.SExpression> expressions = new List<Core.IR.SExpression>();
            while (this.Tokens.Peek().TokenType != Core.TokenType.CloseBracket)
            {
                expressions.Add(BuildSExpression());
            }

            var BetterBeAClosedBracket = this.Tokens.Pop();
            if (BetterBeAClosedBracket.TokenType != Core.TokenType.CloseBracket)
            {
                var msg = $"Expected closing ')' for list instead found {BetterBeAClosedBracket.TokenType} at , ln:{BetterBeAClosedBracket.lineNumber}:{BetterBeAClosedBracket.column}";
                _logger.LogCritical(msg, nameof(BuildList));
                throw new SExpression.ParserException(msg);
            }
            return new SExpressionList(expressions);
        }
    }
}
