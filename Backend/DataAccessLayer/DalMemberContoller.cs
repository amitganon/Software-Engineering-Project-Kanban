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
`   The main DalMemberController class
    Contains all methods for performing DalMemberController's actions.
    */
    /// <summary>
    /// The main DalMemberController class.
    /// Contains all methods for performing DalMemberController's functions.
    /// </summary>
    /// <remarks>
    /// This class holds all the methodes that performing all the members in the db.
    /// </remarks>
    class DalMemberController : DalController
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string MessageTableName = "Members";


        /// <summary>
        /// the constructor of DalBoardController
        /// </summary>
        public DalMemberController() : base(MessageTableName)
        {
        }


        ///<summary>
        ///insert member to db.
        ///</summary>
        ///<param name="board">the DMember.</param>
        /// <returns>true or false if member was added</returns>
        public bool Insert(DMember member)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {MessageTableName} ({DTO.IDColumnName} ,{DMember.MemberEmailColumn}) " +
                        $"VALUES (@idVal,@userEmailVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", member.Id);
                    SQLiteParameter mailParam = new SQLiteParameter(@"userEmailVal", member.UserEmail);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(mailParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error($"error when try to insert member to DB, board id{member.Id}, member email-{member.UserEmail}");
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
        ///loads all the members to business layer.
        ///</summary>
        /// <returns>return list of DMmeber</returns>
        public List<DMember> LoadBoardMembers()
        {
            List<DMember> result = Select().Cast<DMember>().ToList();
            return result;
        }


        /// <summary>
        /// override ConvertReaderToObject from obstruct class - DalController
        /// </summary>
        /// <param name="reader">SQLiteDataReader reader</param>
        /// <returns>return the DMember</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            Console.WriteLine();
            DMember result = new DMember(reader.GetInt32(0), reader.GetString(1));
            return result;
        }
    }
}
