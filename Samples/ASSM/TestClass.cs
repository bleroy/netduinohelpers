using System;
using System.Reflection;
using Microsoft.SPOT;

namespace ASSM
{
    public class TestClass
    {
        public void Print()
        {
            Debug.Print("Hello from " + this.ToString());
        }
    }
}
