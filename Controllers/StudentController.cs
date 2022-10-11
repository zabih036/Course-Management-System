using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JahanInstitute.Models;
using JahanInstitute.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JahanInstitute.Controllers
{
    public class StudentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment hostingEnvironment;
        JahanInstituteContext db = new JahanInstituteContext();
        public StudentController(UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
        }
        [Authorize(Roles = "Teacher")]
        public IActionResult Exam()
        {
            var ExamType = (from d in db.ExamType
                            select new SelectListItem()
                            {
                                Text = d.ExamType1,
                                Value = d.ExamTypeId.ToString()
                            }).ToList();
            ViewBag.ExamType = ExamType;

            var email = _userManager.GetUserAsync(User).Result.Email;
            var empId = db.Employee.Where(x => x.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

            var classes = (from w in db.ClassTeacher.Where(x => x.EmployeeId == empId)
                           join c in db.Class on w.ClassId equals c.ClassId
                           select new SelectListItem()
                           {
                               Text = db.ClassName.Where(r => r.ClassNameId == c.ClassNameId).Select(d => d.ClassName1).FirstOrDefault() + " , Time : " + c.StartHour + " - " + c.EndHour + " , Shift : " + c.Shift,
                               Value = c.ClassId.ToString()
                           }).ToList().GroupBy(x => x.Value).Select(r => r.FirstOrDefault());
            ViewBag.Classes = classes;

            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult StudentRegistration()
        {

            if (TempData["updated"] != null)
            {
                ViewBag.Alert = TempData["updated"];
            }
            else
            {
                ViewBag.Alert = "empty";
            }

            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> StudentRegistration(AllViewModel model, [FromForm] IFormFile img)
        {
            string uploadPath = "";
            string upload = "";
            if (ModelState.IsValid)
            {



                if (img == null && (model.student.camera == null || model.student.camera == "0"))
                {
                    if (model.student.StudentID == 0)
                        upload = "/images/StaticImages/browse.png";
                }
                else if (model.student.camera == null || model.student.camera == "" || model.student.camera == "0")
                {
                    var fileName = model.student.Email + Guid.NewGuid().ToString().Replace("_", "") + Path.GetExtension(img.FileName);
                    upload = Path.Combine("Images/StudentImage/", fileName);
                    uploadPath = Path.Combine(hostingEnvironment.WebRootPath, upload);
                    img.CopyTo(new FileStream(uploadPath, FileMode.Create));
                    upload = "/" + upload;
                }
                else
                {
                    byte[] bitmap = Convert.FromBase64String(model.student.camera.Split(',')[1]);
                    var fileName = model.student.Email + Guid.NewGuid().ToString().Replace("_", "") + ".jpeg";
                    string uploads = Path.Combine("Images/StudentImage/", fileName);
                    uploadPath = Path.Combine(hostingEnvironment.WebRootPath, uploads);
                    System.IO.File.WriteAllBytes(uploadPath, bitmap);
                    //img.CopyTo(new FileStream(uploadPath, FileMode.Create));
                    upload = "/" + uploads;

                }




            }
            if (model.student.StudentID != 0)
            {

                var rec = db.Student.Find(model.student.StudentID);
                if (rec == null)
                {
                    return View("Error");
                }

                rec.Name = model.student.Name;
                rec.FatherName = model.student.FatherName;
                rec.Gender = model.student.Gender;
                rec.Address = model.student.Adress;
                rec.Phone = model.student.Phone;
                rec.Email = model.student.Email;
                //  rec.Reg = model.TazkiraNo;
                rec.Phone = model.student.Phone.ToString();
                rec.Email = model.student.Email;
                rec.IdCardNo = model.student.idCardNumber;
                rec.School = model.student.School;


                if (model.student.defalut != "0")
                {
                    rec.Photo = model.student.defalut;
                }
                else { rec.Photo = upload; }



                if (model.student.defalut == "0")
                {
                    db.Entry(rec).Property(d => d.Photo).IsModified = true;
                }
                else if (model.student.defalut != "0")
                {
                    db.Entry(rec).Property(d => d.Photo).IsModified = true;
                    rec.Photo = model.student.defalut;
                }

                await db.SaveChangesAsync();

                TempData["updated"] = " Student record updated";
                return RedirectToAction("StudentRegistration", "Student");
            }
            else
            {
                try
                {
                    Student it = new Student();
                    it.Name = model.student.Name;
                    it.FatherName = model.student.FatherName;
                    it.Gender = model.student.Gender;
                    it.Address = model.student.Adress;
                    it.Phone = model.student.Phone;
                    it.Email = model.student.Email;
                    it.RegDate = DateTime.Now.Date;
                    it.IdCardNo = model.student.idCardNumber;
                    it.School = model.student.School;
                    it.Photo = upload;
                    db.Student.Add(it);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                TempData["updated"] = "Record Saved Successfully";
                return RedirectToAction("StudentRegistration", "Student");
            }
        }
        public IActionResult IsStudentExist(AllViewModel model)
        {
            if (model.student.StudentID != 0)
            {
                return Json(true);
            }
            var rec = db.Student.Any(d => d.IdCardNo == model.student.idCardNumber);

            if (!rec)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        public JsonResult FetchStudentsForExam(int stdClassId, int ExamType, DateTime Date)
        {
            var SelectedItemRecored = (from s in db.Student
                                       join c in db.StudentClass on s.StudentId equals c.StudentId
                                       where c.ClassId == stdClassId && c.Status != "false"
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
                                           assigned = db.ExamMarks.Where(e => e.ClassId == stdClassId && e.ExamTypeId == ExamType &&
                                                       e.StudentId == s.StudentId &&
                                                       e.Date.Value.Month == Date.Month &&
                                                       e.Date.Value.Day == Date.Day &&
                                                       e.Date.Value.Year == Date.Year).FirstOrDefault()
                                       }).OrderByDescending(r => r.studentId).ToList();


            return Json(SelectedItemRecored);
        }
        public JsonResult FetchStudents()
        {
            var SelectedItemRecored = (from s in db.Student
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
                                           existClasses = (from c in db.StudentClass
                                                           where c.StudentId == s.StudentId
                                                           join cl in db.Class on c.ClassId equals cl.ClassId
                                                           join cll in db.ClassName on cl.ClassNameId equals cll.ClassNameId

                                                           select cll.ClassName1).ToList()
                                           //existClasses = (from d in db.StudentClass
                                           //                where d.StudentId == s.StudentId
                                           //                join c in db.ClassName
                                           //                 on d.ClassId equals c.ClassNameId
                                           //                select c.ClassName1).ToList()

                                       }).OrderByDescending(r => r.studentId).ToList();


            return Json(SelectedItemRecored);
        }
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddStudentMarks(AllViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.exam.ExamId != 0)
                {
                    var em = db.ExamMarks.Find(model.exam.ExamId);

                    em.ExamTypeId = model.exam.ExamTypeId;
                    em.ClassId = model.exam.ClassId;
                    em.StudentId = model.exam.StudentId;
                    em.TotalMarks = model.exam.TotalMarks;
                    em.ObtainMarks = model.exam.ObtainMarks;
                    em.HelpMarks = model.exam.HelpMarks;
                    em.Date = model.exam.Date;
                    em.Description = model.exam.Description;
                    db.Entry(em).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Json("Marks of student updated");
                }
                else
                {
                    ExamMarks ems = new ExamMarks()
                    {
                        ExamTypeId = model.exam.ExamTypeId,
                        ClassId = model.exam.ClassId,
                        StudentId = model.exam.StudentId,
                        TotalMarks = model.exam.TotalMarks,
                        ObtainMarks = model.exam.ObtainMarks,
                        HelpMarks = model.exam.HelpMarks,
                        Date = model.exam.Date,
                        Description = model.exam.Description
                    };
                    db.ExamMarks.Add(ems);
                    await db.SaveChangesAsync();
                    return Json("Marks assigned to student");
                }
            }
            else return Json("Error accured");
        }
        [Authorize(Roles = "Admin,Teacher")]
        public JsonResult FetchStudentClass(int studentId)
        {
            var classes = (from w in db.StudentClass
                           where w.StudentId == studentId
                           join c in db.Class on w.ClassId equals c.ClassId
                           select new SelectListItem()
                           {
                               Text = db.ClassName.Where(r => r.ClassNameId == c.ClassNameId).Select(d => d.ClassName1).FirstOrDefault() + " , Time : " + c.StartHour + " - " + c.EndHour + " , Shift : " + c.Shift,
                               Value = w.ClassId.ToString()
                           }).ToList();

            return Json(classes);
        }
        public JsonResult LoadExamType()
        {
            var rec = db.ExamType.ToList().OrderByDescending(d => d.ExamTypeId);
            return Json(rec);
        }
        public IActionResult IsExamTypeExist(AllViewModel model)
        {
            if (model.examType.ExamTypeId != 0)
            {
                return Json(true);
            }
            var rec = db.ExamType.Any(d => d.ExamType1 == model.examType.ExamType);

            if (!rec)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> SaveExamType(AllViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.examType.ExamTypeId != 0)
                {
                    var rec = await db.ExamType.FindAsync(model.examType.ExamTypeId);
                    if (rec == null)
                    {
                        return View("Error");
                    }
                    rec.ExamType1 = model.examType.ExamType;


                    db.Entry(rec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.Entry(rec).Property(d => d.ExamType1).IsModified = true;

                    await db.SaveChangesAsync();

                    return Json("Exam type updated.");
                }
                else
                {
                    ExamType c = new ExamType()
                    {
                        ExamType1 = model.examType.ExamType
                    };
                    db.ExamType.Add(c);
                    await db.SaveChangesAsync();
                    return Json("Exam type added.");
                }

            }
            return View("Error");
        }
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteExamType(AllViewModel model)
        {
            if (model.examType.ExamTypeId != 0)
            {
                var comp = await db.ExamType.FindAsync(model.examType.ExamTypeId);
                if (comp == null)
                {
                    return View("Error");
                }

                try
                {
                    db.ExamType.Remove(comp);
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
        public JsonResult FetchStudentsExamInfo(int classId, int studentId)
        {
            var rec = (from e in db.ExamMarks
                       where e.StudentId == studentId && e.ClassId == classId
                       join

    t in db.ExamType on e.ExamTypeId equals t.ExamTypeId
                       select new
                       {
                           examId = e.ExamId,
                           classId = e.ClassId,
                           subject = db.ClassName.Where(x => x.ClassNameId == e.ClassId).Select(r => r.ClassName1).FirstOrDefault(),
                           subjectMarks = e.TotalMarks,
                           obtainMarks = e.ObtainMarks,
                           helpedMarks = e.HelpMarks,
                           totalMarks = e.ObtainMarks + e.HelpMarks,
                           examType = t.ExamType1,
                           examTypeId = t.ExamTypeId,
                           description = e.Description,
                           dateShow = Convert.ToDateTime(e.Date).ToShortDateString(),
                           date = e.Date
                       }).ToList();

            if (rec.Count <= 0)
            {
                return Json(0);
            }
            else
            {
                return Json(rec);
            }

        }
        public JsonResult DeleteStudentExamMarks(int examId)
        {
            var rec = db.ExamMarks.Find(examId);
            db.ExamMarks.Remove(rec);
            db.SaveChanges();
            return Json("Student marks removed");

        }
        [Authorize(Roles = "Student")]
        public IActionResult MyAttendanceReport()
        {

            return View();

        }
        public JsonResult loadMyAttendance()
        {
            var email = _userManager.GetUserAsync(User).Result.Email;
            var stdId = db.Student.Where(x => x.Email == email).Select(r => r.StudentId).FirstOrDefault();

            var selectedStudentAttendance = (from sc in db.StudentClass.Where(x => x.StudentId == stdId)
                                             join st in db.Student on sc.StudentId equals st.StudentId
                                             join cl in db.Class on sc.ClassId equals cl.ClassId
                                             join cn in db.ClassName on cl.ClassNameId equals cn.ClassNameId
                                             join de in db.Department on cn.DepartmentId equals de.DepartmentId                                           //  join att in db.AttendanceSheet.Where()
                                                                                                                                                          //  join cl in db.Class on s.ClassId equals cl.ClassId
                                                                                                                                                          //   join att in db.AttendanceSheet on s.ClassId equals att.ClassId
                                             select new
                                             {
                                                 className = cn.ClassName1,
                                                 classId = sc.ClassId,
                                                 department = de.Department1,
                                                 classStartTime = cl.StartHour + ":" + cl.StartMinute + " " + cl.StartMiridiam,
                                                 classEndTime = cl.EndHour + ":" + cl.EndMinute + " " + cl.EndMiridiam,
                                                 studentId = st.StudentId,
                                                 image = st.Photo,
                                                 name = st.Name,
                                                 fName = st.FatherName,
                                                 totalCredit = db.AttendanceSheet.Where(x => x.ClassId == sc.ClassId && x.StudentId == st.StudentId).Count(),
                                                 totalAttended = db.AttendanceSheet.Where(x => x.ClassId == sc.ClassId && x.StudentId == st.StudentId && x.Status == "Present").Count(),
                                                 totalLeave = db.AttendanceSheet.Where(x => x.ClassId == sc.ClassId && x.StudentId == st.StudentId && x.Status == "Leave").Count(),
                                                 totalAbsent = db.AttendanceSheet.Where(x => x.ClassId == sc.ClassId && x.StudentId == st.StudentId && x.Status == "Absent").Count()

                                             }).ToList();
            return Json(selectedStudentAttendance);
        }
        [Authorize(Roles = "Student")]
        public IActionResult StudentExamResult()
        {
            return View();
        }
        public JsonResult FetchLogedInStudentExamInfo()
        {
            var email = _userManager.GetUserAsync(User).Result.Email;
            var stdId = db.Student.Where(x => x.Email == email).Select(r => r.StudentId).FirstOrDefault();

            var rec = (from e in db.ExamMarks
                       where e.StudentId == stdId
                       join t in db.ExamType on e.ExamTypeId equals t.ExamTypeId
                       join s in db.Student on e.StudentId equals s.StudentId
                       select new
                       {
                           examId = e.ExamId,
                           classId = e.ClassId,
                           name = s.Name,
                           phone = s.Phone,
                           photo = s.Photo,
                           subject = e.Class.ClassName.ClassName1,
                           subjectMarks = e.TotalMarks,
                           obtainMarks = e.ObtainMarks,
                           helpedMarks = e.HelpMarks,
                           totalMarks = e.ObtainMarks + e.HelpMarks,
                           examType = t.ExamType1,
                           examTypeId = t.ExamTypeId,
                           description = e.Description,
                           dateShow = Convert.ToDateTime(e.Date).ToShortDateString(),
                           date = e.Date,
                       }).ToList();

            return Json(rec);
        }
        public async Task<IActionResult> TopStudents()
        {
            //var ExamType = (from d in db.ExamType
            //                select new SelectListItem()
            //                {
            //                    Text = d.ExamType1,
            //                    Value = d.ExamTypeId.ToString()
            //                }).ToList();
            //ViewBag.ExamType = ExamType;
            var email = _userManager.GetUserAsync(User).Result.Email;
            var user = await _userManager.FindByEmailAsync(email);
            var role = await _userManager.GetRolesAsync(user);
            var empId = db.Employee.Where(x => x.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

            var classes = (from cl in db.Class
                           join cn in db.ClassName on cl.ClassNameId equals cn.ClassNameId
                           select new SelectListItem()
                           {
                               Text = db.ClassName.Where(r => r.ClassNameId == cn.ClassNameId).Select(d => d.ClassName1).FirstOrDefault() + " , Time : " + cl.StartHour + " - " + cl.EndHour + " , Shift : " + cl.Shift,
                               Value = cl.ClassId.ToString()
                           }).ToList().GroupBy(x => x.Value).Select(r => r.FirstOrDefault());

            if (role[0] == "Admin")
            {
                classes = (from cl in db.ClassTeacher
                           join cn in db.Class on cl.ClassId equals cn.ClassId
                           select new SelectListItem()
                           {
                               Text = db.ClassName.Where(r => r.ClassNameId == cn.ClassNameId).Select(d => d.ClassName1).FirstOrDefault() + " , Time : " + cn.StartHour + " - " + cn.EndHour + " , Shift : " + cn.Shift,
                               Value = cl.ClassId.ToString()
                           }).ToList().GroupBy(x => x.Value).Select(r => r.FirstOrDefault());
            }
            else
            {
                classes = (from cl in db.ClassTeacher
                           join cn in db.Class on cl.ClassId equals cn.ClassId
                           where cl.EmployeeId == empId
                           select new SelectListItem()
                           {
                               Text = db.ClassName.Where(r => r.ClassNameId == cn.ClassNameId).Select(d => d.ClassName1).FirstOrDefault() + " , Time : " + cn.StartHour + " - " + cn.EndHour + " , Shift : " + cn.Shift,
                               Value = cl.ClassId.ToString()
                           }).ToList().GroupBy(x => x.Value).Select(r => r.FirstOrDefault());
            }


            ViewBag.Classes = classes;
            return View();
        }
        public JsonResult FetchTopStudents(ManulReprotViewModel model)
        {
            var email = _userManager.GetUserAsync(User).Result.Email;
            var stdId = db.Student.Where(x => x.Email == email).Select(r => r.StudentId).FirstOrDefault();

            var rec = (from s in db.Student
                       join c in db.StudentClass on s.StudentId equals c.StudentId
                       join cl in db.Class on c.ClassId equals cl.ClassId
                       where c.ClassId == model.classId && c.Status != "false"
                       select new
                       {
                           name = s.Name,
                           fName = s.FatherName,
                           phone = s.Phone,
                           photo = s.Photo,
                           classs = db.ClassName.Where(x => x.ClassNameId == cl.ClassNameId).Select(r => r.ClassName1).FirstOrDefault(),
                           totalMarks = db.ExamMarks.Where(x => x.ClassId == c.ClassId && x.StudentId == s.StudentId && x.Date >= model.FromDate && x.Date <= model.ToDate).Sum(r => r.ObtainMarks) + db.ExamMarks.Where(x => x.ClassId == c.ClassId && x.StudentId == s.StudentId && x.Date >= model.FromDate && x.Date <= model.ToDate).Sum(r => r.HelpMarks),
                           totalExams = db.ExamMarks.Where(x => x.ClassId == c.ClassId && x.StudentId == s.StudentId && x.Date >= model.FromDate && x.Date <= model.ToDate).Count(),
                       }).ToList();

            return Json(rec.OrderByDescending(r => r.totalMarks));
        }
    }
}
