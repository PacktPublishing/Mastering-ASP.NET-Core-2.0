using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Web.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            var ctx = this.Context.Connection.GetHttpContext();
            var user = this.Context.User.Identity.Name;
            var bookId = ctx.Request.Query["bookid"].SingleOrDefault();

            if (string.IsNullOrWhiteSpace(user))
            {
                user = $"anonymous@{this.Context.Connection.GetHttpContext().Connection.RemoteIpAddress}";
            }

            if (string.IsNullOrWhiteSpace(bookId) == false)
            {            
                await this.Clients.Group($"Book:{bookId}").InvokeAsync("Send", new { user = user, message = message });
            }
        }

        public override Task OnConnectedAsync()
        {
            var ctx = this.Context.Connection.GetHttpContext();
            var bookId = ctx.Request.Query["bookid"].SingleOrDefault();

            if (string.IsNullOrWhiteSpace(bookId) == false)
            {
                this.Groups.AddAsync(this.Context.ConnectionId, $"Book:{bookId}");
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
