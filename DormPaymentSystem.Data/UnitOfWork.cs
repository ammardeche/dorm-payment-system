using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Data;

namespace DormPaymentSystem.Data
{
    public class UnitOfWork : IUnitOfWork
    {


        private readonly DormPaymentDbContext _context;
        public UnitOfWork(DormPaymentDbContext context) => _context = context;
        

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
           await using var tx = await _context.Database.BeginTransactionAsync();
        try
        {
            await action();
            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
        }
    }
}