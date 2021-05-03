using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /*
`   The main Board class
    Contains all methods for performing Board's actions.
    */
    /// <summary>
    /// The main Board class.
    /// Contains all methods for performing Board's functions.
    /// </summary>
    /// <remarks>
    /// This class add new task to a board, move it to other columns in the borad and update the task by title , description and due date
    /// get the borad's id and get the columns and the names of the columns of the borads
    /// </remarks>


    public class Board //public for testing
    {
        private int id;
        private string name;
        public string creatorEmail;
        public Column backlog = new();
        public Column inProgress = new();
        public Column done = new();
        private int topTask;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        ///<summary>the constructor of Board.</summary>
        ///<param name="name">the name of the board<param>
        ///<param name="id">the id of the board<param>
        ///<param name="creatorEmail">The user's email to which the board belongs<param>
        ///<exception cref="System.AggregateException">Thrown when name is ilegall.</exception>
        ///<exception cref="System.AggregateException">Thrown when the email of the user is not legall.</exception>
        public Board(string name, int id, string creatorEmail)
        {
            if (name == null || name.Length <= 0)
            {
                log.Error($"Not possible to enter {name} as name of a task");
                throw new ArgumentException("name not legall");
            }
            else
                this.name = name;
            if (creatorEmail == null || creatorEmail.Length <= 0)
            {
                log.Error($"Not possible to enter {creatorEmail} as creator's Email of a task");
                throw new ArgumentException("creatorEmail not legall");
            }
            else
                this.creatorEmail = creatorEmail;
            this.id = id;
            this.topTask = 0;
            log.Info($"Board  + {this.id}  created: name- {this.name },creatorEmail- {this.creatorEmail }");
        }


        /// <summary>
        /// Get id of borad
        /// </summary>
        /// <returns>The id of borad</returns>
        public int GetId() { return this.id; }

        /// <summary>
        /// Get name of the borad
        /// </summary>
        /// <returns>The name of borad</returns>
        /// 
        public string GetName()
        {
            return this.name;
        }


        /// <summary>
        /// Add new task to the board
        /// </summary>
        /// /// <param name="title">the title of the new task</param>
        /// /// <param name="description">the description of the new task</param>
        /// /// <param name="dueDate">the dueDate of the new task</param>
        /// <returns>Return the new Task, throw Exception else</returns>
        public Task AddTask(string title, string description, DateTime dueDate)
        {
            try
            {
                Task newTask = new Task(title, description, dueDate, topTask, this.creatorEmail);
                backlog.AddTask(newTask);
                topTask++;
                return newTask;
            }
            catch (Exception e)
            {
                log.Error($"Faild to add task in board ");
                throw new ArgumentException(e.Message);
            }
        }


        /// <summary>
        /// Add the task to other column
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be moved identified task ID</param>
        /// <returns>Moves the task, throw Exception else</returns>
        public void AdvanceTask(string userEmail, int ColumnOrdinal, int taskid)
        {
            if (!CheckOridanl(ColumnOrdinal))
                throw new ArgumentException("this column not exists");
            if (taskid < 0)
                throw new ArgumentException("id not valid");
            if (userEmail == null)
                throw new ArgumentException("Creator's email is null");
            Task c = GetColumn(ColumnOrdinal).GetTask(taskid);
            if (c.assignee.Equals(userEmail))
            {
                if (ColumnOrdinal == 0)
                {
                    backlog.RemoveTask(c);
                    inProgress.AddTask(c);
                }
                else if (ColumnOrdinal == 1)
                {
                    inProgress.RemoveTask(c);
                    done.AddTask(c);
                }
                else
                {
                    log.Error($"Faild to move task in board ");
                    throw new ArgumentException("cant advance from column done");
                }
            }
            else
            {
                log.Error($"Faild to move task in board , only Assingee can move Task ");
                throw new ArgumentException("cant advance if not assignee");
            }
        }


        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>set the new title of the task, throw Exception else</returns>
        public void UpdateTaskTitle(string userEmail, int ColumnOrdinal, int taskid, string title)
        {
            if (!CheckOridanl(ColumnOrdinal))
                throw new ArgumentException("this column not exists");
            if (title == null)
                throw new ArgumentException("this new title is null");
            if (userEmail == null)
                throw new ArgumentException("the creator's email can't be null");
            if (ColumnOrdinal == 0)
                backlog.UpdateTaskTitle(userEmail, taskid, title);
            else if (ColumnOrdinal == 1)
                inProgress.UpdateTaskTitle(userEmail, taskid, title);
            else
            {
                log.Error($"Cant update task title in this data {ColumnOrdinal} , {taskid} , {title} beacuse it is in done column");
                throw new ArgumentException("can't update Task in Done");
            }
        }


        /// <summary>
        /// Update task description
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>set the new description of the task, throw Exception else</returns>
        public void UpdateTaskDescription(string userEmail, int ColumnOrdinal, int taskid, string description)
        {
            if (!CheckOridanl(ColumnOrdinal))
                throw new ArgumentException("this column not exists");
            if (description == null)
                throw new ArgumentException("this new description is null");
            if (userEmail == null)
                throw new ArgumentException("the creator's email can't be null");
            if (ColumnOrdinal == 0)
                backlog.UpdateTaskDescription(userEmail, taskid, description);
            else if (ColumnOrdinal == 1)
                inProgress.UpdateTaskDescription(userEmail, taskid, description);
            else
            {
                log.Error($"Cant update task description in this data {ColumnOrdinal} , {taskid} , {description} beacuse it is in done column");
                throw new ArgumentException("Task in not in Done");
            }
        }


        /// <summary>
        /// Update task dueDate
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">New dueDate for the task</param>
        /// <returns>set the new dueDate of the task, throw Exception else</returns>
        public void UpdateTaskDueDate(string userEmail, int ColumnOrdinal, int taskid, DateTime dueDate)
        {
            if (!CheckOridanl(ColumnOrdinal))
                throw new ArgumentException("this column not exists");
            if (userEmail == null)
                throw new ArgumentException("the creator's email can't be null");
            if (ColumnOrdinal == 0)
                backlog.UpdateTaskDueDate(userEmail, taskid, dueDate);
            else if (ColumnOrdinal == 1)
                inProgress.UpdateTaskDueDate(userEmail, taskid, dueDate);
            else
            {
                log.Error($"Cant update task due date in this data {ColumnOrdinal} , {taskid} , {dueDate} beacuse it is in done column");
                throw new ArgumentException("Task in not in Done");
            }
        }


        /// <summary>
        /// Returns a column given it's ordinal
        /// </summary>
        /// <param name="ordinal">The column ordinal</param>
        /// <returns>Return the specific column , throw Exception else </returns>
        public Column GetColumn(int ColumnOrdinal)
        {
            if (!CheckOridanl(ColumnOrdinal))
                throw new ArgumentException("this column not exists");
            if (ColumnOrdinal == 0)
                return backlog;
            else if (ColumnOrdinal == 1)
                return inProgress;
            else return done;
        }


        /// <summary>
        /// Returns the name of a column given it's ordinal
        /// </summary>
        /// <param name="ordinal">The column ordinal</param>
        /// <returns>Return the name of a column , throw Exception else </returns>
        public string GetColumnName(int ColumnOrdinal)
        {
            if (!CheckOridanl(ColumnOrdinal))
                throw new ArgumentException("this column not exists");
            if (ColumnOrdinal == 0)
                return "backlog";
            else if (ColumnOrdinal == 1)
                return "in progress";
            else return "done";
        }


        /// <summary>
        /// check true if the given ordianl is valid
        /// </summary>
        /// <param name="ordinal">The column ordinal</param>
        /// <returns>Return true if the given oridnal between 0-2 , false else </returns>
        private bool CheckOridanl(int ordinal)
        {
            if (ordinal > 2 | ordinal < 0)
            {
                log.Debug($"Ordinal {ordinal} is not valid column number");
                return false;
            }
            else
            {
                log.Debug($"Ordinal {ordinal} is valid column number");
                return true;
            }
        }


        /// <summary>
        /// Update task Assignee
        /// </summary>
        /// <param name="newAssigneeEmail">New new email assignee for the task</param>
        /// <param name="ColumnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskid">The task to be updated identified task ID</param>
        /// <returns>set the new assignee email of the task, throw Exception else</returns>
        public void UpdateAssignee(string newAssigneeEmail, int ColumnOrdinal, int taskid)
        {
            if (!CheckOridanl(ColumnOrdinal))
                throw new ArgumentException("this column not exists");
            if (newAssigneeEmail == null)
                throw new ArgumentException("the creator's email can't be null");
            if (taskid < 0)
                throw new ArgumentException("id not valid");
            if (ColumnOrdinal == 0)
                backlog.UpdateTaskAssignee(newAssigneeEmail, taskid);
            else if (ColumnOrdinal == 1)
                inProgress.UpdateTaskAssignee(newAssigneeEmail, taskid);
            else done.UpdateTaskAssignee(newAssigneeEmail, taskid); ;
        }
    }
}
