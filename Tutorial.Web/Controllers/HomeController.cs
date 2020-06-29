using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial.Web.Model;
using Tutorial.Web.Services;
using Tutorial.Web.ViewModel;

namespace Tutorial.Web.Controllers 
{
    public class HomeController : Controller
    {
        private readonly IRepository<Student> _repository;

        public HomeController(IRepository<Student> repository)
        {
            this._repository = repository;
        }
        public IActionResult Index()
        {
            var st = new Student
            {
                Id = 1,
                FirstName = "nick",
                LastName = "carter"
            };
            //return new ObjectResult(st);
            var list = _repository.GetAll();
            var vms = list.Select(x => new StudentViewModel
            {
                Id = x.Id,
                Name = $"{x.FirstName} {x.LastName}",
                Age = DateTime.Now.Subtract(x.BirthDate).Days / 365
            }).ToList();
            var vm = new HomeIndexViewModel
            {
                Students = vms
            };
            return View(vm);
            //return Content("Hello from HomeController");
        }

        public IActionResult Detail(int id) {
            var student = _repository.GeById(id);
            if (student == null)
            {
                return RedirectToAction("index");
            }
            return View(student);
        }
    }
}
