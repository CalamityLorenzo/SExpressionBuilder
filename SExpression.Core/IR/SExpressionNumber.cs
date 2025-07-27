namespace SExpression.Core.IR
{
    public class SExpressionNumber : SExpression
    {
        public SExpressionNumber(double value) : base()
        {
            this.Value = value.ToString();
        }
        public SExpressionNumber() : base()
        {
            this.Value = "0";
        }
        public SExpressionNumber(string value) : base()
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
    }
}
