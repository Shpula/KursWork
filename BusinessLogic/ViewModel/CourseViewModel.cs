using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BusinessLogic.ViewModel
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название лекции")]
        public string CourseName { get; set; }
        [DisplayName("Длительность")]
        public int Duration { get; set; }
        [DisplayName("Цена")]
        public int Cost { get; set; }
        [DisplayName("специализация")]
        public string Specialication { get; set; }
    }
}
