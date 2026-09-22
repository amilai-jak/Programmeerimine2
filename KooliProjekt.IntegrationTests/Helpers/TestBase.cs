using System;
using System.Net.Http;
using KooliProjekt.Application.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KooliProjekt.IntegrationTests.Helpers
{
    // 19.02.2026 - opetaja naitest voetud klass
    public abstract class TestBase : IDisposable
    {
        private ApplicationDbContext _dbContext;
        private IServiceScope _scope;

        public WebApplicationFactory<FakeStartup> Factory { get; private set; }
        public HttpClient Client { get; private set; }

        public TestBase()
        {
            Factory = new TestApplicationFactory<FakeStartup>();
            Client = Factory.CreateClient();
        }

        protected ApplicationDbContext DbContext
        {
            get
            {
                if (_dbContext != null)
                {
                    return _dbContext;
                }

                _scope = Factory.Services.CreateScope();
                _dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                return _dbContext;
            }
        }

        public void Dispose()
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureDeleted();
            }

            if (_scope != null)
            {
                _scope.Dispose();
                _scope = null;
                _dbContext = null;
            }

            if (Factory != null)
            {
                Factory.Dispose();
                Factory = null;
            }

            if (Client != null)
            {
                Client.Dispose();
                Client = null;
            }
        }
    }
}
