namespace SExpression.Core
{
    public enum TokenType
    {
        Comma,
        SemiColon,
        FullStop,
        QuestionMark,
        ForwardSlash,
        BackSlash,
        BackSlashEquals,
        Plus,
        PlusEquals,
        Minus,
        MinusEquals,
        Ping,
        PingEquals,
        GreaterThan,
        GreaterThanEquals,
        LessThan,
        LessThanEquals,
        Asterisk,
        AsteriskEquals,
        Hash,
        Equals,
        Percent,
        UnderScore,
        SinglePipe,
        DoublePipe,


        DoubleQuote,
        SingleQuote,
        Character,
        Number,
        Keyword,
        Identifier,
        String,
        OpenBracket,
        ClosedBracket,

        OpenBrace,
        ClosedBrace,

    }


    public static class LookUp
    {
        public static HashSet<string> keyWords = new(["if", "define", "else", "cond", "while", "xor", "and",]);
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
