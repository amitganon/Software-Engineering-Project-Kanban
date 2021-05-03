using System;
using System.Collections.Generic;
using log4net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct User //public for testing
    {
        public readonly string Email;
        internal User(string email)
        {
            this.Email = email;
        }
    }
}
