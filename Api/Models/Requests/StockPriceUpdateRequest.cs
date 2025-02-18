using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests
{
    public class StockPriceUpdateRequest
    {
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal NewPrice { get; set; }
    }
} 