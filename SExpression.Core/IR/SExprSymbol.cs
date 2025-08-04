using System.Runtime.CompilerServices;

namespace SExpression.Core.IR
{
    public abstract class SExprSymbol : SExpr
    {
        public virtual SymbolType Symbol { get; }

        public SExprSymbol(string name) : base()
        {
            this.Value = name;
        }

        public override void Apply(IExternalAction action)
        {
            action.VisitAtom(this);
        }
    }

    public class SExpressionSymbolKeyword : SExprSymbol
    {
        public override SymbolType Symbol => SymbolType.Keyword;
        public SExpressionSymbolKeyword(string name) : base(name)
        {
        }
    }

    public class SExpressionSymbolIdentifier : SExprSymbol
    {
        public override SymbolType Symbol => SymbolType.Identifier;
        public SExpressionSymbolIdentifier(string name) : base(name)
        {
        }
    }

    public class SExpressionSymbolOperator : SExprSymbol
    {
        public override SymbolType Symbol => SymbolType.Operator;
        public SExpressionSymbolOperator(string value) : base(value)
        {
        }
    }
}
