using Microsoft.Extensions.Logging;
using SExpression.Core.IR;

namespace SExpression.Interpret
{
    public class Interpreter
    {
        private SExprProgram _program;
        private readonly ILogger<Interpreter> _logger;

        public Interpreter(ILogger<Interpreter> logger)
        {
            this._logger = logger;
        }


        public void Input(Core.IR.SExprProgram program)
        {
            this._program = program;
            foreach (var expression in this._program.Expressions)
            {
                InterpretExpression(expression);
            }
            
        }

        private void InterpretExpression(SExpr expression)
        {
            switch (expression)
            {
                case var a when a is SExprList:
                    ProcessList(a as SExprList);
                    break;
                case var a when a is SExprBoolean:
                    ProcessBoolean(a as SExprBoolean);
                    break;
                default: throw new ArgumentException(" Unknown Expression");
            }
        }

        private void ProcessBoolean(SExprBoolean? sExprBoolean)
        {
            throw new NotImplementedException();
        }

        private void ProcessList(SExprList list)
        {
            // get the head as this decides the type of operation we are building
            if(list.Head is SExpressionSymbolOperator)
            {
                // 
            }
        }
    }
}
