using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /*
`   The main boardController class
    Contains all methods for performing boardController's actions.
    */
    /// <summary>
    /// The main boardController class.
    /// Contains all methods for performing boardController's functions.
    /// </summary>
    /// <remarks>
    /// This class holds the list of all the boards in the system, the class maintain this list(add, delete, get).
    /// </remarks>
    

    public class BoardController
    {
        private Dictionary<(string, string), Board> dict;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        ///<summary>the constructor of boardController.</summary>
        public BoardController()
        {
            dict = new Dictionary<(string, string), Board>();
        }


        ///<summary>
        ///Gets a specific board by key.
        ///</summary>
        ///<param name="creatorEmail">the email's creator of the board.</param>
        ///<param name="boardName">the name of the board.</param>
        ///<exception cref="System.AggregateException">Thrown when creatorEmail input is null.</exception>
        ///<exception cref="System.AggregateException">Thrown when boardName input is null.</exception>
        ///<exception cref="System.AggregateException">Thrown when this board's key not in the system.</exception>
        public Board GetBoard(string creatorEmail, string boardName)
        {
            if (boardName == null)
            {
                log.Error("boardName input was null");
                throw new ArgumentNullException("board name can't be null");
            }
            if (creatorEmail == null)
            {
                log.Error("creatorEmail input was null");
                throw new ArgumentNullException("boardCreator name can't be null");
            }
            (string, string) boardKey = (creatorEmail, boardName);
            if (!dict.ContainsKey(boardKey))
            {
                log.Error($"can't return the board beacouse this specific board's key does not contains in boardController. creatorEmail-{creatorEmail}, boardName-{boardName}");
                throw new ArgumentException("this specific board's key does not contains in the system");
            }
            return dict[boardKey];
        }


        ///<summary>
        ///add a new board to boardController.
        ///</summary>
        ///<param name="board">the board to add.</param>
        ///<exception cref="System.AggregateException">Thrown when board input is null.</exception>
        public void AddBoard(Board board)
        {
            if (board == null)
            {
                log.Error("board input was null");
                throw new ArgumentNullException("board can't be null");
            }
            (string, string) boardKey = (board.GetName(), board.creatorEmail);
            if (dict.ContainsKey(boardKey))
            {
                log.Error($"can't add this board beacouse this specific board's key contains in the system");
                throw new ArgumentException("this board already exsist in the system");
            }
            dict.Add(boardKey,board);
            log.Info($"this board was added to boardController,creatorEmail-{board.creatorEmail}, boardName-{board.GetName()}");
        }


        ///<summary>
        ///remove board from boardController.
        ///</summary>
        ///<param name="board">the board to add.</param>
        ///<exception cref="System.AggregateException">Thrown when board input is null.</exception>
        public void RemoveBoard(Board board)
        {
            if (board == null)
            {
                log.Error("board input was null");
                throw new ArgumentNullException("board can't be null");
            }
            (string, string) boardKey = (board.GetName(), board.creatorEmail);
            if (!dict.ContainsKey(boardKey))
            {
                log.Error($"can't remove this board beacouse this specific board's key does not contains in boardController. creatorEmail-{board.creatorEmail}, boardName-{board.GetName}");
                throw new ArgumentException("this board not exsist in the system");
            }
            dict.Remove(boardKey);
            log.Info($"this board removed from boardController,creatorEmail-{board.creatorEmail}, boardName-{board.GetName()}");
        }
    }
}
