using System;
using System.Reflection;
using System.Xml;
using log4net;
using Microsoft.Extensions.Logging;

namespace TRNMNT.Core.Logger
{
    public class Log4NetLogger : ILogger
    {
        #region Dependencies

        private readonly ILog _log;

        #endregion

        #region Properties

        private readonly string _name;
        private readonly XmlElement _xmlElement;

        #endregion

        #region .ctor

        public Log4NetLogger(string name, XmlElement xmlElement)
        {
            _name = name;
            _xmlElement = xmlElement;
            var loggerRepository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            _log = LogManager.GetLogger(loggerRepository.Name, name);
            log4net.Config.XmlConfigurator.Configure(loggerRepository, xmlElement);
        }

        #endregion

        #region Public Metods

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return _log.IsDebugEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Information:
                    return _log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _log.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }
            var message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        _log.Fatal(message);
                        break;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        _log.Debug(message);
                        break;
                    case LogLevel.Error:
                        _log.Error(message);
                        break;
                    case LogLevel.Information:
                        _log.Info(message);
                        break;
                    case LogLevel.Warning:
                        _log.Warn(message);
                        break;
                    default:
                        _log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        _log.Info(message, exception);
                        break;
                }
            }
        }

        #endregion
    }
}