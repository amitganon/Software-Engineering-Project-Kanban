using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Fronted.Model
{
    public class TaskModel : NotifiableModelObject
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                this._title = value;
                RaisePropertyChanged("Title");
            }
        }

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                this._id = value;
            }
        }
        private string _assignee;
        public string Assignee
        {
            get => _assignee;
            set
            {
                this._assignee = value;
                RaisePropertyChanged("Assignee");
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                this._description = value;
                RaisePropertyChanged("Description");
            }
        }
        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                this._dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }
        private DateTime _creationTime;
        public DateTime CreationTime
        {
            get => _creationTime;
            set
            {
                this._creationTime = value;
            }
        }
        private string _ordinal;
        public string Ordinal
        {
            get => _ordinal;
            set
            {
                this._ordinal = value;
                RaisePropertyChanged("Ordinal");

            }
        }

        private IntroSE.Kanban.Backend.ServiceLayer.Task t;
        public IntroSE.Kanban.Backend.ServiceLayer.Task T {get;set;}

        private SolidColorBrush _backgroungColor;
        public SolidColorBrush BackgroungColor
        {
            get => _backgroungColor;
            set
            {
                this._backgroungColor = value;
            }
        }
        private SolidColorBrush _borderColor;
        public SolidColorBrush BorderColor
        {
            get => _borderColor;
            set
            {
                this._borderColor = value;
            }
        }

        public int BoardId { get; set; }
        public int ColumnOrdinal { get; set; }
        
        public ObservableCollection<object> TheTasks { get; set; }


        public object taskPropertySelected;
        public object TaskPropertySelected
        {
            get
            {
                return taskPropertySelected;
            }
            set
            {
                taskPropertySelected = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedPropertyTask");
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
        ///the task model constructor
        ///</summary>
        public TaskModel(IntroSE.Kanban.Backend.ServiceLayer.Task t , string userEmail, BackendController c,int boardId, int columnOrdinal) : base(c)
        {
            Title = t.Title;
            Description = t.Description;
            DueDate = t.DueDate;
            CreationTime = t.CreationTime;
            Assignee = t.EmailAssignee;
            Id = t.Id;
            TheTasks = new();
            TheTasks.Add(Id); TheTasks.Add(CreationTime); TheTasks.Add(Title); TheTasks.Add(Description); TheTasks.Add(DueDate); TheTasks.Add(Assignee);
            coloring(userEmail);
            this.BoardId = boardId;
            this.ColumnOrdinal = columnOrdinal;
            T = t;

        }

        ///<summary>
        ///colord a task by it due date.
        ///</summary>
        ///<param name="userEmail">the user email.</param>
        public void coloring(string userEmail)
        {
            //check if task is overdue
            if (DateTime.Now > DueDate)
               BackgroungColor = new SolidColorBrush(Colors.Red);
            else if ((DueDate - CreationTime).TotalDays * 3 / 4 > (DueDate - DateTime.Now).TotalDays)
                BackgroungColor = new SolidColorBrush(Colors.Orange);
            else
                BackgroungColor = new SolidColorBrush(Colors.White);

            if (Assignee.Equals(userEmail))
                BorderColor = new SolidColorBrush(Colors.Blue);
            else
                BorderColor = new SolidColorBrush(Colors.White);
        }
    }
}
