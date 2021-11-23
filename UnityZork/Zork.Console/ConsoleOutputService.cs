using System;
using Zork.Common;

namespace Zork
{
    internal class ConsoleOutputService : IOutputService
    {
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /*public void Write(string value)
        {
            Console.Write(value);
        }*/

        public void Write(object value)
        {
            Console.WriteLine(value.ToString());
        }

        /*
        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
        */

        public void WriteLine(object value)
        {
            Console.WriteLine(value.ToString());
        }
    }
}

