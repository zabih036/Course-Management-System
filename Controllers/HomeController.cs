using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JahanInstitute.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JahanInstitute.Controllers
{

    public class HomeController : Controller
    {

        JahanInstituteContext db = new JahanInstituteContext();

        public IActionResult Index()
        {

            return View();
        }
        [Authorize(Roles = ("Admin"))]
        public IActionResult dashboard()
        {
            return View();
        }
        [Authorize(Roles = ("Admin"))]
        public IActionResult MonthlyExpenses()
        {
            var monthStart = DateTime.Now;
            var monthEnd = DateTime.Now;
            Dictionary<string, int> dictWeeklySum = new Dictionary<string, int>();
            var currentMonthNo = DateTime.Now.Month;
            var currentYearNo = DateTime.Now.Year;
            for (int i = 1; i < 13; i++)
            {
                var last = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-currentMonthNo + i).Month);
                monthStart = DateTime.Now.AddMonths((-currentMonthNo) + i).AddDays(-(DateTime.Now.Day - 1));
                monthEnd = DateTime.Now.AddMonths((-currentMonthNo) + i).AddDays(-(DateTime.Now.Day)).AddDays(last);


                var expense = db.Expense.Where
              (cat => cat.Date >= monthStart && cat.Date <= monthEnd)
              .Select(cat => cat.Amount)
              .Sum();

                var salary = db.Salary.Where(c => c.Date >= monthStart && c.Date <= monthEnd).Select(r => r.PaidAmount).Sum();

                var amount = Convert.ToInt32(expense) + Convert.ToInt32(salary);


                if (i == 1)
                {
                    if (amount != 0)
                        dictWeeklySum.Add("January", amount);
                }
                else if (i == 2)
                {
                    if (amount != 0)
                        dictWeeklySum.Add("February", amount);
                }
                else if (i == 3)
                {
                    if (amount != 0)
                        dictWeeklySum.Add("March", amount);
                }
                else if (i == 4)
                {
                    if (amount != 0)
                        dictWeeklySum.Add("April", amount);
                }
                else if (i == 5)
                {
                    if (amount != 0)
                        dictWeeklySum.Add("May", amount);
                }
                else if (i == 6)
                {
                    if (amount != 0)
                        dictWeeklySum.Add("June", amount);
                }
                else if (i == 7)
                {
                    if (amount != 0)
                        dictWeeklySum.Add("July", amount);
                }
                else if (i == 8)
                {
                    if (amount != 0)
                        dictWeeklySum.Add("August", amount);
                }
                else if (i == 9)
                {
                    if (amount != 0)
                        dictWeeklySum.Add("September", amount);
                }
                else if (i == 10)
                {
                    if (amount != 0)
                        dictWeeklySum.Add("October", amount);
                }
                else if (i == 11)
                {
                    if (amount != 0)
                        dictWeeklySum.Add("November", amount);
                }
                else if (i == 12)
                {
                    if (amount != 0)
                        dictWeeklySum.Add("December", amount);
                }


            }
            return new JsonResult(dictWeeklySum);
        }
        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }
    }
}
