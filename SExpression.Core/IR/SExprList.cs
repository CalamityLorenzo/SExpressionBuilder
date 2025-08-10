using System.Collections;

namespace SExpression.Core.IR
{
    public class SExprList : SExpr
    {

        public SExprList(SExpr ListValue, int length)
        {
            this.Head = ListValue;
            Length = length;
            this.IsAtom = false;
            this.Value = Head.Value;
        }

        public SExpr Head { get; }
        public int Length { get; }

            
        public override T Apply<T>(IExternalAction<T> action) => action.VisitList(this);


        public SExprList.Enumerator GetEnumerator()
        {
            return new Enumerator(this.Head);
        }

        public struct Enumerator(SExpr StartNode) : IEnumerator<SExpr>
        {
            public SExpr? Current { get; set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                Current = null;
            }

            public bool MoveNext()
            {
                if (Current is null && StartNode is SExprBoolean)
                {
                    return false;
                }
                else if (Current is null)
                {
                    Current = StartNode;
                    return true;
                }

                var next = ((SExprListNode)Current).Next;
                if (next is SExprBoolean)
                    return false;
                else
                {
                    Current = next;
                    return true;
                }
            }

            public void Reset()
            {
                Current = null;
            }
        }

    }

    public class SExprListNode : SExpr
    {
        public SExprListNode(SExpr Current)
        {
            this.CurrentValue = Current;
            this.Value = CurrentValue.Value;
        }
        public SExprListNode(SExpr Current, SExpr Next)
        {
            this.CurrentValue = Current;
            this.Value = CurrentValue.Value;
            this.Next = Next;
        }

        /// <summary>
        /// SExprBoolean(false) or SExprListNode
        /// </summary>
        public SExpr CurrentValue { get; }

        /// <summary>
        /// SExprBoolean(false) or SExprListNode
        /// </summary>
        public required SExpr Next { get; init; }

        public override T Apply<T>(IExternalAction<T> action) => action.VisitListNode(this);

        public override string ToString() => $"Current : '{Value}'\n Next: '{Next.Value}'";

    }




}
