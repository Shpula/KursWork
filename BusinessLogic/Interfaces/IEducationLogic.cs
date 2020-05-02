using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.BindingModel;
using BusinessLogic.ViewModel;

namespace BusinessLogic.Interfaces
{
    public interface IEducationLogic
    {
        List<EducationViewModel> Read(EducationBindingModel model);
        void CreateOrUpdate(EducationBindingModel model);
        void Delete(EducationBindingModel model);
    }
}
