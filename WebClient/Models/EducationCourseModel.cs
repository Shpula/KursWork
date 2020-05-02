using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class EducationCourseModel
    {
        public string CourseName { get; set; }
        public int Cost { get; set; }
        public int Duration { get; set; }
        public string Specialication { get; set; }
        public int Count { get; set; }
    }
}
