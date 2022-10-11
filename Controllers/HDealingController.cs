using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JahanInstitute.Models;
using JahanInstitute.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JahanInstitute.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HDealingController : Controller
    {
        JahanInstituteContext db = new JahanInstituteContext();
        private readonly UserManager<ApplicationUser> _userManager;
        public HDealingController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        public IActionResult Dealing()
        {

            return View();
        }
        public IActionResult NewHdealer(AllViewModel allv)
        {
            if (allv.hdealer.HDealerID == 0)
            {
                Dealer hd = new Dealer();
                hd.Dealer1 = allv.hdealer.HDealer;
                hd.Mobile = allv.hdealer.Mobile;

                db.Dealer.Add(hd);
                db.SaveChanges();
                return Json("New dealing account created for " + allv.hdealer.HDealer );

            }
            else
            {
                var recored = db.Dealer.Where(x => x.DealerId == allv.hdealer.HDealerID).FirstOrDefault();
                recored.Dealer1 = allv.hdealer.HDealer;
                recored.Mobile = allv.hdealer.Mobile;
                db.Entry(recored).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                return Json("Record updated");
            }

        }
        public JsonResult FetchHDealers()
        {
            var allHDealer = db.Dealer;

            return Json(allHDealer);
        }
        public JsonResult GetAllDeals(int DealerID)
        {
            var DealV = (from d in db.Dealing
                         join dealer in db.Dealer on d.DealerId equals dealer.DealerId
                         join cu in db.Currency on d.CurrencyId equals cu.CurrencyId
                         join e in db.Employee on d.EmployeeId equals e.EmployeeId
                         select new
                         {
                             dealid = d.DealingId,
                             debit = d.Debit,
                             credit = d.Credit,
                             currencyid = d.CurrencyId,
                             currency = cu.Currency1,

                             dealerid = d.DealerId,
                             phone = dealer.Mobile,

                             dealer = dealer.Dealer1,
                             employeeid = d.EmployeeId,
                             employee = e.Name,
                             date = Convert.ToDateTime(d.Date).ToShortDateString(),
                             detail = d.Detail
                         }).Where(x => x.dealerid == DealerID).OrderByDescending(r => r.dealid);

            return Json(DealV);
        }
        public async Task<IActionResult> NewDeal(DealViewModel deal)
        {
            var email = _userManager.GetUserAsync(User).Result.Email;
            var empid = db.Employee.Where(d => d.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

            Dealing de = new Dealing();
            de.Credit = Convert.ToDecimal(deal.Credit);
            de.Debit = Convert.ToDecimal(deal.Debit);
            de.DealerId = deal.DealerID;
            de.CurrencyId = deal.CurrencyID;
            de.EmployeeId = empid;
            de.Date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            de.Detail = deal.Detail;
            db.Dealing.Add(de);
            await db.SaveChangesAsync();

            var dealAmount = "";
            if (deal.Credit > 0)
            {
                dealAmount = deal.Credit + " Loan paid by ";
            }
            else
            {
                dealAmount =  deal.Debit + " Loan given to ";
            }
            return Json(dealAmount);
        }
        public JsonResult HasanaLoanReport()
        {

            var allLoan = db.Dealing;
            var afghaniDebit = Convert.ToDecimal(0.00);
            var afghaniCredit = Convert.ToDecimal(0.00);
            var rupeDebit = Convert.ToDecimal(0.00);
            var rupeCredit = Convert.ToDecimal(0.00);
            var dollerDebit = Convert.ToDecimal(0.00);
            var dollerCredit = Convert.ToDecimal(0.00);
            foreach (var item in allLoan)
            {
                switch (item.CurrencyId)
                {
                    case 1:
                        afghaniDebit += Convert.ToDecimal(item.Debit);
                        afghaniCredit += Convert.ToDecimal(item.Credit);
                        break;
                    case 2:
                        rupeDebit += Convert.ToDecimal(item.Debit);
                        rupeCredit += Convert.ToDecimal(item.Credit);
                        break;
                    case 3:
                        dollerDebit += Convert.ToDecimal(item.Debit);
                        dollerCredit += Convert.ToDecimal(item.Credit);
                        break;
                }
            }
            var result = new
            {
                afghaniLoan = afghaniDebit - afghaniCredit,
                rupeLoan = rupeDebit - rupeCredit,
                dollerLoan = dollerDebit - dollerCredit
            };
            return Json(result);
        }
    }
}
