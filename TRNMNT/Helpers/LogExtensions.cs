using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;

namespace TRNMNT.Web.Helpers
{
    public static class LogExtensions
    {
        public static void LogCustom(this ILogger logger, LogLevel level, string message, params object[] args)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            logger.Log(level, 0, new FormattedLogValues(message, args), null, MessageFormatter);
        }

        private static string MessageFormatter(object state, Exception error)
        {
            return state.ToString();
        }
    }
}