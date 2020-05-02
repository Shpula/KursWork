using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using BusinessLogic.Enums;

namespace BusinessLogic.BindingModel
{
    [DataContract]
    public class EducationBindingModel
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public int Duration { get; set; }
        [DataMember]
        public DateTime DateOfBuying { get; set; }
        [DataMember]
        public int FinalCost { get; set; }
        [DataMember]
        public int LeftSum { get; set; }
        [DataMember]
        public EducationStatus Status { get; set; }
        [DataMember]
        public DateTime? DateFrom { get; set; }
        [DataMember]
        public DateTime? DateTo { get; set; }
        [DataMember]
        public List<EducationСourseBindingModel> EducationCourses { get; set; }
    }
}
