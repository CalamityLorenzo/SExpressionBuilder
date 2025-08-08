using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SExpression;
using SExpression.Parsing;
using SExpression.Printer;
using SExpressions;
using System.Text;

namespace Simple.REPL
{
    internal class REPLService : IHostedService
    {
        private readonly ILogger<REPLService> _logger;
        private readonly Parser _parser;
        private readonly AstPrinter _printer;

        // Used by the DS codes
        private bool terminate = false;
        private bool displayScanningResult = true;

        public REPLService(ILogger<REPLService> logger, Parser parser, AstPrinter printer)
        {
            _logger = logger;
            _parser = parser;
            _printer = printer;
            _printer.ConfigureWriter(Console.Write);
        }

        public void Main(string[] args)
        {
            var input = "";
            ClearTheScreen();
            var inputComplete = false;

            StringBuilder inputBuilder = new StringBuilder();

            while (!terminate)
            {
                try
                {
                    Console.Write("> ");
                    input = Console.ReadLine();

                    if (input.StartsWith("##"))
                    {
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
                            if (displayScanningResult)
                                WriteScannerOuput(scannerData);
                            var parserOutput = this._parser.Parse(scannerData.ToList());
                            parserOutput.Apply(_printer);

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

        private void ClearTheScreen()
        {
            var headLine = "S-Expression REPL";
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(headLine);
            Console.WriteLine(new String('=', headLine.Length));
            Console.WriteLine();

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

        private void ProcessCodes(string input)
        {
            switch (input)
            {
                case "##quit":
                    Console.WriteLine("QUIT!");
                    terminate = true;
                    break;
                case "##clear":
                    ClearTheScreen();
                    break;
                case "##scanoutput":
                    displayScanningResult = !displayScanningResult;
                    Console.WriteLine($"Scanner output displayed : {displayScanningResult}");
                    break;
                default: throw new NotImplementedException("That code does not exist");
            }
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

