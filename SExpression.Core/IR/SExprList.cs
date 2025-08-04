using System.Runtime.CompilerServices;

namespace SExpression.Core.IR
{
    public class SExprList : SExpr
    {

        public SExprList(SExpr ListValue)
        {
            this.Elements = ListValue;
            this.IsAtom = false;
            this.Value = (Elements is SExprBoolean) ? "nil" : (Elements as SExprListNode).Value;
        }

        public SExpr Elements { get; }

        public override void Apply(IExternalAction action)
        {
            action.VisitList(this);
        }
    }

    public class SExprListNode : SExpr
    {
        public SExprListNode() { }
        public SExprListNode(SExpr Current) {
            this.CurrentValue = Current;
            this.Value = CurrentValue.Value;
        }
        public SExprListNode(SExpr Current, SExpr Next)
        {
            this.CurrentValue = Current;
            this.Value = CurrentValue.Value;
            this.Next = Next;
        }

        public SExpr CurrentValue { get; set; }
        public SExpr Next { get; set; } // Basically a new list item with value or Boolean Nil

        public override void Apply(IExternalAction action)
        {
            throw new NotImplementedException();
        }
    }

    public class SExprListNodeEmpty() : SExprListNode(new SExprBoolean(false), new SExprBoolean(false))
    {
        public void Apply(IExternalAction action) { }
    }




}
