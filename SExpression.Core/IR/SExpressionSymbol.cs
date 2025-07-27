namespace SExpression.Core.IR
{
    public class SExpressionSymbol : SExpression
    {

        bool IsKeyword { get;init; }

        public SExpressionSymbol(string name, bool isKeyword) : base()
        {
            this.Value = name;
            IsKeyword = isKeyword;
        }
    }
}
