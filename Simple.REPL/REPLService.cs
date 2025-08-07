using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SExpression;
using SExpression.Parsing;
using SExpressions;
using System.ComponentModel.Design;
using System.Text;

namespace Simple.REPL
{
    internal class REPLService : IHostedService
    {
        private readonly ILogger<REPLService> _logger;
        private readonly Parser _parser;

        public REPLService(ILogger<REPLService> logger, Parser parser)
        {
            _logger = logger;
            _parser = parser;
        }

        public void Main(string[] args)
        {
            var input = "";
            var headLine = "S-Expression REPL";
            Console.WriteLine(headLine);
            Console.WriteLine(new String('=', headLine.Length));
            Console.WriteLine();

            var inputComplete = false;

            StringBuilder inputBuilder = new StringBuilder();

            while (input.ToLowerInvariant() != "##quit".ToLowerInvariant())
            {
                try
                {
                    Console.Write("> ");
                    input = Console.ReadLine();

                    if (input.StartsWith("##"))
                    {
                        // Empty new line, and we previously recorded user input
                        ProcessCodes(input);
                    }
                    else
                    {
                        if (input == String.Empty && inputComplete)
                        {
                            inputComplete = false;
                            var inputData = inputBuilder.ToString();
                            inputBuilder.Clear();
                            var scannerData = Scandata(inputData);
                            WriteScannerOuput(scannerData);
                            var parserOutpu = this._parser.Parse(scannerData.ToList());

                        } // Not a new line,
                        else if (input.Length > 0)
                        {
                            inputComplete = true;
                            inputBuilder.AppendLine(input);
                        }
                    }
                }
                catch (ScannerException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (ParserException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("Exiting REPL");
        }

        private IList<ScannerToken> Scandata(string inputData)
        {
           return new Scanner().ScanDocument(inputData);
        }

        private static void WriteScannerOuput(IList<ScannerToken> scannerData)
        {
            foreach (var scannerToken in scannerData)
            {
                Console.WriteLine(scannerToken);
            }
        }

        private static void ProcessCodes(string input)
        {
            Console.WriteLine("System Codes");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            Main([]);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

