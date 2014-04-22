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
    public class GraphicsTableBlock : GraphicsRectangleBase
    {
        public static int DefaultNumTablesTall = 2;
        public static int DefaultNumTablesWide = 2;

        private int numTablesTall = GraphicsTableBlock.DefaultNumTablesTall;
        private int numTablesWide = GraphicsTableBlock.DefaultNumTablesWide;

        public int NumTablesTall
        {
            get { return numTablesTall; }
            set
            {
                numTablesTall = (value < 1 ? 1 : value);
                GraphicsTableBlock.DefaultNumTablesTall = numTablesTall;
                RefreshDrawing();
            }
        }

        public int NumTablesWide
        {
            get { return numTablesWide; }
            set
            {
                numTablesWide = (value < 1 ? 1 : value);
                GraphicsTableBlock.DefaultNumTablesWide = numTablesWide;
                RefreshDrawing();
            }
        }

        #region Constructors

        public GraphicsTableBlock(double left, double top, double right, double bottom,
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

        public GraphicsTableBlock() : this(0.0, 0.0, 100.0, 100.0, 1.0, Colors.Black, 1.0) { }

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

            Pen pen = new Pen(Brushes.Green, 3);
            Rect r = Rectangle;

            drawingContext.DrawRectangle(null, pen, r);
            
            // horizontal lines
            for (int n = 1; n < numTablesTall; n++)
            {
                Point p1 = new Point(r.Left, r.Top + r.Height * (float)n / numTablesTall);
                Point p2 = new Point(r.Right, p1.Y);

                drawingContext.DrawLine(pen, p1, p2);
            }

            // vertical lines
            for (int n = 1; n < numTablesWide; n++)
            {
                Point p1 = new Point(r.Left + r.Width * (float)n / numTablesWide, r.Top);
                Point p2 = new Point(p1.X, r.Bottom);

                drawingContext.DrawLine(pen, p1, p2);
            }


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
            return new PropertiesGraphicsTableBlock(this);
        }


        #endregion Overrides

    }
}
