using WebAPI.Repository;
using WebAPI.Repository.Data;

namespace WebAPI.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public async Task Can_Add_And_Retrieve_Account()
        {

            var repo = new AccountRepository();
            
            var account = new Account() { Email = "khalid@kkk.com", Name = "Khalid" };

            await repo.AddAsync(account);


            var result = await repo.GetByIdAsync(account.Id);


            Assert.NotNull(result);
            Assert.Equal("Khalid", result.Name);

        }


        [Fact]
        public async Task Can_Add_Subscription_To_Account()
        {

            var repo = new AccountRepository();

            var account = new Account() { Email = "khalid@kkk.com", Name = "Khalid" };

            var sub1 = new Subscription() { PlanName = "Plan 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30)};

            account.Subscriptions.Add(sub1);

            await repo.AddAsync(account);


            var result = await repo.GetByIdAsync(account.Id);


            Assert.NotNull(result);
            Assert.NotNull(result.Subscriptions);
            Assert.Single(result.Subscriptions);
            Assert.Equal("Plan 1", result.Subscriptions.FirstOrDefault()?.PlanName);

        }

        [Fact]
        public async Task Can_Update_Account()
        {

            var repo = new AccountRepository();

            var account = new Account() { Email = "khalid@kkk.com", Name = "Khalid" };

            await repo.AddAsync(account);

            account.Name = "Khalid Wasti";

            await repo.UpdateAsync(account);

            var result = await repo.GetByIdAsync(account.Id);


            Assert.NotNull(result);
            Assert.Equal("Khalid Wasti", result.Name);

        }

        [Fact]
        public async Task Can_Delete_Account()
        {

            var repo = new AccountRepository();

            var account = new Account() { Email = "khalid@kkk.com", Name = "Khalid" };

            await repo.AddAsync(account);

            await repo.DeleteAsync(account.Id);

            var result = await repo.GetByIdAsync(account.Id);


            Assert.Null(result);

        }


    }

}