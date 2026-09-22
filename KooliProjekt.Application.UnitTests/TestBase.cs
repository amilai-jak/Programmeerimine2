using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.UnitTests
{
    public abstract class TestBase : IDisposable
    {
        private ApplicationDbContext _dbContext;
        private bool disposedValue;

        protected ApplicationDbContext DbContext
        {
            get
            {
                if (_dbContext != null)
                {
                    return _dbContext;
                }

                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;
                _dbContext = new ApplicationDbContext(options);
                return _dbContext;
            }
        }

        /// <summary>
        /// Andmebaasi kontekst, millel puudub provider.
        /// Iga paring selle konteksti vastu viskab erindi - nii saab kontrollida,
        /// kas handler ulesandele vastates andmebaasi poole uldse poordub.
        /// </summary>
        protected ApplicationDbContext GetFaultyDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>();

            return new ApplicationDbContext(options.Options);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
