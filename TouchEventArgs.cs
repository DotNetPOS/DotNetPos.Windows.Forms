using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotNetPos.Windows.Forms
{
    /// <summary>
    /// The TouchEventArgs class provides access to the logical
    /// Touch device for all derived event args.
    /// </summary>
    public class TouchEventArgs : EventArgs
    {
        private Input.TouchInputCollection _contacts;

        public Input.TouchInputCollection Contacts
        {
            get { return this._contacts; }
        }

        /// <summary>
        /// Extracts lower 16-bit word from an 32-bit int.
        /// </summary>
        /// <param name="number">int</param>
        /// <returns>lower word</returns>
        private int LoWord(int number)
        {
            return (number & 0xffff);
        }

        internal TouchEventArgs(ref Message m)
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
                int error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }


        }

    }
}
