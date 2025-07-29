using SExpression.Core;

namespace SExpressions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="TokenType"></param>
    /// <param name="Value">Canonical value for the language (Identifier/Keywords explicitly)</param>
    /// <param name="SourceValue">The Value from the source file</param>
    /// <param name="lineNumber"></param>
    /// <param name="column">column of lineNumber</param>
    public record ScannerToken(ScannerTokenType TokenType, string Value, string SourceValue, int lineNumber, int column);
}
