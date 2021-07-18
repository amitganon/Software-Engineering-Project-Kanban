using System;
using System.Collections.Generic;
using log4net;
using System.Reflection;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.IO;
using log4net.Config;

namespace IntroSE.Kanban.Backend.BusinessLayer
{

    public class UserController : UserContollerMock
    {
        public readonly Dictionary<string, User> userDict = new();
        private int userTop = 0;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public UserController()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
        /// <summary>
        /// Get the number of users value
        /// </summary>
        public int UserTop { get => userTop; }


        /// <summary>
        /// Gets a specific User
        /// </summary>
        /// <param name="userEmail">The email address of the user</param>
        /// <exception cref="System.ArgumentException">Thrown when the email entered is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when a email with the email entered doesn't exist.</exception>
        /// <returns>The user with the required email.</returns>
        public User GetUser(string userEmail)
        {
            if (userEmail == null)
            {
                log.Error("GetUser got a null-email as input");
                throw new ArgumentException("The email you entered is null (empty)");
            }
            if (!userDict.ContainsKey(userEmail))
            {
                log.Error($"GetUser got an non-existing email as input: {userEmail}");
                throw new ArgumentException("There isn't an account with the email you entered");
            }
            return userDict[userEmail];
        }

        /// <summary>
        /// Load all the users from DB and insert to local program
        /// </summary>
        public void LoadUsers()
        {
            DalUserController duc = new DalUserController("Users");
            List<DataAccessLayer.DUser> dusers = duc.LoadUsers();
            foreach (DUser DU in dusers)
            {
                User user = new User(DU.Email, DU.Password, DU.Id);
                userDict[user.Email] = user;
            }
            userTop = dusers.Count;
            log.Info($"all of the users are insert to businessLayer UserControllerController");
        }



        /// <summary>
        /// Delete all the users from DB 
        /// </summary>
        public void Delete()
        {
            DalUserController duc = new DalUserController("Users");
            if (duc.DeleteAll())
                log.Info("all the users are deleted from the db");
            else
                log.Error($"cant delete from DalUserController");
        }


        /// <summary>
        /// Register a new user
        /// </summary>
        /// <remarks>
        /// Checking if the email and password are valid, if true, create a new user and adding it to
        /// the userController users.
        /// </remarks>
        /// <param name="userEmail">The email address of the user</param>
        /// <param name="password">The password of the user</param>
        /// <exception cref="System.ArgumentException">Thrown when one of the param are null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when email isn't legal .</exception>
        /// <exception cref="System.ArgumentException">Thrown when email entered has already an exsiting user.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the password isn't legal .</exception>
        public void Register(string userEmail, string password)
        {
            if (userEmail == null)
            {
                log.Error("Regiser got a null-email as input");
                throw new ArgumentException("The email you entered is null (empty)");
            }

            if (password == null)
            {
                log.Error("Regiser got a null-password as input");
                throw new ArgumentException("The password you entered is null (empty)");
            }
            IsValidEmail(userEmail);
            if (userDict.ContainsKey(userEmail))
            {
                log.Error($"Regiser got an existing email as input: {userEmail}");
                throw new ArgumentException("There is already an account with the email you entered");
            }
            PasswordValidetor passwordValidetor = new(password);
            passwordValidetor.ValidPassword();
            User myUser = new(userEmail, password, UserTop);
            InsertUserToDal(myUser.Duser);
            userDict[userEmail] = myUser;
            log.Info($"A new user has created: {userEmail}. With the id: {UserTop} and insert to the DB");

        }
        /// <summary>
        /// Insert User to DB
        /// </summary>
        /// <param name="du">The Dal User of User object</param>
        /// <exception cref="System.ArgumentException">Thrown when duis  null.</exception>
        public void InsertUserToDal(DUser du)
        {
            try
            {
                if (du == null)
                {
                    log.Error($"DUser is null and can't insert to DB");
                    throw new ArgumentException("Try to add to DB a Null Object");
                }
                du.InsertUser();
                log.Info($"User {du.Email} insert to DB");
                this.userTop++;
            }
            catch (Exception e)
            {
                log.Error($"There was an exception and the user with email {du.Email} didn't insert to the DB {e.Message}");
            }
        }

