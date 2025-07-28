using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SExpression.Core.IR
{
    public enum SymbolType
    {
        None = 0,
        Keyword,
        Identifier,
        Operator
    }
}
