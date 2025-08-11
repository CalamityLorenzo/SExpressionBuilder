using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SExpression.Parsing;
using SExpression.Printer;

namespace Simple.REPL
{
    internal class Program
    {
        /// <summary>
        /// SImple REPL that will regurgiate some output until we fire the magic command.
        /// </summary>
        /// <param name="args"></param>

        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddLogging();
                    services.AddSingleton<Parser>();
                    services.AddSingleton<AstPrinter>();
                    services.AddHostedService<REPLService>();
                })
                .Build()
                .Run();

            Func<double, double, double> func = new((l, r) => l + r);
        }


    }
}
