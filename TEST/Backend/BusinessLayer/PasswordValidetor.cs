using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /*
`   The main PasswordValidetor class
    Contains all methods for performing PasswordValidetor's actions.
     */
    /// <summary>
    /// The main PasswordValidetor class.
    /// Contains all methods for performing PasswordValidetor's functions.
    /// </summary>
    /// <remarks>
    /// This PasswordValidetor will check the propriety of the input that will be the user's password
    /// the PasswordValidetor object will check of upper and lower case of letters , min and max of password and if conations any numbers
    /// </remarks>



    public class PasswordValidetor : PasswordValidetorMock
    {
        private String password;
        private const int passwordMaxLeangth = 20;
        private const int passwordMinLeangth = 4;
        private string[] bannedPasswords = { "123456", "123456789", "qwerty", "password", "1111111", "12345678", "abc123", "1234567", "password1", "12345", "1234567890", "123123", "000000", "Iloveyou", "1234", "1q2w3e4r5t", "Qwertyuiop", "123", "Monkey", "Dragon" };
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        ///<summary>the constructor of PasswordValidetor.</summary>
        public PasswordValidetor(String pass)
        {
            this.password = pass;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }


        ///<summary>This method checked if the password is valid.</summary>
        /// <exception cref="System.AggregateException">Thrown when password is null.</exception>
        /// <exception cref="System.AggregateException">Thrown when passward not contains upper case.</exception>
        /// <exception cref="System.AggregateException">Thrown when passward not contains lower case.</exception>
        /// <exception cref="System.AggregateException">Thrown when passward not contains a number.</exception>
        ///<returns>return if the password valid or not</returns>
        public bool ValidPassword()
        {
            if (this.password == null)
            {
                log.Error($"validPassword input was null, password- {password}");
                throw new ArgumentException("password cant be null");
            }
            try
            {
                CheckMax(this.password);
                CheckMin(this.password);
            }
            catch (Exception e)
            {
                log.Error($"the length of the password not in range, with this password- {password}");
                throw new ArgumentException(e.Message);
            }
            if (!CheckUpperCase(this.password))
            {
                log.Error($"passward not contains upper case, password- {password}");
                throw new ArgumentException("password must contains at least one upper case");
            }
            if (!CheckLowerCase(this.password))
            {
                log.Error($"passward not contains lower case, password- {password}");
                throw new ArgumentException("password must contains at least one lower case");
            }
            if (!CheckNumber(this.password))
            {
                log.Error($"passward not contains number, password- {password}");
                throw new ArgumentException("password must contains at least one number");
            }
            if (CheckCommonPasswords())
            {
                log.Error($"passward {password} is one of the top 20 common passwords published by the National CyberSecurity Centre and can't be added ");

                throw new ArgumentException($"passward {password} is one of the top 20 common passwords published by the National CyberSecurity Centre and can't be added ");
            }
            log.Debug($"passwordValidetor return true with this password- {password}");
            return true;
        }




        ///<summary>This method checked if the password not pass the limit length.</summary>
        ///<param name="pass">the password of the user.</param>
        /// <exception cref="System.AggregateException">Thrown when password is passed the limit length.</exception>
        ///<returns>return if the password not passed the limit length </returns>
        private bool CheckMax(String pass)
        {
            if (pass.Length > passwordMaxLeangth)
                throw new ArgumentException("password can't be more then 20 characters");
            return true;
        }


        ///<summary>This method checked if the password not less then the limit.</summary>
        ///<param name="pass">the password of the user.</param>
        /// <exception cref="System.AggregateException">Thrown when password is less then the limit.</exception>
        ///<returns>return if the password length is less then the limit </returns>
        private bool CheckMin(String pass)
        {
            if (pass.Length < passwordMinLeangth)
                throw new ArgumentException("password can't be less then 4 characters");
            return true;
        }


        ///<summary>This method checked if the password contains at least one upper case.</summary>
        ///<param name="pass">the password of the user.</param>
        ///<returns>return if the password containse at least one upper case </returns>
        private bool CheckUpperCase(String pass)
        {
            for(int i=0; i < pass.Length; i++)
            {
                char temp = pass[i];
                if (char.IsUpper(temp))
                    return true;
            }
            return false;
        }


        ///<summary>This method checked if the password contains at least one lower case.</summary>
        ///<param name="pass">the password of the user.</param>
        ///<returns>return if the password containse at least one lower case </returns>
        private bool CheckLowerCase(String pass)
        {
            for (int i = 0; i < pass.Length; i++)
            {
                char temp = pass[i];
                if (char.IsLower(temp))
                    return true;
            }
            return false;
        }


        ///<summary>This method checked if the password contains at least one number.</summary>
        ///<param name="pass">the password of the user.</param>
        ///<returns>return if the password containse at least one number </returns>
        private bool CheckNumber(String pass)
        {
            for (int i = 0; i < pass.Length; i++)
            {
                char temp = pass[i];
                if (char.IsDigit(temp))
                    return true;
            }
            return false;
        }

        private bool CheckCommonPasswords()
        {
            return bannedPasswords.Contains(this.password);
        }
    }
}
