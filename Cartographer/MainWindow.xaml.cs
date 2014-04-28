//#define  NO_PROMPT_TO_SAVE

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;
using Microsoft.Win32;
using System.Windows.Media.Effects;
using System.Diagnostics;
using Xceed.Wpf.Toolkit;
using CartographerLibrary;
using CartographerUtilities;
using System.Text.RegularExpressions;

namespace Cartographer
{
    /// <summary>
    /// Main application window
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        #region Class Members

        WindowStateManager windowStateManager;
        MruManager mruManager;
        string fileName;    // name of currently opened file
        public BeaconInfo selectedInfo { get; set; }

        #endregion Class Members

        #region Constructor

        public MainWindow()
        {
            // Create WindowStateManager and associate is with ApplicationSettings.MainWindowStateInfo.
            // This allows to set initial window state and track state changes in
            // the Settings.MainWindowStateInfo instance.
            // When application is closed, ApplicationSettings is saved with new window state
            // information. Next time this information is loaded from XML file.
            windowStateManager = new WindowStateManager(SettingsManager.ApplicationSettings.MainWindowStateInfo, this);

            BeaconInfoList beacons = BeaconInfoManager.Instance().beacons;
            this.DataContext = beacons;
            if (!Application.Current.Resources.Contains("selectedInfo"))
                Application.Current.Resources.Add("selectedInfo", selectedInfo);

            InitializeComponent();
            SubscribeToEvents();
            UpdateTitle();
            InitializeDrawingCanvas();
            InitializeMruList();
        }


        #endregion Constructor

        #region Application Commands

        /// <summary>
        /// New file
        /// </summary>
        void FileNewCommand(object sender, ExecutedRoutedEventArgs args)
        {
            if (!PromptToSave())
            {
                return;
            }

            drawingCanvas.Clear();

            fileName = "";
            UpdateTitle();
        }

        /// <summary>
        /// Exit
        /// </summary>
        void FileCloseCommand(object sender, ExecutedRoutedEventArgs args)
        {
            this.Close();
        }

        /// <summary>
        /// Open file
        /// </summary>
        void FileOpenMapCommand(object sender, ExecutedRoutedEventArgs args)
        {
            if (!PromptToSave())
            {
                return;
            }

            // Show Open File dialog
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "Minimap maps (*.map)|*.map";
            dlg.DefaultExt = "map";
            dlg.InitialDirectory = SettingsManager.ApplicationSettings.InitialDirectory;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog().GetValueOrDefault() != true)
                return;

            try { drawingCanvas.LoadMap(dlg.FileName); }
            catch (DrawingCanvasException e)
            {
                ShowError(e.Message);
                mruManager.Delete(dlg.FileName);
                return;
            }

            this.fileName = dlg.FileName;
            UpdateTitle();

