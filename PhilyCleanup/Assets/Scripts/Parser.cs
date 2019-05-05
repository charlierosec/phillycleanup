using System;
using System.Collections.Generic;

public class Parser
{
    public class ParseError : Exception
    {
        public ParseError(string msg) : base(msg) {}
    }

    public List<Lexer.Token> Tokens;
    public List<ParseError> Errors;
    
    private int Current = 0;

    public Parser() : this(new List<Lexer.Token>()) {}
    
    public Parser(List<Lexer.Token> tokens)
    {
        this.Tokens = tokens;
        this.Errors = new List<ParseError>();
    }

    public List<Stmt> Parse(List<Lexer.Token> tokens)
    {
        this.Tokens = tokens;
        return Parse();
    }
    
    public List<Stmt> Parse()
    {
        var statements = new List<Stmt>();

        try
        {
            while (!isAtEnd())
            {
                Stmt stmt = statement();
                if (stmt != null)
                {
                    statements.Add(statement());
                }
            }

        }
        catch (ParseError e)
        {
            Errors.Add(e);
        }

        return statements;
    }

    Stmt statement()
    {
        try
        {
            if (match(Lexer.Token.TokenType.DO)) return new Stmt.Block(block());
            if (match(Lexer.Token.TokenType.LET)) return assignment();
            if (match(Lexer.Token.TokenType.REPEAT)) return repeat();

            return new Stmt.Expression(expression());
        }
        catch (ParseError e)
        {
            Errors.Add(e);
            synchronize();
        }

        // if we're here, we errored out
        return null;
    }

    List<Stmt> block()
    {
        List<Stmt> statements = new List<Stmt>();

        while (!check(Lexer.Token.TokenType.END) && !isAtEnd())
        {
            statements.Add(statement());
        }

        consume("Expected 'end' after 'do'", Lexer.Token.TokenType.END);
        return statements;
    }

    Stmt assignment()
    {
        Lexer.Token name = consume("Expected identifier after 'let'", Lexer.Token.TokenType.IDENTIFIER);

        Expr initialiser = null;
        if (match(Lexer.Token.TokenType.EQUAL))
        {
            initialiser = expression();
        }

        consume("Expected new line after variable declaration", Lexer.Token.TokenType.NEWLINE);
        return new Stmt.Assign(name, initialiser);
    }

    Stmt repeat()
    {
        /*
         * TODO: implement repeat loop with condition 
         */
        Expr left = expression();
        consume("Expected ':' in repeat", Lexer.Token.TokenType.COLON); 
        Expr right = expression();

        Stmt.Block blk = new Stmt.Block(block());
        
        return new Stmt.Repeat(left, right, blk);
    }

    Expr expression()
    {
        if (match(Lexer.Token.TokenType.LEFT_PAREN)) return grouping();
        if (match(Lexer.Token.TokenType.MINUS, Lexer.Token.TokenType.BANG)) return unary();

        return addition();
    }

    Expr grouping()
    {
        Expr expr = expression();

        consume("Expected ')' after '('", Lexer.Token.TokenType.RIGHT_PAREN);
        return new Expr.Grouping(expr);
    }

    Expr unary()
    {
        Lexer.Token op = previous();

        Expr expr = null;
        if (match(Lexer.Token.TokenType.LEFT_PAREN)) expr = grouping();
            else expr = unary();
        
        return new Expr.Unary(op, expr);
    }

    Expr addition()
    {
        Expr expr = multiplication();

        while (match(Lexer.Token.TokenType.PLUS, Lexer.Token.TokenType.MINUS))
        {
            Lexer.Token op = previous();
            Expr right = multiplication();
            expr = new Expr.Binary(expr, op, right);
        }

        return expr;
    }

    Expr multiplication()
    {
        Expr expr = unary();

        while (match(Lexer.Token.TokenType.MULTIPLY, Lexer.Token.TokenType.DIVIDE))
        {
            Lexer.Token op = previous();
            Expr right = unary();
            expr = new Expr.Binary(expr, op, right);
        }

        return expr;
    }

    Lexer.Token peek()
    {
        return Tokens[Current];
    }

    Lexer.Token previous()
    {
        return Tokens[Current - 1];
    }

    Lexer.Token consume(string message, params Lexer.Token.TokenType[] types)
    {
        foreach(var type in types)
        {
            if (check(type))
            {
                return advance();
            }
        }
        
        throw new ParseError(message);
    }

    bool match(params Lexer.Token.TokenType[] tokens)
    {
        foreach(var type in tokens)
        {
            if (check(type))
            {
                advance();
                return true;
            }
        }

        return false;
    }

    Lexer.Token advance()
    {
        if (!isAtEnd())
        {
            Current += 1;
        }

        return previous();
    }

    void synchronize()
    {
        advance();

        while (!isAtEnd() && !(previous().Type == Lexer.Token.TokenType.NEWLINE))
        {
            advance();
        }
    }
    
    bool isAtEnd()
    {
        return peek().Type == Lexer.Token.TokenType.EOF;
    }

    bool check(Lexer.Token.TokenType type)
    {
        if (isAtEnd())
        {
            return false;
        }
        
        return peek().Type == type;
    }
}
