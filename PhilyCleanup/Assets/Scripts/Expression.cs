using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor.UIElements;
using UnityEngine;
using Object = UnityEngine.Object;

public class Expression : MonoBehaviour
{
    public Dictionary<string, System.Object> Environment;
    public TrashMan Player;

    void Start()
    {
        Environment = new Dictionary<string, System.Object>();
    }

    public void Execute(string expression)
    {
        var split = expression.Split(' ');
        switch (split[0])
        {
            case "let":
                Environment.Add(split[1], Evaluate(split[3]));
                break;
            
            case "print":
                Environment.TryGetValue(split[1], out var prnt);
                print(prnt);
                break;
        }
    }

    System.Object Evaluate(string value)
    {
        if (value == "GetPlayer()")
        {
            return Player;
        }  
        
        if (Int32.TryParse(value, out var ret))
        {
            return ret;
        }

        if (value.StartsWith("\""))
        {
            return value;
        }
        
        

        return null;
    }
}
