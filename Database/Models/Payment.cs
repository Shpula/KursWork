using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        [Required]
        public int EducationId { get; set; }
        [Required]
        public DateTime DatePayment { get; set; }
        [Required]
        public int Sum { get; set; }
        public virtual Client Client { get; set; }
    }
}
