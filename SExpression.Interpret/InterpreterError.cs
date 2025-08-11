
namespace SExpression.Interpret
{
    [Serializable]
    internal class InterpreterError : Exception
    {
        public InterpreterError()
        {
        }

        public InterpreterError(string? message) : base(message)
        {
        }

        public InterpreterError(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}