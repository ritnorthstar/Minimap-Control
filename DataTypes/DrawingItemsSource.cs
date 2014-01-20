﻿using System;
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
        private List<IDrawable> drawingElements;
        private Rect extent;

        public DrawingItemsSource()
        {
            extent = new Rect(0, 0, 500, 500);
            drawingElements = new List<IDrawable>();// { new DebugRect(0, 0, extent.Width, extent.Height) };
            Console.WriteLine("Just made debugrect");
        }

        /// <summary>
        /// Gets a rectangle with the extent of all elements (not just the ones on-screen
        /// </summary>
        public Rect Extent
        {
            get
            {
                return extent;
            }
        }

        /// <summary>
        /// Gets a list of indicies of elements within the current viewbox
        /// </summary>
        /// <param name="viewbox"></param>
        /// <returns></returns>
        public IEnumerable<int> Query(Rect viewbox)
        {
            IEnumerable<int> output;
            //Console.WriteLine("elements: " + drawingElements.Count);

            if (drawingElements.Count > 0)
            {
                output = Enumerable.Range(0, drawingElements.Count);
            }
            else
            {
                output = new List<int>();
            }
            List<string> test = new List<string>();

            foreach(IDrawable d in drawingElements)
                test.Add(String.Format("{0}: {1} - {2}", test.Count, d.GetType(), d.ToString()));
    
            //Console.WriteLine("query results (" + test.Count + ":");

            //foreach (string s in test)
            //    Console.WriteLine(s);

            //Console.WriteLine("Output: " + String.Join(", ", output.ToArray<int>()));

            return output;
        }

        public void AddChild(IDrawable child)
        {
            drawingElements.Add(child);
            updateDrawBounds(child);
        }

        private void updateDrawBounds(IDrawable child)
        {
            if (extent.Width < child.x + child.width) extent = new Rect(0, 0, child.x + child.width + 50, extent.Height);
            if (extent.Height < child.y + child.height) extent = new Rect(0, 0, extent.Width, child.y + child.height + 50);
            //Console.WriteLine("Extent rect updated to " + extent.ToString());
        }

        public void ClearChildren()
        {
            drawingElements.Clear();
        }

        public object this[int i]
        {
            get
            {
                //Console.WriteLine("Getting drawable " + i);

                if (Count == 0)
                {
                    return null;
                }

                try
                {
                    return drawingElements[i].GetDrawable();
                }
                catch (NotImplementedException)
                {
                    Console.WriteLine("Item #" + i + " isn't implemented (" + drawingElements[i].GetType().ToString() + ")");
                    return null;
                }
            }
            set { }
        }

        public int Count
        {
            get
            {
                return drawingElements.Count;
            }
        }

        public event EventHandler ExtentChanged;

        public event EventHandler QueryInvalidated;

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