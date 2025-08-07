namespace SExpression.Core.IR
{
    public interface IExternalAction
    {
        public void VisitList(SExprList list);
        public void VisitAtom(SExprNumber number);
        public void VisitAtom(SExprSymbol symbol);
        public void VisitAtom(SExprString @string);
        public void VisitAtom(SExprBoolean boolean);
        public void VisitProgram(SExprProgram action);
        public void VisitListNode(SExprListNode action);
    }
}