using Microsoft.Extensions.Logging.Testing;
using SExpression.Parsing;
using SExpression.Printer;
using SExpressions;
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
        public void PrintNumbers(string input, string output)
        {
            var fakeLogger = new FakeLogger<Parser>();
            var scannerTokens = ScannerSetup(input);
            var parser = new SExpression.Parsing.Parser(fakeLogger);
            var parserOutput = parser.Parse(scannerTokens.ToList());


            var flp = new FakeLoggerProvider();
            
            var printer = new AstPrinter(flp.CreateLogger(""));
            parserOutput.ForEach(a=>a.Apply(printer));

            foreach (var item in flp.Collector.GetSnapshot())
            {
                logOutput.WriteLine(item.Message);
            }

        }
    }
}
