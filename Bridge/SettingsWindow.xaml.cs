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
using Server.Hosting;

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
            unusedTeams = Team.GetDefaultTeams();
            teams = getTeams();
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

        private void RefreshTeamMembers(object sender, RoutedEventArgs e)
        {
            CurrentTeamMembers.Children.Clear();
            
            foreach (UserObject judge in Minimap.UserManager().GetAll().Where(u => u.TeamId.Equals(selectedTeam.Id)))//for each user in team
            {
                TextBlock judgeLabel = new TextBlock();
                judgeLabel.Text = judge.Name;
                judgeLabel.FontSize = 14;

                CurrentTeamMembers.Children.Add(judgeLabel);
            }
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
        
        private void SetPortButton_Click(object sender, RoutedEventArgs e)
        {
            WebAPIServer server = WebAPIServer.Instance();

            try
            {
                server.Port = Int32.Parse(PortNumberBox.Text);
                PortMessage.Text = "Port successfully set";
            }

            catch (Exception)
            {
                PortMessage.Text = "Bad port number";
            }
        }

        private void PopulateBeaconData(object sender, RoutedEventArgs e)
        {
            Thickness margin = new Thickness(0, 0, 10, 0);

            for (int i = 0; i < SharedDataManager.Beacons.Count; i++)
            {
                BeaconInfo b = SharedDataManager.Beacons[i];
                Grid layout = new Grid();
                ColumnDefinition labelCol = new ColumnDefinition();
                labelCol.Width = new GridLength(1, GridUnitType.Star);
                ColumnDefinition idCol = new ColumnDefinition();
                idCol.Width = new GridLength(7, GridUnitType.Star);
                layout.ColumnDefinitions.Add(labelCol);
                layout.ColumnDefinitions.Add(idCol);

                TextBlock label = new TextBlock();
                label.Text = b.DeviceLabel;
                label.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                label.Margin = margin;

                TextBlock id = new TextBlock();
                id.Text = b.DeviceID;

                Grid.SetRow(label, i);
                Grid.SetRow(id, i);
                Grid.SetColumn(label, 0);
                Grid.SetColumn(id, 1);

                layout.Children.Add(label);
                layout.Children.Add(id);

                BeaconIDs.Children.Add(layout);
            }
        }

        private void ListTeamSelected(object sender, SelectionChangedEventArgs e)
        {
            selectedTeam = (Team)(sender as ListView).SelectedItem;
            if (selectedTeam == null)
                return;
            TeamInfo.Visibility = Visibility.Visible;
            TeamName.Text = selectedTeam.Name;
            PrimaryColorPicker.SelectedColor = selectedTeam.PrimaryColor;
            SecondaryColorPicker.SelectedColor = selectedTeam.SecondaryColor;

            saved = true;
            SaveButton.ToolTip = "No changes to save";
            SaveButton.IsEnabled = false;

            RefreshTeamMembers(null, null);
        }

    }
}
