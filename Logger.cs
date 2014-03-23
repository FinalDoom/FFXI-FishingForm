using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Fishing
{
    /// <summary>
    /// Suggestion: Constructor takes 
    /// Action<string, Color> Log
    /// This is a function that should be passed to the constructor and
    /// set by the constructor. Each method can rely on this function
    /// to output its message, abstracting from where the logger is created.
    /// </summary>
    abstract class Logger : ILogger
    {
        /// <summary>
        /// This is a function, set through the constructor, used for logging
        /// with Logger classes. This enables the logger to be abstracted from
        /// whatever uses it.
        /// </summary>
        protected readonly Action<string, Color> Log;

        /// <summary>
        /// Default constructor sets the Log function. Any implementing classes
        /// should do the same, or call this constructor.
        /// </summary>
        /// <param name="logFunc">The logging function to set</param>
        protected Logger(Action<string, Color> logFunc)
        {
            Log = logFunc;
        }

        /// <summary>
        /// Called to format and output a message termed log level Error.
        /// Should use the Log property/method.
        /// </summary>
        /// <param name="message">the Error class message to format and output</param>
        /// <param name="args">Optional arguments for use in string.format</param>
        [StringFormatMethodAttribute("message")]
        public virtual void Error(string message, params object[] args)
        {
#if DEBUG
            Log(args.Length > 0 ? string.Format(message, args) : message, Color.Red);
#endif
        }

        /// <summary>
        /// Called to format and output a message termed log level Warning.
        /// Should use the Log property/method.
        /// </summary>
        /// <param name="message">the Warning class message to format and output</param>
        /// <param name="args">Optional arguments for use in string.format</param>
        [StringFormatMethodAttribute("message")]
        public virtual void Warning(string message, params object[] args)
        {
#if DEBUG
            Log(args.Length > 0 ? string.Format(message, args) : message, Color.Yellow);
#endif
        }

        /// <summary>
        /// Called to format and output a message termed log level Info.
        /// Should use the Log property/method.
        /// </summary>
        /// <param name="message">the Info class message to format and output</param>
        /// <param name="args">Optional arguments for use in string.format</param>
        [StringFormatMethodAttribute("message")]
        public virtual void Info(string message, params object[] args)
        {
#if DEBUG
            Log(args.Length > 0 ? string.Format(message, args) : message, Color.White);
#endif
        }
    }
}
