using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Helpers
{
   public class ConvertDBVal {

    public static T ConvertFromDBVal<T>(object obj)
    {

      if (obj == null || obj == DBNull.Value)
      {
        if (typeof(T) ==typeof(String)) {
         var message = "Returning Empty string";
         Notification.PostMessage(message);
          return (T)(object)String.Empty;
          }
          
        return default(T); // returns the default value for the type
      }

      else
      {
        return (T)obj;
      }
    }
   }
}
