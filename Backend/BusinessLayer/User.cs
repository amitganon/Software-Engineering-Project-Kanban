using System;
using System.Collections.Generic;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;

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
    public class User //public for testing
    {
        public readonly string email;
        private string password;
        public bool loggedIn;
        private readonly Dictionary<String, Board> boardDict;
        private int boardTop;
        private readonly int id;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public User(string email, string password, int id)
        {
            this.email = email;
            this.password = password;
            this.loggedIn = false;
            this.boardDict = new Dictionary<String, Board>();
            this.boardTop = 1;
            this.id = id;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }


        /// <summary>
        /// Gets the user's ID
        /// </summary>
        /// <returns>The (int) user's Id.</returns>
        public int GetId()
        {
            return this.id;
        }


        /// <summary>
        /// Gets the boardTop value
        /// </summary>
        /// <returns>The (int) boardTop.</returns>
        public int GetBoardTop()
        {
            return this.boardTop;
        }


        /// <summary>
        /// Gets the user's email
        /// </summary>
        /// <returns>The (string) user's email.</returns>
        public string GetEmail()
        {
            return this.email;
        }


        /// <summary>
        /// Checks if the user is logged in to the system.
        /// </summary>
        /// <returns>A boolean value that indicates if the user is logged in.</returns>
        public bool IsLogged()
        {
            return this.loggedIn;
        }


        /// <summary>
        /// Sets the loggedIn value to true
        /// </summary>
        public void LogIn()
        {
            loggedIn = true;
        }


        /// <summary>
        /// Sets the loggedIn value to false
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when The user is already
        /// logged out.</exception>
        public void LogOut()
        {
            if (!IsLogged())
            {
                log.Error($"user: {GetEmail()}, attempted to log out while already logged out");
                throw new ArgumentException("Already logged out");
            }
            loggedIn = false;
            log.Info($"user '{GetEmail()}' has logged out");
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
        /// Changes the user's password
        /// </summary>
        /// <remarks>
        /// Checking match with current password, if true, checking by PasswordValidator class
        /// if the new password is legal, if true, changes the user's password to the new password.
        /// </remarks>
        /// <param name="oldP">The current password of the user</param>
        /// <param name="newP">The new password the user wants to change to</param>
        /// <exception cref="System.ArgumentException">Thrown when one of the param are null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the User is logged out.</exception>
        /// <exception cref="System.ArgumentException">Thrown when OldP doesn't match this.password .</exception>
        /// <exception cref="System.ArgumentException">Thrown when newP is not valid.</exception>
        public void ChangePassword(string oldP, string newP)
        {
            if (oldP == null)
            {
                log.Error($"user: {GetEmail()}, attempted to change password with a null old password");
                throw new ArgumentException("The old password you entered is null (empty)");
            }
            if (newP == null)
            {
                log.Error($"user: {GetEmail()}, attempted to change password with a null new password");
                throw new ArgumentException("The new password you entered is null (empty)");
            }
            if (!IsLogged())
            {
                log.Error($"user: {GetEmail()}, attempted to change password while logged out");
                throw new ArgumentException("Can't make actions on logged-out user");
            }
            if (!ConfirmPassword(oldP))
            {
                log.Error($"user: {GetEmail()}, attempted to change password with a wrong old password");
                throw new ArgumentException("Old password incorrect");
            }
                PasswordValidetor passwordValidetor = new(newP);
                passwordValidetor.ValidPassword();
                this.password = newP;
                log.Info($"user: {GetEmail()}, changed its password");
        }


        /// <summary>
        /// Adds a new board to the user's boards.
        /// </summary>
        /// <param name="name">The required name of the new board</param>
        /// <exception cref="System.ArgumentException">Thrown when the name entered is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the User is logged out.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the name entered is already taken.</exception>

        public void AddBoard(string name)
        {
            if (name == null)
            {
                log.Error($"user: {GetEmail()}, attempted to add board with a null input");
                throw new ArgumentException("name cant be null (empty)");
            }
            if (!IsLogged())
            {
                log.Error($"user: {GetEmail()}, attempted to add board while logged out");
                throw new ArgumentException("Can't make actions on logged-out user");
            }
            if (boardDict.ContainsKey(name))
            {
                log.Error($"user: {GetEmail()}, attempted to add board with an existing name");
                throw new ArgumentException("This board name is already in use! Please choose a different one");
            }
            int boardId = int.Parse(GetId() + "0" + GetBoardTop()); //unique boardId, depends on the userId 
            Board newBoard = new(name, boardId, GetEmail());
            boardTop++; //keeping track on the last boardId made
            boardDict[name] = newBoard;
            log.Info($"A new board has created for user: {GetEmail()}. With the board id: {GetBoardTop() - 1}");
        }

        /// <summary>
        /// Gets a specfic board
        /// </summary>
        /// <param name="name">The name of the required board</param>
        /// <returns>The board with the name entered.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the name entered is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the User is logged out.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the board with the name entered doesn't exist .</exception>
        public Board GetBoard(string name)
        {
            if (name == null)
            {
                log.Error($"user: {GetEmail()}, attempted to get board with a null input");
                throw new ArgumentException("name cant be null (empty)");
            }
            if (!IsLogged())
            {
                log.Error($"user: {GetEmail()}, attempted to get board while logged out");
                throw new ArgumentException("Can't make actions on logged-out user");
            }
            if (!boardDict.ContainsKey(name))
            {
                log.Error($"user: {GetEmail()}, attempted to get board which doesn't exist");
                throw new ArgumentException("This board name doesn't exist");
            }
            return boardDict[name];
        }

        /// <summary>
        /// Removes a specific board from the user's boards.
        /// </summary>
        /// <param name="name">The name of the required board</param>
        /// <exception cref="System.ArgumentException">Thrown when the name entered is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the User is logged out.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the board with the name entered doesn't exist .</exception>
        public void RemoveBoard(string name)
        {
            if (name == null)
            {
                log.Error($"user: {GetEmail()}, attempted to remove board with a null input");
                throw new ArgumentException("name cant be null (empty)");
            }
            if (!IsLogged())
            {
                log.Error($"user: {GetEmail()}, attempted to remove board while logged out");
                throw new ArgumentException("Can't make actions on logged-out user");
            }
            if (!boardDict.ContainsKey(name))
            {
                log.Error($"user: {GetEmail()}, attempted to remove board which doesn't exist");
                throw new ArgumentException("This board name doesn't exist");
            }
            boardDict.Remove(name);
            log.Info($"user: {GetEmail()} removed a board");
        }


        /// <summary>
        /// Gets all the inProgress tasks to one list.
        /// </summary>
        /// <returns>The inProgress task list.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the User is logged out.</exception>
        public IList<Task> InProgressTasks()
        {
            if (!IsLogged())
            {
                log.Error($"user: {GetEmail()}, attempted to get InProgressTasks while logged out");
                throw new ArgumentException("Can't make actions on logged-out user");
            }
            IList<Task> myProgList = new List<Task>();
            foreach (KeyValuePair<string, Board> board in boardDict)
            {
                IList<Task> prog = board.Value.inProgress.TaskList;
                foreach (Task task in prog)
                    myProgList.Add(task);
            }
            return myProgList;
        }
    }
}
