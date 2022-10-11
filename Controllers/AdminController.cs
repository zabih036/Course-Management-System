using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JahanInstitute.Models;
using JahanInstitute.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace JahanInstitute.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        JahanInstituteContext db = new JahanInstituteContext();
       public IActionResult studentAttendance()
        {
            var classes = (from cl in db.Class
                           join cn in db.ClassName on cl.ClassNameId equals cn.ClassNameId
                          
                           select new SelectListItem()
                           {
                               Text = db.ClassName.Where(r => r.ClassNameId == cn.ClassNameId).Select(d => d.ClassName1).FirstOrDefault() + " , Time : " + cl.StartHour + " - " + cl.EndHour + " , Shift : " + cl.Shift,
                               Value = cl.ClassId.ToString()
                           }).ToList().GroupBy(x => x.Value).Select(r => r.FirstOrDefault());
            ViewBag.Classes = classes;
            return View();
        }
    }
}
