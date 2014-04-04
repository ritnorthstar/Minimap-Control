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

namespace Bridge
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public TeamList teams { get; set; }
        public Team selectedTeam { get; set; }
        private bool saved = true;

        public SettingsWindow()
        {
            teams = TeamManager.Instance().teamList;
            this.DataContext = this.teams;
            if (!Application.Current.Resources.Contains("selectedTeam"))
                Application.Current.Resources.Add("selectedTeam", selectedTeam);
            InitializeComponent();
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
            TeamManager manager = TeamManager.Instance();
            Team toAdd = manager.GetSampleTeam(teams.Count);
            Console.WriteLine(toAdd.ToString());
            
            teams.Add(toAdd);
            
            if(teams.Count == manager.sampleTeams.Count)
                (sender as Button).IsEnabled = false;
        }

        private void saveTeamData()
        {
            selectedTeam.name = TeamName.Text;
            selectedTeam.color = PrimaryColorPicker.SelectedColor;
            selectedTeam.secondaryColor = SecondaryColorPicker.SelectedColor;
        }

        private void ClickSaveData(object sender, RoutedEventArgs e)
        {
            saveTeamData();
            saved = true;
            SaveButton.Content = "Saved";
            SaveButton.IsEnabled = false;
        }

        private void ListTeamSelected(object sender, RoutedEventArgs e)
        {
            selectedTeam = (Team)(sender as ListView).SelectedItems[0];
            TeamInfo.Visibility = Visibility.Visible;
            TeamName.Text = selectedTeam.name;
            PrimaryColorPicker.SelectedColor = selectedTeam.color;
            SecondaryColorPicker.SelectedColor = selectedTeam.secondaryColor;

            saved = true;
            SaveButton.Content = "Unchanged";
            SaveButton.IsEnabled = false;
        }

        private void RefreshTeamMembers(object sender, RoutedEventArgs e)
        {

        }

        private void invalidateSaved()
        {
            saved = false;
            SaveButton.Content = "Save Team";
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
    }
}
