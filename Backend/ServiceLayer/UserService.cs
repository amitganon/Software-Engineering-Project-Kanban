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

        ///<summary>register a newe user in the system.</summary>
        ///<param name="uc">the userController of the business layer.</param>
        ///<param name="email">the email of the new user.</param>
        ///<param name="password">the password of the new user.</param>
        ///<returns>return a positive response or response with exception</returns>
        public Response Register(UserController uc, String email, String password)
        {
            try
            {
                uc.Register(email, password);
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }
        }


        ///<summary>log in user to the system.</summary>
        ///<param name="uc">the userController of the business layer.</param>
        ///<param name="email">the email of the user.</param>
        ///<param name="password">the password of the user.</param>
        ///<returns>return response with the user or response with exception</returns>
        public Response<User> LogIn(UserController uc, String email, String password)
        {
            try
            {
                User user =new User (uc.LogIn(email, password).email);
                return Response<User>.FromValue(user);
            }
            catch (Exception e)
            {
                return Response<User>.FromError(e.Message);
            }
        }


        ///<summary>log out a user from the system.</summary>
        ///<param name="uc">the userController of the business layer.</param>
        ///<param name="email">the email of the user.</param>
        ///<returns>return a positive response or response with exception</returns>
        public Response LogOut(UserController uc, String email)
        {
            try
            {
                uc.GetUser(email).LogOut();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        ///<summary>change the  password of an user.</summary>
        ///<param name="uc">the userController of the business layer.</param>
        ///<param name="email">the email of the user.</param>
        ///<param name="password">the user new password.</param>
        ///<param name="oldPassword">the eser old password.</param>
        ///<returns>return a positive response or response with exception</returns>
        public Response ChangePassword(UserController uc, String email, String password, String oldPassword)
        {
            try
            {
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        ///<summary>take from the business layer and copy the user in progress tasks list to ilist.</summary>
        ///<param name="uc">the userController of the business layer.</param>
        ///<param name="email">the email of the user.</param>
        ///<returns>return response with Ilist of the in progress tasks of the user  or response with exception</returns>
        public Response<IList<Task>> InProgressTasks(UserController uc, String email)
        {
            try
            {
                IList<Task> myList = new List<Task>();
                foreach(BusinessLayer.Task task in uc.GetUser(email).InProgressTasks())
                {
                    Task tmpTask = new Task(task.GetId(), task.creationTime, task.title, task.description, task.dueDate);
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
