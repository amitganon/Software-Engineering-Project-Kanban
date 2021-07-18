
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /*
`   The main DalController class
        Contains all methods for performing DalMemberController's actions.
        */
    /// <summary>
    /// The main DalController class. 
    /// Contains all methods for performing DalController's functions.
    /// abstarct controller to common actions to all controllers
    /// </summary>
    /// <remarks>
    /// </remarks>
    public abstract class DalController
    {
        protected readonly string _connectionString;
        protected readonly string _tableName;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
        /// <summary>
        /// Delete all data from given table 
        /// </summary>
        /// <returns>true if deleation succeeded , false else </returns>

        public bool DeleteAll()
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteDataReader dataReader = null;
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Delete from {_tableName}"
                };
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    if (!dataReader.HasRows)
                        return true;
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        /// <summary>
        /// delete Object from DB given table
        /// </summary>
        /// <param name="DTOObj">the object we want to delete</param>
        /// <returns>true if deleation succeeded , false else </returns>
        public bool Delete(DTO DTOObj)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where ID={DTOObj.Id}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Succssesfully delete {DTOObj}");
                }
                catch (Exception e)
                {
                    log.Error($"Faild to delete {DTOObj} , cause {e}" );
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        /// <summary>
        /// pull from the DB all data from given table name
        /// </summary>
        /// <returns> list of all objects from given table</returns>
        protected List<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                        log.Info($"Succssesfully select");
                    }
                }
                catch (Exception e)
                {
                    log.Error($"Faild to select  , cause {e}");
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }
        /// <summary>
        /// convert the data to DTO objects , specific for all controller
        /// </summary>
        /// <param name="reader">Object from the DB</param>
        /// <returns>DTO object </returns>
        protected abstract DTO ConvertReaderToObject(SQLiteDataReader reader);

    }
}