using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetPos.Windows.Forms
{
    /// <summary>
    /// The GestureEventArgs class provides access to the logical
    /// Touch device for all derived event args.
    /// </summary>
    public class GestureEventArgs : EventArgs
    {
        private Point _location;

        public Point Location
        {
            get { return this._location; }
            internal set { this._location = value; }
        }

        internal GestureEventArgs(NativeMethods.GESTUREINFO gestureInfo)
        {
            this._location = new Point(gestureInfo.ptsLocation.x, gestureInfo.ptsLocation.y);
        }
    }
}
