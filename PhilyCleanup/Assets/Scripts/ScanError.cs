using System;

namespace DefaultNamespace
{
    public class ScanError : Exception
    {
        // pretty much just an exception lol
        public ScanError(string msg) : base(msg)
        {
        }
    }
}