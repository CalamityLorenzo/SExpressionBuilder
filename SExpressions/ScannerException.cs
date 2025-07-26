
namespace SExpressions
{
    [Serializable]
    internal class ScannerException : Exception
    {
        public ScannerException()
        {
        }

        public ScannerException(string? message) : base(message)
        {
        }

        public ScannerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}