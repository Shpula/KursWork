using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic.ViewModel;

namespace BusinessLogic.HelperModels
{
    public class ExcelInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<CourseViewModel> Courses { get; set; }
    }
}
