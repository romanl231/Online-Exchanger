using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.Entities
{
    public class ListingCategory
    {
        [Required]
        public Guid ListingId { get; set; }
        public Listing Listing { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
