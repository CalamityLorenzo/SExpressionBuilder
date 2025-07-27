using Microsoft.Extensions.Logging.Testing;
using SExpression.Parsing;
using Xunit.Abstractions;

namespace Compiler.Tests
{
    public class Parsing
    {
        private ITestOutputHelper logOutput;

        public Parsing(ITestOutputHelper testOutputHelper)
        {
            logOutput = testOutputHelper;
        }

        [Theory]
        [InlineData("(\"abcdef\")", 3)]
        [InlineData("(+ 123 \"abcdef\" define)", 6)]
        [InlineData("(+ 123 \"abcdef\")(- 444 2222)", 5)]
        [InlineData("(+ 123 \"abcdef\" (+ 2 (* 4 4))) (- 444 2222)", 5)]
        public void ParseBasicTokens(string input, int tokenCount)
        {
            var fakeLogger = new FakeLogger<Parser>();
            var scanner = new SExpressions.Scanner();
            var outputTokens = scanner.ScanDocument(input);
            logOutput.WriteLine($"Number of tokens : {outputTokens.Count()}");
            var parser = new SExpression.Parsing.Parser(fakeLogger);
            var parserOutput = parser.Parse(outputTokens.ToList());
            logOutput.WriteLine($"Number of SExpressions : {parserOutput.Count()}");
        }
    }
}