using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace BusinessLogic.ViewModel
{
    [DataContract]
    public class PaymentViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]   
        public int ClientId { get; set; }
        [DataMember]
        public int EducationId { get; set; }
        [DataMember]
        [DisplayName("Дата получения")]
        public DateTime DatePayment { get; set; }
        [DataMember]
        [DisplayName("Сумма получения")]
        public int Sum { get; set; }
    }
}
