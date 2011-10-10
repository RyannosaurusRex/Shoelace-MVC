using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ShoelaceMVC.Membership
{
    public class CodeFirstContextInit : DropCreateDatabaseAlways<ShoelaceContext>
    {

        protected override void Seed(ShoelaceContext context)
        {

            CodeFirstSecurity.CreateAccount("Demo", "Demo", "demo@demo.com");

        }

    }
}