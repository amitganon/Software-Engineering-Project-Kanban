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
        private readonly string email;
        internal User(string email)
        {
            this.email = email;
        }

        public string Email { get => email; }

    }
}
