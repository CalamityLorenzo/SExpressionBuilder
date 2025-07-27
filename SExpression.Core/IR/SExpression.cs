using System.Text;

namespace SExpression.Core.IR
{
    public abstract class SExpression
    {
        public string Value { get; init; } = string.Empty;
        public bool IsAtom { get; init; } = true;
    }

    

}
