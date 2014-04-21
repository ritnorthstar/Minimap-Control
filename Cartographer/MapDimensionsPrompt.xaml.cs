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
using Xceed.Wpf.Toolkit;

namespace Cartographer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MapDimensionsPrompt : Window
    {
        private double width = 0, height = 0;

        public MapDimensionsPrompt()
        {
            InitializeComponent();
        }

        public double MapWidth
        {
            get { return width; }
            set
            {
                if (value >= 0.1)
                    width = value;
                else
                    width = 0;
            }
        }

        public double MapHeight
        {
            get { return height; }
            set
            {
                if (value >= 0.1)
                    height = value;
                else
                    height = 0;
            }
        }

        public string MapName
        {
            get { return MapNameBox.Text; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WidthSpinner_Spin(object sender, SpinEventArgs e)
        {
            ButtonSpinner spinner = (ButtonSpinner)sender;
            TextBox txtBox = (TextBox)spinner.Content;

            try
            {
                if (e.Direction == SpinDirection.Increase)
                    MapWidth += 0.1;
                else
                    MapWidth -= 0.1;
                txtBox.Text = String.Format("{0:N1}", MapWidth);
            }

            catch (FormatException)
            {
                txtBox.Text = String.Format("{0:N1}", MapWidth);
            }
        }

        private void HeightSpinner_Spin(object sender, SpinEventArgs e)
        {
            ButtonSpinner spinner = (ButtonSpinner)sender;
            TextBox txtBox = (TextBox)spinner.Content;

            try
            {
                if (e.Direction == SpinDirection.Increase)
                    MapHeight += 0.1;
                else
                    MapHeight -= 0.1;
                txtBox.Text = String.Format("{0:N1}", MapHeight);
            }

            catch (FormatException)
            {
                txtBox.Text = String.Format("{0:N1}", MapHeight);
            }
        }

        private void HeightBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MapHeight = Math.Round(Double.Parse((sender as TextBox).Text), 1);
            }

            catch (FormatException)
            {
                (sender as TextBox).Text = String.Format("{0:N1}", MapHeight);
            }
        }

        private void WidthBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MapWidth = Math.Round(Double.Parse((sender as TextBox).Text), 1);
            }

            catch (FormatException)
            {
                (sender as TextBox).Text = String.Format("{0:N1}", MapWidth);
            }
        }
    }
}
