using System;
using System.Windows.Forms;

namespace Fishing
{
    static class ControlExtensions
    {
        /// <summary>
        /// Helper function to make UI operations threadsafe. Call with on a Control, eg.
        /// this.UIThread(function); This method is asynchronous, so it is best for quick
        /// calls, eg. to update UI elements when the caller isn't waiting for a return.
        /// </summary>
        /// <param name="control">Control to get thread from for invocation</param>
        /// <param name="code">Function to invoke</param>
        static public void UIThread(this Control control, Action code)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(code);
                return;
            }
            code.Invoke();
        }

        /// <summary>
        /// Helper function to make UI operations threadsafe. Call with on a Control, eg.
        /// this.UIThreadInvoke(function); This method blocks until the passed Action has
        /// completed, so it is best used when the passed function sets an output variable.
        /// </summary>
        /// <param name="control">Control to get thread from for invocation</param>
        /// <param name="code">Function to invoke</param>
        static public void UIThreadInvoke(this Control control, Action code)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(code);
                return;
            }
            code.Invoke();
        }
    }
}
