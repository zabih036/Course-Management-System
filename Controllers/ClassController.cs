using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using JahanInstitute.Models.ViewModels;
using JahanInstitute;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace JahanInstitute.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClassController : Controller
    {
        JahanInstituteContext db = new JahanInstituteContext();


        public IActionResult Index()
        {
            var classes = (from d in db.ClassName
                           select new SelectListItem()
                           {
                               Text = d.ClassName1,
                               Value = d.ClassNameId.ToString()
                           }).ToList();
            ViewBag.classes = classes;

            var classType = (from d in db.ClassType
                             select new SelectListItem()
                             {
                                 Text = d.ClassType1,
                                 Value = d.ClassTypeId.ToString()
                             }).ToList();
            ViewBag.classType = classType;
            return View();
        }
        [HttpGet]
        public IActionResult classRegistration()
        {
            var department = (from d in db.Department
                              select new SelectListItem()
                              {
                                  Text = d.Department1,
                                  Value = d.DepartmentId.ToString()
                              }).ToList();
            ViewBag.department = department;

            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Registration(classRegistrationViewModel model)
        {
            if (model.classNameId == 0)
            {
                ClassName cl = new ClassName();
                cl.ClassName1 = model.className;
                cl.DepartmentId = model.department;
                db.ClassName.Add(cl);
                await db.SaveChangesAsync();
                return Json("New Class Added Successfully");
            }
            else
            {
                var recored = db.ClassName.Where(x => x.ClassNameId == model.classNameId).FirstOrDefault();

                recored.ClassName1 = model.className;
                recored.DepartmentId = model.department;
                db.Entry(recored).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Json("Class Record Update Successfully");
            }

        }
        public IActionResult isClassExist(classRegistrationViewModel model)
        {
            if (model.classNameId != 0)
            {
                return Json(true);
            }
            var rec = db.ClassName.Any(d => d.ClassName1 == model.className);

            if (!rec)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        public JsonResult fetchClasses()
        {

            var allClasses = (from c in db.ClassName
                              join d in db.Department on c.DepartmentId equals d.DepartmentId

                              select new
                              {
                                  classNameId = c.ClassNameId,
                                  className = c.ClassName1,
                                  department = d.Department1,
                                  departmentId = c.DepartmentId

                              }).ToList();

            return Json(allClasses);
        }
        public async Task<JsonResult> deleteClass(int classId)
        {
            try
            {
                var record = db.ClassName.Where(x => x.ClassNameId == classId).FirstOrDefault();
                db.ClassName.Remove(record);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Json("You can't delete this record");

            }
            return Json("Target Class Deleted Successfully");
        }
        public JsonResult addClass(AllViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.clas.classId != 0)
                {
                    var rec = db.Class.Find(model.clas.classId);
                    rec.ClassNameId = model.clas.ClassName;
                    rec.Shift = model.clas.shift;
                    //  rec.StartTime =  TimeSpan.Parse(model.startTime.TimeOfDay.ToString("hh:mm:ss tt"));
                    //  rec.EndTime = TimeSpan.Parse(model.endTime.TimeOfDay.ToString("hh:mm:ss tt"));
                    rec.StartHour = model.clas.startHours;
                    rec.StartMinute = model.clas.startMinutes;
                    rec.StartMiridiam = model.clas.startMiridiam;

                    rec.EndHour = model.clas.endHours;
                    rec.EndMinute = model.clas.endMinutes;
                    rec.EndMiridiam = model.clas.endMiridiam;

                    rec.StartDate = model.clas.startDate.Date;
                    rec.EndDate = model.clas.endDate.Date;
                    rec.TotalFee = model.clas.totalFee;
                    db.Entry(rec).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json("Class Record Update Successfully");
                }
                else
                {
                    try
                    {
                        Class cl = new Class();
                        cl.ClassNameId = model.clas.ClassName;
                        cl.Shift = model.clas.shift;
                        // cl.StartTime = TimeSpan.Parse(model.startTime.ToString("hh:mm tt"));
                        //    cl.StartTime = TimeSpan.Parse(model.startTime.TimeOfDay.ToString());
                        //   cl.EndTime = TimeSpan.Parse(model.endTime.TimeOfDay.ToString());

                        cl.StartHour = model.clas.startHours;
                        cl.StartMinute = model.clas.startMinutes;
                        cl.StartMiridiam = model.clas.startMiridiam;

                        cl.EndHour = model.clas.endHours;
                        cl.EndMinute = model.clas.endMinutes;
                        cl.EndMiridiam = model.clas.endMiridiam;

                        cl.StartDate = model.clas.startDate.Date;
                        cl.EndDate = model.clas.endDate.Date;
                        cl.TotalFee = model.clas.totalFee;
                        db.Class.Add(cl);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    return Json("Record Saved Successfully");
                }
            }
            return Json("There is error in model");

        }
        public JsonResult FetchClass()
        {
            var rec = (from s in db.Class
                       join n in db.ClassName on s.ClassNameId equals n.ClassNameId
                       join d in db.Department on n.DepartmentId equals d.DepartmentId
                       select new
                       {
                           classId = s.ClassId,
                           className = n.ClassName1,
                           classNameId = s.ClassNameId,

                           department = d.Department1,
                           departmentId = n.DepartmentId,

                           //   startTime = s.StartTime,
                           //  endTime = s.EndTime,
                           startHour = s.StartHour,
                           startMinute = s.StartMinute,
                           startMiridiam = s.StartMiridiam,

                           endMiridiam = s.EndMiridiam,
                           endHour = s.EndHour,
                           endMinute = s.EndMinute,

                           startDateShow = Convert.ToDateTime(s.StartDate).ToShortDateString(),
                           endDateShow = Convert.ToDateTime(s.EndDate).ToShortDateString(),

                           startDate = s.StartDate,
                           endDate = s.EndDate,

                           shift = s.Shift,

                           totalFee = s.TotalFee,
                           existTeachers = (from te in db.Employee.Where(x => x.EmployeeTypeId == 2)
                                            join cl in db.ClassTeacher.Where(x => x.ClassId == s.ClassId) on te.EmployeeId equals cl.EmployeeId
                                            select new
                                            {
                                                teClassId = cl.ClassTeacherId,
                                                teName = te.Name,
                                                teId = te.EmployeeId,
                                                teClass = cl.ClassId
                                            }).ToList()
                       }).ToList().OrderByDescending(x => x.classId);

            return Json(rec);
        }
        public async Task<IActionResult> AddStudentToclass(AllViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    StudentClass std = new StudentClass()
                    {
                        StudentId = model.stdClass.StudentId,
                        ClassId = model.stdClass.ClassId,
                        RollNumber = model.stdClass.RollNumber,
                        Date = DateTime.Now.Date,
                        Status = "true"
                    };
                    db.StudentClass.Add(std);
                    await db.SaveChangesAsync();
                    Fees fee = new Fees()
                    {
                        StudentId = model.stdClass.StudentId,
                        ClassId = model.stdClass.ClassId,
                        Paid = model.stdClass.PaidAmount,
                        Discount = model.stdClass.Discount,
                        BillNo = model.stdClass.BillNo,
                        Date = DateTime.Now.Date,
                        Description = model.stdClass.Description,
                    };
                    db.Fees.Add(fee);
                    await db.SaveChangesAsync();

                    return Json("Student enrolled in the class");
                }
                catch (Exception e)
                {

                    return Json(e);
                }

            }
            return Json("Error");
        }
        public JsonResult IsStudentEnrolled(AllViewModel model)
        {
            try
            {
                var rec = db.StudentClass.Where(r => r.StudentId == model.stdClass.StudentId && r.ClassId == model.stdClass.ClassId).FirstOrDefault();
                if (rec == null)
                {
                    var rec2 = db.StudentClass.Where(r => r.ClassId == model.stdClass.ClassId).OrderByDescending(r => r.StdClassId).FirstOrDefault();
                    if (rec2 == null)
                    {
                        return Json(1);
                    }
                    else if (rec2.RollNumber == 100)
                    {
                        return Json(1);
                    }
                    else
                    {
                        if (rec2.RollNumber == null)
                        {
                            return Json(1);
                        }
                        else
                        {
                            return Json(rec2.RollNumber + 1);
                        }

                    }
                }
                else return Json("Exist");
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public JsonResult FetchClassStudentAndTeacher(int classId)
        {
            if (classId != 0)
            {
                var students = (from s in db.Student
                                join cl in db.StudentClass on s.StudentId equals cl.StudentId
                                where cl.ClassId == classId
                                select new
                                {
                                    rowId = cl.StdClassId,
                                    studentId = s.StudentId,
                                    classId = cl.ClassId,
                                    image = s.Photo,
                                    name = s.Name,
                                    fName = s.FatherName,
                                    gender = s.Gender,
                                    address = s.Address,
                                    phone = s.Phone,
                                    email = s.Email,
                                    regDate = Convert.ToDateTime(s.RegDate).ToShortDateString(),
                                    idCard = s.IdCardNo,
                                    school = s.School,
                                }).ToList();
                var teachers = (from t in db.Employee
                                join cl in db.ClassTeacher on t.EmployeeId equals cl.EmployeeId
                                where cl.ClassId == classId
                                select new
                                {
                                    rowId = cl.ClassTeacherId,
                                    teacherId = t.EmployeeId,
                                    classId = cl.ClassId,
                                    image = t.Photo,
                                    name = t.Name,
                                    fName = t.FatherName,
                                    phone = t.Phone,
                                    email = t.Email,

                                    hireDate = Convert.ToDateTime(t.HireDate).ToShortDateString(),
                                    idCard = t.IdCardNo,
                                }).ToList();
                var allRecords = new
                {
                    teachers = teachers,
                    students = students
                };
                return Json(allRecords);
            }
            else
            {
                return Json("Class Id is not recongnized");
            }

        }
        public JsonResult FetchTeachersForClassAssign()
        {
            var teachers = db.Employee.Where(x => x.EmployeeTypeId == 2).ToList();
            return Json(teachers);
        }
        [HttpPost]
        public async Task<JsonResult> assignTeacherToClass(int classID, int teacherID, int hourlyAmount)
        {
            var alreadyExist = db.ClassTeacher.Where(x => x.ClassId == classID && x.EmployeeId == teacherID).ToList();
            if (alreadyExist.Count() > 0)
            {
                return Json("Exist");
            }
            else
            {
                if (hourlyAmount > 0)
                {
                    HourlyClass hc = new HourlyClass();
                    hc.ClassId = classID;
                    hc.EmployeeId = teacherID;
                    hc.TotalHours = hourlyAmount;
                    db.HourlyClass.Add(hc);
                    await db.SaveChangesAsync();
                }

                ClassTeacher obj = new ClassTeacher();
                obj.ClassId = classID;
                obj.EmployeeId = teacherID;
                db.ClassTeacher.Add(obj);
                await db.SaveChangesAsync();

                var lastrecord = db.ClassTeacher.OrderByDescending(x => x.ClassTeacherId).FirstOrDefault();
                var data = new
                {
                    classId = classID,
                    teacherId = teacherID,
                    classTeacherId = lastrecord.ClassTeacherId
                };
                return Json(data);
            }
        }
        public async Task<JsonResult> deleteTeacherFromClass(int ClassIDForDeleteTeacher)
        {
            var record = db.ClassTeacher.Where(x => x.ClassTeacherId == ClassIDForDeleteTeacher).FirstOrDefault();
            var removeHourly = db.HourlyClass.Where(x => x.ClassId == record.ClassId && x.EmployeeId == record.EmployeeId).FirstOrDefault();
            db.ClassTeacher.Remove(record);
            await db.SaveChangesAsync();
            if (removeHourly != null)
            {
                db.HourlyClass.Remove(removeHourly);
                await db.SaveChangesAsync();
            }

            return Json("Selected teacher removed successfullay form class");
        }
        [HttpGet]
        public IActionResult Department()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Department(DepartmentViewModel model)
        {
            if (model.DepartmentId == 0)
            {
                Department dep = new Department
                {
                    Department1 = model.Department
                };
                db.Department.Add(dep);
                await db.SaveChangesAsync();
                return Json("New Department Added Successfully");
            }
            else
            {
                var recored = db.Department.Find(model.DepartmentId);
                recored.Department1 = model.Department;

                db.Entry(recored).State = EntityState.Modified;
                db.Entry(recored).Property(x => x.Department1).IsModified = true;
                await db.SaveChangesAsync();
                return Json("Department Update Successfully");
            }

        }
        public IActionResult isDepartmentExist(DepartmentViewModel model)
        {
            if (model.DepartmentId != 0)
            {
                return Json(true);
            }
            var rec = db.Department.Any(d => d.Department1 == model.Department);

            if (!rec)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        public async Task<JsonResult> deleteDepartment(int departmentId)
        {
            try
            {
                var record = db.Department.Where(x => x.DepartmentId == departmentId).FirstOrDefault();
                db.Department.Remove(record);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Json("You can't delete this Department First remove all its instance");

            }
            return Json("Department Deleted Successfully");
        }
        public JsonResult fetchDepartments()
        {

            var rec = db.Department.ToList();

            return Json(rec);
        }
        public async Task<JsonResult> CheckStudentFees()
        {
            var rec = (from sc in db.StudentClass
                       join c in db.Class on sc.ClassId equals c.ClassId
                       join f in db.Fees on sc.ClassId equals f.ClassId
                       where f.Paid == 0 && sc.Date <= DateTime.Now.Date.AddDays(-4) && sc.Status != "false"
                       select sc).ToList();
            foreach (var item in rec)
            {
                item.Status = "false";
                db.Entry(item).State = EntityState.Modified;
                db.Entry(item).Property(x => x.Status).IsModified = true;
                await db.SaveChangesAsync();
            }


            return Json("done");
        }
        public async Task<JsonResult> checkStudentAbsent()
        {
            //var rec = (from sc in db.StudentClass
            //           let stu = db.StudentClass.Where(x => x.StudentId == sc.StudentId && sc.ClassId == sc.ClassId && x.Status != "false").FirstOrDefault()
            //           let f = db.AttendanceSheet.Where(x => x.StudentId == sc.StudentId && sc.ClassId == sc.ClassId && x.Status == "Absent").Count()
            //           where f == 4
            //           select stu).ToList();
            foreach (var item in db.StudentClass.Where(x=>x.Status!="false").ToList())
            {
                try
                {
                    var f = db.AttendanceSheet.Where(x => x.StudentId == item.StudentId && x.ClassId == item.ClassId && x.Status == "Absent").Count();
                    if (f == 4)
                    {
                        item.Status = "false";
                        db.Entry(item).State = EntityState.Modified;
                        db.Entry(item).Property(x => x.Status).IsModified = true;
                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {

                    return Json(ex);
                }
            }

            return Json("done");
        }
    }
}
