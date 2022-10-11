using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JahanInstitute.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JahanInstitute.Controllers
{
    [Authorize(Roles = ("Admin"))]
    public class ExpenseReportController : Controller
    {

        JahanInstituteContext db = new JahanInstituteContext();
        public IActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public IActionResult ManualReprot(ManulReprotViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = db.ExpenseType.ToList();
                Dictionary<string, int> dictMonthlySum = new Dictionary<string, int>();
                foreach (var item in category)
                {
                    var amount = db.Expense.Where
                   (cat => cat.Date >= model.FromDate.Date && cat.Date <= model.ToDate.Date && cat.ExpenseTypeId == item.ExpenseTypeId)
                   .Select(cat => cat.Amount)
                   .Sum();

                    dictMonthlySum.Add(item.ExpenseType1, amount);
                }

                var salary = db.Salary.Where(c => c.Date >= model.FromDate.Date && c.Date <= model.ToDate.Date).Select(r => r.PaidAmount).Sum();
                dictMonthlySum.Add("Salaries",Convert.ToInt32( salary));
                return new JsonResult(dictMonthlySum);
            }
            return View("Error");
        }
        public JsonResult GetWeeklyExpense()
        {
            var category = db.ExpenseType.ToList();
            Dictionary<string, int> dictWeeklySum = new Dictionary<string, int>();

            var last = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);

            var monthStart = DateTime.Now.Date.AddMonths(-1).AddDays(-(DateTime.Now.Date.Day - 1));
            var monthEnd = DateTime.Now.Date.AddMonths(-1).AddDays(-(DateTime.Now.Date.Day)).AddDays(last);
            foreach (var item in category)
            {
                var amount = db.Expense.Where
               (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.ExpenseTypeId == item.ExpenseTypeId)
               .Select(cat => cat.Amount)
               .Sum();

                dictWeeklySum.Add(item.ExpenseType1, amount);
            }

            var salary = db.Salary.Where(c => c.Date >= monthStart && c.Date <= monthEnd).Select(r => r.PaidAmount).Sum();
            dictWeeklySum.Add("Salaries", Convert.ToInt32(salary));
            return new JsonResult(dictWeeklySum);
        }
        public JsonResult GetYearExpense()
        {
            var monthStart = DateTime.Now;
            var monthEnd = DateTime.Now;
            var category = db.ExpenseType.ToList();
            Dictionary<string, int> yearDate = new Dictionary<string, int>();
            foreach (var item in category)
            {
                monthStart = DateTime.Now.Date.AddYears(-1).AddMonths(-(DateTime.Now.Date.Month - 1)).AddDays(-(DateTime.Now.Date.Day - 1));
                monthEnd = DateTime.Now.Date.AddYears(-1).AddMonths(-(DateTime.Now.Date.Month - 1)).AddMonths(11).AddDays(-(DateTime.Now.Date.Day)).AddDays(31);

                var amount = db.Expense.Where
               (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.ExpenseTypeId == item.ExpenseTypeId)
               .Select(cat => cat.Amount)
               .Sum();

                yearDate.Add(item.ExpenseType1, amount);
            }
            var salary = db.Salary.Where(c => c.Date >= monthStart && c.Date <= monthEnd).Select(r => r.PaidAmount).Sum();
            yearDate.Add("Salaries", Convert.ToInt32(salary));
            return new JsonResult(yearDate);
        }
        public JsonResult GetDailyExpense()
        {

            var category = db.ExpenseType.ToList();
            Dictionary<string, int> dictMonthlySum = new Dictionary<string, int>();
            foreach (var item in category)
            {
                var amount = db.Expense.Where
               (cat => cat.Date == DateTime.Now.Date && cat.ExpenseTypeId == item.ExpenseTypeId)
               .Select(cat => cat.Amount)
               .Sum();

                dictMonthlySum.Add(item.ExpenseType1, amount);
            }
            var salary = db.Salary.Where(c => c.Date == DateTime.Now.Date).Select(r => r.PaidAmount).Sum();
            dictMonthlySum.Add("Salaries", Convert.ToInt32(salary));
            return new JsonResult(dictMonthlySum);
        }
        public IActionResult Salaries()
        {
            return View();
        }
        public JsonResult LoadCurrentMonthsSalaries()
        {
            var last = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            var monthStart = DateTime.Now.Date.AddDays(-(DateTime.Now.Date.Day - 1));
            var monthEnd = DateTime.Now.Date.AddDays(-(DateTime.Now.Date.Day)).AddDays(last);
            var rec = (from s in db.Salary
                       join e in db.Employee on s.EmployeeId equals e.EmployeeId
                       where s.Date >= monthStart && s.Date <= monthEnd
                       select new
                       {
                           name = e.Name,
                           phone = e.Phone,
                           email = e.Email,
                           paidAmount = s.PaidAmount,
                           paidDate = Convert.ToDateTime(s.Date).Date.ToShortDateString(),
                           paidBy = db.Employee.Where(d => d.EmployeeId == s.PaidBy).Select(r => r.Name).FirstOrDefault(),
                       }).ToList();
            return new JsonResult(rec);
        }
        public JsonResult LoadLastMonthsSalaries()
        {
            var last = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);

            var monthStart = DateTime.Now.Date.AddMonths(-1).AddDays(-(DateTime.Now.Date.Day - 1));
            var monthEnd = DateTime.Now.Date.AddMonths(-1).AddDays(-(DateTime.Now.Date.Day)).AddDays(last);
            var rec = (from s in db.Salary
                       join e in db.Employee on s.EmployeeId equals e.EmployeeId
                       where s.Date >= monthStart && s.Date <= monthEnd
                       select new
                       {
                           name = e.Name,
                           phone = e.Phone,
                           email = e.Email,
                           paidAmount = s.PaidAmount,
                           paidDate = Convert.ToDateTime(s.Date).Date.ToShortDateString(),
                           paidBy = db.Employee.Where(d => d.EmployeeId == s.PaidBy).Select(r => r.Name).FirstOrDefault(),
                       }).ToList();
            return new JsonResult(rec);
        }
        public JsonResult LoadLastYearSalaries()
        {
            var monthStart = DateTime.Now.Date.AddYears(-1).AddMonths(-(DateTime.Now.Date.Month - 1)).AddDays(-(DateTime.Now.Date.Day - 1));
            var monthEnd = DateTime.Now.Date.AddYears(-1).AddMonths(-(DateTime.Now.Date.Month - 1)).AddMonths(11).AddDays(-(DateTime.Now.Date.Day)).AddDays(31);

            var rec = (from s in db.Salary
                       join e in db.Employee on s.EmployeeId equals e.EmployeeId
                       where s.Date >= monthStart && s.Date <= monthEnd
                       select new
                       {
                           name = e.Name,
                           phone = e.Phone,
                           email = e.Email,
                           paidAmount = s.PaidAmount,
                           paidDate = Convert.ToDateTime(s.Date).Date.ToShortDateString(),
                           paidBy = db.Employee.Where(d => d.EmployeeId == s.PaidBy).Select(r => r.Name).FirstOrDefault(),
                       }).ToList();
            return new JsonResult(rec);
        }
        public IActionResult ManualSalaryReprot(ManulReprotViewModel model)
        {
            if (ModelState.IsValid)
            {
                var monthStart = model.FromDate.Date;
                var monthEnd = model.ToDate.Date;

                var rec = (from s in db.Salary
                           join e in db.Employee on s.EmployeeId equals e.EmployeeId
                           where s.Date >= monthStart && s.Date <= monthEnd
                           select new
                           {
                               name = e.Name,
                               phone = e.Phone,
                               email = e.Email,
                               paidAmount = s.PaidAmount,
                               paidDate = Convert.ToDateTime(s.Date).Date.ToShortDateString(),
                               paidBy = db.Employee.Where(d => d.EmployeeId == s.PaidBy).Select(r => r.Name).FirstOrDefault(),
                           }).ToList();
                return new JsonResult(rec);
            }
            return View("Error");
        }
    }
}