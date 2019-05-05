using System;
using System.Collections.Generic;

public class Environment
{
    public Environment Enclosing;
    private Dictionary<string, Object> values = new Dictionary<string, Object>();
    
    public Environment() : this(null) {}

    public Environment(Environment enclosing)
    {
        this.Enclosing = enclosing;
    }

    public object Get(Lexer.Token name)
    {
        if (values.ContainsKey(name.Literal))
        {
            return values[name.Literal];
        }

        if (Enclosing != null) return Enclosing.Get(name);

        throw new Interpreter.RuntimeError("Attempting to access undefined variable " + name.Literal);
    }

    public Environment Define(Lexer.Token name, Object value)
    {
        if (!values.ContainsKey(name.Literal))
        {
            values.Add(name.Literal, value);
            return this;
        }
        
        values[name.Literal] = value;
        return this;
    }
}
