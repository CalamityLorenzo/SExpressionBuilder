
namespace SExpression.Core.IR
{
    public class SExpressionString : SExpression
    {
        public SExpressionString(string s) : base()
        {
            
            this.Value = s ?? throw new ArgumentNullException($"Cannot pass a null string! SExpressionString Constructor");
        }
    }
}
