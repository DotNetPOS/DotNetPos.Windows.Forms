using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetPos.Windows.Forms.Input
{
    public class TouchInput
    {
        /// <summary>
        /// The location of the touch input in pixels.
        /// </summary>
        private Point _location;

        /// <summary>
        /// The size of the touch contact area in pixels.
        /// </summary>
        private Size _size;
                
        /// <summary>
        /// A device handle for the source input device. Each device is given a unique provider at run time by the touch input provider.
        /// </summary>
        private System.IntPtr _source;

        /// <summary>
        /// A touch point identifier that distinguishes a particular touch input. This value stays consistent in a touch contact sequence from the point a contact comes down until it comes back up. An ID may be reused later for subsequent contacts.
        /// </summary>
        private int _id;

        /// <summary>
        /// A set of bit flags that specify various aspects of touch point press, release, and motion. The bits in this member can be any reasonable combination of the values in the Remarks section.
        /// </summary>
        private int _flags;

        /// <summary>
        /// A set of bit flags that specify which of the optional fields in the structure contain valid values. The availability of valid information in the optional fields is device-specific. Applications should use an optional field value only when the corresponding bit is set in dwMask. This field may contain a combination of the dwMask flags mentioned in the Remarks section.
        /// </summary>
        private int _mask;

        /// <summary>
        /// The time stamp for the event, in milliseconds. The consuming application should note that the system performs no validation on this field; when the TOUCHINPUTMASKF_TIMEFROMSYSTEM flag is not set, the accuracy and sequencing of values in this field are completely dependent on the touch input provider.
        /// </summary>
        private int _timestamp;

        /// <summary>
        /// An additional value associated with the touch event.
        /// </summary>
        private System.IntPtr _extraInfo;

        #region Properties

        /// <summary>
        /// Gets an additional value associated with the touch event.
        /// </summary>
        internal System.IntPtr ExtraInfo
        {
            get { return this._extraInfo; }
        }

        /// <summary>
        /// Gets a set of bit flags that specify various aspects of touch point press, release, and motion.
        /// </summary>
        internal int Flags
        {
            get { return this._flags; }
        }

        /// <summary>
        /// Gets a touch point identifier that distinguishes a particular touch input. This value stays consistent in a touch contact sequence from the point a contact comes down until it comes back up. An ID may be reused later for subsequent contacts.
        /// </summary>
        internal int Id
        {
            get { return this._id; }
        }

        /// <summary>
        /// Gets if this is the primary touch.
        /// </summary>
        public bool IsPrimaryContact
        {
            get { return (this._flags & NativeMethods.TOUCHEVENTF_PRIMARY) != 0; }
        }

        /// <summary>
        /// Gets the location of the touch input in pixels.
        /// </summary>
        public Point Location
        {
            get { return this._location; }
        }
            
        /// <summary>
        /// Gets a set of bit flags that specify which of the optional fields in the structure contain valid values.
        /// </summary>
        internal int Mask
        {
            get { return this._mask; }
        }
            
        /// <summary>
        /// Gets the size of the touch contact area in pixels.
        /// </summary>
        public Size Size
        {
            get { return this._size; }
        }

        /// <summary>
        /// Gets the device handle for the source input device.
        /// </summary>
        /// <remarks>
        /// Each device is given a unique provider at run time by the touch input provider.
        /// </remarks>
        public System.IntPtr Source
        {
            get { return this._source; }
        }

        /// <summary>
        /// Gets the time stamp for the event, in milliseconds.
        /// </summary>
        /// <remarks>
        /// The consuming application should note that the system performs no validation on this field; when the TOUCHINPUTMASKF_TIMEFROMSYSTEM flag is not set, the accuracy and sequencing of values in this field are completely dependent on the touch input provider.
        /// </remarks>
        internal int Timestamp
        {
            get { return this._timestamp; }
        }
        
        #endregion

        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="touchInput">The <see cref="NativeMethods.TOUCHINPUT">TOUCHINPUT</see> structure.</param>
        internal TouchInput(System.IntPtr hWnd, NativeMethods.TOUCHINPUT touchInput)
        {
            this._flags = touchInput.dwFlags;
            this._mask = touchInput.dwMask;
            this._extraInfo = touchInput.dwExtraInfo;
            this._id = touchInput.dwID;
            this._source = touchInput.hSource;
            this._timestamp = touchInput.dwTime;

            // TOUCHINFO point coordinates and contact size is in 1/100 of a pixel; convert it to pixels.
            // Also convert screen to client coordinates.
            this._size = new Size(touchInput.cxContact / 100, touchInput.cyContact / 100);

            NativeMethods.POINT lpPoint = new NativeMethods.POINT();
            lpPoint.x = touchInput.x;
            lpPoint.y = touchInput.y;

            if (!NativeMethods.ScreenToClient(hWnd, ref lpPoint))
            {
                int error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }

            this._location = new Point(lpPoint.x, lpPoint.y);
        }
    }
}
