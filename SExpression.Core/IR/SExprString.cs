
namespace SExpression.Core.IR
{
    public class SExprString : SExpr
    {
        public SExprString(string s) : base()
        {

            this.Value = s ?? throw new ArgumentNullException($"Cannot pass a null string! SExpressionString Constructor");
        }

        public override void Apply(IExternalAction action)
        {
            action.VisitAtom(this);
        }
    }
}
