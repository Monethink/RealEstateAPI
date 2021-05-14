using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Models
{
    public class Feedback
    {
        public String Email { get; set; }
        public Guid? FeedbackId { get; set; }

        [StringLength(150)]
        public String Subject { get; set; }

        [StringLength(5000)]
        public String Body { get; set; }
    }
}
