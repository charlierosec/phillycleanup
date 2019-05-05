using System;
using System.Collections.Generic;
using System.Data;

public class Interpreter : Expr.Visitor<System.Object>, Stmt.Visitor<string> // this string doesn't actually matter, but we can't use Void in C#
{
    public class RuntimeError : Exception
    {
        public RuntimeError(string msg) : base(msg) {}
    }
    
    private Environment IntEnvironment = new Environment();

    void interpret(List<Stmt> statements)
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
        
        throw new NotImplementedException(); // more fun implementing stuffs
    }

    public object VisitGroupingExpr(Expr.Grouping expr)
    {
        return evaluate(expr.Expression);
    }

    public object VisitUnaryExpr(Expr.Unary expr)
    {
        object right = evaluate(expr.Right);

        switch (expr.Operator.Type)
        {
            case Lexer.Token.TokenType.BANG:
                // do stuf
            
            case Lexer.Token.TokenType.MINUS:
                // do stuff
            
                // fall through for now because it sucks
            default:
                throw new RuntimeError("Invalid operator in unary expression");
        }
    }

    public object VisitVariableExpr(Expr.Variable expr)
    {
        return IntEnvironment.Get(expr.Name);
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

    public string VisitRepeatStmt(Stmt.Repeat stmt)
    {
        // still need to implement a repeat statement
        // but im lazy
        // so that's a job for 3am me
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
