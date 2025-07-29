using System.Runtime.CompilerServices;

namespace SExpression.Core.IR
{
    public class SExpressionSymbol : SExpr
    {
        public virtual SymbolType Symbol { get; }
        bool IsKeyword { get; init; }

        public SExpressionSymbol(string name) : base()
        {
            this.Value = name;
        }
    }

    public class SExpressionSymbolKeyword : SExpressionSymbol
    {
        public override SymbolType Symbol => SymbolType.Keyword;
        public SExpressionSymbolKeyword(string name) : base(name)
        {
        }
    }

    public class SExpressionSymbolIdentifier : SExpressionSymbol
    {
        public override SymbolType Symbol => SymbolType.Identifier;
        public SExpressionSymbolIdentifier(string name) : base(name)
        {
        }
    }

    public class SExpressionSymbolOperator : SExpressionSymbol
    {
        public override SymbolType Symbol => SymbolType.Identifier;
        public SExpressionSymbolOperator(string value) : base(value)
        {
        }
    }
}
