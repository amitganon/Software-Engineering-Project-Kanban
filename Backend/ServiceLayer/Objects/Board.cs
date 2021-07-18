using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Board
    {
        private readonly int id;
        private readonly string name;
        private readonly string creatorEmail;
        private readonly Dictionary<int, Column> columnDict;
        private readonly IList<string> membersList;

        internal Board(BusinessLayer.Board board)
        {
            this.id = board.Id;
            this.name = board.Name;
            this.creatorEmail = board.CreatorEmail;
            this.membersList = board.MembersList;
            this.columnDict = new Dictionary<int, Column>();
            Dictionary<int, BusinessLayer.Column> businessColumnDict = board.ColumnDict;
            int columnAmount = businessColumnDict.Count;
            for (int i = 0; i < columnAmount; i++)
                columnDict.Add(i, new Column(businessColumnDict.GetValueOrDefault(i)));
        }
        public Board(int s)
        {
            this.id = 1;
            this.name = "sadsfdfadf";
            this.creatorEmail = "sasfadsgfass";
            this.membersList = new List<String>();
            this.columnDict = new Dictionary<int, Column>();
        }
        public int Id { get => id; }
        public string Name { get => name; }
        public string CreatorEmail { get => creatorEmail; }
        public Dictionary<int, Column> ColumnDict { get => columnDict; }
        public IList<string> MembersList { get => membersList; }

    }
}
