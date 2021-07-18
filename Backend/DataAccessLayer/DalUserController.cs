using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /*
`   The main DalUserController class
Contains all methods for performing DalUserController's actions.
*/
    /// <summary>
    /// The main DalUserController class.
    /// Contains all methods for performing DalUserController's functions.
    /// </summary>
    /// <remarks>
    /// This class holds all the methodes that performing all the Users in the db.
    /// </remarks>

    public class DalUserController : DalController
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public DalUserController(string tableName) : base(tableName)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
        /// <summary>
        /// Insert user to DB
        /// </summary>
        /// <param name="duser">the user to insert</param>
        /// <param name="idColumn">the id column name in the db </param>
        /// <param name="emailColumn">the email column name in the db </param>
        /// <param name="passwordColumn">the password column name in the db </param>
        /// <returns>true if deleation succeeded , false else </returns>
        public bool InsertUser(DUser duser, string idColumn , string emailColumn , string passwordColumn)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {_tableName} ({idColumn} , {emailColumn}, {passwordColumn}) " +
                        $"VALUES (@idVal,@emailVal,@passwordVal);";

                    SQLiteParameter emailVal = new SQLiteParameter(@"emailVal", duser.Email);
                    SQLiteParameter passwordVal = new SQLiteParameter(@"passwordVal", duser.Password);
                    SQLiteParameter idVal = new SQLiteParameter(@"idVal", duser.Id);

                    command.Parameters.Add(emailVal);
                    command.Parameters.Add(passwordVal);
                    command.Parameters.Add(idVal);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Info($"Succesfully inserted to database the user. id:'{duser.Id}', email:'{duser.Email}'");

                }
                catch (Exception e)
                {
                    log.Error($"Error in  inserted to database the user. id:'{duser.Id}', email:'{duser.Email}'. cause: {e}");
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }
        /// <summary>
        /// Load all users from DB 
        /// </summary>
        /// <returns>list of users from DB</returns>
        public List<DUser> LoadUsers()
        {
            List<DUser> result = Select().Cast<DUser>().ToList();
            return result;
        }
        /// <summary>
        /// convert the data to DTO objects , specific for all controller
        /// </summary>
        /// <param name="reader">Object from the DB</param>
        /// <returns>DTO object </returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            DUser result = new DUser(reader.GetString(1), reader.GetString(2), reader.GetInt32(0));
            log.Debug($"Succesfully pull from DataReader user: id: {result.Id} , email {result.Email}");
            return result;

        }

    }

}