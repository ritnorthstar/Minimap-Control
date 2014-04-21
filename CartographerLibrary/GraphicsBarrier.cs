using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;



namespace CartographerLibrary
{
    /// <summary>
    ///  Rectangle graphics object.
    /// </summary>
    public class GraphicsBarrier : GraphicsRectangleBase
    {
        #region Constructors

        public GraphicsBarrier(double left, double top, double right, double bottom,
            double lineWidth, Color objectColor, double actualScale)
        {
            this.rectangleLeft = left;
            this.rectangleTop = top;
            this.rectangleRight = right;
            this.rectangleBottom = bottom;
            this.graphicsLineWidth = lineWidth;
            this.graphicsObjectColor = objectColor;
            this.graphicsActualScale = actualScale;

            //RefreshDrawng();
        }

        public GraphicsBarrier() : this(0.0, 0.0, 100.0, 100.0, 1.0, Colors.Black, 1.0) { }

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Draw object
        /// </summary>
        public override void Draw(DrawingContext drawingContext)
        {
            if (drawingContext == null)
            {
                throw new ArgumentNullException("drawingContext");
            }

            Pen pen = new Pen(Brushes.Red, 3);
            Rect r = Rectangle;

            drawingContext.DrawRectangle(null, pen, r);

            drawingContext.DrawLine(pen, r.TopLeft, r.BottomRight);
            drawingContext.DrawLine(pen, r.BottomLeft, r.TopRight);

            base.Draw(drawingContext);
        }

        /// <summary>
        /// Test whether object contains point
        /// </summary>
        public override bool Contains(Point point)
        {
            return this.Rectangle.Contains(point);
        }

        /// <summary>
        /// Serialization support
        /// </summary>
        public override PropertiesGraphicsBase CreateSerializedObject()
        {
            return new PropertiesGraphicsBarrier(this);
        }


        #endregion Overrides

    }
}
