using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.ViewModels
{
    public class SaleViewModel
    {
        public int SaleId { get; set; }
        //////////////// <Bill No>
        [Required(ErrorMessage = "بېل نمبر داخل کړئ")]
        [Display(Name = "بیل نمبر:")]
        public string Bill_No { get; set; }
        //////////////// <\Bill No>

        //////////////// <Customer>
        [Display(Name = "مشتري:")]
        [ForeignKey("DealerId")]
        public int DealerID { get; set; }
        //////////////// <\Customer>

        //////////////// <Dealer>
        [Display(Name = "موقت مشتري:")]
        [ForeignKey("DealerID")]
        public string tempCustomerName { get; set; }

        //////////////// <Item>
        [Required(ErrorMessage = "جنس انتخاب کړئ!!")]
        [Display(Name = "جنس:")]
        [ForeignKey("ItemId")]
        public int ItemID { get; set; }
        //////////////// <\Item>

        ///////////////// <Company>
        [Required(ErrorMessage = "د کمپنی نوم انتخاب کړئ!!")]
        [Display(Name = "کمپني:")]
        [ForeignKey("CompanyId")]
        public int CompanyID { get; set; }
        /////////////// </company>

        ///////////////// <Country>
        [Required(ErrorMessage = "د هېواد نوم انتخاب کړئ!!")]
        [Display(Name = "هېواد:")]
        [ForeignKey("CountryId")]
        public int CountryID { get; set; }
        /////////////// </Country>

        ///////////////// <currency>
        [Required(ErrorMessage = "د پیسو ډول انتخاب کړئ!!")]
        [Display(Name = "د پیسو ډول:")]
        [ForeignKey("CurrencyId")]
        public int CurrencyID { get; set; }
        /////////////// </Country>

        ///////////////// <sale Price>
        [Required(ErrorMessage = "د پېرلو قیمت انتخاب کړئ!!")]
        [Display(Name = "پېرلو بیه:")]
        public float SalePrice { get; set; }
        /////////////// </Sale Price>

        ///////////////// <quantity>
        [Required(ErrorMessage = "د جنس مقدار انتخاب کړئ!!")]
        [Display(Name = "تعداد:")]
        public int Quantity { get; set; }
        /////////////// </quantity>
        ///////////////// <quantity in package>
        [Required(ErrorMessage = "د کارتن ظرفیت ورسوئ!!")]
        [Display(Name = "د کارتن ظرفیت:")]
        public int AmountInPackage { get; set; }
        /////////////// </quantity>

        /////////////// </quantity>
        //////////// <Discount>////////
        [Display(Name = "تخفیف:")]
        public float Discount { get; set; }
        /////////// </Discount>////////
        public float purchaseprice { get; set; }
        //////////// <Discount>////////
        [Display(Name = "نوټ:")]
        public string Note { get; set; }
        /////////// </Discount>////////
        public int parchonsale { get; set; }
    }
}