        /// <summary>
        /// <summary>
        /// Log In a user
        /// </summary>
        /// <remarks>
        /// Checking if the email and password are valid, if true, changing the loggedIn value 
        /// of the user to true.
        /// </remarks>
        /// <param name="userEmail">The email address of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>The User with the email&password entered.</returns>
        /// <exception cref="System.ArgumentException">Thrown when one of the param are null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when email entered doesn't exists in the userDict.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the User is already logged in.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the password is incorrect (doesn't match) .</exception>
        public User LogIn(string userEmail, string password)
        {
            if (userEmail == null)
            {
                log.Error("LogIn got a null-email as input");
                throw new ArgumentException("The email you entered is null (empty)");
            }
            if (password == null)
            {
                log.Error("LogIn got a null-password as input");
                throw new ArgumentException("The password you entered is null (empty)");
            }
            if (!userDict.ContainsKey(userEmail))
            {
                log.Error($"LogIn got a non-existing email as input: {userEmail}");
                throw new ArgumentException("This email does not exists in the system");
            }
            User myUser = GetUser(userEmail);
            if (myUser.LoggedIn)
            {
                log.Error($"LogIn got an (already) loggedIn email as input: {userEmail}");
                throw new ArgumentException("User is already logged in");
            }
            if (!myUser.ConfirmPassword(password))
            {
                log.Error($"LogIn attempted with a wrong password: {userEmail}");
                throw new ArgumentException("Password is incorrect");
            }
            myUser.LogIn();
            log.Info($"LogIn succesfully occured for the user: {userEmail}");
            return myUser;
        }


        /// <summary>
        /// Checks if the email address entered is legal, by a regex pattern.
        /// </summary>
        /// <param name="email">The email address that needs to be checked</param>
        /// <returns>The limit of the column.</returns>
        /// <exception cref="System.ArgumentException">Thrown when email isn't legal.</exception>
        private void IsValidEmail(string userEmail)
        {
            const string emailPattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            if (!Regex.IsMatch(userEmail, emailPattern, RegexOptions.IgnoreCase))
            {
                log.Error($"An illegal email adress attempted Registeration: {userEmail}");
                throw new ArgumentException("The email address you entered isn't legal");
            }
        }
        /// <summary>
        /// Checks if the user is logged before commiting action
        /// </summary>
        /// <param name="userEmail">The email address that needs to be checked</param>
        /// <returns>True if the user is logged, false else</returns>
        /// <exception cref="System.ArgumentException">Thrown when email isn't legal.</exception>
        public bool validLogin(string userEmail)
        {
            if (userEmail != null)
                return GetUser(userEmail).LoggedIn;
            else 
                throw new ArgumentException("validLogin got null Email as an input");
        }

        public void RegisterMock(string userEmail, string password , PasswordValidetorMock pv)
        {
            if (userEmail == null)
            {
                log.Error("Regiser got a null-email as input");
                throw new ArgumentException("The email you entered is null (empty)");
            }

            if (password == null)
            {
                log.Error("Regiser got a null-password as input");
                throw new ArgumentException("The password you entered is null (empty)");
            }
            IsValidEmail(userEmail);
            if (userDict.ContainsKey(userEmail))
            {
                log.Error($"Regiser got an existing email as input: {userEmail}");
                throw new ArgumentException("There is already an account with the email you entered");
            }
            pv.ValidPassword();
            User myUser = new(userEmail, password, UserTop);
            userDict[userEmail] = myUser;
            log.Info($"A new user has created: {userEmail}. With the id: {UserTop} and insert to the DB");

        }

        bool UserContollerMock.IsValidEmail(string s)
        {
            throw new NotImplementedException();
        }
    }
}