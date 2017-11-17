using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BookShop.Web.Hubs
{
    public class TimerHub : Hub
    {
        public Task Notify()
        {
            return Task.CompletedTask;
        }
    }
}
