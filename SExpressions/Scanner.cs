using SExpression.Core;
using System.Net.Http.Headers;

namespace SExpressions
{
    public class Scanner
    {
        public Scanner() { }

        private int CurrentIdx;
        private int CurrentLine;
        private int CurrentColumn;
        private ReadOnlyMemory<char> AllChars;

        private Char CurrentChar;
        private List<ScannerToken> Tokens = new List<ScannerToken>();
        public IList<ScannerToken> ScanDocument(string document)
        {
            AllChars = document.AsMemory();
            CurrentIdx = 0;
            while (!IsAtEnd())
            {
                Process(GetCurrentChar());
                MoveNext();
            }
            return Tokens;
        }

        private void MoveNext()
        {
            if (!IsAtEnd())
            {
                CurrentIdx++;
                CurrentChar++;
            }
        }

        private void Process(char currentChar)
        {
            switch (currentChar)
            {
                case ',':
                    Tokens.Add(CreateToken(TokenType.Comma, ","));
                    break;
                case '\n' or '\r':
                    AddNewLine();
                    break;
                case '!':
                    if (Peek() == '=') {
                        Tokens.Add(CreateToken(TokenType.PingEquals, "!="));
                        MoveNext();
                    }
                    else
                    {
                        Tokens.Add(CreateToken(TokenType.Ping, "!="));
                    }
                    break;
                case '(':
                        Tokens.Add(CreateToken(TokenType.OpenBracket, "("));
                    break;
                case ')':
                    Tokens.Add(CreateToken(TokenType.ClosedBracket, ")"));
                    break;
                case '{':
                    Tokens.Add(CreateToken(TokenType.OpenBrace, "{"));
                    break;
                case '}':
                    Tokens.Add(CreateToken(TokenType.ClosedBrace, "}"));
                    break;
                case ' ': break;
                case char c when char.IsLetter(c):
                    ProcessIdentifier();
                    break;
            }
        }

        private void ProcessIdentifier()
        {
            var startIdx = CurrentIdx;
            while (!IsAtEnd() && char.IsLetterOrDigit(GetCurrentChar()))
            {
                MoveNext();
            }
            var value = AllChars.Span.Slice(startIdx, CurrentIdx - startIdx).ToString();
            Tokens.Add(new ScannerToken(TokenType.Keyword, value, value, CurrentLine, CurrentColumn));
        }

        private ScannerToken CreateToken(TokenType ping, string Value, int length =1)
        {
            return new(SExpression.Core.TokenType.PingEquals, "!=", AllChars.Span.Slice(CurrentIdx, length).ToString(), CurrentLine, CurrentColumn);
        }

        private void AddNewLine()
        {
            if (!IsAtEnd())
            {
                CurrentLine++;
                CurrentColumn = 0;
            }
        }

        private char GetCurrentChar() => AllChars.Span[CurrentIdx];

        private char Peek()
        {
            if(CurrentIdx+1 <= AllChars.Length-1)
                return AllChars.Span[CurrentIdx+1];
            else
                return '\0';
        }
        private bool IsAtEnd()
        {
            return CurrentIdx == AllChars.Length - 1;
        }
    }
}
