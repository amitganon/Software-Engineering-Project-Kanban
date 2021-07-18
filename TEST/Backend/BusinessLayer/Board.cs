using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net.Config;
using System.IO;

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
        public int Id { get => id; }
        private string name;
        public string Name { get => name; }
        private string creatorEmail;
        public string CreatorEmail { get => creatorEmail; }
        private int topTask;
        public int TopTask { set { topTask = value; DBoard.TaskTop = value; } }
        private DBoard Dboard;
        public DBoard DBoard { get => Dboard; }
        private Dictionary<int, Column> columnDict;
        public Dictionary<int, Column> ColumnDict { get => columnDict; }
        private List<string> membersList;
        public List<string> MembersList { get => membersList; }
        private const int minimumCoulumns = 2;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        ///<summary>the constructor of Board.</summary>
        ///<param name="name">the name of the board<param>
        ///<param name="id">the id of the board<param>
        ///<param name="creatorEmail">The user's email to which the board belongs<param>
        ///<exception cref="System.ArgumentException">Thrown when name is ilegall.</exception>
        ///<exception cref="System.ArgumentException">Thrown when the email of the user is not legall.</exception>
        public Board(string name, int id, string creatorEmail)
        {
            if (name == null || name.Length == 0)
            {
                log.Error($"Not possible to enter null or empty name as name of a board");
                throw new ArgumentException("Board constructor got an empty name or null board name");
            }
            else
                this.name = name;
            if (creatorEmail == null || creatorEmail.Length == 0)
            {
                log.Error($"Not possible to enter an empty creatorEmail or null as creator's Email of a board");
                throw new ArgumentException("Board constructor got an empty or null creator email");
            }
            else
                this.creatorEmail = creatorEmail;
            this.id = id;
            //build 3 coulumns in default
            columnDict = new Dictionary<int, Column>();
            AddColumn("backlog",0);
            AddColumn("in progress",1);
            AddColumn("done",2);
            this.topTask = 0;
            membersList = new List<string>();
            Dboard = new DBoard(id, name, creatorEmail, topTask);
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info($"A new board has created for user: {this.creatorEmail}. The board name is '{this.name}' and its id is {this.id}");
        }
        public Board (string name, int id, string creatorEmail,int newTaskTop)
        {
            this.name = name;
            this.creatorEmail = creatorEmail;
            this.id = id;
            this.topTask = newTaskTop;
            membersList = new List<string>();
            Dboard = new DBoard(id, name, creatorEmail, newTaskTop);
            this.columnDict = new();

        }


        /// <summary>
        /// add the user email to the boards list
        /// </summary>
        /// <exception cref = "System.ArgumentNullException" > Thrown when user email is null.</exception>
        /// <exception cref = "System.ArgumentException" > Thrown when user alredy member in this board.</exception>
        public void AddUserToList(string userEmail)
        {
            if (userEmail == null)
            {
                log.Error($"user email is null");
                throw new ArgumentNullException("the creator's email can't be null");
            }
            if (membersList.Contains(userEmail))
            {
                log.Error($"user alredy member in this board, userEmail- {userEmail}, board Id- {id}");
                throw new ArgumentException($"user alredy member in this board, userEmail- {userEmail}, board Id- {id}");
            }
            log.Debug($"the user {userEmail} added to userList, in this board: board id-{id}");
            membersList.Add(userEmail);
        }


        /// <summary>
        /// Returns a column given it's ordinal
        /// </summary>
        /// <param name="memberEmail">The member email</param>
        /// <param name="ColumnOrdinal">The column ordinal</param>
        /// <exception cref="System.ArgumentException">Thrown when column not in board.</exception>
        /// <returns>Return the specific column</returns>
        public Column GetColumn(string memberEmail, int columnOrdinal)
        {
            CheckOridanl(columnOrdinal, "GetColumn");
            ValidMember(memberEmail, "GetColumn");
            log.Info($"GetColumn return the column-{columnOrdinal}");
            if (!columnDict.ContainsKey(columnOrdinal))
            {
                log.Error($"column not in board, column ordinal- {columnOrdinal}, board Id- {id}");
                throw new ArgumentException($"column not in board, column ordinal- {columnOrdinal}, board Id- {id}");
            }
            return columnDict.GetValueOrDefault(columnOrdinal);
        }


        /// <summary>
        /// add a new column to board
        /// </summary>
        /// <param name="boardCreator">The board creator email</param>
        /// <param name="columnName">The column name</param>
        /// <param name="ColumnOrdinal">The column ordinal</param>
        /// <exception cref="System.ArgumentException">Thrown when column alredy in board.</exception>
        public void AddColumn(string columnName, int columnOrdinal)
        {
            if (columnOrdinal != columnDict.Count+1)
                CheckOridanl(columnOrdinal, "AddColumn");

            Dictionary<int, Column> newColumnDict = new();
            int columnDictCount = columnDict.Count;
            for (int i = columnOrdinal; i < columnDictCount; i++)//advance all the columns forword
            {
                newColumnDict.Add(i, columnDict.GetValueOrDefault(i));
                columnDict.Remove(i);
            }
            Column c = new Column(id, columnOrdinal, columnName);
            columnDict.Add(columnOrdinal, c);
            for (int i = columnOrdinal+1; i < columnDictCount+1; i++)//advance all the columns forword
            {
                columnDict.Add(i, newColumnDict.GetValueOrDefault(i-1));
                columnDict.GetValueOrDefault(i).SetColumnOrdinal(i);

            }
            c.DColumn.Insert();
            log.Debug($"columnDict added column, ordinal-{columnOrdinal}");
            log.Info($"a new column was added to board, board id-{id}, column ordinal-{columnOrdinal}");
        }


        /// <summary>
        /// remove column from board
        /// </summary>
        /// <param name="boardCreator">The board creator email</param>
        /// <param name="ColumnOrdinal">The column ordinal</param>
        /// <exception cref="System.ArgumentException">Thrown when column not in the board.</exception>
        /// <exception cref="System.ArgumentException">Thrown when board has only 2 column and can't remove more columns.</exception>
        public void RemoveColumn(int columnOrdinal)
        {
            CheckOridanl(columnOrdinal, "RemoveColumn");
            if (!columnDict.ContainsKey(columnOrdinal))
            {
                log.Error($"column not in board, column ordinal- {columnOrdinal}, board Id- {id}");
                throw new ArgumentException($"column not in board, column ordinal- {columnOrdinal}, board Id- {id}");
            }
            if (columnDict.Count == minimumCoulumns)
            {
                log.Error($"cant remove column from a board with 2 columns, board id{id}");
                throw new ArgumentException($"cant remove column from a board with 2 columns, board id{id}");
            }
            Column c = ColumnDict.GetValueOrDefault(columnOrdinal);
            Column prevOrNext = new Column(id,0,"");
            if(columnOrdinal < columnDict.Count - 1)//move all the tasks to the next or prev column
            {
                prevOrNext = ColumnDict.GetValueOrDefault(columnOrdinal+1);
                if (prevOrNext.TaskList.Count + c.TaskList.Count > prevOrNext.TasksLimit & prevOrNext.TasksLimit!=-1)
                {
                    log.Error($"can't remove this column (ordinal- {columnOrdinal}), the tasks cant be added to the next column");
                    throw new ArgumentException($"can't remove this column (ordinal- {columnOrdinal}), the tasks cant be added to the next column");
                }
                foreach (Task t in c.TaskList)
                {
                    prevOrNext.AddTask(t, id, columnOrdinal + 1,"prevOrNext");
                }
            }
            else
            {
                prevOrNext = ColumnDict.GetValueOrDefault(columnOrdinal -1);
                if (prevOrNext.TaskList.Count + c.TaskList.Count > prevOrNext.TasksLimit & prevOrNext.TasksLimit != -1)
                {
                    log.Error($"can't remove this column (ordinal- {columnOrdinal}), the tasks cant be added to the previous column");
                    throw new ArgumentException($"can't remove this column (ordinal- {columnOrdinal}), the tasks cant be added to the previous column");
                }
                foreach (Task t in c.TaskList)
                {
                    prevOrNext.AddTask(t, id, columnOrdinal - 1,"prevOrNext");
                }
            }
            c.DColumn.Delete();//delete the column from db and dict
            columnDict.Remove(columnOrdinal);            
            for (int i = columnOrdinal; i < columnDict.Count; i++)//reduce all column's ordinals
            {
                columnDict.Add(i, columnDict.GetValueOrDefault(i+1));
                columnDict.GetValueOrDefault(i + 1).SetColumnOrdinal(i);
                columnDict.Remove(i+1);
            }
            log.Debug($"the column removed from columnDict, column ordinal-{columnOrdinal}");
            log.Info($"the column removed fron the board, board id-{id}, column ordinal-{columnOrdinal}");

        }


        /// <summary>
        /// move column to a new ordinal
        /// </summary>
        /// <param name="boardCreator">The board creator email</param>
        /// <param name="ColumnOrdinal">The column ordinal</param>
        /// <param name="shiftSize">The shift size</param>
        /// <exception cref="System.ArgumentException">Thrown when try to shift column out of board's boundary .</exception>
        /// <exception cref="System.ArgumentException">Thrown when column not in the board.</exception>
        public void MoveColumn(int columnOrdinal, int shiftSize)
        {
            CheckOridanl(columnOrdinal, "MoveColumn");
            try
            {
                CheckOridanl(columnOrdinal + shiftSize, "MoveColumn");
            }
            catch
            {
                log.Error($"cant shift column out of board's boundary , board id- {id}, columnOrdinal- {columnOrdinal}, shiftSize- {shiftSize}");
                throw new ArgumentException($"cant shift column out of board's boundary , board id- {id}, columnOrdinal- {columnOrdinal}, shiftSize- {shiftSize}");
            }
            if (!columnDict.ContainsKey(columnOrdinal))
            {
                log.Error($"column not in board, column ordinal- {columnOrdinal}, board Id- {id}");
                throw new ArgumentException($"column not in board, column ordinal- {columnOrdinal}, board Id- {id}");
            }
            Column c = columnDict.GetValueOrDefault(columnOrdinal);
            int currentOridnal = columnOrdinal;
            if (shiftSize >= 0)
            {
               // for (int i= shiftSize + columnOrdinal; i>= columnOrdinal; i--)
               // {
                //    Column temp = columnDict.GetValueOrDefault(i);
                //    columnDict.Remove(i);
                //    columnDict.Add(i, c);
                //    c.SetColumnOrdinal(i);
                //    c = temp;
                //}

                //while (shiftSize != 0)
                while (shiftSize != 0)
                {
                    Column temp = columnDict.GetValueOrDefault(currentOridnal + 1);
                    columnDict.Remove(currentOridnal);
                    columnDict.Remove(currentOridnal+1);
                    c.DColumn.Delete();
                    temp.DColumn.Delete();
                    c.DColumn.ColumnOrdinal = currentOridnal + 1;
                    temp.DColumn.ColumnOrdinal = currentOridnal;
                    c.DColumn.Insert();
                    temp.DColumn.Insert();
                    columnDict.Add(currentOridnal, temp);
                    columnDict.Add(currentOridnal+1, c);
                    currentOridnal++;
                    shiftSize--;
                }

            }
            else
            {
                for (int i = shiftSize + columnOrdinal; i <= columnOrdinal; i++)
                {
                    Column temp = columnDict.GetValueOrDefault(i);
                    columnDict.Remove(i);
                    columnDict.Add(i, c);
                    c.SetColumnOrdinal(i);
                    c = temp;
                }
            }
            log.Info($"MoveColumn");
        }


        /// <summary>
        /// Add new task to the board
        /// </summary>
        /// <param name="userEmail">the userEmail that added a task</param>
        /// <param name="title">the title of the new task</param>
        /// <param name="description">the description of the new task</param>
        /// <param name="dueDate">the dueDate of the new task</param>
        /// <returns>Return the new Task</returns>
        public Task AddTask(string userEmail, string title, string description, DateTime dueDate)
        {
            ValidMember(userEmail, "AddTask");
            int taskId = topTask;
            Task newTask = new Task(title, description, dueDate, taskId, userEmail);
            newTask.DTask = new DTask(taskId, this.Id, 0, userEmail, title, description, DateTime.Now, dueDate);
            columnDict.GetValueOrDefault(0).AddTask(newTask, id, 0,"AddTask");
            topTask = topTask + 1;
            log.Error($"the task added to the board, board -{id}");
            return newTask;
        }


        /// <summary>
        /// return all the in progress tasks by user email
        /// </summary>
        public List<Task> inProgressTasks(string userEmail)
        {
            List<Task> tasksList = new List<Task>();
            for(int i = 1; i < columnDict.Count-1; i++)
            {
                List<Task> temp = columnDict.GetValueOrDefault(i).GetAssignedTaskList(userEmail);
                tasksList.AddRange(temp);
            }
            log.Debug($"inProgressTasks in board return tasks list, board id- {id}, user email- {userEmail}");
            return tasksList;
        }


        /// <summary>
        /// Advance task to the next column
        /// </summary>
        /// <param name="userEmail">The user email.</param>
        /// <param name="ColumnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskid">The task to be moved identified task ID</param>
        /// <exception cref="System.ArgumentException">Thrown when task is lower then 0.</exception>
        /// <exception cref="System.ArgumentException">Thrown when user try to advence task from done column.</exception>       
        /// <exception cref="System.ArgumentException">Thrown when user tried to Advance a task in a board which he isn't assigned to the task.</exception>
        public void AdvanceTask(string userEmail, int ColumnOrdinal, int taskid)
        {
            if (taskid < 0)
            {
                log.Error($"AdvanceTask got an illegal taskId input, task id-{taskid}");
                throw new ArgumentException("AdvanceTask got an illegal taskId input");
            }
            CheckOridanl(ColumnOrdinal, "AdvanceTask");
            ValidMember(userEmail, "AdvanceTask");
            Column c = GetColumn(userEmail, ColumnOrdinal);
            Task t = c.GetTask(taskid);            
            if (t.CheckAssignee(userEmail))
            {
                ValidTaskChangeable(ColumnOrdinal, "AdvanceTask");
                if (t.DTask == null)
                {
                    t.DTask = new(t.Id, Id, ColumnOrdinal, t.Assignee, t.Title, t.Description, t.CreationTime, t.DueDate);
                }
                c.RemoveTask(t ,Id, ColumnOrdinal);
                c = columnDict.GetValueOrDefault(ColumnOrdinal + 1);
                c.AddTask(t, id, ColumnOrdinal+1,"AdvanceTask");
            }
            else
            {
                log.Error($"AdvanceTask faild to advance task because the user trying to commit isn't the assigned user. User:{userEmail}, Task:{taskid}, Board:{id}");
                throw new ArgumentException($"AdvanceTask faild to advance task because it was in Done column. Task:{taskid}. Board:{id}");
            }
            log.Info($"the task, id-{taskid}, advance to column- {ColumnOrdinal+1}, in board, id- {id}");
        }


        /// <summary>
        /// check if the given ordianl is valid
        /// </summary>
        /// <param name="ordinal">The column ordinal</param>
        /// <param name="function">The function name that called to this method</param>
        /// <exception cref="System.ArgumentException">Thrown when ordinal is not in columnDict range.</exception>
        private void CheckOridanl(int ordinal,string function)
        {
            if ((ordinal >= columnDict.Count & function!= "AddColumn") | (ordinal > columnDict.Count & function == "AddColumn") | ordinal < 0)
            {
                log.Error($"this column not exists, column ordinal-{ordinal}, when try to commit {function}");
                throw new ArgumentException("this column not exists");
            }
            log.Debug($"Ordinal {ordinal} is a valid column number");
        }


        /// <summary>
        /// valid that columnOrdinal is not Done column
        /// </summary>
        /// <param name="columnOrdianl">The column ordinal</param>
        /// <param name="functionName">The function name that called to this method</param>
        /// <exception cref="System.ArgumentException">Thrown when ordinal is a done column.</exception>
        public void ValidTaskChangeable(int columnOrdianl, string functionName)
        {
            if (columnOrdianl == columnDict.Count - 1)
            {
                log.Error($"can't change task if it in Done column, commit by- {functionName}, column ordinal- {columnOrdianl}, Board id- {id}");
                throw new ArgumentException($"can't change task if it in Done column, commit by- {functionName}, column ordinal- {columnOrdianl}, Board id- {id}");
            }
            CheckOridanl(columnOrdianl, "ValidTaskChangeable");
            log.Debug($"columnOrdianl {columnOrdianl} is not a Done column in this board, board id- {id}");
        }


        /// <summary>
        /// valid that a user is a member in this board
        /// </summary>
        /// <param name="columnOrdianl">The column ordinal</param>
        /// <param name="function">The function name that called to this method</param>
        /// <exception cref="System.ArgumentNullException">Thrown when member's email is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the user is not member in this board.</exception>
        public void ValidMember(string memberEmail, string function)
        {
            if (memberEmail == null)
            {
                log.Error($"member email is null, when try to commit {function}");
                throw new ArgumentNullException($"the member's email can't be null, when try to commit {function}");
            }
            if (!membersList.Contains(memberEmail))
            {
                if (membersList.Count != 0)
                {
                    log.Error($"this user not a member in this board, board id- {id}, when try to commit {function}");
                    throw new ArgumentException($"this user not a member in this board, board id- {id}, when try to commit {function}");
                }

            }
            log.Debug($"validMember was called by -{function}, and return true");
        }


        /// <summary>
        /// load all the columns that realated to this board from db
        /// </summary>
        public void LoadColumns()
        {
            int taskTop = 0;
            DalColumnController dc = new DalColumnController();
            List<DColumn> list = new List<DColumn>();
            list = dc.LoadBoardColumns(id);
            foreach (DColumn Dcolumn in list)
            {
                Column c = new Column(Dcolumn.Id, Dcolumn.ColumnOrdinal, Dcolumn.Name,Dcolumn.TaskLimit);
                
                c.LoadTasks(Id, Dcolumn.ColumnOrdinal);
                columnDict.Add(Dcolumn.ColumnOrdinal, c);
                taskTop += c.TaskList.Count();
            }
            TopTask = taskTop;
            log.Info($"all the columns are insert to the board, board id- {id}");
        }


        /// <summary>
        /// delete all the board columns
        /// </summary>
        public void DeleteColumns()
        {
            int index = 0;
            foreach(Column c in ColumnDict.Values)
            {
                c.DeleteTasks(Id,index);
                c.DColumn.Delete();
                index++;
            }
            columnDict = new Dictionary<int, Column>();
            log.Info($"all board tasks has removed,board id-{id}");
        }
    }
}
