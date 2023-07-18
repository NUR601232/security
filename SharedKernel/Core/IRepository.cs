using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Core
{
    public interface IRepository<T, in TId> where T : AggregateRoot<TId>
    {
        Task<T?> FindByIdAsync(TId id);

        Task CreateAsync(T obj);


    }
}
