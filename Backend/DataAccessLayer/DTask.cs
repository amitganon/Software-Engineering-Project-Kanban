using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DTask
    {
        private int id;
        private readonly DateTime creationTime;
        private string title;
        private string description;
        private DateTime dueDate;
        private string assignee;
        private int ordinalColumn;
        private string boardName;
        private string boardCreator;

        public DTask(int id, string title, string description, DateTime dueDate, string assignee, int ordinalColumn, string boardName, string boardCreator)
        {
            this.id = id;
            this.creationTime = DateTime.Now;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.assignee = assignee;
            this.ordinalColumn = ordinalColumn;
            this.boardName = boardName;
            this.boardCreator = boardCreator;
        }
    }
}
