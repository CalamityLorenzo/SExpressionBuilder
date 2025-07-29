using System.Text;

namespace SExpression.Core.IR
{
    public abstract class SExpr
    {
        public string Value { get; init; } = string.Empty;
        public bool IsAtom { get; init; } = true;
    }

    

}
