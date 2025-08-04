using SExpression.Core;

namespace SExpressions
{
    public class Scanner
    {
        public Scanner() { }

        private int CurrentIdx;
        private int CurrentLine;
        private int CurrentColumn;
        private ReadOnlyMemory<char> AllChars;

        private List<ScannerToken> Tokens = new List<ScannerToken>();
        public IList<ScannerToken> ScanDocument(string document)
        {
            AllChars = document.AsMemory();
            CurrentIdx = 0;
            CurrentLine = 1;
            CurrentColumn = 1;
            do
            {
                Process(GetCurrentChar());
                MoveNext();
            }
            while (!IsAtEnd());
            return Tokens;
        }

        private ScannerToken CreateToken(SExpression.Core.ScannerTokenType token, string canonicalValue, int startIndex, int length = 1)
        {
            return new(token, canonicalValue, AllChars.Span.Slice(startIndex, length).ToString(), CurrentLine, CurrentColumn);
        }

        private ScannerToken CreateToken(SExpression.Core.ScannerTokenType token, string canonicalValue, int startIndex, int startLine, int startColumn, int length = 1)
        {
            return new(token, canonicalValue, AllChars.Span.Slice(startIndex, length).ToString(), startLine, startColumn);
        }

        private void Process(char currentChar)
        {
            switch (currentChar)
            {
                case ',':
                    Tokens.Add(CreateToken(SExpression.Core.ScannerTokenType.Comma, ",", CurrentIdx));
                    break;
                case '\n' or '\r':
                    AddNewLine();
                    break;
                case '!' or '-' or '+' or '>' or '<' or '*' or '\\' or '=':

                    if (Peek() == '=')
                    {
                        var stringOperator = $"{currentChar}=";
                        var doubleOperator = LookUps.Operators[stringOperator];
                        Tokens.Add(CreateToken(doubleOperator, stringOperator, CurrentIdx, 2));
                    }
                    else
                    {
                        var stringOperator = $"{currentChar}";
                        var @operator = LookUps.Operators[stringOperator];
                        Tokens.Add(CreateToken(@operator, stringOperator, CurrentIdx));
                    }
                    break;
                case '\"':
                    ScanString();
                    break;
                case '(':
                    Tokens.Add(CreateToken(SExpression.Core.ScannerTokenType.OpenBracket, "(", CurrentIdx));
                    break;
                case ')':
                    Tokens.Add(CreateToken(SExpression.Core.ScannerTokenType.CloseBracket, ")", CurrentIdx));
                    break;
                case '{':
                    Tokens.Add(CreateToken(SExpression.Core.ScannerTokenType.OpenBrace, "{", CurrentIdx));
                    break;
                case '}':
                    Tokens.Add(CreateToken(SExpression.Core.ScannerTokenType.ClosedBrace, "}", CurrentIdx));
                    break;
                case ' ': break;
                case char c when char.IsLetter(c):
                    ScanIdentifierKeyword();
                    break;
                case char c when char.IsDigit(c):
                    ScanNumber();
                    break;
                default:
                    if (char.IsWhiteSpace(currentChar))
                    {
                        // Ignore whitespace
                    }
                    else if (currentChar == ';')
                    {
                        // Comment, ignore rest of line
                        while (!IsAtEnd() && GetCurrentChar() != '\n')
                        {
                            MoveNext();
                        }
                    }
                    else
                    {
                        // Unknown character, throw an error or handle it as needed
                        throw new InvalidOperationException($"Unknown character '{currentChar}' at line {CurrentLine}, column {CurrentColumn}.");
                    }
                    break;
            }
       }

        private void ScanString()
        {
            // When we arrrive here we know we have a double quote.
            // Scan the string until we find the terminating quote.
            // eg "grg" but "asdfsdf\"" and ""
            var startIdx = CurrentIdx+1;
            var startColumn = CurrentColumn;
            var peekedChar = Peek();
            // Have we entere4d into an escapted char stream eg \n \t \" etc
            var escapeChar= false;
            while (!IsAtEnd() && ((!escapeChar && peekedChar != '"') || (escapeChar)))
            {
                if(peekedChar == '\n') this.CurrentLine++;
                MoveNext();
                peekedChar = Peek();
                escapeChar = (GetCurrentChar() == '\\' && !IsAtEnd());
            }

            if (IsAtEnd() && GetCurrentChar() != '\"')
                throw new ScannerException($"{nameof(ScanString)} Reached end of input, expected terminating \"");
            
            MoveNext(); // Move past the closing quote
            // +1 to include the closing quote in the token length
            Tokens.Add(CreateToken(SExpression.Core.ScannerTokenType.String, "", startIdx, CurrentLine, startColumn, CurrentIdx - startIdx));

        }

        private void ScanNumber()
        {

            // When we arrrive here we know we have a number already.
            // So I'm trying to ensur what's next
            var startIdx = CurrentIdx;
            var startColumn = CurrentColumn;
            while (!IsAtEnd() && char.IsDigit(Peek()))
            {
                MoveNext();
            }
            var word = AllChars.Span.Slice(startIdx, ((CurrentIdx+1) - startIdx)).ToString().ToLowerInvariant();

            Tokens.Add(CreateToken(SExpression.Core.ScannerTokenType.Number, word, startIdx, CurrentLine, startColumn, CurrentIdx - startIdx));
        }

        private void ScanIdentifierKeyword()
        {
            var startIdx = CurrentIdx;
            var startColumn = CurrentColumn;

            while (!IsAtEnd() && char.IsLetterOrDigit(Peek()))
            {
                MoveNext();
            }

            var word = AllChars.Span.Slice(startIdx, ((CurrentIdx+1) - startIdx)).ToString().ToLowerInvariant();

            if (LookUps.Keywords.TryGetValue(word, out var canonicalValue))
            {
                Tokens.Add(
                CreateToken(SExpression.Core.ScannerTokenType.Keyword, canonicalValue, startIdx, CurrentLine, startColumn, CurrentIdx - startIdx)
                );

            }
            else
            {
                Tokens.Add(
                CreateToken(SExpression.Core.ScannerTokenType.Identifier, word, startIdx, CurrentLine, startColumn, CurrentIdx - startIdx)
                );
            }
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

        private void MoveNext()
        {
            if (!IsAtEnd())
            {
                CurrentIdx++;
                CurrentColumn++;
            }
        }

        private char Peek()
        {
            if (CurrentIdx + 1 < AllChars.Length)
                return AllChars.Span[CurrentIdx + 1];
            else
                return '\0';
        }

        private char PeekNext()
        {
            if (CurrentIdx + 2 < AllChars.Length)
                return AllChars.Span[CurrentIdx + 1];
            else
                return '\0';
        }

        private bool IsAtEnd()
        {
            return !(CurrentIdx != AllChars.Length);
        }
    }
}
