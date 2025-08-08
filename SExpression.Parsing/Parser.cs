using Microsoft.Extensions.Logging;
using SExpression.Core;
using SExpression.Core.IR;

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

        public Stack<SExpressions.ScannerToken> Tokens { get; private set; }

        public Core.IR.SExprProgram Parse(List<SExpressions.ScannerToken> tokens)
        {
            this.Tokens = new Stack<SExpressions.ScannerToken>(Enumerable.Reverse(tokens));
            return FindProgram();
        }

        private Core.IR.SExprProgram FindProgram()
        {
            List<SExpr> expressions = new();
            while (this.Tokens.TryPeek(out var peekedToken))
            {
                expressions.Add(BuildSExpression());
            }
            return new SExprProgram(expressions);
        }

        private Core.IR.SExpr BuildSExpression()
        {
            var root = this.Tokens.Peek();

            return root.TokenType switch
            {
                ScannerTokenType.OpenBracket => BuildList(),
                _ => BuildAtom()
            };

        }

        private Core.IR.SExpr BuildAtom()
        {
            var current = this.Tokens.Peek();

            var Atom = current.TokenType switch
            {
                Core.ScannerTokenType.Number => AtomNumber(),
                Core.ScannerTokenType.String => AtomString(),
                Core.ScannerTokenType a when a == Core.ScannerTokenType.Keyword && (current.Value == "t" || current.Value == "nil") => AtomBoolean(),
                _ => AtomSymbol()
            };
            return Atom;

        }

        private Core.IR.SExpr AtomSymbol()
        {
            var symbolToken = this.Tokens.Pop();

            // A Keyword
            return symbolToken.TokenType switch
            {
                Core.ScannerTokenType.Keyword => new SExpressionSymbolKeyword(symbolToken.Value, symbolToken),
                Core.ScannerTokenType.Identifier => new SExpressionSymbolIdentifier(symbolToken.Value, symbolToken),
                Core.ScannerTokenType ops when LookUps.Operators.ContainsKey(symbolToken.SourceValue) => new SExpressionSymbolOperator(symbolToken.SourceValue, symbolToken),
                _ => throw new ParserException($"Atom Symbol type not found {symbolToken.Value}")
            };

        }
        private Core.IR.SExpr AtomBoolean()
        {
            var booleanToken = this.Tokens.Pop();
            if (booleanToken.TokenType != Core.ScannerTokenType.Keyword && (booleanToken.Value != "t" || booleanToken.Value != "nil"))
            {
                var msg = $"Expected a boolean but found {booleanToken.TokenType} at ln:{booleanToken.lineNumber}:{booleanToken.column}";
                _logger.LogCritical(msg, nameof(AtomBoolean));
                throw new SExpression.ParserException(msg);
            }
            // Process the boolean token as needed
            return new SExprBoolean(booleanToken.Value == "t", booleanToken);
        }

        private Core.IR.SExpr AtomString()
        {
            var stringToken = this.Tokens.Pop();
            if (stringToken.TokenType != Core.ScannerTokenType.String)
            {
                var msg = $"Expected a string but found {stringToken.TokenType} at ln:{stringToken.lineNumber}:{stringToken.column}";
                _logger.LogCritical(msg, nameof(AtomString));
                throw new SExpression.ParserException(msg);
            }

            return new SExprString(stringToken.SourceValue, stringToken);

        }

        private Core.IR.SExpr AtomNumber()
        {
            var numberToken = this.Tokens.Pop();
            if (numberToken.TokenType != Core.ScannerTokenType.Number)
            {
                var msg = $"Expected a number but found {numberToken.TokenType} at ln:{numberToken.lineNumber}:{numberToken.column}";
                _logger.LogCritical(msg, nameof(AtomNumber));
                throw new SExpression.ParserException(msg);
            }
            // Process the number token as needed
            return new SExprNumber(numberToken.Value, numberToken);
        }

        private Core.IR.SExpr BuildList()
        {
            var BetterBeAOpenBracket = this.Tokens.Pop();
            if (BetterBeAOpenBracket.TokenType != Core.ScannerTokenType.OpenBracket)
            {
                var msg = $"Expected opening '(' for list instead found {BetterBeAOpenBracket.TokenType} at , ln:{BetterBeAOpenBracket.lineNumber}:{BetterBeAOpenBracket.column}";
                _logger.LogCritical(msg, nameof(BuildList));
                throw new SExpression.ParserException(msg);
            }
            var length = 0;
            var ListHead = BuildListNodes(ref length);
            return new SExprList(ListHead, length);
        }

        private Core.IR.SExpr BuildListNodes(ref int counter)
        {
            if (this.Tokens.Count > 0)
            {
                if (this.Tokens.Peek().TokenType != Core.ScannerTokenType.CloseBracket)
                {
                    counter++;
                    var currentNode = BuildSExpression();
                    if (currentNode is SExpressionSymbolOperator && counter > 1)
                        throw new ParserException("Operator must be the first operation of a list.");
                    return new SExprListNode(currentNode)
                    {
                        Next = BuildListNodes(ref counter)
                    };
                }
                else
                {
                    Tokens.Pop();
                    return SExprBoolean.False();
                }

            }
            else
            {
                throw new ParserException($"List incorrectly terminated! Expected closing bracket instead found end of the input");
            }
        }
    }
}
