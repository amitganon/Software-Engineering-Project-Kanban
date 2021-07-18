using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /*
       The main DalColumnController class
        Contains all methods for performing changing the database according to Columns' actions.
        */
    /// <summary>
    /// The main DalColumnController class
    /// Contains all methods for performing changing the database according to Columns' actions.
    /// </summary>
    /// <remarks>
    /// This class inherit methods from abstract class DalController 
    /// </remarks>
    public class DalColumnController : DalController
    {
        private const string ColumnsTableName = "Columns";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DalColumnController() : base(ColumnsTableName)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        ///<summary>
        ///This method insert a new line of column information to the database
        ///</summary>
        /// <param name="dColumn">The object that holds the information about a specific column</param>
        /// <exception cref="System.ArgumentException">Thrown when the insertion fails.</exception>
        ///<returns>return a boolean value on if lines in the database has changed</returns>
        public bool Insert(DColumn dColumn)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnsTableName} ({DTO.IDColumnName},{DColumn.ColumnOrdinalColumn},{DColumn.ColumnNameColumn},{DColumn.TaskLimitColumn}) " +
                        $"VALUES (@boardIdVal,@columnOrdinalVal,@columnNameVal,@taskLimitVal);";

                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", dColumn.Id);
                    SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"columnOrdinalVal", dColumn.ColumnOrdinal);
                    SQLiteParameter columnNameParam = new SQLiteParameter(@"columnNameVal", dColumn.Name);
                    SQLiteParameter taskLimitParam = new SQLiteParameter(@"taskLimitVal", dColumn.TaskLimit);

                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(columnOrdinalParam);
                    command.Parameters.Add(columnNameParam);
                    command.Parameters.Add(taskLimitParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Info($"Succesfully inserted to database the column of the board. ColumnOrdinal:'{dColumn.ColumnOrdinal}', BoardId:'{dColumn.Id}'");
                }
                catch (Exception e)
                {
                    log.Error($"Insertion failed for column of the board. ColumnOrdinal:'{dColumn.ColumnOrdinal}', BoardId:'{dColumn.Id}'. cause: {e}");
                    //throw new ArgumentException(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        ///<summary>
        ///This method loads all columns from the database that belongs to the input board id.
        ///</summary>
        /// <param name="boardId">The id of the board of the columns we want</param>
        /// <exception cref="System.ArgumentException">Thrown when the load fails.</exception>
        ///<returns>return a list of all the DColumn relevant to the board</returns>
        public List<DColumn> LoadBoardColumns(int boardId)
        {
            List<DColumn> results = new List<DColumn>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} where {DColumn.IDColumnName} = {boardId};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add((DColumn)ConvertReaderToObject(dataReader));
                    }
                    log.Info($"Succesfully loaded to columns which are in board {boardId}");
                }
                catch (Exception e)
                {
                    log.Error($"Load failed for columns which are in board {boardId}");
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

        ///<summary>
        ///This method updates a specific value in the database according to boardId, columnOrdinal, table's column and (int)value
        ///</summary>
        /// <param name="boardId">The id of the board of the tasks we want</param>
        /// <param name="columnOrdinal">The ordinal of the column, by it, we can get to the specified line</param>
        /// <param name="columnName">The string of the column we want to replace the value in</param>
        /// <param name="value">The (int) value we want to update to</param>
        /// <exception cref="System.ArgumentException">Thrown when the update fails.</exception>
        ///<returns>return a boolean value on if lines in the database has changed</returns>
        public bool Update(int boardId, int columnOrdinal, string columnName,string nameOfColumn, int value)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                //UPDATE Columns SET columnOrdinal=1 WHERE ID=0 AND columnOrdinal=0
                //command.CommandText = $"UPDATE Columns SET columnOrdinal=1 WHERE ID=0 AND columnOrdinal=0 and name = {nameOfColumn}";
                command.CommandText = $"UPDATE {ColumnsTableName} SET {columnName}={value} WHERE {DTO.IDColumnName}={boardId} AND {DColumn.ColumnOrdinalColumn} = {columnOrdinal} AND name = @noc";
                try
                {
                    command.Parameters.Add(new SQLiteParameter(columnName, value));
                    command.Parameters.Add(new SQLiteParameter("noc", nameOfColumn));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Succesfully updated the columns database for column {columnOrdinal} in board {boardId}. updated in column {columnName} with the value {value}");
                }
                catch (Exception e)
                {
                    log.Error($"Update failed for column {columnOrdinal} in board {boardId} with the value {value}.");
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

        ///<summary>
        ///This method updates a specific value in the database according to boardId, columnOrdinal, table's column and (string)value
        ///</summary>
        /// <param name="boardId">The id of the board of the tasks we want</param>
        /// <param name="columnOrdinal">The ordinal of the column, by it, we can get to the specified line</param>
        /// <param name="columnName">The string of the column we want to replace the value in</param>
        /// <param name="value">The (string) value we want to update to</param>
        /// <exception cref="System.ArgumentException">Thrown when the update fails.</exception>
        ///<returns>return a boolean value on if lines in the database has changed</returns>
        public bool Update(int boardId, int columnOrdinal, string columnName, string value)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"UPDATE {ColumnsTableName} set {columnName}=@{columnName} where {DTO.IDColumnName}={boardId} AND {DColumn.ColumnOrdinalColumn} = {columnOrdinal}";
                try
                {
                    command.Parameters.Add(new SQLiteParameter(columnName, value));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Succesfully updated the columns database for column {columnOrdinal} in board {boardId}. updated in column {columnName} with the value {value}");
                }
                catch (Exception e)
                {
                    log.Error($"Update failed for column {columnOrdinal} in board {boardId} with the value {value}.");
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

        ///<summary>
        ///This method converts all loaded data to a DTask object
        ///</summary>
        /// <param name="reader">The SQLite Data Reader which holds all the data from the specific line in the database</param>
        ///<returns>return DTask with all the information from the reader</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            DColumn result = new DColumn(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3));
            return result;
        }

        public bool DeleteC(DTO DTOObj)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where ID={DTOObj.Id} and columnOrdinal = {((DColumn)DTOObj).ColumnOrdinal}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Succssesfully delete {DTOObj}");
                }
                catch (Exception e)
                {
                    log.Error($"Faild to delete {DTOObj} , cause {e}");
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
    }
}
