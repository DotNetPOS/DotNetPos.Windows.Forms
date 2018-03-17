using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetPos.Windows.Forms
{
    [System.Security.SuppressUnmanagedCodeSecurityAttribute()]
    internal static class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll")]
        internal static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        #region Touch

        // Touch event window message constants [winuser.h]
        internal const int WM_TOUCH = 0x0240;

        // Touch event flags ((TOUCHINPUT.dwMask) [winuser.h]
        internal const int TOUCHEVENTF_MOVE = 0x0001;
        internal const int TOUCHEVENTF_DOWN = 0x0002;
        internal const int TOUCHEVENTF_UP = 0x0004;
        internal const int TOUCHEVENTF_INRANGE = 0x0008;
        internal const int TOUCHEVENTF_PRIMARY = 0x0010;
        internal const int TOUCHEVENTF_NOCOALESCE = 0x0020;
        internal const int TOUCHEVENTF_PEN = 0x0040;

        // Touch event flags ((TOUCHINPUT.dwFlags) [winuser.h]
        internal const int TOUCHINPUTMASKF_TIMEFROMSYSTEM = 0x0001;
        internal const int TOUCHINPUTMASKF_EXTRAINFO = 0x0002;
        internal const int TOUCHINPUTMASKF_CONTACTAREA = 0x0004;

        /// <summary>
        /// Encapsulates data for touch input.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct TOUCHINPUT
        {
            /// <summary>
            /// The x-coordinate (horizontal point) of the touch input. This member is indicated in hundredths of a pixel of physical screen coordinates.
            /// </summary>
            public int x;

            /// <summary>
            /// The y-coordinate (vertical point) of the touch input. This member is indicated in hundredths of a pixel of physical screen coordinates.
            /// </summary>
            public int y;

            /// <summary>
            /// A device handle for the source input device. Each device is given a unique provider at run time by the touch input provider.
            /// </summary>
            public System.IntPtr hSource;

            /// <summary>
            /// A touch point identifier that distinguishes a particular touch input. This value stays consistent in a touch contact sequence from the point a contact comes down until it comes back up. An ID may be reused later for subsequent contacts.
            /// </summary>
            public int dwID;

            /// <summary>
            /// A set of bit flags that specify various aspects of touch point press, release, and motion. The bits in this member can be any reasonable combination of the values in the Remarks section.
            /// </summary>
            public int dwFlags;

            /// <summary>
            /// A set of bit flags that specify which of the optional fields in the structure contain valid values. The availability of valid information in the optional fields is device-specific. Applications should use an optional field value only when the corresponding bit is set in dwMask. This field may contain a combination of the dwMask flags mentioned in the Remarks section.
            /// </summary>
            public int dwMask;

            /// <summary>
            /// The time stamp for the event, in milliseconds. The consuming application should note that the system performs no validation on this field; when the TOUCHINPUTMASKF_TIMEFROMSYSTEM flag is not set, the accuracy and sequencing of values in this field are completely dependent on the touch input provider.
            /// </summary>
            public int dwTime;

            /// <summary>
            /// An additional value associated with the touch event.
            /// </summary>
            public System.IntPtr dwExtraInfo;

            /// <summary>
            /// The width of the touch contact area in hundredths of a pixel in physical screen coordinates. This value is only valid if the dwMask member has the TOUCHEVENTFMASK_CONTACTAREA flag set.
            /// </summary>
            public int cxContact;

            /// <summary>
            /// The height of the touch contact area in hundredths of a pixel in physical screen coordinates. This value is only valid if the dwMask member has the TOUCHEVENTFMASK_CONTACTAREA flag set.
            /// </summary>
            public int cyContact;
        }

        /// <summary>
        /// Checks whether a specified window is touch-capable and, optionally, retrieves the modifier flags set for the window's touch capability.
        /// </summary>
        /// <param name="hWnd">
        /// The handle of the window. The function fails with ERROR_ACCESS_DENIED if the calling thread is not on the same desktop as the 
        /// specified window.
        /// </param>
        /// <param name="pulFlags">
        /// The address of the ULONG variable to receive the modifier flags for the specified window's touch capability.
        /// </param>
        /// <returns>
        /// Returns TRUE if the window supports Windows Touch; returns FALSE if the window does not support Windows Touch.
        /// </returns>
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsTouchWindow(System.IntPtr hWnd, [Out, Optional] ulong pulFlags);

        /// <summary>
        /// Registers a window as being touch-capable.
        /// </summary>
        /// <param name="hWnd">
        /// The handle of the window being registered. The function fails with ERROR_ACCESS_DENIED if the calling thread does not own the 
        /// specified window.
        /// </param>
        /// <param name="ulFlags">
        /// A set of bit flags that specify optional modifications. This field may contain 0 or one of the following values.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.To get extended error information, use the GetLastError function.
        /// </returns>
        /// <remarks>
        /// Note  RegisterTouchWindow must be called on every window that will be used for touch input. This means that if you have an 
        /// application that has multiple windows within it, RegisterTouchWindow must be called on every window in that application that uses 
        /// touch features. Also, an application can call RegisterTouchWindow any number of times for the same window if it desires to change the 
        /// modifier flags. A window can be marked as no longer requiring touch input using the UnregisterTouchWindow function.
        /// 
        /// If TWF_WANTPALM is enabled, packets from touch input are not buffered and palm detection is not performed before the packets are sent 
        /// to your application.Enabling TWF_WANTPALM is most useful if you want minimal latencies when processing WM_TOUCH messages.
        /// </remarks>
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool RegisterTouchWindow(System.IntPtr hWnd, ulong ulFlags);

        /// <summary>
        /// Registers a window as no longer being touch-capable.
        /// </summary>
        /// <param name="hWnd">
        /// The handle of the window. The function fails with ERROR_ACCESS_DENIED if the calling thread does not own the specified window.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.To get extended error information, use the GetLastError function.
        /// </returns>
        /// <remarks>
        /// The UnregisterTouchWindow function succeeds even if the specified window was not previously registered as being touch-capable.
        /// </remarks>
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnregisterTouchWindow(System.IntPtr hWnd);

        /// <summary>
        /// Retrieves detailed information about touch inputs associated with a particular touch input handle.
        /// </summary>
        /// <param name="hTouchInput">
        /// The touch input handle received in the LPARAM of a touch message. The function fails with ERROR_INVALID_HANDLE if this handle 
        /// is not valid. Note that the handle is not valid after it has been used in a successful call to CloseTouchInputHandle or after it has been 
        /// passed to DefWindowProc, PostMessage, SendMessage or one of their variants.
        /// </param>
        /// <param name="cInputs">
        /// The number of structures in the pInputs array. This should ideally be at least equal to the number of touch points associated with the 
        /// message as indicated in the message WPARAM. If cInputs is less than the number of touch points, the function will still succeed and 
        /// populate the pInputs buffer with information about cInputs touch points.
        /// </param>
        /// <param name="pInputs">
        /// A pointer to an array of TOUCHINPUT structures to receive information about the touch points associated with the specified touch 
        /// input handle.
        /// </param>
        /// <param name="cbSize">
        /// The size, in bytes, of a single TOUCHINPUT structure. If cbSize is not the size of a single TOUCHINPUT structure, the function fails 
        /// with ERROR_INVALID_PARAMETER.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, use the 
        /// GetLastError function.
        /// </returns>
        /// <remarks>
        /// Calling CloseTouchInputHandle will not free memory associated with values retrieved in a call to GetTouchInputInfo. Values in structures 
        /// passed to GetTouchInputInfo will be valid until you delete them.
        /// </remarks>
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetTouchInputInfo(System.IntPtr hTouchInput, int cInputs, [In, Out] TOUCHINPUT[] pInputs, int cbSize);

        /// <summary>
        /// Closes a touch input handle, frees process memory associated with it, and invalidates the handle.
        /// </summary>
        /// <param name="lParam">
        /// The touch input handle received in the LPARAM of a touch message. The function fails with ERROR_INVALID_HANDLE if this handle 
        /// is not valid. Note that the handle is not valid after it has been used in a successful call to CloseTouchInputHandle or after it has been 
        /// passed to DefWindowProc, PostMessage, SendMessage or one of their variants.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.To get extended error information, use the GetLastError function.
        /// </returns>
        /// <remarks>
        /// Calling CloseTouchInputHandle will not free memory associated with values retrieved in a call to GetTouchInputInfo. Values in structures 
        /// passed to GetTouchInputInfo will be valid until you delete them.
        /// </remarks>
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern void CloseTouchInputHandle(System.IntPtr lParam);

        #endregion

    }
}
