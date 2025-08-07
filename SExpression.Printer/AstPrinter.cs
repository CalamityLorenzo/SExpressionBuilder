using Microsoft.Extensions.Logging;
using SExpression.Core.IR;
using System.Diagnostics;

namespace SExpression.Printer
{
    public class AstPrinter : IExternalAction
    {
        private readonly Action<string> writer;

        public AstPrinter(ILogger logger)
        {
            writer= (str)=> writer(str);
        }
        public AstPrinter(Action<string> toWrite)
        {
            writer = toWrite;
        }

        public void VisitAtom(SExprNumber number)
        {
            writer($"{number.Value} ");
        }

        public void VisitAtom(SExprSymbol symbol)
        {
            writer($"{symbol.Value} ");

        }

        public void VisitAtom(SExprString @string)
        {
            writer($"{@string.Value} ");
        }

        public void VisitAtom(SExprBoolean boolean)
        {
            writer($"{boolean.Value} ");
        }


        public void VisitProgram(SExprProgram action)
        {
            writer("Program Start\n");
            var counter = 1;
            foreach (var express in action.Expressions)
            {
                writer($"{counter} : ");
                express.Apply(this);
                counter++;
            }
        }
        public void VisitList(SExprList list)
        {
            writer($"\t\n(");
            list.Head.Apply(this);
            writer(")");
        }

        public void VisitListNode(SExprListNode action)
        {
            action.CurrentValue.Apply(this);
            action.Next.Apply(this);
        }

        public void VisitListNode(SExprBoolean action)
        {
            writer($"{action.Value}");
        }
    }
}
