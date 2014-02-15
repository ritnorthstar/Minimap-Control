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
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using Northstar.Minimap.Control.Server.Host;
using System.Collections.Specialized;
using System.Net;


namespace Bridge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Map activeMap;
        ZoomableCanvas zoomCanvas;
        private DrawingItemsSource source;
        private WebAPIServer server;

        public MainWindow()
        {
            InitializeComponent();
            source = new DrawingItemsSource();
            ListboxContainer.ItemsSource = source;
            Console.WriteLine("Hooked up item source; num items: " + source.Count);

            server = new WebAPIServer();
        }

        private void QuitProgram(object sender, ExecutedRoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }
        
        #region Dialog launching

        private void LaunchAboutWindow(object sender, RoutedEventArgs args)
        {
            AboutWindow aboutWindow = new AboutWindow { Owner = this };
            aboutWindow.ShowDialog();
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

            source.ClearChildren();
            activeMap.DrawOn(source);
            source.AddChild(new DebugRect(0, 0, source.Extent.Width, source.Extent.Height));
        }

        #endregion
        
        #region UI handling

        #region Canvas

        Point LastMousePosition;

        private void ZoomableCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            // Store the canvas in a local variable since x:Name doesn't work.
            zoomCanvas = (ZoomableCanvas)sender;
        }

        private void ClearScreen(object sender, RoutedEventArgs e)
        {
            source.ClearChildren();
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            Point position = e.GetPosition(ListboxContainer);

            if (e.LeftButton == MouseButtonState.Pressed && !(e.OriginalSource is Thumb)) // Don't block the scrollbars.
            {
                CaptureMouse();

                Vector delta = position - LastMousePosition;
                double nextX = zoomCanvas.Offset.X - delta.X;
                double nextY = zoomCanvas.Offset.Y - delta.Y;

                if (zoomCanvas.ActualWidth < source.Extent.Width * zoomCanvas.Scale) // viewport is narrower than contents, constrain if necessary
                {
                    if (nextX < 0) nextX = 0;
                    else if (nextX + zoomCanvas.ActualWidth > source.Extent.Width * zoomCanvas.Scale)
                        nextX = source.Extent.Width * zoomCanvas.Scale - zoomCanvas.ActualWidth;
                }

                else { nextX = (zoomCanvas.ActualWidth - source.Extent.Width * zoomCanvas.Scale) / -2; } // viewport is wider than contents, center

                if (zoomCanvas.ActualHeight < source.Extent.Height * zoomCanvas.Scale) // viewport is shorter than contents, constrain if necessary
                {
                    if (nextY < 0) nextY = 0;
                    else if (nextY + zoomCanvas.ActualHeight > source.Extent.Height * zoomCanvas.Scale)
                        nextY = source.Extent.Height * zoomCanvas.Scale - zoomCanvas.ActualHeight;
                }

                else { nextY = (zoomCanvas.ActualHeight - source.Extent.Height * zoomCanvas.Scale) / -2; } // viewport is taller than contents, center

                zoomCanvas.Offset = new Point(nextX, nextY);
                e.Handled = true;
            }
            else { ReleaseMouseCapture(); }

            LastMousePosition = position;
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (zoomCanvas == null) return;
            if (e.NewSize.Width > source.Extent.Width * zoomCanvas.Scale || e.NewSize.Height > source.Extent.Height * zoomCanvas.Scale)
            {
                double nextX = e.NewSize.Width > source.Extent.Width * zoomCanvas.Scale ? (zoomCanvas.ActualWidth - source.Extent.Width * zoomCanvas.Scale) / -2 : zoomCanvas.Offset.X;
                double nextY = e.NewSize.Height > source.Extent.Height * zoomCanvas.Scale ? (zoomCanvas.ActualHeight - source.Extent.Height * zoomCanvas.Scale) / -2 : zoomCanvas.Offset.Y;

                zoomCanvas.Offset = new Point(nextX, nextY);
            }
        }

        private void scaleOnPoint(double factor, Point p)
        {

            double scaleDifference = factor / zoomCanvas.Scale;
            zoomCanvas.Scale = factor;

            // translate to the proper center
            Vector position = (Vector)p;
            zoomCanvas.Offset = (Point)((Vector)(zoomCanvas.Offset + position) * scaleDifference - position);
        }

        private void ListboxContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedObjectText.Text = e.AddedItems[0].ToString();
        }

        #endregion

        #region Menu

        bool serverIsRunning = false;
        const string SERVER_STOPPED = "Stopped", SERVER_RUNNING = "Running", SERVER_PAUSED = "Paused";

        private void RestartServer(object sender, RoutedEventArgs args)
        {
            serverIsRunning = false;
            runningStatusItem.Text = SERVER_STOPPED;
            toggleRunningMenuItem.Header = "Start";

            server.Stop();
        }

        private void ToggleRunningStatus(object sender, RoutedEventArgs args)
        {
            bool success = false;
            string status;

            if (serverIsRunning)
            {
                serverIsRunning = false; // pause server
                success = true;
                status = SERVER_PAUSED;
                toggleRunningMenuItem.Header = "Continue";

                server.Stop();
            }

            else
            {
                if(activeMap == null) OpenMapDialog(null, null);
                if (activeMap == null) return; // user cancelled map-opening
                serverIsRunning = true; // resume server
                success = true;
                status = SERVER_RUNNING;
                toggleRunningMenuItem.Header = "Pause";

                server.Start();
            }

            if (success)
            {
                runningStatusItem.Text = status;

                try
                {
                    using (WebClient wb = new WebClient())
                    {
                        NameValueCollection data = new NameValueCollection();
                        data["map"] = activeMap.ToJson();

                        var response = wb.UploadValues("http://127.0.0.1:9000/api/Maps/", "POST", data);
                    }
                }

                catch (WebException e) { Console.WriteLine(e.Data); }
            }
        }
      
        #endregion

        #region Sidebar



        #endregion

        #region Statusbar

        private void ResetZoomWindow(object sender, MouseButtonEventArgs args)
        {
            mapCanvasScaleSlider.Value = 1.0;
        }

        private void mapCanvasScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                scaleOnPoint(e.NewValue, new Point(zoomCanvas.ActualWidth / 2, zoomCanvas.ActualHeight / 2));
                e.Handled = true;
            }
            catch (NullReferenceException) { }
        }

        #endregion

        #endregion
    }
}