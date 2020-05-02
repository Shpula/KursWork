using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BusinessLogic.BindingModel
{
    [DataContract]
    public class PaymentBindingModel
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int EducationId { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public DateTime DatePayment { get; set; }
        [DataMember]
        public int Sum { get; set; }
    }
}
