using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial.Web.Model;
using Tutorial.Web.Services;

namespace Tutorial.Web.ViewComponents
{
    public class WelcomeViewComponent : ViewComponent
    {
        private readonly IRepository<Student> repository;

        public WelcomeViewComponent(IRepository<Student> repository)
        {
            this.repository = repository;
        }

        public IViewComponentResult Invoke()
        {
            var count = repository.GetAll().Count().ToString();
            return View("Default", count);
        }
    }
}
