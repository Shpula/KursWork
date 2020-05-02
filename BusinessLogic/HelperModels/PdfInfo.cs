using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.ViewModel;

namespace BusinessLogic.HelperModels
{
    public class PdfInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }   
        public List<EducationViewModel> Educations { get; set; }
        public Dictionary<int, List<PaymentViewModel>> Payments { get; set; }
    }
}
