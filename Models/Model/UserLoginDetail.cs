using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class UserLoginDetail
    {
        public int DetailId { get; set; }
        public DateTime? LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }
        public string EmployeeEmail { get; set; }
    }
}
