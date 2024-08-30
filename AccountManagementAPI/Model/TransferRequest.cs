using System.ComponentModel.DataAnnotations;

namespace AccountManagementAPI.Model
{
    public class TransferRequest
    {

        public int PersonId { get; set; }

        [Required(ErrorMessage = "Source account number is required")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Source Account Number must be between 8 and 16 characters")]
        public string SourceAccountNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Target account number is required")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Target Account Number must be between 8 and 16 characters")]
        public string TargetAccountNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account is Required")]
        public decimal? Amount { get; set; }
        public DateTime? CreatedOn { get; set; }

    }
}
