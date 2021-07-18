using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Windows;

namespace Fronted.Model
{
    public class BoardModel : NotifiableModelObject
    {
        private UserModel User;
        public int Id { get; set; }
        public ObservableCollection<ColumnModel> Columns { get; set; }
        public ColumnModel ModelColumn { get; set; }
        public string Name { get; set; }
        public string CreatorEmail { get; set; }
        public int NumberOfColumns { get; set; }
        private IList<string> memberList;
        public IList<string> MemberList { get => memberList; set { memberList = value; } }
        private bool SI { get; set; }

        public ColumnModel selectedColumn;
        public ColumnModel SelectedColumn
        {
            get
            {
                return selectedColumn;
            }
            set
            {
                selectedColumn = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedTask");
            }
        }
        /// <summary>
        /// get in progress task from all columns
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        internal List<TaskModel> GetInProgressTasks(string userEmail)
        {
            List<TaskModel> tasksList = new();
            for (int i = 1; i < Columns.Count - 1; i++)
            {
                List<TaskModel> temp = Columns.ElementAtOrDefault<ColumnModel>(i).GetInProgressTasks(userEmail);
                foreach (TaskModel task in temp)
                    tasksList.Add(task);
            }
            return tasksList;
        }

        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }
        public BoardModel(UserModel u, Board b) : base(u.Controller)
        {
            this.SI = true;
            this.User = u;
            this.Name = b.Name;
            this.NumberOfColumns = b.ColumnDict.Count;
            this.CreatorEmail = b.CreatorEmail;
            memberList = b.MembersList;
            this.Columns = new();
            this.ModelColumn = new(Controller, "", 0 , 0) ;
            this.Columns = new();
            this.Columns = LoadColumns();
            this.DueDateSort = false;
            this.Id = b.Id;
        }
        /// <summary>
        /// add column
        /// </summary>
        /// <param name="columnModel"></param>
        public void Add_Column(ColumnModel columnModel)
        {
            Columns.Insert(columnModel.Ordinal, columnModel);
        }
        /// <summary>
        /// load columns from DB
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ColumnModel> LoadColumns()
        {
            ObservableCollection<ColumnModel> ColumnsToLoad = new();
            for (int i = 0; i < NumberOfColumns; i++)
            {
                Response<Column> newColumn = Controller.service.GetColumn(User.Email, CreatorEmail, Name, i, "");
                string columnName = newColumn.Value.Name;
                int takslimit = newColumn.Value.TaskLimit;
                
                IList<IntroSE.Kanban.Backend.ServiceLayer.Task> ColumnOfTasks = newColumn.Value.TaskList;
                ColumnModel cm = new ColumnModel(Controller,columnName, takslimit , i);
                ColumnsToLoad.Add(cm.LoadTasks (ColumnOfTasks, columnName,takslimit , i , User.Email, this.Id));
            }
            return ColumnsToLoad;
        }

