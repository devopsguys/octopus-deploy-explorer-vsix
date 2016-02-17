// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogService.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Implementation of the log service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using NLog;
    using NLog.Config;
    using NLog.Targets;

    /// <summary>
    /// Implementation of the log service.
    /// </summary>
    /// <seealso cref="DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services.ILogService" />
    internal class LogService : ILogService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly Logger logger;

        /// <summary>
        /// The logging.
        /// </summary>
        private readonly bool logging;

        /// <summary>
        /// Initialises a new instance of the <see cref="LogService" /> class.
        /// </summary>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "To Review")]
        public LogService()
        {
            this.logging = false;

            // Logging configuration
            var logConfig = new LoggingConfiguration();

            // Target
            var fileTarget = new FileTarget
                                 {
                                     CreateDirs = true,
                                     Layout = "${longdate}|${level:uppercase=true}|${message}|${exception}",
                                     FileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\logs\\DevOpsGuys.Vsix.Explorer.OctopusDeploy.log",
                                     ArchiveNumbering = ArchiveNumberingMode.Date,
                                     ArchiveEvery = FileArchivePeriod.Day
                                 };
            logConfig.AddTarget("file", fileTarget);

            // Rule
            logConfig.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, fileTarget));

            LogManager.Configuration = logConfig;
            this.logger = LogManager.GetLogger("DevOpsGuys.Vsix.Explorer.OctopusDeploy");
        }

        /// <summary>
        /// Logs a message as a trace.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Trace(string message)
        {
            if (this.logging)
            {
                this.logger.Trace("{0}|{1}", GetCalleeMethod(), message);
            }
        }

        /// <summary>
        /// Logs a message as information.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            if (this.logging)
            {
                this.logger.Info("{0}|{1}", GetCalleeMethod(), message);
            }
        }

        /// <summary>
        /// Logs a message as a warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(string message)
        {
            if (this.logging)
            {
                this.logger.Warn("{0}|{1}", GetCalleeMethod(), message);
            }
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Error(Exception exception)
        {
            if (this.logging)
            {
                this.logger.Error(exception, GetCalleeMethod());
            }
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Error(string message, Exception exception)
        {
            if (this.logging)
            {
                this.logger.Error(exception, string.Format(CultureInfo.InvariantCulture, "{0}|{1}", GetCalleeMethod(), message));
            }
        }

        /// <summary>
        /// Gets the callee method.
        /// </summary>
        /// <returns>
        ///   <see cref="String"/>.
        /// </returns>
        private static string GetCalleeMethod()
        {
            var method = new StackFrame(2).GetMethod();
            var methodSignature = new StringBuilder();

            foreach (var param in method.GetParameters())
            {
                if (methodSignature.Length == 0)
                {
                    methodSignature.Append(param.Name);
                }
                else
                {
                    methodSignature.AppendFormat(",{0}", param.Name);
                }
            }

            return method.DeclaringType == null ? string.Empty : string.Format(CultureInfo.InvariantCulture, "{0}.{1}({2})", method.DeclaringType.FullName, method.Name, methodSignature);
        }
    }
}
