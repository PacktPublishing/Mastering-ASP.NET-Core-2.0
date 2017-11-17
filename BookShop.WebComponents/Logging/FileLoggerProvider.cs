using Microsoft.Extensions.Logging;
using System;

namespace BookShop.WebComponents.Logging
{
    public sealed class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _path;
        private readonly Func<string, LogLevel, bool> _func;

        public FileLoggerProvider(string path, Func<string, LogLevel, bool> func)
        {
            this._path = path;
            this._func = func;
        }

        public FileLoggerProvider(string path, LogLevel minimumLogLevel) :
          this(path, (category, logLevel) => logLevel >= minimumLogLevel)
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName, this._path, this._func);
        }

        public void Dispose()
        {
        }
    }
}
