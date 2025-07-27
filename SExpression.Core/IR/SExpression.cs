namespace SExpression.Core.IR
{
    public abstract class SExpression
    {
        public SExpression(IEnumerable<SExpression> expressions)
        {
            this.Expressions = new List<SExpression>(expressions);
        }

        public SExpression(){}

        public List<SExpression> Expressions { get; init; } = new List<SExpression>();
        public string? Name { get; set; }
        public string? Value { get; set; }
        public bool IsList => this.Expressions.Count > 0;
        public bool IsAtom => !this.IsList;

        public override string ToString()
        {
            if (this.IsList)
            {
                return $"({string.Join(" ", this.Expressions.Select(e => e.ToString()))})";
            }
            else
            {
                return this.Value ?? string.Empty;
            }
        }

    }

}
