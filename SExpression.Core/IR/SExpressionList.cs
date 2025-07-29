namespace SExpression.Core.IR
{
    public class SExpressionList : SExpr
    {
        public List<SExpr> Expressions { get; private set; }  

        public SExpressionList(IEnumerable<SExpr> expressions)
        {
            this.Expressions = new List<SExpr>(expressions);
            this.IsAtom = false;
            this.Value = "List";
        }

    }



}
