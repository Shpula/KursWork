using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace BusinessLogic.ViewModel
{
    [DataContract]
    public class EducationCourseViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int EducationId { get; set; }
        [DataMember]
        public int CourseId { get; set; }
        [DataMember]
        public string CourseName { get; set; }
        [DataMember]
        public int Cost { get; set; }
        [DataMember]
        public int Duration { get; set; }
        [DataMember]
        public string Specialication { get; set; }
        [DataMember]
        [DisplayName("Количество")]
        public int Count { get; set; }
    }
}
