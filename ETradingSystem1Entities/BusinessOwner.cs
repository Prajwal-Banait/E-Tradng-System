using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETradingSystem1.Entities
{
    public partial class BusinessOwner
    {
        [Required(ErrorMessage = "BusinessOwnerId is required")]
        public int BusinessOwnerId { get; set; }

        [Required(ErrorMessage = " Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Contact is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Contact No. should be of 10 digits")]
        public string? Contact { get; set; }

        [Required(ErrorMessage = "MailId is required")]
        public string? MailId { get; set; }
    }
}
