using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataTypes.UserManagement
{
    public class Team : INotifyPropertyChanged
    {
        private string _name;
        private Color _color;
        private Color _secondaryColor;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        
        }
        public Color color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged("color");
            }

        }
        public Color secondaryColor
        {
            get { return _secondaryColor; }
            set
            {
                _secondaryColor = value;
                OnPropertyChanged("secondaryColor");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Team(string name, Color color, Color secondaryColor)
        {
            _name = name;
            _color = color;
            _secondaryColor = secondaryColor;
        }

        override public string ToString()
        {
            return String.Format("{0}({1})", Name, color.ToString());
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}