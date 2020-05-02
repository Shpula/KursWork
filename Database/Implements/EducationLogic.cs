using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BusinessLogic.BindingModel;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModel;
using Database.Models;

namespace Database.Implements
{
    public class EducationLogic : IEducationLogic
    {
        public void CreateOrUpdate(EducationBindingModel model)
        {
            using (var context = new Database())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Education element = context.Educations.FirstOrDefault(rec =>
                       rec.Id == model.Id);
                        
                        if (model.Id.HasValue)
                        {

                            if (element == null)
                            {
                                throw new Exception("Элемент не найден");
                            }       
                        }
                        else
                        {
                            element = new Education();
                            context.Educations.Add(element);
                        }
                        element.ClientId = model.ClientId;
                        element.DateOfBuying = model.DateOfBuying;
                        element.Duration = model.Duration;
                        element.FinalCost = model.FinalCost;
                        element.Status = model.Status;
                        context.SaveChanges();
                      
                            var groupCourses = model.EducationCourses
                               .GroupBy(rec => rec.CourseId)
                               .Select(rec => new
                               {
                                   CourseId = rec.Key,
                                   Count = rec.Sum(r => r.Count)
                               });

                            foreach (var groupCourse in groupCourses)
                            {
                                context.EducationCourses.Add(new EducationCourse
                                {
                                    EducationId = element.Id,
                                    CourseId = groupCourse.CourseId,
                                    Count = groupCourse.Count
                                });
                                context.SaveChanges();
                            }
                      
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Delete(EducationBindingModel model)
        {
            using (var context = new Database())
            {
                Education element = context.Educations.FirstOrDefault(rec => rec.Id == model.Id.Value);

                if (element != null)
                {
                    context.Educations.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        public List<EducationViewModel> Read(EducationBindingModel model)
        {
           
            using (var context = new Database())
            {
               
                return context.Educations.Where(rec => rec.Id == model.Id|| (rec.ClientId == model.ClientId) && (model.DateFrom==null && model.DateTo==null || rec.DateOfBuying >= model.DateFrom && rec.DateOfBuying <= model.DateTo))
               .Select(rec => new EducationViewModel
                {
                         Id = rec.Id,
                        ClientId = rec.ClientId,
                          Duration = rec.Duration,
                         FinalCost = rec.FinalCost,
                         DateOfBuying = rec.DateOfBuying,
                         LeftSum =rec.FinalCost - context.Payments.Where(recP => recP.EducationId == rec.Id).Select(recP => recP.Sum).Sum(),
                         Status = rec.Status,
                   EducationCourses = GetVisitCourseViewModel(rec)
               })
            .ToList();
            }
        }
        public static List<EducationCourseViewModel> GetVisitCourseViewModel(Education visit)
        {
            using (var context = new Database())
            {
                var VisitCourses = context.EducationCourses
                    .Where(rec => rec.EducationId == visit.Id)
                    .Include(rec => rec.Course)
                    .Select(rec => new EducationCourseViewModel
                    {
                        Id = rec.Id,
                        EducationId = rec.EducationId,
                        CourseId = rec.CourseId,
                        Count = rec.Count
                    }).ToList();
                foreach (var course in VisitCourses)
                {
                    var courseData = context.Courses.Where(rec => rec.Id == course.CourseId).FirstOrDefault();
                    course.CourseName = courseData.CourseName;
                    course.Specialication = courseData.Specialication;
                    course.Duration = courseData.Duration;
                    course.Cost = courseData.Cost;
                }
                return VisitCourses;
            }
        }
    }
}
