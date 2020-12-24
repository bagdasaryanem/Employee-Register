using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstProj.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Employee Employee { get; set; }
        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Employees.ToList());
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null)
            {
                //create
                return View(new Employee());
            }
            //update
            Employee = _db.Employees.FirstOrDefault(u => u.Id == id);
            if (Employee == null)
            {
                return NotFound();
            }
            return View(Employee);
        }

        [HttpPost]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Employee.Id == 0)
                {
                    //create
                    _db.Employees.Add(Employee);
                }
                else
                {
                    _db.Employees.Update(Employee);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }

        public IActionResult Delete(int? id)
        {
            var employee = _db.Employees.Find(id);
            _db.Employees.Remove(employee);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
