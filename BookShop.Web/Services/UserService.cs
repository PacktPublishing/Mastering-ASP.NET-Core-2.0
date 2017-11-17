using BookShop.DomainModel;
using System.Diagnostics;

namespace BookShop.Web.Services
{
    public class UserService : IUserService
    {
        private readonly BookShopContext _ctx;
        private readonly DiagnosticSource _diagnosticSource;

        public UserService(BookShopContext ctx, DiagnosticSource diagnosticSource)
        {
            this._ctx = ctx;
            this._diagnosticSource = diagnosticSource;
        }

        public int AddUser(string name, string email)
        {
            var user = new User { Name = name, Email = email };
            this._ctx.Users.Add(user);
            this._ctx.SaveChanges();
            this._diagnosticSource.Write("OnUserAdded", new { userId = user.UserId });
            return user.UserId;
        }
    }
}
