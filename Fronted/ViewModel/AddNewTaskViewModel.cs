using Fronted.Model;
using Frontend;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fronted.ViewModel
{
    class AddNewTaskViewModel : NotifiableObject
    {
        public BackendController controller { get; private set; }
        private string userName;
        private string message;
        private string title;
        public string Title { get => title; set { title = value; RaisePropertyChanged("Title"); } }
        public string taskTitle;
        public string TaskTitle { get => taskTitle; set { taskTitle = value; RaisePropertyChanged("TaskTitle"); } }
        public DateTime dueDate;
        public DateTime DueDate { get => dueDate; set { dueDate = value; RaisePropertyChanged("DueDate"); } }
        public string description;
        public string Description { get => description; set { description = value; RaisePropertyChanged("Description"); } }

        ///<summary>the constructor</summary>
        public AddNewTaskViewModel(UserModel u)
        {
            controller = u.Controller;
            Title = "Add Task";
            UserName =u.Email;
            DueDate = DateTime.Today;
            message = "";
            
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

        ///<summary>
        ///add a task to the  board
        ///</summary>
        /// <param name="userEmail">the user email</param>
        /// <param name="boardName">the board name</param>
        /// <param name="userEmail">the board id</param>
        ///<returns>return the task that added or null if error</returns>
        public TaskModel AddTask(string userEmail, string boardName, int boardId)
        {
            Message = "";
            try
            {
                TaskModel task = controller.AddTask(userName, userEmail, boardName, taskTitle, description, dueDate, boardId);
                MessageBox.Show("", "Task added Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
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
