using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;


namespace CartographerLibrary
{
    /// <summary>
    /// Ellipse tool
    /// </summary>
    class ToolBeacon : ToolObject
    {
        public ToolBeacon()
        {
            MemoryStream stream = new MemoryStream(Properties.Resources.Ellipse);
            ToolCursor = new Cursor(stream);
        }

        /// <summary>
        /// Create new rectangle
        /// </summary>
        public override void OnMouseDown(DrawingCanvas drawingCanvas, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(drawingCanvas);

            AddNewObject(drawingCanvas,
                new GraphicsBeacon(
                p.X - 10,
                p.Y - 10,
                p.X + 10,
                p.Y + 10,
                drawingCanvas.LineWidth,
                drawingCanvas.ObjectColor,
                drawingCanvas.ActualScale));
        }

        /// <summary>
        /// Set cursor and resize new polyline
        /// </summary>
        public override void OnMouseMove(DrawingCanvas drawingCanvas, MouseEventArgs e)
        {
            drawingCanvas.Cursor = ToolCursor;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (drawingCanvas.IsMouseCaptured)
                {
                    if (drawingCanvas.Count > 0)
                    {
                        drawingCanvas[drawingCanvas.Count - 1].MoveTo(e.GetPosition(drawingCanvas));
                    }
                }

            }
        }
    }
}
