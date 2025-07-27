namespace SExpression.Core.IR
{
    public class SExpressionList : SExpression
    {
        public List<SExpression> Expressions { get; private set; }  

        public SExpressionList(IEnumerable<SExpression> expressions)
        {
            this.Expressions = new List<SExpression>(expressions);
            this.IsAtom = false;
            this.Value = "List";
        }

    }



}
