using Amazon.Core.Entities;
using Amazon.Core.Repositories;
using Amazon.Core.Specifications;
using Amazon.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext ) //ask clr creating object
        {
            _dbContext = dbContext;
        }

    

        public async Task<IReadOnlyList<T>> GetAllAsync()

        {
            //((mosaken)) solution of specification design patern
            //if(typeof(T)==typeof(Product))
            //    return (IEnumerable<T>) await _dbContext.Products.Include(p => p.productBrand).Include(p=>p.productType).ToListAsync();
            //else
                return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id); 
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync(); 
        }

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec) 
        {
           return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec); 
         }


        public async Task Add(T entity)
          => await _dbContext.Set<T>().AddAsync(entity);
        

        public void Update(T entity)
          => _dbContext.Set<T>().Update(entity);
        

        public void Delete(T entity)
          => _dbContext.Set<T>().Remove(entity);
        

    }
}
