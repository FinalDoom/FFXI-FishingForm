using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using Fishing.Properties;

namespace Fishing
{
    class DebugLogger : ILogger
    {
        private readonly Action<string, Color> Log;

        public DebugLogger(Action<string, Color> logFunc)
        {
            Log = logFunc;
        }
#if DEBUG
        public void Error(string message)
        {
            Log(string.Format(Resources.MessageFormatError, message), Color.Red);
        }

        public void Warning(string message)
        {
            Log(string.Format(Resources.MessageFormatWarning, message), Color.Yellow);
        }

        public void Info(string message)
        {
            Log(message, Color.White);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        public static string GetCurrentThread()
        {
            return Thread.CurrentThread.Name;
        }
#else
        public void Error(string message)
        {
        }

        public void Warning(string message)
        {
        }

        public void Info(string message)
        {
        }

        public static string GetCurrentMethod() {
            return string.Empty;
        }
#endif
    }
}
