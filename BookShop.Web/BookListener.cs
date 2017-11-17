using Microsoft.Extensions.DiagnosticAdapter;
using System;

namespace BookShop.Web
{
    public class BookListener
    {
        [DiagnosticName(nameof(OnOrderCreated))]
        public void OnOrderCreated(int orderId)
        {
        }

        [DiagnosticName(nameof(OnAuthorCreated))]
        public void OnAuthorCreated(int authorId)
        {
        }

        [DiagnosticName(nameof(OnBookCreated))]
        public void OnBookCreated(int bookId)
        {
        }
    }
}
