using Exchanger.API.Data;
using Exchanger.API.Entities;
using Exchanger.API.Enums.ListingErrors;
using Exchanger.API.Services;
using Exchanger.API.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exchanger.API.Controllers
{
    [ApiController]
    [Route("api/listing/category")]
    public class ListingCategoryController : ControllerBase
    {
        private readonly IListingCategoryService _categoryService;
        private readonly ILogger<ListingCategoryController> _logger;

        public ListingCategoryController(
            IListingCategoryService categoryService,
            ILogger<ListingCategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpPost("listing-{listingId}/add")]
        [Authorize]
        public Task<IActionResult> AddListingCategory([FromBody] int[] categoryIds, Guid listingId) =>
           SafeExecuteAsync(async () =>
           {
               var userId = UserHelper.GetCurrentUserId(HttpContext);
               var result = await _categoryService.AddCategorysAsync(userId, listingId, categoryIds);
               return HandleListingResult(result);
           }, "addition of new categories");

        [HttpDelete("listing-{listingId}/{categoryId}")]
        public Task<IActionResult> DeleteListingCategory(Guid listingId, int categoryId) =>
        SafeExecuteAsync(async () =>
           {
               var userId = UserHelper.GetCurrentUserId(HttpContext);
               var result = await _categoryService.DeleteCategoryAsync(userId, listingId, categoryId);
               return HandleListingResult(result);
           }, "deletion of category");

        [HttpGet("categories/all")]
        public Task<IActionResult> GetAllCategories() =>
            SafeExecuteAsync(async () =>
            {
                return Ok(await _categoryService.GetAllCategoriesAsync());
            }, "getting categories");

        private IActionResult HandleListingResult(ListingResult result)
        {
            if (!result.IsSuccess)
            {
                if (ListingErrorMessages.Messages
                    .TryGetValue(result.ErrorCode.Value, out var message))
                    return BadRequest(message);

                return BadRequest("Unknown error");
            }

            return result.Listings != null ? Ok(result.Listings) :
                   result.Listing != null ? Ok(result.Listing) :
                                             Ok();
        }

        private async Task<IActionResult> SafeExecuteAsync(Func<Task<IActionResult>> action, string logContext)
        {
            try
            {
                return await action();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred during {logContext}.");
                return StatusCode(500, "An unexpected error occurred");
            }
        }
    }
}