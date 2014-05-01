using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CartographerLibrary;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;

namespace Cartographer
{
    /// <summary>
    /// Interaction logic for BeaconSettings.xaml
    /// </summary>
    public partial class BeaconSettings : Window
    {
        public BeaconInfo selectedInfo { get; set; }

        public BeaconSettings()
        {
            BeaconInfoList beacons = BeaconInfoManager.Instance().beacons;
            this.DataContext = beacons;
            if (!Application.Current.Resources.Contains("selectedInfo"))
                Application.Current.Resources.Add("selectedInfo", selectedInfo);
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AddBeaconButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(shortIdRegex.IsMatch(ShortIdTextbox.Text) || bluetoothIdRegex.IsMatch(BluetoothIdTextbox.Text)))
                return;

            BeaconInfoManager.Instance().beacons.Add(new BeaconInfo(ShortIdTextbox.Text, BluetoothIdTextbox.Text));
            ShortIdTextbox.Text = String.Empty;
            BluetoothIdTextbox.Text = String.Empty;
        }

        Regex shortIdRegex = new Regex(@"^\d+$");
        Regex bluetoothIdRegex = new Regex(@"^[A-F|\d|:]+$");

        private void ShortID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !shortIdRegex.IsMatch(e.Text);
        }

        private void BluetoothID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !bluetoothIdRegex.IsMatch(e.Text);
        }

        private void DeleteBeacon(object sender, RoutedEventArgs e)
        {
            BeaconInfo oldInfo = selectedInfo.Clone();
            if (BeaconInfoManager.Instance().beacons.Remove(selectedInfo))
            {
                ShortIdTextbox.Text = oldInfo.DeviceLabel;
                BluetoothIdTextbox.Text = oldInfo.DeviceID;
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedInfo = (BeaconInfo)(sender as ListView).SelectedItem;
        }

        private void SaveBeaconInfo(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "Minimap beacon files (*.bcn)|*.bcn";
            dlg.OverwritePrompt = true;
            dlg.DefaultExt = "bcn";
            dlg.InitialDirectory = SettingsManager.ApplicationSettings.InitialDirectory;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog().GetValueOrDefault() != true)
            {
                return;
            }

            System.IO.File.WriteAllText(dlg.FileName, JsonConvert.SerializeObject(BeaconInfoManager.Instance().beacons));
        }

        private void LoadBeaconInfo(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "Minimap beacon files (*.bcn)|*.bcn";
            dlg.DefaultExt = "map";
            dlg.InitialDirectory = SettingsManager.ApplicationSettings.InitialDirectory;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog().GetValueOrDefault() != true)
                return;

            BeaconInfoList openedList = JsonConvert.DeserializeObject<BeaconInfoList>(File.ReadAllText(dlg.FileName));
            BeaconInfoManager manager = BeaconInfoManager.Instance();

            foreach (BeaconInfo b in openedList)
                manager.beacons.Add(b);

            BeaconInfoList list = BeaconInfoManager.Instance().beacons;
            return;
        }
    }


}
