using System;
using System.Collections.Generic;
using log4net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class UserController //public for testing

    {
        private readonly Dictionary<string, User> userDict = new();
        private int userTop = 0;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Checks if the email address entered is legal, by a regex pattern.
        /// </summary>
        /// <param name="email">The email address that needs to be checked</param>
        /// <returns>The limit of the column.</returns>
        /// <exception cref="System.ArgumentException">Thrown when email isn't legal.</exception>
        private void IsValidEmail(string email)
        {
            const string emailPattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            if (!Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase))
            {
                log.Error($"An illegal email adress attempted Registeration: {email}");
                throw new ArgumentException("The email address you entered isn't legal");
                //Regex regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                //Match match = regex.Match(email);
                //if(!match.Success)
                //    throw new ArgumentException("The email address you entered isn't legal");
            }
        }


        /// <summary>
        /// Gets a specific User
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <exception cref="System.ArgumentException">Thrown when the email entered is null.</exception>
        /// <returns>The user with the required email.</returns>
        public User GetUser(string email)
        {
            if (email == null)
            {
                log.Error("GetUser got a null-email as input");
                throw new ArgumentException("The email you entered is null (empty)");
            }
            if (!userDict.ContainsKey(email))
            {
                log.Error($"GetUser got an non-existing email as input: {email}");
                throw new ArgumentException("There isn't an account with the email you entered");
            }
            return userDict[email];
        }


        /// <summary>
        /// Gets the userTop value
        /// </summary>
        /// <returns>The (int) userTop value.</returns>
        public int GetUserTop()
        {
            return this.userTop;
        }


        /// <summary>
        /// Register a new user
        /// </summary>
        /// <remarks>
        /// Checking if the email and password are valid, if true, create a new user and adding it to
        /// the userController users.
        /// </remarks>
        /// <param name="email">The email address of the user</param>
        /// <param name="password">The password of the user</param>
        /// <exception cref="System.ArgumentException">Thrown when one of the param are null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when email isn't legal .</exception>
        /// <exception cref="System.ArgumentException">Thrown when email entered has already an exsiting user.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the password isn't legal .</exception>
        public void Register(string email, string password)
        {
            if (email == null)
            {
                log.Error("Regiser got a null-email as input");
                throw new ArgumentException("The email you entered is null (empty)");
            }

            if (password == null)
            {
                log.Error("Regiser got a null-password as input");
                throw new ArgumentException("The password you entered is null (empty)");
            }
            IsValidEmail(email);
            if (userDict.ContainsKey(email))
            {
                log.Error($"Regiser got an existing email as input: {email}");
                throw new ArgumentException("There is already an account with the email you entered");
            }
            PasswordValidetor passwordValidetor = new(password);
            passwordValidetor.ValidPassword();
            User myUser = new(email, password, GetUserTop());
            userTop++; //keeping track on the last UserId made
            userDict[email] = myUser;
            log.Info($"A new user has created: {email}. With the id: {GetUserTop() - 1}");
        }


        /// <summary>
        /// Log In a user
        /// </summary>
        /// <remarks>
        /// Checking if the email and password are valid, if true, changing the loggedIn value 
        /// of the user to true.
        /// </remarks>
        /// <param name="email">The email address of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>The User with the email&password entered.</returns>
        /// <exception cref="System.ArgumentException">Thrown when one of the param are null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when email entered doesn't exists in the userDict.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the User is already logged in.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the password is incorrect (doesn't match) .</exception>
        public User LogIn(string email, string password)
        {
            if (email == null)
            {
                log.Error("LogIn got a null-email as input");
                throw new ArgumentException("The email you entered is null (empty)");
            }
            if (password == null)
            {
                log.Error("LogIn got a null-password as input");
                throw new ArgumentException("The password you entered is null (empty)");
            }
            if (!userDict.ContainsKey(email))
            {
                log.Error($"LogIn got a non-existing email as input: {email}");
                throw new ArgumentException("This email does not exists in the system");
            }
            User myUser = GetUser(email);
            if (myUser.IsLogged())
            {
                log.Error($"LogIn got an (already) loggedIn email as input: {email}");
                throw new ArgumentException("User is already logged in");
            }
            if (!myUser.ConfirmPassword(password))
            {
                log.Error($"LogIn attempted with a wrong password: {email}");
                throw new ArgumentException("Password is incorrect");
            }
            myUser.LogIn();
            log.Info($"LogIn succesfully occured for the user: {email}");
            return myUser;
        }
    }
}