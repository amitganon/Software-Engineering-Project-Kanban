using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Column
    {
        private readonly string name;
        private readonly int taskLimit;
        private readonly IList<Task> taskList;

        internal Column(BusinessLayer.Column bColumn)
        {
            name = bColumn.Name;
            taskLimit = bColumn.TasksLimit;
            taskList = new List<Task>();
            foreach (BusinessLayer.Task t in bColumn.TaskList)
                taskList.Add(new Task(t));
        }
        // for tal test
        public Column(string name)
        {
            this.name = name;
            this.taskLimit = 1;
            taskList = new List<Task>();
        }

        public string Name { get => name; }
        public int TaskLimit { get => taskLimit; }
        public IList<Task> TaskList { get => taskList; }

    }
}
