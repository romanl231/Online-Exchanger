using Exchanger.API.Entities;

namespace Exchanger.API.DTOs.ListingDTOs
{
    public class ListingParams
    {
        public decimal MaxValue { get; set; }
        public decimal MinValue { get; set; }
        public List<Category> Categories { get; set; }
    }
}
