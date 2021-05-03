using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;

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
        public int id;
        public readonly DateTime creationTime;
        public string title;
        public string description;
        public DateTime dueDate;
        public string assignee;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        ///<summary>the constructor of Task.</summary>
        public Task(string title, string description, DateTime dueDate, int id, String assignee)
        {
            if (assignee == null)
            {
                log.Error("assignee input was null");
                throw new ArgumentException("assignee input can't be null");
            }
            this.id = id;
            if (ValidTitle(title))
                this.title = title;
            else
            {
                log.Error($"The title {title} is not valid");
                throw new ArgumentException("title not valid");
            }

            if (ValidDescription(description))
                this.title = title;
            else
            {
                log.Error($"The description {description} is not valid");
                throw new ArgumentException("description not valid");
            }
            DateTime now = DateTime.Now;
            if (dueDate < now)
            {
                log.Error($"The due date {dueDate} is not valid");
                throw new ArgumentException("can't set due date that in the past");
            }
            this.assignee = assignee;
            this.dueDate = dueDate;
            this.creationTime = DateTime.Now;
            log.Info($"Task -{this.id}  created: creation time- {this.creationTime },title- {this.title },description- {this.description },dueDate- {this.dueDate },assignee- {assignee}");
        }


        /// <summary>
        /// Get id of Task
        /// </summary>
        /// <returns>The id of Task</returns>
        public int GetId()
        {
            return this.id;
        }


        /// <summary>
        /// Check if the title is valid
        /// </summary>
        /// /// <param name="title">the title to check if valid</param>
        /// <returns>true if valid , false else</returns>
        private bool ValidTitle(string title)
        {
            if (title != null & title.Length > 0 & title.Length < 51)
            {
                log.Debug($"Return true when check {title} is valid");
                return true;
            }
            else
            {
                log.Debug($"Return false when check {title} is valid");
                return false;
            }
        }


        /// <summary>
        /// Check if the description is valid
        /// </summary>
        /// /// <param name="description">the description to check if valid</param>
        /// <returns>true if valid , false else</returns>
        private bool ValidDescription(string description)
        {
            if (description != null & description.Length <= 300)
            {
                log.Debug($"Return true when check {description} is valid");
                return true;
            }
            else
            {
                log.Debug($"Return false when check {description} is valid");
                return false;
            }
        }


        /// <summary>
        /// Set new title for the task
        /// </summary>
        /// <param name="assignee">the user's email that change the title</param>
        /// <param name="newTitle">the new title of the task</param>
        /// <exception cref="System.AggregateException">Thrown when the user's email that chenge the task is not the assignee user to this task.</exception>
        /// <exception cref="System.AggregateException">Thrown when the new title is not valid.</exception>
        /// <returns>set the new title , throw Exception if not</returns>
        public void SetTitle(string assignee, string newTitle)
        {
            if (ValidTitle(newTitle))
            {
                if (CheckAssignee(assignee))
                {
                    log.Debug($"this task title was change to-{newTitle}, the user assignee is-{assignee}");
                    this.title = newTitle;
                }
                else
                {
                    log.Error($"user that not assignee to this tasks try to change the title, user email-{assignee}");
                    throw new ArgumentException("user that not assignee to this tasks can't change the task");
                }
            }
            else
            {
                log.Error($"New title input not valid, the title-{newTitle}");
                throw new ArgumentException("New title input not valid");
            }
        }


        /// <summary>
        /// Set new DueDate for the task
        /// </summary>
        /// <param name="assignee">the user's email that change the dueDate</param>
        /// <param name="inputDate">the new DueDate of the task</param>
        /// <exception cref="System.AggregateException">Thrown when the new dueDate is not valid.</exception>
        /// <exception cref="System.AggregateException">Thrown when the user's email that chenge the task is not the assignee user to this task.</exception>
        /// <returns>set the new DueDate , throw Exception if not</returns>
        public void SetDueDate(string assignee, DateTime inputDate)
        {
            DateTime now = DateTime.Now;
            if (dueDate < now)
            {
                log.Error($"The due date {dueDate} is not valid");
                throw new ArgumentException("can't set due date that in the past");
            }
            if (CheckAssignee(assignee))
            {
                log.Debug($"this task dueDate was change to-{inputDate}, the user assignee is-{assignee}");
                this.dueDate = Convert.ToDateTime(inputDate);
            }
            else
            {
                log.Error($"user that not assignee to this tasks try to change the dueDate, user email-{assignee}");
                throw new ArgumentException("user that not assignee to this tasks can't change the task");
            }
        }


        /// <summary>
        /// Set new description for the task
        /// </summary>
        /// <param name="assignee">the user's email that change the description</param>
        /// <param name="newDescription">the new description of the task</param>
        /// <exception cref="System.AggregateException">Thrown when the user's email that chenge the task is not the assignee user to this task.</exception>
        /// <exception cref="System.AggregateException">Thrown when the new description is not valid.</exception>
        /// <returns>set the new description , throw Exception if not</returns>
        public void SetDescription(string assignee, string newDescription)
        {
            if (ValidDescription(newDescription))
            {
                if (CheckAssignee(assignee))
                {
                    log.Debug($"this task description was change to-{newDescription}, the user assignee is-{assignee}");
                    this.description = newDescription;
                }
                else
                {
                    log.Error($"user that not assignee to this tasks try to change the description, user email-{assignee}");
                    throw new ArgumentException("user that not assignee to this tasks can't change the description");
                }
            }
            else
            {
                log.Error($"description input not valid, the description{newDescription}");
                throw new ArgumentException("New description not valid");
            }
        }

        /// <summary>
        /// check if the user that assignee to this task is equals to email in input
        /// </summary>
        /// <param name="emailAssignee">the user's email that is checked</param>
        /// <exception cref="System.AggregateException">Thrown when the user's email input is null.</exception>
        /// <returns>true/false if input equals to the user thet assignee to this task</returns>
        public bool CheckAssignee(string emailAssignee)
        {
            if (emailAssignee == null)
            {
                log.Error("the input 'assignee' is null");
                throw new ArgumentNullException("emailAssignee was null");
            }
            if (emailAssignee == this.assignee)
            {
                log.Debug($"CheckAssignee returns true to this email assignee-{emailAssignee}");
                return true;
            }
            log.Debug($"CheckAssignee returns false to this email assignee-{emailAssignee}");
            return false;
        }

        /// <summary>
        /// chenge the user that assignee to this task
        /// </summary>
        /// <param name="newAssigneeEmail">the user's email that will be assignee to this task</param>
        /// <exception cref="System.AggregateException">Thrown when the user's email input is null.</exception>
        public void UpdateAssignee(string newAssigneeEmail)
        {
            if (newAssigneeEmail == null)
            {
                log.Error("the input 'newAssigneeEmail' is null");
                throw new ArgumentNullException("emailAssignee was null");
            }
            this.assignee = newAssigneeEmail;
            log.Info($"task id- {this.id}, set assignee email to- {newAssigneeEmail}");
        }
    }
}
