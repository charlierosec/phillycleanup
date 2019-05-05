using System.Collections.Generic;

public abstract class Stmt
{
    public interface Visitor<R>
    {
        R VisitBlockStmt(Block stmt);
        R VisitExpressionStmt(Expression stmt);
        R VisitAssignStmt(Assign stmt);
        R VisitRepeatStmt(Repeat stmt);
    }
    
    public class Block : Stmt
    {
        public List<Stmt> Statements;
        
        public Block(List<Stmt> statements)
        {
            this.Statements = statements;
        }   
        
        public override R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitBlockStmt(this);
        }
    }

    public class Expression : Stmt
    {
        public Expr InnerExpression;

        public Expression(Expr expression)
        {
            this.InnerExpression = expression;
        }

        public override R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitExpressionStmt(this);
        }
    }

    public class Assign : Stmt
    {
        public Lexer.Token Name;
        public Expr Initialiser;

        public Assign(Lexer.Token name, Expr initializer)
        {
            this.Name = name;
            this.Initialiser = initializer;
        }

        public override R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitAssignStmt(this);
        }
    }

    public class Repeat : Stmt
    {
        public Expr LowValue;
        public Expr HighValue;
        public Expr Condition;
        public Stmt.Block Block;

        public Repeat(Expr cond, Stmt.Block bl)
        {
            this.Condition = cond;
            this.Block = bl;
        }
        
        public Repeat(Expr lv, Expr hv, Stmt.Block bl)
        {
            this.LowValue = lv;
            this.HighValue = hv;
            this.Block = bl;
        }

        public override R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitRepeatStmt(this);
        }
    }

    public abstract R Accept<R>(Visitor<R> visitor);
}
