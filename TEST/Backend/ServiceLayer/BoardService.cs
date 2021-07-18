using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService //public for testing
    {

    /// <summary>
    /// Loads all the board that in the DataBase
    /// </summary>
    /// <param name="bc">The boardController consisting all boards</param>
    /// <returns>A response object. The response should contain a error message in case of an error</returns>
    public Response LoadBoards(BoardController bc)
        {
            try
            {
                bc.LoadBoards();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        /// <summary>
        /// Deletes All the DataBase
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response DeleteAll(BoardController bc)
        {
            try
            {
                bc.DeleteAllBoards();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Adds a board to the specific user.
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the new board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddBoard(BoardController bc, string userEmail, string boardName)
        {
            try
            {
                bc.AddBoard(userEmail, boardName); //uses validLogin
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }

        }


        /// <summary>
        /// Removes a board to the specific user.
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">The email of the craetor of the board the user want to remove</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveBoard(BoardController bc, string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                bc.RemoveBoard(creatorEmail, boardName, userEmail); //uses validLogin // should check for Creator before removing
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }

        }

        /// <summary>
        /// joins borad to user's borads
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">The email of the craetor of the board the user want to join</param>
        /// <param name="boardName">The name of the board the user wnat to join</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response JoinBoard(BoardController bc, string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                bc.JoinBoard(userEmail, creatorEmail, boardName); //uses validLogin
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Get a list of all borads names of user
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <returns>A response object with a value set of the names of the board, The response should contain a error message in case of an error</returns>
        public Response<IList<String>> GetBoardNames(BoardController bc, string userEmail)
        {
            try
            {
                return Response<IList<String>>.FromValue(bc.GetBoarsdNames(userEmail)); //uses validLogin
            }
            catch (Exception e)
            {
                return Response<IList<String>>.FromError(e.Message);
            }
        }


        /// <summary>
        /// Lists all the inProgress Tasks by userEmail.
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the user.</param>
        /// <returns>A response whith list of tasks. The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> InProgressTasks(BoardController bc, string userEmail)
        {
            try
            {
                IList<Task> myList = new List<Task>();
                IList<BusinessLayer.Task> taskList = bc.InProgressTasks(userEmail); //uses validLogin
                foreach (BusinessLayer.Task task in taskList)
                {
                    Task tmpTask = new Task(task);
                    myList.Add(tmpTask);
                }
                return Response<IList<Task>>.FromValue(myList);
            }
            catch (Exception e)
            {
                return Response<IList<Task>>.FromError(e.Message);
            }
        }


        /// <summary>
        /// Get the limit of a specific column
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit/param>
        /// <returns>The limit of the column.</returns>
        public Response LimitColumn(BoardController bc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                BusinessLayer.Board tempBoard = bc.GetBoard(userEmail, creatorEmail, boardName); //uses validLogin //need to use validMember
                BusinessLayer.Column bColumn = tempBoard.GetColumn(userEmail, columnOrdinal); //NO NEED VALIDS
                bColumn.TasksLimit = limit;
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }

        }

        /// <summary>
        /// Get the limit of a specific column
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The limit of the column.</returns>

        public Response<int> GetColumnLimit(BoardController bc, string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                BusinessLayer.Board tempBoard = bc.GetBoard(userEmail, creatorEmail, boardName); //uses validLogin //need to use validMember
                BusinessLayer.Column bColumn = tempBoard.GetColumn(userEmail, columnOrdinal); //NO NEED VALIDS
                Column c = new (bColumn);
                return Response<int>.FromValue(c.TaskLimit);
            }
            catch (Exception e)
            {
                return Response<int>.FromError(e.Message);
            }
        }


        /// <summary>
        /// Get the name of a specific column
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The name of the column.</returns>
        public Response<string> GetColumnName(BoardController bc, string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                BusinessLayer.Board tempBoard = bc.GetBoard(userEmail, creatorEmail, boardName); //uses validLogin //need to use validMember
                BusinessLayer.Column bColumn = tempBoard.GetColumn(userEmail, columnOrdinal); //NO NEED VALIDS
                Column c = new(bColumn);
                return Response<string>.FromValue(c.Name);
            }
            catch (Exception e)
            {
                return Response<string>.FromError(e.Message);
            }
        }


        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> GetColumn(BoardController bc, string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                BusinessLayer.Board tempBoard = bc.GetBoard(userEmail, creatorEmail, boardName); //uses validLogin //need to use validMember
                BusinessLayer.Column bColumn = tempBoard.GetColumn(userEmail, columnOrdinal); //NO NEED VALIDS
                Column c = new(bColumn);
                return Response<IList<Task>>.FromValue(c.TaskList);
            }
            catch (Exception e)
            {
                return Response<IList<Task>>.FromError(e.Message);
            }
        }

        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the user. The user must be logged in.</param>
        /// <param name="creatorEmail">Email of the Board's creator.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public Response<Task> AddTask(BoardController bc, string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                BusinessLayer.Board tempBoard = bc.GetBoard(userEmail, creatorEmail, boardName); //uses validLogin //need to use validMember
                BusinessLayer.Task task = tempBoard.AddTask(userEmail, title, description, dueDate); //NO NEED VALIDS
                Task t = new Task(task);
                return Response<Task>.FromValue(t);
            }
            catch (Exception e)
            {
                return Response<Task>.FromError(e.Message);
            }
        }

        //
        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the user. The user must be logged in.</param>
        /// <param name="creatorEmail">Email of the Board's creator.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDueDate(BoardController bc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                BusinessLayer.Board tempBoard = bc.GetBoard(userEmail, creatorEmail, boardName); //uses validLogin //need to use validMember
                BusinessLayer.Column tempColumn = tempBoard.GetColumn(userEmail, columnOrdinal); //need to use validTaskChangeable
                BusinessLayer.Task tempTask = tempColumn.GetTask(taskId);
                tempTask.SetDueDate(userEmail, dueDate); //need to use validAssignee
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the user. The user must be logged in.</param>
        /// <param name="creatorEmail">Email of the Board's creator.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskTitle(BoardController bc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                BusinessLayer.Board tempBoard = bc.GetBoard(userEmail, creatorEmail, boardName); //uses validLogin //need to use validMember
                BusinessLayer.Column tempColumn = tempBoard.GetColumn(userEmail, columnOrdinal); //need to use validTaskChangeable
                BusinessLayer.Task tempTask = tempColumn.GetTask(taskId);
                tempTask.SetTitle(userEmail, tempBoard.Id, columnOrdinal,title); //need to use validAssignee
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the user. The user must be logged in.</param>
        /// <param name="creatorEmail">Email of the Board's creator.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDescription(BoardController bc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                BusinessLayer.Board tempBoard = bc.GetBoard(userEmail, creatorEmail, boardName); //uses validLogin //need to use validMember
                BusinessLayer.Column tempColumn = tempBoard.GetColumn(userEmail, columnOrdinal); //need to use validTaskChangeable
                BusinessLayer.Task tempTask = tempColumn.GetTask(taskId);
                tempTask.SetDescription(userEmail, description); //need to use validAssignee
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the user. The user must be logged in.</param>
        /// <param name="creatorEmail">Email of the Board's creator.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AdvanceTask(BoardController bc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                BusinessLayer.Board tempBoard = bc.GetBoard(userEmail, creatorEmail, boardName); //uses validLogin //need to use validMember
                tempBoard.AdvanceTask(userEmail, columnOrdinal, taskId); //need to use validTaskChangeable //need to use validAssignee
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">Email of the user to assign to task to</param>
        /// <returns>A response object. The response should contain da error message in case of an error</returns>
        public Response AssignTask(BoardController bc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            try
            {
                BusinessLayer.Board tempBoard = bc.GetBoard(userEmail, creatorEmail, boardName); //uses validLogin //need to use validMember
                BusinessLayer.Column tempColumn = tempBoard.GetColumn(userEmail, columnOrdinal); //need to use validTaskChangeable
                BusinessLayer.Task tempTask = tempColumn.GetTask(taskId);
                tempTask.SetAssignee(userEmail, emailAssignee); //need to use validAssignee
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        /// <summary>
        /// Adds a new column
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The location of the new column. Location for old columns with index>=columnOrdinal is increased by 1 (moved right). The first column is identified by 0, the location increases by 1 for each column.</param>
        /// <param name="columnName">The name for the new columns</param>        
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddColumn(BoardController bc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName)
        {
            try
            {
                BusinessLayer.Board tempBoard = bc.GetBoard(userEmail, creatorEmail, boardName); //uses validLogin //need to use validMember
                BusinessLayer.Column bColumn = tempBoard.GetColumn(userEmail, columnOrdinal); //NO NEED VALIDS
                tempBoard.AddColumn(columnName, columnOrdinal);
                List<BusinessLayer.Task> MovingTasks = bColumn.TaskList;
                foreach (BusinessLayer.Task task in MovingTasks)
                {
                    task.setColumnAfterMoving(tempBoard.Id, columnOrdinal, 1);
                }
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }



        /// <summary>
        /// Removes a specific column
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveColumn(BoardController bc, string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                BusinessLayer.Board tempBoard = bc.GetBoard(userEmail, creatorEmail, boardName); //uses validLogin //need to use validMember
                tempBoard.RemoveColumn(columnOrdinal);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        /// <summary>
        /// Renames a specific column
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <param name="newColumnName">The new column name</param>        
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RenameColumn(BoardController bc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            try
            {
                BusinessLayer.Board tempBoard = bc.GetBoard(userEmail, creatorEmail, boardName); //uses validLogin //need to use validMember
                BusinessLayer.Column tempColumn = tempBoard.GetColumn(userEmail, columnOrdinal);
                tempColumn.Name = newColumnName;
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Moves a column shiftSize times to the right. If shiftSize is negative, the column moves to the left
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <param name="shiftSize">The number of times to move the column, relativly to its current location. Negative values are allowed</param>  
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response MoveColumn(BoardController bc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            try
            {
                BusinessLayer.Board tempBoard = bc.GetBoard(userEmail, creatorEmail, boardName); //uses validLogin //need to use validMember
                BusinessLayer.Column bColumn = tempBoard.GetColumn(userEmail, columnOrdinal); //NO NEED VALIDS
                tempBoard.MoveColumn(columnOrdinal, shiftSize);
                List<BusinessLayer.Task> MovingTasks = bColumn.TaskList;
                foreach (BusinessLayer.Task task in MovingTasks)
                {
                    task.setColumnAfterMoving(tempBoard.Id,columnOrdinal, shiftSize);
                }

                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// The function receives a email and gets all the boards that the email's user is a member of
        /// </summary>
        /// <param name="bc">The boardController consisting all boards</param>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <returns>A response object with a value set to the list of boards (of the user as a member), The response should contain a error message in case of an error</returns>
        public Response<List<Board>> GetUserBoards(BoardController bc, string userEmail)
        {
            try
            {
                List<Board> ServiceBoards = new List<Board>();
                List<BusinessLayer.Board> businessBoards = bc.GetUserBoards(userEmail);
                foreach(BusinessLayer.Board board in businessBoards)
                {
                    Board b = new Board(board);
                    ServiceBoards.Add(b);
                }
                return Response<List<Board>>.FromValue(ServiceBoards);
            }
            catch (Exception e)
            {
                return Response<List<Board>>.FromError(e.Message);
            }
        }
    }
}






