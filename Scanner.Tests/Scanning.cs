using Xunit.Abstractions;

namespace Scanner.Tests
{
    public class Scanning
    {
        private ITestOutputHelper logOutput;

        public Scanning(ITestOutputHelper testOutputHelper)
        {
            logOutput = testOutputHelper;
        }

        [Theory]
        [InlineData("()", 2)]
        [InlineData(" ", 0)]
        [InlineData("        ", 0)]
        [InlineData("a", 1)]
        [InlineData("1234567", 1)]
        [InlineData("123+4567", 3)]
        [InlineData("123+4567-123", 5)]
        [InlineData("(+ 123 \"abcdef\" define)", 6)]
        [InlineData("(+ 123 abc (define (= a 2)))", 13)]
        public void ScanBasicTokens(string input, int tokenCount)
        {
            var scanner = new SExpressions.Scanner();
            var output = scanner.ScanDocument(input);
            logOutput.WriteLine($"Number of tokens : {output.Count()}");
            Assert.True(output.Count == tokenCount);
        }

        [Theory]
        [InlineData("\n\n \na",0,4,1)]
        [InlineData("a\n\n \na",0,1,1)]
        public void LinesCount(string input, int element,int line, int col)
        {
            var scanner = new SExpressions.Scanner();
            var output = scanner.ScanDocument(input);
            logOutput.WriteLine($"Line : {output[element].lineNumber}");
            logOutput.WriteLine($"Row : {output[element].column}");

            Assert.True(output[element].lineNumber == line);
            Assert.True(output[element].column == col);
        }


        [Theory]
        [InlineData("123+4567", 3)]
        public void ScannerBasicExpression(string input, int tokenCount)
        {
            var scanner = new SExpressions.Scanner();
            var output = scanner.ScanDocument(input);
            logOutput.WriteLine($"Number of tokens : {output.Count()}");
            Assert.True(output.Count == tokenCount);
        }

        [Theory]
        [InlineData("\"abcdef\"", 1)]
        [InlineData("(\"abcdef\")", 3)]
        [InlineData("(\"ab\" \"cdef\")", 4)]
        public void ScannerStrings(string input, int tokenCount)
        {
            var scanner = new SExpressions.Scanner();
            var output = scanner.ScanDocument(input);
            logOutput.WriteLine($"Number of tokens : {output.Count()}");
            Assert.True(output.Count == tokenCount);
        }
    }
}
