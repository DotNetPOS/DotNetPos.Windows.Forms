using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotNetPos.Windows.Forms
{
    public class Control : System.Windows.Forms.Control
    {
        // Touch event handlers
        public event EventHandler<TouchEventArgs> TouchDown;   // touch down event handler
        public event EventHandler<TouchEventArgs> TouchUp;     // touch up event handler
        public event EventHandler<TouchEventArgs> TouchMove;   // touch move event handler


        /// <summary>
        /// Extracts lower 16-bit word from an 32-bit int.
        /// </summary>
        /// <param name="number">int</param>
        /// <returns>lower word</returns>
        private static int LoWord(int number)
        {
            return (number & 0xffff);
        }

        /// <summary>
        /// Decodes and handles WM_TOUCH message.
        /// Unpacks message arguments and invokes appropriate touch events.
        /// </summary>
        /// <param name="m">
        /// Window message.
        /// </param>
        /// <returns>
        /// whether the message has been handled
        /// </returns>
        private bool DecodeTouch(ref Message m)
        {
            // GetTouchInputInfo needs to be
            // passed the size of the structure it will be filling.
            // We get the size upfront so it can be used later.
            int touchInputSize = Marshal.SizeOf(new NativeMethods.TOUCHINPUT());

            // More than one touchinput may be associated with a touch message,
            // so an array is needed to get all event information.
            int inputCount = LoWord(m.WParam.ToInt32()); // Number of touch inputs, actual per-contact messages

            NativeMethods.TOUCHINPUT[] inputs; // Array of TOUCHINPUT structures
            inputs = new NativeMethods.TOUCHINPUT[inputCount]; // Allocate the storage for the parameters of the per-contact messages

            // Unpack message parameters into the array of TOUCHINPUT structures, each
            // representing a message for one single contact.
            if (!NativeMethods.GetTouchInputInfo(m.LParam, inputCount, inputs, touchInputSize))
            {
                // Get touch info failed.
                return false;
            }

            // For each contact, dispatch the message to the appropriate message
            // handler.
            bool handled = false; // Boolean, is message handled
            for (int i = 0; i < inputCount; i++)
            {
                NativeMethods.TOUCHINPUT ti = inputs[i];

                // Assign a handler to this message.
                EventHandler<TouchEventArgs> handler = null;     // Touch event handler
                if ((ti.dwFlags & NativeMethods.TOUCHEVENTF_DOWN) != 0)
                {
                    handler = TouchDown;
                }
                else if ((ti.dwFlags & NativeMethods.TOUCHEVENTF_UP) != 0)
                {
                    handler = TouchUp;
                }
                else if ((ti.dwFlags & NativeMethods.TOUCHEVENTF_MOVE) != 0)
                {
                    handler = TouchMove;
                }

                // Convert message parameters into touch event arguments and handle the event.
                if (handler != null)
                {
                    // Convert the raw touchinput message into a touchevent.
                    TouchEventArgs te = new TouchEventArgs(); // Touch event arguments

                    te.Location = PointToClient(new Point(ti.x / 100, ti.y / 100));

                    // Call the virtual methods
                    if ((ti.dwFlags & NativeMethods.TOUCHEVENTF_DOWN) != 0)
                    {
                        OnTouchDown(te);
                    }
                    else if ((ti.dwFlags & NativeMethods.TOUCHEVENTF_UP) != 0)
                    {
                        OnTouchUp(te);
                    }
                    else if ((ti.dwFlags & NativeMethods.TOUCHEVENTF_MOVE) != 0)
                    {
                        OnTouchMove(te);
                    }

                    // Invoke the event handler.
                    handler(this, te);

                    // Mark this event as handled.
                    handled = true;
                }
            }

            NativeMethods.CloseTouchInputHandle(m.LParam);

            return handled;
        }

        
        protected override void WndProc(ref Message m)
        {
            bool handled;
            switch (m.Msg)
            {
                case NativeMethods.WM_TOUCH:
                    handled = DecodeTouch(ref m);
                    break;
                default:
                    handled = false;
                    break;
            }

            // Call parent WndProc for default message processing.
            base.WndProc(ref m);

            if (handled)
            {
                // Acknowledge event if handled.
                m.Result = new System.IntPtr(1);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            NativeMethods.RegisterTouchWindow(this.Handle, 0);
        }
        protected virtual void OnTouchDown(TouchEventArgs e)
        {

        }

        protected virtual void OnTouchUp(TouchEventArgs e)
        {

        }

        protected virtual void OnTouchMove(TouchEventArgs e)
        {

        }
    }
}
