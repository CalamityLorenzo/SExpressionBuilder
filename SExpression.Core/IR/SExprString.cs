
using SExpressions;

namespace SExpression.Core.IR
{
    public class SExprString : SExpr
    {
        public SExprString(string s, ScannerToken token) : base(token)
        {
            this.Value = s ?? throw new ArgumentNullException($"Cannot pass a null string! SExpressionString Constructor");
        }

        public override T Apply<T>(IExternalAction<T> action)=> action.VisitAtom(this);
    }
}
