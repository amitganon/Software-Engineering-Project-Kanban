using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService //public for testing
    {


        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="uc">The user controller needed to get the user.</param>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public Response<Task> AddTask(UserController uc, string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                BusinessLayer.Board tempBoard = uc.GetUser(userEmail).GetBoard(creatorEmail, boardName);
                BusinessLayer.Task task = tempBoard.AddTask(title, description, dueDate);
                Task t = new Task(task.GetId(), task.creationTime, task.title, task.description, task.dueDate, task.assignee);
                return Response<Task>.FromValue(t);
            }
            catch (Exception e)
            {
                return Response<Task>.FromError(e.Message);
            }
        }


        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="uc">The user controller needed to get the user.</param>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDueDate(UserController uc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                BusinessLayer.Board tempBoard = uc.GetUser(userEmail).GetBoard(creatorEmail, boardName);
                tempBoard.UpdateTaskDueDate(userEmail, columnOrdinal, taskId, dueDate);
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
        /// <param name="uc">The user controller needed to get the user.</param>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskTitle(UserController uc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                BusinessLayer.Board tempBoard = uc.GetUser(userEmail).GetBoard(creatorEmail, boardName);
                tempBoard.UpdateTaskTitle(userEmail, columnOrdinal, taskId, title);
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
        /// <param name="uc">The user controller needed to get the user.</param>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDescription(UserController uc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                BusinessLayer.Board tempBoard = uc.GetUser(userEmail).GetBoard(creatorEmail, boardName);
                tempBoard.UpdateTaskDescription(userEmail, columnOrdinal, taskId, description);
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
        /// <param name="uc">The user controller needed to get the user.</param>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AdvanceTask(UserController uc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                BusinessLayer.Board tempBoard = uc.GetUser(userEmail).GetBoard(creatorEmail, boardName);
                tempBoard.AdvanceTask(userEmail, columnOrdinal, taskId);
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
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">Email of the user to assign to task to</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AssignTask(UserController uc, string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            try
            {
                BusinessLayer.Board tempBoard = uc.GetUser(userEmail).GetBoard(creatorEmail, boardName);
                tempBoard.UpdateAssignee(emailAssignee, columnOrdinal, taskId);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
    }
}
