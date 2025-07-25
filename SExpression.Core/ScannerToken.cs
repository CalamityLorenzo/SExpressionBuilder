using SExpression.Core;

namespace SExpressions
{
    public record ScannerToken(TokenType TokenType, string Value, string SourceValue, int lineNumber, int column);
}
