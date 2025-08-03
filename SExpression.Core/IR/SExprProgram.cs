namespace SExpression.Core.IR
{
    public class SExprProgram : SExpr
    {
        public readonly IReadOnlyList<SExpr> Expressions;

        public SExprProgram(IReadOnlyList<SExpr> expressions)
        {
            this.IsAtom = false;
            this.Value = "Program";
            Expressions = expressions;
        }

        public override void Apply(IExternalAction action)
        {
            action.VisitProgram(this);
        }
    }
}
