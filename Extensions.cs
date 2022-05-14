using Budget.Dtos;
using Budget.Entities;


namespace Budget
{
    public static class Extensions
    {
        public static UserDto AsDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                BudgetItemDtos = user.BudgetItems.ConvertAll(new Converter<BudgetItem, BudgetItemDto>(AsItemDto)),
            };
        }


        public static BudgetItemDto AsItemDto(this BudgetItem item)
        {
            
            return new BudgetItemDto
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                Amount = item.Amount,
                IsCredit = item.IsCredit,
                Date = item.Date,
            };
        }
    }
}
