using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.BindingModel;
using BusinessLogic.ViewModel;

namespace BusinessLogic.Interfaces
{
    public interface ICourseLogic
    {
        List<CourseViewModel> Read(CourseBindingModel model);
        void SaveToDatabase();
    }
}
