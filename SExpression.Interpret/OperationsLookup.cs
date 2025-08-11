using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SExpression.Interpret
{
    internal static class OperationsLookup
    {
        internal static Dictionary<string, Func<double, double, double>> Operations = new()
        {
            { "+", new((l, r) => l + r)},
            { "-", new((l, r) => l - r)},
            { "*", new((l, r) => l * r)},
            { "/", new((l, r) => l / r)},
        };
    }
}
