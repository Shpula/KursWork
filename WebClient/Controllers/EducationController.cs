using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.BindingModel;
using BusinessLogic.BusinessLogic;
using BusinessLogic.Enums;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModel;
using Database.Models;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class EducationController : Controller
    {
        private readonly IEducationLogic _educationLogic;
        private readonly ICourseLogic _courseLogic;
        private readonly IPaymentLogic _paymentLogic;
        private readonly ReportLogic _reportLogic;
        public EducationController(IEducationLogic educationLogic, ICourseLogic doctorLogic, IPaymentLogic paymentLogic, ReportLogic reportLogic)
        {
            _educationLogic = educationLogic;
            _courseLogic = doctorLogic;
            _paymentLogic = paymentLogic;
            _reportLogic = reportLogic;
        }

        public IActionResult Education()
        {
            ViewBag.Educations = _educationLogic.Read(new EducationBindingModel
            {
                ClientId = Program.Client.Id
            });
            return View();
        }

        [HttpPost]
        public IActionResult Education(ReportModel model)
        {
            var paymentList =new List<PaymentViewModel>();
            var educations = new List<EducationViewModel>();
            educations = _educationLogic.Read(new EducationBindingModel
            {
                ClientId = Program.Client.Id,
                DateFrom = model.From,
                DateTo = model.To
            });
            var payments = _paymentLogic.Read(null);
            foreach(var education in educations)
            {
                foreach(var payment in payments)
                {
                    if (payment.ClientId == Program.Client.Id && payment.EducationId == education.Id)
                        paymentList.Add(payment);
                }
            }
            ViewBag.Payments = paymentList;
            ViewBag.Educations = educations;
            string fileName = "C:\\data\\pdfreport.pdf";
            if (model.SendMail)
            {
                _reportLogic.SaveEducationPaymentsToPdfFile(fileName, new EducationBindingModel
                {
                    ClientId = Program.Client.Id,
                    DateFrom = model.From,
                    DateTo = model.To
                }, Program.Client.Email);
            }
            return View();
        }

        public IActionResult CreateEducation()
        {
            ViewBag.EducationCourses = _courseLogic.Read(new CourseBindingModel
            {
                Specialication = Program.Client.Specialication
            });
            return View();
        }

        [HttpPost]
        public ActionResult CreateEducation(CreateEducationModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EducationCourses = _courseLogic.Read(null);
                return View(model);
            }

            var educationСourses = new List<EducationСourseBindingModel>();

            foreach (var course in model.EducationCourses)
            {
                if (course.Value > 0)
                {
                    educationСourses.Add(new EducationСourseBindingModel
                    {
                        CourseId = course.Key,
                        Count = course.Value
                    });
                }
            }
            if (educationСourses.Count == 0)
            {
                ViewBag.Products = _courseLogic.Read(null);
                ModelState.AddModelError("", "Ни одна лекция не выбрана");
                return View(model);
            }
            _educationLogic.CreateOrUpdate(new EducationBindingModel
            {
                ClientId = Program.Client.Id,
                DateOfBuying = DateTime.Now,
                Status = EducationStatus.Имеется,
                FinalCost = CalculateSum(educationСourses),
                Duration = CalculateDuration(educationСourses),
                EducationCourses = educationСourses
            });
            return RedirectToAction("Education");
        }
        private int CalculateSum(List<EducationСourseBindingModel> educationСourses)
        {
            int sum = 0;

            foreach (var course in educationСourses)
            {
                var courseData = _courseLogic.Read(new CourseBindingModel { Id = course.CourseId }).FirstOrDefault();

                if (courseData != null)
                {
                    for (int i = 0; i < course.Count; i++)
                        sum += courseData.Cost;
                }
            }
            return sum;
        }
        private int CalculateDuration(List<EducationСourseBindingModel> visitDoctors)
        {
            int duration = 0;
            foreach (var course in visitDoctors)
            {
                var courseData = _courseLogic.Read(new CourseBindingModel { Id = course.CourseId }).FirstOrDefault();
                if (courseData != null)
                {
                    for (int i = 0; i < course.Count; i++)
                        duration += courseData.Duration;
                }
            }
            return duration;
        }
        public IActionResult Payment(int id)
        {
            var visit = _educationLogic.Read(new EducationBindingModel
            {
                Id = id
            }).FirstOrDefault();
            ViewBag.Education = visit;
            ViewBag.LeftSum = CalculateLeftSum(visit);
            return View();
        }
        [HttpPost]
        public ActionResult Payment(PaymentModel model)
        {
            EducationViewModel visit = _educationLogic.Read(new EducationBindingModel
            {
                Id = model.EducationId
            }).FirstOrDefault();
            int leftSum = CalculateLeftSum(visit);
            if (!ModelState.IsValid)
            {
                ViewBag.Education = visit;
                ViewBag.LeftSum = leftSum;
                return View(model);
            }
            if (leftSum < model.Sum)
            {
                ViewBag.Education = visit;
                ViewBag.LeftSum = leftSum;
                return View(model);
            }
            _paymentLogic.CreateOrUpdate(new PaymentBindingModel
            {
                EducationId = visit.Id,
                ClientId = Program.Client.Id,
                DatePayment = DateTime.Now,
                Sum = model.Sum
            });
            leftSum -= model.Sum;
            _educationLogic.CreateOrUpdate(new EducationBindingModel
            {
                Id = visit.Id,
                ClientId = visit.ClientId,
                DateOfBuying = visit.DateOfBuying,
                Duration = visit.Duration,
                Status = leftSum > 0 ? EducationStatus.Получено_не_полностью : EducationStatus.Получено,
                FinalCost = visit.FinalCost,
                EducationCourses = visit.EducationCourses.Select(rec => new EducationСourseBindingModel
                {
                    Id = rec.Id,
                    EducationId = rec.EducationId,
                    CourseId = rec.CourseId,
                    Count = rec.Count
                }).ToList()
            });
            return RedirectToAction("Education");
        }

        private int CalculateLeftSum(EducationViewModel visit)
        {
            int sum = visit.FinalCost;
            int paidSum = _paymentLogic.Read(new PaymentBindingModel
            {
                EducationId = visit.Id
            }).Select(rec => rec.Sum).Sum();

            return sum - paidSum;
        }
        public IActionResult SendWordReport(int id)
        {
            var education = _educationLogic.Read(new EducationBindingModel { Id = id }).FirstOrDefault();
            string fileName = "C:\\data\\" + education.Id + ".docx";
            _reportLogic.SaveEducationCourseToWordFile(fileName, education, Program.Client.Email);
            return RedirectToAction("Education");
        }
        public IActionResult SendExcelReport(int id)
        {
            var education = _educationLogic.Read(new EducationBindingModel { Id = id }).FirstOrDefault();
            string fileName = "C:\\data\\" + education.Id + ".xlsx";
            _reportLogic.SaveEducationCoursesToExcelFile(fileName, education, Program.Client.Email);
            return RedirectToAction("Education");
        }
    }
}