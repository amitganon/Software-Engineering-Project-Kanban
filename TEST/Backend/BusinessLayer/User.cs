using System;
using System.Collections.Generic;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /*
    The main User class
    Contains all methods for performing User's actions.
    */
    /// <summary>
    /// The main User class.
    /// Contains all methods for performing User's functions.
    /// </summary>
    /// <remarks>
    /// This class can log in+out, add+get+remove boards, get togetger all the inProgress Tasks,
    /// get the user's Id+email, and get indication on if the user is logged in.
    /// </remarks>
    public class User : UserMock
    {
        public string email;
        private string password;
        public bool loggedIn;
        private int id;
        private DUser dUser;
        private List<int> boardList;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public User(string email, string password, int id)
        {
            this.email = email;
            this.password = password;
            this.loggedIn = false;
            this.id = id;
            this.dUser = new DUser(email, password, id);
            this.boardList = new List<int>();

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        /// <summary>
        /// Get the Email value of user
        /// </summary>
        public string Email { get => email; }
        /// <summary>
        /// Get if the user is login or not
        /// </summary>
        public bool LoggedIn { get => loggedIn; }
        /// <summary>
        /// Get the Id value of user
        /// </summary>
        public int Id { get => id; }
        /// <summary>
        /// Get the DUser value of user
        /// </summary>
        public DUser Duser { get  => dUser; }
        /// <summary>
        /// Get the list of id borads of user
        /// </summary>
        public List<int> BoardList { get => boardList; }

        /// <summary>
        /// Sets the loggedIn value to true
        /// </summary>
        public void LogIn() { 
            loggedIn = true; 
        }

        /// <summary>
        /// Sets the loggedIn value to false
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when The user is already
        /// logged out.</exception>
        public void LogOut()
        {
            if (!this.LoggedIn)
            {
                log.Error($"user: {email}, attempted to log out while already logged out");
                throw new ArgumentException("Already logged out");
            }
            loggedIn = false;
            log.Info($"user '{email}' has logged out");
        }

        /// <summary>
        /// Equalizes the user's password with the entered password.
        /// </summary>
        /// <param name="password">The password entered by the user</param>
        /// <returns>A boolean value indicating on if the user's password matches the password entered.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the entered password is null.</exception>
        public bool ConfirmPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentException("The password you entered is null (empty)");
            }
            bool same = this.password == password;
            log.Debug($"ConfirmPassword match {same}");
            return same;
        }
        /// <summary>
        /// Add borad ID to the list of borads id that user holds.
        /// </summary>
        /// <param name="boardId">The borad id to add to the list</param>
        /// <exception cref="System.ArgumentException">Thrown when the entered borad id is not valid.</exception>

        public void AddBoardToList(int boardId)
        {
            if (boardId < 0)
            {
                log.Error($"user: {email}, attempted to add illigel board id to his list");
                throw new ArgumentException("Board id not valid to enter the borad list of user");
            }
            else boardList.Add(boardId);
            log.Info($"user:{email} added to his board's list. boardId:{boardId}");
        }
        /// <summary>
        /// Remove borad ID from the list of borads id that user holds.
        /// </summary>
        /// <param name="boardId">The borad id to remove from  the list</param>
        /// <exception cref="System.ArgumentException">Thrown when the entered borad id is not valid or not exixst in the list.</exception>
        /// 
        public void DeleteBoardFromList(int boardId)
        {
            if (boardId < 0)
            {
                log.Error($"user: {email}, attempted to add illigel board id to his list");
                throw new ArgumentException("Board id not valid to enter the borad list of user");
            }
            if (!BoardList.Contains(boardId))
            {
                log.Error($"user: {email}, attempted to remove board that not in his board list");
                throw new ArgumentException("attempted to remove board that not in his board list");
            }
            boardList.Remove(boardId);
            log.Info($"user: {email} remove borad with id {boardId}");
        }
    }
}
