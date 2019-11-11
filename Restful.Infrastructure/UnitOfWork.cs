using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Restful.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyContext context;

        public UnitOfWork(MyContext context)
        {
            this.context = context;
        }

        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
