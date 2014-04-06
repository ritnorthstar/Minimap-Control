/*
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataTypes
{
    public class TeamManager
    {
        private static TeamManager manager = null;

        public static TeamManager Instance()
        {
            if (manager == null)
                manager = new TeamManager();
            return manager;
        }

        private Dictionary<Team, List<Judge>> teams;
        public TeamList teamList;

        public List<Team> unusedTeams = new List<Team> {
            new Team("Salamander", Colors.MediumSpringGreen, Colors.MediumSlateBlue),
            new Team("Phoenix", Colors.Gold, Colors.Red),
            new Team("Dragon", Colors.Crimson, Colors.Maroon),
            new Team("Asp", Colors.DarkGreen, Colors.Sienna),
            new Team("Badger", Colors.DimGray, Colors.SeaShell),
            new Team("Tiger", Colors.DarkOrange, Colors.Black),
            new Team("Manticore", Colors.Orange, Colors.DeepSkyBlue),
            new Team("Condor", Colors.SkyBlue,Colors.Peru),
            new Team("Griffin", Colors.DarkTurquoise,Colors.Brown),
            new Team("Scorpion", Colors.SandyBrown,Colors.Maroon),
            new Team("Spider", Colors.Teal,Colors.Purple),
        };

        public static Brush DarkenBrush(SolidColorBrush b)
        {
            Color c = Color.FromRgb((byte)(b.Color.R*.75), (byte)(b.Color.G*.75), (byte)(b.Color.B*.75));

            SolidColorBrush output = b.Clone();
            output.Color = c;

            Console.WriteLine("Input: " + b.ToString() + "; ouptut: " + output.ToString());

            return output;
        }

        public Team GetSampleTeam(int i)
        {
            return i < unusedTeams.Count ? unusedTeams[i] : null;
        }

        private TeamManager()
        {
            teams = new Dictionary<Team, List<Judge>>();
            teamList = new TeamList();
        }

        public int Count
        {
            get { return teams.Keys.Count; }
        }

        public void DrawOnSource(DrawingItemsSource source)
        {
            foreach(List<Judge> judgeList in teams.Values)
                foreach(Judge j in judgeList)
                    source.AddChild(j);
        }

        public List<Judge> Get(Team t)
        {
            if (teams.ContainsKey(t))
                return teams[t];
            return null;
        }

        public void UpdateIdTable(Map map)
        {
            foreach (List<Judge> judgeList in teams.Values)
                foreach (Judge j in judgeList)
                    if (!map.IdTable().ContainsKey(j.guid))
                        map.IdTable()[j.guid] = j;
        }

        public void Add(Judge j, Team t)
        {
            if(!teams.ContainsKey(t))
            {
                teams.Add(t, new List<Judge>());
                teamList.Add(t);
            }
            j.team = t;
            teams[t].Add(j);

        }

        public bool Remove(Judge j, Team t)
        {
            if (!teams[t].Contains(j))
                return false;
            teams[t].Remove(j);
            j.team = null;
            return true;
        }

        public bool Remove(Judge j)
        {
            foreach (Team t in teams.Keys)
                if (Remove(j, t))
                    return true;

            return false;
        }

        public void ChangeTeam(Judge j, Team from, Team to)
        {
            if(!Remove(j, from)) return;
            Add(j, to);
        }
    }

    public class TeamList : ObservableCollection<Team>
    {
        public TeamList() : base() { }
    }
}
*/