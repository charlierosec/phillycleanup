using System;
using System.Collections.Generic;
using System.Data;

public class Interpreter : Expr.Visitor<System.Object>, Stmt.Visitor<string> // this string doesn't actually matter, but we can't use Void in C#
{
    public class RuntimeError : Exception
    {
        public RuntimeError(string msg) : base(msg) {}
    }
    
    public Environment IntEnvironment = new Environment();

    public void Interpret(List<Stmt> statements)
    {
        try
        {
            foreach (var stmt in statements)
            {
                execute(stmt);
            }
        }
        catch (RuntimeError e)
        {
            // do something
        }
    }

    public object VisitBinaryExpr(Expr.Binary expr)
    {
        object left = evaluate(expr.Left);
        object right = evaluate(expr.Right);

        switch (expr.Operator.Type)
        {
            case Lexer.Token.TokenType.PLUS:
                return (int) left + (int) right;
            
            case Lexer.Token.TokenType.MINUS:
                return (int) left - (int) right;
            
            case Lexer.Token.TokenType.MULTIPLY:
                return (int) left * (int) right;
            
            case Lexer.Token.TokenType.DIVIDE:
                return (int) left / (int) right;
        }
        
        throw new RuntimeError($"Unknown operator '{expr.Operator.Literal}'");
    }

    public object VisitGroupingExpr(Expr.Grouping expr)
    {
        return evaluate(expr.Expression);
    }

    public object VisitUnaryExpr(Expr.Unary expr)
    {
        object right = evaluate(expr.Right);

        if (right is int)
        {
            if (expr.Operator.Type == Lexer.Token.TokenType.MINUS)
            {
                return -1 * (int) right;
            }
            
        } else if (right is bool)
        {
            if (expr.Operator.Type == Lexer.Token.TokenType.BANG)
            {
                return !((bool) right);
            }
        }
        
        throw new RuntimeError($"Wrong type give for {expr.Operator.Literal} operator");
    }

    public object VisitVariableExpr(Expr.Variable expr)
    {
        return IntEnvironment.Get(expr.Name);
    }

    public object VisitLiteralExpr(Expr.Literal expr)
    {
        return expr.Value;
    }

    public string VisitBlockStmt(Stmt.Block stmt)
    {
        executeBlock(stmt.Statements, new Environment(IntEnvironment));
        return "";
    }

    public string VisitExpressionStmt(Stmt.Expression stmt)
    {
        evaluate(stmt.InnerExpression);
        return "";
    }

    public string VisitAssignStmt(Stmt.Assign stmt)
    {
        object value = evaluate(stmt.Initialiser);

        IntEnvironment.Define(stmt.Name, value);
        return ""; 
    }

    public string VisitPrintStmt(Stmt.Print stmt)
    {
        object value = evaluate(stmt.Expression);
        Console.WriteLine(value);

        return "";
    }

    public string VisitRepeatStmt(Stmt.Repeat stmt)
    {
        // we've got ourselves a low-high expression bois
        if (stmt.LowValue != null)
        {
        }
        throw new NotImplementedException();
    }

    object evaluate(Expr expr)
    {
        return expr.Accept(this);
    }

    void execute(Stmt stmt)
    {
        stmt.Accept(this);
    }

    void executeBlock(List<Stmt> statements, Environment env)
    {
        Environment previous = this.IntEnvironment;
        
        try
        {
            this.IntEnvironment = env;

            foreach (var statement in statements)
            {
                execute(statement);
            }
        }
        finally
        {
            this.IntEnvironment = previous;
        }
    }
}
