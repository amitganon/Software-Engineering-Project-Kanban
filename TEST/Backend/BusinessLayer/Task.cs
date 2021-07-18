using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /*
`   The main Task class
    Contains all methods for performing Task's actions.
    */
    /// <summary>
    /// The main Task class.
    /// Contains all methods for performing Task's functions.
    /// </summary>
    /// <remarks>
    /// This class represnt the task , and allow to update the title , decription and the due date of the specific task
    /// </remarks>

    public class Task //public for testing
    {
        private int id;
        public int Id{ get => id; }
        private readonly DateTime creationTime;
        public DateTime CreationTime { get => creationTime; }
        private string title;
        public string Title { get => title; }
        private string description;
        public string Description { get => description; }
        private DateTime dueDate;
        public DateTime DueDate { get => dueDate; }
        private string assignee;
        public string Assignee { get => assignee; }
        private const int minTitleLength = 0;
        private const int maxTitleLength = 50;
        private const int maxDescriptionLength = 300;
        private DTask dTask;
        public DTask DTask { get => dTask; set { dTask = value; } }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        ///<summary>The constructor of Task.</summary>
        ///<param name="title">The title of the task.<param>
        ///<param name="description">The description of the task.<param>
        ///<param name="dueDate">The dueDate of the task.<param>
        ///<param name="id">The id of the task.<param>
        ///<param name="assignee">The assignee of the task.<param>
        ///<exception cref="System.ArgumentNullException">Thrown when asignee is nullll.</exception>
        ///<exception cref="System.ArgumentException">Thrown when the title inserted is illegall.</exception>
        ///<exception cref="System.ArgumentException">Thrown when the description inserted is illegall.</exception>
        ///<exception cref="System.ArgumentException">Thrown when the duedate inserted is illegall.</exception>
        public Task(string title, string description, DateTime dueDate, int id, String assignee)
        {
            if (assignee == null)
            {
                log.Error("Task Constructor got a null asignee as input");
                throw new ArgumentNullException("Task Constructor got a null asignee as input");
            }
            this.id = id;
            if (ValidTitle(title))
                this.title = title;
            else
            {
                log.Error($"Task Constructor got a non-valid title: '{title}'");
                throw new ArgumentException($"Task Constructor got a non-valid title: '{title}'");
            }
            if (ValidDescription(description))
                this.description = description;
            else
            {
                log.Error($"Task Constructor got a non-valid description: '{description}'");
                throw new ArgumentException($"Task Constructor got a non-valid description: '{description}'");
            }
            DateTime now = DateTime.Now;
            if (dueDate < now)
            {
                log.Error($"Task Constructor got a non-valid due date: '{dueDate}'");
                throw new ArgumentException($"Task Constructor got a non-valid due date: '{dueDate}'");
            }
            this.assignee = assignee;
            this.dueDate = dueDate;
            this.creationTime = DateTime.Now;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info($"Task created. id:'{this.id}', creation time:'{this.creationTime }', Assignee:'{assignee}'");
        }
        public Task(DTask dTask)
        {
            this.title = dTask.TaskTitle;
            this.description = dTask.TaskDescription;
            this.dueDate = dTask.TaskDueDate;
            this.creationTime = dTask.TaskCreationTime;
            this.id = dTask.Id;
            this.assignee = dTask.Assignee;
        }

        /// <summary>
        /// Checks if the title is valid
        /// </summary>
        /// /// <param name="title">The title that being checked</param>
        /// <returns>true if valid , false else</returns>
        private bool ValidTitle(string title)
        {
            if (title != null & title.Length > minTitleLength & title.Length <= maxTitleLength)
            {
                log.Debug($"ValidTitle returned that the title:'{title}' is valid");
                return true;
            }
            log.Debug($"ValidTitle returned that the title:'{title}' is invalid");
            return false;
        }


        /// <summary>
        /// Checks if the description is valid
        /// </summary>
        /// /// <param name="description">The description that being checked</param>
        /// <returns>true if valid , false else</returns>
        private bool ValidDescription(string description)
        {
            if (description != null & description.Length <= maxDescriptionLength)
            {
                log.Debug($"ValidDescription returned that the description:'{description}' is valid");
                return true;
            }
            log.Debug($"ValidDescription returned that the description:'{description}' is invalid");
            return false;
        }


        /// <summary>
        /// Set new title for the task, only if the user is the one assigned to the task
        /// </summary>
        /// <param name="userEmail">the user's email that try to change the title</param>
        /// <param name="newTitle">the new title of the task</param>
        /// <exception cref="System.ArgumentException">Thrown when the user's email that try to change the task is not the assigned user.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the new title is not valid.</exception>
        /// <returns>set the new title , throw Exception if not</returns>
        public void SetTitle(string userEmail, int boardId,int columnOrdinal, string newTitle)
        {
            if (ValidTitle(newTitle))
            {
                if (CheckAssignee(userEmail))
                {
                    log.Debug($"This task title changed to:'{newTitle}', the user assignee is:'{userEmail}'");
                    this.title = newTitle;
                    if (DTask == null)
                    {
                        DTask = new(Id, boardId, columnOrdinal, Assignee, Title, Description, CreationTime, DueDate);
                    }
                    DTask.TaskTitle = newTitle;
                }
                else
                {
                    log.Error($"SetTitle failed because a user that's not assigned to this task tried to commit, userEmail:'{userEmail}'");
                    throw new ArgumentException($"SetTitle failed because a user that's not assigned to this task tried to commit, userEmail:'{userEmail}'");
                }
            }
            else
            {
                log.Error($"SetTitle got a title input which is not valid, the title:'{newTitle}'");
                throw new ArgumentException($"SetTitle got a title input which is not valid, the title:'{newTitle}'");
            }
        }


        /// <summary>
        /// Set new description for the task
        /// </summary>
        /// <param name="userEmail">the user's email that try to change the title</param>
        /// <param name="newDescription">the new description of the task</param>
        /// <exception cref="System.ArgumentException">Thrown when the user's email that chenge the task is not the assignee user to this task.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the new description is not valid.</exception>
        /// <returns>set the new description , throw Exception if not</returns>
        public void SetDescription(string userEmail, string newDescription)
        {
            if (ValidDescription(newDescription))
            {
                if (CheckAssignee(userEmail))
                {
                    log.Debug($"This task description changed to:'{newDescription}', the user assignee is:'{userEmail}'");
                    this.description = newDescription;
                    DTask.TaskDescription = newDescription;
                }
                else
                {
                    log.Error($"SetDescription failed because a user that's not assigned to this task tried to commit, userEmail:'{userEmail}'");
                    throw new ArgumentException($"SetDescription failed because a user that's not assigned to this task tried to commit, userEmail:'{userEmail}'");
                }
            }
            else
            {
                log.Error($"SetDescription got a description input which is not valid, the description:'{newDescription}'");
                throw new ArgumentException($"SetDescription got a description input which is not valid, the description:'{newDescription}'");
            }
        }



        /// <summary>
        /// Set new DueDate for the task, only if the user is the one assigned to the task
        /// </summary>
        /// <param name="userEmail">the user's email that try to change the title</param>
        /// <param name="newDueDate">the new DueDate of the task</param>
        /// <exception cref="System.ArgumentException">Thrown when the new newDueDate is not valid.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the user's email that chenge the task is not the assignee user to this task.</exception>
        /// <returns>set the new DueDate , throw Exception if not</returns>
        public void SetDueDate(string userEmail, DateTime newDueDate)
        {
            DateTime now = DateTime.Now;
            if (newDueDate < now)
            {
                log.Error($"SetDueDate got a non valid due date:'{newDueDate}'");
                throw new ArgumentException($"SetDueDate got a non valid due date:'{newDueDate}'");
            }
            if (CheckAssignee(userEmail))
            {
                log.Info($"This task dueDate changed to-{newDueDate}, the user assignee is-{userEmail}");
                this.dueDate = Convert.ToDateTime(newDueDate);
                DTask.TaskDueDate = newDueDate;
            }
            else
            {
                log.Error($"SetDueDate failed because a user that's not assigned to this task tried to commit, userEmail:'{userEmail}'");
                throw new ArgumentException($"SetDueDate failed because a user that's not assigned to this task tried to commit, userEmail:'{userEmail}'");
            }
        }


        /// <summary>
        /// chenge the user that assignee to this task
        /// </summary>
        /// <param name="userEmail">the user's email that is currently the task's assignee/param>
        /// <param name="newAssigneeEmail">the user's email that will be assignee to this task</param>
        /// <exception cref="System.ArgumentException">Thrown when the user's email executing the function isn't the current assignee</exception>
        public void SetAssignee(string userEmail, string newAssigneeEmail)
        {
            if (!CheckAssignee(userEmail))
            {
                log.Error($"SetAssignee failed because a user that's not assigned to this task tried to commit, userEmail:'{userEmail}'");
                throw new ArgumentException($"SetAssignee failed because a user that's not assigned to this task tried to commit, userEmail:'{userEmail}'");
            }
            this.assignee = newAssigneeEmail;
            DTask.Assignee = newAssigneeEmail;
            log.Info($"Task id- {this.id}, set assignee email to- {newAssigneeEmail}");
        }



        /// <summary>
        /// check if the user that assignee to this task is equals to email in input
        /// </summary>
        /// <param name="assigneeEmail">the user's email that is checked</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the user's email input is null.</exception>
        /// <returns>true/false if input equals to the user thet assignee to this task</returns>
        public bool CheckAssignee(string assigneeEmail)
        {
            if (assigneeEmail == null)
            {
                log.Error("CheckAssignee received a null emailAssignee as input");
                throw new ArgumentNullException("CheckAssignee received a null emailAssignee as input");
            }
            if (assigneeEmail == this.assignee)
            {
                log.Debug($"CheckAssignee returns true to this email assignee:'{assigneeEmail}'");
                return true;
            }
            log.Debug($"CheckAssignee returns false to this email assignee:'{assigneeEmail}'");
            return false;
        }
        public void setColumnAfterMoving(int id, int columnOrdinal, int shiftSize)
        {
            dTask = new DTask(Id, id, columnOrdinal, Assignee, Title, Description, CreationTime, DueDate);
            DTask.ColumnOrdinal = columnOrdinal+ shiftSize;
        }
    }
}
