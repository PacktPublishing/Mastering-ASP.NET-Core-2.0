using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace BookShop.WebComponents.Logging
{
    public static class LoggerFactoryExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory, Func<string, LogLevel, bool> func)
        {
            return AddFile(loggerFactory, Directory.GetCurrentDirectory(), func);
        }

        public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory, string path, Func<string, LogLevel, bool> func)
        {
            loggerFactory.AddProvider(new FileLoggerProvider(path, func));
            return loggerFactory;
        }


        public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory, string path, LogLevel minimumLogLevel)
        {
            return AddFile(loggerFactory, path, (category, logLevel) => logLevel >= minimumLogLevel);
        }

        public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory, LogLevel minimumLogLevel)
        {
            return AddFile(loggerFactory, Directory.GetCurrentDirectory(), minimumLogLevel);
        }
    }
}
