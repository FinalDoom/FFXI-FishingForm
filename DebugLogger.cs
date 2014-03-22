using System;
using System.Drawing;
using Fishing.Properties;

namespace Fishing
{
    class DebugLogger
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
#endif
    }
}
