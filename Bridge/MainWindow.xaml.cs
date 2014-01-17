﻿using System;
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
using System.IO;


namespace Bridge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point currentPoint = new Point(-1, -1);
        Map activeMap;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs args)
        {
            Console.WriteLine("Event!");
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                currentPoint = e.GetPosition(sender as FrameworkElement);
            }
        }
        
        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            currentPoint.X = -1;
            currentPoint.Y = -1;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && currentPoint.X != -1)
            {
                Line line = new Line();

                line.Stroke = SystemColors.WindowFrameBrush;
                line.X1 = currentPoint.X;
                line.Y1 = currentPoint.Y;
                line.X2 = e.GetPosition(sender as FrameworkElement).X;
                line.Y2 = e.GetPosition(sender as FrameworkElement).Y;

                currentPoint = e.GetPosition(sender as FrameworkElement);

                canvas.Children.Add(line);
            }
        }

        private void OpenMapDialog(object sender, ExecutedRoutedEventArgs args)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();

            openDialog.DefaultExt = ".map";
            openDialog.Filter = "Minimap map files (*.map)|*.map";
            
            Nullable<bool> result = openDialog.ShowDialog();

            if (result == false) return; // TODO: throw an exception

            string filename = openDialog.FileName;
            Console.WriteLine("File selected: " + filename);

            activeMap = Map.FromFile(filename);
            Console.WriteLine("Name: " + activeMap.name);
            Console.WriteLine("Number of tables: " + ((HashSet<DataTypes.TableBlock>)activeMap.tables).Count);
            canvas.Children.Clear();
            activeMap.DrawSelf(canvas);
        }

        private void QuitProgram(object sender, ExecutedRoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

        bool serverIsRunning = false; // TODO: use an actual server class
        const string SERVER_STOPPED = "Stopped", SERVER_RUNNING = "Running", SERVER_PAUSED = "Paused";

        private void ToggleRunningStatus(object sender, RoutedEventArgs args)
        {
            bool success = false;
            string status;

            if(serverIsRunning)
            {
                serverIsRunning = false; // pause server
                success = true;
                status = SERVER_PAUSED;
                toggleRunningMenuItem.Header = "Continue";
            }

            else
            {
                serverIsRunning = true; // resume server
                success = true;
                status = SERVER_RUNNING;
                toggleRunningMenuItem.Header = "Pause";
            }

            if(success)
            {
                runningStatusItem.Text = status;
            }
        }

        private void RestartServer(object sender, RoutedEventArgs args)
        {
            serverIsRunning = false;
            runningStatusItem.Text = SERVER_STOPPED;
            toggleRunningMenuItem.Header = "Start";
        }

        private void LaunchAboutWindow(object sender, RoutedEventArgs args)
        {
            AboutWindow aboutWindow = new AboutWindow { Owner = this };
            aboutWindow.ShowDialog();
        }

        private void ResetZoomWindow(object sender, MouseButtonEventArgs args)
        {
            mapCanvasScaleSlider.Value = 1.0;
        }

        private void ClearScreen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
        }

    }
}
