using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using JahanInstitute.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JahanInstitute.Controllers
{

    [Authorize(Roles = "Admin")]
    public class ExpenseController : Controller
    {

        JahanInstituteContext db = new JahanInstituteContext();
        public IActionResult Expense()
        {
            var c = (from d in db.ExpenseType
                     where d.ExpenseTypeId != 1
                     select new SelectListItem()
                     {
                         Text = d.ExpenseType1,
                         Value = d.ExpenseTypeId.ToString()
                     }).ToList();

            ViewBag.ExpenseType = c;
            return View();
        }
        public JsonResult LoadExpenseType()
        {
            var rec = db.ExpenseType.Where(d => d.ExpenseTypeId != 1).ToList().OrderByDescending(d => d.ExpenseTypeId);
            return Json(rec);
        }
        public JsonResult LoadExpense()
        {


            var rec = (from i in db.Expense
                       join c in db.ExpenseType on i.ExpenseTypeId equals c.ExpenseTypeId
                       where i.ExpenseTypeId != 1
                       select new
                       {
                           expenseId = i.ExpenseId,
                           expenseAmount = i.Amount,
                           expenseType = c.ExpenseType1,
                           expenseDate = Convert.ToDateTime(i.Date),
                           expenseDateShow = Convert.ToDateTime(i.Date).ToShortDateString(),
                           detail = i.Description,
                           expenseTypeId = i.ExpenseTypeId

                       }).ToList().OrderByDescending(d => d.expenseId);
            return Json(rec);
        }
        public IActionResult IsExpenseTypeExist(AllViewModel model)
        {
            if (model.expenseType.ExpenseTypeId != 0)
            {
                return Json(true);
            }
            var rec = db.ExpenseType.Any(d => d.ExpenseType1 == model.expenseType.ExpenseType);

            if (!rec)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        public async Task<IActionResult> SaveExpenseType(AllViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.expenseType.ExpenseTypeId != 0)
                {
                    var rec = await db.ExpenseType.FindAsync(model.expenseType.ExpenseTypeId);
                    if (rec == null)
                    {
                        return View("Error");
                    }
                    rec.ExpenseType1 = model.expenseType.ExpenseType;


                    db.Entry(rec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.Entry(rec).Property(d => d.ExpenseType1).IsModified = true;

                    await db.SaveChangesAsync();

                    return Json("Expense type updated.");
                }
                else
                {
                    ExpenseType c = new ExpenseType()
                    {
                        ExpenseType1 = model.expenseType.ExpenseType
                    };
                    db.ExpenseType.Add(c);
                    await db.SaveChangesAsync();
                    return Json("Expense type added.");
                }

            }
            return View("Error");
        }
        public async Task<IActionResult> SaveExpense(AllViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.expense.ExpenseId != 0)
                {
                    var rec = await db.Expense.FindAsync(model.expense.ExpenseId);
                    if (rec == null)
                    {
                        return View("Error");
                    }
                    rec.Amount = model.expense.Expense;
                    rec.ExpenseTypeId = model.expense.ExpenseTypeId;
                    rec.Date = model.expense.ExpenseDate;
                    rec.Description = model.expense.Detail;


                    db.Entry(rec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.Entry(rec).Property(d => d.Amount).IsModified = true;
                    db.Entry(rec).Property(d => d.ExpenseTypeId).IsModified = true;
                    db.Entry(rec).Property(d => d.Date).IsModified = true;
                    db.Entry(rec).Property(d => d.Description).IsModified = true;

                    await db.SaveChangesAsync();

                    return Json("Record updated.");
                }
                else
                {
                    Expense c = new Expense()
                    {
                        Amount = model.expense.Expense,
                        ExpenseTypeId = model.expense.ExpenseTypeId,
                        Date = model.expense.ExpenseDate,
                        Description = model.expense.Detail
                    };
                    db.Expense.Add(c);
                    await db.SaveChangesAsync();
                    return Json(" Expense record added.");
                }

            }
            return View("Error");
        }
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteExpenseType(AllViewModel model)
        {
            if (model.expenseType.ExpenseTypeId != 0)
            {
                var comp = await db.ExpenseType.FindAsync(model.expenseType.ExpenseTypeId);
                if (comp == null)
                {
                    return View("Error");
                }

                try
                {
                    db.ExpenseType.Remove(comp);
                    await db.SaveChangesAsync();
                    return Json("Record deleted");
                }
                catch (Exception)
                {

                    return Json("You can't delete this record.");
                }
            }
            return NotFound();

        }
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteExpense(int cId)
        {
            if (cId != 0)
            {
                var comp = await db.Expense.FindAsync(cId);
                if (comp == null)
                {
                    return View("Error");
                }
                db.Expense.Remove(comp);
                await db.SaveChangesAsync();
                return Json("deleted");
            }
            return NotFound();

        }
    }
}