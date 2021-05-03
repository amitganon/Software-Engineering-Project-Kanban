using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class DBoardController //public for testing
    {
        private const string TableName = "Board";
        private const string BoardId = "Id";
        private const string BoardName = "Name";
        private const string BoardCreatoreEmail = "creatoreEmail";
        private const string BoardTaskTop = "taskTop";
        private const string BoardUserId = "userId";
        readonly string connectionString;
        SQLiteCommand command;
        string path;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DBoardController()
        {
            path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "DB.db"));
            connectionString = $"Data Sourace={path}; Version=3; ";
        }

        public bool Insert(DBoard board)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName}({BoardId}, {BoardName}, {BoardCreatoreEmail}, {BoardTaskTop}, {BoardUserId})" +
                            $"VALUES(@Idval, @NameVal, @creatorEmailVal, @taskTopVal, @userIdVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"Idval", board.getId);
                    SQLiteParameter nameParam = new SQLiteParameter(@"NameVal", board.getName);
                    SQLiteParameter creatorEmailParam = new SQLiteParameter(@"creatorEmailVal", board.getCreatorEmail);
                    SQLiteParameter taskTopParam = new SQLiteParameter(@"taskTopVal", board.getTaskTop);
                    SQLiteParameter userIdParam = new SQLiteParameter(@"userIdVal", board.getUserId);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(creatorEmailParam);
                    command.Parameters.Add(taskTopParam);
                    command.Parameters.Add(userIdParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    log.Info($"new board was added to the DB, creator email-{board.getCreatorEmail}, board name-{board.getName}");
                }
                catch (Exception e)
                {
                    log.Error($"the error-{e}, board creatore email-{board.getCreatorEmail}, board name-{board.getName}");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }
        
        public bool Remove(DBoard board)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE from {TableName} where {BoardName} = @NameVal and {BoardCreatoreEmail} = @creatorEmailVal";
                    SQLiteParameter nameParam = new SQLiteParameter(@"NameVal", board.getName);
                    SQLiteParameter creatorEmailParam = new SQLiteParameter(@"creatorEmailVal", board.getCreatorEmail);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(creatorEmailParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    log.Info($"a board was removed from the DB, creator email-{board.getCreatorEmail}, board name-{board.getName}");
                }
                catch (Exception e)
                {
                    log.Error($"the error-{e}, board creatore email-{board.getCreatorEmail}, board name-{board.getName}");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        public List<DBoard> GetAllBoards()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                List<DBoard> result = new List<DBoard>(); ;
                int res = -1;
                command = new SQLiteCommand(null, connection);
                SQLiteDataReader dataReader;
                try
                {
                    connection.Open();
                    command.CommandText = $"SELECT * from {TableName}";
                    command.Prepare();
                    dataReader = command.ExecuteReader();
                    res = command.ExecuteNonQuery();
                    while (dataReader.Read())
                    {
                        DBoard temp = new DBoard((int)dataReader.GetValue(0), (string)dataReader.GetValue(1), (string)dataReader.GetValue(2), (int)dataReader.GetValue(3), (int)dataReader.GetValue(4));
                        result.Add(temp);
                    }
                    log.Info($"the method GetAllBoards from DB was succcessful");
                }
                catch (Exception e)
                {
                    log.Error(e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }                
                return result;
            }
        }
        public bool UpdateTaskTop(DBoard board, int max)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"UPDATE {TableName} set {BoardTaskTop} ={max+1} where {BoardName} = @NameVal and {BoardCreatoreEmail} = @creatorEmailVal";
                    SQLiteParameter nameParam = new SQLiteParameter(@"NameVal", board.getName);
                    SQLiteParameter creatorEmailParam = new SQLiteParameter(@"creatorEmailVal", board.getCreatorEmail);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(creatorEmailParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    log.Info($"the taskTop was update successfuly in the DB,in the board: creator email-{board.getCreatorEmail}, board name-{board.getName}");
                }
                catch (Exception e)
                {
                    log.Error($"the error-{e}, board creatore email-{board.getCreatorEmail}, board name-{board.getName}");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }        
    }
}
