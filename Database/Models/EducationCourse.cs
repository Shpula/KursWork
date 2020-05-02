using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    public class EducationCourse
    {
        public int Id { get; set; }
        public int EducationId { get; set; }
        public int CourseId { get; set; }
        public int Count { get; set; }
        public virtual Course Course { get; set; }
        public virtual Education Education { get; set; }
    }
}
