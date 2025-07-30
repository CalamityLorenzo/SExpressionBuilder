using System.Runtime.CompilerServices;

namespace SExpression.Core.IR
{
    public class SExprList : SExpr
    {
        public List<SExpr> Expressions { get; private set; }

        public SExprList(IEnumerable<SExpr> expressions)
        {
            this.Expressions = new List<SExpr>(expressions);
            this.IsAtom = false;
            this.Value = "List";
        }

        public override void Apply(IExternalAction action)
        {
            action.VisitList(this);
        }
    }



}
