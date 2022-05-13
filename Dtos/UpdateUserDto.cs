using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Budget.Dtos
{
    public record UserDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string UserName { get; init; }

        [Required]
        public List<BudgetItemDto>? BudgetItemDtos { get; set; }
    }


}

