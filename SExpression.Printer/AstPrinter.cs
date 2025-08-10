using Microsoft.Extensions.Logging;
using SExpression.Core.IR;

namespace SExpression.Printer
{
    public class AstPrinter : IExternalAction
    {
        private Action<string> writer;
        private readonly ILogger<AstPrinter> _logger;

        public AstPrinter(ILogger<AstPrinter> logger)
        {
            this._logger = logger;
        }

        public void ConfigureWriter(Action<string> toWrite)
        {
            writer = toWrite;
        }

        public void Print(SExpr expression) => expression.Apply(this);

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
            writer("\n");
        }
        public void VisitList(SExprList list)
        {
            writer($"\t(");
            list.Head.Apply(this);
            writer(")\n");
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
