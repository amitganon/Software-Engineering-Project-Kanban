using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /*
`   The main DalBoardController class
    Contains all methods for performing DalBoardController's actions.
    */
    /// <summary>
    /// The main DalBoardController class.
    /// Contains all methods for performing DalBoardController's functions.
    /// </summary>
    /// <remarks>
    /// This class holds all the methodes that performing all the board in the db.
    /// </remarks>
    class DalBoardController : DalController
    {
        private const string MessageTableName = "Boards";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// the constructor of DalBoardController
        /// </summary>
        public DalBoardController() : base(MessageTableName)
        {

        }


        ///<summary>
        ///insert board to db.
        ///</summary>
        ///<param name="board">the DBoard.</param>
        /// <returns>true or false if board was added</returns>
        public bool Insert(DBoard board)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {MessageTableName} ({DTO.IDColumnName} ,{DBoard.BoardNameColumn},{DBoard.BoardCreatorEmailColumn},{DBoard.BoardTaskTopColumn}) " +
                        $"VALUES (@idVal,@boardNameVal,@creatorEmailVal,@TaskTopVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", board.Id);
                    SQLiteParameter NameParam = new SQLiteParameter(@"boardNameVal", board.Name);
                    SQLiteParameter creatorMailParam = new SQLiteParameter(@"creatorEmailVal", board.CreatorEmail);
                    SQLiteParameter taskTopParam = new SQLiteParameter(@"TaskTopVal", board.TaskTop);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(NameParam);
                    command.Parameters.Add(creatorMailParam);
                    command.Parameters.Add(taskTopParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error($"error when try to insert board to DB, board id{board.Id}");
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
        ///loads all the board to business layer.
        ///</summary>
        /// <returns>return list of Dboard</returns>
        public List<DBoard> LoadBoards()
        {
            List<DBoard> result = Select().Cast<DBoard>().ToList();
            log.Info("all the boards where upload from db");
            return result;
        }

        ///<summary>
        ///update board fields in db .
        ///</summary>
        /// <returns>return true or false if update was successful</returns>
        public bool Update(int boardId, string columnName, int value)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                    command.CommandText = $"UPDATE {MessageTableName} set [{columnName}]=@{value} where {DTO.IDColumnName}={boardId}";
                try
                {
                    command.Parameters.Add(new SQLiteParameter(columnName, value));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"database successfully update, board id-{boardId}, column name-{columnName},update value-{value}");
                }
                catch
                {
                    log.Error($"The database failed the operation and was not updated, board id-{boardId}, column name-{columnName},update value-{value}");
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
        /// override ConvertReaderToObject from obstruct class - DalController
        /// </summary>
        /// <param name="reader">SQLiteDataReader reader</param>
        /// <returns>return the Dboard</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            DBoard result = new DBoard(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
            return result;
        }
    }
}
