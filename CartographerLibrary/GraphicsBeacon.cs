using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CartographerLibrary
{
    /// <summary>
    ///  Rectangle graphics object.
    /// </summary>
    public class GraphicsBeacon : GraphicsRectangleBase
    {
        #region Constructors

        private BeaconInfo info;
        private FormattedText formattedText;

        Typeface font = new Typeface("Calibri");


        public BeaconInfo Info
        {
            get { return info; }
            set
            {
                info = value;
                RefreshDrawing();
            }
        }

        public GraphicsBeacon(double left, double top, double right, double bottom,
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

        public GraphicsBeacon() : this(0.0, 0.0, 100.0, 100.0, 1.0, Colors.Black, 1.0) { }

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

            Rect r = Rectangle;

            Point center = new Point((r.Left + r.Right) / 2.0, (r.Top + r.Bottom) / 2.0);

            double radiusX = (r.Right - r.Left) / 2.0;
            double radiusY = (r.Bottom - r.Top) / 2.0;

            drawingContext.DrawEllipse(Brushes.DodgerBlue, null, center, radiusX, radiusY);
            drawingContext.DrawEllipse(Brushes.SkyBlue, null, center, radiusX * 2 / 3, radiusY * 2 / 3);

            formattedText = new FormattedText((info == null ? String.Empty : info.ShortID), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, font, 18, Brushes.MediumBlue);

            drawingContext.DrawText(formattedText, new Point(r.Left, r.Top));

            base.Draw(drawingContext);
        }

        public override void DrawTracker(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(null, new Pen(Brushes.Black, 2), Rectangle);
        }

        public override Cursor GetHandleCursor(int handleNumber)
        {
            return HelperFunctions.DefaultCursor;
        }

        /// <summary>
        /// Test whether object contains point
        /// </summary>
        public override bool Contains(Point point)
        {
            if (IsSelected)
            {
                return this.Rectangle.Contains(point);
            }
            else
            {
                EllipseGeometry g = new EllipseGeometry(Rectangle);

                return g.FillContains(point) || g.StrokeContains(new Pen(Brushes.Black, ActualLineWidth), point);
            }
        }

        /// <summary>
        /// Test whether object intersects with rectangle
        /// </summary>
        public override bool IntersectsWith(Rect rectangle)
        {
            RectangleGeometry rg = new RectangleGeometry(rectangle);    // parameter
            EllipseGeometry eg = new EllipseGeometry(Rectangle);        // this object rectangle

            PathGeometry p = Geometry.Combine(rg, eg, GeometryCombineMode.Intersect, null);

            return (!p.IsEmpty());
        }


        /// <summary>
        /// Serialization support
        /// </summary>
        public override PropertiesGraphicsBase CreateSerializedObject()
        {
            return new PropertiesGraphicsBeacon(this);
        }

        public override void MoveHandleTo(Point point, int handleNumber)
        {
            return;
        }
        #endregion Overrides

    }
}