        /// <summary>
        /// chnage column name
        /// </summary>
        /// <param name="newName"></param>
        /// <returns></returns>
        public string Change_Column_Name(string newName)
        {
            if (SelectedColumn != null)
            {
                string s = SelectedColumn.Change_Column_Name(User.Email, this.CreatorEmail, this.Name, newName, Columns.IndexOf(SelectedColumn));
                if (s != null)
                {
                    return "";
                }
                else
                    return s;
            }
            else
                return "Need to select Column and than change column name";
        }
        /// <summary>
        /// change column limit
        /// </summary>
        /// <param name="newLimit"></param>
        /// <returns></returns>
        public string Change_Column_Limit(int newLimit)
        {
            if (SelectedColumn != null)
            {
                if (newLimit > 0 && newLimit >= SelectedColumn.Tasks.Count)
                {
                    return SelectedColumn.Change_Column_Limit(User.Email, this.CreatorEmail, this.Name, Columns.IndexOf(SelectedColumn), newLimit);
                }
                else
                {
                    if (SelectedColumn != null)
                        SelectedColumn.TaskLimit = -1;

                    return "error";
                }
            }
            return "Need to select Column and than change Limit";
        }
        /// <summary>
        /// remove board
        /// </summary>
        /// <returns></returns>
        public string RemoveBoard()
        {
            try
            {
                User.Controller.RemoveBoard(User.Email, CreatorEmail, Name);
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }
        /// <summary>
        /// move column right
        /// </summary>
        /// <returns></returns>
        public string Move_Column_Right()
        {
            try
            {
                if (SelectedColumn.Tasks.Count == 0)
                {
                    User.Controller.Move_Column_Right(User.Email, CreatorEmail, Name, Columns.IndexOf(SelectedColumn));
                    if (Columns.Count != Columns.IndexOf(SelectedColumn) + 1)
                        Columns.Move(Columns.IndexOf(SelectedColumn), Columns.IndexOf(SelectedColumn) + 1);
                    return "";
                }
                else
                    return "Only empty columns can be moved.";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        /// <summary>
        /// move column left
        /// </summary>
        /// <returns></returns>
        public string Move_Column_Left()
        {
            try
            {
                User.Controller.Move_Column_Left(User.Email, CreatorEmail, Name, Columns.IndexOf(SelectedColumn));
                if (Columns.IndexOf(SelectedColumn) > 0)
                    Columns.Move(Columns.IndexOf(SelectedColumn), Columns.IndexOf(SelectedColumn) - 1);
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        /// <summary>
        /// delet column
        /// </summary>
        /// <param name="deletedColumn"></param>
        /// <returns></returns>
        public string Delete_Column(int deletedColumn)
        {
            try
            {
                if (User.Controller.Delete_Column(User.Email, CreatorEmail, Name, deletedColumn))
                {
                    if (Columns.IndexOf(SelectedColumn) != 0)
                    {
                        int index = 0;
                        foreach (ColumnModel CM in Columns)
                        {
                            if (index == deletedColumn - 1)
                            {
                                foreach (TaskModel TM in SelectedColumn.Tasks)
                                {
                                    CM.Tasks.Add(TM);
                                }
                            }
                            index++;
                        }

                    }
                    else
                    {
                        foreach (TaskModel TM in SelectedColumn.Tasks)
                        {
                            Columns.First().Tasks.Add(TM);
                        }
                    }
                    Columns.Remove(SelectedColumn);
                }

                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public bool DueDateSort { get; set; }
        /// <summary>
        /// sort the task in specific column by due date
        /// </summary>
        public void DueDateOrder()
        {
            List<DateTime> allDueDates = new();
            if (SelectedColumn != null)
            {
                foreach (TaskModel t in SelectedColumn.Tasks)
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
                    for (int i = 0; i < selectedColumn.Tasks.Count; i++)
                    {
                        if (selectedColumn.Tasks[i].DueDate == allDueDates[j])
                        {
                            SelectedColumn.Tasks.Add(selectedColumn.Tasks[i]);
                            SelectedColumn.Tasks.Remove(selectedColumn.Tasks[i]);

                        }
                    }
                }
            }
        }
        /// <summary>
        /// search and show all tasks inclode the keyword typed in
        /// </summary>
        /// <param name="sender"></param>
        public void Search_By_Key_Word(string sender)
        {
            if (SaveDataBoard == null) HardCopyBoard();
            Columns.Clear();
            if (!sender.Equals(""))
            {
                ObservableCollection<ColumnModel> newBoard = new();
                foreach (ColumnModel ob1 in SaveDataBoard)
                {
                    ColumnModel newCM = new ColumnModel(Controller, ob1.ColumnName, ob1.TaskLimit , ob1.Ordinal);
                    foreach (TaskModel ob2 in ob1.Tasks)
                    {
                        if (ob2.Title.Contains(sender) || (ob2.Description.Contains(sender)))
                            newCM.Tasks.Add(ob2);
                    }
                    if (newCM.Tasks.Count != 0)
                        Columns.Add(newCM);
                }
                int x = 0;
            }
            else
            {
                ObservableCollection<ColumnModel> newBoard = new();
                foreach (ColumnModel ob1 in SaveDataBoard)
                {
                    ColumnModel newCM = new ColumnModel(Controller, ob1.ColumnName, ob1.TaskLimit, ob1.Ordinal);
                    foreach (TaskModel ob2 in ob1.Tasks)
                    {
                        newCM.Tasks.Add(ob2);
                    }
                    Columns.Add(newCM);
                }

            }
        }
        public ObservableCollection<ColumnModel> SaveDataBoard { get; set; }
        /// <summary>
        /// copy list to other
        /// </summary>
        public void HardCopyBoard()
        {
            SaveDataBoard = new();
            foreach (ColumnModel ob1 in Columns)
            {
                ColumnModel newCM = new ColumnModel(Controller, ob1.ColumnName, ob1.TaskLimit, ob1.Ordinal);
                foreach (TaskModel ob2 in ob1.Tasks)
                {
                    newCM.Tasks.Add(ob2);
                }
                SaveDataBoard.Add(newCM);
            }
        }
        /// <summary>
        /// move task
        /// </summary>
        public void Move_Task()
        {
            
            bool newColumn = false;
            TaskModel MovingTask = null;
            int index = -1;
           foreach (ColumnModel CM in  Columns)
            {
                TaskModel TM = CM.Find_Task(CM.Tasks);
                if (newColumn)
                {
                    CM.Tasks.Add(MovingTask);
                    newColumn = false;
                }
                if (TM != null)
                {
                    index = Columns.IndexOf(CM);
                    MovingTask = TM;
                    newColumn = true;
                    CM.Tasks.Remove(TM);
                }
                
            }
            Controller.Move_Task(User.Email, CreatorEmail, Name, index,MovingTask.Id);
        }


    }
}
