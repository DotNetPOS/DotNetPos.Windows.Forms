using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetPos.Windows.Forms
{
    /// <summary>
    /// The TouchEventArgs class provides access to the logical
    /// Touch device for all derived event args.
    /// </summary>
    public class TouchEventArgs : EventArgs
    {
        // Private data members
        private int _id;                 // contact ID
        private int _mask;               // mask which fields in the structure are valid
        private int _flags;              // flags
        private int _time;               // touch event time
        
        private Point _location;
        private Size _size;

        /// <summary>
        /// Gets the location of the touch during the generating touch event.
        /// </summary>
        /// <value>
        /// A <see cref="Point">Point</see> that contains the x- and y- mouse coordinates, in pixels, relative to the upper-left corner of the client.
        /// </value>
        public Point Location
        {
            get { return this._location; }
            internal set { this._location = value; }
        }

        /// <summary>
        /// Get the contact area size in pixels.
        /// </summary>
        /// <value>
        /// A <see cref="Size">Size</see> that contains the width and height of the contact area of the touch event in pixels.
        /// </value>
        public Size Size
        {
            get { return this._size; }
            internal set { this._size = value; }
        }

        /// <summary>
        /// Gets the x-coordinate of the mouse during the generating mouse event.
        /// </summary>
        /// <value>
        /// The x-coordinate of the mouse, in pixels.
        /// </value>
        public int X
        {
            get { return this._location.X; }
            internal set { this._location.X = value; }
        }

        /// <summary>
        /// Gets the y-coordinate of the mouse during the generating mouse event.
        /// </summary>
        /// <value>
        /// The Y-coordinate of the touch, in pixels.
        /// </value>
        public int Y
        {
            get { return this._location.Y; }
            internal set { this._location.Y = value; }
        }

        // Access to data members

        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }
        public int Flags
        {
            get { return this._flags; }
            set { this._flags = value; }
        }
        public int Mask
        {
            get { return this._mask; }
            set { this._mask = value; }
        }
        public int Time
        {
            get { return this._time; }
            set { this._time = value; }
        }
        
        public bool IsPrimaryContact
        {
            get { return (this._flags & UnsafeMethods.TOUCHEVENTF_PRIMARY) != 0; }
        }

        // Constructor
        public TouchEventArgs()
        {
        }

        internal TouchEventArgs(UnsafeMethods.TOUCHINPUT touchInput)
        {
            // TOUCHINFO point coordinates and contact size is in 1/100 of a pixel; convert it to pixels.
            // Also convert screen to client coordinates.
            this._size = new Size(touchInput.cxContact / 100, touchInput.cyContact / 100);

            this._id = touchInput.dwID;
            //{
            //    te.Location = PointToClient(new Point(ti.x / 100, ti.y / 100));
            //}

            this._time = touchInput.dwTime;
            this._mask = touchInput.dwMask;
            this._flags = touchInput.dwFlags;
        }

    }
}
