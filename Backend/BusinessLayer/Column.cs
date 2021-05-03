using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        private int tasksLimit;
        private List<Task> tasksList;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        ///<summary>the constructor of Column.</summary>
        public Column()
        {
            tasksList = new List<Task>();
            tasksLimit = -1;
        }


        ///<summary>
        ///This method is a get/set of the TasksLimit.
        ///</summary>
        ///<param name="value">the limit of tasks that can be in the column.</param>
        /// <exception cref="System.AggregateException">Thrown when value is lower then 1 and not equals to -1.</exception>
        ///<returns>return the current limit of task that can be in the column</returns>
        public int TasksLimit
        {
            set
            {
                if (value < 1 & value != -1)
                {
                    log.Error($"TasksLimit not in range, limit- {value}");
                    throw new AggregateException("cant Limit less then 1 tasks");
                }
                if (value < tasksList.Count() & value != -1)
                {
                    log.Error($"TasksLimit can't change to lower then the correct number of tasks, limit- {value}");
                    throw new AggregateException("TasksLimit can't change to lower then the correct number of tasks");
                }
                log.Info($"TasksLimit change to- {value}");
                tasksLimit = value;
            }
            get { log.Info($"the system use TasksLimit(get), the limit is- {tasksLimit}"); return tasksLimit; }
        }


        ///<summary>
        ///This method is a get of the tasksList.
        ///</summary>
        ///<returns>return the current tasksList in the column</returns>
        public List<Task> TaskList
        {
            get { log.Info($"the system use TasksList(get)"); return tasksList; }
        }


        ///<summary>
        ///add a task to the column.
        ///</summary>
        ///<param name="newTask">the tasks that will add to the column.</param>
        /// <exception cref="System.AggregateException">Thrown when newTask is null.</exception>
        /// <exception cref="System.AggregateException">Thrown when newTask is alredy in the column.</exception>
        /// <exception cref="System.AggregateException">Thrown when we cant add the task becouse of the limit.</exception>
        public void AddTask(Task newTask)
        {
            if (newTask == null)
            {
                log.Error("addTask input was null");
                throw new ArgumentException("Cant add null");
            }
            if (Contains(newTask.GetId()))
            {
                log.Error($"the task was alredy in this column, task id- {newTask.GetId()}");
                throw new ArgumentException("This task already exists");
            }
            if (tasksLimit <= tasksList.Count & tasksLimit != -1)
            {
                log.Error($"can't add the task because of the column limit, the limit is-{TasksLimit}");
                throw new ArgumentException("Can't add Task more then the limit");
            }
            log.Info($"this task add to the column, task id-{newTask.GetId()}");
            tasksList.Add(newTask);
        }


        ///<summary>
        ///remove a task from the column.
        ///</summary>
        ///<param name="newTask">the tasks that will remove from the column.</param>
        ///<exception cref="System.AggregateException">Thrown when newTask is null.</exception>
        ///<exception cref="System.AggregateException">Thrown when newTask is not in the column.</exception>
        public void RemoveTask(Task newTask)
        {
            if (newTask == null)
            {
                log.Error("addTask input was null");
                throw new ArgumentException("Cant remove null");
            }
            if (!Contains(newTask.id))
            {
                throw new ArgumentException("this column not contains this task");
            }
            log.Info($"this task removedfrom the column, task id-{newTask.GetId()}");
            tasksList.Remove(newTask);
        }


        ///<summary>
        ///check if the task exists in the column.
        ///</summary>
        ///<param name="taskId">the tasks id.</param>
        ///<returns>return if the task exists in the column</returns>
        public bool Contains(int taskId)
        {
            foreach (Task task in tasksList)
            {
                if (task.GetId() == taskId)
                {
                    log.Debug($"Contains returns true, task id- {taskId}");
                    return true;
                }
            }
            log.Debug($"Contains returns false, task id- {taskId}");
            return false;
        }


        /// <summary>get the task by is id.
        /// </summary>
        ///<param name="taskId">the tasks id.</param>
        ///<exception cref="System.AggregateException">Thrown when newTask is not in the column.</exception>
        ///<returns>return the task by is id</returns>
        public Task GetTask(int taskId)
        {
            bool contains = false;
            Task task = null;
            foreach (Task tempTask in tasksList)
            {
                if (tempTask.id == taskId)
                {
                    task = tempTask;
                    contains = true;
                    break;
                }
            }
            if (!contains)
            {
                log.Error($"the task not in this column, task id- {taskId}");
                throw new ArgumentException("this column not contains this task");
            }
            log.Debug($"GetTask return the task with this id- {taskId}");
            return task;
        }


        ///<summary>
        ///Update task due date.
        ///</summary>
        ///<param name="userEmail">the email of the user that want to change the description.</param>
        ///<param name="taskId">the tasks id.</param>
        ///<param name="time">the new Due date time of the task.</param>
        ///<exception cref="System.AggregateException">Thrown when userEmail is null.</exception>
        public void UpdateTaskDueDate(string userEmail, int taskId, DateTime time)
        {
            if (userEmail == null)
            {
                log.Error($"the userEmail input was null");
                throw new ArgumentException("userEmail cant be null");
            }
            Task task = GetTask(taskId);
            task.SetDueDate(userEmail, time);
            log.Info($"task id- {taskId}, set due date to- {time}");
        }


        ///<summary>
        ///Update task title.
        ///</summary>
        ///<param name="userEmail">the email of the user that want to change the description.</param>
        ///<param name="taskId">the tasks id.</param>
        ///<param name="title">the new title of the task.</param>
        ///<exception cref="System.AggregateException">Thrown when userEmail is null.</exception>
        ///<exception cref="System.AggregateException">Thrown when title is null.</exception>
        public void UpdateTaskTitle(string userEmail, int taskId, String title)
        {
            if (userEmail == null)
            {
                log.Error($"the userEmail input was null");
                throw new ArgumentException("userEmail cant be null");
            }
            if (title == null)
            {
                log.Error($"the title input was null");
                throw new ArgumentException("title can't be null");
            }
            Task task = GetTask(taskId);
            task.SetTitle(userEmail, title);
            log.Info($"task id- {taskId}, set title to- {title}");
        }


        ///<summary>
        ///Update task description.
        ///</summary>
        ///<param name="userEmail">the email of the user that want to change the description.</param>
        ///<param name="taskId">the tasks id.</param>
        ///<param name="description">the new title of the task.</param>
        ///<exception cref="System.AggregateException">Thrown when userEmail is null.</exception>
        ///<exception cref="System.AggregateException">Thrown when description is null.</exception>
        public void UpdateTaskDescription(string userEmail, int taskId, String description)
        {
            if (userEmail == null)
            {
                log.Error($"the userEmail input was null");
                throw new ArgumentException("userEmail cant be null");
            }
            if (description == null)
            {
                log.Error($"the description input was null");
                throw new ArgumentException("description can't be null");
            }
            Task task = GetTask(taskId);
            task.SetDescription(userEmail, description);
            log.Info($"task id- {taskId}, set description to- {description}");
        }

        ///<summary>
        ///Update task description.
        ///</summary>
        ///<param name="newAssigneeEmail">the new Assignee of the task.</param>
        ///<param name="taskId">the tasks id.</param>
        ///<exception cref="System.AggregateException">Thrown when newAssigneeEmail is null.</exception>
        public void UpdateTaskAssignee(string newAssigneeEmail, int taskId)
        {
            if (newAssigneeEmail == null)
            {
                log.Error($"the newAssigneeEmail input was null");
                throw new ArgumentException("newAssigneeEmail cant be null");
            }
            Task task = GetTask(taskId);
            task.UpdateAssignee(newAssigneeEmail);
        }
    }
}
