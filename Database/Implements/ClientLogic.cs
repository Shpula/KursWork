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
    public class ClientLogic : IClientLogic
    {
        public void CreateOrUpdate(ClientBindingModel model)
        {
            using (var context = new Database())
            {
                Client element = model.Id.HasValue ? null : new Client();                
                if (model.Id.HasValue)
                {
                    element = context.Clients.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                }
                else
                {
                    element = new Client();
                    context.Clients.Add(element);
                }
                element.Email = model.Email;
                element.Login = model.Login;
                element.Specialication = model.Specialication;
                element.Block = model.Block;
                element.Password = model.Password;
                context.SaveChanges();
            }
        }
        public void Delete(ClientBindingModel model)
        {
            using (var context = new Database())
            {
                Client element = context.Clients.FirstOrDefault(rec => rec.Id == model.Id);

                if (element != null)
                {
                    context.Clients.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }
        public List<ClientViewModel> Read(ClientBindingModel model)
        {
            using (var context = new Database())
            {
                return context.Clients
                 .Where(rec => model == null
                   || rec.Id == model.Id
                   || rec.Login == model.Login && rec.Password == model.Password)
               .Select(rec => new ClientViewModel
                {
                    Id = rec.Id,
                    Login=rec.Login,
                   Specialication = rec.Specialication,
                    Email = rec.Email,
                    Password = rec.Password,
                    Block=rec.Block
                })
                .ToList();
            }
        }
        public List<ClientViewModel> ReadofRegistration(ClientBindingModel model)
        {
            using (var context = new Database())
            {
                return context.Clients
                 .Where(rec => model == null
                   || rec.Id == model.Id
                   || rec.Login == model.Login || rec.Email == model.Email || rec.Specialication == model.Specialication)
               .Select(rec => new ClientViewModel
               {
                   Login = rec.Login,
                   Email = rec.Email,
                   Specialication = rec.Specialication,
               })
                .ToList();
            }
        }
    }
}