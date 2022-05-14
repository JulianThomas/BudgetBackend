using Budget;
using Budget.Authentication;
using Budget.Dtos;
using Budget.Entities;
using Budget.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Budget.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/Budget/v1/")]
    public class BudgetController : ControllerBase
    {
        private readonly IUsersRepository repository;
        private readonly UserManager<ApplicationUser> userManager;


        //protected string UserId => User.Claims.First(i => i.Type == ClaimTypes.Name).Value;
        //private string u => User.FindFirst(ClaimTypes.Email).Value;
        public BudgetController(IUsersRepository repository, UserManager<ApplicationUser> userManager)
        {
            //if (User.Identity.IsAuthenticated)
            //    id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //else id = Guid.NewGuid().ToString();
            this.repository = repository;
            this.userManager = userManager;
        }



        //Get
        //[Route("users")]
        //TODO: either remove the route as no one should be able to get other user info
        //      or set it to Admin role
        [Authorize(Roles ="Admin")]
        [HttpGet("users")]
        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await repository.GetUsersAsync();
            return users.Select(user => user.AsDto());
        }

        //[Route("user")]
        [HttpGet("user")]
        public async Task <ActionResult<UserDto>> GetUserAsync ()
        {
            //TODO: modify so it sends user's info - done
            var userId = await GetCurrentUserIdAsync();

            var user = await repository.GetUserAsync(userId);

            if (user is null)
                return NotFound();
            
            return user.AsDto();
        }
        //[Route("users")]
        [HttpPost("users")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<UserDto>> CreateUserAsync()
        {
            var userAccount = await GetCurrentUser();
            
            User user = new()
            {
                Id = new Guid(userAccount.Id),
                UserName = userAccount.Name,
                BudgetItems = new List<BudgetItem>()
            };
            await repository.CreateUserAsync(user);
            
            return CreatedAtAction(nameof(GetUserAsync), new { userId = user.Id }, user.AsDto());

        }


        //[Route("/items")]
        [HttpGet("items/{itemId}")]
        public async Task<ActionResult<BudgetItemDto>> GetItemAsync(Guid itemId)
        {
            var userId = await GetCurrentUserIdAsync();

            var item = await repository.GetBudgetItemAsync(userId, itemId);
            if (item == null)
                return NotFound();
            return item.AsItemDto();
        }

        //[Route("items")]
        [HttpPost("items")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<BudgetItemDto>> CreateItemAsync( CreateBudgetItemsDto itemDto)
        {
            var userId = await GetCurrentUserIdAsync();

            BudgetItem item = new()
            {
                ItemId = Guid.NewGuid(),
                ItemName = itemDto.ItemName,
                IsCredit = itemDto.IsCredit,
                Amount = itemDto.Amount,
                Date = itemDto.Date.HasValue ? itemDto.Date.Value : DateTime.UtcNow,
            };
            await repository.CreateBudgetItemAsync(userId, item);

            return CreatedAtAction(nameof(GetItemAsync), new { userId, item.ItemId }, item.AsItemDto());
        }

        //[Route("items")]
        [HttpDelete("items/{itemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteItemAsync(Guid itemId)
        {
            var userId = await GetCurrentUserIdAsync();

            var item = await repository.GetBudgetItemAsync(userId, itemId);
            if (item == null)
            {
                return NotFound();
            }
            await repository.DeleteBudgetItemAsync(userId, itemId);
            return NoContent();
        }

        //[Route("items")]
        [HttpPut("items/{itemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateItemAsync( UpdateBudgetItemsDto itemDto, Guid itemId)
        {
            //Console.WriteLine(itemDto.ToString(), itemId);
            var userId = await GetCurrentUserIdAsync();

            var existingItem = await repository.GetBudgetItemAsync(userId, itemId);
            if (existingItem == null)
                return NotFound();
            BudgetItem updatedItem = existingItem with
            {
                ItemName = itemDto.ItemName,
                Amount = itemDto.Amount,
                IsCredit = itemDto.IsCredit,
                Date = itemDto.Date,
            };
            await repository.UpdateBudgetItemAsync(userId, updatedItem);
            return NoContent();
        }
        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await userManager.FindByEmailAsync(User.Claims.First(i => i.Type == ClaimTypes.Email).Value);
        }

        private async Task<Guid> GetCurrentUserIdAsync ()
        {
            return new Guid((await GetCurrentUser()).Id);
        }
    }
}