using Microsoft.AspNetCore.Mvc.Testing;

using System;
using System.Threading.Tasks;

using UpToDocu;

using Xunit;
//https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0
namespace PocWebAppTest {
    public class UnitTest1 : IClassFixture<UpToDocuWebApplicationFactory<PocWebApp.Startup>> {
        private readonly UpToDocuWebApplicationFactory<PocWebApp.Startup> _Factory;

        public UnitTest1(UpToDocuWebApplicationFactory<PocWebApp.Startup> factory) {
            this._Factory = factory;
        }
        [Fact]
        public async Task Test1Async() {
            using var client= this._Factory.CreateClient();
            var response = await client.GetAsync("/");
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
