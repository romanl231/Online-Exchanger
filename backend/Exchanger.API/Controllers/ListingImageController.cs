using Exchanger.API.Data;
using Exchanger.API.Enums.ListingErrors;
using Exchanger.API.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exchanger.API.Controllers
{
    [ApiController]
    [Route("api/listing/image")]
    public class ListingImageController : ControllerBase
    {
        private readonly IListingImageService _imageService;
        private readonly ILogger<ListingImageController> _logger;

        public ListingImageController(
            IListingImageService imageService,
            ILogger<ListingImageController> logger)
        {
            _imageService = imageService;
            _logger = logger;
        }

        [HttpPost("{listingId}/image/add")]
        [Authorize]
        public Task<IActionResult> AddListingImage([FromForm] List<IFormFile> images, Guid listingId) =>
        SafeExecuteAsync(async () =>
        {
            var userId = UserHelper.GetCurrentUserId(HttpContext);
            var result = await _imageService.AddImageAsync(images, listingId, userId);
            return HandleListingResult(result);
        }, "addition of new images");

        [HttpDelete("{listingId}/image/delete/{avatarUrl}")]
        [Authorize]
        public Task<IActionResult> DeleteListingImage(string avatarUrl, Guid listingId) =>
            SafeExecuteAsync(async () =>
            {
                var userId = UserHelper.GetCurrentUserId(HttpContext);
                var result = await _imageService.DeleteImageAsync(listingId, userId, avatarUrl);
                return HandleListingResult(result);
            }, "deletion of images");

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
