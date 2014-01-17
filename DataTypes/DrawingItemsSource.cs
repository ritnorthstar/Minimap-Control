using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataTypes
{
    public class DrawingItemsSource : IList, ZoomableCanvas.ISpatialItemsSource
    {
        private List<Drawable> drawingElements;

        public DrawingItemsSource()
        {
            drawingElements = new List<Drawable>();
        }

        /// <summary>
        /// Gets a rectangle with the extent of all elements (not just the ones on-screen
        /// </summary>
        public Rect Extent
        {
            get
            {
                var sqrt = Math.Sqrt(Count);
                return new Rect(0, 0,
                    100 * (Math.Ceiling(sqrt)),
                    100 * (Math.Round(sqrt)));
            }
        }

        /// <summary>
        /// Gets a list of indicies of elements within the current viewbox
        /// </summary>
        /// <param name="viewbox"></param>
        /// <returns></returns>
        public IEnumerable<int> Query(Rect viewbox)
        {
            viewbox.Intersect(Extent);

            var top = Math.Floor(viewbox.Top / 100);
            var left = Math.Floor(viewbox.Left / 100);
            var right = Math.Ceiling(viewbox.Right / 100);
            var bottom = Math.Ceiling(viewbox.Bottom / 100);
            var width = Math.Max(right - left, 0);
            var height = Math.Max(bottom - top, 0);

            foreach (var cell in Quadivide(new Rect(left, top, width, height)))
            {
                var x = cell.X;
                var y = cell.Y;
                var i = x > y ?
                    Math.Pow(x, 2) + y :
                    Math.Pow(y, 2) + 2 * y - x;
                if (i < Count)
                {
                    yield return (int)i;
                }
            }
        }

        public object this[int i]
        {
            get
            {
                // Give the item a deterministic seed.
                var rand = new Random(i);
                var sqrt = (int)Math.Sqrt(i);

                // With a random width and height between 50 and 100.
                var width = rand.Next(50, 100);
                var height = rand.Next(50, 100);

                // And place it at a random position in a grid-like pattern.
                var top = Math.Min(sqrt, i - Math.Pow(sqrt, 2))
                          * 100 + rand.Next(100 - height);
                var left = Math.Min(sqrt, sqrt * 2 - (i - Math.Pow(sqrt, 2)))
                           * 100 + rand.Next(100 - width);

                // Randomly generate the outline of the item.
                var type = rand.Next(3);
                var data = type == 0 ? "ellipse" :
                           type == 1 ? "rectangle" :
                           String.Format("M{0},{1} C{2},{3} {4},{5} {6},{7}",
                               rand.NextDouble(),
                               rand.NextDouble(),
                               rand.NextDouble(),
                               rand.NextDouble(),
                               rand.NextDouble(),
                               rand.NextDouble(),
                               rand.NextDouble(),
                               rand.NextDouble());

                // Give it random gradient colors.
                var brush = new LinearGradientBrush(
                                Color.FromScRgb(1f, (float)rand.NextDouble(),
                                                    (float)rand.NextDouble(),
                                                    (float)rand.NextDouble()),
                                Color.FromScRgb(1f, (float)rand.NextDouble(),
                                                    (float)rand.NextDouble(),
                                                    (float)rand.NextDouble()),
                                180 * rand.NextDouble());

                return new { top, left, width, height, data, brush, i };
            }
            set
            {
            }
        }

        public int Count
        {
            get
            {
                return 25;// drawingElements.Count;
            }
        }

        public event EventHandler ExtentChanged;

        public event EventHandler QueryInvalidated;

        /// <summary>
        /// Performant method for prioritizing grid-based elements for evenly-spread drawing
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        private IEnumerable<Point> Quadivide(Rect area)
        {
            if (area.Width > 0 && area.Height > 0)
            {
                var center = area.GetCenter();
                var x = Math.Floor(center.X);
                var y = Math.Floor(center.Y);
                yield return new Point(x, y);

                var quad1 = new Rect(area.TopLeft, new Point(x, y + 1));
                var quad2 = new Rect(area.TopRight, new Point(x, y));
                var quad3 = new Rect(area.BottomLeft, new Point(x + 1, y + 1));
                var quad4 = new Rect(area.BottomRight, new Point(x + 1, y));

                var quads = new Queue<IEnumerator<Point>>();
                quads.Enqueue(Quadivide(quad1).GetEnumerator());
                quads.Enqueue(Quadivide(quad2).GetEnumerator());
                quads.Enqueue(Quadivide(quad3).GetEnumerator());
                quads.Enqueue(Quadivide(quad4).GetEnumerator());
                while (quads.Count > 0)
                {
                    var quad = quads.Dequeue();
                    if (quad.MoveNext())
                    {
                        yield return quad.Current;
                        quads.Enqueue(quad);
                    }
                }
            }
        }

        #region Irrelevant IList Members

        int IList.Add(object value)
        {
            return 0;
        }

        void IList.Clear()
        {
        }

        bool IList.Contains(object value)
        {
            return false;
        }

        int IList.IndexOf(object value)
        {
            return 0;
        }

        void IList.Insert(int index, object value)
        {
        }

        void IList.Remove(object value)
        {
        }

        void IList.RemoveAt(int index)
        {
        }

        void ICollection.CopyTo(Array array, int index)
        {
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return true; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return null; }
        }

        int ICollection.Count
        {
            get { return int.MaxValue; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield break;
        }

        #endregion
    }
}