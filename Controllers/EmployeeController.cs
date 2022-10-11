using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using JahanInstitute.Models;
using JahanInstitute.Models.ViewModels;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace JahanInstitute.Controllers
{

    public class EmployeeController : Controller
    {

        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IConfiguration configuration;
        protected SqlConnection _conn;
        protected SqlDataReader dr;
        DataTable dt = new DataTable();
        protected string _address;
        protected string _user;
        protected string _pass;
        protected string _dbname = "JahanInstitute";
        JahanInstituteContext db = new JahanInstituteContext();
        [Obsolete]
        public EmployeeController(IWebHostEnvironment hostingEnvironment,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            this.hostingEnvironment = hostingEnvironment;


            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            configuration = builder.Build();



            _conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            _conn.Open();


        }

        [Authorize(Roles = "Admin")]
        public IActionResult EmployeeSalary()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Employee()
        {
            var c = (from d in db.EmployeeType
                     select new SelectListItem()
                     {
                         Text = d.EmployeeId,
                         Value = d.EmployeeTypeId.ToString()
                     }).ToList();

            ViewBag.EmployeeType = c;

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
        public JsonResult LoadEmployee()
        {
            var rec = (from e in db.Employee
                       join s in db.EmployeeType on e.EmployeeTypeId equals s.EmployeeTypeId
                       select new
                       {
                           employeeId = e.EmployeeId,
                           name = e.Name,
                           fName = e.FatherName,
                           phone = e.Phone,
                           email = e.Email,
                           province = e.Province,
                           district = e.District,
                           village = e.Village,
                           idCardNo = e.IdCardNo,
                           employeeType = s.EmployeeId,
                           employeeTypeId = s.EmployeeTypeId,
                           salaryType = e.SalaryType,
                           salary = e.Salary,
                           hireDate = e.HireDate,
                           hireDateShow = Convert.ToDateTime(e.HireDate).ToShortDateString(),
                           fireDate = e.FireDate,
                           fireDateShow = Convert.ToDateTime(e.FireDate).ToShortDateString(),
                           photo = e.Photo,
                       }).OrderByDescending(d => d.employeeId);
            return Json(rec);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult LoadEmployeeSalary()
        {
            try
            {
                var rec = (from e in db.Employee
                           where e.SalaryType == "Fixed"
                           select new
                           {
                               id = e.EmployeeId,
                               salaryId = db.Salary.Where(x => x.EmployeeId == e.EmployeeId).OrderByDescending(x => x.SalaryId).Select(r => r.SalaryId).FirstOrDefault(),
                               imagePath = e.Photo,
                               name = e.Name,
                               phone = e.Phone,
                               hireDate = Convert.ToDateTime(e.HireDate).ToShortDateString(),
                               email = e.Email,
                               absentCount = db.TeacherAttendance.Where(x => x.EmployeeId == e.EmployeeId && x.Status == "Absent"&& x.Date.Value.Year ==DateTime.Now.Year && x.Date.Value.Month == DateTime.Now.Month ).Count(),                              
                               absentAmount = ((e.Salary / 30) * db.TeacherAttendance.Where(x => x.EmployeeId == e.EmployeeId && x.Status == "Absent" && x.Date.Value.Year == DateTime.Now.Year && x.Date.Value.Month == DateTime.Now.Month).Count()),
                               salary = e.Salary,
                               paidAmount = Convert.ToInt32(db.Salary.Where(x => x.EmployeeId == e.EmployeeId).OrderByDescending(x => x.SalaryId).Select(r => r.PaidAmount).FirstOrDefault()),
                               totalMonths = Math.Floor(DateTime.Now.Date.Subtract(Convert.ToDateTime(e.HireDate)).TotalDays / 30),
                               totalPaid = Convert.ToInt32(db.Salary.Where(p => p.EmployeeId == e.EmployeeId).Sum(d => d.PaidAmount).ToString()),
                               totalSalary = e.Salary * Math.Floor(DateTime.Now.Date.Subtract(Convert.ToDateTime(e.HireDate)).TotalDays / 30) ,
                               paidDate = db.Salary.Where(x => x.EmployeeId == e.EmployeeId).OrderByDescending(x => x.SalaryId).Select(r => r.Date).FirstOrDefault()
                           }).ToList();
                return Json(rec);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.StackTrace);
            }

        }
        [Authorize(Roles = "Admin")]
        public IActionResult LoadEmployeeSalaryPercentage()
        {
            try
            {
                var rec = (from c in db.ClassTeacher
                           join e in db.Employee on c.EmployeeId equals e.EmployeeId
                           let cl = db.Class.FirstOrDefault(x => x.ClassId == c.ClassId && c.EmployeeId == c.EmployeeId)
                           where e.SalaryType == "Percentage"
                           select new
                           {
                               id = e.EmployeeId,
                               classId = c.ClassId,
                               salaryId = db.Salary.Where(x => x.EmployeeId == e.EmployeeId && x.ClassId == c.ClassId).OrderByDescending(x => x.SalaryId).Select(r => r.SalaryId).FirstOrDefault(),
                               imagePath = e.Photo,
                               name = e.Name,
                               phone = e.Phone,
                               percentage = e.Salary,
                               absentCount = db.TeacherAttendance.Where(x => x.EmployeeId == e.EmployeeId && x.Status == "Absent" && x.Date >= cl.StartDate && x.Date <= cl.EndDate).Count(),
                               totalClassDays = Convert.ToDateTime(cl.EndDate).Subtract(Convert.ToDateTime( cl.StartDate)).TotalDays,
                               hireDate = Convert.ToDateTime(e.HireDate).ToShortDateString(),
                               className = c.Class.ClassName.ClassName1,
                               fees = db.Class.Where(r => r.ClassId == c.ClassId).Select(w => w.TotalFee).FirstOrDefault(),
                               students = db.StudentClass.Where(q => q.ClassId == c.ClassId).Count(),
                               totalPaid = Convert.ToInt32(db.Salary.Where(x => x.EmployeeId == e.EmployeeId && x.ClassId == c.ClassId && x.Date >= cl.StartDate && x.Date <= cl.EndDate).Sum(r => r.PaidAmount)),
                               lastPaid = Convert.ToInt32(db.Salary.Where(x => x.EmployeeId == e.EmployeeId && x.ClassId == c.ClassId && x.Date >= cl.StartDate && x.Date <= cl.EndDate).OrderByDescending(r => r.SalaryId).Select(r => r.PaidAmount).FirstOrDefault()),
                               date = db.Salary.Where(x => x.EmployeeId == e.EmployeeId && x.ClassId == c.ClassId).OrderByDescending(r => r.SalaryId).Select(r => r.Date).FirstOrDefault().ToString(),
                               //totalRemain = Convert.ToInt32(e.Salary) * Convert.ToInt32(Math.Floor(DateTime.Now.Date.Subtract(Convert.ToDateTime(e.HireDate)).TotalDays / 30)) - Convert.ToInt32(db.Salary.Where(x => x.EmployeeId == e.EmployeeId).Select(r => r.PaidAmount).FirstOrDefault()), 
                               //paidDate = db.Salary.Where(x => x.EmployeeId == e.EmployeeId).OrderByDescending(x => x.SalaryId).Select(r => r.Date).FirstOrDefault()
                           }).ToList();

                return Json(rec);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.StackTrace);
            }

        }
        [Authorize(Roles = "Admin")]
        public IActionResult LoadEmployeeSalaryHourly()
        {
            try
            {
                var rec = (from  c in db.ClassTeacher
                           join e in db.Employee on c.EmployeeId equals e.EmployeeId
                           let cl = db.Class.FirstOrDefault(x => x.ClassId == c.ClassId && c.EmployeeId == c.EmployeeId)
                           where e.SalaryType == "Hourly"
                           select new
                           {
                               id = e.EmployeeId,
                               classId= c.ClassId,
                               className = c.Class.ClassName.ClassName1,
                               totalHours = db.HourlyClass.Where(r=>r.ClassId == c.ClassId && r.EmployeeId == e.EmployeeId).Select(r=>r.TotalHours).FirstOrDefault(),
                               absentCount = db.TeacherAttendance.Where(x => x.EmployeeId == e.EmployeeId && x.Status == "Absent" && x.Date >= cl.StartDate && x.Date <= cl.EndDate).Count(),
                               classFees = db.Class.Where(r=>r.ClassId == c.ClassId).Select(r=>r.TotalFee).FirstOrDefault(),
                               salaryId = db.Salary.Where(x => x.EmployeeId == e.EmployeeId).OrderByDescending(x => x.SalaryId).Select(r => r.SalaryId).FirstOrDefault(),
                               imagePath = e.Photo,
                               name = e.Name,
                               phone = e.Phone,
                               hireDate = Convert.ToDateTime(e.HireDate).ToShortDateString(),
                               perHour = e.Salary,
                               totalPaid = Convert.ToInt32(db.Salary.Where(x => x.EmployeeId == e.EmployeeId && x.ClassId==c.ClassId && x.Date >= cl.StartDate && x.Date <= cl.EndDate).Sum(r => r.PaidAmount)),
                               lastPaid = Convert.ToInt32(db.Salary.Where(x => x.EmployeeId == e.EmployeeId && x.ClassId == c.ClassId && x.Date >= cl.StartDate && x.Date <= cl.EndDate).OrderByDescending(r => r.SalaryId).Select(r => r.PaidAmount).FirstOrDefault()),
                               date = db.Salary.Where(x => x.EmployeeId == e.EmployeeId).OrderByDescending(r => r.SalaryId).Select(r => r.Date).FirstOrDefault().ToString(),
                               //totalRemain = Convert.ToInt32(e.Salary) * Convert.ToInt32(Math.Floor(DateTime.Now.Date.Subtract(Convert.ToDateTime(e.HireDate)).TotalDays / 30)) - Convert.ToInt32(db.Salary.Where(x => x.EmployeeId == e.EmployeeId).Select(r => r.PaidAmount).FirstOrDefault()), 
                               //paidDate = db.Salary.Where(x => x.EmployeeId == e.EmployeeId).OrderByDescending(x => x.SalaryId).Select(r => r.Date).FirstOrDefault()
                           }).ToList();

                return Json(rec);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.StackTrace);
            }

        }
        public JsonResult FetchEmployeeClassSalary(int employeeId, int classId)
        {
            var rec = (from c in db.Salary
                       where c.EmployeeId == employeeId && c.ClassId == classId
                       select new
                       {
                           salaryId = c.SalaryId,
                           paidAmount = c.PaidAmount,
                           paidDate = Convert.ToDateTime(c.Date).ToShortDateString(),

                       }).ToList();

            return Json(rec);
        }
        public JsonResult FetchEmployeeClassHourly(int employeeId,int classId)
        {
            var rec = (from c in db.Salary
                       where c.EmployeeId == employeeId && c.ClassId == classId
                       select new
                       {
                           salaryId = c.SalaryId,
                           paidAmount = c.PaidAmount,
                           paidDate = Convert.ToDateTime(c.Date).ToShortDateString(),
                       }).ToList();

            return Json(rec);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EmployeeRegistration(AllViewModel model, [FromForm] IFormFile img)
        {
            if (ModelState.IsValid)
            {
                string uploadPath = "";
                string upload = "";
                if (img == null)
                {
                    if (model.employee.EmployeeId == 0)
                        upload = "/images/StaticImages/browse.png";
                }
                else
                {


                    var fileName = model.employee.Email + Guid.NewGuid().ToString().Replace("_", "") + Path.GetExtension(img.FileName);
                    upload = Path.Combine("Images/EmployeeImage/", fileName);
                    uploadPath = Path.Combine(hostingEnvironment.WebRootPath, upload);
                    img.CopyTo(new FileStream(uploadPath, FileMode.Create));
                    upload = "/" + upload;

                }


                if (model.employee.EmployeeId != 0)
                {
                    var user1 = await _userManager.FindByEmailAsync(model.employee.Email);
                    string ro = "";
                    if (user1 != null)
                    {
                        var role = await _userManager.GetRolesAsync(user1);
                        ro = role[0].ToString();
                        var logeduser = await _userManager.GetUserAsync(User);
                        var rol = await _userManager.GetRolesAsync(logeduser);
                        string r = rol[0].ToString();
                        if (ro == "Admin")
                        {
                            if (r == "Admin")
                            {
                                var rec1 = await db.Employee.FindAsync(model.employee.EmployeeId);
                                if (rec1 == null)
                                {
                                    return View("Error");
                                }
                                rec1.Name = model.employee.Name;
                                rec1.FatherName = model.employee.FatherName;
                                rec1.Phone = model.employee.Phone;
                                rec1.Email = model.employee.Email;
                                rec1.Province = model.employee.Province;
                                rec1.District = model.employee.District;
                                rec1.Village = model.employee.Vilage;
                                rec1.IdCardNo = model.employee.IdCardNo;
                                rec1.EmployeeTypeId = model.employee.EmployeeType_Id;
                                rec1.SalaryType = model.employee.SalaryType;
                                rec1.Salary = model.employee.Salary;
                                rec1.HireDate = model.employee.HireDate;
                                rec1.FireDate = model.employee.FireDate;

                                var user2 = await _userManager.FindByEmailAsync(model.employee.oldEmail);

                                if (img == null && model.employee.defalut != null)
                                {
                                    rec1.Photo = model.employee.defalut;
                                }
                                else { rec1.Photo = upload; }


                                db.Entry(rec1).State = EntityState.Modified;
                                if (img != null && model.employee.defalut == "0")
                                {
                                    db.Entry(rec1).Property(d => d.Photo).IsModified = true;
                                    if (user2 != null)
                                        user2.ImagePath = upload;
                                }
                                else if (model.employee.defalut != "0")
                                {
                                    db.Entry(rec1).Property(d => d.Photo).IsModified = true;
                                    if (user2 != null)
                                        user2.ImagePath = model.employee.defalut;
                                }
                                if (user2 != null)
                                {


                                    var result = await _userManager.SetEmailAsync(user2, model.employee.Email);
                                    await _userManager.SetUserNameAsync(user2, model.employee.Email);
                                    if (result.Succeeded)
                                    {
                                        user2.EmployeeName = model.employee.Name;
                                        user2.EmailConfirmed = true;
                                        await _userManager.UpdateAsync(user2);
                                    }
                                    else
                                    {
                                        return View("Error");
                                    }

                                }

                                await db.SaveChangesAsync();



                                TempData["updated"] = " Employee record updated";

                                return RedirectToAction("Employee", "Employee");
                            }
                            else
                            {
                                return View("Error");
                            }

                        }
                    }


                    var rec = await db.Employee.FindAsync(model.employee.EmployeeId);
                    if (rec == null)
                    {
                        return View("Error");
                    }
                    rec.Name = model.employee.Name;
                    rec.FatherName = model.employee.FatherName;
                    rec.Phone = model.employee.Phone;
                    rec.Email = model.employee.Email;
                    rec.Province = model.employee.Province;
                    rec.District = model.employee.District;
                    rec.Village = model.employee.Vilage;
                    rec.IdCardNo = model.employee.IdCardNo;
                    rec.EmployeeTypeId = model.employee.EmployeeType_Id;
                    rec.SalaryType = model.employee.SalaryType;
                    rec.Salary = model.employee.Salary;
                    rec.HireDate = model.employee.HireDate;
                    rec.FireDate = model.employee.FireDate;

                    var user = await _userManager.FindByEmailAsync(model.employee.oldEmail);

                    if (img == null && model.employee.defalut != null)
                    {
                        rec.Photo = model.employee.defalut;
                    }
                    else { rec.Photo = upload; }


                    db.Entry(rec).State = EntityState.Modified;
                    if (img != null && model.employee.defalut == "0")
                    {
                        db.Entry(rec).Property(d => d.Photo).IsModified = true;
                        if (user != null)
                            user.ImagePath = upload;
                    }
                    else if (model.employee.defalut != "0")
                    {
                        db.Entry(rec).Property(d => d.Photo).IsModified = true;
                        if (user != null)
                            user.ImagePath = model.employee.defalut;
                    }
                    if (user != null)
                    {


                        var result = await _userManager.SetEmailAsync(user, model.employee.Email);
                        await _userManager.SetUserNameAsync(user, model.employee.Email);
                        if (result.Succeeded)
                        {
                            user.EmployeeName = model.employee.Name;
                            user.EmailConfirmed = true;
                            await _userManager.UpdateAsync(user);
                        }
                        else
                        {
                            return View("Error");
                        }

                    }

                    await db.SaveChangesAsync();



                    TempData["updated"] = " Employee record updated";
                    // ViewBag.Alert = "";
                    return RedirectToAction("Employee", "Employee");

                }
                else
                {
                    try
                    {


                        Employee it = new Employee()
                        {
                            Name = model.employee.Name,
                            FatherName = model.employee.FatherName,
                            Phone = model.employee.Phone,
                            Email = model.employee.Email,
                            Province = model.employee.Province,
                            District = model.employee.District,
                            Village = model.employee.Vilage,
                            IdCardNo = model.employee.IdCardNo,
                            EmployeeTypeId = model.employee.EmployeeType_Id,
                            SalaryType = model.employee.SalaryType,
                            Salary = model.employee.Salary,
                            HireDate = Convert.ToDateTime(model.employee.HireDate.ToShortDateString()).Date,
                            FireDate = Convert.ToDateTime(model.employee.FireDate.ToShortDateString()).Date,
                            Photo = upload
                        };
                        db.Employee.Add(it);

                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                        {

                        throw new Exception(e.Message);
                    }
                    TempData["updated"] = "Employee registered";
                    return RedirectToAction("Employee", "Employee");
                }

            }
            return View("Error");

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(AllViewModel model)
        {
            if (model.employee.EmployeeId != 0)
            {
                var comp = await db.Employee.FindAsync(model.employee.EmployeeId);
                if (comp == null)
                {
                    return View("Error");
                }
              
                try
                {
                    var recs = db.Note.Where(d => d.UserEmail == model.employee.Email);
                    if (recs != null)
                        db.Note.RemoveRange(recs);
                    var account = await _userManager.FindByEmailAsync(model.employee.Email);
                    if (account != null)
                    {
                        var result = await _userManager.DeleteAsync(account);
                        if (!result.Succeeded)
                        {
                            return View("Error");
                        }
                    }

                    db.Employee.Remove(comp);
                    await db.SaveChangesAsync();
                    return Json("Employee recored deleted.");
                }
                catch (Exception)
                {

                    return Json("You can't delete this record.");
                }
            }
            return NotFound();

        }
        public IActionResult IsEmployeeEmailExist(AllViewModel model)
        {
            if (model.employee.EmployeeId != 0)
            {
                var em = db.Employee.Where(d => d.Email == model.employee.Email && d.EmployeeId != model.employee.EmployeeId).ToList().Count();
                if (em > 0)
                {
                    return Json(false);

                }
                else
                {
                    return Json(true);
                }
            }
            var rec = db.Employee.Any(d => d.Email == model.employee.Email);

            if (!rec)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployeeSalary(AllViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = _userManager.GetUserAsync(User).Result.Email;
                var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();
                if (model.salary.SalaryId != 0)
                {
                    var rec = await db.Salary.FindAsync(model.salary.SalaryId);
                    if (rec == null)
                    {
                        return View("Error");
                    }

                    rec.PaidAmount = model.salary.PaidAmount;
                    rec.Date = DateTime.Now.Date;
                    rec.PaidBy = empId;
                    db.Entry(rec).State = EntityState.Modified;
                    db.Entry(rec).Property(d => d.PaidAmount).IsModified = true;
                    db.Entry(rec).Property(d => d.Date).IsModified = true;
                    db.Entry(rec).Property(d => d.PaidBy).IsModified = true;
                    await db.SaveChangesAsync();
                    return Json("Record updated");

                }
                else
                {
                    try
                    {

                        Salary it = new Salary()
                        {
                            PaidAmount = model.salary.PaidAmount,
                            Date = DateTime.Now.Date,
                            EmployeeId = model.salary.EmployeeId,
                            ClassId = model.salary.ClassId,
                            PaidBy = empId
                        };
                        db.Salary.Add(it);
                        await db.SaveChangesAsync();

                        return Json("Salary paid to employee.");
                    }
                    catch (Exception e)
                    {

                        throw new Exception(e.StackTrace);
                    }
                }

            }
            return Json("Error");

        }
        [Authorize(Roles = "Admin")]
        public IActionResult LoadEmployeeAllPayments(AllViewModel model)
        {
            if (model.employee.EmployeeId != 0)
            {
                var comp = (from s in db.Salary
                            where s.EmployeeId == model.employee.EmployeeId
                            select new
                            {
                                paidDate = Convert.ToDateTime(s.Date).Date.ToShortDateString(),
                                paidAmount = s.PaidAmount
                            }).ToList();
                if (comp == null)
                {
                    return View("Error");
                }

                try
                {

                    return Json(comp);
                }
                catch (Exception)
                {

                    return Json("Payment not done!!!");
                }
            }
            return NotFound();

        }
        [Authorize(Roles = "Admin,Teacher,Student")]
        public IActionResult Notes()
        {
            return View();
        }
        public async Task<JsonResult> AddNote(NoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = _userManager.GetUserAsync(User).Result.Email;
            
                if (model.NoteId == 0)
                {
                    Note nt = new Note()
                    {
                        Note1 = model.Note,
                        TargetDate = Convert.ToDateTime(model.TargetDate.ToShortDateString()),
                        RemainDate = Convert.ToDateTime(model.RemindDate.ToShortDateString()),
                        UserEmail = email,
                        NoteStatus = "Active"
                    };
                    db.Note.Add(nt);
                    await db.SaveChangesAsync();
                    return Json("Note added !!!");
                }
                else
                {
                    Note targetedNote = db.Note.Where(x => x.NoteId == model.NoteId).FirstOrDefault();
                    targetedNote.NoteId = model.NoteId;
                    targetedNote.Note1 = model.Note;
                    targetedNote.TargetDate = Convert.ToDateTime(model.TargetDate.ToShortDateString());
                    targetedNote.RemainDate = Convert.ToDateTime(model.RemindDate.ToShortDateString());
                    db.Entry(targetedNote).Property(x => x.NoteStatus).IsModified = false;
                    db.Entry(targetedNote).Property(x => x.UserEmail).IsModified = false;
                    db.Entry(targetedNote).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Json("Note updated!!!");
                }
            }


            return Json("");
        }
        public JsonResult FetchNotes()
        {
            var email = _userManager.GetUserAsync(User).Result.Email;
           
            var AcitveNotes = (from N in db.Note
                               where N.UserEmail == email && N.NoteStatus == "Active"

                               select new
                               {
                                   noteid = N.NoteId,
                                   note = N.Note1,
                                   targetdate = N.TargetDate,
                                   targetdateShow = Convert.ToDateTime(N.TargetDate).ToShortDateString(),
                                   reminddate = N.RemainDate,
                                   reminddateShow = Convert.ToDateTime(N.RemainDate).ToShortDateString(),
                                   notestatus = N.NoteStatus,

                               }).ToList();

            return Json(AcitveNotes);
        }
        public JsonResult DeleteNote(Note note)
        {
            var record = db.Note.Find(note.NoteId);
            db.Note.Remove(record);
            db.SaveChanges();
            return Json("Note deleted !!!");
        }
        public JsonResult countActiveNotes()
        {
            var email = _userManager.GetUserAsync(User).Result.Email;
           
            var activeNotes = db.Note.Where(x => x.RemainDate <= DateTime.Now && x.TargetDate >= DateTime.Now && x.UserEmail == email).ToList();
            return Json(activeNotes.Count());
        }
        public JsonResult LoadActiveNotes()
        {
            var email = _userManager.GetUserAsync(User).Result.Email;
           
            var activeNotes = from note in db.Note.Where(x => x.RemainDate <= DateTime.Now.Date && x.TargetDate >= DateTime.Now.Date && x.UserEmail == email)

                              select new
                              {
                                  note = note.Note1,
                                  targetDate = Convert.ToDateTime(note.TargetDate).ToShortDateString()
                              };
            return Json(activeNotes);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromClass(AllViewModel model)
        {
            if (model.STC.ClassId != 0)
            {
                if (model.STC.Type == 0)
                {
                    var rec = db.StudentClass.Find(model.STC.PersonId);
                    db.StudentClass.Remove(rec);

                    var rec2 = db.Fees.Where(x => x.ClassId == model.STC.ClassId && x.StudentId == model.STC.StudentId).FirstOrDefault();
                    db.Fees.Remove(rec2);

                    await db.SaveChangesAsync();
                    return Json(model.STC.ClassId);
                }
                else
                {
                    var rec = db.ClassTeacher.Find(model.STC.PersonId);
                    db.ClassTeacher.Remove(rec);
                    await db.SaveChangesAsync();
                    return Json(model.STC.ClassId);
                }
            }
            else
            {
                return Json("Class Id is required");
            }

        }
        public JsonResult assignTeacher(AllViewModel model)
        {
            ClassTeacher obj = new ClassTeacher();
            obj.ClassId = model.classTeacher.Class_Id;
            obj.EmployeeId = model.classTeacher.Teacher_Id;
            db.ClassTeacher.Add(obj);
            db.SaveChanges();
            return Json("The Class Assigned to Teacher");
        }
    }
}