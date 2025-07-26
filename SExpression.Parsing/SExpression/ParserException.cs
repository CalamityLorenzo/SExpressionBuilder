
namespace SExpression
{
    [Serializable]
    internal class ParserException : Exception
    {
        private string v1;
        private string v2;

        public ParserException()
        {
        }

        public ParserException(string? message) : base(message)
        {
        }

        public ParserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}