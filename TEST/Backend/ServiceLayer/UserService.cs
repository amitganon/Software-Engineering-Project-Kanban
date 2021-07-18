using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService //public for testing
    {
        /*
        The main UserController class
        Contains all methods for performing UserController's actions.
        */
        /// <summary>
        /// The main UserController class.
        /// Contains all methods for performing UserController's functions.
        /// </summary>
        /// <remarks>
        /// The UserController will list all the users that register to the program , and will manage user's log in and log out
        /// The UserController will be able to change user's password and will save all the task from the user's borad that in the in progress column
        /// </remarks>


        ///<summary>Load all the users from DB</summary>
        ///<param name="uc">the userController of the business layer.</param>
        ///<returns>return a positive response or response with exception</returns>
        public Response LoadUsers(UserController uc)
        {
            try
            {
                uc.LoadUsers();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
        ///<summary>Delete all the users from DB</summary>
        ///<param name="uc">the userController of the business layer.</param>
        ///<returns>return a positive response or response with exception</returns>
        public Response DeleteAll(UserController uc)
        {
            try
            {
                uc.Delete();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        ///<summary>register a newe user in the system.</summary>
        ///<param name="uc">the userController of the business layer.</param>
        ///<param name="userEmail">the email of the new user.</param>
        ///<param name="password">the password of the new user.</param>
        ///<returns>return a positive response or response with exception</returns>
        public Response Register(UserController uc, String userEmail, String password)
        {
            try
            {
                uc.Register(userEmail, password);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        ///<summary>log in user to the system.</summary>
        ///<param name="uc">the userController of the business layer.</param>
        ///<param name="userEmail">the email of the user.</param>
        ///<param name="password">the password of the user.</param>
        ///<returns>return response with the user or response with exception</returns>
        public Response<User> LogIn(UserController uc, String userEmail, String password)
        {
            try
            {
                User user = new User(uc.LogIn(userEmail, password).Email);
                return Response<User>.FromValue(user);
            }
            catch (Exception e)
            {
                return Response<User>.FromError(e.Message);
            }
        }


        ///<summary>log out a user from the system.</summary>
        ///<param name="uc">the userController of the business layer.</param>
        ///<param name="userEmail">the email of the user.</param>
        ///<returns>return a positive response or response with exception</returns>
        public Response LogOut(UserController uc, String userEmail)
        {
            try
            {
                uc.GetUser(userEmail).LogOut();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


    }
}
