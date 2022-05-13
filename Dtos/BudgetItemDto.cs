using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Budget.Dtos
{
    public record BudgetItemDto
    {
        [Required]
        public Guid ItemId { get; set; }
        [Required]
        public string ItemName { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsCredit { get; set; }
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}