using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BusinessLogic.Enums;

namespace Database.Models
{
    public class Education
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        [Required]
        public int FinalCost { get; set; }
        [Required]
        public int Duration { get; set; }
        public DateTime DateOfBuying { get; set; }
        public int PaidSum { get; set; }
        public EducationStatus Status { get; set; }
        public virtual List<EducationCourse> VisitCourses { get; set; }   
    }
}
