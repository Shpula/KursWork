using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string CourseName { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int Cost { get; set; }
        [Required]
        public string Specialication { get; set; }
    }
}
