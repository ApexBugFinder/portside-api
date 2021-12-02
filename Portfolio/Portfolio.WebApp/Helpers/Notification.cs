using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Helpers
{
    public class Notification
    {
        private string message = "";
        public static void PostMessage(string message)
        {
            Console.WriteLine(message);
            Debug.WriteLine(message);
        }

    }
}
