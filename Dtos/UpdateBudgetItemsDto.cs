using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Budget.Dtos
{
    public record UpdateBudgetItemsDto
    {

        
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string ItemName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public bool IsCredit { get; set; }
        
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
