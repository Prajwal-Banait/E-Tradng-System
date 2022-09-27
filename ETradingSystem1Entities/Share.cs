using ETradingSystem1.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETradingSystem1.Entities
{
    public partial class Share
    {
        public Share()
        {
            Accounts = new HashSet<Account>();
        }

        [Required(ErrorMessage = "ShareId is required")]
        public int ShareId { get; set; }

        [Required(ErrorMessage = "SharesName is required")]
        public string? SharesName { get; set; }

        [Required(ErrorMessage = "SharePrice is required")]
        public decimal? SharePrice { get; set; }

        [Required(ErrorMessage = "ShareQuantity is required")]
        public int? ShareQuantity { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
