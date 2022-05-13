
using System.ComponentModel.DataAnnotations;

namespace Budget.Entities
{
    public record User
    {
        [EmailAddress]
        public Guid Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string UserName { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
                              
        public List<BudgetItem>? BudgetItems { get; set; }
    }

    //// Gonna possibly add back after fixing the fronend
    //// or might just end up doing the model on the frontend
    //public record FormattedUser
    //{
    //    public Guid Id { get; set; }
    //    public string UserName { get; set; }
    //    public List<BudgetAccountYear> Years { get; set; }

    //}


    // //Possibly add back in the future
    //public record BudgetAccountYear
    //{
    //    public int Year { get; set; }
    //    public List<BudgetAccountMonth>? Months { get; set; }
    //}


    //// Possibly Add back in the future
    //public record BudgetAccountMonth
    //{
    //    public int Month { get; set; }
    //    //public List<BudgetCategory>? BudgetCategories { get; set; }
    //    public List<BudgetItem> BudgetItems { get; set; }
    //    public decimal Total { get; set; }
    //}

    // //Possibly add back in the future
    //public record BudgetCategory
    //{
    //    public string CategoryName { get; set; }
    //    public int Sum { get; set; }
    //    public List<BudgetItem>? Item { get; set; }
    //}

    public record BudgetItem
    {
        public Guid ItemId { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string ItemName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public bool IsCredit { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

}
