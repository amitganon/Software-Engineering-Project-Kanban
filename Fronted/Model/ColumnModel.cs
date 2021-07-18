using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronted.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        private ObservableCollection<TaskModel> tasks;
        public ObservableCollection<TaskModel> Tasks { get=>tasks; set{ tasks = value;RaisePropertyChanged("Tasks"); } }

        public string ColumnName { get; set; }
        public int TaskLimit { get; set; }
        public int Ordinal { get; set; }

        public TaskModel selectedTask;
        public TaskModel SelectedTask
        {
            get
            {
                return selectedTask;
            }
            set
            {
                selectedTask = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedTask");
            }
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

        ///<summary>
        ///the column model constructor
        ///</summary>
        public ColumnModel(BackendController c , string name, int tasklimit , int ordinal) : base(c)
        {
            this.Controller = c;
            this.Tasks = new ObservableCollection<TaskModel>();
            this.Ordinal = ordinal;
            this.ColumnName = name;
            this.TaskLimit = tasklimit;
        }

        internal List<TaskModel> GetInProgressTasks(string userEmail)
        {
            List<TaskModel> tasksList = new();
            foreach(TaskModel task in Tasks)
            {
                if (task.Assignee == userEmail)
                    tasksList.Add(task);
            }
            return tasksList;
        }

        public string Create_Column(string UserEmail, string boardCreator, string boardName)
        {
            return Controller.Create_Column(UserEmail, boardCreator , boardName, ColumnName ,Ordinal);
        }
        public ColumnModel LoadTasks(IList<IntroSE.Kanban.Backend.ServiceLayer.Task> ColumnOfTasks , string name,int tasklimit,  int ordinal , string userEmail, int boardId)
        {
            ColumnModel MC = new ColumnModel(Controller ,name, tasklimit, ordinal);
            foreach (IntroSE.Kanban.Backend.ServiceLayer.Task t in ColumnOfTasks)
            {
                TaskModel mt = new TaskModel( t , userEmail, Controller, boardId, Ordinal);
                MC.Tasks.Add(mt);
            }
            return MC;


        }

        public string Change_Column_Name(string email , string creator , string boardName, string newName,int ordinal)
        {
            try
            {
                Controller.Change_Column_Name(email, creator, boardName, newName, ordinal);
                ColumnName = newName;
                return newName;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string Change_Column_Limit(string email, string creator, string boardName, int ordinal, int newLimit)
        {
            try
            {
                Controller.Change_Column_Limit(email, creator, boardName, ordinal , newLimit);
                TaskLimit = newLimit;
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public TaskModel Find_Task(ObservableCollection<TaskModel> tasks)
        {
            foreach ( TaskModel TM in tasks)
            {
                if (SelectedTask != null)
                {
                    if (TM.Id == SelectedTask.Id)
                        return SelectedTask;
                }
                else
                    if (TM.TaskPropertySelected != null)
                    {
                    return TM;
                    }
                    
            }
            return null;
        }

    }
}
