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
    public class Control : System.Windows.Forms.Control
    {
        // Touch event handlers
        public event EventHandler<TouchEventArgs> Touch;
        
        protected override void WndProc(ref Message m)
        {
            bool handled;
            switch (m.Msg)
            {
                case NativeMethods.WM_TOUCH:
                    try
                    {
                        TouchEventArgs touchEventArgs = new Forms.TouchEventArgs(ref m);
                        handled = true;
                    }
                    catch
                    {
                        handled = false;
                    }
                    
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
            try
            {
                if (!NativeMethods.RegisterTouchWindow(Handle, 0))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        protected void RaiseTouchEvent(object key, TouchEventArgs e)
        {
            Touch(this, e);
        }

        protected virtual void OnTouch(TouchEventArgs e)
        {
            // Raise the event
            RaiseTouchEvent(this, e);
        }
        
    }
}
