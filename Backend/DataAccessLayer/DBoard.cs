using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class DBoard //public for testing
    {
        private int id;
        private string name;
        private string creatorEmail;
        private int taskTop;
        private int userId;


        public DBoard(int id, string name, string creatorEmail, int taskTop, int userId)
        {
            this.id = id;
            this.name = name;
            this.creatorEmail = creatorEmail;
            this.taskTop = taskTop;
            this.userId = userId;
        }

        public int getId {get { return id;}}
        public string getName { get { return name;}}
        public string getCreatorEmail { get { return creatorEmail;}}
        public int getTaskTop { get { return taskTop;}}
        public int getUserId { get { return userId; }}
    }
}
