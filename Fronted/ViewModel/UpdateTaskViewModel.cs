using Fronted.Model;
using Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fronted.ViewModel
{
    class UpdateTaskViewModel : NotifiableObject
    {
        public BackendController controller { get; private set; }
        private string userName;
        private string message;
        private string title;
        public string Title { get => title; set { title = value; RaisePropertyChanged("Title"); } }

        private string taskTitle;
        public string TaskTitle { get => taskTitle; set { taskTitle = value; RaisePropertyChanged("TaskTitle"); } }

        private DateTime dueDate;
        public DateTime DueDate { get => dueDate; set { dueDate = value; RaisePropertyChanged("DueDate"); } }

        private string description;
        public string Description { get => description; set { description = value; RaisePropertyChanged("Description"); } }

        private string assignee;
        public string Assignee { get => assignee; set { assignee = value; RaisePropertyChanged("Assignee"); } }

        private TaskModel task;
        private int taskId;

        ///<summary>
        ///the constructor
        ///</summary>
        public UpdateTaskViewModel(UserModel u,TaskModel task)
        {
            controller = u.Controller;
            Title = "Update Task";
            UserName = u.Email;
            this.task = task;
            taskTitle = task.Title;
            dueDate = task.DueDate;
            description = task.Description;
            assignee = task.Assignee;
            taskId = task.Id;
            userName = u.Email;
         }

        public string UserName
        {
            get => userName;
            set
            {
                this.userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        public string Message
        {
            get => message;
            set
            {
                this.message = value;
                RaisePropertyChanged("Message");
            }
        }

        /// <summary>
        /// update task
        /// </summary>
        /// <param name="boarCreatorEmail">the board creator email</param>
        /// <param name="boardName">the board name enail</param>
        /// <param name="columnOrdinal">the column ordinal that the task came from</param>
        /// <param name="boardId">the board id</param>
        /// <returns>the task that updated or null if error</returns>
        public TaskModel UpdateTask(string boarCreatorEmail, string boardName,int columnOrdinal, int boardId)
        {
            Message = "";
            try
            {
                TaskModel task = controller.UpdateTask(UserName, boarCreatorEmail, boardName, TaskTitle, Description, DueDate, Assignee, columnOrdinal, taskId, boardId, this.task);
                MessageBox.Show("", "Task changed Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                return task;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

        ///<summary>
        ///log out from the sistem
        ///</summary>
        ///<param name="userEmail">the user email</param>
        public void LogOut(string userEmail)
        {
            controller.Logout(userEmail);
        }
    }
}
