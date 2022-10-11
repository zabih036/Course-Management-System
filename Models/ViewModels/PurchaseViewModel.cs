using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.ViewModels
{
    public class PurchaseViewModel
    {
        public int PurchaseId { get; set; }
        //////////////// <Bill No>
        [Required(ErrorMessage = "بېل نمبر داخل کړئ")]
        [Display(Name = "بیل نمبر:")]
        public string Bill_No { get; set; }
        //////////////// <\Bill No>

        //////////////// <Item>
        [Required(ErrorMessage = "جنس انتخاب کړئ!!")]
        [Display(Name = "جنس:")]
        [ForeignKey("ItemId")]
        public int ItemID { get; set; }
        //////////////// <\Item>

        //////////////// <Dealer>
        [Required(ErrorMessage = "د شرکت نوم انتخاب کړئ!!")]
        [Display(Name = "شرکت:")]
        [ForeignKey("DealerID")]
        public int DealerId { get; set; }
        ////////////////// </dealer>

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

        ///////////////// <quantity>
        [Required(ErrorMessage = "د جنس تعداد ورسوئ!!")]
        [Display(Name = "تعداد:")]
        public int Quantity { get; set; }
        /////////////// </quantity>

        ///////////////// <quantity>
        [Required(ErrorMessage = "په کارتن کې د اجناسو تعداد مشخص کړئ!!")]
        [Display(Name = "په کارتن کې تعداد:")]
        public int AmountInPackage { get; set; }
        /////////////// </quantity>

        ///////////////// <PurchasePrice>
        [Required(ErrorMessage = "د پیرلو بیه داخل کړئ!!")]
        [Display(Name = "د پیرلو بیه:")]
        public int PurchasePrice { get; set; }
        /////////////// </PurchasePrice>

        ///////////////// <Sale Price>
        [Required(ErrorMessage = "د خرڅلاو بیه داخل کړئ!!")]
        [Display(Name = "د خرڅلاو بیه:")]
        public int SalePrice { get; set; }
        /////////////// </Sale Price>

        ///////////////// <currency>
        [Required(ErrorMessage = "د پیسو ډول انتخاب کړئ!!")]
        [Display(Name = "د پیسو ډول:")]
        [ForeignKey("CurrencyId")]
        public int CurrencyID { get; set; }
        /////////////// </Country>

        ///////////////// <Manufictur Date>
        [Required(ErrorMessage = "د جوړېدو نیټه انتخاب کړئ!!")]
        [Display(Name = "د جوړېدو نیټه:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime ManuficturDate { get; set; }
        /////////////// </Manufictur Date>

        ///////////////// <Expire Date>
        [Required(ErrorMessage = "د ختمېدو نیټه انتخاب کړئ!!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "د ختم نیټه:")]
        [DataType(DataType.Date)]
        public DateTime ExpireDate { get; set; }
        /////////////// </ManuficturDate>

        ///////////////// <stock>
        [Required(ErrorMessage = "ګودام انتخاب کړئ!!")]
        [Display(Name = "ګودام:")]
        [ForeignKey("StockId")]
        public int StockID { get; set; }
        /////////////// </stock>
        ///////////////// <stock>
        [Required(ErrorMessage = "د جنس مقدار انتخاب کړئ!!")]
        [Display(Name = "مقدار:")]
        [ForeignKey("UnitID")]
        public int UnitID { get; set; }
        /////////////// </stock>
        ///////////////// <stock>
        [Required(ErrorMessage = "د ختمېدو نیټې لپاره د ورځو تعداد ورسوئ!!")]
        [Display(Name = "د پای ورځې:")]
        [ForeignKey("UnitID")]
        public int DateToExpiry { get; set; }
        /////////////// </stock>
        ///////////////// <stock>
        [Required(ErrorMessage = "د جنس د کمبودی تعداد ورسوئ!!")]
        [Display(Name = "د کمبودی تعداد:")]
        [ForeignKey("UnitID")]
        public int shortageQuantity { get; set; }
        /////////////// </stock>
    }
}
