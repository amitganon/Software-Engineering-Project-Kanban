using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public interface UserContollerMock
    {
        bool IsValidEmail(string s);
        bool validLogin(string userEmail);
        User GetUser(string userEmail);
        void LoadUsers();
        void Delete();
        User LogIn(string userEmail, string password);
    }
}
