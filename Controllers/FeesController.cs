using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JahanInstitute.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JahanInstitute.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FeesController : Controller
    {
        JahanInstituteContext db = new JahanInstituteContext();
        public IActionResult Fees()
        {
            return View();
        }
        public JsonResult LoadAllStudentsFees()
        {
            var rec = (from cs in db.StudentClass
                       
                       join s in db.Student on cs.StudentId equals s.StudentId
                       join c in db.Class on cs.ClassId equals c.ClassId
                       let f = db.Fees.FirstOrDefault(x=>x.StudentId ==s.StudentId && x.ClassId == c.ClassId)
                       let ab = db.AttendanceSheet.Count(x=>x.StudentId ==s.StudentId && x.ClassId == c.ClassId && x.Status =="Absent")
                       
                       where f.Paid < c.TotalFee || cs.Status == "false" 
                       select new 
                       {
                           feesId = f.FeesId,
                           absent = ab,
                           emailConfirmed  = cs.Status,
                           stdclassid = cs.StdClassId,
                           classId = c.ClassId,
                           studentId = s.StudentId,
                           photo = s.Photo,
                           name = s.Name,
                           fName = s.FatherName,
                           phone = s.Phone,
                           classes = c.ClassName.ClassName1,
                           classFees = c.TotalFee,
                           paidAmount = f.Paid,
                           date = Convert.ToDateTime(f.Date).ToShortDateString(),
                           remain = c.TotalFee - f.Paid,
                           desctription = f.Description ?? "",
                       }).ToList();
            return Json(rec);
        }
        public async Task<IActionResult> AddStudentFees(AllViewModel model)
        {
            if (ModelState.IsValid)
            {
                var rec = db.Fees.Find(model.fees.FeesId);
                rec.Paid = model.fees.PaidAmount+model.fees.OldPaid;
                rec.Date = DateTime.Now.Date;
                db.Entry(rec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.Entry(rec).Property(c => c.Paid).IsModified = true;
                db.Entry(rec).Property(c => c.Date).IsModified = true;
                await db.SaveChangesAsync();
                return Json("Payment successfully done");

            }
            else
            {
                return Json("Error");
            }

        }
        public async Task<IActionResult> UnLockStudent(AllViewModel model)
        {
            var id = Convert.ToInt32(model.register.email);
            var user = await db.StudentClass.FindAsync(id);
            if (model.register.trueOrfalse == "lock")
            {
                user.Status = "false";
                db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.Entry(user).Property(x => x.Status).IsModified = true;
                await db.SaveChangesAsync();
                return Json("true");
            }
            else
            {
                user.Status = "true";
                db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.Entry(user).Property(x => x.Status).IsModified = true;
                await db.SaveChangesAsync();
                return Json("true");
            }


        }

        public async Task<IActionResult> UnblockAttendance(int classId, int studentId)
        {
            var rec = db.AttendanceSheet.Where(x => x.ClassId == classId && x.StudentId == studentId && x.Status=="Absent").FirstOrDefault();
            rec.Status = "Present";
            db.Entry(rec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.Entry(rec).Property(x => x.Status).IsModified = true;
            await db.SaveChangesAsync();
            return Json("done");
        }
    }
}
