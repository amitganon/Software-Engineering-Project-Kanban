using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /*
`   The main DMember class
    Contains all methods for performing DMember's actions.
    */
    /// <summary>
    /// The main DBoard class.
    /// Contains all methods for performing DMember's actions.
    /// </summary>
    /// <remarks>
    /// this class performing the object-board in the db.
    /// </remarks>
    class DMember : DTO
    {
        public const string MemberEmailColumn = "UserEmail";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private string userEmail;
        public string UserEmail { get {return userEmail; } }

        /// <summary>the DMember constructor</summary>
        public DMember(int boardId, String userEmail) : base(new DalMemberController(), boardId)
        {
            this.userEmail = userEmail;
            log.Info($"DMember created, board id-{boardId} , userEmail-{userEmail}");
        }

        /// <summary>
        /// insert board member to db.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown if error accured when try to insert member to DB.</exception>
        /// <returns>true or false if member was added</returns>
        public bool Insert()
        {
            if (((DalMemberController)_controller).Insert(this)) {
                log.Info($"DMember added to DB, board id-{Id} , userEmail-{userEmail}");
                return true;
            }
            throw new ArgumentException($"cant insert Dmember,board id-{Id} , userEmail-{userEmail}");
        }


        ///<summary>
        ///delete member from db.
        ///</summary>
        ///<exception cref="System.ArgumentException">Thrown if error accured when try to delete member from DB.</exception>
        /// <returns>true or false if member was deleted</returns>
        public bool Delete()
        {
            if (((DalMemberController)_controller).Delete(this)) {
                log.Info($"DMember removed from DB, board id-{Id} , userEmail-{userEmail}");
                return true;
            }
             log.Error($"exception in Delet Dmember from the db, board id-{Id}, user email-{userEmail}");
             throw new ArgumentException($"cant Delete Dmember,board id-{Id} , userEmail-{userEmail}");
        }

    }
}
