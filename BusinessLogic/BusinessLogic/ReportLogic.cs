using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using BusinessLogic.BindingModel;
using BusinessLogic.HelperModels;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModel;

namespace BusinessLogic.BusinessLogic
{
    public class ReportLogic
    {
        private readonly ICourseLogic doctorLogic;
        private readonly IEducationLogic visitLogic;
        private readonly IPaymentLogic paymentLogic;
        public ReportLogic(ICourseLogic doctorLogic, IEducationLogic visitLogic, IPaymentLogic paymentLogic)
        {
            this.doctorLogic = doctorLogic;
            this.visitLogic = visitLogic;
            this.paymentLogic = paymentLogic;
        }
        public List<CourseViewModel> GetEducationCourses(EducationViewModel visit)
        {
            var doctors = new List<CourseViewModel>();

            foreach (var doctor in visit.EducationCourses)
            {
                doctors.Add(doctorLogic.Read(new CourseBindingModel
                {
                    Id = doctor.CourseId
                }).FirstOrDefault());

            }
            return doctors;
        }
        public Dictionary<int, List<PaymentViewModel>> GetEducationPayments(EducationBindingModel model)
        {
            var visits = visitLogic.Read(model).ToList();
            Dictionary<int, List<PaymentViewModel>> payments = new Dictionary<int, List<PaymentViewModel>>();
            foreach (var visit in visits)
            {
                var visitPayments = paymentLogic.Read(new PaymentBindingModel { EducationId = visit.Id }).ToList();
                payments.Add(visit.Id, visitPayments);
            }
            return payments;
        }
        public void SaveEducationPaymentsToPdfFile(string fileName, EducationBindingModel visit, string email)
        {
         var   Visits = visitLogic.Read(visit).ToList();
            string title = "Список курсов в период с " + visit.DateFrom.ToString() + " по " + visit.DateTo.ToString();
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = fileName,
                Title = title,
                Educations = visitLogic.Read(visit).ToList(),
                Payments = GetEducationPayments(visit)
            });
            SendMail(email, fileName, title);
        }
        public void SaveEducationCourseToWordFile(string fileName, EducationViewModel visit, string email)
        {
            string title = "Список лекций по курсам №" + visit.Id;
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = fileName,
                Title = title,
                Courses = GetEducationCourses(visit)
            });
            SendMail(email, fileName, title);
        }
        public void SaveEducationCoursesToExcelFile(string fileName, EducationViewModel visit, string email)
        {
            string title = "Список лекций по курсам №" + visit.Id;
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = fileName,
                Title = title,
                Courses = GetEducationCourses(visit)
            });
            SendMail(email, fileName, title);
        }
        public void SendMail(string email, string fileName, string subject)
        {
            MailAddress from = new MailAddress("Butin.nik73@gmail.com", "Отчет!");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = subject;
            m.Attachments.Add(new Attachment(fileName));
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("Butin.nik73@gmail.com", "nikita737");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
}
