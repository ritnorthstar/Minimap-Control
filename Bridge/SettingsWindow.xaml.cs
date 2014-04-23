using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using DataTypes;
using Xceed.Wpf.Toolkit;
using System.Globalization;
using Core.Data;
using Core;

namespace Bridge
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public ObservableCollection<Team> teams { get; set; }
        public Team selectedTeam { get; set; }

        private List<Team> unusedTeams;
        private bool saved = true;

        public SettingsWindow()
        {
            teams = getTeams();
            unusedTeams = Team.GetDefaultTeams();
            this.DataContext = this.teams;
            if (!Application.Current.Resources.Contains("selectedTeam"))
                Application.Current.Resources.Add("selectedTeam", selectedTeam);
            InitializeComponent();
        }

        private ObservableCollection<Team> getTeams()
        {
            IEnumerable<TeamObject> teamObjs = Minimap.TeamManager().GetAll();
            ObservableCollection<Team> copy = new ObservableCollection<Team>();
            foreach (TeamObject team in teamObjs)
            {
                copy.Add(new Team(team));
            }
            return copy;
        }

        private void SettingsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (saved) return;

            MessageBoxResult response = System.Windows.MessageBox.Show("You haven't saved your changes.  Would you like to?", "Close Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (response == MessageBoxResult.Yes)
            {
                saveTeamData();
                e.Cancel = false;
            }
            else if(response == MessageBoxResult.No)
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void ClickAddTeam(object sender, RoutedEventArgs e)
        {
            Team toAdd = unusedTeams.ElementAt(0);
            unusedTeams.RemoveAt(0);
            teams.Add(toAdd);
            Minimap.TeamManager().Add(toAdd);

            if (unusedTeams.Count == 0)
            {
                AddTeamButton.IsEnabled = false;
            }
        }

        private void saveTeamData()
        {
            if (selectedTeam != null)
            {
                selectedTeam.Name = TeamName.Text;
                selectedTeam.PrimaryColor = PrimaryColorPicker.SelectedColor;
                selectedTeam.SecondaryColor = SecondaryColorPicker.SelectedColor;

                Minimap.TeamManager().Add(selectedTeam);
            }
        }

        private void ClickSaveData(object sender, RoutedEventArgs e)
        {
            saveTeamData();
            saved = true;
            SaveButton.ToolTip = "Saved";
            SaveButton.IsEnabled = false;
        }

        private void ListTeamSelected(object sender, RoutedEventArgs e)
        {
            selectedTeam = (Team)(sender as ListView).SelectedItem;
            if(selectedTeam == null)
                return;
            TeamInfo.Visibility = Visibility.Visible;
            TeamName.Text = selectedTeam.Name;
            PrimaryColorPicker.SelectedColor = selectedTeam.PrimaryColor;
            SecondaryColorPicker.SelectedColor = selectedTeam.SecondaryColor;

            saved = true;
            SaveButton.ToolTip = "No changes to save";
            SaveButton.IsEnabled = false;
        }

        private void RefreshTeamMembers(object sender, RoutedEventArgs e)
        {

        }

        private void invalidateSaved()
        {
            saved = false;
            SaveButton.ToolTip = "Save team";
            SaveButton.IsEnabled = true;
        }

        private void ColorSelectionChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            invalidateSaved();
        }

        private void TeamName_TextChanged(object sender, TextChangedEventArgs e)
        {
            invalidateSaved();
        }

        private void DeleteTeam(object sender, RoutedEventArgs e)
        {
            if (Minimap.TeamManager().Remove(selectedTeam.Id))
            {
                unusedTeams.Add(selectedTeam);
                teams.Remove(selectedTeam);
                AddTeamButton.IsEnabled = true;

                TeamName.Text = "";
                PrimaryColorPicker.SelectedColor = Colors.Black;
                SecondaryColorPicker.SelectedColor = Colors.Black;

                saved = true;
                SaveButton.ToolTip = "No changes to save";
                SaveButton.IsEnabled = false;
            }
        }
    }
}
