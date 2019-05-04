
using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Lexer
{
    private string Source { get; set; }
    private Dictionary<string, Token.TokenType> Keywords;

    public int Start;
    public int Current;
    public List<Token> Tokens;

    public Lexer(string expr)
    {
        Source = expr;

        Start = 0;
        Current = 0;
        
        // add keywords
        Keywords = new Dictionary<string, Token.TokenType>();
            Keywords.Add("repeat", Token.TokenType.REPEAT);
            Keywords.Add("if", Token.TokenType.IF);
            Keywords.Add("else", Token.TokenType.ELSE);
            Keywords.Add("macro", Token.TokenType.MACRO);
            Keywords.Add("do", Token.TokenType.DO);
            Keywords.Add("endm", Token.TokenType.ENDM);
    }
    
    public class Token
    {
        public enum TokenType
        {
            NUMBER, IDENTIFIER,
            PLUS, MINUS, MULTIPLY, DIVIDE,
            
            REPEAT, COLON, IF, ELSE, 
            HASH, MACRO, EOF, DO, ENDM
        };

        public TokenType type;
        public System.Object value;
        public string literal;
    }

    public void Scan()
    {
        while (!isAtEnd())
        {
            Start = Current;
            scanToken();
        }
        
        addToken(Token.TokenType.EOF, "");
    }

    void scanToken()
    {
        char c = advance();

        switch (c)
        {
            // nothing to see here, move along
            case ' ':
            case '\r':
            case '\t':
            case '\n':
                break;
            
            // single character tokens
            case '+':
                addToken(Token.TokenType.PLUS, "+");
                break;
            
            case '-':
                addToken(Token.TokenType.MINUS, "-");
                break;
                
            case '*':
                addToken(Token.TokenType.MULTIPLY, "*");
                break;
            
            case '/':
                addToken(Token.TokenType.DIVIDE, "/");
                break;
            
            case '#':
                addToken(Token.TokenType.HASH, "#");
                break;
            
            case ':':
                addToken(Token.TokenType.COLON, ":");
                break;
            
            default:
                if (isDigit(c))
                {
                    number();
                } else if (isAlpha(c))
                {
                    identifier();
                }
                else
                {
                    throw new ScanError("Invalid character found in input!");
                }
                break;
        }
    }

    void addToken(Token.TokenType toAdd, string lit, System.Object val = null)
    {
        Tokens.Add(new Token{type = toAdd, value = val, literal = lit});
    }

    void number()
    {
        while (isDigit(peek()))
        {
            advance();
        }
        
        addToken(Token.TokenType.NUMBER, Source.Substring(Start, Current));
    }

    void identifier()
    {
        while (isAlpha(peek()) || isDigit(peek()))
        {
            advance();
        }

        string text = Source.Substring(Start, Current);
        if (Keywords.ContainsKey(text))
        {
            addToken(Keywords[text], text);
        }
        else
        {
            addToken(Token.TokenType.IDENTIFIER, text);
        }
    }

    char advance()
    {
        Current += 1;
        return Source[Current - 1];
    }

    char peek()
    {
        if (Current + 1 >= Source.Length)
        {
            return '\0';
        }

        return Source[Current + 1];
    }

    bool isDigit(char c)
    {
        return (c >= '0' && c <= '9');
    }

    bool isAlpha(char c)
    {
        return ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'));
    }

    bool isAtEnd()
    {
        return Current >= Source.Length;
    }
}
