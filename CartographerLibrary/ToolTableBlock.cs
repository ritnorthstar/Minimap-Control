using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;


namespace CartographerLibrary
{
    /// <summary>
    /// TableBlock tool
    /// </summary>
    class ToolTableBlock : ToolRectangleBase
    {
        public ToolTableBlock()
        {
            MemoryStream stream = new MemoryStream(Properties.Resources.Rectangle);
            ToolCursor = new Cursor(stream);
        }

        /// <summary>
        /// Create new TableBlock
        /// </summary>
        public override void OnMouseDown(DrawingCanvas drawingCanvas, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(drawingCanvas);

            AddNewObject(drawingCanvas, 
                new GraphicsTableBlock(
                p.X,
                p.Y,
                p.X + 1,
                p.Y + 1,
                drawingCanvas.LineWidth,
                drawingCanvas.ObjectColor,
                drawingCanvas.ActualScale));
        }
    }
}
