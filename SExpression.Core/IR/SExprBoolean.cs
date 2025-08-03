namespace SExpression.Core.IR
{
    public class SExprBoolean : SExpr
    {
        public SExprBoolean(bool value) : base()
        {
            this.Value = value ? "t" : "nil";
        }
        public SExprBoolean() : base()
        {
            this.Value = "nil";
        }
        public SExprBoolean(string value) : base()
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

        public override void Apply(IExternalAction action)
        {
            action.VisitAtom(this);
        }
    }
}
