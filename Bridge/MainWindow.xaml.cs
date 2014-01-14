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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataTypes;
using Newtonsoft.Json;
using System.IO;


namespace Bridge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point currentPoint = new Point();
        Point offset;
        Map map;

        public MainWindow()
        {
            InitializeComponent();
            //makeMap();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs args)
        {
            Console.WriteLine("Event!");
        }

        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                offset = e.GetPosition(sender as FrameworkElement);
                Console.WriteLine("Offset: " + offset.ToString());
                currentPoint = e.GetPosition(this);
            }
        }

        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Line line = new Line();

                line.Stroke = SystemColors.WindowFrameBrush;
                line.X1 = currentPoint.X;
                line.Y1 = currentPoint.Y;
                line.X2 = e.GetPosition(this).X;
                line.Y2 = e.GetPosition(this).Y;

                currentPoint = e.GetPosition(this);

                canvas.Children.Add(line);
            }
        }

        private void makeMap()
        {
            string output = String.Empty;

            Map map = new Map("Gordon Field House");
            map.tables.Add(new DataTypes.TableRow(20, 20, 80, 30));
            map.tables.Add(new DataTypes.TableRow(20, 50, 80, 60));
            map.tables.Add(new DataTypes.TableRow(20, 80, 80, 90));

            output = JsonConvert.SerializeObject(map);

            Console.WriteLine(output);
        }

        private void OpenMapDialog(object sender, ExecutedRoutedEventArgs args)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();

            openDialog.DefaultExt = ".map";
            openDialog.Filter = "Minimap map files (*.map)|*.map";
            
            Nullable<bool> result = openDialog.ShowDialog();

            if(result == true)
            {
                string filename = openDialog.FileName;
                Console.WriteLine("File selected: " + filename);

                string json = File.ReadAllText(filename);
                map = JsonConvert.DeserializeObject<Map>(json);
                Console.WriteLine("Name: " + map.name);
                Console.WriteLine("Number of tables: " + ((HashSet<DataTypes.TableRow>)map.tables).Count);

            }
        }
    }
}
