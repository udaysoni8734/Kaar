using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests
{
    public class CreateUserPreferenceRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(10)]
        public string StockSymbol { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Threshold price must be greater than 0")]
        public decimal ThresholdPrice { get; set; }
    }
} 