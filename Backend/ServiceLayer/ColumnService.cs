using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ColumnService //public for testing
    {

        /// <summary>
        /// Set the limit of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The wanted limit to set to the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error.</returns>
        /// 
        public Response LimitColumn(UserController uc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                BusinessLayer.User tempUser = uc.GetUser(userEmail);
                BusinessLayer.Board tempBoard = tempUser.GetBoard(creatorEmail, boardName);
                tempBoard.GetColumn(columnOrdinal).TasksLimit = limit;
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
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The limit of the column.</returns>
        public Response<int> GetColumnLimit(UserController uc, string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                BusinessLayer.User tempUser = uc.GetUser(userEmail);
                BusinessLayer.Board tempBoard = tempUser.GetBoard(creatorEmail, boardName);
                return Response<int>.FromValue(tempBoard.GetColumn(columnOrdinal).TasksLimit);
            }
            catch (Exception e)
            {
                return Response<int>.FromError(e.Message);
            }
        }


        /// <summary>
        /// Get the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The name of the column.</returns>
        public Response<string> GetColumnName(UserController uc, string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                BusinessLayer.User tempUser = uc.GetUser(userEmail);
                BusinessLayer.Board tempBoard = tempUser.GetBoard(creatorEmail, boardName);
                return Response<string>.FromValue(tempBoard.GetColumnName(columnOrdinal));
            }
            catch (Exception e)
            {
                return Response<string>.FromError(e.Message);
            }
        }


        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> GetColumn(UserController uc, string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                IList<Task> myList = new List<Task>();
                BusinessLayer.User tempUser = uc.GetUser(userEmail);
                BusinessLayer.Board tempBoard = tempUser.GetBoard(creatorEmail, boardName);
                foreach (BusinessLayer.Task task in tempBoard.GetColumn(columnOrdinal).TaskList)
                {
                    Task tmpTask = new Task(task.GetId(), task.creationTime, task.title, task.description, task.dueDate, task.assignee);
                    myList.Add(tmpTask);
                }
                return Response<IList<Task>>.FromValue(myList);
            }
            catch (Exception e)
            {
                return Response<IList<Task>>.FromError(e.Message);
            }
        }
    }
}
