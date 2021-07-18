using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fronted.Model
{
    public class BackendController
    {
        public Service service { get; set; }
        public BackendController()
        {
            this.service = new Service();
            Response response = service.LoadData();
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        /// <summary>
        /// login user to system
        /// </summary>
        /// <param name="userName">user that login</param>
        /// <param name="password">password given to enter</param>
        /// <returns></returns>
        public UserModel LogIn(string userName, string password)
        {
            Response<User> u = service.Login(userName, password);
            if (u.ErrorOccured)
            {
                throw new Exception(u.ErrorMessage);
            }
            return new UserModel(this, userName);
        }
        /// <summary>
        /// register user to system
        /// </summary>
        /// <param name="userName">user that register</param>
        /// <param name="password">password given to register</param>
        public void Register(string userEmail, string password)
        {
            Response res = service.Register(userEmail, password);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// logout from system
        /// </summary>
        /// <param name="userLogout">user that out from system</param>
        public void Logout(string userLogout)
        {
            Response u = service.Logout(userLogout);
            if (u.ErrorOccured)
            {
                throw new Exception(u.ErrorMessage);
            }

        }
        /// <summary>
        /// remove Borad
        /// </summary>
        /// <param name="userEmail">user thats log in</param>
        /// <param name="Creator">creator of board</param>
        /// <param name="name">board name</param>
        public void RemoveBoard(string userEmail, string Creator, string name)
        {
            Response u = service.RemoveBoard(userEmail, Creator, name);
            if (u.ErrorOccured)
            {
                throw new Exception(u.ErrorMessage);
            }
        }
        /// <summary>
        /// move column right
        /// </summary>
        /// <param name="userEmail">user thats log in</param>
        /// <param name="Creator">creator of board</param>
        /// <param name="name">board name</param>
        /// <param name="currentOrdinal">column move to that ordinal</param>
        public void Move_Column_Right(string userEmail, string Creator, string name, int currentOrdinal)
        {
            Response u = service.MoveColumn(userEmail, Creator, name, currentOrdinal, 1);
            if (u.ErrorOccured)
            {
                throw new Exception(u.ErrorMessage);
            }
        }
        /// <summary>
        /// move column left
        /// </summary>
        /// <param name="userEmail">user thats log in</param>
        /// <param name="Creator">creator of board</param>
        /// <param name="name">board name</param>
        /// <param name="currentOrdinal">column move to that ordinal</param>
        public void Move_Column_Left(string userEmail, string Creator, string name, int currentOrdinal)
        {
            Response u = service.MoveColumn(userEmail, Creator, name, currentOrdinal, -1);
            if (u.ErrorOccured)
            {
                throw new Exception(u.ErrorMessage);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail">user thats log in</param>
        /// <param name="Creator">creator of board</param>
        /// <param name="name">board name</param>
        /// <param name="newColumnName">new column name</param>
        /// <param name="columnOrdinal">column ordinal</param>

        public void Change_Column_Name(string userEmail, string Creator, string name, string newColumnName, int columnOrdinal)
        {
            Response u = service.RenameColumn(userEmail, Creator, name, columnOrdinal, newColumnName);
            if (u.ErrorOccured)
            {
                throw new Exception(u.ErrorMessage);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail">user thats log in</param>
        /// <param name="Creator">creator of board</param>
        /// <param name="name">board name</param>
        /// <param name="limit">new column limit</param>
        /// <param name="columnOrdinal">column ordinal</param>
        public void Change_Column_Limit(string userEmail, string Creator, string name, int columnOrdinal, int limit)
        {
            Response u = service.LimitColumn(userEmail, Creator, name, columnOrdinal, limit);
            if (u.ErrorOccured)
            {
                throw new Exception(u.ErrorMessage);
            }
            
        }
        /// <summary>
        /// delete column
        /// </summary>
        /// <param name="userEmail">user thats log in</param>
        /// <param name="Creator">creator of board</param>
        /// <param name="name">board name</param>
        /// <param name="deletedColumn">ordinal of deleted column</param>
        /// <returns></returns>
        public bool Delete_Column(string userEmail, string Creator, string name, int deletedColumn)
        {
            Response u = service.RemoveColumn(userEmail, Creator, name, deletedColumn);
            if (u.ErrorOccured)
            {
                throw new Exception(u.ErrorMessage);
            }
            return !u.ErrorOccured;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail">user thats log in</param>
        /// <param name="creatorEmail">creator of board</param>
        /// <param name="boardName">board name</param>
        /// <param name="ColumnName">column name</param>
        /// <param name="TaskLimit">column limit</param>
        /// <param name="Ordinal">column ordinal</param>
        /// <returns></returns>
        public string Create_Column(string userEmail, string creatorEmail, string boardName, string ColumnName, int Ordinal)
        {
            Response u = service.AddColumn(userEmail, creatorEmail, boardName, Ordinal, ColumnName);
            try { 
                if (u.ErrorOccured)
                {
                 throw new Exception(u.ErrorMessage);
                }
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        /// <summary>
        /// add task
        /// </summary>
        /// <param name="userEmail">user thats log in</param>
        /// <param name="userCreator">creator of board</param>
        /// <param name="boardName">board name</param>
        /// <param name="title">task title</param>
        /// <param name="description">task description</param>
        /// <param name="dueDate">task due date</param>
        /// <param name="boardId">board ID</param>
        /// <returns></returns>
        public TaskModel AddTask(string userEmail, string userCreator, string boardName, string title, string description, DateTime dueDate, int boardId)
        {
            Response<IntroSE.Kanban.Backend.ServiceLayer.Task> t = service.AddTask(userEmail, userCreator, boardName, title, description, dueDate);
            if (t.ErrorOccured)
            {
                throw new Exception(t.ErrorMessage);
            }
            return new TaskModel(t.Value, userEmail, this, boardId, 0);
        }
        /// <summary>
        /// update task property
        /// </summary>
        /// <param name="userEmail">user thats log in</param>
        /// <param name="userCreator">creator of board</param>
        /// <param name="boardName">board name</param>
        /// <param name="title">update title task</param>
        /// <param name="description">update description task</param>
        /// <param name="dueDate">update dueDate task</param>
        /// <param name="assignee">update assignee task</param>
        /// <param name="columnOrdinal">update columnOrdinal task</param>
        /// <param name="taskId">task id</param>
        /// <param name="boardId">board ID</param>
        /// <param name="task">task from model task</param>
        /// <returns></returns>
        public TaskModel UpdateTask(string userEmail, string userCreator, string boardName, string title, string description, DateTime dueDate, string assignee, int columnOrdinal, int taskId,int boardId, TaskModel task)
        {
            bool error = false;
            Response response;
            response = service.UpdateTaskTitle(userEmail, userCreator, boardName, columnOrdinal, taskId, title);
            error = error | response.ErrorOccured;
            response = service.UpdateTaskDescription(userEmail, userCreator, boardName, columnOrdinal, taskId, description);
            error = error | response.ErrorOccured;
            response = service.UpdateTaskDueDate(userEmail, userCreator, boardName, columnOrdinal, taskId, dueDate);
            error = error | response.ErrorOccured;
            response = service.AssignTask(userEmail, userCreator, boardName, columnOrdinal, taskId, assignee);
            error = error | response.ErrorOccured;
            if (error)
            {
                throw new Exception(response.ErrorMessage);
            }
            task = new TaskModel(task.T, userEmail, this, boardId, columnOrdinal);
            return task;
        }
        /// <summary>
        /// get board names
        /// </summary>
        /// <param name="user">user log in</param>
        /// <returns></returns>
        public List<BoardModel> GetUserBoards(UserModel user)
        {
            Response<List<Board>> response= service.GetUserBoards(user.Email);
            List<BoardModel> userBoards = new();
            if (response.ErrorOccured)
                return userBoards;
            List<Board> tempList = response.Value;  
            foreach(Board board in tempList)
            {
                BoardModel b = new(user, board);
                userBoards.Add(b);
            }
            return userBoards;
        }
        /// <summary>
        /// add board
        /// </summary>
        /// <param name="userEmail">user login</param>
        /// <param name="newBoardName">new board name</param>
        public void AddBoard(string userEmail, string newBoardName)
        {
            Response response = service.AddBoard(userEmail, newBoardName);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail">user thats log in</param>
        /// <param name="creatorEmail">creator of board of jon board</param>
        /// <param name="newBoardName">board to join name</param>
        public void JoinBoard(string userEmail, string newBoardName, string creatorEmail)
        {
            Response response = service.JoinBoard(userEmail, creatorEmail, newBoardName);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        /// <summary>
        /// get board
        /// </summary>
        /// <param name="user">user thats log in</param>
        /// <param name="creatorEmail">creator of board</param>
        /// <param name="boardName">board name</param>
        /// <returns></returns>
        public BoardModel GetBoard(UserModel user, string creatorEmail, string boardName)
        {
            Board tmp = service.GetBoard(user.Email, creatorEmail, boardName).Value;
            BoardModel b = new(user, tmp);
            return b;
        }
        /// <summary>
        /// move task to other column
        /// </summary>
        /// <param name="userEmail">user login</param>
        /// <param name="userCreator">creator of board</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">current task ordinal</param>
        /// <param name="taskId">task id</param>
        /// <returns></returns>
        public string Move_Task(string userEmail, string userCreator, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                Response response = service.AdvanceTask(userEmail, userCreator, boardName, columnOrdinal, taskId);
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }
    }
}
