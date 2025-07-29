namespace SExpression.Core.IR
{
    public class SExpressionBoolean : SExpr
    {
        public SExpressionBoolean(bool value) : base()
        {
            this.Value = value ? "t" : "nil";
        }
        public SExpressionBoolean() : base()
        {
            this.Value = "nil";
        }
        public SExpressionBoolean(string value) : base()
        {
            if (value != "t" && value != "nil")
            {
                throw new ArgumentException("Value must be 't' or 'nil'.", nameof(value));
            }
            this.Value = value;
        }
        public bool IsTrue => this.Value == "t";
        public override string ToString()
        {
            return this.Value ?? "nil";
        }
    }
}
