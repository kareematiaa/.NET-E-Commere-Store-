using Amazon.Core;
using Amazon.Core.Entities;
using Amazon.Core.Repositories;
using Amazon.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;

        private Hashtable _repositories;    //hashtable value is object and key is object (hashtable has key and value)

        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity>? Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repositories is null)
                _repositories = new Hashtable();

            var type =typeof(TEntity).Name; // like product

            if(!_repositories.ContainsKey(type))
            {
                var repositrory = new GenericRepository<TEntity>(_dbContext);

                _repositories.Add(type, repositrory);
            }

            return _repositories[type] as IGenericRepository<TEntity>;
        }

        public async Task<int> Complete()
           => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();

       
    }
}
