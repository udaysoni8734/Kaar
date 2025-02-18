using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests
{
    public class UpdateUserPreferenceRequest
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Threshold price must be greater than 0")]
        public decimal NewThresholdPrice { get; set; }
    }
} 