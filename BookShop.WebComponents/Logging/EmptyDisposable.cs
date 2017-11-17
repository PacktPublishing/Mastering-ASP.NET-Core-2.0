using System;

namespace BookShop.WebComponents.Logging
{
    internal sealed class EmptyDisposable : IDisposable
    {
        public static readonly IDisposable Instance = new EmptyDisposable();

        private EmptyDisposable()
        {
        }

        public void Dispose() { }
    }
}