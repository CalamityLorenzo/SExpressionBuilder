﻿using Microsoft.Extensions.Logging.Testing;
using SExpression.Core.IR;
using SExpression.Parsing;
using SExpressions;
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

        private IEnumerable<ScannerToken> ScannerSetup(string input)
        {
            var scanner = new SExpressions.Scanner();
            return scanner.ScanDocument(input);
        }

        [Theory]
        [InlineData("(\"abcdef\")", 3)]
        [InlineData("(+ 123 \"abcdef\" define)", 6)]
        [InlineData("(+ 123 \"abcdef\")(- 444 2222)", 5)]
        [InlineData("(+ 123 \"abcdef\" (+ 2 (* 4 4))) (- 444 2222)", 5)]
        public void ParseBasicElementsCount(string input, int tokenCount)
        {
            var fakeLogger = new FakeLogger<Parser>();
            var scannerTokens = ScannerSetup(input);
            var parser = new SExpression.Parsing.Parser(fakeLogger);
            var parserOutput = parser.Parse(scannerTokens.ToList());
            logOutput.WriteLine($"Number of SExpressions : {parserOutput.Count()}");
        }

        [Theory]
        [InlineData("(\"abcdef\")", "abcdef")]
        [InlineData("(\"\")", "")]
        [InlineData("(\"1\")", "1")]
        [InlineData("(\"abc    def\")", "abc    def")]
        public void ParseStringTests(string input, string value)
        {
            var expectedValue = new SExprString(value);
            var fakeLogger = new FakeLogger<Parser>();
            var scannerTokens = ScannerSetup(input);
            var parser = new SExpression.Parsing.Parser(fakeLogger);
            var parserOutput = parser.Parse(scannerTokens.ToList());
            var stringOutput = (parserOutput.First() as SExprList).Expressions[0] as SExprString;

            logOutput.WriteLine($"Does they do the match: {stringOutput.Value == expectedValue.Value}");
            logOutput.WriteLine($"input:\t{stringOutput.Value}");
            logOutput.WriteLine($"output:\t{expectedValue.Value}");
            Assert.True(stringOutput.Value == expectedValue.Value);
        }
    }
}