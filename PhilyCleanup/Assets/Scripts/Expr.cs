using System.Collections.Generic;
using UnityEditor;

public abstract class Expr
{
    public interface Visitor<R>
    {
        R VisitAssignExpr(Assign expr);
        R VisitBinaryExpr(Binary expr);
        R VisitGroupingExpr(Grouping expr);
        R VisitUnaryExpr(Unary expr);
        R VisitVariableExpr(Variable expr);
    }

    public class Assign : Expr
    {
        public Lexer.Token Name;
        public Expr Value;

        public Assign(Lexer.Token name, Expr value)
        {
            this.Name = name;
            this.Value = value;
        }

        public R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitAssignExpr(this);
        }
    }

    public class Binary : Expr
    {
        public Expr Left;
        public Lexer.Token Operator;
        public Expr Right;

        public Binary(Expr left, Lexer.Token opr, Expr right)
        {
            this.Left = left;
            this.Operator = opr;
            this.Right = right;
        }

        R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }
    }

    public class Grouping : Expr
    {
        public Expr Expression;

        public Grouping(Expr expression)
        {
            this.Expression = expression;
        }

        public R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitGroupingExpr(this);
        }
    }

    public class Unary : Expr
    {
        public Lexer.Token Operator;
        public Expr Right;

        public Unary(Lexer.Token oprt, Expr right)
        {
            this.Operator = oprt;
            this.Right = right;
        }

        R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }

    public class Variable : Expr
    {
        public Lexer.Token Name;

        public Variable(Lexer.Token name)
        {
            this.Name = name;
        }

        public R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitVariableExpr(this);
        }
    }
}
