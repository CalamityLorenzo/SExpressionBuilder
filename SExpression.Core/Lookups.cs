using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SExpression.Core
{
    public static class LookUps
    {
        public static HashSet<string> keyWords = new(["if", "define", "else", "cond", "while", "xor", "and", "t", "nil"]);
        private static Dictionary<string, TokenType> operators = new()
                                                        {
                                                            {"+",TokenType.Plus },
                                                            {"+=",TokenType.PlusEquals  },
                                                            {"-",TokenType.Minus },
                                                            {"-=",TokenType.MinusEquals  } ,
                                                            {"*",TokenType.Asterisk },
                                                            {"*=",TokenType.AsteriskEquals},
                                                            {"\\",TokenType.BackSlash},
                                                            {"\\=",TokenType.BackSlashEquals  },
                                                            {">",TokenType.GreaterThan },
                                                            {">=",TokenType.GreaterThanEquals  },
                                                            {"<",TokenType.LessThan},
                                                            {"<=",TokenType.LessThanEquals  },
                                                            {"!",TokenType.Ping},
                                                            {"!=",TokenType.PingEquals  },
                                                            {"=",TokenType.Ping},
                                                            {"==",TokenType.PingEquals  },
        };


        public static HashSet<string> Keywords => keyWords;
        public static Dictionary<string, TokenType> Operators => operators;
    }

}
