namespace SExpression.Core.IR
{
    public class SExpressionList : SExpr
    {
        public SExpr List { get; }

        // Car First
        // CDR Rest
        public SExpressionList(SExpr head)
        {
            this.List = head;
            this.IsAtom = false;
            this.Value = $"List : {head.Value}";
        }

    }

    /// <summary>
    /// This may need to be a records for immutablility.
    /// Can we always gurantee that we know what the Next is?
    /// </summary>
    public class SExpressionListNode : SExpr
    {
        public SExpressionListNode() { }
        public SExpressionListNode(SExpr Current, SExpr Next)
        {
            this.CurrentValue = Current;
            this.Value = CurrentValue.Value;
            this.Next = Next;
        }

        public SExpr CurrentValue { get; set; }
        public SExpr Next { get; set; } // Basically a new list item with value or Boolean Nil
    }

    public class SPExpressionEmptyListItem() : SExpressionListNode(new SExpressionBoolean(false), new SExpressionBoolean(false));
          
    



}
