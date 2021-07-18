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
   The main DalTaskController class
    Contains all methods for performing changing the database according to Tasks' actions.
    */
    /// <summary>
    /// The main DalTaskController class
    /// Contains all methods for performing changing the database according to Tasks' actions.
    /// </summary>
    /// <remarks>
    /// This class inherit methods from abstract class DalController 
    /// </remarks>
    public class DalTaskController : DalController
    {
        private const string TaskTableName = "Tasks";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DalTaskController() : base(TaskTableName)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        ///<summary>
        ///This method insert a new line of task information to the database
        ///</summary>
        /// <param name="dTask">The object that holds the information about a specific task</param>
        /// <exception cref="System.ArgumentException">Thrown when the insertion fails.</exception>
        ///<returns>return a boolean value on if lines in the database has changed</returns>
        public bool Insert(DTask dTask)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} ({DTO.IDColumnName},{DTask.BoardIdColumnName},{DTask.TaskOrdinalColumnName},{DTask.TaskAssigneeColumnName},{DTask.TaskTitleColumnName},{DTask.TaskDescriptionColumnName},{DTask.TaskCreationTimeColumnName},{DTask.TaskDueDateColumnName}) " +
                        $"VALUES (@idVal,@boardIdVal,@columnOrdinalVal,@assigneeVal,@titleVal,@descriptionVal,@creationTimeVal,@dueDateVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", dTask.Id);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", dTask.BoardId);
                    SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"columnOrdinalVal", dTask.ColumnOrdinal);
                    SQLiteParameter assigneeParam = new SQLiteParameter(@"assigneeVal", dTask.Assignee);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", dTask.TaskTitle);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", dTask.TaskDescription);
                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"creationTimeVal", dTask.TaskCreationTime);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", dTask.TaskDueDate);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(columnOrdinalParam);
                    command.Parameters.Add(assigneeParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(dueDateParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Info($"Succesfully inserted to database the task of the board. TaskId:'{dTask.Id}', BoardId:'{dTask.BoardId}'");
                }
                catch (Exception e)
                {
                    log.Error($"Insertion failed for task of the board. TaskId:'{dTask.Id}', BoardId:'{dTask.BoardId}'. cause: {e}");
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
        ///This method loads all tasks from the database that belongs to the input board id.
        ///</summary>
        /// <param name="boardId">The id of the board of the tasks we want</param>
        /// <exception cref="System.ArgumentException">Thrown when the load fails.</exception>
        ///<returns>return a list of all the DTask relevant to the board</returns>
        public List<DTask> LoadBoardTasks(int boardId)
        {
            List<DTask> results = new List<DTask>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} where {DTask.BoardIdColumnName} = {boardId};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add((DTask)ConvertReaderToObject(dataReader));

                    }
                    log.Info($"Succesfully loaded to tasks which are in board {boardId}");
                }
                catch (Exception e)
                {
                    log.Error($"Load failed for tasks which are in board {boardId}");
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
        ///This method loads all tasks from the database that belongs to the input column ordinal and by board id.
        ///</summary>
        /// <param name="boardId">The id of the board of the tasks we want</param>
        /// <param name="columnOrdinal">The ordinal of the column of the tasks we want</param>
        /// <exception cref="System.ArgumentException">Thrown when the load fails.</exception>
        ///<returns>return a list of all the DTask relevant to the board and column</returns>
        public List<DTask> LoadColumnTasks(int boardId , int columnOrdinal) //TAL-CHANGE
        {
            List<DTask> results = new List<DTask>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} where {DTask.BoardIdColumnName} = {boardId} and {DTask.TaskOrdinalColumnName} = {columnOrdinal} ;";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add((DTask)ConvertReaderToObject(dataReader));

                    }
                    log.Info($"Succesfully loaded to tasks to column {columnOrdinal} in board {boardId}");
                }
                catch (Exception e)
                {
                    log.Error($"Load failed for tasks to column {columnOrdinal} in board {boardId}");
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
        ///This method updates a specific value in the database according to line, column and (int)value
        ///</summary>
        /// <param name="taskId">represent the id of the task, by it, we can get to the specified line</param>
        /// <param name="boardId">The id of the board of the tasks we want</param>
        /// <param name="columnName">The string of the column we want to replace the value in</param>
        /// <param name="value">The (int) value we want to update to</param>
        /// <exception cref="System.ArgumentException">Thrown when the update fails.</exception>
        ///<returns>return a boolean value on if lines in the database has changed</returns>
        public bool Update(int taskId, int boardId, string columnName, int value)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"UPDATE {TaskTableName} set [{columnName}]={value} where {DTO.IDColumnName}={taskId} AND {DTask.BoardIdColumnName} = {boardId}";
                try
                {
                    command.Parameters.Add(new SQLiteParameter(columnName, value));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Succesfully updated the tasks database for task {taskId} in column {columnName} with the value {value}");
                }
                catch (Exception e)
                {
                    log.Error($"Update failed for task {taskId} in column {columnName} with the value {value}");
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
        ///This method updates a specific value in the database according to line, column and (string)value
        ///</summary>
        /// <param name="taskId">represent the id of the task, by it, we can get to the specified line</param>
        /// <param name="columnName">The string of the column we want to replace the value in</param>
        /// <param name="value">The (string) value we want to update to</param>
        /// <exception cref="System.ArgumentException">Thrown when the update fails.</exception>
        ///<returns>return a boolean value on if lines in the database has changed</returns>
        public bool Update(int taskId, string columnName, string value)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"UPDATE {TaskTableName} set [{columnName}]=@{columnName} where {DTO.IDColumnName}={taskId}";
                try
                {
                    command.Parameters.Add(new SQLiteParameter(columnName, value));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Succesfully updated the tasks database for task {taskId} in column {columnName} with the value {value}");
                }
                catch (Exception e)
                {
                    log.Error($"Update failed for task {taskId} in column {columnName} with the value {value}");
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
            DTask result = new DTask(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), DateTime.Parse(reader.GetString(6)), DateTime.Parse(reader.GetString(7)));
            return result;
        }
    }
}
