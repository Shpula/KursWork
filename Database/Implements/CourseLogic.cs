using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
    public class CourseLogic : ICourseLogic
    {
        private readonly string FileName = @"C:\Spek.txt";

        public List<Course> Courses { get; set; }
        public CourseLogic()
        {
            Courses = LoadCourses();
        }
        private List<Course> LoadCourses()
        {
            var list = new List<Course>();
            File.Exists(FileName);
            if (File.Exists(FileName))
            {
                XDocument xDocument = XDocument.Load(FileName);
                var xElements = xDocument.Root.Elements("Course").ToList();
                foreach (var elem in xElements)
                {
                    list.Add(new Course
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        CourseName = elem.Element("CourseName").Value,
                        Duration = Convert.ToInt32(elem.Element("Duration").Value),
                        Cost= Convert.ToInt32(elem.Element("Cost").Value),
                        Specialication = elem.Element("Specialication").Value,
                    });
                }
            }
            return list;
        }
        public void SaveToDatabase()
        {
            var courses = LoadCourses();
            using (var context = new Database())
            {
                foreach (var course in courses)
                {
                    Course element = context.Courses.FirstOrDefault(rec => rec.Id == course.Id);
                    if (element != null)
                    {
                        break;
                    }
                    else
                    {
                        element = new Course();
                        context.Courses.Add(element);
                    }
                    element.CourseName = course.CourseName;
                    element.Duration = course.Duration;
                    element.Specialication = course.Specialication;
                    element.Cost = course.Cost;
                    context.SaveChanges();
                }
            }
        }
        public List<CourseViewModel> Read(CourseBindingModel model)
        {
            SaveToDatabase();
            return Courses
            .Where(rec => model == null || rec.Id == model.Id|| rec.Specialication == model.Specialication)
            .Select(rec => new CourseViewModel
            {
                Id = rec.Id,
                CourseName = rec.CourseName,
                Duration=rec.Duration,
                Cost=rec.Cost,
                Specialication = rec.Specialication
            })
            .ToList();
        }
    }
}
