using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /*
    The main DUser class
    Contains all methods for performing DUser's actions.
    */
    /// <summary>
    /// The main DUser class.
    /// Exist the Insert User to DB and maintain separate between layers 
    /// </summary>
    /// <remarks>
    /// This class kept only function ,  Insert the user to DB
    /// </remarks>
    public class DUser : DTO
    {

        private string email;
        private string password;

        public const string UserIdColumnName = "ID";
        public const string UserEmailColumnName = "UserEmail";
        public const string UserPasswordColumnName = "UserPassword";

        public string Email { get => email; }
        public string Password { get => password; }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public DUser(string email, string password, int id) : base(new DalUserController("Users"), id)
        {
            this.email = email;
            this.password = password;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        /// <summary>
        /// Insert this DUser to DB
        /// </summary>
        public bool InsertUser()
        {
            if (((DalUserController)_controller).InsertUser(this, UserIdColumnName, UserEmailColumnName, UserPasswordColumnName))
            {
                log.Info($"Succesfully inserted user to 'Users' database. id:'{Id}', and email:'{email}'");
                return true;
            }
            throw new AggregateException($"error when try to insert user to 'Users' database. id:'{Id}', and email:'{email}'");

        }

    }
}
