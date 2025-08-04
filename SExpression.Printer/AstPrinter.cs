using Microsoft.Extensions.Logging;
using SExpression.Core.IR;
using System.Diagnostics;

namespace SExpression.Printer
{
    public class AstPrinter : IExternalAction
    {
        private readonly ILogger logger;

        public AstPrinter(ILogger logger)
        {
            this.logger = logger;
        }
        public void VisitAtom(SExprNumber number)
        {
            logger.LogTrace(number.Value);
        }

        public void VisitAtom(SExprSymbol symbol)
        {
            logger.LogTrace(symbol.Value);

        }

        public void VisitAtom(SExprString @string)
        {
            logger.LogTrace(@string.Value);
        }

        public void VisitAtom(SExprBoolean boolean)
        {
            logger.LogTrace(boolean.Value);
        }


        public void VisitProgram(SExprProgram action)
        {
            logger.LogTrace("Program Start\n");
            var counter = 1;
            foreach (var express in action.Expressions)
            {
                logger.LogTrace($"{counter} : ");
                express.Apply(this);
                counter++;
            }
        }
        public void VisitList(SExprList list)
        {
            logger.LogTrace($"(");
            list.Elements.Apply(this);
            logger.LogTrace("\n)");
        }

        public void VisitListNode(SExprListNode action)
        {
            action.CurrentValue.Apply(this);
            action.Next.Apply(this);
        }

        public void VisitListNode(SExprBoolean action)
        {
            logger.LogTrace(action.Value);
            logger.LogTrace("List End");
        }
    }
}
