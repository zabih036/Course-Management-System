using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JahanInstitute.Models;
using JahanInstitute.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace JahanInstitute.Controllers
{
    public class TeacherAttendanceController : Controller
    {
        JahanInstituteContext db = new JahanInstituteContext();
        private readonly UserManager<ApplicationUser> _userManager;
        public TeacherAttendanceController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult teacherAttendance()
        {

            return View();
        }
        public JsonResult FetchTeacherForAttendance()
        {
     
            var SelectedItemRecored = (from e in db.Employee
                                       select new
                                       {
                                           studentId = e.EmployeeId,
                                           image = e.Photo,
                                           name = e.Name,
                                           fName = e.FatherName,
                                           assignedAttendance = db.TeacherAttendance.Where(z => z.Date == DateTime.Now.Date
                                                   && z.EmployeeId == e.EmployeeId   ).FirstOrDefault(),
                                           attendance = db.TeacherAttendance.Where(x => x.EmployeeId == e.EmployeeId && x.Date == DateTime.Now.Date).FirstOrDefault()

                                       }).OrderByDescending(r => r.studentId).ToList();
            return Json(SelectedItemRecored);
        }
        public async Task<JsonResult> AssignAttendance(int attStudentID, string attState, int saveOrEdit)
        {
            if (saveOrEdit == 1)
            {
                TeacherAttendance at = new TeacherAttendance();
                at.EmployeeId = attStudentID;
                //at.ClassId = attClassID;
                at.Status = attState;
                at.Date = DateTime.Now.Date;
                db.TeacherAttendance.Add(at);
                await db.SaveChangesAsync();
            }
            else
            {
                var record = db.TeacherAttendance.Where(x =>  x.EmployeeId == attStudentID && x.Date == DateTime.Now.Date).FirstOrDefault();
                record.EmployeeId = attStudentID;
                //record.ClassId = attClassID;
                record.Status = attState;
                record.Date = DateTime.Now.Date;
                db.Entry(record).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await db.SaveChangesAsync();
            }
            var data = new
            {
                state = attState,
                studentId = attStudentID
            };
            return Json(data);
        }
        public JsonResult attendanceAllTeacher(int year, int month)
        {
        

            var selectedClassAttendance = (from em in db.Employee.Where(x => x.EmployeeTypeId == 2)
                                      
                    select new
                    {
                        studentId = em.EmployeeId,
                        image = em.Photo,
                        name = em.Name,
                        fName = em.FatherName,
                        totalCredit = db.TeacherAttendance.Where(x => x.Date.Value.Year == year && x.Date.Value.Month == month && x.EmployeeId == em.EmployeeId).Count(),
                        totalAttended = db.TeacherAttendance.Where(x => x.Date.Value.Year == year && x.Date.Value.Month == month && x.Status == "Present" && x.EmployeeId == em.EmployeeId).Count(),
                        totalLeave = db.TeacherAttendance.Where(x => x.Date.Value.Year == year && x.Date.Value.Month == month && x.Status == "Leave" && x.EmployeeId == em.EmployeeId).Count(),
                        totalAbsent = db.TeacherAttendance.Where(x => x.Date.Value.Year == year && x.Date.Value.Month == month && x.Status == "Absent" && x.EmployeeId == em.EmployeeId).Count(),
                     
                      year = year,
                      month = month
                    }).ToList();
            return Json(selectedClassAttendance);
        }
        public JsonResult attendanceOfSelectedTeacher(int studentIdForAttendance, int yearForDetaile, int monthForDetaile)
        {
            //var attendance = db.AttendanceSheet.Where(x => x.ClassId == classIdForAttendance && x.StudentId == studentIdForAttendance);
            var attendance = (from at in db.TeacherAttendance.Where(x => x.EmployeeId == studentIdForAttendance && x.Date.Value.Year == yearForDetaile && x.Date.Value.Month == monthForDetaile)
                              select new
                              {
                                  date = at.Date.ToString(),
                                  status = at.Status
                              });
            return Json(attendance);
        }

        public IActionResult MyAttendance()
        {

            return View();
        }
        public JsonResult loadLoginedTeacherAttendance(int year, int month)
        {
            var email = _userManager.GetUserAsync(User).Result.Email;
            var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();
            var selectedClassAttendance = (from em in db.Employee.Where(x=> x.EmployeeId == empId)
                                           select new
                                           {
                                               studentId = em.EmployeeId,
                                               image = em.Photo,
                                               name = em.Name,
                                               fName = em.FatherName,
                                               totalCredit = db.TeacherAttendance.Where(x => x.Date.Value.Year == year && x.Date.Value.Month == month && x.EmployeeId == em.EmployeeId).Count(),
                                               totalAttended = db.TeacherAttendance.Where(x => x.Date.Value.Year == year && x.Date.Value.Month == month && x.Status == "Present" && x.EmployeeId == em.EmployeeId).Count(),
                                               totalLeave = db.TeacherAttendance.Where(x => x.Date.Value.Year == year && x.Date.Value.Month == month && x.Status == "Leave" && x.EmployeeId == em.EmployeeId).Count(),
                                               totalAbsent = db.TeacherAttendance.Where(x => x.Date.Value.Year == year && x.Date.Value.Month == month && x.Status == "Absent" && x.EmployeeId == em.EmployeeId).Count(),

                                               year = year,
                                               month = month
                                           }).ToList();
            return Json(selectedClassAttendance);
        }

    }
}
