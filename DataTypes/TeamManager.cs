using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private TeamManager()
        {
            teams = new Dictionary<Team, List<Judge>>();
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
                teams.Add(t, new List<Judge>());
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
}
