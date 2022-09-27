using System.ComponentModel.DataAnnotations;

namespace ETradingSystem1.Entities
{
    public partial class Account
    {
        [Required(ErrorMessage = "AccountID is required")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Customer Name is required")]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Customer Id is required")]
        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "ShareId is required")]
        public int? ShareId { get; set; }

        [Required(ErrorMessage = "Share Name is required")]
        public string? ShareName { get; set; }

        [Required(ErrorMessage = "ShareQuantity Name is required")]
        public int? ShareQuantity { get; set; }

        [Required(ErrorMessage = "Share Price is required")]
        public decimal? SharePrice { get; set; }

        [Required(ErrorMessage = "AccountBalance is required")]
        public decimal? AccountBalance { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Share? Share { get; set; }
    }
}
