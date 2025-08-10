using Microsoft.Extensions.Logging;
using SExpression.Core.IR;
using System.Text;

namespace SExpression.Printer
{
    public class AstPrinter : IExternalAction<string>
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

        public string VisitAtom(SExprNumber number)
        {
            return ($"{number.Value} ");
        }

        public string VisitAtom(SExprSymbol symbol)
        {
            return ($"{symbol.Value} ");

        }

        public string VisitAtom(SExprString @string)
        {
            return ($"{@string.Value} ");
        }

        public string VisitAtom(SExprBoolean boolean)
        {
            return ($"{boolean.Value} ");
        }


        public string VisitProgram(SExprProgram action)
        {
            var counter = 1;
            StringBuilder sb = new();
            foreach (var express in action.Expressions)
            {
                sb.Append(express.Apply(this));
                counter++;
            }
            sb.Append("\n");
            return sb.ToString();
        }
        public string VisitList(SExprList list)
        {
            return $"\t({list.Head.Apply(this)})\n";
        }

        public string VisitListNode(SExprListNode action)
        {
            var result =action.Next.Apply(this);
            return $"{action.CurrentValue.Apply(this)} {result}";
        }

        public string VisitListNode(SExprBoolean action)
        {
            return ($"{action.Value}");
        }
    }
}
