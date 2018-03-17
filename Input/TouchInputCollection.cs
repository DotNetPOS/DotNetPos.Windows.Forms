using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetPos.Windows.Forms.Input
{
    public class TouchInputCollection : IReadOnlyCollection<TouchInput>
    {
        private TouchInput[] _inputs;

        public TouchInput this[int index]
        {
            get { return this._inputs[index]; }
        }

        public int Count
        {
            get
            {
                return ((IReadOnlyCollection<TouchInput>)_inputs).Count;
            }
        }

        public IEnumerator<TouchInput> GetEnumerator()
        {
            return ((IReadOnlyCollection<TouchInput>)_inputs).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyCollection<TouchInput>)_inputs).GetEnumerator();
        }

        internal TouchInputCollection(System.IntPtr hWnd, NativeMethods.TOUCHINPUT[] touchInputs)
        {
            this._inputs = (TouchInput[])Array.CreateInstance(typeof(TouchInput), touchInputs.Length);

            for (int i = 0; i < touchInputs.Length; i++)
            {
                this._inputs[i] = new TouchInput(hWnd, touchInputs[i]);
            }
        }
    }
}
