using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.PorfolioDomain.Core.Helpers
{
    public class Notification
    {
        public static void PostMessage(string message)
        {
            Console.WriteLine(message);
            Debug.WriteLine(message);
        }
    }
}
