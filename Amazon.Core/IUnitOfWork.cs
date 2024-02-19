using Amazon.Core.Entities;
using Amazon.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core
{
    public interface IUnitOfWork: IAsyncDisposable 
    {

        IGenericRepository<TEntity>? Repository<TEntity>() where TEntity : BaseEntity; 

        Task<int> Complete();

    }
}
