using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using JahanInstitute.Models;
using JahanInstitute.Models.AccountViewModels;
using JahanInstitute.Models.ViewModels;
using JahanInstitute.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JahanInstitute.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {

        JahanInstituteContext db = new JahanInstituteContext();

        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        public IConfiguration configuration;


        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = logger;
            this.configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
        }

        [TempData]
        public string ErrorMessage { get; set; }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddClaimToUser(AllViewModel model, [FromForm] string radio)
        {


            var user = await _userManager.FindByEmailAsync(model.claim.Email);


            if (user == null)
            {
                return NotFound();
            }

            var ClaimsOfUser = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, ClaimsOfUser);

            if (!result.Succeeded)
            {
                return View("Error");
            }

            if (radio == "None")
            {
                return Json("This user dont have any role");
            }
            if (radio == "View")
            {
                result = await _userManager.AddClaimAsync(user, new Claim("View Role", "View Role"));

            }
            if (radio == "Insert")
            {
                result = await _userManager.AddClaimsAsync(user, new List<Claim>() {
                    new Claim("View Role", "View Role"),
                    new Claim("Insert Role", "Insert Role")
                });
            }
            if (radio == "Edit")
            {
                result = await _userManager.AddClaimsAsync(user, new List<Claim>() {
                    new Claim("View Role", "View Role"),
                    new Claim("Insert Role", "Insert Role"),
                    new Claim("Edit Role", "Edit Role")
                });
            }
            if (radio == "Delete")
            {
                result = await _userManager.AddClaimsAsync(user, new List<Claim>() {
                    new Claim("View Role", "View Role"),
                    new Claim("Insert Role", "Insert Role"),
                    new Claim("Edit Role", "Edit Role"),
                     new Claim("Delete Role", "Delete Role")
                });
            }
            if (!result.Succeeded)
            {
                return View("Error");
            }

            return Json("The selected authority added to the user.");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult LoadAccounts()
        {

            var rec = _userManager.Users.ToList();
            if (rec == null)
            {
                return NotFound();
            }
            return Json(rec);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeUserRole(AllViewModel model)
        {
            try
            {
                if (model.register.id2 != null)
                {
                    var user = await _userManager.FindByEmailAsync(model.register.email2);
                    if (user == null)
                    {
                        return View("Error");
                    }
                    var role = await _userManager.RemoveFromRoleAsync(user, model.register.role);
                    if (!role.Succeeded)
                    {
                        return View("Error");
                    }
                    var assign = await _userManager.AddToRoleAsync(user, model.register.id2);
                    return Json("User role changed");
                }
                else
                {
                    return Json("Please select a role.");
                }
            }
            catch (Exception)
            {

                return Json("Error");
            }

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LoadUserClaims(AllViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.claim.Email);

            var logs = (from l in db.UserLoginDetail
                        where l.EmployeeEmail == model.claim.Email
                        select new
                        {
                            email = l.EmployeeEmail,
                            loginDate = Convert.ToDateTime(l.LoginDate).ToString("yyyy/MM/dd hh:mm:ss"), /*Convert.ToDateTime(l.LoginDate).Date.ToUniversalTime()*/ /*+"-" + Convert.ToDateTime(l.LoginDate).ToLocalTime().ToString("HH:mm:ss")*/
                            logoutDate = Convert.ToDateTime(l.LogoutDate).ToString("yyyy/MM/dd hh:mm:ss"),
                        }).ToList();
            var role = await _userManager.GetRolesAsync(user);


            if (user == null)
                return NotFound();
            var Claims = await _userManager.GetClaimsAsync(user);
            if (Claims == null)
            {
                return Json("NoRoles");
            }
            var rec = new
            {
                claims = Claims.Count(),
                role = role,
                logs = logs
            };
            return Json(rec);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);


            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    UserLoginDetail log = new UserLoginDetail()
                    {
                        EmployeeEmail = model.Email,
                        LoginDate = DateTime.Now.ToLocalTime(),
                    };
                    db.UserLoginDetail.Add(log);

                    await db.SaveChangesAsync();
                    //if (user != null)
                    //{
                    //    role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                    //}
                    //if (role == "Finance Department")
                    //{
                    //    return RedirectToAction("Deal", "Deal");
                    //}
                    //if (role == "HR Department")
                    //{
                    //    return RedirectToAction("Sale", "Sale");
                    //}

                    return RedirectToLocal(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, " Your account has been blocked for 15 minutes .");
                    return View(model);
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, " Your accout has been blocked by dean of institute.");
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> LockAccount(AllViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.register.email);
            if (model.register.trueOrfalse == "lock")
            {
                user.EmailConfirmed = false;
                await _userManager.UpdateAsync(user);
                return Json("true");
            }
            else
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return Json("true");
            }


        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(AllViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.register.email);
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Json("Account deleted");
            }
            else { return Json("Error"); }
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
                           hireDate = e.HireDate.ToString(),
                           fireDate = e.FireDate.ToString(),
                           photo = e.Photo,
                       }).OrderByDescending(d => d.employeeId);
            return Json(rec);
        }
        public JsonResult LoadStudent()
        {
            var rec = (from e in db.Student

                       select new
                       {
                           studentId = e.StudentId,
                           name = e.Name,
                           fName = e.FatherName,
                           phone = e.Phone,
                           email = e.Email,
                           idCardNo = e.IdCardNo,
                           address = e.Address,
                           gender = e.Gender,
                           regDate = e.RegDate.ToString(),
                           photo = e.Photo,
                       }).OrderByDescending(d => d.studentId);
            return Json(rec);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Register(string returnUrl = null)
        {

            var Employee = (from d in db.Employee
                            select new SelectListItem()
                            {
                                Text = d.Name,
                                Value = d.EmployeeId.ToString()
                            }).ToList();
            ViewBag.Employee = Employee;

            var Roles = (from d in _roleManager.Roles
                         where d.Name != "Student"
                         select new SelectListItem()
                         {
                             Text = d.Name,
                             Value = d.Name
                         }).ToList();
            ViewBag.Roles = Roles;
            ViewBag.Roles2 = Roles;
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        public IActionResult IsEmployeeAccountExist(AllViewModel model)
        {
            //int id = Convert.ToInt32(EmployeeId);
            var rec = db.Employee.Where(d => d.EmployeeId == model.register.EmployeeId).FirstOrDefault();
            var Email = "";
            if (rec != null)
                Email = rec.Email;
            var user = _userManager.FindByEmailAsync(Email);

            if (user.Result == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AllViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {

                if (model.register.studentOrEmployee == "employee")
                {
                    var rec = db.Employee.Where(e => e.EmployeeId == model.register.EmployeeId).FirstOrDefault();
                    var email = rec.Email;
                    var exist = _userManager.FindByEmailAsync(email).Result;
                    if (exist == null)
                    {
                        var user = new ApplicationUser { UserName = rec.Email, Email = rec.Email, EmployeeName = rec.Name, ImagePath = rec.Photo, UserType = "employee" };
                        var result = await _userManager.CreateAsync(user, model.register.Password);
                        if (result.Succeeded)
                        {
                            var userToRole = await _userManager.FindByEmailAsync(rec.Email);
                            await _userManager.AddToRoleAsync(userToRole, model.register.id);
                           
                            return Json(" Account registered for " + rec.Name);
                        }
                        return Json("Password must contain special symbol, digit and capital letter");
                    }
                    else
                    {
                        return Json("This employee has an account you can see in account list");
                    }
                }
                else
                {
                    var rec = db.Student.Where(e => e.StudentId == model.register.EmployeeId).FirstOrDefault();
                    var email = rec.Email;
                    var exist = _userManager.FindByEmailAsync(email).Result;
                    if (exist == null)
                    {
                        var user = new ApplicationUser { UserName = rec.Email, Email = rec.Email, EmployeeName = rec.Name, ImagePath = rec.Photo, UserType = "student" };
                        var result = await _userManager.CreateAsync(user, model.register.Password);
                        if (result.Succeeded)
                        {
                            var userToRole = await _userManager.FindByEmailAsync(rec.Email);
                            await _userManager.AddToRoleAsync(userToRole, "Student");
                        
                                var user2 = _userManager.FindByEmailAsync(rec.Email).Result;
                                result = await _userManager.AddClaimsAsync(user2, new List<Claim>() {
                                  new Claim("View Role", "View Role"),
                                  new Claim("Insert Role", "Insert Role"),
                                  new Claim("Edit Role", "Edit Role"),
                                  new Claim("Delete Role", "Delete Role")
                                });
                            
                            return Json(" Account registered for " + rec.Name);
                        }
                        return Json("Password must contain special symbol, digit and capital letter");
                    }
                    else
                    {
                        return Json("This student has an account you can see in account list");
                    }
                }


            }
            return Json("Error");
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = _userManager.GetUserAsync(User).Result.Email;
                var rec1 = db.UserLoginDetail.Where(d => d.EmployeeEmail == userId).ToList().OrderByDescending(r => r.DetailId);
                UserLoginDetail rec;
                if (rec1 != null)
                {
                    rec = rec1.First();
                    if (rec != null)
                    {
                        rec.LogoutDate = DateTime.Now.ToLocalTime();

                        db.Entry(rec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.Entry(rec).Property(r => r.LogoutDate).IsModified = true;
                        await db.SaveChangesAsync();
                    }
                    await _signInManager.SignOutAsync();
                }
                await _signInManager.SignOutAsync();

                return RedirectToAction("Login", "Account");

            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(AllViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(model.forgot.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return Json(1);

                }
                if (!(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return Json(2);

                }

                var code = SendCodeToUser(user.EmployeeName, user.Email);
                if (code == 500)
                {
                    return Json(3);
                }
                else
                {

                    return Json(code);
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                //   $"مهربانی له مخې په دې بټنې کلیک وکړی تر څو مو پټ نوم تغیر کړی.: <a href='{callbackUrl}'>link</a>");

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddNewPassword(AllViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.reset.userEmail);
                await _userManager.RemovePasswordAsync(user);
                var pass = await _userManager.AddPasswordAsync(user, model.reset.NewPassword);
                if (!pass.Succeeded)
                {
                    return Json("Password must contain special seymbol, digit and capital letter.");
                }
                await _signInManager.SignInAsync(user, isPersistent: false);

                UserLoginDetail log = new UserLoginDetail()
                {
                    EmployeeEmail = model.reset.userEmail,
                    LoginDate = DateTime.Now.ToLocalTime(),
                };
                db.UserLoginDetail.Add(log);
                await db.SaveChangesAsync();

                return Json("done");


            }

            return RedirectToAction(nameof(ForgotPassword));
        }
        public int SendCodeToUser(string fName, string userEmailOrPhone)
        {
            var systemEmail = configuration["SystemEmail"];
            var key = configuration["key"];
            Random ran = new Random();
            var code = ran.Next(1, 999999);
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(userEmailOrPhone);
                mail.From = new MailAddress("smarthealthconsultation@gmail.com");
                mail.Subject = "Password Reset";
                mail.Body = string.Format("Dear " + fName + "  <br/>  <br/>" + code + "<br/> is your password reset code");
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(systemEmail, key);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return code;
            }
            catch (Exception)
            {

                return 500;
            }

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return View("Error");
                //throw new ApplicationException("کوډ نه بغیر پاسورډ نه شی بدلولی.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult SaveDocuments()
        {
            if (TempData["updated"] != null)
            {
                ViewBag.Alert = TempData["updated"];
            }
            else
            {
                ViewBag.Alert = "empty";
            }

            if (TempData["delete"] != null)
            {
                ViewBag.delete = TempData["delete"];
            }
            else
            {
                ViewBag.delete = "empty";
            }
            //ViewBag.Documents = db.Document.ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveDocumentss(DocumentViewModel model, [FromForm] IFormFile img)
        {
            var upload = "";
            var uploadPath = "";

            if (img != null)
            {
                var fileName = Guid.NewGuid().ToString().Replace("_", "") + Path.GetExtension(img.FileName);
                upload = Path.Combine("Images/DocumentImage/", fileName);
                uploadPath = Path.Combine(hostingEnvironment.WebRootPath, upload);
                img.CopyTo(new FileStream(uploadPath, FileMode.Create));
                upload = "/" + upload;
                //Document doc = new Document()
                //{
                //    DocumentDetails = model.DocumentDetails,
                //    DocumentPicture = upload,
                //};
                //db.Document.Add(doc);
                await db.SaveChangesAsync();
                TempData["updated"] = "اسناد ذخیره شو";
                return RedirectToAction("SaveDocuments", "Account");
            }
            else { return View("Error"); }
        }
        public async Task<IActionResult> DeleteDocument(int id)
        {
            //var rec = db.Document.Find(id);
            //if (rec == null)
            //{
            //    return View("Error");
            //}
            //db.Document.Remove(rec);
            await db.SaveChangesAsync();
            TempData["delete"] = "Record deleted";
            return RedirectToAction("SaveDocuments", "Account");
        }
        [AllowAnonymous]
        public async Task<IActionResult> UpdateDatabase()
        {
            IdentityRole Student = new IdentityRole()
            {
                Name = "Student"
            };
            IdentityRole Admin = new IdentityRole()
            {
                Name = "Admin"
            };
            IdentityRole Teacher = new IdentityRole()
            {
                Name = "Teacher"
            };
            await _roleManager.CreateAsync(Student);
            await _roleManager.CreateAsync(Admin);
            await _roleManager.CreateAsync(Teacher);

            var cur1 = db.Currency.Where(c => c.CurrencyId == 1).FirstOrDefault();
            var cur2 = db.Currency.Where(c => c.CurrencyId == 2).FirstOrDefault();
            var cur3 = db.Currency.Where(c => c.CurrencyId == 3).FirstOrDefault();
            if (cur1 == null)
            {
                Currency c1 = new Currency()
                {
                    Currency1 = "Af"
                };
                db.Currency.Add(c1);
                await db.SaveChangesAsync();
            }
            if (cur2 == null)
            {
                Currency c1 = new Currency()
                {
                    Currency1 = "Pk"
                };
                db.Currency.Add(c1);
                await db.SaveChangesAsync();
            }
            if (cur3 == null)
            {
                Currency c1 = new Currency()
                {
                    Currency1 = "Us"
                };
                db.Currency.Add(c1);
                await db.SaveChangesAsync();
            }



            var employee = db.Employee.Where(d => d.Email == "Admin@gmail.com").FirstOrDefault();
            if (employee == null)
            {
                Employee rec = new Employee()
                {
                    Email = "Admin@gmail.com",
                    Photo = "/images/StaticImages/Admin.png",
                    Name = "AdminAccount",
                    EmployeeTypeId = 1,
                    SalaryType = "fixed",
                };
                db.Employee.Add(rec);
                await db.SaveChangesAsync();
            }

            var users = _userManager.Users.ToList();
            if (users.Count == 0)
            {
                var user = new ApplicationUser { UserName = "Admin@gmail.com", Email = "Admin@gmail.com", ImagePath = "/images/StaticImages/Admin.png", EmployeeName = "admin" };
                var result = await _userManager.CreateAsync(user, "Admin1@");
                if (!result.Succeeded)
                {
                    return View("Error");
                }


                var us = await _userManager.FindByEmailAsync("Admin@gmail.com");


                us.EmailConfirmed = true;
                await _userManager.UpdateAsync(us);

                var res = await _userManager.AddToRoleAsync(us, "Admin");
                if (!res.Succeeded)
                {
                    return View("Error");

                }
            }


            return View("Login");
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
