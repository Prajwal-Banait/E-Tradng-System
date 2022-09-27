using ETradingSystem1.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETradingSystem1.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
        }

        [Required(ErrorMessage = "CustomerId is required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "CustomerName is required")]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Contact is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Contact No. should be of 10 digits")]
        public string? Contact { get; set; }

        [Required(ErrorMessage = "MailId is required")]
        public string? MailId { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
