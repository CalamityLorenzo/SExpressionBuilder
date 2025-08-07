using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SExpression.Parsing;

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
                    services.AddHostedService<REPLService>();
                })
                .Build()
                .Run();
        }
    }
}
