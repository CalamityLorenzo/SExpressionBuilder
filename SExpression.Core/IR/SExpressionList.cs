namespace SExpression.Core.IR
{
    public class SExpressionList : SExpression
    {
        public SExpressionList(IEnumerable<SExpression> expressions) : base(expressions)
        {
        }
        public SExpressionList() : base(new List<SExpression>())
        {
        }
    }



}
