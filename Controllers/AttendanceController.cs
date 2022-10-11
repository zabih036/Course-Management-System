using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JahanInstitute.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace JahanInstitute.Controllers
{
    public class AttendanceController : Controller
    {
        JahanInstituteContext db = new JahanInstituteContext();
        private readonly UserManager<ApplicationUser> _userManager;
        public AttendanceController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [Authorize(Roles = "Teacher")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult studentAttendance()
        {
            var email = _userManager.GetUserAsync(User).Result.Email;
            var empId = db.Employee.Where(x => x.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

            var classes = (from w in db.ClassTeacher.Where(x=>x.EmployeeId == empId)
                           join c in db.Class on w.ClassId equals c.ClassId
                           select new SelectListItem()
                           {
                               Text = db.ClassName.Where(r => r.ClassNameId == c.ClassNameId).Select(d => d.ClassName1).FirstOrDefault() + " , Time : " + c.StartHour + " - " + c.EndHour + " , Shift : " + c.Shift,
                               Value = c.ClassId.ToString()
                           }).ToList().GroupBy(x => x.Value).Select(r => r.FirstOrDefault());
            ViewBag.Classes = classes;
            return View();
        }
        public JsonResult FetchStudentsForAttendance(int stdClassId)
        {
            var SelectedItemRecored = (from s in db.Student
                                       join c in db.StudentClass on s.StudentId equals c.StudentId
                                       where c.ClassId == stdClassId && c.Status !="false"
                                       select new
                                       {
                                           studentId = s.StudentId,
                                           image = s.Photo,
                                           name = s.Name,
                                           fName = s.FatherName,
                                           classId = c.ClassId,
                                           assignedAttendance = db.AttendanceSheet.Where(e => e.ClassId == stdClassId &&
                                                       e.StudentId == s.StudentId && e.Date == DateTime.Now.Date
                                                      ).FirstOrDefault(),
                                           attendance = db.AttendanceSheet.Where(x => x.StudentId == s.StudentId && x.ClassId == c.ClassId && x.Date == DateTime.Now.Date).FirstOrDefault()

                                       }).OrderByDescending(r => r.studentId).ToList();
            return Json(SelectedItemRecored);
        }
        public JsonResult FetchStudents()
        {
            var SelectedItemRecored = (from s in db.Student
                                       join c in db.StudentClass on s.StudentId equals c.StudentId
                                       select new
                                       {
                                           studentId = s.StudentId,
                                           image = s.Photo,
                                           name = s.Name,
                                           fName = s.FatherName,
                                           gender = s.Gender,
                                           address = s.Address,
                                           phone = s.Phone,
                                           email = s.Email,
                                           regDate = s.RegDate,
                                           idCard = s.IdCardNo,
                                           school = s.School,
                                       }).OrderByDescending(r => r.studentId).ToList();
            return Json(SelectedItemRecored);
        }
        public async Task<JsonResult> AssingAttendance(int attStudentID, int attClassID, string attState, int saveOrEdit)
        {
            if(saveOrEdit == 1)
            {
                AttendanceSheet at = new AttendanceSheet();
                at.StudentId = attStudentID;
                at.ClassId = attClassID;
                at.Status = attState;
                at.Date = DateTime.Now.Date;
                db.AttendanceSheet.Add(at);
                await db.SaveChangesAsync();
            }
            else
            {
                var record = db.AttendanceSheet.Where(x => x.ClassId == attClassID && x.StudentId == attStudentID && x.Date == DateTime.Now.Date).FirstOrDefault();
                record.StudentId = attStudentID;
                record.ClassId = attClassID;
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
        public JsonResult attendanceOfAllStudent(int stdClassId)
        {

            var selectedClassAttendance = (from sc in db.StudentClass.Where(x=> x.ClassId == stdClassId)
                                           join st in db.Student  on sc.StudentId equals st.StudentId
                                           join cl in db.Class on sc.ClassId equals cl.ClassId
                                           where sc.Status !="false"
                                       select new
                                       {
                                           studentId = st.StudentId,
                                           image = st.Photo,
                                           name = st.Name,
                                           fName = st.FatherName,
                                           totalCredit = db.AttendanceSheet.Where(x => x.ClassId == stdClassId && x.StudentId == st.StudentId).Count(),
                                           totalAttended = db.AttendanceSheet.Where(x => x.ClassId == stdClassId && x.StudentId == st.StudentId && x.Status == "Present").Count(),
                                           totalLeave = db.AttendanceSheet.Where(x => x.ClassId == stdClassId && x.StudentId == st.StudentId && x.Status == "Leave").Count(),
                                           totalAbsent = db.AttendanceSheet.Where(x => x.ClassId == stdClassId && x.StudentId == st.StudentId && x.Status == "Absent").Count(),
                                           className = cl.ClassName,
                                           classId = cl.ClassId
                                       }).ToList();
                   return Json(selectedClassAttendance);
        }
      public JsonResult attendanceOfSelectedStudent(int studentIdForAttendance, int classIdForAttendance)
        {
            //var attendance = db.AttendanceSheet.Where(x => x.ClassId == classIdForAttendance && x.StudentId == studentIdForAttendance);
            var attendance = (from at in db.AttendanceSheet.Where(x => x.ClassId == classIdForAttendance && x.StudentId == studentIdForAttendance)
                              select new
                              {
                                  date = at.Date.ToString(),
                                  status = at.Status
                              });
            return Json(attendance);
        }
    }
}
