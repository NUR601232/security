using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Core
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
