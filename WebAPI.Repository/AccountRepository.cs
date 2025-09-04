using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Repository.Data;

namespace WebAPI.Repository
{
    public class AccountRepository : IRepository<Account>
    {
        
        private readonly List<Account> accounts;

        public AccountRepository() { 

            accounts = new List<Account>();
        }

        public Task AddAsync(Account entity)
        {

            accounts.Add(entity);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var accountToDelete = accounts.FirstOrDefault();

            if (accountToDelete != null)
            {
                accounts.Remove(accountToDelete);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Account>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Account>>(accounts);
        }

        public Task<Account?> GetByIdAsync(Guid id)
        {
            var accountToGet = accounts.FirstOrDefault(a => a.Id == id);

            return Task.FromResult(accountToGet);
        }

        public Task UpdateAsync(Account entity)
        {
            
            var index = accounts.FindIndex(a => a.Id == entity.Id);

            if (index >= 0) { 

                accounts[index] = entity;
            }

            return Task.CompletedTask;

        }
    }
}
