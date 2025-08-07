using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SExpression.Core
{
    public static class LookUps
    {
        public static HashSet<string> keyWords = new(["if", "define", "else", "cond", "while", "xor", "and", "t", "nil", "list", "setv", "and"]);
        private static Dictionary<string, ScannerTokenType> operators = new()
                                                        {
                                                            {"+",ScannerTokenType.Plus },
                                                            {"+=",ScannerTokenType.PlusEquals  },
                                                            {"-",ScannerTokenType.Minus },
                                                            {"-=",ScannerTokenType.MinusEquals  } ,
                                                            {"*",ScannerTokenType.Asterisk },
                                                            {"*=",ScannerTokenType.AsteriskEquals},
                                                            {"/",ScannerTokenType.ForwardSlash},
                                                            {"\\=",ScannerTokenType.ForwardSlashEquals},
                                                            {">",ScannerTokenType.GreaterThan },
                                                            {">=",ScannerTokenType.GreaterThanEquals  },
                                                            {"<",ScannerTokenType.LessThan},
                                                            {"<=",ScannerTokenType.LessThanEquals  },
                                                            {"!",ScannerTokenType.Ping},
                                                            {"!=",ScannerTokenType.PingEquals  },
                                                            {"=",ScannerTokenType.Ping},
                                                            {"==",ScannerTokenType.PingEquals  },
        };


        public static HashSet<string> Keywords => keyWords;
        public static Dictionary<string, ScannerTokenType> Operators => operators;
    }

}
