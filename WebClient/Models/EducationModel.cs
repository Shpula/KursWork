using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Enums;

namespace WebClient.Models
{
    public class EducationModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int Duration { get; set; }
        public DateTime DateOfBuying { get; set; }
        public int FinalCost { get; set; }
        public int LeftSum { get; set; }
        public EducationStatus Status { get; set; }
        public List<EducationCourseModel> EducationCourses { get; set; }
    }
}
