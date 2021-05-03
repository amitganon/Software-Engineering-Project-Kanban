using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DUser
    {
        private string email;
        private string password;
        private int id;
        private int boardTop;

        public DUser(string email, string password , int id , int boardTop)
        {
            this.email = email;
            this.password = password;
            this.id = id;
            this.boardTop = boardTop;
        }

    }
}
