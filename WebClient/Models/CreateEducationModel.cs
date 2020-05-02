using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.ViewModel;

namespace WebClient.Models
{
    public class CreateEducationModel
    {
        public Dictionary<int, int> EducationCourses { get; set; }
    }
}
