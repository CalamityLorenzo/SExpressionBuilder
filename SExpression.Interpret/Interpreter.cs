using Microsoft.Extensions.Logging;
using SExpression.Core.IR;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace SExpression.Interpret
{
    public class Interpreter : IExternalAction<object>
    {
        private SExprProgram _program;
        private readonly ILogger<Interpreter> _logger;

        public Interpreter(ILogger<Interpreter> logger)
        {
            this._logger = logger;
        }


        public void Interpret(Core.IR.SExprProgram program)
        {
            this._program = program;
            foreach (var expression in this._program.Expressions)
            {
                InterpretExpression(expression);
            }

        }

        private object InterpretExpression(SExpr expression)
        {
            return expression switch
            {
                var a when a is SExprList => VisitList(a as SExprList),
                var a when a is SExprBoolean => VisitAtom(a as SExprBoolean),
                var n when n is SExprNumber => VisitAtom(n as SExprNumber),
                _ => throw new ArgumentException(" Unknown Expression")
            };
        }


        public object VisitList(SExprList list)
        {
            // get the head as this decides the type of operation we are building
            // Then 'apply' the procedure to the elements
            if (list.Head is SExprListNode)
            {
                if ((list.Head as SExprListNode).CurrentValue is SExpressionSymbolOperator)
                {
                    var @operator = (list.Head as SExprListNode).CurrentValue as SExpressionSymbolOperator;
                    return ApplyOperator(@operator, list);
                }
            }
            return new object();
        }

        private object ApplyOperator(SExpressionSymbolOperator @operator, SExprList list)
        {
            if (OperationsLookup.Operations.TryGetValue(@operator.Value, out var func))
            {
                return ProcessListNumbers(func, list);
            }
            return "nil";
        }

        private double ProcessListNumbers(Func<double, double, double> func, SExprList list)
        {
            double? accumulator = null;
            foreach (var item in list)
            {
                if (!(item!.CurrentValue is SExpressionSymbolOperator))
                {
                    continue;
                }
                if (accumulator is null)
                {
                    var number = InterpretExpression(item.CurrentValue);
                    if (!(number is double))
                    {
                        throw new InterpreterError($"Not a number!");
                    }
                    else
                    {
                        accumulator = (double)number;
                    }

                    // Is the next node a value?
                    // or a terminator, cos that changes how we process.
                    if (item.Next is SExprBoolean)
                    {
                        // Get unary values;
                    }
                }
                else
                {
                    var number = InterpretExpression(item.CurrentValue);
                    if (!(number is double))
                    {
                        throw new InterpreterError($"Not a number!");
                    }
                    else
                    {
                        accumulator += func(accumulator.Value, (double)number);
                    }
                    if (item.Next is SExprBoolean)
                        break;
                }
            }
            return accumulator.Value;
        }

        public object VisitAtom(SExprNumber number)
        {
            throw new NotImplementedException();
        }

        public object VisitAtom(SExprSymbol symbol)
        {
            throw new NotImplementedException();
        }

        public object VisitAtom(SExprString @string)
        {
            throw new NotImplementedException();
        }

        public object VisitAtom(SExprBoolean boolean)
        {
            throw new NotImplementedException();
        }

        public object VisitProgram(SExprProgram action)
        {
            throw new NotImplementedException();
        }

        public object VisitListNode(SExprListNode action)
        {
            throw new NotImplementedException();
        }
    }
}
