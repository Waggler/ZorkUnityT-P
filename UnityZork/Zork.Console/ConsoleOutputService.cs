using System;
using Zork.Common;

namespace Zork
{
    internal class ConsoleOutputService : IOutputService
    {
        //---------------------//
        public void Clear()
        //---------------------//
        {
            throw new NotImplementedException();

        }//END Clear

        //---------------------//
        public void Write(object value)
        //---------------------//
        {
            Console.Write(value.ToString());

        }//END Write

        //---------------------//
        public void WriteLine(object value)
        //---------------------//
        {
            Console.WriteLine(value.ToString());

        }//END WriteLine

    }//END ConsoleOutputService
}

