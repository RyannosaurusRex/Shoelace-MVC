using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using ShoelaceMVC.Models;
using ShoelaceMVC.Models;

namespace ShoelaceMVC.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private int tenantId;
        private ShoelaceDbContext context = new ShoelaceDbContext();
        private GenericRepository<Person> _personRepository;
        
        public UnitOfWork(int tenantId)
        {
            this.tenantId = tenantId;
        }

        public GenericRepository<Person> PersonRepository
        {
            get
            {
                if (this._personRepository == null)
                {
                    this._personRepository = new GenericRepository<Person>(context, tenantId);
                }
                return _personRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
