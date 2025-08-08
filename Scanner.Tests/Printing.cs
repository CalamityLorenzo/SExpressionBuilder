using Microsoft.Extensions.Logging.Testing;
using SExpression.Parsing;
using SExpression.Printer;
using SExpressions;
using System.Text;
using Xunit.Abstractions;

namespace Compiler.Tests
{
    public class Printing
    {
        private ITestOutputHelper logOutput;

        public Printing(ITestOutputHelper testOutputHelper)
        {
            logOutput = testOutputHelper;
        }

        private IEnumerable<ScannerToken> ScannerSetup(string input)
        {
            var scanner = new SExpressions.Scanner();
            return scanner.ScanDocument(input);
        }
        [Theory]
        [InlineData("(456 789 123)", "(456 789 123)")]
        [InlineData("(+ 1 2)", "(+ 1 2)")]
        [InlineData("(+ 1 2 (/ 3 4))", "(+ 1 2 (/ 3 4))")]
        [InlineData("(list 1 2 3)", "(list 1 2 3)")]
        [InlineData("(define myFunc \"My function has meaning duded\" (setv a 10) (setv b 20) (return (+ a b)))","(define myFunc \"My function has meaning duded\" (setv a 10) (set b 20) (return (+ a b))")]
        public void PrintNumbers(string input, string output)
        {
            var fakeLogger = new FakeLogger<Parser>();
            var scannerTokens = ScannerSetup(input);
            var parser = new SExpression.Parsing.Parser(fakeLogger);
            var parserOutput = parser.Parse(scannerTokens.ToList());


            var flp = new FakeLoggerProvider();
            var stringBuilder = new StringBuilder();
            var printer = new AstPrinter(new FakeLogger<AstPrinter>());
            printer.ConfigureWriter((str) => stringBuilder.Append(str));
            parserOutput.Expressions.ToList().ForEach(a=>a.Apply(printer));

            logOutput.WriteLine(stringBuilder.ToString());

            //foreach (var item in flp.Collector.GetSnapshot())
            //{
            //    logOutput.WriteLine(item.Message);
            //}
            //Assert(out == )
        }
    }
}
