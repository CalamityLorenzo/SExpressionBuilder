using Microsoft.Extensions.Logging.Testing;
using SExpression.Core.IR;
using SExpression.Parsing;
using SExpressions;
using System.ComponentModel;
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
        [InlineData("(456 789 123)", 3)]
        [InlineData("(\"abcdef\")", 3)]
        [InlineData("(+ 123 \"abcdef\" define)", 6)]
        [InlineData("(+ 123 \"abcdef\")(- 444 2222)", 5)]
        [InlineData("(+ 123 \"abcdef\" (+ 2 (* 4 4)) (- 444 2222))", 5)]
        public void ParseBasicTokens(string input, int tokenCount)
        {
            var fakeLogger = new FakeLogger<Parser>();
            var scannerTokens = ScannerSetup(input);
            var parser = new SExpression.Parsing.Parser(fakeLogger);
            var parserOutput = parser.Parse(scannerTokens.ToList());
            logOutput.WriteLine($"Number of SExpressions : {parserOutput.Count()}");
        }


        [Theory]
        [InlineData("(+ 123 \"abcdef\" define)", 4)]
        [InlineData("(+ \"abcdef\" define)", 3)]
        [InlineData("()", 0)]
        public void BasicListElementCount(string input, int listLength)
        {
            var fakeLogger = new FakeLogger<Parser>();
            var scannerTokens = ScannerSetup(input);
            var parser = new SExpression.Parsing.Parser(fakeLogger);
            var parserOutput = parser.Parse(scannerTokens.ToList());
            logOutput.WriteLine($"Number of List nodes : {listLength} == {(parserOutput.First() as SExprList).Length}");
            Assert.True(listLength == (parserOutput.First() as SExprList).Length);
        }
        [Fact]
        //[InlineData("(+ 123 \"abcdef\" define)", 4)]
        public void ListElementsCount()
        {
            string[] source = ["(+ 123 \"abcdef\" define)", "(+ 123 \"abcdef\" (+ 2 (* 4 4)) (- 444 2222))"];

            for (var x = 0; x < source.Length; x++)
            {
                var fakeLogger = new FakeLogger<Parser>();
                var scannerTokens = ScannerSetup(source[x]);
                var parser = new SExpression.Parsing.Parser(fakeLogger);
                var parserOutput = parser.Parse(scannerTokens.ToList());
                if (x == 0)
                {
                    var list = parserOutput.First() as SExprList;
                    logOutput.WriteLine($"List Length: {list.Length}== {4}");
                    Assert.True(list.Length == 4);
                }
                else
                {
                    var list = parserOutput.First() as SExprList;
                    logOutput.WriteLine($"************************************");
                    logOutput.WriteLine($"List Length: {list.Length}== {4}");
                    foreach (var node in list)
                    {
                        if (node is SExprListNode && ((SExprListNode)node).CurrentValue is SExprList)
                        {
                            var subList = ((SExprListNode)node).CurrentValue as SExprList;
                            logOutput.WriteLine($"SubList : Length: {subList.Length}== {3}");
                            foreach (var subNode in subList)
                            {
                                if (subNode is SExprListNode && ((SExprListNode)subNode).CurrentValue is SExprList)
                                {
                                    var subSubList = ((SExprListNode)subNode).CurrentValue as SExprList;
                                    logOutput.WriteLine($"subSubList  : Length: {subSubList.Length}== {2}");
                                }
                            }
                        }
                    }
                }
            }
        }




        [Theory]
        [InlineData("(+ 123)", 5)]
        [InlineData("(+ 123 \"abcdef\")", 5)]
        [InlineData("(+ 123 (12 12) \"abcdef\")", 5)]
        [InlineData("(+ 123 \"abcdef\" 567)", 5)]
        [InlineData("(+ 123 \"abcdef\" (+ 2 (* 4 4)))", 5)]
        public void ParserStructure(string input, int tokenCount)
        {
            var fakeLogger = new FakeLogger<Parser>();
            var scannerTokens = ScannerSetup(input);
            var parser = new SExpression.Parsing.Parser(fakeLogger);
            var parserOutput = parser.Parse(scannerTokens.ToList());
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
            var stringOutput = (parserOutput.First() as SExprList).Value;
            logOutput.WriteLine($"Does they do the match: {stringOutput == expectedValue.Value}");
            logOutput.WriteLine($"input:\t{stringOutput}");
            logOutput.WriteLine($"output:\t{expectedValue}");
            Assert.True(stringOutput == expectedValue.Value);
        }

        [Fact]
        public void EnumerateListNodes()
        {
            string[] source = ["(+ 123 \"abcdef\")", "(+ 123 (12 12) \"abcdef\")"];
            var fakeLogger = new FakeLogger<Parser>();
            foreach (var src in source)
            {
                var scannerTokens = ScannerSetup(src);
                var parser = new SExpression.Parsing.Parser(fakeLogger);
                var parserOutput = parser.Parse(scannerTokens.ToList());
                var list = parserOutput[0] as SExprList;

                foreach (var node in list!)
                {
                    logOutput.WriteLine(node.ToString());
                    if (node is not null && node is SExprListNode)
                    {
                        var nodeData = (node as SExprListNode).CurrentValue;
                        if (nodeData is SExprList)
                        {
                            foreach (var node2 in (nodeData as SExprList))
                            {
                                logOutput.WriteLine($"\tnode2.ToString()");
                            }
                            logOutput.WriteLine("SUB Complete\n============\n\n");

                        }
                    }
                }
                logOutput.WriteLine("List Complete\n============\n\n");
            }

        }
    }
}