using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic.BindingModel;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModel;
using Database.Models;

namespace Database.Implements
{
    public class PaymentLogic : IPaymentLogic
    {
        public void CreateOrUpdate(PaymentBindingModel model)
        {
            using (var context = new Database())
            {
                Payment element = model.Id.HasValue ? null : new Payment ();             
                if (model.Id.HasValue)
                {
                    element = context.Payments.FirstOrDefault(rec => rec.Id ==
                   model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                }
                else
                {
                    element = new Payment();
                    context.Payments.Add(element);
                }
                element.EducationId = model.EducationId;
                element.ClientId = model.ClientId;
                element.Sum = model.Sum;
                element.DatePayment = model.DatePayment;
                context.SaveChanges();
            }
        }
        public void Delete(PaymentBindingModel model)
        {
            using (var context = new Database())
            {
                Payment element = context.Payments.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element != null)
                {
                    context.Payments.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }
        public List<PaymentViewModel> Read(PaymentBindingModel model)
        {
            using (var context = new Database())
            {
                return context.Payments
                .Where(rec => model == null || rec.Id == model.Id || rec.EducationId.Equals(model.EducationId))
                .Select(rec => new PaymentViewModel
                {
                    Id = rec.Id,
                    ClientId = rec.ClientId,
                    DatePayment = rec.DatePayment,
                    EducationId = rec.EducationId,
                    Sum = rec.Sum
                })
                .ToList();
            }
        }
    }
}

