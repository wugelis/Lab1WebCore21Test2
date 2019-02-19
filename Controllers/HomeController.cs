using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lab1WebCore21Test2.Models;
using Microsoft.AspNetCore.Http;
using Lab1WebCore21Test2.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Lab1WebCore21Test2.Data;

namespace Lab1WebCore21Test2.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public HomeController(
            IHostingEnvironment hostingEnvironment,
            ApplicationDbContext context
            )
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(IFormCollection forms)
        {
            ViewData.Model = forms["txtName"];
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ToDoList()
        {
            var result = _context
                .ToDoLists
                .Select(c => new ToDoListViewModel(){
                    Id = c.Id,
                    Name = c.Name,
                    ToDo = c.ToDo,
                    StartDate = c.StartDate.HasValue?c.StartDate.Value.ToString("yyyy/MM/dd"):"",
                    EndDate = c.EndDate.HasValue?c.EndDate.Value.ToString("yyyy/MM/dd"):""
                });
            
            //var result = new ToDoListViewModel[] { };
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ToDoListViewModel model)
        {
            if (ModelState.IsValid)
            {
                var todoList = model;

                _context.ToDoLists.Add(new Models.ToDoList() {
                    Name = model.Name,
                    ToDo = model.ToDo,
                    StartDate = string.IsNullOrEmpty(model.StartDate)?null:(DateTime?) Convert.ToDateTime(model.StartDate),
                    EndDate = string.IsNullOrEmpty(model.EndDate)?null:(DateTime?) Convert.ToDateTime(model.EndDate)
                });

                _context.SaveChanges();
            }

            return RedirectToAction("ToDoList");
        }

        public IActionResult Edit(int Id)
        {
            ToDoList model = GetOneToDo(Id);
            ToDoListViewModel result = null;

            if (model != null)
            {
                result = GetToDoViewModelFromEntitiy(model);
            }
            return View(result);
        }

        private ToDoList GetOneToDo(int Id)
        {
            return _context.ToDoLists
                            .Where(c => c.Id == Id)
                            .Select(c => c)
                            .FirstOrDefault();
        }

        [HttpPost]
        public IActionResult Edit(ToDoListViewModel model)
        {
            ToDoList entity = GetOneToDo(model.Id);

            if (entity != null)
            {
                entity.Name = model.Name;
                entity.ToDo = model.ToDo;
                entity.StartDate = !string.IsNullOrEmpty(model.StartDate) ? (DateTime?) Convert.ToDateTime(model.StartDate) : null;
                entity.EndDate = !string.IsNullOrEmpty(model.EndDate) ? (DateTime?)Convert.ToDateTime(model.EndDate) : null;

                _context.SaveChanges();
            };

            return RedirectToAction("ToDoList");
        }

        public IActionResult Details(int Id)
        {
            ToDoList model = GetOneToDo(Id);
            ToDoListViewModel result = GetToDoViewModelFromEntitiy(model);

            return View(result);
        }

        private static ToDoListViewModel GetToDoViewModelFromEntitiy(ToDoList model)
        {
            return new ToDoListViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                ToDo = model.ToDo,
                StartDate = model.StartDate.HasValue ? model.StartDate.Value.ToString("yyyy/MM/dd") : "",
                EndDate = model.StartDate.HasValue ? model.EndDate.Value.ToString("yyyy/MM/dd") : ""
            };
        }

        [HttpPost]
        public IActionResult Details(ToDoListViewModel model)
        {
            return View();
        }
        public IActionResult GetPdfFile()
        {
            byte[] file = System.IO.File.ReadAllBytes(
                Path.Combine(
                    _hostingEnvironment.ContentRootPath,
                    "Docker and Microsoft-TechnicalDeepdive-slidshare.pdf"));

            return File(
                file, 
                "application/pdf",
                "Docker and Microsoft-TechnicalDeepdive-slidshare.pdf");
        }

        public IActionResult Delete(int Id)
        {
            var result = _context.ToDoLists.Remove(
                _context.ToDoLists
                    .Where(c => c.Id == Id)
                    .FirstOrDefault()
                );

            //result.State == Microsoft.EntityFrameworkCore.EntityState.Deleted;
            int deletedCount = _context.SaveChanges();

            return RedirectToAction("ToDoList");
        }
    }
}
