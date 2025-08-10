namespace SExpression.Core.IR
{
    public interface IExternalAction<T>
    {
        public T VisitList(SExprList list);
        public T VisitAtom(SExprNumber number);
        public T VisitAtom(SExprSymbol symbol);
        public T VisitAtom(SExprString @string);
        public T VisitAtom(SExprBoolean boolean);
        public T VisitProgram(SExprProgram action);
        public T VisitListNode(SExprListNode action);
    }
}