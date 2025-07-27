namespace SExpression.Core.IR
{
    public class SExpressionSymbol : SExpression
    {
        public SExpressionSymbol(string name) : base()
        {
            this.Name = name;
        }
        public SExpressionSymbol() : base()
        {
            this.Name = string.Empty;
        }
        public override string ToString()
        {
            return this.Name ?? string.Empty;
        }
    }
}
