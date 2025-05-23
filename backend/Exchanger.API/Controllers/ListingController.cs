using Exchanger.API.Data;
using Exchanger.API.DTOs.ListingDTOs;
using Exchanger.API.Enums.AuthErrors;
using Exchanger.API.Enums.ListingErrors;
using Exchanger.API.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exchanger.API.Controllers
{
    [ApiController]
    [Route("api/listing")]
    public class ListingController : ControllerBase
    {
        private readonly IListingService _listingService; 
        private readonly ILogger<ListingController> _logger;

        public ListingController( 
            IListingService listingService, 
            ILogger<ListingController> logger)
        {
            _listingService = listingService;
            _logger = logger;
        }

        [HttpPost("create")]
        [Authorize]
        public Task<IActionResult> CreateNew([FromForm] ListingCreationDTO listingCreationDTO) =>
            SafeExecuteAsync(async () =>
            {
                var userId = UserHelper.GetCurrentUserId(HttpContext);
                var result = await _listingService.CreateNewListingAsync(listingCreationDTO, userId);
                return HandleListingResult(result);
            }, "creating new listing");

        [HttpPost("search/by-params")]
        public Task<IActionResult> FindByParams([FromBody] ListingParams listingParams) =>
            SafeExecuteAsync(async () =>
            {
                var result = await _listingService.GetListingByParamsAsync(listingParams);
                return HandleListingResult(result);
            }, "searching by params");

        [HttpPost("search/by-title")]
        public Task<IActionResult> FindByTitle([FromBody] SearchByTitleDTO dto) =>
            SafeExecuteAsync(async () =>
            {
                var result = await _listingService.SearchByTitleAsync(
                    dto.Title, 
                    dto.Pagination.LastId, 
                    dto.Pagination.Limit);

                return HandleListingResult(result);
            }, "searching by title");

        [HttpPost("user-{userId}/listings")]
        public Task<IActionResult> GetUserListings([FromBody] PaginationDTO dto, Guid userId) =>
            SafeExecuteAsync(async () =>
            {
                var result = await _listingService.GetListingInfoByUserIdAsync(userId, dto.LastId, dto.Limit);
                return HandleListingResult(result);
            }, "getting user listings");

        [HttpDelete("{listingId}/delete")]
        [Authorize]
        public Task<IActionResult> DeleteListing(Guid listingId) =>
            SafeExecuteAsync(async () =>
            {
                var userId = UserHelper.GetCurrentUserId(HttpContext);
                var result = await _listingService.DeleteListingAsync(listingId, userId);
                return HandleListingResult(result);
            }, "deletion of listing");

        [HttpPatch("{listingId}/activate")]
        public Task<IActionResult> ActivateListing(Guid listingId) =>
            SafeExecuteAsync(async () =>
            {
                var userId = UserHelper.GetCurrentUserId(HttpContext);
                var result = await _listingService.ActivateListingAsync(listingId, userId);
                return HandleListingResult(result);
            }, "activation of listing");

        [HttpPatch("{listingId}/deactivate")]
        public Task<IActionResult> DeactivateListing(Guid listingId) =>
            SafeExecuteAsync(async () =>
            {
                var userId = UserHelper.GetCurrentUserId(HttpContext);
                var result = await _listingService.DeactivateListingAsync(listingId, userId);
                return HandleListingResult(result);
            }, "deactivation of listing");

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
                   result.Listing != null  ? Ok(result.Listing) : 
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
