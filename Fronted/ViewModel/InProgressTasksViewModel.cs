using Fronted.Model;
using Frontend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Fronted.ViewModel
{
    class InProgressTasksViewModel : NotifiableObject
    {
        UserModel user;
        public string title;
        private TaskModel selectedTask;
        public TaskModel SelectedTask { get => selectedTask; set { selectedTask = value; RaisePropertyChanged("SelectedBoard"); } }

        public string Title { get => title; set { title = value; RaisePropertyChanged("Title"); } }
        public ObservableCollection<TaskModel> InProgressTasks { get; set; }
        ObservableCollection<BoardModel> boardList;

        ///<summary>
        ///the constructor
        ///</summary>
        public InProgressTasksViewModel(UserModel user, ObservableCollection<BoardModel> boardList)
        {
            this.user = user;
            title = "In Progress tasks";
            InProgressTasks = new ObservableCollection<TaskModel>();
            //InProgressTasks = user.Controller.InProgressTasks(user.Email);
            GetInProgressTasks(boardList);
            this.boardList = boardList;
        }

        ///<summary>
        ///get all the in progress tasks of a user
        ///</summary>
        ///<param name="boardList">the boards that the user is member in</param>
        private void GetInProgressTasks(ObservableCollection<BoardModel> boardList)
        {
            foreach(BoardModel board in boardList)
            {
                List<TaskModel>temp=board.GetInProgressTasks(user.Email);
                foreach(TaskModel task in temp)
                {
                    InProgressTasks.Add(task);
                }
            }
        }

        public string UserName
        {
            get => user.Email;
            set
            {
                this.user.Email = value;
                RaisePropertyChanged("UserName");
            }
        }

        ///<summary>
        ///log out from the sistem
        ///</summary>
        ///<param name="userEmail">the user email</param>
        public void LogOut(string userEmail)
        {
            user.Controller.Logout(userEmail);
        }

        ///<summary>
        ///get the board from the list of board the user member in by id
        ///</summary>
        ///<param name="boardId">the DMember.</param>
        /// <returns>return board</returns>
        public BoardModel GetMyBoard(int boardId)
        {
            foreach (BoardModel board in boardList)
                if (board.Id == boardId)
                    return board;
            return null;
        }

        ///<summary>
        ///search and show only tasks that contains the key word
        ///</summary>
        ///<param name="sender">the search word</param>
        public void Search_By_Key_Word(string sender)
        {
            if (SaveDataBoard == null) HardCopyTasks();
            InProgressTasks.Clear();
            if (!sender.Equals(""))
            {
                ObservableCollection<TaskModel> newListTasks = new();
                foreach (TaskModel task in SaveDataBoard)
                {
                    if (task.Title.Contains(sender) || (task.Description.Contains(sender)))
                        InProgressTasks.Add(task);
                }
            }
            else
            {
                ObservableCollection<TaskModel> newListTasks = new();
                foreach (TaskModel task in SaveDataBoard)
                {
                    InProgressTasks.Add(task);
                }
            }
        }

        public ObservableCollection<TaskModel> SaveDataBoard { get; set; }

        ///<summary>
        ///copy all the tasks beafore shows only tasks that contains search word
        ///</summary>
        public void HardCopyTasks()
        {
            SaveDataBoard = new();
            foreach (TaskModel newTask in InProgressTasks)
            {
                SaveDataBoard.Add(newTask);
            }
        }

        public bool DueDateSort { get; set; }

        ///<summary>
        ///sord all the tasks by due date
        ///</summary>
        public void DueDateOrder()
        {
            List<DateTime> allDueDates = new();
            if (InProgressTasks.Count >1)
            {
                foreach (TaskModel t in InProgressTasks)
                {
                    allDueDates.Add(t.DueDate);
                }
                allDueDates.Sort();
                if (DueDateSort)
                {
                    allDueDates.Reverse();
                    DueDateSort = false;
                }
                else DueDateSort = true;
                for (int j = 0; j < allDueDates.Count; j++)
                {
                    for (int i = 0; i < InProgressTasks.Count; i++)
                    {
                        if (InProgressTasks.ElementAt(i).DueDate == allDueDates[j])
                        {
                            InProgressTasks.Add(InProgressTasks.ElementAt(i));
                            InProgressTasks.Remove(InProgressTasks.ElementAt(i));

                        }
                    }
                }
            }
        }
    }
}
