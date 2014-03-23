using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using Fishing.Properties;
using JetBrains.Annotations;

namespace Fishing
{
    class DebugLogger : Logger
    {
        public DebugLogger(Action<string, Color> logFunc) : base(logFunc)
        {
        }
#if DEBUG
        public override void Error(string message, params object[] args)
        {
            Log(Resources.MessageError + string.Format(message, args), Color.Red);
        }

        public override void Warning(string message, params object[] args)
        {
            Log(Resources.MessageWarning + string.Format(message, args), Color.Yellow);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace(1, true);
            StackFrame sf = st.GetFrame(0);
            string filename = Path.GetFileName(sf.GetFileName());

            return string.Format("{0}:{1}:{2}", filename, sf.GetFileLineNumber(), sf.GetMethod().Name);
        }

        public static string GetCurrentThread()
        {
            return Thread.CurrentThread.Name;
        }
#else
        public override void Error(string message, params object[] args)
        {
        }

        public override void Warning(string message, params object[] args)
        {
        }

        public override void Info(string message, params object[] args)
        {
        }

        public static string GetCurrentMethod() {
            return string.Empty;
        }

        public static string GetCurrentThread()
        {
            return string.Empty;
        }
#endif
    }
}
