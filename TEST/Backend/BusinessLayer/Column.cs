using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("TestKanban")]

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /*
`   The main Column class
    Contains all methods for performing Column's actions.
    */
    /// <summary>
    /// The main Column class.
    /// Contains all methods for performing Column's functions.
    /// </summary>
    /// <remarks>
    /// This class holds the list of the task assinged to this column , and can return or set its limit and the list itself
    /// Add task to the list of task, remove task and update the fields of a task.
    /// </remarks>


    public class Column //public for testing
    {
        private string name;
        private int tasksLimit;
        private List<Task> tasksList;
        private DColumn dColumn;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        ///<summary>the constructor of Column.</summary>
        public Column(int boardId , int ordinal, string name)
        {
            if (name == null)
            {
                log.Error($"New column name can't be null");
                throw new ArgumentException($"New column name can't be null");
            }
            if (ordinal < 0)
            {
                log.Error($"ordinal need to be above 0");
                throw new ArgumentException($"ordinal need to be above 0");
            }
            if (boardId < 0)
            {
                log.Error($"board id need to be above 0");
                throw new ArgumentException($"board id need to be above 0");
            }
            this.name = name;
            tasksList = new List<Task>();
            tasksLimit = -1;
            dColumn = new DColumn(boardId, ordinal, name, tasksLimit);
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info($"New column created: column name {name} , in ordinal {0}");
        }
        public Column (int boardId, int ordinal, string name,int taskLimit)
        {
            if (name == null)
            {
                ///
                log.Error($"New column name can't be null");
                throw new ArgumentException($"New column name can't be null");
            }
            if (ordinal < 0)
            {
                log.Error($"ordinal need to be above 0");
                throw new ArgumentException($"ordinal need to be above 0");
            }
            if (boardId < 0)
            {
                log.Error($"board id need to be above 0");
                throw new ArgumentException($"board id need to be above 0");
            }
            this.name = name;
            tasksList = new List<Task>();
            this.tasksLimit = taskLimit;
            dColumn = new DColumn(boardId, ordinal, name, tasksLimit);
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info($"New column created: column name {name} , in ordinal {0}");
        }


        ///<summary>
        ///This method is a get/set of the TasksLimit.
        ///</summary>
        ///<param name="value">the limit of tasks that can be in the column.</param>
        /// <exception cref="System.ArgumentException">Thrown when value is lower then 1 and not equals to -1.</exception>
        /// <exception cref="System.ArgumentException">Thrown when value is lower then the task list amount.</exception>
        ///<returns>return the current limit of task that can be in the column</returns>
        public int TasksLimit
        {
            set
            {
                if (value < 1 & value != -1)
                {
                    log.Error($"TasksLimit entered is not in range, limit entered:'{value}'");
                    throw new ArgumentException($"TasksLimit entered is not in range, limit entered:'{value}'");
                }
                if (value < tasksList.Count() & value != -1)
                {
                    log.Error($"TasksLimit can't change to lower than the current number of tasks, limit entered:'{value}'");
                    throw new ArgumentException($"TasksLimit can't change to lower than the current number of tasks, limit entered:'{value}'");
                }
                log.Info($"TasksLimit changed to- {value}");
                tasksLimit = value;
                dColumn.TaskLimit = value;
            }
            get { return tasksLimit; }
        }
        /// <summary>
        /// This method is a get/set of the Name of column.
        /// </summary>
        public string Name {get => name;
            set
            {
                if (value == null)
                {
                    log.Error($"new name can't be null in rename column");
                    throw new ArgumentException($"new name can't be null in rename column");
                }
                this.name = value;
                dColumn.Name = value;
                log.Info($"column name changed to- {value}");
            }
        }

        /// <summary>
        /// This method is a get/set of the DColumn.
        /// </summary>
        public DColumn DColumn { get => dColumn; set { dColumn = value; } }
        ///<summary>
        ///This method is a get of the tasksList.
        ///</summary>
        ///<returns>returns the current tasksList in the column</returns>
        public List<Task> TaskList
        {
            get { log.Info($"the system used TasksList(get)"); return tasksList; }
        }

        /// <summary>
        /// set column ordinal when moving column
        /// </summary>
        /// <param name="ordinal">the new ordinal</param>
        public void SetColumnOrdinal(int ordinal)
        {
            if (ordinal < 0)
            {
                log.Error($"ordinal need to be above 0 when moving column");
                throw new ArgumentException($"ordinal need to be above 0 when moving column");
            }
            dColumn.ColumnOrdinal = ordinal;
        }

        ///<summary>
        ///This method gets all the tasks that the userEmail is assigned to.
        ///</summary>
        /// <param name="userEmail">The email address of the user we want to get his assigned tasks</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the email entered is null.</exception>
        ///<returns>return the current user assigned's tasksList in the column</returns>
        public List<Task> GetAssignedTaskList(string userEmail)
        {
            if (userEmail == null)
            {
                log.Error("GetTaskList got a null userEmail as input");
                throw new ArgumentNullException("GetTaskList got a null userEmail as input");
            }
            List<Task> myList = new();
            foreach (Task task in TaskList)
            {
                if (task.CheckAssignee(userEmail))
                    myList.Add(task);
            }
            log.Debug($"Column used GetTasksList for the user: {userEmail}");
            return myList;
        }

        ///<summary>
        ///adds a task to the column.
        ///</summary>
        ///<param name="newTask">the tasks that will add to the column.</param>
        ///<param name="boardId">the board id referenced to task.</param>
        ///<param name="columnOrdinal">an integer that represent the column the task belongs to.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when newTask is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when newTask is alredy in the column.</exception>
        /// <exception cref="System.ArgumentException">Thrown when we cant add the task becouse of the limit.</exception>
        /// <exception cref="System.ArgumentException">Thrown when we cant add the Dtask to the Tasks database.</exception>
        /// <remarks>
        /// This method checks if the column we need to add to is backlog or not
        /// because if not, we just need to update the columnOrdinal field in the task database
        /// </remarks>
        public void AddTask(Task newTask,  int boardId, int columnOrdinal, string Function)
        {
            if (newTask == null)
            {
                log.Error("GetTaskList got a null-newTask as input");
                throw new ArgumentNullException("GetTaskList got a null-newTask as input");
            }
            if (Contains(newTask.Id))
            {
                log.Error($"the task is alredy in this column, task id:'{newTask.Id}'");
                throw new ArgumentException($"the task is alredy in this column, task id:'{newTask.Id}'");
            }
            if (tasksLimit <= tasksList.Count & tasksLimit != -1)
            {
                log.Error($"can't add the task because of the column limit, the limit is: '{TasksLimit}'");
                throw new ArgumentException($"can't add the task because of the column limit, the limit is: '{TasksLimit}'");
            }
            if (columnOrdinal == 0 || Function.Equals("AdvanceTask"))
            {
                newTask.DTask = new DTask(newTask.Id, boardId, columnOrdinal, newTask.Assignee, newTask.Title, newTask.Description, newTask.CreationTime, newTask.DueDate);
            }
            else
            {
                log.Error($"can't add the task because new task is added to the leftmost column in ordinal 0 and input is {columnOrdinal} ");
                throw new ArgumentException($"can't add the task because new task is added to the leftmost column  in ordinal 0 and input is {columnOrdinal} ");

            }
            tasksList.Add(newTask);
            newTask.DTask.Insert(); 
            log.Info($"this task added to the column, task id: '{newTask.Id}', column ordinal:'{columnOrdinal}'");
        }


        ///<summary>
        ///remove a task from the column , and delet him from DB
        ///</summary>
        ///<param name="task">the task that will be removed from the column.</param>
        ///<exception cref="System.ArgumentNullException">Thrown when newTask is null.</exception>
        ///<exception cref="System.AggregateException">Thrown when newTask is not in the column.</exception>
        public void RemoveTask(Task task , int boardId , int ordinal)
        {
            if (task == null)
            {
                log.Error("RemoveTask got a null-task as input");
                throw new ArgumentNullException("RemoveTask got a null-task as input");
            }
            if (!Contains(task.Id))
            {
                log.Error("RemoveTask failed becuase this column does not contain this task");
                throw new ArgumentException("RemoveTask failed becuase this column does not contain this task");
            }
            log.Info($"this task was removed from the column, task id:'{task.Id}'");
            tasksList.Remove(task);
            if (task.DTask == null)
            {
                task.DTask = new DTask(task.Id, boardId, ordinal,task.Assignee, task.Title, task.Description, task.CreationTime, task.DueDate);
            }
            task.DTask.Delete();

        }


        ///<summary>
        ///check if the task exists in the column.
        ///</summary>
        ///<param name="taskId">the tasks id.</param>
        ///<returns>return if the task exists in the column</returns>
        private bool Contains(int taskId)
        {
            foreach (Task task in tasksList)
            {
                if (task.Id == taskId)
                {
                    log.Debug($"Contains returned true, task id:'{taskId}'");
                    return true;
                }
            }
            log.Debug($"Contains returned false, task id:'{taskId}'");
            return false;
        }


        /// <summary>get the task by its id.
        /// </summary>
        ///<param name="taskId">the tasks id.</param>
        ///<exception cref="System.ArgumentException">Thrown when newTask is not in the column.</exception>
        ///<returns>return the task by its id</returns>
        public Task GetTask(int taskId)
        {
            bool contains = false;
            Task task = null;
            foreach (Task tempTask in tasksList)
            {
                if (tempTask.Id == taskId)
                {
                    task = tempTask;
                    contains = true;
                    break;
                }
            }
            if (!contains)
            {
                log.Error($"GetTask failed becuase this column does not contain this task. task id:'{taskId}'");
                throw new ArgumentException($"GetTask failed becuase this column does not contain this task. task id:'{taskId}'");
            }
            log.Debug($"GetTask return the task with this id- {taskId}");
            return task;
        }

        /// <summary>
        /// Load all task given the borad the column in and the ordinal of the colum
        /// </summary>
        /// <param name="boardId">the borad id that the column in</param>
        /// <param name="columnOrdinal">the ordinal of the column</param>
        public void LoadTasks(int boardId,int columnOrdinal)
        {
            List<DTask> Dtasks = new List<DTask>();
            Dtasks = new DalTaskController().LoadColumnTasks(boardId,columnOrdinal);
            foreach (DTask dTask in Dtasks)
            {
                Task dt = new Task(dTask);
                tasksList.Add(dt);
            }
            log.Info($"all of the tasks in column {columnOrdinal} uploaded from db to the column");
        }

        ///<summary>
        ///This method deletes all the tasks in the columns & delete the specific lines in the database
        ///</summary>
        public void DeleteTasks(int boradID , int ordinal)
        {
            List<Task> newlisttemp = new();
            foreach (Task task in tasksList)
            {
                //task.DTask.Delete();
                newlisttemp.Add(task);
            }
            foreach (Task task in newlisttemp)
            {
                //task.DTask.Delete();
                RemoveTask(task, boradID, ordinal);
                int x = 0;
            }
            log.Info($"All tasks from the column were removed");
        }
    }
}
