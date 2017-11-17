using BookShop.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace BookShop.UnitTests
{
    public class IntegrationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public IntegrationTests()
        {
            var asm = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var path = PlatformServices.Default.Application.ApplicationBasePath;
            var contentPath = Path.GetFullPath(Path.Combine(path, $@"..\..\..\..\{asm}"));

            this._server = new TestServer(new WebHostBuilder()
               .UseContentRoot(contentPath)
               .UseStartup<Startup>()
               .UseEnvironment("Development"));
            this._server.BaseAddress = new Uri("http://localhost:8000");

            this._client = this._server.CreateClient();
        }

        [Fact]
        public async Task CanInvokeController()
        {
            var response = await this._client.GetAsync("/Home/About");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Welcome", content);
        }
    }
}
