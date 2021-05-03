using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService //public for testing
    {


        /// <summary>
        /// Adds a board to the specific user.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the new board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddBoard(UserController uc, string userEmail, string boardName)
        {
            try
            {
                BusinessLayer.User tempUser = uc.GetUser(userEmail);
                tempUser.AddBoard(boardName);
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
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveBoard(UserController uc, string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                BusinessLayer.User tempUser = uc.GetUser(userEmail);
                tempUser.RemoveBoard(userEmail, boardName);
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
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <returns>A response object with a value set of the names of the board, The response should contain a error message in case of an error</returns>
        public Response<IList<String>> GetBoardNames(UserController uc, string userEmail)
        {
            try
            {
                return uc.GetUser(userEmail).GetBoardsNames();
            }
            catch (Exception e)
            {
                return Response<IList<String>>.FromError(e.Message);
            }
        }
        //+ JoinBoard(uc:UserController, userEmail:string, creatorEmail:string, boardName:string): Response


        /// <summary>
        /// joins borad to user's borads
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">The email of the craetor of the board the user wnat to join</param>
        /// <param name="boardName">The name of the board the user wnat to join</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response JoinBoard(UserController uc, string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                BusinessLayer.User tempUser = uc.GetUser(userEmail);
                tempUser.JoinBoard(creatorEmail, boardName);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

    }
}






