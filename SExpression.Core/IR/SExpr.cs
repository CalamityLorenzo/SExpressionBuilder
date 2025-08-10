using SExpressions;
using System.Text;

namespace SExpression.Core.IR
{
    public abstract class SExpr
    {
        public SExpr() { }

        protected SExpr(ScannerToken token)
        {
            Token = token;
        }

        public string Value { get; init; } = string.Empty;
        public bool IsAtom { get; init; } = true;
        public ScannerToken Token { get; }
        public abstract T Apply<T>(IExternalAction<T> action);
    }



}
