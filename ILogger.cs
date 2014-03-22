using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Fishing
{
    /// <summary>
    /// Suggestion: Constructor takes 
    /// Action<string, Color> Log
    /// This is a function that should be passed to the constructor and
    /// set by the constructor. Each method can rely on this function
    /// to output its message, abstracting from where the logger is created.
    /// </summary>
    interface ILogger
    {
        /// <summary>
        /// Called to format and output a message termed log level Error.
        /// Should use the Log property/method.
        /// </summary>
        /// <param name="message">the Error class message to format and output</param>
        void Error(string message);

        /// <summary>
        /// Called to format and output a message termed log level Warning.
        /// Should use the Log property/method.
        /// </summary>
        /// <param name="message">the Warning class message to format and output</param>
        void Warning(string message);

        /// <summary>
        /// Called to format and output a message termed log level Info.
        /// Should use the Log property/method.
        /// </summary>
        /// <param name="message">the Info class message to format and output</param>
        void Info(string message);
    }
}
