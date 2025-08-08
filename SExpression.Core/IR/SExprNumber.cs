using SExpressions;

namespace SExpression.Core.IR
{
    public class SExprNumber : SExpr
    {
        public SExprNumber(double value, ScannerToken token) : base(token)
        {
            this.Value = value.ToString();
        }
        public SExprNumber() : base()
        {
            this.Value = "0";
        }
        public SExprNumber(string value, ScannerToken token) : base(token)
        {
            if (!double.TryParse(value, out _))
            {
                throw new ArgumentException("Value must be a valid number.", nameof(value));
            }
            this.Value = value;
        }
        public double AsDouble => double.Parse(this.Value ?? "0");
        public override string ToString()
        {
            return this.Value ?? "0";
        }

        public override void Apply(IExternalAction action)
        {
            action.VisitAtom(this);
        }
    }
}
