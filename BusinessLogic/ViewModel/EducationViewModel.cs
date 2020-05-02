using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using BusinessLogic.Enums;

namespace BusinessLogic.ViewModel
{
    [DataContract]
    public class EducationViewModel
    {
        [DataMember]
        public int Id { get; set; }
        public int ClientId { get; set; }
        [DataMember]
        [DisplayName("Клиент")]
        public string Login { get; set; }
        [DataMember]
        [DisplayName("Цена")]
        public int FinalCost { get; set; }
        [DataMember]
        [DisplayName("Длительность")]
        public int Duration { get; set; }
        [DataMember]
        [DisplayName("Дата создания")]
        public DateTime DateOfBuying { get; set; }
        [DataMember]
        [DisplayName("Статус")]
        public EducationStatus Status { get; set; }
        [DataMember]
        [DisplayName("Получено")]
        public int LeftSum { get; set; }
        [DataMember]
        public List<EducationCourseViewModel> EducationCourses { get; set; }
    }
}