            // Remember initial directory
            SettingsManager.ApplicationSettings.InitialDirectory = System.IO.Path.GetDirectoryName(dlg.FileName);
        }

        /// <summary>
        /// Open file
        /// </summary>
        void FileOpenMapImageCommand(object sender, ExecutedRoutedEventArgs args)
        {
            if (!PromptToSave())
            {
                return;
            }

            // Show Open File dialog
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "Image files (*.png, *.jpg)|*.png;*.jpg ";
            //dlg.DefaultExt = "xml";
            dlg.InitialDirectory = SettingsManager.ApplicationSettings.InitialDirectory;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog().GetValueOrDefault() != true)
            {
                return;
            }

            try
            {
                // Load file
                Uri uri = new Uri(dlg.FileName);

                BitmapImage source = new BitmapImage(uri);
                imageBackground.Source = source;
                double xScale = ActualWidth * 0.8 / source.Width;
                double yScale = ActualHeight * 0.8 / source.Height;
                drawingCanvas.ActualScale = xScale < yScale ? xScale : yScale;

                OpenMapButton.IsEnabled = true;

                PromptForWidthAndHeight();
            }
            catch (DrawingCanvasException e)
            {
                ShowError(e.Message);
                mruManager.Delete(dlg.FileName);
                return;
            }

            // Remember initial directory
            SettingsManager.ApplicationSettings.InitialDirectory = System.IO.Path.GetDirectoryName(dlg.FileName);
        }

        void PromptForWidthAndHeight()
        {
            MapDimensionsPrompt prompt = new MapDimensionsPrompt { Owner = this };
            prompt.ShowDialog();

            if (prompt.Next == MapDimensionsPrompt.NextState.New)
            {
                drawingCanvas.MapWidth = prompt.MapWidth;
                drawingCanvas.MapHeight = prompt.MapHeight;
                drawingCanvas.MapName = prompt.MapName;
                drawingCanvas.Clear();
            }

            else
            {
                FileOpenMapCommand(null, null);
                drawingCanvas.RefreshClip();
            }


        }

        /// <summary>
        /// Save command
        /// </summary>
        void FileSaveCommand(object sender, ExecutedRoutedEventArgs args)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                FileSaveAsCommand(sender, args);
                return;
            }

            Save(fileName);
        }

        /// <summary>
        /// Save As Command
        /// </summary>
        void FileSaveAsCommand(object sender, ExecutedRoutedEventArgs args)
        {
            // Show Save File dialog
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "Minimap map files (*.map)|*.map";
            dlg.OverwritePrompt = true;
            dlg.DefaultExt = "map";
            dlg.InitialDirectory = SettingsManager.ApplicationSettings.InitialDirectory;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog().GetValueOrDefault() != true)
            {
                return;
            }

            // Save
            if (!Save(dlg.FileName))
            {
                return;
            }

            // Remember initial directory
            SettingsManager.ApplicationSettings.InitialDirectory = System.IO.Path.GetDirectoryName(dlg.FileName);
        }

        /// <summary>
        /// Undo command
        /// </summary>
        void EditUndoCommand(object sender, ExecutedRoutedEventArgs args)
        {
            drawingCanvas.Undo();
        }

        /// <summary>
        /// Redo command
        /// </summary>
        void EditRedoCommand(object sender, ExecutedRoutedEventArgs args)
        {
            drawingCanvas.Redo();
        }

        #endregion ApplicationCommands

        #region Tools Event Handlers

        /// <summary>
        /// One of Tools menu items is clicked.
        /// ToolType enumeration member name is in the item tag.
        /// </summary>
        void ToolMenuItem_Click(object sender, RoutedEventArgs e)
        {
            drawingCanvas.Tool = (ToolType)Enum.Parse(typeof(ToolType), ((MenuItem)sender).Tag.ToString());
        }

        /// <summary>
        /// One of Tools toolbar buttons is clicked.
        /// ToolType enumeration member name is in the button tag.
        /// 
        /// For toolbar buttons I use PreviewMouseDown event instead of Click,
        /// because IsChecked property is not handled by standard
        /// way. For example, every click on the Pointer button keeps it checked
        /// instead of changing state. IsChecked property of every button is bound 
        /// to DrawingCanvas.Tool property.
        /// Using normal Click handler toggles every button, which doesn't
        /// match my requirements. So, I catch click in PreviewMouseDown handler
        /// and set Handled to true preventing standard IsChecked handling.
        /// 
        /// Other way to do the same: handle Click event and set buttons state
        /// at application idle time, without binding IsChecked property.
        /// </summary>
        void ToolButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            drawingCanvas.Tool = (ToolType)Enum.Parse(typeof(ToolType), ((System.Windows.Controls.Primitives.ButtonBase)sender).Tag.ToString());

            e.Handled = true;
        }

        #endregion Tools Event Handlers

        #region Edit Event Handlers

        void menuEditSelectAll_Click(object sender, RoutedEventArgs e)
        {
            drawingCanvas.SelectAll();
        }

        void menuEditUnselectAll_Click(object sender, RoutedEventArgs e)
        {
            drawingCanvas.UnselectAll();
        }

        void menuEditDelete_Click(object sender, RoutedEventArgs e)
        {
            drawingCanvas.Delete();
        }

        void menuEditDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            drawingCanvas.DeleteAll();
        }

        void menuEditMoveToFront_Click(object sender, RoutedEventArgs e)
        {
            drawingCanvas.MoveToFront();
        }

        void menuEditMoveToBack_Click(object sender, RoutedEventArgs e)
        {
            drawingCanvas.MoveToBack();
        }

        void menuEditSetProperties_Click(object sender, RoutedEventArgs e)
        {
            drawingCanvas.SetProperties();
        }


        #endregion Edit Event Handlers

        #region Properties Event Handlers

        /// <summary>
        /// Show Font dialog
        /// </summary>
        void PropertiesFont_Click(object sender, RoutedEventArgs e)
        {
            Petzold.ChooseFont.FontDialog dlg = new Petzold.ChooseFont.FontDialog();
            dlg.Owner = this;
            dlg.Background = SystemColors.ControlBrush;
            dlg.Title = "Select Font";


            dlg.FaceSize = drawingCanvas.TextFontSize;

            dlg.Typeface = new Typeface(
                new FontFamily(drawingCanvas.TextFontFamilyName),
                drawingCanvas.TextFontStyle,
                drawingCanvas.TextFontWeight,
                drawingCanvas.TextFontStretch);



            if (dlg.ShowDialog().GetValueOrDefault() != true)
            {
                return;
            }

            // Set new font in drawing canvas
            drawingCanvas.TextFontSize = dlg.FaceSize;
            drawingCanvas.TextFontFamilyName = dlg.Typeface.FontFamily.ToString();
            drawingCanvas.TextFontStyle = dlg.Typeface.Style;
            drawingCanvas.TextFontWeight = dlg.Typeface.Weight;
            drawingCanvas.TextFontStretch = dlg.Typeface.Stretch;

            // Set new font in application settings
            SettingsManager.ApplicationSettings.TextFontSize = dlg.FaceSize;
            SettingsManager.ApplicationSettings.TextFontFamilyName = dlg.Typeface.FontFamily.ToString();
            SettingsManager.ApplicationSettings.TextFontStyle = FontConversions.FontStyleToString(dlg.Typeface.Style);
            SettingsManager.ApplicationSettings.TextFontWeight = FontConversions.FontWeightToString(dlg.Typeface.Weight);
            SettingsManager.ApplicationSettings.TextFontStretch = FontConversions.FontStretchToString(dlg.Typeface.Stretch);
        }

        /// <summary>
        /// Show Color dialog
        /// </summary>
        void PropertiesColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Owner = this;
            dlg.Color = drawingCanvas.ObjectColor;

            if (dlg.ShowDialog().GetValueOrDefault() != true)
            {
                return;
            }

            // Set selected color in drawing canvas
            // and in application settings
            drawingCanvas.ObjectColor = dlg.Color;

            SettingsManager.ApplicationSettings.ObjectColor = drawingCanvas.ObjectColor;
        }

        #endregion Properties Event Handlers

        #region Other Event Handlers


        /// <summary>
        /// File is selected from MRU list
        /// </summary>
        void mruManager_FileSelected(object sender, MruFileOpenEventArgs e)
        {
            if (!PromptToSave())
            {
                return;
            }

            try
            {
                // Load file
                drawingCanvas.LoadMap(e.FileName);
            }
            catch (DrawingCanvasException ex)
            {
                ShowError(ex.Message);
                mruManager.Delete(e.FileName);

                return;
            }

            this.fileName = e.FileName;
            UpdateTitle();
            mruManager.Add(this.fileName);
        }



        /// <summary>
        /// IsDirty is changed
        /// </summary>
        void drawingCanvas_IsDirtyChanged(object sender, RoutedEventArgs e)
        {
            UpdateTitle();
        }


        /// <summary>
        /// Check Tools menu items according to active tool
        /// </summary>
        void menuTools_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            menuToolsPointer.IsChecked = (drawingCanvas.Tool == ToolType.Pointer);
            menuToolsTableBlock.IsChecked = (drawingCanvas.Tool == ToolType.TableBlock);
            menuToolsBeacon.IsChecked = (drawingCanvas.Tool == ToolType.Beacon);
            menuToolsBarrier.IsChecked = (drawingCanvas.Tool == ToolType.Barrier);
        }

        /// <summary>
        /// Enable Edit menu items according to DrawingCanvas stare
        /// </summary>
        void menuEdit_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            menuEditDelete.IsEnabled = drawingCanvas.CanDelete;
            menuEditDeleteAll.IsEnabled = drawingCanvas.CanDeleteAll;
            menuEditMoveToBack.IsEnabled = drawingCanvas.CanMoveToBack;
            menuEditMoveToFront.IsEnabled = drawingCanvas.CanMoveToFront;
            menuEditSelectAll.IsEnabled = drawingCanvas.CanSelectAll;
            menuEditUnselectAll.IsEnabled = drawingCanvas.CanUnselectAll;
            menuEditSetProperties.IsEnabled = drawingCanvas.CanSetProperties;
            menuEditUndo.IsEnabled = drawingCanvas.CanUndo;
            menuEditRedo.IsEnabled = drawingCanvas.CanRedo;
        }

        /// <summary>
        /// Form is closing - ask to save
        /// </summary>
        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!PromptToSave())
            {
                e.Cancel = true;
            }
        }


        /// <summary>
        /// Function executes different actions depending on
        /// XAML version.
        /// Use one of them in actual program.
        /// </summary>
        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Instead of using control names directly,
            // find them dynamically. This allows to compile
            // the program with different XAML versions.


            Object o = FindName("imageBackground");

            if (o == null)
            {
                // Refresh clip area in the canvas.
                // This is required when canvas is used in standalone mode without
                // background image.
                drawingCanvas.RefreshClip();

                return;
            }

            Image image = o as Image;

            if (image == null)
            {
                return;
            }

            o = FindName("viewBoxContainer");

            if (o == null)
            {
                return;
            }

            Viewbox v = o as Viewbox;

            if (v == null)
            {
                return;     // precaution
            }

            // Compute actual scale of image drawn on the screen.
            // Image is resized by ViewBox.
            //
            // Note: when image is placed inside ScrollView with slider,
            // the same correction is done in XAML using ActualScale binding.

            double viewBoxWidth = v.ActualWidth;
            double viewBoxHeight = v.ActualHeight;


            double imageWidth = image.Source.Width;
            double imageHeight = image.Source.Height;

            double scale;

            if (viewBoxWidth / imageWidth > viewBoxHeight / imageHeight)
            {
                scale = viewBoxWidth / imageWidth;
            }
            else
            {
                scale = viewBoxHeight / imageHeight;
            }

            // Apply actual scale to the canvas to keep overlay line width constant.
            drawingCanvas.ActualScale = scale;
        }

        #endregion Other Event Handlers

        #region Other Functions

        /// <summary>
        /// Prompt to save and make Save operation if necessary.
        /// </summary>
        /// <returns>
        /// true - caller can continue (open new file, close program etc.
        /// false - caller should cancel current operation.
        /// </returns>
        bool PromptToSave()
        {
#if NO_PROMPT_TO_SAVE
            return true;
#else

            if (!drawingCanvas.IsDirty)
            {
                return true;    // continue
            }

            MessageBoxResult result = System.Windows.MessageBox.Show(
                this,
                "Do you want to save changes?",
                Properties.Resources.ApplicationTitle,
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question,
                MessageBoxResult.Yes);


            switch (result)
            {
                case MessageBoxResult.Yes:
                    // Save
                    FileSaveCommand(null, null);    // any better way to do this?

                    // If Saved succeeded (IsDirty  false), return true
                    return (!drawingCanvas.IsDirty);

                case MessageBoxResult.No:

                    return true;        // continue without save

                case MessageBoxResult.Cancel:

                    return false;

                default:

                    return true;
            }
#endif
        }

        /// <summary>
        /// Subscribe to different events
        /// </summary>
        void SubscribeToEvents()
        {
            this.SizeChanged += new SizeChangedEventHandler(MainWindow_SizeChanged);
            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);

            drawingCanvas.IsDirtyChanged += new RoutedEventHandler(drawingCanvas_IsDirtyChanged);

            // Menu opened - used to set menu items state
            menuTools.SubmenuOpened += new RoutedEventHandler(menuTools_SubmenuOpened);
            menuEdit.SubmenuOpened += new RoutedEventHandler(menuEdit_SubmenuOpened);

            // Tools menu
            menuToolsPointer.Click += ToolMenuItem_Click;
            menuToolsTableBlock.Click += ToolMenuItem_Click;
            menuToolsBeacon.Click += ToolMenuItem_Click;
            menuToolsBarrier.Click += ToolMenuItem_Click;

            // Tools buttons
            buttonToolPointer.PreviewMouseDown += new MouseButtonEventHandler(ToolButton_PreviewMouseDown);
            buttonToolTableBlock.PreviewMouseDown += new MouseButtonEventHandler(ToolButton_PreviewMouseDown);
            buttonToolBeacon.PreviewMouseDown += new MouseButtonEventHandler(ToolButton_PreviewMouseDown);
            buttonToolBarrier.PreviewMouseDown += new MouseButtonEventHandler(ToolButton_PreviewMouseDown);

            // Edit menu
            menuEditSelectAll.Click += new RoutedEventHandler(menuEditSelectAll_Click);
            menuEditUnselectAll.Click += new RoutedEventHandler(menuEditUnselectAll_Click);
            menuEditDelete.Click += new RoutedEventHandler(menuEditDelete_Click);
            menuEditDeleteAll.Click += new RoutedEventHandler(menuEditDeleteAll_Click);
            menuEditMoveToFront.Click += new RoutedEventHandler(menuEditMoveToFront_Click);
            menuEditMoveToBack.Click += new RoutedEventHandler(menuEditMoveToBack_Click);
            menuEditSetProperties.Click += new RoutedEventHandler(menuEditSetProperties_Click);
        }

        /// <summary>
        /// Initialize MRU list
        /// </summary>
        void InitializeMruList()
        {
            mruManager = new MruManager(SettingsManager.ApplicationSettings.RecentFilesList, menuFileRecentFiles);
            mruManager.FileSelected += new EventHandler<MruFileOpenEventArgs>(mruManager_FileSelected);
        }

        /// <summary>
        /// Set initial properties of drawing canvas
        /// </summary>
        void InitializeDrawingCanvas()
        {
            drawingCanvas.LineWidth = SettingsManager.ApplicationSettings.LineWidth;
            drawingCanvas.ObjectColor = SettingsManager.ApplicationSettings.ObjectColor;

            drawingCanvas.TextFontSize = SettingsManager.ApplicationSettings.TextFontSize;
            drawingCanvas.TextFontFamilyName = SettingsManager.ApplicationSettings.TextFontFamilyName;
            drawingCanvas.TextFontStyle = FontConversions.FontStyleFromString(SettingsManager.ApplicationSettings.TextFontStyle);
            drawingCanvas.TextFontWeight = FontConversions.FontWeightFromString(SettingsManager.ApplicationSettings.TextFontWeight);
            drawingCanvas.TextFontStretch = FontConversions.FontStretchFromString(SettingsManager.ApplicationSettings.TextFontStretch);
        }

        /// <summary>
        /// Show error message
        /// </summary>
        void ShowError(string message)
        {
            System.Windows.MessageBox.Show(
                this,
                message,
                Properties.Resources.ApplicationTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        /// <summary>
        /// Update window title
        /// </summary>
        void UpdateTitle()
        {
            string s = Properties.Resources.ApplicationTitle + " - ";

            if (string.IsNullOrEmpty(fileName))
            {
                s += Properties.Resources.Untitled;
            }
            else
            {
                s += System.IO.Path.GetFileName(fileName);
            }

            if (drawingCanvas.IsDirty)
            {
                s += " *";
            }

            this.Title = s;
        }

        /// <summary>
        /// Save to file
        /// </summary>
        bool Save(string file)
        {
            try
            {
                // Save file
                drawingCanvas.Save(file);
            }
            catch (DrawingCanvasException e)
            {
                ShowError(e.Message);
                return false;
            }

            this.fileName = file;
            UpdateTitle();

            return true;
        }

        /// <summary>
        /// This function prints graphics when background image doesn't exist
        /// </summary>
        void PrintWithoutBackground()
        {
            PrintDialog dlg = new PrintDialog();

            if (dlg.ShowDialog().GetValueOrDefault() != true)
            {
                return;
            }

            // Calculate rectangle for graphics
            double width = dlg.PrintableAreaWidth / 2;
            double height = width * drawingCanvas.ActualHeight / drawingCanvas.ActualWidth;

            double left = (dlg.PrintableAreaWidth - width) / 2;
            double top = (dlg.PrintableAreaHeight - height) / 2;

            Rect rect = new Rect(left, top, width, height);

            // Create DrawingVisual and get its drawing context
            DrawingVisual vs = new DrawingVisual();
            DrawingContext dc = vs.RenderOpen();

            double scale = width / drawingCanvas.ActualWidth;

            // Keep old existing actual scale and set new actual scale.
            double oldActualScale = drawingCanvas.ActualScale;
            drawingCanvas.ActualScale = scale;

            // Remove clip in the canvas - we set our own clip.
            drawingCanvas.RemoveClip();

            // Draw frame
            dc.DrawRectangle(null, new Pen(Brushes.Black, 1),
                new Rect(rect.Left - 1, rect.Top - 1, rect.Width + 2, rect.Height + 2));

            // Prepare drawing context to draw graphics
            dc.PushClip(new RectangleGeometry(rect));
            dc.PushTransform(new TranslateTransform(left, top));
            dc.PushTransform(new ScaleTransform(scale, scale));


            // Ask canvas to draw overlays
            drawingCanvas.Draw(dc);

            // Restore old actual scale.
            drawingCanvas.ActualScale = oldActualScale;

            // Restore clip
            drawingCanvas.RefreshClip();

            dc.Pop();
            dc.Pop();
            dc.Pop();

            dc.Close();

            // Print DrawVisual
            dlg.PrintVisual(vs, "Graphics");
        }

        /// <summary>
        /// This function prints graphics with background image.
        /// </summary>
        void PrintWithBackgroundImage(Image image)
        {
            PrintDialog dlg = new PrintDialog();

            if (dlg.ShowDialog().GetValueOrDefault() != true)
            {
                return;
            }

            // Calculate rectangle for image
            double width = dlg.PrintableAreaWidth / 2;
            double height = width * image.Source.Height / image.Source.Width;

            double left = (dlg.PrintableAreaWidth - width) / 2;
            double top = (dlg.PrintableAreaHeight - height) / 2;

            Rect rect = new Rect(left, top, width, height);

            // Create DrawingVisual and get its drawing context
            DrawingVisual vs = new DrawingVisual();
            DrawingContext dc = vs.RenderOpen();

            // Draw image
            dc.DrawImage(image.Source, rect);

            double scale = width / image.Source.Width;

            // Keep old existing actual scale and set new actual scale.
            double oldActualScale = drawingCanvas.ActualScale;
            drawingCanvas.ActualScale = scale;

            // Remove clip in the canvas - we set our own clip.
            drawingCanvas.RemoveClip();

            // Prepare drawing context to draw graphics
            dc.PushClip(new RectangleGeometry(rect));
            dc.PushTransform(new TranslateTransform(left, top));
            dc.PushTransform(new ScaleTransform(scale, scale));

            // Ask canvas to draw overlays
            drawingCanvas.Draw(dc);

            // Restore old actual scale.
            drawingCanvas.ActualScale = oldActualScale;

            // Restore clip
            drawingCanvas.RefreshClip();

            dc.Pop();
            dc.Pop();
            dc.Pop();

            dc.Close();

            // Print DrawVisual
            dlg.PrintVisual(vs, "Graphics");
        }

        #endregion Other Functions

        private void TableBlockSpinnerWide_Spin(object sender, SpinEventArgs e)
        {
            if (!(drawingCanvas.SelectedObject is GraphicsTableBlock))
                return;

            ButtonSpinner spinner = (ButtonSpinner)sender;
            TextBox txtBox = (TextBox)spinner.Content;
            GraphicsTableBlock selected = (drawingCanvas.SelectedObject as GraphicsTableBlock);

            if (e.Direction == SpinDirection.Increase)
                selected.NumTablesWide++;
            else
                selected.NumTablesWide--;
            txtBox.Text = selected.NumTablesWide.ToString();
        }

        private void TableBlockSpinnerHeight_Spin(object sender, SpinEventArgs e)
        {
            if (!(drawingCanvas.SelectedObject is GraphicsTableBlock))
                return;

            ButtonSpinner spinner = (ButtonSpinner)sender;
            TextBox txtBox = (TextBox)spinner.Content;
            GraphicsTableBlock selected = (drawingCanvas.SelectedObject as GraphicsTableBlock);

            if (e.Direction == SpinDirection.Increase)
                selected.NumTablesTall++;
            else
                selected.NumTablesTall--;
            txtBox.Text = selected.NumTablesTall.ToString();
        }

        private void OpenBluetoothIdWindow(object sender, RoutedEventArgs e)
        {
            BeaconSettings beaconWindow = new BeaconSettings { Owner = this };
            beaconWindow.ShowDialog();
        }

        Regex shortIdRegex = new Regex(@"^\d+$");

        private void BeaconIdSetter_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {


            if (shortIdRegex.IsMatch(e.Text))
            {
                e.Handled = true;
                GraphicsBeacon selected = (drawingCanvas.SelectedObject as GraphicsBeacon);

                BeaconInfo thisOne = null;

                foreach (BeaconInfo check in BeaconInfoManager.Instance().beacons)
                {
                    if (check.ShortID.Equals(e.Text))
                    {
                        thisOne = check;
                        break;
                    }
                }

                selected.Info = thisOne;
            }

            else { }
            e.Handled = false;
        }

        private void RefreshBeaconList(object sender, EventArgs e)
        {
            BeaconIdSelector.ItemsSource = BeaconInfoManager.Instance().beacons;
            BeaconIdSelector.DisplayMemberPath = "ShortID";
        }

        private void BeaconIdSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(drawingCanvas.SelectedObject is GraphicsBeacon))
                return;

            GraphicsBeacon selected = (drawingCanvas.SelectedObject as GraphicsBeacon);
            BeaconInfo thisOne = null;

            foreach (BeaconInfo check in BeaconInfoManager.Instance().beacons)
            {
                if (check.Equals(BeaconIdSelector.SelectedValue))
                {
                    thisOne = check;
                    break;
                }
            }

            selected.Info = thisOne;
        }

        private void SwapDimensions(object sender, RoutedEventArgs e)
        {
            if (!(drawingCanvas.SelectedObject is GraphicsTableBlock))
            {
                int temp = GraphicsTableBlock.DefaultNumTablesTall;
                GraphicsTableBlock.DefaultNumTablesTall = GraphicsTableBlock.DefaultNumTablesWide;
                GraphicsTableBlock.DefaultNumTablesWide = temp;
            }

            else
            {
                GraphicsTableBlock selected = (drawingCanvas.SelectedObject as GraphicsTableBlock);
                int temp = selected.NumTablesTall;
                selected.NumTablesTall = selected.NumTablesWide;
                selected.NumTablesWide = temp;
            }

            (WidthSpinner.Content as TextBox).Text = GraphicsTableBlock.DefaultNumTablesWide.ToString();
            (HeightSpinner.Content as TextBox).Text = GraphicsTableBlock.DefaultNumTablesTall.ToString();
        }
    }
}