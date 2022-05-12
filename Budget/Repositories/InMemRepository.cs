using Budget.Entities;
using System.Globalization;

namespace Budget.Repositories
{
    public class InMemRepository 
    {

        //private readonly List<FormattedUser> formattedUsers = new()
        //{
        //    new FormattedUser
        //    {
        //        Id = Guid.NewGuid(),
        //        UserName = "testname",
        //        Years = new()
        //        {
        //            new BudgetAccountYear
        //            {
        //                Year = 2022,
        //                Months = new List<BudgetAccountMonth>()
        //                {
        //                    new BudgetAccountMonth
        //                    {
        //                        Month = 1,
        //                        Total = 0,
        //                        BudgetItems = new List<BudgetItem>()
        //                        {
        //                            new BudgetItem { ItemName = "Salary", IsCredit = true, Amount= 0, Date = DateTime.Parse("02/01/2022") },
        //                            new BudgetItem { ItemName = "Internet", IsCredit = false, Amount= 0, Date= DateTime.Parse("24/01/2022") },
        //                            new BudgetItem { ItemName = "Electricity", IsCredit = false, Amount= 0, Date= DateTime.Parse("09/01/2022") },
        //                        }
        //                    }
        //                }
        //            }
        //        },
        //    }
        //};

        private readonly List<User> users = new()
        {
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "Julian",
                BudgetItems = new List<BudgetItem>
                {
                    new BudgetItem { ItemId=Guid.NewGuid(), ItemName = "Salary", IsCredit = true, Amount= 0, Date = DateTime.Parse("2022-01-02") },
                    new BudgetItem { ItemId=Guid.NewGuid(), ItemName = "Internet", IsCredit = false, Amount=0, Date= DateTime.Parse("2022-01-24") },
                    new BudgetItem { ItemId=Guid.NewGuid(), ItemName = "Electricity", IsCredit=false, Amount=0, Date= DateTime.Parse("2022-01-09") },
                }
            }
        };

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await Task.FromResult(users);
        }
        public async Task<User> GetUserAsync(Guid id)
        {
            var user = users.Where(user => user.Id == id).SingleOrDefault();
            return await Task.FromResult(user);
        }

        public async Task<BudgetItem> GetBudgetItemAsync(Guid userId, Guid itemId)
        {
            var user = FilterUser(userId);
            //var index = users[user].BudgetItems.FindIndex(existingItem => existingItem.ItemId == itemId);
            var item = users[user].BudgetItems.Where(existingItem => existingItem.ItemId == itemId).SingleOrDefault();
            return await Task.FromResult(item);
        }

        public async Task CreateBudgetItemAsync(Guid userId, BudgetItem item)
        {

            var user = FilterUser(userId);
            var year = item.Date.Year;
            var month = item.Date.Month;
            if (user == -1)
                users.Add(new User { Id = userId, UserName = "Guest", BudgetItems = new List<BudgetItem>() { item } });
            else users[user].BudgetItems.Add(item);
            await Task.CompletedTask;
        }

        public async Task DeleteBudgetItemAsync (Guid userId, Guid itemId)
        {
            var user = FilterUser(userId);
            //var index = users.Where(user => user.Id == userId).Single().BudgetItems
            //    .FindIndex(existingItem => existingItem.ItemId == itemId);
            var index = users[user].BudgetItems.FindIndex(existingItem => existingItem.ItemId == itemId);
            users[user].BudgetItems.RemoveAt(index);
            await Task.CompletedTask;
        }

        public async Task UpdateBudgetItemAsync(Guid userId, BudgetItem item)
        {
            var user = FilterUser(userId);
            var index = users[user].BudgetItems.FindIndex(existingItem => existingItem.ItemId == item.ItemId);
            users[user].BudgetItems[index] = item;
            await Task.CompletedTask;
        }


        private int FilterUser(Guid userId)
        {
            //DefaultIfEmpty( new User { }).
            return users.FindIndex(user => user.Id == userId);
        }





        //private void GetItems()
        //{
        //    //placeholder function to implement formatted structure
        //    var user = users.Where(user => user.Id == id).SingleOrDefault();
        //    var year = user.Years.Where(Year => Year.Year == item.Date.Year).SingleOrDefault();
        //    var month = year.Months.Where(Month => Month.Month == item.Date.Month).SingleOrDefault();
        //}

        //public void UpdateUser(Guid id, User user)
        //{
        //    var index = users.FindIndex(oldUser => oldUser.Id == user.Id);
        //    users[index] = user;
        //}
        //public BudgetAccountYear? GetYear(Guid id, int Year)
        //{
        //    var user = users.Where(user => user.Id == id).SingleOrDefault();
        //    if (user == null)
        //    {
        //        return null;
        //    }
        //    var year = user.Years.Where(year => year.Year == Year).SingleOrDefault();
        //    if (year == null)
        //        return new BudgetAccountYear() { Year = Year, Months = new List<BudgetAccountMonth>(12)};
        //    return year;
        //}
        //public void CreateMonth(Guid id, BudgetAccountMonth month, int year)
        //{
        //    var user = users.Where(user => user.Id == id).SingleOrDefault();
        //    if (user == null)
        //    {
        //        //return null;
        //    }
        //    user.Years.Where(Year => Year.Year == year).SingleOrDefault().Months.Add(month);
        //}
        //public void CreateYear (Guid id, BudgetAccountYear year)
        //{
        //    users.Where(user => user.Id ==id).SingleOrDefault().Years.Add(year);
        //}
    }
}
