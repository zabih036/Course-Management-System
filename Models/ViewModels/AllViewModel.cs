
using JahanInstitute.Models.AccountViewModels;

namespace JahanInstitute.Models.ViewModels
{
    public class AllViewModel
    {
        public CompanyViewModel company { get; set; }
        public CorporationViewModel corporation { get; set; }
        public CurrencyViewModel currency { get; set; }
        public ExamViewModel stock { get; set; }
        public ExpenseViewModel expense { get; set; }
        public ExpenseTypeViewModel expenseType { get; set; }
        public ItemViewModel item { get; set; }
        public ItemCategoryViewModel itemCategory { get; set; }
        public EmployeeRegistratoinViewModel employee { get; set; }
        public SalaryViewModel salary { get; set; }
        public RegisterViewModel register { get; set; }
        public ClaimViewModel claim { get; set; }
        public DealViewModel deal { get; set; }
        public PurchaseViewModel purchase { get; set; }
        public CreditDebitWithoutRecordViewModel dealingform { get; set; }
        public SaleViewModel sale { get; set; }
        public CancellingCurrentSaleViewModel cancellingSinlgeSale { get; set; }
        public UnitViewModel unit { get; set; }
        public ForgotPasswordViewModel forgot { get; set; }
        public ForgotPasswordCodeViewModel code { get; set; }
        public ResetPasswordForgotViewModel reset { get; set; }
        public ExchangeBillViewModel changeBill { get; set; }
        public ManulReprotViewModel manulReport { get; set; }
        public HDealerViewModel hdealer { get; set; }
        public StudentViewModel student { get; set; }
        public StudentClassViewModel stdClass { get; set; }
        public ClassViewModel clas { get; set; }
        public StdTeachClassViewModel STC { get; set; }
        public ExamViewModel exam { get; set; }
        public ExamTypeViewModel examType { get; set; }
        public Class_StudentViewModel classTeacher { get; set; }
        public FeesViewModel fees { get; set; }

    }
}
