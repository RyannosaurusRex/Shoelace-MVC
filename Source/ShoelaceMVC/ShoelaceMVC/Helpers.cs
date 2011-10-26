using System;
using System.Linq;
using System.Linq.Expressions;
using ShoelaceMVC.Membership;

namespace ShoelaceMVC
{
    public class Helpers
    {
         public static string UserEmail(string userName)
         {
             return CodeFirstSecurity.GetUserEmail(userName);
         }
    }
}