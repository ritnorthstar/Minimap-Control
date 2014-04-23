using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataTypes
{
    public class Team : TeamObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Team(string name, Color primaryColor, Color secondaryColor)
        {
            Name = name;
            PrimaryColor = primaryColor;
            SecondaryColor = secondaryColor;
        }

        public Team(TeamObject team) : base(team)
        {
            // do nothing
        }

        public new string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public new Color PrimaryColor
        {
            get
            {
                return Color.FromArgb(base.PrimaryColor.A, base.PrimaryColor.R, base.PrimaryColor.G, base.PrimaryColor.B);
            }
            set
            {
                base.PrimaryColor = new TeamColor(value.A, value.R, value.G, value.B);
                OnPropertyChanged("PrimaryColor");
            }
        }

        public new Color SecondaryColor
        {
            get
            {
                return Color.FromArgb(base.SecondaryColor.A, base.SecondaryColor.R, base.SecondaryColor.G, base.SecondaryColor.B);
            }
            set
            {
                base.SecondaryColor = new TeamColor(value.A, value.R, value.G, value.B);
                OnPropertyChanged("SecondaryColor");
            }
        }

        public static List<Team> GetDefaultTeams()
        {
            List<Team> defaultTeams = new List<Team> {
                new Team("Salamander", Colors.MediumSpringGreen, Colors.MediumSlateBlue),
                new Team("Phoenix", Colors.Gold, Colors.Red),
                new Team("Dragon", Colors.Crimson, Colors.Maroon),
                new Team("Asp", Colors.DarkGreen, Colors.Sienna),
                new Team("Badger", Colors.DimGray, Colors.SeaShell),
                new Team("Tiger", Colors.DarkOrange, Colors.Black),
                new Team("Manticore", Colors.Orange, Colors.DeepSkyBlue),
                new Team("Condor", Colors.SkyBlue,Colors.Peru),
                new Team("Griffin", Colors.DarkTurquoise,Colors.Brown)
            };

            return defaultTeams;
        }

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
