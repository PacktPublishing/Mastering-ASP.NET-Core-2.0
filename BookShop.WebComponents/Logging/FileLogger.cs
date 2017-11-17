using System;
using Microsoft.Extensions.Logging;
using System.IO;

namespace BookShop.WebComponents.Logging
{
    public sealed class FileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly string _path;
        private readonly Func<string, LogLevel, bool> _func;

        public FileLogger(string categoryName, string path, Func<string, LogLevel, bool> func)
        {
            this._categoryName = categoryName;
            this._path = path;
            this._func = func;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return EmptyDisposable.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return this._func(this._categoryName, logLevel);
        }

        public void Log<TState>(
          LogLevel logLevel,
          EventId eventId,
          TState state,
          Exception exception,
          Func<TState, Exception, string> formatter)
        {
            if (this.IsEnabled(logLevel) == true)
            {
                var now = DateTime.UtcNow;
                var today = now.ToString("yyyy-MM-dd");
                var fileName = $"{this._categoryName}_{today}.log";
                var message = formatter(state, exception);

                File.AppendAllText(Path.Combine(this._path, fileName), $"{message}\n");
            }
        }
    }
}