using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace $safeprojectname$.Membership
{
    public class CodeFirstContextInit : DropCreateDatabaseAlways<$safeprojectname$Context>
    {

        protected override void Seed($safeprojectname$Context context)
        {

            CodeFirstSecurity.CreateAccount("Demo", "Demo", "demo@demo.com");

        }

    }
}