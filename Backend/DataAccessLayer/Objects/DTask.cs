using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /*
    The main DTask class
    Contains all methods for performing DTask's actions.
    */
    /// <summary>
    /// The main DTask class.
    /// Exist to Insert Task data to the database and maintain separation between layers 
    /// </summary>
    /// <remarks>
    /// This class inherit methods from abstract class DTO 
    /// </remarks>
    //internal 
    public class DTask : DTO
    {
        public const string BoardIdColumnName = "BoardId";
        public const string TaskOrdinalColumnName = "ColumnOrdinal";
        public const string TaskAssigneeColumnName = "Assignee";
        public const string TaskTitleColumnName = "TaskTitle";
        public const string TaskDescriptionColumnName = "TaskDescription";
        public const string TaskCreationTimeColumnName = "TaskCreationTime";
        public const string TaskDueDateColumnName = "TaskDueDate";

        private int _boardId;
        /// <summary>
        /// Gets the DTask boardId
        /// </summary>
        public int BoardId { get => _boardId; }

        private int _columnOrdinal;
        public int ColumnOrdinal { get => _columnOrdinal; set { _columnOrdinal = value; ((DalTaskController)_controller).Update(Id, BoardId, TaskOrdinalColumnName, value); } }
        
        private string _assignee;
        public string Assignee { get => _assignee; set { _assignee = value; ((DalTaskController)_controller).Update(Id, TaskAssigneeColumnName, value); } }
        
        private string _taskTitle;
        public string TaskTitle { get => _taskTitle; set { _taskTitle = value; ((DalTaskController)_controller).Update(Id, TaskTitleColumnName, value); } }
        
        private string _taskDescription;
        public string TaskDescription { get => _taskDescription; set { _taskDescription = value; ((DalTaskController)_controller).Update(Id, TaskDescriptionColumnName, value); } }
        
        private DateTime _taskCreationTime;
        public DateTime TaskCreationTime { get => _taskCreationTime; }
        
        private DateTime _taskDueDate;
        public DateTime TaskDueDate { get => _taskDueDate; set { _taskDueDate = value; ((DalTaskController)_controller).Update(Id, TaskDueDateColumnName, value.ToString()); } }
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ///<summary>the constructor of DTask.</summary>
        public DTask(int TaskId, int BoardId, int ColumnOrdinal, string Assignee, string TaskTitle, string TaskDescription, DateTime TaskCreationTime, DateTime TaskDueDate): base(new DalTaskController(), TaskId)
        {
            _boardId = BoardId;
            _columnOrdinal = ColumnOrdinal;
            _assignee = Assignee;
            _taskTitle = TaskTitle;
            _taskDescription = TaskDescription;
            _taskCreationTime = TaskCreationTime;
            _taskDueDate = TaskDueDate;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }


        ///<summary>
        ///Inserts the DTask line to the database
        ///</summary>
        /// <exception cref="System.AggregateException">Thrown when we cant add the Dtask to the Tasks database.</exception>
        ///<returns>returns a boolean value on wether the Insertion worked</returns>
        public bool Insert()
        {
            if (((DalTaskController)_controller).Insert(this))
            {
                log.Info($"Succesfully inserted task to 'Tasks' database. id:'{Id}', which is in board:'{BoardId}'");
                return true;
            }
            throw new AggregateException($"error when try to insert task to 'Tasks' database. id:'{Id}', which is in board:'{BoardId}'");
        }


        ///<summary>
        ///Deletes the DTask line from the database
        ///</summary>
        /// <exception cref="System.AggregateException">Thrown when we cant delete the Dtask from the Tasks database.</exception>
        ///<returns>returns a boolean value on wether the Deletion worked</returns>
        public bool Delete()
        {
            if (((DalTaskController)_controller).Delete(this))
            {
                log.Info($"Succesfully deleted from 'Tasks' database the task with the id:'{Id}', which is in board:'{BoardId}'");
                return true;
            }
            log.Error($"Deletion failed for task with the id:{Id}, which is in board{BoardId}");
            throw new AggregateException($"Deletion failed for task with the id:'{Id}', which is in board:'{BoardId}'");
        }
    }
}
